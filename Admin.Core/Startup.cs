﻿using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
//using FluentValidation;
//using FluentValidation.AspNetCore;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Auth;
using Admin.Core.Auth;
using Admin.Core.Enums;
using Admin.Core.Filters;
using Admin.Core.Db;
using Admin.Core.Common.Cache;
using Admin.Core.Aop;
using Admin.Core.Logs;
using PermissionHandler = Admin.Core.Auth.PermissionHandler;
using Admin.Core.Extensions;
using Admin.Core.Common.Attributes;
using Admin.Core.Hubs;
using Quartz;

namespace Admin.Core
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        private static string basePath => AppContext.BaseDirectory;
        private readonly AppConfig _appConfig;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            _appConfig = new ConfigHelper().Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //数据库
            services.AddDb(_env, _appConfig);

            //应用配置
            services.AddSingleton(_appConfig);

            //上传配置
            var uploadConfig = new ConfigHelper().Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);

            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("Admin.Core.Service");
            services.AddAutoMapper(serviceAssembly);
            #endregion

            #region Cors 跨域
            services.AddCors(c =>
            {
                c.AddPolicy("Limit", policy =>
                {
                    policy
                    //.WithOrigins(_appConfig.Urls)
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials()
                    //.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion

            //添加SignalR
            #region
            services.AddSignalR();
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                services.AddSwaggerGen(c =>
                {
                    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    {
                        c.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = "Admin.Core"
                        });
                        //c.OrderActionsBy(o => o.RelativePath);
                    });

                    var xmlPath = Path.Combine(basePath, "Admin.Core.xml");
                    c.IncludeXmlComments(xmlPath, true);

                    var xmlCommonPath = Path.Combine(basePath, "Admin.Core.Common.xml");
                    c.IncludeXmlComments(xmlCommonPath, true);

                    var xmlModelPath = Path.Combine(basePath, "Admin.Core.Model.xml");
                    c.IncludeXmlComments(xmlModelPath);

                    var xmlServicesPath = Path.Combine(basePath, "Admin.Core.Service.xml");
                    c.IncludeXmlComments(xmlServicesPath);

                    //添加设置Token的按钮
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    //添加Jwt验证设置
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                });
            }
            #endregion

            #region Jwt身份认证
            var jwtConfig = new ConfigHelper().Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();
            services.TryAddSingleton<IUserToken, UserToken>();
            services.AddScoped<IPermissionHandler, PermissionHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { }); ;
            #endregion

            #region 控制器
            if (_appConfig.Log.Operation)
            {
                services.AddSingleton<ILogHandler, LogHandler>();
            }
            services.AddControllers(options =>
            {
                options.Filters.Add<AdminExceptionFilter>();
                if (_appConfig.Log.Operation)
                {
                    options.Filters.Add<LogActionFilter>();
                }
            })
            //.AddFluentValidation(config =>
            //{
            //    var assembly = Assembly.LoadFrom(Path.Combine(basePath, "Admin.Core.dll"));   
            //    config.RegisterValidatorsFromAssembly(assembly);
            //})
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //使用驼峰 首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region 缓存
            var cacheConfig = new ConfigHelper().Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if (cacheConfig.Type == CacheType.Redis)
            {
                var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<RedisDataHelper>();
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            #endregion

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            //services.AddQuartz(q =>
            //{
            //    // handy when part of cluster or you want to otherwise identify multiple schedulers
            //    q.SchedulerId = "Scheduler-Core";

            //    // we take this from appsettings.json, just show it's possible
            //    // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

            //    // we could leave DI configuration intact and then jobs need
            //    // to have public no-arg constructor
            //    // the MS DI is expected to produce transient job instances
            //    // this WONT'T work with scoped services like EF Core's DbContext
            //    q.UseMicrosoftDependencyInjectionJobFactory(options =>
            //    {
            //        // if we don't have the job in DI, allow fallback 
            //        // to configure via default constructor
            //        options.AllowDefaultConstructor = true;
            //    });

            //    // or for scoped service support like EF Core DbContext
            //    // q.UseMicrosoftDependencyInjectionScopedJobFactory();

            //    // these are the defaults
            //    q.UseSimpleTypeLoader();
            //    q.UseInMemoryStore();
            //    q.UseDefaultThreadPool(tp =>
            //    {
            //        tp.MaxConcurrency = 10;
            //    });

            //    // quickest way to create a job with single trigger is to use ScheduleJob
            //    // (requires version 3.2)
            //    q.ScheduleJob<TestJob>(trigger => trigger
            //        .WithIdentity("Combined Configuration Trigger")
            //        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
            //        .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
            //        .WithDescription("my awesome trigger configured for a job with single call")
            //    );

            //    // convert time zones using converter that can handle Windows/Linux differences
            //    q.UseTimeZoneConverter();
            //});

            //services.AddQuartzServer(options =>
            //{
            //    // when shutting down we want jobs to complete gracefully
            //    options.WaitForJobsToComplete = true;
            //});
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器
            try
            {
                #region Aop
                var interceptorServiceTypes = new List<Type>();
                if (_appConfig.Aop.Transaction)
                {
                    builder.RegisterType<TransactionInterceptor>();
                    interceptorServiceTypes.Add(typeof(TransactionInterceptor));
                }
                #endregion

                #region Service
                var assemblyServices = Assembly.Load("Admin.Core.Service");
                builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorServiceTypes.ToArray());
                #endregion

                #region Repository
                var assemblyRepository = Assembly.Load("Admin.Core.Repository");
                builder.RegisterAssemblyTypes(assemblyRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency();
                #endregion

                #region SingleInstance
                var assemblyCore = Assembly.Load("Admin.Core");
                var assemblyCommon = Assembly.Load("Admin.Core.Common");
                builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
                .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
                .SingleInstance(); 
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }

        public void Configure(IApplicationBuilder app)
        {
            //启动事件 
            //, IHostApplicationLifetime lifetime
            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    Console.WriteLine($"{_appConfig.Urls}\r\n");
            //});

            #region app配置
            //异常
            app.UseExceptionHandler("/Error");

            app.UseDefaultFiles();

            //静态文件
            app.UseUploadConfig();

            //路由
            app.UseRouting();

            //跨域
            app.UseCors("Limit");

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            //配置端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hub");
            });
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Admin.Core {version}");
                    });
                    c.RoutePrefix = "";//直接根目录访问
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                    //c.DefaultModelsExpandDepth(-1);//不显示Models
                });
            }
            #endregion
        }
    }
}