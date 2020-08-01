using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordType.Input;

namespace Admin.Core.Service.Record.RecordType
{
    public partial interface IRecordTypeService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetAllRecordTypeAsync();

        Task<IResponseOutput> AddRecordTypeAsync(RecordTypeAddInput entity);

        Task<IResponseOutput> UpdateRecordTypeAsync(RecordTypeUpdateInput entity);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);
    }
}
