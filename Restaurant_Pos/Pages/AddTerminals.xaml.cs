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
    /// Interaction logic for AddTerminals.xaml
    /// </summary>
    public partial class AddTerminals : Page
    {
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());
        public int Sequenc_id { get; set; }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string connstring = PostgreSQL.ConnectionString;

        public AddTerminals()
        {
            InitializeComponent();
        }
        public void ClearData()
        {
            txtName.Text = "";
            txtDescription.Text = "";
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
                NpgsqlCommand cmd_select_sequenc_no_Terminals = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name='m_kitchen';", connection);
                NpgsqlDataReader _get__Ad_sequenc_no_Terminals = cmd_select_sequenc_no_Terminals.ExecuteReader();
                if (_get__Ad_sequenc_no_Terminals.Read())
                {
                    Sequenc_id = _get__Ad_sequenc_no_Terminals.GetInt32(4) + 1;
                }
                connection.Close();

                connection.Open();
                NpgsqlCommand INSERT_cmd_Terminals_Detail = new NpgsqlCommand("insert into m_kitchen(m_kitchen_id,ad_client_id,ad_org_id,Createdby,updatedby,isactive, name, Description)    " + " " + "" + "" +
                                                                                              "VALUES(" + Sequenc_id + "," + _clientId + " ," + _OrgId + " ," + _UserID + "," + _UserID + ",'Y','" + txtName.Text + "','" + txtDescription.Text + "') ;", connection);
                INSERT_cmd_Terminals_Detail.ExecuteNonQuery();
                //this.Dispatcher.Invoke(() => { LoginProcessingText.Text = "Process: ad_client_id  new data  Inserted"; });
                ClearData();
                connection.Close();
                connection.Open();
                NpgsqlCommand cmd_update_sequenc_Terminals = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name='m_kitchen';", connection);
                cmd_update_sequenc_Terminals.ExecuteReader();
                connection.Close();
                MessageBox.Show("Record Saved Successfully !");
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Insert Terminals Detail  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Insert Terminals Detail", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
    }
}
