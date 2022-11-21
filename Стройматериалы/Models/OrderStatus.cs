using System;
using System.Collections.Generic;

namespace Стройматериалы.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int StatusId { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
