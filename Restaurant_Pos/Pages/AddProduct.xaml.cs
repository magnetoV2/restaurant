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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        #region 
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());
        List<M_UOM> m_UOM_items = new List<M_UOM>();
        List<M_ItemCategory> m_ItemCategory_items = new List<M_ItemCategory>();

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
        public AddProduct()
        {
            InitializeComponent();
            BindUOM();
            BindItemCategory();
        }
        public void ClearData()
        {
            txtBarCode.Text = "";
            txtName.Text = "";
            txtMaximum.Text = "";
            txtMinimum.Text = "";
            txtPurchasePrice.Text = "";
            txtSalePrice.Text = "";
            product_image.Source = null;

        }
        #endregion

        #region Bind Data Function
        public void BindItemCategory()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand cmd_ItemCategory_GetData = new NpgsqlCommand("select m_product_category_id,name from  m_product_category  WHERE ad_org_id =" + _OrgId + " AND ad_client_id =" + _clientId + " ;", connection);//
                NpgsqlDataReader _ItemCategory_GetData = cmd_ItemCategory_GetData.ExecuteReader();

                while (_ItemCategory_GetData.Read())
                {
                    m_ItemCategory_items.Add(new M_ItemCategory()
                    {
                        ItemId = _ItemCategory_GetData.GetInt32(0),
                        Name = _ItemCategory_GetData.GetString(1)
                    });
                }
                connection.Close();

                if (m_ItemCategory_items.Count == 0)
                {
                    MessageBox.Show("Please Add UOM!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        ItemCategory_DropDown.ItemsSource = m_ItemCategory_items;
                    });
                }
                
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Item Category POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Item Category Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindUOM()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand cmd_UOM_GetData = new NpgsqlCommand("select c_uom_id,name  from  m_product_uom  WHERE ad_org_id =" + _OrgId + " AND ad_client_id =" + _clientId + " ;", connection);//
                NpgsqlDataReader _UOM_GetData = cmd_UOM_GetData.ExecuteReader();

                while (_UOM_GetData.Read())
                {
                    m_UOM_items.Add(new M_UOM()
                    {
                        UOMId = _UOM_GetData.GetInt32(0),
                        Name = _UOM_GetData.GetString(1)
                    });
                }
                connection.Close();

                if (m_UOM_items.Count == 0)
                {
                    MessageBox.Show("Please Add UOM!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        UOM_DropDown.ItemsSource = m_UOM_items;
                    });
                }
              
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In UOM POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In UOM Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }

        #endregion

        #region Event
        private void Btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtBarCode.Text == String.Empty)
                {
                    txtBarCode.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The BarCode ! ");
                    return;
                }
                if (txtName.Text == String.Empty)
                {
                    txtName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Name ! ");
                    return;
                }
                if (txtMaximum.Text == String.Empty)
                {
                    txtMaximum.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Maximum Qty! ");
                    return;
                }
                if (txtMinimum.Text == String.Empty)
                {
                    txtMinimum.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Minimum Qty! ");
                    return;
                }
                if (txtPurchasePrice.Text == String.Empty)
                {
                    txtPurchasePrice.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Purchase Price ! ");
                    return;
                }
                if (txtSalePrice.Text == String.Empty)
                {
                    txtSalePrice.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Sale Price ! ");
                    return;
                }
                if (imagePath == "")
                {
                    btnupload.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Add the image ! ");
                    return;
                }

                dynamic UC = UOM_DropDown.SelectedItem as dynamic;
                int UOMId= UC.UOMId;
               string UOMName = UC.Name;
                dynamic IC = ItemCategory_DropDown.SelectedItem as dynamic;
                int ItemId = IC.ItemId;
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                string PriceEditable = "";
                string SellOnline = "";
                if (cblPriceEditable.IsChecked == true)
                {
                    PriceEditable = "Y";
                }
                else
                {
                    PriceEditable = "N";

                }
                 if (cblSellOnline.IsChecked == true)
                {
                    SellOnline = "Y";
                }
                 else
                {
                    SellOnline = "N";
                }
                connection.Open();
                NpgsqlCommand cmd_select_sequenc_no_ad_user_pos = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name='m_product';", connection);
                NpgsqlDataReader _get__Ad_sequenc_no_ad_user_pos = cmd_select_sequenc_no_ad_user_pos.ExecuteReader();
                if (_get__Ad_sequenc_no_ad_user_pos.Read())
                {
                    Sequenc_id = _get__Ad_sequenc_no_ad_user_pos.GetInt32(4) + 1;
                }
                connection.Close();

                connection.Open();
                NpgsqlCommand INSERT_cmd_Product_Detail = new NpgsqlCommand("insert into m_product(id,ad_client_id,ad_org_id,m_product_id,Createdby,updatedby,isactive, m_product_category_id, Name, barcode, uomid, uomname, image, sellonline, purchaseprice, Currentcostprice, max_qty, min_qty,PriceEditable)    " + " " + "" + "" +
                                                                                              "VALUES(" + Sequenc_id + "," + _clientId + " ," + _OrgId + " ,"+Sequenc_id+"," + _UserID + "," + _UserID + ",'Y','"+ ItemId + "','" + txtName.Text + "','" + txtBarCode.Text + "','" + UOMId + "','" + UOMName + "','" + imagePath+"','" + SellOnline + "','" + txtPurchasePrice.Text + "','" + txtSalePrice.Text + "','"+txtMaximum.Text+"','"+txtMinimum.Text+"','"+ PriceEditable + "') ;", connection);
                INSERT_cmd_Product_Detail.ExecuteNonQuery();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: ad_client_id  new data  Inserted"; });
               
                connection.Close();
                connection.Open();
                NpgsqlCommand cmd_update_sequenc_no_ad_user_pos = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name='m_product';", connection);
                cmd_update_sequenc_no_ad_user_pos.ExecuteReader();
                connection.Close();
                ClearData();
                MessageBox.Show("Record Edit Successfully");
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

        private void Btnupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                string fileName = openFileDialog.SafeFileName;

                string destinationPath = "/Resources/Images/ProductCAT";
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

        #region validation
        private void TxtBarCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBarCode.Text = Regex.Replace(txtBarCode.Text, "[^0-9]+", "");
        }

        private void TxtPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPurchasePrice.Text = Regex.Replace(txtPurchasePrice.Text, "[^0-9]+", "");
        }

        private void TxtMinimum_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtMinimum.Text = Regex.Replace(txtMinimum.Text, "[^0-9]+", "");
            
        }

        private void TxtMaximum_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtMaximum.Text = Regex.Replace(txtMaximum.Text, "[^0-9]+", "");
            
        }

        private void TxtSalePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSalePrice.Text = Regex.Replace(txtSalePrice.Text, "[^0-9]+", "");
            
        }
        #endregion
    }
}
