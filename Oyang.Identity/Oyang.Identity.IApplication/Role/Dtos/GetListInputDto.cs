using Oyang.Identity.Domain;
using Oyang.Identity.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.Role.Dtos
{
    public class GetListInputDto : Pagination, IInputDto
    {
        public string Name { get; set; }
    }
}
