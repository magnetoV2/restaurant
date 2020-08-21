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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class ViewProduct : Page
    {

        #region 
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());
        
        List<M_UOM> m_UOM_items = new List<M_UOM>();
        List<M_ItemCategory> m_ItemCategory_items = new List<M_ItemCategory>();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public List<m_Product> _Product_items = new List<m_Product>();

        public List<m_ProductList> m_ProductList_items = new List<m_ProductList>();
        public List<m_ProductWise> m_ProductWise_items = new List<m_ProductWise>();

        public string connstring = PostgreSQL.ConnectionString;
        #endregion

        #region Public Function
        public ViewProduct()
        {
            InitializeComponent();
            BindProduct();
            Enabled();
            BindUOM();
            BindItemCategory();
        }
        public void Enabled()
        {
            txtbarcode.IsEnabled = false;
            txtMaximum.IsEnabled = false;
            txtMinimum.IsEnabled = false;
            txtName.IsEnabled = false;
            txtPurchasePrice.IsEnabled = false;
            txtSalePrice.IsEnabled = false;
            UOM_DropDown.IsEnabled = false;
            ItemCategory_DropDown.IsEnabled = false;
            cblPriceEditable.IsEnabled = false;
            cblSellOnline.IsEnabled = false;
            btn_edit.IsEnabled = false;
            btn_Cancel.Visibility = Visibility.Hidden;
            btn_Save.Visibility = Visibility.Hidden;
            bdCancel.Visibility = Visibility.Hidden;
            bdSave.Visibility = Visibility.Hidden;

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
                    MessageBox.Show("Please Add Item Category!");

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
        public void BindProduct()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductList_GetData = new NpgsqlCommand("select m_product_id,Name from m_product  where ad_org_id=" + _OrgId + " ;", connection);//
                NpgsqlDataReader _ProductList_GetData = cmd_ProductList_GetData.ExecuteReader();
                m_ProductList_items.Add(new m_ProductList()
                {
                    id =0,
                    Name = "All"


                });
                while (_ProductList_GetData.Read())
                {

                    m_ProductList_items.Add(new m_ProductList()
                    {
                        id = _ProductList_GetData.GetInt32(0),
                        Name = _ProductList_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (m_ProductList_items.Count == 0)
                {
                    MessageBox.Show("Please Add Product !");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Product_DropDown.ItemsSource = m_ProductList_items;
                    });
                }
               
             
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product  Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindProductList()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductList_GetData = new NpgsqlCommand("select m_product_id,Name from m_product  where ad_org_id=" + _OrgId + " ;", connection);//
                NpgsqlDataReader _ProductList_GetData = cmd_ProductList_GetData.ExecuteReader();
                m_ProductWise_items.Clear();
                Product_ListBox.ItemsSource = "";
                while (_ProductList_GetData.Read())
                {

                    m_ProductWise_items.Add(new m_ProductWise()
                    {
                        id = _ProductList_GetData.GetInt32(0),
                        Name = _ProductList_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (m_ProductWise_items.Count == 0)
                {
                    MessageBox.Show("Please Add Product !");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Product_ListBox.ItemsSource = m_ProductWise_items;
                    });
                }
               
               
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product  Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindProductWise(int id)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductWise_GetData = new NpgsqlCommand("select m_product_id,Name from m_product  where ad_org_id=" + _OrgId + " And m_product_id='" + id + "' ;", connection);//
                NpgsqlDataReader _ProductWise_GetData = cmd_ProductWise_GetData.ExecuteReader();
                
                m_ProductWise_items.Clear();
                Product_ListBox.ItemsSource = "";
                while (_ProductWise_GetData.Read())
                {

                    m_ProductWise_items.Add(new m_ProductWise()
                    {
                        id = _ProductWise_GetData.GetInt32(0),
                        Name = _ProductWise_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (m_ProductWise_items.Count == 0)
                {
                    MessageBox.Show("Please Add Product Category!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Product_ListBox.ItemsSource = m_ProductWise_items;
                    });
                }
               
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product Category  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product Category Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindProductLst(string Search_Key)
        {
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductList_GetData = new NpgsqlCommand("select m_product_id,Name from m_product  where ad_org_id=" + _OrgId + " and Name Like '%" + Search_Key + "%' ;", connection);//
                NpgsqlDataReader _ProductList_GetData = cmd_ProductList_GetData.ExecuteReader();
                m_ProductWise_items.Clear();
                m_ProductList_items.Clear();
                Product_ListBox.ItemsSource = "";
                while (_ProductList_GetData.Read())
                {

                    m_ProductList_items.Add(new m_ProductList()
                    {
                        id = _ProductList_GetData.GetInt32(0),
                        Name = _ProductList_GetData.GetString(1)


                    });
                }

                connection.Close();
                if (m_ProductList_items.Count == 0)
                {
                    MessageBox.Show("ProductList Is Not Found! ");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Product_ListBox.ItemsSource = m_ProductList_items;
                    });
                }

              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In ProductList Is Not Found!   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In ProductList Is Not Found!", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        #endregion

        #region Event
        private void Product_Search_KeyUp(object sender, KeyEventArgs e)
        {
         string Search_Key=  product_Search.Text;
            BindProductLst(Search_Key);

        }

        private void Product_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            dynamic PC = Product_DropDown.SelectedItem as dynamic;
            int id = PC.id;
            if (Product_DropDown.SelectedIndex <= 0)
            {
                BindProductList();
            }
            else
            {
                BindProductWise(id);
            }
        }

        private void Btn_edit_Click(object sender, RoutedEventArgs e)
        {
            txtbarcode.IsEnabled = true;
            txtMaximum.IsEnabled = true;
            txtMinimum.IsEnabled = true;
            txtName.IsEnabled = true;
            txtPurchasePrice.IsEnabled = true;
            txtSalePrice.IsEnabled = true;
            UOM_DropDown.IsEnabled = true;
            ItemCategory_DropDown.IsEnabled = true;
            cblPriceEditable.IsEnabled = true;
            cblSellOnline.IsEnabled = true;
            btn_Cancel.Visibility = Visibility.Visible;
            btn_Save.Visibility = Visibility.Visible;
            bdCancel.Visibility = Visibility.Visible;
            bdSave.Visibility = Visibility.Visible;
        }

        private void Btn_Add_Category_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProduct());
           
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewProduct());
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtbarcode.Text == String.Empty)
                {
                    txtbarcode.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
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
               

                dynamic UD = UOM_DropDown.SelectedItem as dynamic;
                int UOMId = UD.UOMId;
                string Name = UD.Name;
                dynamic RD = Product_DropDown.SelectedItem as dynamic;
                int ItemId = RD.id;
                dynamic PL = Product_ListBox.SelectedItem as dynamic;
                int id = PL.id;
                string PriceEditable = "";
                string SellOnline = "";
                if (cblPriceEditable.IsChecked==true)
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
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand Edit_Product_List = new NpgsqlCommand("update m_product set m_product_category_id='"+ ItemId + "', Name='"+txtName.Text+"', barcode='"+txtbarcode.Text+"', uomid='"+ UOMId + "', uomname='"+ Name + "', sellonline='"+ SellOnline + "',PriceEditable='"+ PriceEditable + "', purchaseprice='"+txtPurchasePrice.Text+"', Currentcostprice='"+ txtSalePrice.Text + "', max_qty='"+txtMaximum.Text+"', min_qty='"+txtMinimum.Text+ "'   where m_product_id='" + id + "' and ad_org_id=" + _OrgId + ";", connection);//
                Edit_Product_List.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Record Edit Successfully");
                NavigationService.Navigate(new ViewProduct());
               
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Update User Detail  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Update User Detail ", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        
        private void Product_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                dynamic PL = Product_ListBox.SelectedItem as dynamic;
                int id = PL.id;
                string Name = PL.Name;
                btn_edit.IsEnabled = true;

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand cmd_Userlist_GetData = new NpgsqlCommand("select barcode,Name,m_product_category_id,uomname,purchaseprice, Currentcostprice, max_qty, min_qty,Sellonline,PriceEditable from m_product   where m_product_id=" + id + " and ad_org_id=" + _OrgId + " and Name='" + Name + "' ;", connection);//
                NpgsqlDataReader _UserList_GetData = cmd_Userlist_GetData.ExecuteReader();
                _UserList_GetData.Read();
                txtbarcode.Text = _UserList_GetData.GetString(0);
                txtName.Text = _UserList_GetData.GetString(1);
                Product_DropDown.SelectedItem = _UserList_GetData.GetString(2);
                UOM_DropDown.SelectedItem = _UserList_GetData.GetString(3);
                txtPurchasePrice.Text = _UserList_GetData.GetString(4);
                txtSalePrice.Text = _UserList_GetData.GetString(5);
                txtMaximum.Text = _UserList_GetData.GetString(6);
                txtMinimum.Text = _UserList_GetData.GetString(7);

                if(_UserList_GetData.GetString(8)!=null)
                {
                    if (_UserList_GetData.GetString(8) == "Y")
                    {
                        cblSellOnline.IsChecked = true;
                    }
                  
                }
                if (_UserList_GetData.GetString(9) != null)
                {
                    if (_UserList_GetData.GetString(9) == "Y")
                    {
                        cblPriceEditable.IsChecked = true;
                    }

                }
                txtProductID.Text = id.ToString();
                connection.Close();



            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product  Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
       #endregion

        #region validation

        private void Txtbarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtbarcode.Text = Regex.Replace(txtbarcode.Text, "[^0-9]+", "");
        }

        private void TxtPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPurchasePrice.Text = Regex.Replace(txtPurchasePrice.Text, "[^0-9]+", "");
        }

        private void TxtSalePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSalePrice.Text = Regex.Replace(txtSalePrice.Text, "[^0-9]+", "");
        }

        private void TxtMinimum_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtMinimum.Text = Regex.Replace(txtMinimum.Text, "[^0-9]+", "");
        }

        private void TxtMaximum_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtMaximum.Text = Regex.Replace(txtMaximum.Text, "[^0-9]+", "");
        }
        #endregion

        private void ItemCategory_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
