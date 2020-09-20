using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;

namespace Admin.Core.Service.Questionnaire.SectionCode
{
    public interface ISectionCodeService
    {
        /// <summary>
        /// 获取行政村代码列表
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetListAsync();
    }
}
