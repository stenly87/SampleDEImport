using System;
using System.Collections.Generic;

namespace Стройматериалы
{
    public partial class ProductSupplier
    {
        public ProductSupplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
