using System;
using System.Collections.Generic;

namespace Стройматериалы.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public string ProductArticleNumber { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public int ProductCategoryId { get; set; }
        public byte[]? ProductPhoto { get; set; }
        public int ProductManufacturerId { get; set; }
        public decimal ProductCost { get; set; }
        public byte? ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public int ProductUnitId { get; set; }
        public byte? ProductDiscountMax { get; set; }
        public int? SupplierId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ProductManufacturer ProductManufacturer { get; set; } = null!;
        public virtual ProductUnit ProductUnit { get; set; } = null!;
        public virtual ProductSupplier? Supplier { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
