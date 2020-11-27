using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oyang.Identity.Domain;
using Oyang.Identity.IApplication.User.Dtos;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.IApplication.User
{
    public interface IUserAppService : IApplicationService
    {
        Pagination<UserDto> GetList(GetListInputDto input);
        void Add(AddInputDto input);
        void Update(UpdateInputDto input);
        void Remove(Guid id);
        void ResetPassword(ResetPasswordInputDto input);
        void SetRole(SetRoleInputDto input);
    }
}
