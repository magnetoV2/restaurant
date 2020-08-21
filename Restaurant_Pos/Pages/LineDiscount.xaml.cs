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

namespace Restaurant_Pos.Pages
{
    /// <summary>
    /// Interaction logic for LineDiscount.xaml
    /// </summary>
    public partial class LineDiscount : Page
    {
        public LineDiscount()
        {
            InitializeComponent();
            MessageBox.Show("hello");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           string discount=txtOpeningBalance.Text;
            string discoun_type = discount_amount.IsChecked ==true? "amount" : "percentage";

            MessageBox.Show("discount is", discount);
            MessageBox.Show("discount is", discoun_type);
        }
    }
}
