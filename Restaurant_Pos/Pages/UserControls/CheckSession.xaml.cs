using Newtonsoft.Json;
using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant_Pos.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for CheckSession.xaml
    /// </summary>
    public partial class CheckSession : UserControl
    {
        private string jsonCreateSession;
        private dynamic CreateSessionApiStringResponce;
        private dynamic CreateSessionApiJSONResponce;
        private int CheckServerError = 0;

        public CheckSession()
        {
            InitializeComponent();
        }

        private void SessionResume_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Grid).Children.Remove(this);
        }

        private void SessionCreateNew_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);

            SessionCart CreateSession = new SessionCart
            {
                operation = "createSession",
                username = RetailPage.AD_UserName,
                password = RetailPage.AD_UserPassword,
                clientId = RetailPage.AD_Client_ID.ToString(),
                orgId = RetailPage.AD_ORG_ID.ToString(),
                userId = RetailPage.AD_USER_ID.ToString(),
                businessPartnerId = RetailPage.AD_bpartner_Id.ToString(),
                roleId = RetailPage.AD_ROLE_ID.ToString(),
                warehouseId = RetailPage.AD_Warehouse_Id.ToString(),
                remindMe = "Y",
                macAddress = "54:C9:DF:B8:23:B6", //LoginViewModel._DeviceMacAddress,
                version = "1.0",
                appName = "POS",
            };
            jsonCreateSession = JsonConvert.SerializeObject(CreateSession);
            try
            {
                CreateSessionApiStringResponce = PostgreSQL.ApiCallPost(jsonCreateSession);
                CheckServerError = 1;
            }
            catch
            {
                CheckServerError = 0;
            }
            if (CheckServerError == 1)
            {
                CreateSessionApiJSONResponce = JsonConvert.DeserializeObject(CreateSessionApiStringResponce);

                if (CreateSessionApiJSONResponce.responseCode == "200")
                {
                    double _sessionId = CreateSessionApiJSONResponce.sessionId;
                    RetailPage.AD_SessionID = _sessionId;

                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("UPDATE ad_user_pos " +
                    "SET sessionid = " + _sessionId + " " +
                    "WHERE ad_client_id = " + RetailPage.AD_Client_ID + "  and ad_org_id = " + RetailPage.AD_ORG_ID + " and ad_user_id = " + RetailPage.AD_USER_ID + " ; ", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                    connection.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            Console.WriteLine(RetailPage.AD_SessionID);
            RetailPage.Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";

            (this.Parent as Grid).Children.Remove(this);
        }
    }
}