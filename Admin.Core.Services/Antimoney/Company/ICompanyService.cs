using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Company.Input;

namespace Admin.Core.Service.Antimoney.Company
{
    public partial interface ICompanyService
    {
        /// <summary>
        /// 返回公司分页数据
        /// </summary>
        /// <param name="input">分页条件</param>
        /// <returns></returns>
        Task<IResponseOutput> CompanyPageAsync(PageInput<CompanyEntity> input);

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="input">添加公司基本信息</param>
        /// <returns></returns>
        Task<IResponseOutput> AddCompanyAsync(CompanyAddInput input);

        /// <summary>
        /// 编辑公司
        /// </summary>
        /// <param name="input">编辑公司基本信息</param>
        /// <returns></returns>
        Task<IResponseOutput> EditCompanyAsync(CompanyEditInput input);

        /// <summary>
        /// 根据公司主键获取公司信息
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetCompanyAsync(long companyId);

        /// <summary>
        /// 根据公司主键删除公司
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteCompanyAsync(long companyId);
    }
}
