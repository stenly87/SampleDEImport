using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Стройматериалы
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            DEContext dEContext = new DEContext();

            /*string file = @"C:\Users\Student\Desktop\09_1.1-2022_4\Вариант 4\Вариант 4\Сессия 1\Товар_import_Стройматериалы.csv";
            string dirImage = @"C:\Users\Student\Desktop\09_1.1-2022_4\Вариант 4\Вариант 4\Сессия 1\Товар_import\";
            string[] rows = File.ReadAllLines(file).Skip(1).ToArray();
            var spl = new char[] { ';' };
            Dictionary<string, ProductManufacturer> pm = new Dictionary<string, ProductManufacturer>();
            Dictionary<string, ProductSupplier> ps = new Dictionary<string, ProductSupplier>();
            Dictionary<string, ProductCategory> pc = new Dictionary<string, ProductCategory>();



            foreach (var row in rows)
            {
                var cols = row.Split(spl, StringSplitOptions.RemoveEmptyEntries);
                ProductManufacturer productManufacturer;
                 // dEContext.ProductManufacturers.FirstOrDefault(s => s.Title == cols[5]);
                if (!pm.TryGetValue(cols[5], out productManufacturer))
                {
                    productManufacturer = new ProductManufacturer { Title = cols[5] };
                    pm.Add(cols[5], productManufacturer);
                    dEContext.ProductManufacturers.Add(productManufacturer);
                }
                ProductSupplier productSupplier = dEContext.ProductSuppliers.FirstOrDefault(s => s.Title == cols[6]);
                if (productSupplier == null)
                {
                    productSupplier = new ProductSupplier { Title = cols[6] };
                    dEContext.ProductSuppliers.Add(productSupplier);
                }
                ProductCategory productCategory = dEContext.ProductCategories.FirstOrDefault(s => s.Title == cols[7]);
                if (productCategory == null)
                {
                    productCategory = new ProductCategory { Title = cols[7] };
                    dEContext.ProductCategories.Add(productCategory);
                }
                Product product = new Product
                {
                     ProductArticleNumber = cols[0],
                     ProductName = cols[1],
                     ProductUnitId = 1,
                     ProductCost = decimal.Parse(cols[3]),
                     ProductDiscountMax = byte.Parse(cols[4]),
                     ProductDiscountAmount = byte.Parse(cols[8]),
                     ProductQuantityInStock = int.Parse(cols[9]),
                     ProductDescription = cols[10],
                     ProductManufacturer = productManufacturer,
                     Supplier = productSupplier,
                     ProductCategory = productCategory,
                };
                if (cols.Length == 12)
                    product.ProductPhoto = File.ReadAllBytes(dirImage + cols[11]);
                dEContext.Products.Add(product);
            }
            dEContext.SaveChanges();*/
            /*
            var puncts = File.ReadAllLines(@"C:\Users\Student\Desktop\09_1.1-2022_4\Вариант 4\Вариант 4\Сессия 1\Пункты выдачи_import.csv");
            dEContext.PickupPoints.AddRange(puncts.Select(s=>new PickupPoint { Address = s  }));
            dEContext.SaveChanges();*/

            var rows = File.ReadAllLines(@"C:\Users\Student\Desktop\09_1.1-2022_4\Вариант 4\Вариант 4\Сессия 1\Заказ_import.csv").Skip(1).ToArray();
            var spl = new char[] { ';' };
            foreach (var row in rows)
            {
                var cols = row.Split(spl);
                Order order = new Order
                {
                    OrderDeliveryDate = Convert.ToDateTime(cols[3]),
                    OrderDate = Convert.ToDateTime(cols[2]),
                    OrderPickupPointId = Convert.ToInt32(cols[4]),
                    CodeOrder = Convert.ToInt32(cols[6]),
                    OrderStatusId = Convert.ToInt32(cols[7])
                };

                if (!string.IsNullOrEmpty(cols[5]))
                {
                    var fio = cols[5].Split();
                    order.Client = dEContext.Users.First(s => s.UserSurname == fio[0] && s.UserName == fio[1] && s.UserPatronymic == fio[2]);
                }
                var p = cols[1].Split(',');
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductArticleNumber = p[0],
                    Count = Convert.ToInt32(p[1])
                });
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductArticleNumber = p[2],
                    Count = Convert.ToInt32(p[3])
                });
                dEContext.Orders.Add(order);
            }
            dEContext.SaveChanges();
        }
    }
}
