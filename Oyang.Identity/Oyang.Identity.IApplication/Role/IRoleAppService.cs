using Oyang.Identity.Domain;
using Oyang.Identity.IApplication.Role.Dtos;
using Oyang.Identity.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.Role
{
    public interface IRoleAppService : IApplicationService
    {
        Pagination<RoleDto> GetList(GetListInputDto input);
        RoleDto Get(Guid id);
        void Add(AddInputDto input);
        void Update(UpdateInputDto input);
        void Remove(Guid id);
        void SetUser(SetUserInputDto input);
        void SetPermission(SetPermissionInputDto input);
    } 
}
