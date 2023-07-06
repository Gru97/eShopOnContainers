using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.SeedWork
{
    public abstract class ValueObject
    {
        // value object camparison is done by comparing all properties of it. Since it has no identity
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            ValueObject other = (ValueObject)obj;
            IEnumerator<object> thisValues = GetPropertyValues().GetEnumerator();
            IEnumerator<object> otherValues = GetPropertyValues().GetEnumerator();

            while(thisValues.MoveNext() && otherValues.MoveNext())
            {
                //Since a value object can have properties that are entity, we should check for reference equality
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                    return false;
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                    return false;

            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
           
        }


        //This method will be implemented in derived class and it will return values of all properties of that class
        protected abstract IEnumerable<object> GetPropertyValues();
    }
}
