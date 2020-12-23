using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Common
{
    public class AppSettings : IAppSettings
    {
        public AppSettings()
        {
            Jwt = new Jwt();
        }

        public IJwt Jwt { get; set; }

        public string AllowedHosts { get; set; }

        public IReadOnlyList<string> AllowedHostList => AllowedHosts.Split(",").ToList();

        public string DefaultConnectionString { get; set; }
    }
    public class Jwt : IJwt
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }
    }
}
