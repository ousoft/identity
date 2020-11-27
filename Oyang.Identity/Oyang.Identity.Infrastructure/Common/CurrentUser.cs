using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Common
{
    public class CurrentUser : ICurrentUser
    {
        public CurrentUser(
            Guid id,
            string loginName
            )
        {
            Id = id;
            LoginName = loginName;
        }
        public Guid Id { get;  }
        public string LoginName { get;  }
        public bool IsAuthenticated => Id != Guid.Empty;
        public IReadOnlyList<string> Roles { get; set; }
        public IReadOnlyList<string> Permissions { get; set; }

        public bool HasRole(string name)
        {
            return Roles.Any(t => t == name);
        }
        public bool HasPermission(string code)
        {
            return Permissions.Any(t => t == code);
        }
    }
}
