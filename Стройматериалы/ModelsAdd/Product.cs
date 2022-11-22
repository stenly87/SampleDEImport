using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;

namespace Стройматериалы.Models
{
    public partial class Product
    {
        [NotMapped]
        public BitmapImage Image { get; set; }
    }
}
