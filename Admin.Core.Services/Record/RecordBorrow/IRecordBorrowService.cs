using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordBorrow.Input;

namespace Admin.Core.Service.Record.RecordBorrow
{
    public partial interface IRecordBorrowService
    {
        Task<IResponseOutput> BorrowOrReadAsync(RecordBorrowAddInput input);

        Task<IResponseOutput> CancelBorrowOrReadAsync(long id);

        Task<IResponseOutput> GetBorrowListAsync();

        Task<IResponseOutput> GetVerifyListAsync(PageInput<RecordBorrowEntity> input);

        Task<IResponseOutput> GetBorrowDetailAsync(long id);

        Task<IResponseOutput> VerifyBorrowAsync(RecordBorrowVerifyInput input);

        Task<IResponseOutput> GetReturnPageAsync(PageInput<RecordBorrowEntity> input);

        Task<IResponseOutput> ReturnRecordAsync(long id);

        Task<UserEntity> GetUserByBorrowId(long id);
    }
}
