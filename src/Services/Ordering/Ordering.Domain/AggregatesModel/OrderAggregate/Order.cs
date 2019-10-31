﻿using Ordering.Domain.Events;
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
        private DateTime orderDate;
        public Address Address { get; private set; }
        private int? buyerId;

        public int? BuyerId
        {
            get { return buyerId; }
            set { buyerId = value; }
        }

        public OrderState OrderState { get;  private set; }

        private string description;

        private readonly List<OrderItem> orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems { get { return orderItems; } }

        private int paymentMethodId;

        public int PaymentMethodId
        {
            get { return paymentMethodId; }
            set { paymentMethodId = value; }
        }

        public Order()
        {
            orderItems = new List<OrderItem>();
        }

        public Order(string userId, string userName,Address address, int? buyerId, int paymentMethodId)
        {
            Address = address;
            this.buyerId = buyerId;
            this.paymentMethodId = paymentMethodId;
            orderDate = DateTime.UtcNow;
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

        public void AddOrderItem(int productId,string productName,decimal unitPrice,decimal discount,string pictureUri, int quantity=1)
        {
            var existingProduct = orderItems.SingleOrDefault(e => e.Id == productId);
            if(existingProduct!=null)
            {
                if (existingProduct.Discount < discount)
                    existingProduct.Discount = discount;

                existingProduct.AddToQuantity(quantity);

            }
            else
            {
                var orderItem = new OrderItem(quantity, unitPrice, productId, productName, discount);
                orderItems.Add(orderItem);
            }
        }

        public void SetAwaitingValidationStatus()
        {
            //after order submited
            if(OrderState==OrderState.Submitted)
            {
                AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id,orderItems));
                OrderState = OrderState.AwaitingValidation;
                
            }
        }
        public void SetStockConfirmedStatus()
        {
            if(OrderState==OrderState.AwaitingValidation)
            {
                AddDomainEvent(new OrderStateChangedToStockConfirmedDomainEvent(id));
                OrderState = OrderState.StockConfirmed;
                description = "All items were available in stock";
            }
        }
        public void SetPaidStatus()
        {
            if(OrderState==OrderState.StockConfirmed)
            {
                AddDomainEvent(new OrderStateChangedToPaidDomainEvent(Id, orderItems));
                OrderState = OrderState.Paid;
                description = "Payment is done successfully";
            }
        }
        public void SetShippedStatus()
        {
            if(OrderState==OrderState.Paid)
            {
                OrderState = OrderState.Shipped;
                description = "The order was shipped";
                AddDomainEvent(new OrderShippedDomainEvent(this));
            }
        }
        public void SetCancelledStatus() { }
        public void SetCancelledStatusWhenStockIsRejected() { }
        
    }
}