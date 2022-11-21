using System;
using System.Collections.Generic;

namespace Стройматериалы.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string? Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
