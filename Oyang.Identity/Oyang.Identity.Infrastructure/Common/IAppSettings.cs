using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Common
{
    public interface IAppSettings
    {
        string DefaultConnectionString { get; }
        IJwt Jwt { get; }
        IReadOnlyList<string> AllowedHostList { get; }
    }
    public interface IJwt
    {
        string Issuer { get; }
        string Audience { get; }
        string SecurityKey { get; }
    }
}
