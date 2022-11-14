using System;
using System.Collections.Generic;

namespace Стройматериалы
{
    public partial class PickupPoint
    {
        public PickupPoint()
        {
            Orders = new HashSet<Order>();
        }

        public int PointId { get; set; }
        public string Address { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
