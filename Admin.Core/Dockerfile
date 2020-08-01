#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
WORKDIR /app
EXPOSE 8888

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Admin.Core/Admin.Core.csproj Admin.Core/
COPY Admin.Core.Services/Admin.Core.Service.csproj Admin.Core.Services/
COPY Admin.Core.Model/Admin.Core.Model.csproj Admin.Core.Model/
COPY Admin.Core.Common/Admin.Core.Common.csproj Admin.Core.Common/
COPY Admin.Core.Repository/Admin.Core.Repository.csproj Admin.Core.Repository/
RUN dotnet restore "Admin.Core/Admin.Core.csproj"
COPY . .
WORKDIR "/src/Admin.Core"
RUN dotnet build "Admin.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.Core.dll", "--server.urls", "http://*:8888"]
