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
        string GenerateToken(LoginInputDto input);
        string RefreshToken(string token);
    }
}
