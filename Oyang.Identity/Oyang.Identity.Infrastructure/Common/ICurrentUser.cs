using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oyang.Identity.Infrastructure.Common
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string LoginName { get; }

        bool IsAuthenticated { get; }
    }
}
