﻿using FluentValidator;
using ModernStore.Domain.Enums;
using ModernStore.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernStore.Domain.Entities
{
    public class Order : Entity
    {
        protected Order() { }
        public readonly IList<OrderItem> _items;
        public Order(Customer customer, decimal deliveryFee, decimal discount)
        {
            Customer = customer;
            CreateDate = DateTime.Now;
            Number = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Status = EOrderStatus.Created;
            _items = new List<OrderItem>();
            DeliveryFee = deliveryFee;
            Discount = discount;

            new ValidationContract<Order>(this)
                .IsGreaterThan(x => x.DeliveryFee, 0)
                .IsGreaterThan(x => x.Discount, -1);
        }

        public Customer Customer { get; private set; }
        public DateTime CreateDate { get; private set; }

        public string Number { get; private set; }
        public EOrderStatus Status  { get; private set; }
        public ICollection<OrderItem> Items => _items.ToArray();
        public decimal DeliveryFee { get; private set; }
        public decimal Discount { get; private set; }

        public decimal SubTotal() => Items.Sum(x => x.Total());

        public decimal Total() => SubTotal() + DeliveryFee - Discount;

        public void AddItem(OrderItem item)
        {
            AddNotifications(item.Notifications);
            if (item.IsValid())
            {
                _items.Add(item);
            }
        }
        
    }
}
