using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Domain
{
    public class DomainException : Exception
    {
        public DomainException()
        {

        }

        public DomainException(string message) : base(message)
        {

        }
        public DomainException(string message, string member) : base(message)
        {
            Member = member;
        }

        public string Member { get; }
    }
}
