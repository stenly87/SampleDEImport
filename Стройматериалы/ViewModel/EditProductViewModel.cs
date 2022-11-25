using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Стройматериалы.DB;
using Стройматериалы.Models;
using Стройматериалы.Tools;
using Стройматериалы.View;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Стройматериалы.ViewModel
{
    internal class EditProductViewModel: BaseViewModel
    {
        private readonly MainViewModel mainViewModel;
        public Product EditProduct { get; set; }

        public List<ProductCategory> Categories { get; set; }
        public List<ProductUnit> Units { get; set; }
        public List<ProductSupplier> Suppliers { get; set; }
        public List<ProductManufacturer> Manufacturers { get; set; }

        public ViewCommand SelectImage { get; set; }
        public ViewCommand RemoveImage { get; set; }
        public ViewCommand BackToList { get; set; }
        public ViewCommand Save { get; set; }

        public bool CanEditArticle { get; set; }

        Product original;
        public EditProductViewModel(Product edit, MainViewModel mainViewModel)
        {
            original = edit;

            CanEditArticle = !string.IsNullOrEmpty(edit.ProductArticleNumber);
            var db = DEContext.GetInstance();
            Manufacturers = db.ProductManufacturers.ToList();
            Categories = db.ProductCategories.ToList();
            Units = db.ProductUnits.ToList();
            Suppliers = db.ProductSuppliers.ToList();

            this.EditProduct = new Product { 
                Image = edit.Image,
                OrderProducts = edit.OrderProducts,
                ProductArticleNumber = edit.ProductArticleNumber,
                ProductCategory = edit.ProductCategory,
                ProductCost = edit.ProductCost,
                ProductDescription = edit.ProductDescription,
                ProductDiscountAmount = edit.ProductDiscountAmount,
                ProductDiscountMax = edit.ProductDiscountMax,
                ProductManufacturer = edit.ProductManufacturer,
                ProductName = edit.ProductName,
                ProductPhoto = edit.ProductPhoto,
                ProductQuantityInStock = edit.ProductQuantityInStock,
                ProductUnit = edit.ProductUnit,
                Supplier = edit.Supplier,
                ProductCategoryId = edit.ProductCategoryId,
                ProductManufacturerId = edit.ProductManufacturerId,
                ProductUnitId = edit.ProductUnitId,
                SupplierId = edit.SupplierId
            };
            this.mainViewModel = mainViewModel;

            SelectImage = new ViewCommand(() => {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "*.jpg|*.png";
                if (fileDialog.ShowDialog() == true)
                {
                    EditProduct.ProductPhoto = File.ReadAllBytes(
                        fileDialog.FileName);
                    var ms = new MemoryStream(EditProduct.ProductPhoto);
                    UpdateImage(ms);
                }
            });

            RemoveImage = new ViewCommand(() => {
                if (MessageBox.Show("Вы действительно хотите удалить изображение?", "Предупреждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
                EditProduct.ProductPhoto = null;
                var fs = File.OpenRead("picture.png");
                UpdateImage(fs);
            });

            BackToList = new ViewCommand(() => {
                mainViewModel.CurrentPage = new ProductListPage(mainViewModel);
            });

            Save = new ViewCommand(() => {
                try
                {
                    if (EditProduct.ProductPhoto != null)
                    {
                        if (EditProduct.Image.PixelHeight > 200 ||
                            EditProduct.Image.PixelWidth > 300)
                        {
                            MessageBox.Show("Невозможно сохранить продукт. Изображение слишком большое.");
                            return;
                        }
                    }
                    if (EditProduct.ProductQuantityInStock < 0)
                    {
                        MessageBox.Show("Невозможно сохранить продукт. Кол-во не может быть меньше 0");
                        return;
                    }
                    if (EditProduct.ProductCost < 0)
                    {
                        MessageBox.Show("Невозможно сохранить продукт. Стоимость не может быть меньше 0");
                        return;
                    }
                    DEContext.GetInstance().Entry<Product>(original).
                        CurrentValues.SetValues(EditProduct);
                    DEContext.GetInstance().SaveChanges();
                    BackToList.Execute(null);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Произошла ошибка при сохранении изменений");
                }
            });
        }

        void UpdateImage(Stream stream)
        {
            EditProduct.Image = new BitmapImage();
            EditProduct.Image.BeginInit();
            EditProduct.Image.CacheOption = BitmapCacheOption.OnLoad;
            EditProduct.Image.StreamSource = stream;
            EditProduct.Image.EndInit();
            EditProduct.Image.StreamSource.Close();
            SignalChanged(nameof(EditProduct));
        }
    }
}
