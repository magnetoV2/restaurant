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
    /// Interaction logic for ViewUser.xaml
    /// </summary>
    public partial class ViewUser : Page
    {
        #region 
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<ImgPath> _Img = new List<ImgPath>();
        public List<M_warehouse> m_warehouse_items = new List<M_warehouse>();
        public List<M_user> m_user_items = new List<M_user>();
        public List<M_Roles> m_Roles_items = new List<M_Roles>();
        public string connstring = PostgreSQL.ConnectionString;

        #endregion

        #region Public Function
        public ViewUser()
        {
            InitializeComponent();
             
            //BindWareHouse();
            TextEnabled();
            BindUser();
            BindWareHouse();
            BindRole();
        }
        public void TextEnabled()
        {
            txtQID.IsEnabled = false;
            txtName.IsEnabled = false;
            txtUserName.IsEnabled = false;
            txtPassword.IsEnabled = false;
            txtMobile.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtAddress.IsEnabled = false;
            Warehouse_DropDown.IsEditable = false;
            Role_DropDown.IsEditable = false;
            btn_edit.IsEnabled = false;
            btnCancel.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            btnCancelBorder.Visibility = Visibility.Hidden;
            btnSaveBorder.Visibility = Visibility.Hidden;

        }
        #endregion

        #region Bind Data Function
        public void BindRole()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand cmd_Roles_GetData = new NpgsqlCommand("SELECT Name,ad_role_id FROM ad_role  WHERE  ad_client_id = " + _clientId + "; ", connection);//
                NpgsqlDataReader _Roles_GetData = cmd_Roles_GetData.ExecuteReader();

                while (_Roles_GetData.Read())
                {
                    m_Roles_items.Add(new M_Roles()
                    {
                        Name = _Roles_GetData.GetString(0),
                        RoleID = _Roles_GetData.GetInt32(1)
                    });
                }
                connection.Close();

                if (m_Roles_items.Count == 0)
                {
                    MessageBox.Show("Please Add Roles!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Role_DropDown.ItemsSource = m_Roles_items;
                    });
                }
             
               
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Roles  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Roles Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindWareHouse()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand cmd_warehouseDetails_GetData = new NpgsqlCommand("SELECT  name,m_warehouse_id FROM m_warehouse  WHERE ad_org_id =" + _OrgId + " AND ad_client_id =" + _clientId + " order by id;", connection);//
                NpgsqlDataReader _warehouseDetails_GetData = cmd_warehouseDetails_GetData.ExecuteReader();

                while (_warehouseDetails_GetData.Read())
                {
                    m_warehouse_items.Add(new M_warehouse()
                    {
                        WarehouseName = _warehouseDetails_GetData.GetString(0),
                        WarehouseId= _warehouseDetails_GetData.GetInt32(1)
                    });
                }
                connection.Close();

                if (m_warehouse_items.Count == 0)
                {
                    MessageBox.Show("Please Add Warehouse!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Warehouse_DropDown.ItemsSource = m_warehouse_items;
                    });
                }
               
             
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Login POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Warehouse Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindUser()
        {
            try
                {

                    NpgsqlConnection connection = new NpgsqlConnection(connstring);
                    // connection.Close();
                    connection.Open();
                    //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                    NpgsqlCommand cmd_User_GetData = new NpgsqlCommand("select ad_user_id,Name from ad_user_pos  WHERE ad_org_id =" + _OrgId + "  ;", connection);//
                    NpgsqlDataReader _cmd_User_GetData = cmd_User_GetData.ExecuteReader();
                m_user_items.Add(new M_user()
                {
                    UserID = 0,
                    Name = "All"
                });
                while (_cmd_User_GetData.Read())
                    {
                        m_user_items.Add(new M_user()
                        {
                            UserID = _cmd_User_GetData.GetInt32(0),
                            Name = _cmd_User_GetData.GetString(1)
                        });
                    }
                    connection.Close();

                if (m_user_items.Count == 0)
                {
                    MessageBox.Show("Please Add User!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        User_DropDown.ItemsSource = m_user_items;
                    });

                }
            }
               
                catch (Exception ex)
                {

                    log.Error(" ===================  Error In User =========================== ");
                    log.Error(DateTime.Now.ToString());
                    log.Error(ex.ToString());
                    log.Error(" ===================  End of Error  =========================== ");
                    if (MessageBox.Show(ex.ToString(),
                            "Error In User Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        Environment.Exit(0);
                    }
                    Environment.Exit(0);
                }




            
        }
        public void BindUserList()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_User_GetData = new NpgsqlCommand("select ad_user_id,name,ImgPath from ad_user_pos  where ad_org_id="+_OrgId+ " ;", connection);//
                NpgsqlDataReader _User_GetData = cmd_User_GetData.ExecuteReader();
                _Img.Clear();
                User_ListBox.ItemsSource = "";
                while (_User_GetData.Read())
                {
                   
                    _Img.Add(new ImgPath()
                    {
                        id = _User_GetData.GetString(0),
                        Name = _User_GetData.GetString(1),
                        Path = "/Restaurant_Pos;component/Resources/Images/UsersImg/" + _User_GetData.GetString(2)

                    });
                }

                connection.Close();

                if (_Img.Count == 0)
                {
                    MessageBox.Show("Please Add User name And Path!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        User_ListBox.ItemsSource = _Img;
                    });
                }
              
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Login POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Users Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindUserWise(int ID)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_User_GetData = new NpgsqlCommand("select ad_user_id,name,ImgPath from ad_user_pos  where ad_org_id=" + _OrgId + " And ad_user_id="+ID+" ;", connection);//
                NpgsqlDataReader _User_GetData = cmd_User_GetData.ExecuteReader();
                _Img.Clear();
                User_ListBox.ItemsSource = "";
                while (_User_GetData.Read())
                {
                    _Img.Add(new ImgPath()
                    {
                        id = _User_GetData.GetString(0),
                        Name = _User_GetData.GetString(1),
                        Path = "/Restaurant_Pos;component/Resources/Images/UsersImg/" + _User_GetData.GetString(2)

                    });
                }

                connection.Close();

                if (_Img.Count == 0)
                {
                    MessageBox.Show("Please Add User name And Path!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        User_ListBox.ItemsSource = _Img;
                    });
                }
              
             
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Login POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Users Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void UserSearch(string Key)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_User_GetData = new NpgsqlCommand("select ad_user_id,name,ImgPath from ad_user_pos  where ad_org_id=" + _OrgId + " And Name LIKE  '" + Key  + "%' ;", connection);//
                NpgsqlDataReader _User_GetData = cmd_User_GetData.ExecuteReader();
                _Img.Clear();
                User_ListBox.ItemsSource = "";
                while (_User_GetData.Read())
                {
                    _Img.Add(new ImgPath()
                    {
                        id = _User_GetData.GetString(0),
                        Name = _User_GetData.GetString(1),
                        Path = "/Restaurant_Pos;component/Resources/Images/UsersImg/" + _User_GetData.GetString(2)

                    });
                }

                connection.Close();

                if (_Img.Count == 0)
                {
                    MessageBox.Show("Please Add Is Not Found!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        User_ListBox.ItemsSource = _Img;
                    });
                }
                
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In User Is Not Found!  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Users Is Not Found!", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        #endregion

        #region Event
        private void User_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    dynamic UL = User_ListBox.SelectedItem as dynamic;
                    string id = UL.id;
                    string Name = UL.Name;
                    btn_edit.IsEnabled = true;

                   NpgsqlConnection connection = new NpgsqlConnection(connstring);
                    // connection.Close();
                    connection.Open();
                    //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                    NpgsqlCommand cmd_Userlist_GetData = new NpgsqlCommand("select aup.QID,aup.name,aup.username,aup.password,aup.mobile,aup.email,aup.address,aup.m_warehouse_id,ar.Name from ad_user_pos aup inner join ad_role ar on ar.ad_role_id=aup.ad_role_id  where aup.ad_user_id=" + id + " and aup.ad_org_id=" + _OrgId+ " and aup.Name='" + Name + "' ;", connection);//
                    NpgsqlDataReader _UserList_GetData = cmd_Userlist_GetData.ExecuteReader();
                    _UserList_GetData.Read();

                    

                    txtQID.Text= _UserList_GetData.GetString(0);
                    txtName.Text= _UserList_GetData.GetString(1);
                    txtUserName.Text= _UserList_GetData.GetString(2);
                    txtPassword.Text= _UserList_GetData.GetString(3);
                    txtMobile.Text= _UserList_GetData.GetString(4);
                    txtEmail.Text= _UserList_GetData.GetString(5);
                    txtAddress.Text = _UserList_GetData.GetString(6);
                    Warehouse_DropDown.SelectedValue = _UserList_GetData.GetString(7);
                    Role_DropDown.SelectedItem= _UserList_GetData.GetString(8);
                    txtUserID.Text= id;
                    connection.Close();
            
            }
            catch (Exception)
            {

                throw;
            }
        
        }
       
        private void Product_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (product_Search.Text != null)
            {
                UserSearch(product_Search.Text);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text == String.Empty)
                {
                    txtName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Name ! ");
                    return;
                }
                if (txtUserName.Text == String.Empty)
                {
                    txtUserName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The User Name ! ");
                    return;
                }
                if (txtQID.Text == String.Empty)
                {
                    txtQID.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The txtQID ! ");
                    return;
                }
                if (txtPassword.Text == String.Empty)
                {
                    txtPassword.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Password ! ");
                    return;
                }
                if (txtAddress.Text == String.Empty)
                {
                    txtAddress.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Address ! ");
                    return;
                }
                if (txtEmail.Text == String.Empty)
                {
                    txtEmail.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Email ! ");
                    return;
                }
              
                if (txtMobile.Text == String.Empty)
                {
                    txtMobile.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Mobile No. ! ");
                    return;
                }
                dynamic WH = Warehouse_DropDown.SelectedItem as dynamic;
                int WarehouseId = WH.WarehouseId;
                dynamic RD = Role_DropDown.SelectedItem as dynamic;
                int RoleID = RD.RoleID;
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                // connection.Close();
                connection.Open();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: Getting All Warehouse Present"; });
                NpgsqlCommand Edit_User_List = new NpgsqlCommand("Update ad_user_pos set QID=" + txtQID.Text + ", name='" + txtName.Text + "', username='" + txtUserName.Text + "', password='" + txtPassword.Text + "', mobile='" + txtMobile.Text + "', email='" + txtEmail.Text + "', address='" + txtAddress.Text + "', m_warehouse_id='" + WarehouseId + "', ad_role_id='" + RoleID + "' ,updatedby=" + _UserID + "   where ad_user_id='" + txtUserID.Text + "' and ad_org_id=" + _OrgId + ";", connection);//
                Edit_User_List.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Record Edit Successfully");
                NavigationService.Navigate(new ViewUser());
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewUser());
        }

        private void Btn_edit_Click(object sender, RoutedEventArgs e)
        {
            txtQID.IsEnabled = true;
            txtName.IsEnabled = true;
            txtUserName.IsEnabled = true;
            txtPassword.IsEnabled = true;
            txtMobile.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtAddress.IsEnabled = true;
          
            btnCancel.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
            btnCancelBorder.Visibility = Visibility.Visible;
            btnSaveBorder.Visibility= Visibility.Visible;
        }

        private void Btn_Add_Category_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUser());
        }

        private void User_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic UD = User_DropDown.SelectedValue as dynamic;
            int id = UD.UserID;
            if (User_DropDown.SelectedIndex <= 0)
            {
                BindUserList();
            }
            else
            {
                BindUserWise(id);
            }
        }
        #endregion
    }
}
