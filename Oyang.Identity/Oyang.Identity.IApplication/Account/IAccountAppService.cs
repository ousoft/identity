using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oyang.Identity.Infrastructure;
using Oyang.Identity.IApplication.Account.Dtos;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.IApplication.Account
{
    public interface IAccountAppService :IApplicationService
    {
        CurrentUser Validate(LoginInputDto input);
        CurrentUser Get(string loginName);
    }
}
