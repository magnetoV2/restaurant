using System.Windows;
using System.Windows.Controls;

namespace Restaurant_Pos.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for ErrorPopUp.xaml
    /// </summary>
    public partial class ErrorPopUp : UserControl
    {
        public ErrorPopUp()
        {
            InitializeComponent();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Login;
        }
    }
}