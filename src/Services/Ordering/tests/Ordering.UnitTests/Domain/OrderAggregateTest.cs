using Ordering.Domain.AggregatesModel.OrderAggregate;
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
    }
}
