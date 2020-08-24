using System;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Record.RecordHistory.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordHistory
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordHistoryEntity, RecordHistoryGetOutput>().ForMember(
                i => i.CreatedUserName,
                m => m.MapFrom(n => n.CreatedUser.NickName)
            ).ForMember(
                i => i.CreatedUserId,
                m => m.MapFrom(n => n.CreatedUser.UserName)
            );
        }
    }
}
