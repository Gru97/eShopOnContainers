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

        // This is a backing field. _propertyName is the ef convention for configuring them
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                    throw new OrderingDomainException("Ivalid quantity");
                _quantity = value;
            }

        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }

        }

        private int _productId;
        public int ProductId
        {
            get { return _productId; } set { }

        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName=value; }

        }

        private decimal _discount;
        public decimal Discount
        {
            get { return _discount; }
            set
            {
                if (value < 0)
                    throw new OrderingDomainException("Discount is not valid");
                _discount = value;
            }


        }

        private string _pictureUri;

        public string PictureUri
        {
            get { return _pictureUri; }
            set { }
        }


        public OrderItem()
        {

        }

        public OrderItem(int _quantity, decimal _unitPrice, int _productId, string _productName, decimal _discount):this()
        {

            //These are validations. We don't let an object enter an invalid state so we would check if it's valid or not later.
            //Validation happens in ctor or setters of props
            if(_quantity <= 0)
                throw new OrderingDomainException("Invalid product _quantity");
            if (_discount > _unitPrice)
                throw new OrderingDomainException("The _discount is greater than price of th product");

            this._quantity = _quantity;
            this._unitPrice = _unitPrice;
            this._productId = _productId;
            this._productName = _productName;
            this._discount = _discount;
        }
        public void AddToQuantity(int units)
        {
            if (units < 0)
                throw new OrderingDomainException("Invalid units");
            this._quantity += units;
        }
    }
}
