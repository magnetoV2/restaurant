using System.Windows;
using System.Windows.Controls;

namespace Restaurant_Pos.Pages
{
    /// <summary>
    /// Interaction logic for LoginSetting.xaml
    /// </summary>
    public partial class LoginSetting : UserControl
    {
        private string servername;
        private string serverport;
        private string api_url;
        private string server_local_name;
        private string server_local_port;
        private string server_local_userid;
        private string server_local_password;
        private string server_local_dbname;
        private string display_language;
        private string on_printer;

        public LoginSetting()
        {
            InitializeComponent();
            var setting = PostgreSQL.Get_App_setting();
            servername = setting.servername;
            serverport = setting.serverport;
            api_url = setting.api_url;
            server_local_name = setting.server_local_name;
            server_local_port = setting.server_local_port;
            server_local_userid = setting.server_local_userid;
            server_local_password = setting.server_local_password;
            server_local_dbname = setting.server_local_dbname;
            display_language = setting.display_language;
            on_printer = setting.on_printer;

            server_name.Text = servername;
            server_port.Text = serverport;
        }

        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CloseSetting_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Grid).Children.Remove(this);
        }

        private void Server_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            servername = server_name.Text;
        }

        private void Server_port_TextChanged(object sender, TextChangedEventArgs e)
        {
            serverport = server_port.Text;
        }
    }
}