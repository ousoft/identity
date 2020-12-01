using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.Role.Dtos
{
    public class RoleDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
