using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Eight
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            CmbType.ItemsSource = Helper.GetContext().ProductType.ToList();
            ProductsBox.ItemsSource = Helper.GetContext().Material.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            if (ProductsBox.SelectedItem is Material material)
            {
                var materialDuplicate = BasketBox.Items.Cast<Record>().ToList().SingleOrDefault(x => x.Material == material);
                if (materialDuplicate == null)
                {
                    BasketBox.Items.Add(new Record() { Material = material, Count = 1 });
                }
                else
                {
                    materialDuplicate.Count++;
                }
                BasketBox.Items.Refresh();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (BasketBox.SelectedItem is Record record)
            {
                BasketBox.Items.Remove(record);
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbType.SelectedItem is ProductType productType
                && decimal.TryParse(TbMinCost.Text, out decimal cost)
                && int.TryParse(TbPerson.Text, out int personCount)
                && int.TryParse(TbWorkshop.Text, out int workshop))
            {
                Product product = new Product()
                {
                    Title = TbName.Text,
                    ProductType = productType,
                    Description = TbDescription.Text,
                    ArticleNumber = TbArticle.Text,
                    Image = path,
                    MinCostForAgent = cost,
                    ProductionPersonCount = personCount,
                    ProductionWorkshopNumber = workshop,
                    ProductMaterial = BasketBox.Items.Cast<Record>().ToList().Select(x => new ProductMaterial() { Material = x.Material, Count = x.Count }).ToList(),
                };
                Helper.GetContext().Product.Add(product);
                Helper.GetContext().SaveChanges();
            }
        }
        class Record
        {
            public Material Material { get; set; }
            public int Count { get; set; }
        }
        public string path;
        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = openFileDialog.FileName;
                    path = "\\products\\" + openFileDialog.SafeFileName;
                    File.Copy(fileName, Environment.CurrentDirectory + "\\products\\" + openFileDialog.SafeFileName, true);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\products\\" + openFileDialog.SafeFileName);
                    bitmapImage.EndInit();
                    Img.Source = bitmapImage;
                }
            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
