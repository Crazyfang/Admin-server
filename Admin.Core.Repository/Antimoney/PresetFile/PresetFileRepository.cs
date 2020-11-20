using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.PresetFile
{
    public class PresetFileRepository:RepositoryBase<PresetFileEntity>, IPresetFileRepository
    {
        public PresetFileRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
