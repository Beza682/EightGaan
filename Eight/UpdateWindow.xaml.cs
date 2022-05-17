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
using System.Windows.Shapes;

namespace Eight
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private List<Product> _products;
        public UpdateWindow(List<Product> products)
        {
            InitializeComponent();
            _products = products;
            TbCost.Text = _products.Select(x => x.MinCostForAgent).Average().ToString();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

            if (decimal.TryParse(TbCost.Text, out decimal cost))
            {
                foreach (var product in _products)
                {
                    product.MinCostForAgent = cost;
                }
                Helper.GetContext().SaveChanges();
                MessageBox.Show("Стоимость для агентов изменена.", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Close();
        }
    }
}
