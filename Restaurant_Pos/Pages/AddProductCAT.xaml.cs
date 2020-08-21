using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using Restaurant_Pos.Mail;
using Restaurant_Pos.Pages.Session;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Restaurant_Pos.Pages
{
    /// <summary>
    /// Interaction logic for AddProductCAT.xaml
    /// </summary>
    public partial class AddProductCAT : Page
    {

        #region 
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());

       public string connstring = PostgreSQL.ConnectionString;
        public int Warehouse_selection { get; set; }

        public int WarehouseId_Selected { get; set; }
        public string Sys_config_pricelistId { get; set; }

        public string Sys_config_costElementId { get; set; }
        public string Sys_config_businessPartnerId { get; set; }

        public int AD_Client_ID { get; set; }
        public int AD_ORG_ID { get; set; }
       public int AD_USER_ID { get; set; }
        public int AD_ROLE_ID { get; set; }
        public int Sequenc_id { get; set; }
        string imagePath = "";

        #endregion

        #region Public Function
        public AddProductCAT()
        {
            InitializeComponent();
        }
        public void ClearData()
        {
            txtCategory.Text = "";
            txtName.Text = "";
            txtSearchKey.Text = "";
            product_image.Source = null;
        }

        #endregion

        #region Event

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtSearchKey.Text == String.Empty)
                {
                    txtSearchKey.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The SearchKey ! ");
                    return;
                }
                if (txtName.Text == String.Empty)
                {
                    txtName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Name ! ");
                    return;
                }
                
                if (txtCategory.Text == String.Empty)
                {
                    txtCategory.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Category! ");
                    return;
                }
                if (imagePath == "")
                {
                    btnImgUpload.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Add The Image! ");
                    return;
                }
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
               string DigitalMenu = "";
                string SelfService = "";
                if (CBL_DigitalMenu.IsChecked==true)
                {
                    DigitalMenu = "Y";
                }
                else
                {
                    DigitalMenu = "N";
                }
                 if(Self_Service.IsChecked == true)
                 {
                    SelfService = "Y";
                }
                else
                {
                    SelfService = "N";
                }
                connection.Open();
                NpgsqlCommand cmd_select_sequenc_no_ad_user_pos = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name='m_product_category';", connection);
                NpgsqlDataReader _get__Ad_sequenc_no_ad_user_pos = cmd_select_sequenc_no_ad_user_pos.ExecuteReader();
                if (_get__Ad_sequenc_no_ad_user_pos.Read())
                {
                    Sequenc_id = _get__Ad_sequenc_no_ad_user_pos.GetInt32(4) + 1;
                }
                connection.Close();

                connection.Open();
                NpgsqlCommand INSERT_cmd_productCAT_Detail = new NpgsqlCommand("insert into m_product_category(id,ad_client_id,ad_org_id,Createdby,updatedby,isactive, m_product_category_id,Name,searchkey,categoryColour,image,SelfService,DigitalMenu)    " + " " + "" + "VALUES(" + Sequenc_id + "," + _clientId + " ," + _OrgId + " ," + _UserID + "," + _UserID + ",'Y'," + Sequenc_id + ",'" + txtName.Text + "','" + txtSearchKey.Text + "','" + txtCategory.Text + "','" + imagePath + "','"+ SelfService + "','"+ DigitalMenu + "') ;", connection);
                INSERT_cmd_productCAT_Detail.ExecuteNonQuery();
                MessageBox.Show("Record Save Successfully");
                ClearData();
                connection.Close();
                connection.Open();
                NpgsqlCommand cmd_update_sequenc_no_ad_user_pos = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name='m_product_category';", connection);
                cmd_update_sequenc_no_ad_user_pos.ExecuteReader();
                connection.Close();
                
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Insert Product Category Detail  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Insert Product Category Detail", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void BtnImgUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                string fileName = openFileDialog.SafeFileName;

                string destinationPath = "C:/Users/Admin/Desktop/POS/Restaurant/Restaurant_Pos/Resources/Images/ProductCAT";
                string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                DirectoryInfo diSource = new DirectoryInfo(directoryPath);
                DirectoryInfo diTarget = new DirectoryInfo(destinationPath);
                Copy(diSource, diTarget, fileName.ToString());

                product_image.Source = new BitmapImage(fileUri);
                imagePath = fileName;
            }
        }

        public static void Copy(DirectoryInfo source, DirectoryInfo target, string fileName)
        {

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            try
            {
                foreach (FileInfo fi in source.GetFiles())
                {
                    if (fi.Name == fileName)
                        fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), false);
                }

            }
            catch
            {

            }


        }
        #endregion
    }
}
