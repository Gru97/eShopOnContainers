using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Exceptions
{
    public class OrderingDomainException:Exception
    {
        //We define custom exceptions so that later we would be able to catch them explicitly and do something related to this specific excetion
        public OrderingDomainException() { }
        public OrderingDomainException(string message) : base(message) { }
        public OrderingDomainException(string message,Exception innerException) : base(message, innerException) { }



    }
}
