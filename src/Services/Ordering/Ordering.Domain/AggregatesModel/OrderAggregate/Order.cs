using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    // IAggregateRoot plays the role of a marker
    // No navigation property of buyer here. Entites are related to each other only by foreign key
    // Encapsulation is very important to make sure business rules and validations has applied, we make setters private and we use validation before setting value to a field
    // For collections, they all expose "Add" and other methods and they can  be manipulated from outside unwantedly, even when their setters are private
    // So we must make them Readonly and define custom methods for adding items to them etc.
    public class Order:Entity, IAggregateRoot
    {
        private DateTime _orderDate;


        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }


        public Address Address { get; private set; }
        private int? _buyerId;

        public int? BuyerId
        {
            get { return _buyerId; }
            set { _buyerId = value; }
        }


        public OrderState OrderState { get;  private set; }

        private string _description;

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems { get { return _orderItems; } }

        //private int paymentMethodId;

        //public int PaymentMethodId
        //{
        //    get { return paymentMethodId; }
        //    set { paymentMethodId = value; }
        //}

        public Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(string userId, string userName,Address address, int? _buyerId=null, int? paymentMethodId=null)
        {
            _orderItems = new List<OrderItem>();
            Address = address;
            this._buyerId = _buyerId;
            //this.paymentMethodId = paymentMethodId;
            _orderDate = DateTime.Now;
            OrderState = OrderState.Submitted;

            //When an order is created, it means the user becomes a buyer. 
            //so we need to raise an event to say an order is started, and create a buyer out of the current user info
            //Although we do not "raise" the event here. We add it to a list of events to be raised/dispatched later
            AddOrderStartedDomainEvent(userId, userName);

        }

        private void AddOrderStartedDomainEvent(string userId, string userName)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName);

            this.AddDomainEvent(orderStartedDomainEvent);
        }

        public void AddOrderItem(int productId,string productName,decimal unitPrice,decimal discount, int quantity=1)
        {
            var existingProduct = _orderItems.SingleOrDefault(e => e.ProductId == productId);
            if(existingProduct!=null)
            {
                if (existingProduct.Discount < discount)
                    existingProduct.Discount = discount;

                existingProduct.AddToQuantity(quantity);

            }
            else
            {
                var orderItem = new OrderItem(quantity, unitPrice, productId, productName, discount);
                _orderItems.Add(orderItem);
            }
        }

        public void SetAwaitingValidationStatus()
        {
            //after order submited
            if(OrderState==OrderState.Submitted)
            {
                AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id,_orderItems));
                OrderState = OrderState.AwaitingValidation;
                
            }
        }
        public void SetStockConfirmedStatus()
        {
            if(OrderState==OrderState.AwaitingValidation)
            {
                //AddDomainEvent(new OrderStateChangedToStockConfirmedDomainEvent(Id));
                OrderState = OrderState.StockConfirmed;
                _description = "همه اقلام در انبار موجودند. خرید شما با موفقیت انجام شد.";
            }
        }
        public void SetPaidStatus()
        {
            if(OrderState==OrderState.StockConfirmed)
            {
                AddDomainEvent(new OrderStateChangedToPaidDomainEvent(Id, _orderItems));
                OrderState = OrderState.Paid;
                _description = "Payment is done successfully";
            }
        }
        public void SetShippedStatus()
        {
            if(OrderState==OrderState.Paid)
            {
                OrderState = OrderState.Shipped;
                _description = "The order was shipped";
                AddDomainEvent(new OrderShippedDomainEvent(this));
            }
        }
        public void SetCancelledStatus() { }
        public void SetCancelledStatusWhenStockIsRejected()
        {
            if (OrderState == OrderState.AwaitingValidation)
            {
                
                 OrderState = OrderState.Cancelled;
                _description = "بعضی اقلام در انبار موجود نیست. خرید شما لغو شد.";
            }
        }
        
    }
}
