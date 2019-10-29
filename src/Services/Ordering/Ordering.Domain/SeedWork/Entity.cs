using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.SeedWork
{
    public abstract class Entity
    {
        private int id;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }
        public override bool Equals(object obj)
        {
            //TODO Implement this
            return base.Equals(obj);
        }


    }
}
