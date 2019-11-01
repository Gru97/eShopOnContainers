using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.SeedWork
{
    // Repository pattern is used only for operations that change the state of the data in database such as adding, updating and deleting
    // Mainly the "command part" of the CQRS pattern
    // We has a repository per aggregate root (not per entity/table), to maintain consistancy across the aggregate
    // So it's good to enforce those who implement this interface to be aggregate roots, hence the "where" part
    public interface IRepository<T> where T:IAggregateRoot
    {
        //A wrapper for DbContext in EF
         IUnitOfWork UnitOfWork { get; }
    }
}
