using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.User.Dtos
{
    public class UserDto : IDto
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
    }
}
