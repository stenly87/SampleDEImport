using System;
using System.Collections.Generic;

namespace Стройматериалы
{
    public partial class ProductUnit
    {
        public ProductUnit()
        {
            Products = new HashSet<Product>();
        }

        public int UnitId { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
