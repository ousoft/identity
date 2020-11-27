using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.IApplication.Account.Dtos
{
    public class LoginInputDto : IInputDto
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
}
