using System;
using System.Collections.Generic;
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

namespace Eight
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _sort = new List<string>()
        {
            "Наименование (по возрастанию)","Наименование (по убыванию)",
            "Номер производственного цеха (по возрастанию)","Номер производственного цеха (по убыванию)",
            "Минимальная стоимость для агента (по возрастанию)", "Минимальная стоимость для агента (по убыванию)"
        };
        private int _pagesIndex = 1;
        private int _totalPages = 0;
        private int _takesProducts = 20;
        public MainWindow()
        {
            InitializeComponent();
            ProductsList.ItemsSource = Helper.GetContext().Product.ToList();
            CmbSort.ItemsSource = _sort;
            var filterCollection = Helper.GetContext().ProductType.ToList();
            filterCollection.Insert(0, new ProductType() { ID = 0, Title = "Все типы" });
            CmbFilter.ItemsSource = filterCollection;
        }

        private void SetNumbers(List<Product> products)
        {
            NumbersList.Items.Clear();
            _totalPages = products.Count % 20 == 0 ? products.Count / 20 : products.Count / 20 + 1;
            for (int i = 1; i <= _totalPages; i++)
            {
                NumbersList.Items.Add(i);
            }
        }

        private void LoadData()
        {
            var products = Helper.GetContext().Product.ToList();
            switch (CmbSort.SelectedIndex)
            {
                case 0:
                    products = products.OrderBy(x => x.Title).ToList();
                    break;
                case 1:
                    products = products.OrderByDescending(x => x.Title).ToList();
                    break;
                case 2:
                    products = products.OrderBy(x => x.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    products = products.OrderByDescending(x => x.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    products = products.OrderBy(x => x.MinCostForAgent).ToList();
                    break;
                case 5:
                    products = products.OrderByDescending(x => x.MinCostForAgent).ToList();
                    break;
            }
            if (CmbFilter.SelectedItem is ProductType productType)
            {
                products = products
                    .Where(x =>
                (x.ProductType == productType || productType.ID == 0) &&
                (x.Title.Contains(TbSearch.Text) || x.NDescription.Contains(TbSearch.Text)))
                    .ToList();
            }
            SetNumbers(products);
            products = products.Skip((_pagesIndex - 1) * 20).Take(_takesProducts).ToList();
            ProductsList.ItemsSource = products;
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsList.SelectedItems.Count > 1)
            {
                BtnUpdate.Visibility = Visibility.Visible;
            }
            else
            {
                BtnUpdate.Visibility = Visibility.Collapsed;
            }
        }

        private void TbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void TbDown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_pagesIndex > 1)
            {
                _pagesIndex -= 1;
                LoadData();
            }
        }

        private void TbNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_pagesIndex < _totalPages)
            {
                _pagesIndex += 1;
                LoadData();
            }
        }

        private void TbNumber_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _pagesIndex = Convert.ToInt32((sender as TextBlock).Text);
            LoadData();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var products=ProductsList.SelectedItems.Cast<Product>().ToList();
            new UpdateWindow(products).ShowDialog();
            LoadData();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow().ShowDialog();
            LoadData();
        }

        private void BtnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
