using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.Role.Dtos
{
    public class AddInputDto : IInputDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
