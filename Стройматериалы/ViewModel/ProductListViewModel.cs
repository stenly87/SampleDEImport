using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Стройматериалы.DB;
using Стройматериалы.Models;
using Стройматериалы.Tools;

namespace Стройматериалы.ViewModel
{
    internal class ProductListViewModel : BaseViewModel
    {
        private User user;
        private MainViewModel mainViewModel;
        private List<Product> products;

        public List<ProductManufacturer> Manufacturers { get; set; }

        public List<Product> Products
        {
            get => products;
            set
            {
                products = value;
                SignalChanged();
            }
        }

        public ProductListViewModel(User user, MainViewModel mainViewModel)
        {
            this.user = user;
            this.mainViewModel = mainViewModel;

            Manufacturers = DEContext.GetInstance().ProductManufacturers.ToList();
            Manufacturers.Insert(0, new ProductManufacturer { Title = "Все производители" });
            Search();
        }

        private void Search()
        {
            Products = DEContext.GetInstance().Products
                            .Include("ProductManufacturer")
                            .Include("ProductUnit")
                            .Include("Supplier")
                            .Include("ProductCategory").ToList();
            
            Products.ForEach(s =>
            {
                s.Image = new BitmapImage();
                s.Image.BeginInit();
                s.Image.CacheOption = BitmapCacheOption.OnLoad;
                s.Image.StreamSource = File.OpenRead("picture.png");
                s.Image.EndInit();
                s.Image.StreamSource.Close(); // обязательно
            });
        }
    }
}
