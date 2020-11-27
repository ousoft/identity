using Oyang.Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Utility
{
    public class ValidationObject
    {
        private static readonly ValidationObject _instance = new ValidationObject();

        public static ValidationObject Validate(bool condition, string errorMessage)
        {
            if (condition)
            {
                throw new DomainException(errorMessage);
            }
            return _instance;
        }
        public static ValidationObject Validate(bool condition, string errorMessage, string member)
        {
            if (condition)
            {
                throw new DomainException(errorMessage, member);
            }
            return _instance;
        }

        public ValidationObject ThenValidate(bool condition, string errorMessage)
        {
            if (condition)
            {
                throw new DomainException(errorMessage);
            }
            return _instance;
        }
        public ValidationObject ThenValidate(bool condition, string errorMessage, string member)
        {
            if (condition)
            {
                throw new DomainException(errorMessage, member);
            }
            return _instance;
        }
    }
}
