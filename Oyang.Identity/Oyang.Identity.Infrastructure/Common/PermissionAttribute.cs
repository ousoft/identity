using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Common
{
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string code)
        {
            this.Code = code;
        }
        public string Code { get; }
    }
}
