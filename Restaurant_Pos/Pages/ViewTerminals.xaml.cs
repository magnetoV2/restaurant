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
    /// Interaction logic for ViewTerminals.xaml
    /// </summary>
    public partial class ViewTerminals : Page
    {
        #region 
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<m_TerminalsList> _m_TerminalsList = new List<m_TerminalsList>();
        public List<m_TerminalsWise> _m_TerminalsWise = new List<m_TerminalsWise>();

        public string connstring = PostgreSQL.ConnectionString;

        #endregion

        #region Public Function
        public ViewTerminals()
        {
            InitializeComponent();
            BindTerminalsList();
            Enabled();


        }
        public void Enabled()
        {
            txtDescription.IsEnabled = false;
            txtName.IsEnabled = false;
            btn_Cancel.Visibility = Visibility.Hidden;
            btn_Save.Visibility = Visibility.Hidden;
            bdCancel.Visibility = Visibility.Hidden;
            bdSave.Visibility = Visibility.Hidden;
            btn_edit.IsEnabled = false;
        }
        public void ClearData()
        {
            txtName.Text = "";
            txtDescription.Text = "";
        }
        #endregion

        #region Bind Data Function
        public void BindTerminalsList()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TerminalsList_GetData = new NpgsqlCommand("select m_kitchen_id,name from  m_kitchen where ad_org_id=" + _OrgId + " ;", connection);//
                NpgsqlDataReader _TerminalsList_GetData = cmd_TerminalsList_GetData.ExecuteReader();
                _m_TerminalsList.Clear();
                
                    _m_TerminalsList.Add(new m_TerminalsList()
                    {

                        id = 0,
                        Name = "All"


                    });
                    while (_TerminalsList_GetData.Read())
                    {

                        _m_TerminalsList.Add(new m_TerminalsList()
                        {

                            id = _TerminalsList_GetData.GetInt32(0),
                            Name = _TerminalsList_GetData.GetString(1)


                        });
                    }

                    connection.Close();
                if (_m_TerminalsList.Count == 0)
                {

                    MessageBox.Show("Please Add Terminals!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Terminals_DropDown.ItemsSource = _m_TerminalsList;

                    });
                }

                
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Terminals  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Terminals Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindTerminalsLst()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_Terminals_GetData = new NpgsqlCommand("select m_kitchen_id,name from  m_kitchen where ad_org_id=" + _OrgId + "  ;", connection);//
                NpgsqlDataReader _Terminals_GetData = cmd_Terminals_GetData.ExecuteReader();
                _m_TerminalsWise.Clear();
                Terminals_ListBox.ItemsSource = "";
                while (_Terminals_GetData.Read())
                {

                    _m_TerminalsWise.Add(new m_TerminalsWise()
                    {
                        id = _Terminals_GetData.GetInt32(0),
                        Name = _Terminals_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (_m_TerminalsWise.Count == 0)
                {
                    MessageBox.Show("Please Add Terminals List!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Terminals_ListBox.ItemsSource = _m_TerminalsWise;
                    });
                }
              
              
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Terminals List  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Terminals List Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }
        public void BindTerminalsWise(int id)
        {
         try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TerminalsWise_GetData = new NpgsqlCommand("select m_kitchen_id,name from  m_kitchen where ad_org_id=" + _OrgId + "  And m_kitchen_id='" + id + "' ;", connection);//
                NpgsqlDataReader __TerminalsWise_GetData = cmd_TerminalsWise_GetData.ExecuteReader();
                _m_TerminalsWise.Clear();
                Terminals_ListBox.ItemsSource = "";
                while (__TerminalsWise_GetData.Read())
                {

                    _m_TerminalsWise.Add(new m_TerminalsWise()
                    {
                        id = __TerminalsWise_GetData.GetInt32(0),
                        Name = __TerminalsWise_GetData.GetString(1)


                    });
                }

                connection.Close();
                if (_m_TerminalsWise.Count == 0)
                {
                    MessageBox.Show("Please Add Terminals !");

                    return;

                }
                else
                {

                    this.Dispatcher.Invoke(() =>
                    {

                        Terminals_ListBox.ItemsSource = _m_TerminalsWise;
                    });
                }
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error Terminals Wise List  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Terminals Wise List  Not Bind", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        public void BindTerminals(string Search_Key)
        {
            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TerminalsWise_GetData = new NpgsqlCommand("select m_kitchen_id,name from  m_kitchen where ad_org_id=" + _OrgId + "  And name like '%" + Search_Key + "%' ;", connection);//
                NpgsqlDataReader __TerminalsWise_GetData = cmd_TerminalsWise_GetData.ExecuteReader();
                _m_TerminalsWise.Clear();
                Terminals_ListBox.ItemsSource = "";

                while (__TerminalsWise_GetData.Read())
                {

                    _m_TerminalsWise.Add(new m_TerminalsWise()
                    {
                        id = __TerminalsWise_GetData.GetInt32(0),
                        Name = __TerminalsWise_GetData.GetString(1)


                    });
                }

                connection.Close();

                if (_m_TerminalsWise.Count == 0)
                {
                    MessageBox.Show("Terminals Is Not Found!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Terminals_ListBox.ItemsSource = _m_TerminalsWise;
                    });

                }
        }
            
            catch (Exception ex)
            {

                log.Error(" ===================  Error Terminals Is Not Found  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Terminals Is Not Found!", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion

        #region Event
        private void Terminals_Search_KeyUp(object sender, KeyEventArgs e)
        {
            string Search_Key = Terminals_Search.Text;
            BindTerminals(Search_Key);
        }

        private void Terminals_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic TL = Terminals_ListBox.SelectedItem as dynamic;
            int id = TL.id;
            string Name = TL.Name;
            btn_edit.IsEnabled = true;
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            connection.Open();
            NpgsqlCommand cmd_Terminals_GetData = new NpgsqlCommand("select name,Description from  m_kitchen where ad_org_id=" + _OrgId + "  And m_kitchen_id='" + id + "' and name='"+ Name + "' ;", connection);//
            NpgsqlDataReader _Terminals_GetData = cmd_Terminals_GetData.ExecuteReader();
            _Terminals_GetData.Read();
            txtName.Text = _Terminals_GetData.GetString(0);
            txtDescription.Text = _Terminals_GetData.GetString(1);
            txtTerminalsID.Text = id.ToString();
            connection.Close();
        }

        private void Terminals_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic PC = Terminals_DropDown.SelectedItem as dynamic;
            int id = PC.id;
            if (Terminals_DropDown.SelectedIndex <= 0)
            {
                BindTerminalsLst();
            }
            else
            {
                BindTerminalsWise(id);
            }
        }

        private void Btn_edit_Click(object sender, RoutedEventArgs e)
        {
            txtDescription.IsEnabled = true;
            txtName.IsEnabled = true;
            btn_Cancel.Visibility = Visibility.Visible;
            btn_Save.Visibility = Visibility.Visible;
            bdCancel.Visibility = Visibility.Visible;
            bdSave.Visibility = Visibility.Visible;

        }

        private void Btn_Add_Terminals_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTerminals());
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewTerminals());
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text == String.Empty)
                {
                    txtName.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Name ! ");
                    return;
                }
                if (txtDescription.Text == String.Empty)
                {
                    txtDescription.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartRedBrush);
                    MessageBox.Show("Please Enter The Description ! ");
                    return;
                }

                NpgsqlConnection connection = new NpgsqlConnection(connstring);

                connection.Open();
                NpgsqlCommand cmd_update_Terminals = new NpgsqlCommand("update m_kitchen set  name='" + txtName.Text + "', Description='" + txtDescription.Text + "' where m_kitchen_id='" + txtTerminalsID.Text + "' and ad_org_id='" + _OrgId + "';", connection);
                cmd_update_Terminals.ExecuteReader();
                ClearData();
                connection.Close();

                MessageBox.Show("Record Edit Successfully !");
                NavigationService.Navigate(new ViewTerminals());
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Edit Terminals  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Edit Terminals", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
