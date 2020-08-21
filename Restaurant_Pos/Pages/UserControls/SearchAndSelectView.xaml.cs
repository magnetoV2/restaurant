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

namespace Restaurant_Pos.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for SearchAndSelectView.xaml
    /// </summary>
    public partial class SearchAndSelectView : UserControl
    {
        public SearchAndSelectView()
        {
            InitializeComponent();
            this.DataContext = new SearchAndSelectViewModel();
        }
    }
}
