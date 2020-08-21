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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using System.Data;

namespace Restaurant_Pos.Pages
{
    /// <summary>
    /// Interaction logic for ProductCAT.xaml
    /// </summary>
    public partial class ViewProductCAT : Page
    {
        #region 
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public List<m_ProductCATLst> _m_ProductCATLst = new List<m_ProductCATLst>();
        public List<m_ProductCATWise> _m_ProductCATWise = new List<m_ProductCATWise>();

        public string connstring = PostgreSQL.ConnectionString;
        #endregion

        #region Public Function
        public ViewProductCAT()
        {
            InitializeComponent();
            BindProductCATLst();
            EnableData();
        }
        public void EnableData()
        {
            txtName.IsEnabled = false;
            txtSearchKey.IsEnabled = false;
            txtCategory.IsEnabled = false;
            CBL_DigitalMenu.IsEnabled = false;
            Self_Service.IsEnabled = false;
            btn_edit.IsEnabled = false;
            bdCancel.Visibility = Visibility.Hidden;
            btn_Cancel.Visibility= Visibility.Hidden;
            bdSave.Visibility = Visibility.Hidden;
            btn_Save.Visibility = Visibility.Hidden;

}
        #endregion

        #region Bind Data Function
        public void BindProductCATLst()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductCATLst_GetData = new NpgsqlCommand("select m_product_category_id,Name from m_product_category  where ad_org_id=" + _OrgId + " ;", connection);//
                NpgsqlDataReader _ProductCATLst_GetData = cmd_ProductCATLst_GetData.ExecuteReader();
                _m_ProductCATLst.Clear();
                _m_ProductCATLst.Add(new m_ProductCATLst()
                {
                    id = 0,
                    Name = "All"


                });
                while (_ProductCATLst_GetData.Read())
                {

                    _m_ProductCATLst.Add(new m_ProductCATLst()
                    {
                        id = _ProductCATLst_GetData.GetInt32(0),
                        Name = _ProductCATLst_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (_m_ProductCATLst.Count == 0)
                {
                    MessageBox.Show("Please Add Product Category!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ProductCAT_DropDown.ItemsSource = _m_ProductCATLst;

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

        public void BindProductCATList()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductCATList_GetData = new NpgsqlCommand("select m_product_category_id,Name from m_product_category  where ad_org_id=" + _OrgId + " ;", connection);//
                NpgsqlDataReader _ProductCATList_GetData = cmd_ProductCATList_GetData.ExecuteReader();
                _m_ProductCATWise.Clear();
                ProductCD_ListBox.ItemsSource = "";
                while (_ProductCATList_GetData.Read())
                {

                    _m_ProductCATWise.Add(new m_ProductCATWise()
                    {
                        id = _ProductCATList_GetData.GetInt32(0),
                        Name = _ProductCATList_GetData.GetString(1)
                      

                    });
                }

                connection.Close();

                if (_m_ProductCATWise.Count == 0)
                {
                    MessageBox.Show("Please Add Product Category!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        ProductCD_ListBox.ItemsSource = _m_ProductCATWise;
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

        public void BindProductCATWise(int id)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductCATWise_GetData = new NpgsqlCommand("select m_product_category_id,Name from m_product_category  where ad_org_id=" + _OrgId + " And m_product_category_id='" + id+"' ;", connection);//
                NpgsqlDataReader _ProductCATWise_GetData = cmd_ProductCATWise_GetData.ExecuteReader();
                _m_ProductCATWise.Clear();
                ProductCD_ListBox.ItemsSource = "";
                while (_ProductCATWise_GetData.Read())
                {

                    _m_ProductCATWise.Add(new m_ProductCATWise()
                    {
                        id = _ProductCATWise_GetData.GetInt32(0),
                        Name = _ProductCATWise_GetData.GetString(1)


                    });
                }

                connection.Close();
                if (_m_ProductCATWise.Count == 0)
                {
                    MessageBox.Show("Please Add Product Category!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        ProductCD_ListBox.ItemsSource = _m_ProductCATWise;
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

        public void BindProductCAT(string Search_Key)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_ProductCAT_GetData = new NpgsqlCommand("select m_product_category_id,Name from m_product_category  where ad_org_id=" + _OrgId + " And Name like '%" + Search_Key + "%' ;", connection);//
                NpgsqlDataReader _ProductCAT_GetData = cmd_ProductCAT_GetData.ExecuteReader();
                _m_ProductCATWise.Clear();
                ProductCD_ListBox.ItemsSource = "";
                while (_ProductCAT_GetData.Read())
                {

                    _m_ProductCATWise.Add(new m_ProductCATWise()
                    {
                        id = _ProductCAT_GetData.GetInt32(0),
                        Name = _ProductCAT_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (_m_ProductCATWise.Count == 0)
                {
                    MessageBox.Show("Please Add Product Category!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ProductCD_ListBox.ItemsSource = _m_ProductCATWise;
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
        #endregion

        #region Event
        private void Product_Search_KeyUp(object sender, KeyEventArgs e)
        {
          string Search_Key=   product_Search.Text;
            BindProductCAT(Search_Key);
        }

        private void ProductCD_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic PC = ProductCD_ListBox.SelectedItem as dynamic;
            int id = PC.id;
            string Name = PC.Name;
            btn_edit.IsEnabled = true;

            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            // connection.Close();
            connection.Open();
            //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
            NpgsqlCommand cmd_ProductCAT_GetData = new NpgsqlCommand("select m_product_category_id,Name,searchkey,categoryColour,DigitalMenu,selfService from m_product_category  where m_product_category_id=" + id + " and ad_org_id=" + _OrgId + " and Name='" + Name + "' ;", connection);//
            NpgsqlDataReader _ProductCAT_GetData = cmd_ProductCAT_GetData.ExecuteReader();
            _ProductCAT_GetData.Read();
            txtProductCATID.Text= _ProductCAT_GetData.GetString(0);
            txtName.Text= _ProductCAT_GetData.GetString(1);
            txtSearchKey.Text = _ProductCAT_GetData.GetString(2);
            txtCategory.Text= _ProductCAT_GetData.GetString(3);
            if (_ProductCAT_GetData.GetString(4) !=null)
            {
                if (_ProductCAT_GetData.GetString(4) =="Y")
                {
                    CBL_DigitalMenu.IsChecked = true;
                }
            }
            if (_ProductCAT_GetData.GetString(5) != null)
            {
                if (_ProductCAT_GetData.GetString(5) == "Y")
                {
                    Self_Service.IsChecked = true;
                }
            }
            connection.Close();
           
        }

        private void ProductCAT_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic PC = ProductCAT_DropDown.SelectedItem as dynamic;
            int id = PC.id;
            if (ProductCAT_DropDown.SelectedIndex<=0)
            {
                BindProductCATList();
            }
            else
            {
                BindProductCATWise(id);
            }
        }

        private void Btn_edit_Click(object sender, RoutedEventArgs e)
        {
            txtName.IsEnabled = true;
            txtSearchKey.IsEnabled = true;
            txtCategory.IsEnabled = true;
            CBL_DigitalMenu.IsEnabled = true;
            Self_Service.IsEnabled = true;
            bdCancel.Visibility = Visibility.Visible;
            btn_Cancel.Visibility = Visibility.Visible;
            bdSave.Visibility = Visibility.Visible;
            btn_Save.Visibility = Visibility.Visible;
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewProductCAT());
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
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
                string DigitalMenu = "";
                string SelfService = "";
                if (CBL_DigitalMenu.IsChecked == true)
                {
                    DigitalMenu = "Y";
                }
                else
                {
                    DigitalMenu = "N";
                }
                if (Self_Service.IsChecked == true)
                {
                    SelfService = "Y";
                }
                else
                {
                    SelfService = "N";
                }
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand Edit_ProductCAT_List = new NpgsqlCommand("Update m_product_category set Name='" + txtName.Text + "', searchkey='" + txtSearchKey.Text + "', categoryColour='" + txtCategory.Text + "',updatedby=" + _UserID + " ,DigitalMenu='"+ DigitalMenu + "',selfService='" + SelfService + "'  where m_product_category_id='" + txtProductCATID.Text + "' and ad_org_id=" + _OrgId + ";", connection);//
                Edit_ProductCAT_List.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Record Edit Successfully");
                NavigationService.Navigate(new ViewProductCAT());
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Update Product Category  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Update Product Category ", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Btn_Add_Category_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductCAT());
        }
        #endregion
    }
}
