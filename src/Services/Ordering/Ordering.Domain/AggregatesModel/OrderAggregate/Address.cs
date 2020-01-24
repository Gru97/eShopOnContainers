using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Address:ValueObject
    {
        //This is a value object inside order aggregate. It doesn't have identity.
        //It's more like "a bunch of data in the form of properties" for Order class
        //Value objects are immutable. That's why we have private set and no setters. 
        [JsonProperty]
        public string Street { get; private set; }

        [JsonProperty]

        public string City { get; private set; }
        [JsonProperty]

        public string State { get; private set; }
        [JsonProperty]

        public string Country { get; private set; }
        [JsonProperty]

        public string ZipCode { get; private set; }
        public Address()
        {

        }
        public Address(string Street, string City, string State, string Country, string ZipCode)
        {
            this.Street = Street;
            this.City = City;
            this.Country = Country;
            this.State = State;
            this.ZipCode = ZipCode;
        }

        protected override IEnumerable<object> GetPropertyValues()
        {
            yield return Street;
            yield return City;
            yield return Country;
            yield return State;
            yield return ZipCode;
        }
    }
}
