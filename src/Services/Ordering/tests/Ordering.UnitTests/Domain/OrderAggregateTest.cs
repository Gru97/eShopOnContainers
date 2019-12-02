using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ordering.UnitTests.Domain
{
    public class OrderAggregateTest
    {
        [Fact]
        public void Create_order_Item_success()
        {
            //Arrange
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 10;
            var units = 5;

            //Act
            var fakeOrderItem = new OrderItem(units, unitPrice,productId, productName, discount );

            //Assert
            Assert.NotNull(fakeOrderItem);

        }

        [Fact]
        public void Invalid_number_of_units()
        {

            //Arrange
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 10;
            var units = -1;

            //Act-Assert
            Assert.Throws<OrderingDomainException>(() => new OrderItem(units, unitPrice, productId, productName, discount));

        }

        [Fact]
        public void Invalid_total_of_order_item_lower_than_discount()
        {
            //Arrange
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 15;
            var units = 1;

            //Act-Assert
            Assert.Throws<OrderingDomainException>(() => new OrderItem(units, unitPrice, productId, productName, discount));
        }   

        [Fact]
        public void Invalid_discount_setting()
        {
            //Arrange
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 10;
            var units = 1;

            //Act
            var fakeOrderItem = new OrderItem(units, unitPrice, productId, productName, discount);

            Assert.Throws<OrderingDomainException>(() => fakeOrderItem.Discount = -1);

        }

        [Fact]
        public void Invalid_quantity_setting()
        {
            //Arrange
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 10;
            var units = 1;

            //Act
            var fakeOrderItem = new OrderItem(units, unitPrice, productId, productName, discount);

            //Assert
            Assert.Throws<OrderingDomainException>(()=>fakeOrderItem.Quantity=-1);
        }

        [Fact]
        public void Add_new_order_raises_new_event()
        {
            //Arrange
            var street = "fakeStreet";
            var city = "FakeCity";
            var state = "fakeState";
            var country = "fakeCountry";
            var zipcode = "FakeZipCode";
            var expectedResult = 1;

            //Act 
            var fakeOrder = new Order("1", "fakeName", 
                new Address(street, city,state, country, zipcode) );

            //Assert
            Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);


        }
        [Fact]
        public void Add_event_order_explicitly_raises_new_event()
        {
            //Arrange
            var street = "fakeStreet";
            var city = "FakeCity";
            var state = "fakeState";
            var country = "fakeCountry";
            var zipcode = "FakeZipCode";
            var expectedResult = 2;

            //Act 
            var fakeOrder = new Order("1", "fakeName",
                new Address(street, city, state, country, zipcode));
            fakeOrder.AddDomainEvent(new OrderStartedDomainEvent(fakeOrder,"fakename","1"));

            //Assert
            Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);


        }

        [Fact]
        public void Remove_event_order_explicitly_raises_new_event()
        {
            //Arrange
            var street = "fakeStreet";
            var city = "FakeCity";
            var state = "fakeState";
            var country = "fakeCountry";
            var zipcode = "FakeZipCode";
            var expectedResult = 1;

            //Act 
            var fakeOrder = new Order("1", "fakeName",
                new Address(street, city, state, country, zipcode));
            var fakeEvent = new OrderStartedDomainEvent(fakeOrder, "fakename", "1");
            fakeOrder.AddDomainEvent(fakeEvent);
            fakeOrder.RemoveDomainEvent(fakeEvent);

            //Assert
            Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);


        }
    }
}
