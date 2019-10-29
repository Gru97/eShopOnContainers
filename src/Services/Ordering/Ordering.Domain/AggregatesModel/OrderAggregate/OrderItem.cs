using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem:Entity
    {
        //This is a child entity inside OrderAggregate. 
        //The root aggregate is Order entitiy. We don't have an "AddItem" method here.
        //This entity is controlled by it's agge. root for the sake of maintaining consistancy across aggregate
        //So Order entity has an AddItem method which uses this class'c ctor
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            private set { }

        }

        private decimal unitPrice;
        public decimal UnitPrice
        {
            get { return unitPrice; }
            private set { }

        }

        private int productId;
        public int ProductId
        {
            get { return productId; }
            private set { }

        }
        private string productName;

        public string ProductName
        {
            get { return productName; }
            private set { }

        }

        private decimal discount;

        public decimal Discount
        {
            get { return discount; }
            set
            {
                if (value < 0)
                    throw new OrderingDomainException("Discount is not valid");
                discount = value;
            }


        }
        private string pictureUri;

        public string PictureUri
        {
            get { return pictureUri; }
            private set { }
        }


        public OrderItem()
        {

        }

        public OrderItem(int quantity, decimal unitPrice, int productId, string productName, decimal discount):this()
        {

            //These are validations. We don't let an object enter an invalid state so we would check if it's valid or not later.
            //Validation happens in ctor or setters of props
            if(quantity <= 0)
                throw new OrderingDomainException("Invalid product quantity");
            if (discount > unitPrice)
                throw new OrderingDomainException("The discount is greater than price of th product");

            this.quantity = quantity;
            this.unitPrice = unitPrice;
            this.productId = productId;
            this.productName = productName;
            this.discount = discount;
        }
        public void AddToQuantity(int units)
        {
            if (units < 0)
                throw new OrderingDomainException("Invalid units");
            this.quantity += units;
        }
    }
}
