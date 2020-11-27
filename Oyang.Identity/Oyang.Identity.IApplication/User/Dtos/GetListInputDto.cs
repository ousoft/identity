using Oyang.Identity.Domain;
using Oyang.Identity.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyang.Identity.IApplication.User.Dtos
{
    public class GetListInputDto : Pagination, IInputDto
    {
        public string LoginName { get; set; }
    }
}
