using System;
using System.Collections.Generic;

namespace Стройматериалы.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public int OrderPickupPointId { get; set; }
        public int CodeOrder { get; set; }
        public int? ClientId { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual User? Client { get; set; }
        public virtual PickupPoint OrderPickupPoint { get; set; } = null!;
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
