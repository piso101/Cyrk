using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndCyrk.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
            OrderDate = DateTime.Now;
        }

        public void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
        }

        public decimal GetTotalAmount()
        {
            return OrderItems.Sum(item => item.TotalPrice);
        }
    }
}
