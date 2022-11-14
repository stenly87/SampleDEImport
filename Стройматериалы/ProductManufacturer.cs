using System;
using System.Collections.Generic;

namespace Стройматериалы
{
    public partial class ProductManufacturer
    {
        public ProductManufacturer()
        {
            Products = new HashSet<Product>();
        }

        public int ManufacturerId { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
