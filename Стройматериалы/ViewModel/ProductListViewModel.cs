using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Стройматериалы.DB;
using Стройматериалы.Models;
using Стройматериалы.Tools;
using Стройматериалы.View;

namespace Стройматериалы.ViewModel
{
    internal class ProductListViewModel : BaseViewModel
    {
        private User user;
        private MainViewModel mainViewModel;
        private List<Product> products;
        private string search = "";
        private ProductManufacturer selectedManufacturer;
        private string sortPrice;

        public List<ProductManufacturer> Manufacturers { get; set; }
        public ProductManufacturer SelectedManufacturer
        {
            get => selectedManufacturer;
            set
            {
                selectedManufacturer = value;
                DoSearch();
            }
        }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                DoSearch();
            }
        }
        bool sortPriceAsc = false;
        private string rowsCount;

        public string SortPrice
        {
            get => sortPrice;
            set
            {
                sortPrice = value;
                sortPriceAsc = sortPrice.Contains("По возрастанию");
                DoSearch();
            }
        }

        public List<Product> Products
        {
            get => products;
            set
            {
                products = value;
                SignalChanged();
            }
        }
        public Product SelectedProduct { get; set; }
        public string RowsCount
        {
            get => rowsCount;
            set
            {
                rowsCount = value;
                SignalChanged();
            }
        }

        public Visibility IsAdminVisibility { get => user.UserRole == 1 ? Visibility.Visible : Visibility.Collapsed; }

        public ViewCommand RemoveProduct { get; set; }
        public ViewCommand AddProduct { get; set; }
        public ViewCommand EditProduct { get; set; }

        public ProductListViewModel(MainViewModel mainViewModel)
        {
            this.user = Auth.Auth.CurrentUser;
            this.mainViewModel = mainViewModel;

            Manufacturers = DEContext.GetInstance().ProductManufacturers.ToList();
            Manufacturers.Insert(0, new ProductManufacturer { Title = "Все производители" });
            SelectedManufacturer = Manufacturers[0];

            RemoveProduct = new ViewCommand(() => {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Необходимо выбрать товар из списка для его удаления");
                    return;
                }
                if (SelectedProduct.OrderProducts.Count > 0)
                {
                    MessageBox.Show("Выбранный товар нельзя удалить, поскольку он участвует в заказах");
                    return;
                }
                try
                {
                    DEContext.GetInstance().Products.Remove(SelectedProduct);
                    DEContext.GetInstance().SaveChanges();
                    DoSearch(); // либо удалять из Products в вьюмодели и использовать ObservableCollection
                    MessageBox.Show("Выбранный товар удален");
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при удалении товара");
                }
            });

            AddProduct = new ViewCommand(() => {
                mainViewModel.CurrentPage = new EditProductPage(new Product(), mainViewModel);
            });

            EditProduct = new ViewCommand(() => {
                if (SelectedProduct == null)
                {
                    MessageBox.Show("Для редактирования продукта нужно его выбрать");
                    return;
                }
                mainViewModel.CurrentPage = new EditProductPage(SelectedProduct, mainViewModel);
            });
        }

        private void DoSearch()
        {
            try
            {
                var countAll = DEContext.GetInstance().Products.Count();

                var searchProducts = DEContext.GetInstance().Products
                                .Include("ProductManufacturer")
                                .Include("ProductUnit")
                                .Include("Supplier")
                                .Include("OrderProducts")
                                .Include("ProductCategory").Where(
                    s => s.ProductArticleNumber.Contains(Search) ||
                        s.ProductCategory.Title.Contains(Search) ||
                        s.ProductDescription.Contains(Search) ||
                        s.ProductManufacturer.Title.Contains(Search) ||
                        s.ProductName.Contains(Search) ||
                        s.Supplier.Title.Contains(Search)
                        );

                if (SelectedManufacturer.ManufacturerId != 0)
                    searchProducts = searchProducts.Where(s => s.ProductManufacturerId == SelectedManufacturer.ManufacturerId);
                if (sortPriceAsc)
                    searchProducts = searchProducts.OrderBy(s => s.ProductCost);
                else
                    searchProducts = searchProducts.OrderByDescending(s => s.ProductCost);

                var prod = searchProducts.ToList();

                prod.ForEach(s =>
                {
                    s.Image = new BitmapImage();
                    s.Image.BeginInit();
                    s.Image.CacheOption = BitmapCacheOption.OnLoad;
                    if (s.ProductPhoto != null)
                    {
                        var ms = new MemoryStream(s.ProductPhoto);
                        s.Image.StreamSource = ms;
                    }
                    else
                        s.Image.StreamSource = File.OpenRead("picture.png");
                    s.Image.EndInit();
                    s.Image.StreamSource.Close(); // обязательно
                });

                if (prod.Count == 0)
                    MessageBox.Show("По данному запросу ничего не найдено");

                RowsCount = $"Получено {prod.Count} из {countAll} записей";

                Products = prod;
            }
            catch { }
        }
    }
}
