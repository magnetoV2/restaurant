using log4net;
using System.Windows;

namespace Restaurant_Pos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            //log4net.Config.XmlConfigurator.Configure();
            DataContext = new WindowViewModel(this);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            // Application activated
            this.Focusable = true;
            this.Focus();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            // Application deactivated

        }

      

    }
}
