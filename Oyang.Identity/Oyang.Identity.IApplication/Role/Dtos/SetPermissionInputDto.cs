using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.Role.Dtos
{
    public class SetPermissionInputDto : IInputDto
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } 
    }
}
