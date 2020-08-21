using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using Restaurant_Pos.Pages.Session;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ToastNotifications.Messages;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Collections.Specialized;
using Restaurant_Pos.ViewModels.Page;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;

namespace Restaurant_Pos.Pages
{
    public partial class RetailPage : Page
    {
        #region Public Iteams

        public object ProductIteams { get; set; }
        public object ProductHold_list { get; set; }
        public object View_Order_Completed_object { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CurrentDateAndTime { get; set; }
        public string LineIteam_Up_Rf { get; set; }
        public string is_Return { get; set; }
        public static string Check_keyboard_Focus { get; set; }
        public static string quick_Check_windows_Focus { get; set; }
        /// <summary>
        /// 0 - Not Selected 1 - Cash 2 - Card 3 - Complementry 4 - Credit 5 - Split Payment
        /// </summary>
        public int payment_method_selected = 0;

        private double addAmount;
        public List<Product> items = new List<Product>();
        public List<Product_Hold> ProductHold_list_items = new List<Product_Hold>();
        public List<OrderHeaders> OrderHeaders_items = new List<OrderHeaders>();
        public List<Invoice_Post> Invoice_Post_items = new List<Invoice_Post>();
        public List<OrderDetails> OrderDetails_items = new List<OrderDetails>();
        public List<invoiceList> invoiceList_items = new List<invoiceList>();
        public List<View_Order_Completed> View_Order_Completed_items = new List<View_Order_Completed>();
        public static int ListView_Index_No;
        public string Percentage_OR_Price { get; set; }
        public string ButtonColor { get; set; }
        public double Cart_OverAllDiscount = 0;
        public double Grand_Cart_Total = 0;
        public double SubToTal_Balance_Amount = 0;
        public string connstring = PostgreSQL.ConnectionString;
        public long _sequenc_id { get; set; }

        #endregion Public Iteams

        #region Private

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string DeviceMacAdd = LoginViewModel.DeviceMacAddress();
        private int invoice_number = 0;
        private int document_no = 0;
        private int Product_Each_Item_Count = 0;
        private string jsonCreateSession;
        private string jsonResumeSession;
        private dynamic CreateSessionApiStringResponce;
        private dynamic CreateSessionApiJSONResponce;
        private dynamic ResumeSessionApiStringResponce;
        private dynamic ResumeSessionApiJSONResponce;
        private dynamic POSReleaseOrderApiStringResponce; private dynamic POSReleaseOrderApiJSONResponce;
        private dynamic POSPostInvoiceApiStringResponce; private dynamic POSPostInvoiceApiJSONResponce;
        private dynamic POSGetOrderListCustomerApiJSONResponce;
        private dynamic POSAddproductApiStringResponce;
        private dynamic POSAddcustomerApiStringResponce;
        private dynamic POSAddcategoryApiStringResponce;
        private dynamic POSGetProductApiJSONResponce;
        private dynamic POSGetProductApiJSONResponce_Bom;
        private int CheckServerError = 0;
        private bool _selectedtext_valuecharger;
        private bool _selectedtext_quickvaluecharger;
        Boolean isDuplicateCopy;


        #endregion Private

        #region Global

        public static long AD_Client_ID { get; set; }
        public static long AD_ORG_ID { get; set; }
        public static string AD_ORG_name { get; set; }
        public static string AD_ORG_arabicname { get; set; }
        public static object AD_ORG_logo { get; set; }
        public static string AD_ORG_phone { get; set; }
        public static string AD_ORG_email { get; set; }
        public static string AD_ORG_add { get; set; }
        public static string AD_ORG_city { get; set; }
        public static string AD_ORG_country { get; set; }
        public static string AD_ORG_postal { get; set; }
        public static string AD_ORG_weburl { get; set; }
        public static string AD_ORG_footermessage { get; set; }
        public static string AD_ORG_arabicfootermessage { get; set; }
        public static string AD_ORG_termsmessage { get; set; }
        public static string AD_ORG_arabictermsmessage { get; set; }
        public static long AD_USER_ID { get; set; }
        public static long AD_ROLE_ID { get; set; }
        public static string AD_ROLE_Name { get; set; }
        public static long AD_Warehouse_Id { get; set; }
        public static string AD_Warehouse_Name { get; set; }
        public static string AD_Warehouse_phone { get; set; }
        public static string AD_Warehouse_city { get; set; }
        public static string AD_Warehouse_pricelistid { get; set; }
        public static long AD_bpartner_Id { get; set; }
        public static long AD_PricelistID { get; set; }
        public static long AD_CostelementID { get; set; }
        public static long AD_CurrencyID { get; set; }
        public static string AD_CurrencyCode { get; set; }
        public static long AD_CashbookID { get; set; }
        public static long AD_PeriodID { get; set; }
        public static long AD_Paymenttermid { get; set; }
        public static long AD_ADtableID { get; set; }
        public static long AD_AccountSchemaid { get; set; }
        public static string AD_PaymentRule { get; set; }
        public static string AD_PrintSalesSummary { get; set; }
        public static string AD_PrintPrebill { get; set; }
        public static string AD_ShowComplement { get; set; }
        public static string AD_IsDiscount { get; set; }
        public static long AD_DiscPercent { get; set; }
        public static string AD_UserName { get; set; }
        public static string AD_UserPassword { get; set; }
        public static double AD_SessionID { get; set; }
        public static string AD_Session_Started_at { get; set; } = String.Empty;
        public static bool _NetworkUpStatus { get; set; }
        public static long AD_Inv_SessionID { get; set; }

        #endregion Global

        #region Constructor
        class QuickProductList
        {
            private string _prodName;

            public string Product
            {
                get { return _prodName; }
                set { _prodName = value; }
            }
            private long _prodID;

            public long ProdID
            {
                get { return _prodID; }
                set { _prodID = value; }
            }
            private string _searchKey;

            public string SearchKey
            {
                get { return _searchKey; }
                set { _searchKey = value; }
            }

            private string _searchField;

            public string SearchField
            {
                get { return _searchField; }
                set { _searchField = value; }
            }

            private Double _prodPrice;
            public Double ProdPrice
            {
                get { return _prodPrice; }
                set { _prodPrice = value; }
            }
        }
        class ProductList
        {
            private string _prodName;

            public string Product
            {
                get { return _prodName; }
                set { _prodName = value; }
            }
            private long _prodID;

            public long ProdID
            {
                get { return _prodID; }
                set { _prodID = value; }
            }
            private string _searchKey;

            public string SearchKey
            {
                get { return _searchKey; }
                set { _searchKey = value; }
            }

            private string _searchField;

            public string SearchField
            {
                get { return _searchField; }
                set { _searchField = value; }
            }

            private string _prodPrice;
            public string ProdPrice
            {
                get { return _prodPrice; }
                set { _prodPrice = value; }
            }
        }
        class CategoryList
        {
            private string _Name;

            public string categoryName
            {
                get { return _Name; }
                set { _Name = value; }
            }
            private long _categoryID;

            public long categoryID
            {
                get { return _categoryID; }
                set { _categoryID = value; }
            }
            private string _searchField;
            public string SearchField
            {
                get { return _searchField; }
                set { _searchField = value; }
            }
        }
        class UomList
        {
            private string _uomName;

            public string uomName
            {
                get { return _uomName; }
                set { _uomName = value; }
            }
            private long _uomID;

            public long uomID
            {
                get { return _uomID; }
                set { _uomID = value; }
            }
        }
        class CustomerList
        {
            private int _Bp_partner_id;
            public int Bp_partner_id
            {
                get { return _Bp_partner_id; }
                set { _Bp_partner_id = value; }
            }
            private string _searchField;

            public string SearchField
            {
                get { return _searchField; }
                set { _searchField = value; }
            }
            private int _CR_Number;
            public int CR_Number
            {
                get { return _CR_Number; }
                set { _CR_Number = value; }
            }
            private string _firstName;
            public string firstName
            {
                get { return _firstName; }
                set { _firstName = value; }
            }
            private string _mobilenumber;
            public string mobilenummber
            {
                get { return _mobilenumber; }
                set { _mobilenumber = value; }
            }
            private string _lastName;
            public string lastName
            {
                get { return _lastName; }
                set { _lastName = value; }
            }
            private string _emailaddress;
            public string emailaddress
            {
                get { return _emailaddress; }
                set { _emailaddress = value; }
            }
            private string _allowcredit_lmt;
            public string allow_credit_lmt
            {
                get { return _allowcredit_lmt; }
                set { _allowcredit_lmt = value; }
            }
            private double _creditLimit;
            public double creditLimit
            {
                get { return _creditLimit; }
                set { _creditLimit = value; }
            }
            private string _address;
            public string address
            {
                get { return _address; }
                set { _address = value; }
            }
            private string _city;
            public string city
            {
                get { return _city; }
                set { _city = value; }
            }
            private string _country;
            public string country
            {
                get { return _country; }
                set { _country = value; }
            }
            private string _zipcode;
            public string zipcode
            {
                get { return _zipcode; }
                set { _zipcode = value; }
            }
        }
        private static readonly List<UomList> UomList_dataSource = new List<UomList>();
        private static readonly List<Product_Uom> uom_dataSource = new List<Product_Uom>();
        private static readonly List<Product_Uom_Erp> uom_erp_dataSource = new List<Product_Uom_Erp>();

        private static readonly List<ProductList> dataSource = new List<ProductList>();
        private static readonly List<QuickProductList> quickdataSource = new List<QuickProductList>();
        private static readonly List<CustomerList> CustomerList_dataSource = new List<CustomerList>();
        private static readonly List<QuickProductList> lstprodadd_quickdataSource = new List<QuickProductList>();
        private static readonly List<ProductList> lstproduct_quickdataSource = new List<ProductList>();
        private static readonly List<CategoryList> CategoryList_quickdataSource = new List<CategoryList>();
        private static readonly List<CategoryList> CategoryList_quickdataSource1 = new List<CategoryList>();
        private static readonly List<invoiceList> cust_invoiceList = new List<invoiceList>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");


            SerialPort.display(AD_ORG_name, "", "                       ", "QSale.Qa");
            // SerialPort.display(AD_ORG_name, "","" , "         QSale.Qa");
            //this.productSearch_cart.Focus();
        }
        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Window.Current.CoreWindow.KeyDown += coreWindow_KeyDown;
        //    Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        //}
        //protected void Products_PatternChanged(object sender, Gui.Controls.AutoComplete.AutoCompleteArgs args)
        //{
        //    //check
        //    if (string.IsNullOrEmpty(args.Pattern))
        //    {
        //      //  productSearch_cart.IsDropDownOpen = false;
        //        args.CancelBinding = true;
        //    }
        //    else
        //        args.DataSource = RetailPage.GetProducts(args.Pattern);

        //    if (args.DataSource != null)
        //        if (args.DataSource.Cast<Object>().Count() == 0)
        //         //   productSearch_cart.IsDropDownOpen = false;
        //    if (productSearch_cart.Text == "")
        //        //productSearch_cart.IsDropDownOpen = false;
        //}

        /// <summary>
        /// Get a list of cities that follow a pattern
        /// </summary>
        /// <returns></returns>
         //private static ObservableCollection<ProductList> GetProducts(string Pattern)
        //{
        //    // match on contain (could do starts with)
        //    //return new ObservableCollection<ProductList>(
        //    //    RetailPage.dataSource.
        //    //    Where((prod, match) => prod.Product.ToLower().Contains(Pattern.ToLower())));
        //}

        public RetailPage()
        {

            InitializeComponent();
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            //Loaded += Retail_MainWindow_Keyboard_Focus;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            #region Time Loop Refresh

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            #endregion Time Loop Refresh

            #region Invoice and LoadProducts  Post Timmer

            DispatcherTimer Invoicetimer = new DispatcherTimer();
            Invoicetimer.Interval = TimeSpan.FromSeconds(27);
            Invoicetimer.Tick += InvoicPost;
            Invoicetimer.Start();

            DispatcherTimer LoadProductstimer = new DispatcherTimer();
            LoadProductstimer.Interval = TimeSpan.FromSeconds(35);
            LoadProductstimer.Tick += Load_Products;
            LoadProductstimer.Start();
            #endregion Invoice and LoadProducts  Post Timmer

            #region Checking Old User Logged Status

            connection.Open();
            NpgsqlCommand cmd_ad_user_pos_check = new NpgsqlCommand("SELECT " +
                "t1.ad_client_id, " +                                           //0
                "t1.ad_org_id, " +                                              //1
                "t1.ad_role_id, " +                                             //2
                "t1.ad_user_id," +                                              //3
                "t1.c_bpartner_id, " +                                          //4
                "t1.m_warehouse_id," +                                          //5
                "t1.name, " +                                                   //6
                "t1.password," +                                                //7
                "t1.sessionid ," +                                              //8
                "t2.name as ad_org_name, " +                                    //9     ad_org_name
                "t2.arabicname as ad_org_arabicname, " +                        //10    ad_org_arabicname
                "t2.logo as ad_org_logo, " +                                    //11    ad_org_logo
                "t2.phone as ad_org_phone, " +                                  //12    ad_org_phone
                "t2.email as ad_ord_email, " +                                  //13    ad_ord_email
                "t2.address as ad_ord_add, " +                                  //14    ad_ord_add
                "t2.city as ad_org_city, " +                                    //15    ad_org_city
                "t2.country as ad_org_country, " +                              //16    ad_org_country
                "t2.postal as ad_org_postal, " +                                //17    ad_org_postal
                "t2.weburl as ad_org_weburl , " +                               //18    ad_org_weburl
                "t2.footermessage as ad_org_footermessage, " +                  //19    ad_org_footermessage
                "t2.arabicfootermessage as ad_org_arabicfootermessage, " +      //20    ad_org_arabicfootermessage
                "t2.termsmessage as ad_ord_termsmessage, " +                    //21    ad_ord_termsmessage
                "t2.arabictermsmessage as ad_org_arabictermsmessage," +         //22    ad_org_arabictermsmessage
                "t3.name as ad_role_name, " +                                   //23    ad_role_name
                "t4.name as m_warehouse_name , " +                              //24    m_warehouse_name
                "t4.phone as m_warehouse_phone, " +                             //25    m_warehouse_phone
                "t4.city as m_warehouse_city, " +                               //26    m_warehouse_city
                "t4.warehouepricelistid as m_warehouse_warehouepricelistid, " + //27    m_warehouse_warehouepricelistid
                "t1.attribute1 as session_start_time " +                        //28
                "FROM ad_user_pos  t1,ad_org t2,ad_role t3,m_warehouse t4 " +
                "where t1.islogged = 'Y' AND t1.isactive = 'Y' AND t1.ad_org_id = t2.ad_org_id " +
                "AND t1.ad_client_id = t3.ad_client_id AND t1.ad_role_id = t3.ad_role_id " +
                "AND t1.m_warehouse_id = t4.m_warehouse_id; ", connection);
            NpgsqlDataReader _get_ad_user_pos_check = cmd_ad_user_pos_check.ExecuteReader();

            if (_get_ad_user_pos_check.Read())
            {
                AD_Client_ID = _get_ad_user_pos_check.GetInt64(0);
                AD_ORG_ID = _get_ad_user_pos_check.GetInt64(1);
                AD_ROLE_ID = _get_ad_user_pos_check.GetInt64(2);
                AD_USER_ID = _get_ad_user_pos_check.GetInt64(3);
                AD_bpartner_Id = _get_ad_user_pos_check.GetInt64(4);
                AD_Warehouse_Id = _get_ad_user_pos_check.GetInt64(5);
                AD_UserName = _get_ad_user_pos_check.GetString(6);
                AD_UserPassword = _get_ad_user_pos_check.GetString(7);
                AD_SessionID = _get_ad_user_pos_check.GetDouble(8);
                AD_ORG_name = _get_ad_user_pos_check.GetString(9);
                AD_ORG_arabicname = _get_ad_user_pos_check.GetString(10);
                AD_ORG_logo = _get_ad_user_pos_check.GetString(11);
                AD_ORG_phone = _get_ad_user_pos_check.GetString(12);
                AD_ORG_email = _get_ad_user_pos_check.GetString(13);
                AD_ORG_add = _get_ad_user_pos_check.GetString(14);
                AD_ORG_city = _get_ad_user_pos_check.GetString(15);
                AD_ORG_country = _get_ad_user_pos_check.GetString(16);
                AD_ORG_postal = _get_ad_user_pos_check.GetString(17);
                AD_ORG_weburl = _get_ad_user_pos_check.GetString(18);
                AD_ORG_footermessage = _get_ad_user_pos_check.GetString(19);
                AD_ORG_arabicfootermessage = _get_ad_user_pos_check.GetString(20);
                AD_ORG_termsmessage = _get_ad_user_pos_check.GetString(21);
                AD_ORG_arabictermsmessage = _get_ad_user_pos_check.GetString(22);
                AD_ROLE_Name = _get_ad_user_pos_check.GetString(23);
                AD_Warehouse_Name = _get_ad_user_pos_check.GetString(24);
                AD_Warehouse_phone = _get_ad_user_pos_check.GetString(25);
                AD_Warehouse_city = _get_ad_user_pos_check.GetString(26);
                AD_Warehouse_pricelistid = _get_ad_user_pos_check.GetInt64(27).ToString();
                if (AD_SessionID != 0)
                {
                    AD_Session_Started_at = _get_ad_user_pos_check.GetString(28);
                    SessionClose_User_Start_Time.Text = string.Format(new MyCustomDateProvider(), "{0}", Convert.ToDateTime(AD_Session_Started_at));
                }

                connection.Close();

                connection.Open();
                NpgsqlCommand cmd_sys_config = new NpgsqlCommand("SELECT pricelistid,costelementid, currencyid, currencycode, cashbookid, periodid,paymenttermid, adtableid, accountschemaid, paymentrule, printsalessummary,printprebill, showcomplement, isdiscount, discpercent" +
                " FROM ad_sys_config where ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + "; ", connection);
                NpgsqlDataReader _get_sys_config = cmd_sys_config.ExecuteReader();
                _get_sys_config.Read();

                AD_PricelistID = _get_sys_config.GetInt64(0);
                AD_CostelementID = _get_sys_config.GetInt64(1);
                AD_CurrencyID = _get_sys_config.GetInt64(2);
                AD_CurrencyCode = _get_sys_config.GetString(3);
                AD_CashbookID = _get_sys_config.GetInt64(4);
                AD_PeriodID = _get_sys_config.GetInt64(5);
                AD_Paymenttermid = _get_sys_config.GetInt64(6);
                AD_ADtableID = _get_sys_config.GetInt64(7);
                AD_AccountSchemaid = _get_sys_config.GetInt64(8);
                AD_PaymentRule = _get_sys_config.GetString(9);
                AD_PrintSalesSummary = _get_sys_config.GetString(10);
                AD_PrintPrebill = _get_sys_config.GetString(11);
                AD_ShowComplement = _get_sys_config.GetString(12);
                AD_IsDiscount = _get_sys_config.GetString(13);
                AD_DiscPercent = _get_sys_config.GetInt64(14);

                connection.Close();
            }
            else
            {
                connection.Close();
                Error_page.Visibility = Visibility.Visible;
                LoginError.Visibility = Visibility.Visible;
                return;
            }

            #endregion Checking Old User Logged Status

            #region Check the User Session

            CheckUserSession();

            #endregion Check the User Session
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            #region Initial Data Load
            try
            {
                BitmapImage bi = new BitmapImage();
                byte[] binaryData = Convert.FromBase64String(AD_ORG_logo.ToString());

                bi.BeginInit();
                bi.StreamSource = new MemoryStream(binaryData);
                bi.EndInit();

                Org_Logo.Source = bi;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            ////Org_Logo.Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            User_Name.Text = AD_UserName;
            Role_With_Warehouse.Text = AD_ROLE_Name + "(" + AD_Warehouse_Name + ")";
            Tittle_bar_User_Role.Text = AD_ROLE_Name;
            GetOrderHold_Count();
            Cart_Hold_Count.Text = ProductHold_list_items.Count().ToString();
            #endregion Initial Data Load
            // Bind_Product_Search();
            ProductIteams = items;
            ProductHold_list = ProductHold_list_items;
            View_Order_Completed_object = View_Order_Completed_items;
            DataContext = this;

        }
        private void Bind_Product_Search()
        {
            if (dataSource.Count == 0)
            {
                //  dataSource.Clear();
                NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                connection.Open();

                NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select  (m_product.name || '    |    ' || searchkey|| '    |    '  || sopricestd) as product," +
                   " m_product.m_product_id as prodid,searchkey,sopricestd,m_product.name from m_product WHERE  m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + "  order by product asc  ;", connection);
                NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
                while (_get_c_products.Read())
                {
                    dataSource.Add(new ProductList { SearchField = _get_c_products.GetString(0), ProdID = _get_c_products.GetInt64(1), SearchKey = _get_c_products.GetString(2), ProdPrice = _get_c_products.GetDouble(3).ToString("0.00"), Product = _get_c_products.GetString(4) });

                }
                connection.Close();
            }
            lstProdSearch.ItemsSource = null;
            lstProdSearch.ItemsSource = dataSource;
            if (lstProdSearch.Items.Count > 0)
            {
                lstProdSearch.SelectedIndex = 0;

            }

            txtProdSearch.Text = string.Empty;
            txtprodTotalsearchcount.Text = dataSource.Count.ToString();
            ICollectionView view1 = CollectionViewSource.GetDefaultView(dataSource);

            new TextSearchFilter(view1, this.txtProdSearch, lstProdSearch);



            this.txtProdSearch.Text = productSearch_cart.Text;
            productSearch_cart.Text = string.Empty;
            txtProdSearch.SelectionStart = txtProdSearch.Text.Length;

        }
        private void CheckUserSession()
        {
            if (AD_SessionID != 0)
            {
                SessionHead.Visibility = Visibility.Visible;
                SessionHead.Text = "Session Alert";
                Error_page.Visibility = Visibility.Visible;
                Session_Check.Visibility = Visibility.Visible;
                SessionCreateNew.Visibility = Visibility.Hidden;
                txt_openingBal.Visibility = Visibility.Hidden;
                txt_Price.Visibility = Visibility.Hidden;
                SessionChangePrice.Visibility = Visibility.Hidden;
                SessionResume.Focus();
                CustLeft.Visibility = Visibility.Hidden;
                WrongOrder.Visibility = Visibility.Hidden;
                CancelYes.Visibility = Visibility.Hidden;
                CancelNo.Visibility = Visibility.Hidden;
            }
            if (AD_SessionID == 0)
            {
                SessionHead.Visibility = Visibility.Visible;
                SessionHead.Text = "Session Alert";
                SessionDescription.Text = "Welcome To The New Session!!";
                Error_page.Visibility = Visibility.Visible;
                Session_Check.Visibility = Visibility.Visible;
                SessionResume.Visibility = Visibility.Hidden;
                SessionClose.Visibility = Visibility.Hidden;
                SessionCreateNew.Visibility = Visibility.Visible;
                txt_openingBal.Visibility = Visibility.Visible;
                txt_openingBal.Focus();
                txt_Price.Visibility = Visibility.Hidden;
                SessionChangePrice.Visibility = Visibility.Hidden;

                CustLeft.Visibility = Visibility.Hidden;
                WrongOrder.Visibility = Visibility.Hidden;
                CancelYes.Visibility = Visibility.Hidden;
                CancelNo.Visibility = Visibility.Hidden;
                CreateNewSession();
            }

        }

        #endregion Constructor

        #region Initial Run

        #region Current Time Timmer Function

        /// <summary>
        /// To get current data and time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lblTime.Text = DateTime.Now.ToString();
                _NetworkUpStatus = NetworkInterface.GetIsNetworkAvailable();
                if (_NetworkUpStatus == true)
                {
                    NetworkIndicatorIcon.Fill = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    NetworkIndicatorIcon.Fill = new SolidColorBrush(Colors.Red);
                }
                var formattedDate = string.Format(new MyCustomDateProvider(), "{0}", DateTime.Now);
                SessionClose_User_End_Time.Text = formattedDate;

                TimeSpan span = (DateTime.Now - Convert.ToDateTime(AD_Session_Started_at));

                String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                    span.Days, span.Hours, span.Minutes, span.Seconds);
                SessionClose_User_Total_Time_inDays.Text = String.Format("{0}", span.Days);
                SessionClose_User_Total_Time_inHours.Text = String.Format("{0}", span.Hours);
                SessionClose_User_Total_Time_inMinutes.Text = String.Format("{0}", span.Minutes);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();


                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Current Time Timmer Function

        private void Retail_MainWindow_Keyboard_Focus(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(productSearch_cart);
            Percentage_OR_Price = "%";
            OverAllDiscount_button.IsEnabled = false;
            OverAllDiscount_button_short.IsEnabled = false;
            OrderComplected_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
        }

        #region Invoic Posting Timmer Function

        private async void InvoicPost(object sender, EventArgs e)
        {
            try
            {
                if (_NetworkUpStatus == false)
                {
                    Console.WriteLine("No Network");
                    return;
                }

                Console.WriteLine("Invoice Timmer Function Running");

                #region Posting Background Function

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                //Getting Last 5 Invoice Which is Complected and Not Posted
                connection.Open();
                NpgsqlCommand cmd_c_invoic_iPaymentD = new NpgsqlCommand("SELECT t1.c_invoice_id, t1.ad_client_id, t1.ad_org_id,t1.ad_user_id," +
                    "t1.ad_role_id,t1.documentno, t1.m_warehouse_id, t1.c_bpartner_id, t1.qid, t1.mobilenumber, t1.discounttype, t1.discountvalue, " +
                    "t1.grandtotal, t1.orderid, t1.reason as invoice_reason,t1.created, t1.grandtotal_round_off, t1.total_items_count, t1.balance, t1.change, t1.lossamount, " +
                    "t1.extraamount,t2.cash, t2.card, t2.exchange, t2.redemption, t2.iscomplementary, t2.iscredit,t2.name_id, t2.mobile_numbler,t2.reason,t3.name,'Completed' as status,t1.is_return " +
                    "FROM c_invoice t1 ,c_invoicepaymentdetails t2,c_bpartner t3 " +
                    "WHERE is_completed = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'N' AND t1.c_invoice_id = t2.c_invoice_id AND t1.c_bpartner_id = t3.c_bpartner_id " +
                    "Union SELECT t1.c_invoice_id, t1.ad_client_id, t1.ad_org_id,t1.ad_user_id," +
                    "t1.ad_role_id,t1.documentno, t1.m_warehouse_id, t1.c_bpartner_id, t1.qid, t1.mobilenumber, t1.discounttype, t1.discountvalue, " +
                    "t1.grandtotal, t1.orderid, t1.reason as invoice_reason,t1.created, t1.grandtotal_round_off, t1.total_items_count, t1.balance, t1.change, t1.lossamount, " +
                    "t1.extraamount,0 as cash, 0 as card, 0 as exchange, 0 as redemption, 'N' as iscomplementary, 'N' as iscredit,'NA' as name_id, 'NA' as mobile_numbler,reason,t3.name ,'Canceled' as status,t1.is_return " +
                    "FROM c_invoice t1 ,c_bpartner t3 " +
                    "WHERE is_canceled = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'N' AND t1.c_bpartner_id = t3.c_bpartner_id LIMIT 5", connection);

                NpgsqlDataReader _get_c_invoic_iPaymentD = cmd_c_invoic_iPaymentD.ExecuteReader();
                if (_get_c_invoic_iPaymentD.HasRows == false)
                {
                    Console.WriteLine("All Complected Invoice Posted");
                    //log.Info("All Complected Invoice Posted");
                    connection.Close();
                    return;
                }
                while (_get_c_invoic_iPaymentD.Read())
                {
                    Invoice_Post_items.Add(new Invoice_Post()
                    {
                        c_invoice_id = _get_c_invoic_iPaymentD.GetDouble(0).ToString(),
                        ad_client_id = _get_c_invoic_iPaymentD.GetDouble(1).ToString(),
                        ad_org_id = _get_c_invoic_iPaymentD.GetDouble(2).ToString(),
                        ad_user_id = _get_c_invoic_iPaymentD.GetDouble(3).ToString(),
                        ad_role_id = _get_c_invoic_iPaymentD.GetDouble(4).ToString(),
                        documentno = _get_c_invoic_iPaymentD.GetString(5),
                        m_warehouse_id = _get_c_invoic_iPaymentD.GetDouble(6).ToString(),
                        c_bpartner_id = _get_c_invoic_iPaymentD.GetDouble(7).ToString(),
                        qid = _get_c_invoic_iPaymentD.GetString(8),
                        mobilenumber = _get_c_invoic_iPaymentD.GetString(9),
                        discounttype = _get_c_invoic_iPaymentD.GetDouble(10).ToString(),
                        discountvalue = _get_c_invoic_iPaymentD.GetDouble(11).ToString(),
                        grandtotal = _get_c_invoic_iPaymentD.GetDouble(12).ToString(),
                        orderid = _get_c_invoic_iPaymentD.GetDouble(13).ToString(),
                        invoice_reason = _get_c_invoic_iPaymentD.GetString(14),
                        created = _get_c_invoic_iPaymentD.GetDateTime(15).ToString("yyyy-MM-dd HH:mm:ss"),
                        grandtotal_round_off = _get_c_invoic_iPaymentD.GetDouble(16).ToString(),
                        total_items_count = _get_c_invoic_iPaymentD.GetDouble(17).ToString(),
                        balance = _get_c_invoic_iPaymentD.GetDouble(18).ToString(),
                        change = _get_c_invoic_iPaymentD.GetDouble(19).ToString(),
                        lossamount = _get_c_invoic_iPaymentD.GetDouble(20).ToString(),
                        extraamount = _get_c_invoic_iPaymentD.GetDouble(21).ToString(),
                        cash = _get_c_invoic_iPaymentD.GetDouble(22).ToString(),
                        card = _get_c_invoic_iPaymentD.GetDouble(23).ToString(),
                        exchange = _get_c_invoic_iPaymentD.GetDouble(24).ToString(),
                        redemption = _get_c_invoic_iPaymentD.GetDouble(25).ToString(),
                        iscomplementary = _get_c_invoic_iPaymentD.GetString(26),
                        iscredit = _get_c_invoic_iPaymentD.GetString(27),
                        name_id = _get_c_invoic_iPaymentD.GetString(28),
                        mobile_numbler = _get_c_invoic_iPaymentD.GetString(29),
                        reason = _get_c_invoic_iPaymentD.GetString(30),
                        c_bpartner_name = _get_c_invoic_iPaymentD.GetString(31),
                        c_status = _get_c_invoic_iPaymentD.GetString(32),
                        is_return = _get_c_invoic_iPaymentD.GetString(33)
                    });
                }
                connection.Close();

                await Task.Run(() =>
                {
                    Invoice_Post_items.ToList().ForEach(x =>
                    {
                        #region Posting Each Invoice to Sever and Updating the is_posted flag to 'Y'

                        string pricelistid, costelementid, currencyid, currencycode, cashbookid, periodid, paymenttermid, adtableid,
                        accountschemaid, paymentrule, printsalessummary, printprebill, showcomplement, isdiscount,
                        discpercent, name, password, islogged, isactive, sessionid;
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_ad_sys_config = new NpgsqlCommand("SELECT pricelistid, c_bpartner_id,costelementid, currencyid, currencycode, " +
                            "cashbookid, periodid,paymenttermid, adtableid, accountschemaid, paymentrule, printsalessummary, printprebill, showcomplement, " +
                            "isdiscount, discpercent  " +
                            "FROM ad_sys_config " +
                            "WHERE " +
                            "ad_client_id = " + x.ad_client_id + " AND ad_org_id = " + x.ad_org_id + " AND ad_user_id = " + x.ad_user_id + " ;", connection);
                        NpgsqlDataReader _get_c_invoic_ad_sys_config = cmd_c_invoic_ad_sys_config.ExecuteReader();

                        _get_c_invoic_ad_sys_config.Read();
                        pricelistid = _get_c_invoic_ad_sys_config.GetInt64(0).ToString();
                        costelementid = _get_c_invoic_ad_sys_config.GetInt64(2).ToString();
                        currencyid = _get_c_invoic_ad_sys_config.GetInt64(3).ToString();
                        currencycode = _get_c_invoic_ad_sys_config.GetString(4);
                        cashbookid = _get_c_invoic_ad_sys_config.GetInt64(5).ToString();
                        periodid = _get_c_invoic_ad_sys_config.GetInt64(6).ToString();
                        paymenttermid = _get_c_invoic_ad_sys_config.GetInt64(7).ToString();
                        adtableid = _get_c_invoic_ad_sys_config.GetInt64(8).ToString();
                        accountschemaid = _get_c_invoic_ad_sys_config.GetInt64(9).ToString();
                        paymentrule = _get_c_invoic_ad_sys_config.GetString(10);
                        printsalessummary = _get_c_invoic_ad_sys_config.GetString(11);
                        printprebill = _get_c_invoic_ad_sys_config.GetString(12);
                        showcomplement = _get_c_invoic_ad_sys_config.GetString(13);
                        isdiscount = _get_c_invoic_ad_sys_config.GetString(14);
                        discpercent = _get_c_invoic_ad_sys_config.GetInt64(15).ToString();

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_c_invoiceline = new NpgsqlCommand("SELECT " +
                            "t1.m_product_id," +          //0
                            "t1.productname," +           //1
                            "t1.paroductarabicname," +    //2
                            "t1.productbarcode," +        //3
                            "t1.c_uom_id," +              //4
                            "t1.uomname," +               //5
                            "t1.qtyinvoiced," +           //6
                            "t1.qtyentered," +            //7
                            "t1.saleprice," +             //8
                            "t1.costprice," +             //9
                            "t1.discounttype," +          //10
                            "t1.discountvalue," +         //11
                            "t1.linetotalamt," +          //12
                            "t1.pricelistid," +           //13
                            "t1.islinediscounted," +      //14
                            "t2.m_product_category_id " + //15
                            "FROM c_invoiceline t1 , m_product t2 " +
                            "WHERE t1.m_product_id = t2.m_product_id " +
                            "AND t1.ad_client_id = " + x.ad_client_id + " " +
                            "AND t1.ad_org_id =" + x.ad_org_id + "  " +
                            "AND t1.c_invoice_id = " + x.c_invoice_id + " " +
                            "AND t1.ad_user_id = " + x.ad_user_id + " ;", connection);
                        NpgsqlDataReader _get_c_invoic_c_invoiceline = cmd_c_invoic_c_invoiceline.ExecuteReader();

                        while (_get_c_invoic_c_invoiceline.Read())
                        {
                            double _dicount = _get_c_invoic_c_invoiceline.GetDouble(11),
                            PdiscountType = _get_c_invoic_c_invoiceline.GetDouble(10),
                            _price = _get_c_invoic_c_invoiceline.GetDouble(12),
                            _actualPrice = _get_c_invoic_c_invoiceline.GetDouble(8),
                            _qty = _get_c_invoic_c_invoiceline.GetInt64(7);
                            string _discountType, _dicountPercent, _discountAmount;
                            if (PdiscountType == 0)
                            {
                                _discountType = "P";
                                _dicountPercent = _dicount.ToString();
                                _discountAmount = Math.Round((((_actualPrice * _qty) * _dicount) / 100), 2).ToString();
                            }
                            else
                            {
                                _discountType = "A";
                                _dicountPercent = Math.Round(((_dicount / _price) * 100), 2).ToString();
                                _discountAmount = _dicount.ToString();
                            }
                            OrderDetails_items.Add(new OrderDetails()
                            {
                                isExists = "N",
                                KotLineID = "0",
                                description = "",
                                uomId = _get_c_invoic_c_invoiceline.GetInt64(4).ToString(),
                                productUOMValue = _get_c_invoic_c_invoiceline.GetString(5),
                                actualPrice = _get_c_invoic_c_invoiceline.GetDouble(8).ToString(),
                                costPrice = _get_c_invoic_c_invoiceline.GetInt64(9).ToString(),
                                orderedQty = _get_c_invoic_c_invoiceline.GetInt64(6).ToString(),
                                qty = _get_c_invoic_c_invoiceline.GetInt64(7).ToString(),
                                discountType = _discountType,
                                dicountPercent = _dicountPercent,
                                discountAmount = _discountAmount,
                                price = _price.ToString(),
                                productId = _get_c_invoic_c_invoiceline.GetInt64(0).ToString(),
                                productName = _get_c_invoic_c_invoiceline.GetString(1),
                                productCategoryId = _get_c_invoic_c_invoiceline.GetDouble(15).ToString(),
                            });
                        }
                        //Console.WriteLine("OrderDetails_items  "+ OrderDetails_items.Count);
                        //log.Info("OrderDetails_items  " + OrderDetails_items.Count);
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_ad_user_pos = new NpgsqlCommand("SELECT " +
                            "name, " +             //0
                            "password, " +         //1
                            "islogged, " +         //2
                            "isactive," +          //3
                            "sessionid  " +        //4
                            "FROM ad_user_pos " +
                            "WHERE ad_client_id = " + x.ad_client_id + " AND ad_org_id = " + x.ad_org_id + " AND ad_user_id = " + x.ad_user_id + " ; ", connection);
                        NpgsqlDataReader _get_c_invoic_ad_user_pos = cmd_c_invoic_ad_user_pos.ExecuteReader();
                        _get_c_invoic_ad_user_pos.Read();
                        name = _get_c_invoic_ad_user_pos.GetString(0);
                        password = _get_c_invoic_ad_user_pos.GetString(1);
                        islogged = _get_c_invoic_ad_user_pos.GetString(2);
                        isactive = _get_c_invoic_ad_user_pos.GetString(3);
                        sessionid = _get_c_invoic_ad_user_pos.GetInt64(4).ToString();


                        connection.Close();

                        #region JSON Array Formate 
                        JObject rss = new JObject();
                        if (x.c_status == "Canceled")
                        {
                            rss =
                          new JObject(

                              new JProperty("authorizedBy", accountschemaid),
                              new JProperty("SyncedTime", name),
                              new JProperty("remindMe", password),
                              new JProperty("reason", x.ad_client_id),
                              new JProperty("OrderDetails",
                                    new JArray(
                                        from p in OrderDetails_items
                                        select new JObject(
                                            new JProperty("costPrice", p.costPrice),
                                            new JProperty("orderedQty", p.orderedQty),
                                            new JProperty("dicountPercent", p.dicountPercent),
                                            new JProperty("qty", p.qty),
                                            new JProperty("isExists", p.isExists),
                                            new JProperty("uomId", p.uomId),
                                            new JProperty("discountAmount", p.discountAmount),
                                            new JProperty("productId", p.productId),
                                            new JProperty("actualPrice", p.actualPrice),
                                            new JProperty("KotLineID", p.KotLineID),
                                            new JProperty("price", (Convert.ToDouble(p.actualPrice) - (Convert.ToDouble(p.discountAmount) / Convert.ToDouble(p.qty)))),
                                            new JProperty("description", p.description),
                                            new JProperty("productUOMValue", p.productUOMValue),
                                           new JProperty("discountType", p.discountType),
                                            new JProperty("productName", p.productName),
                                            new JProperty("productCategoryId", p.productCategoryId)
                                                           )
                                                )//JArray END
                                           ),//OrderDetails END 
                                              new JProperty("showImage", "Y"),
                                             new JProperty("warehouseId", x.card),
                                              new JProperty("macAddress", DeviceMacAdd),
                                              new JProperty("businessPartnerId", periodid),
                                              new JProperty("password", password),
                                              new JProperty("clientId", x.redemption),
                                             new JProperty("version", 1.0),
                                              new JProperty("appName", "POS"),
                                              new JProperty("orgId", currencyid),
                                              new JProperty("operation", "POSOrderCancel"),
                                              new JProperty("username", name),
                                            new JProperty("sessionId", sessionid),
                                             new JProperty("userId", accountschemaid),
                              new JProperty("PaymentDetails",
                                  new JArray(
                                      new JObject(
                                              new JProperty("amount", x.exchange),
                                              new JProperty("paymenttype", "EXCHANGE")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.cash),
                                              new JProperty("paymenttype", "CASH")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.card),
                                              new JProperty("paymenttype", "CARD")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.redemption),
                                              new JProperty("paymenttype", "LOYALTY")
                                          )
                                      )
                                    ),//PaymentDetails END
                              new JProperty("OrderHeaders",
                                    new JObject(
                                         new JProperty("IsCash", "N"),
                                         new JProperty("periodId", periodid),
                                         new JProperty("docNo", x.documentno),
                                         new JProperty("currencyId", currencyid),
                                         new JProperty("creditName", x.name_id),
                                         new JProperty("businessPartnerId", x.c_bpartner_id),
                                         new JProperty("orgId", x.ad_org_id),
                                         new JProperty("cardAmount", x.card),
                                         new JProperty("mobilenumber", x.mobile_numbler),
                                         new JProperty("cashbookId", cashbookid),
                                         new JProperty("dueAmount", x.balance),
                                         new JProperty("userId", x.ad_user_id),
                                         new JProperty("totalAmount", x.grandtotal),
                                         new JProperty("posId", x.c_invoice_id),
                                        new JProperty("isReturned", x.is_return),
                                        new JProperty("redemptionAmount", x.redemption),
                                        new JProperty("createdDate", x.created),
                                        new JProperty("customerName", x.c_bpartner_name),
                                        new JProperty("lossAmount", x.lossamount),
                                        new JProperty("exchangeAmount", x.exchange),
                                        new JProperty("extraAmount", x.extraamount),
                                        new JProperty("refInvoiceId", 0),
                                        new JProperty("totalLines", Convert.ToUInt32(OrderDetails_items.Count())),
                                        new JProperty("qid", x.qid),
                                        new JProperty("IsCard", "N"),
                                        new JProperty("warehouseId", x.m_warehouse_id),
                                        new JProperty("paymentTermId", paymenttermid),
                                        new JProperty("clientId", x.ad_client_id),
                                        new JProperty("pricelistId", pricelistid),
                                        new JProperty("adTableId", adtableid),
                                         new JProperty("paidAmount", Math.Round(Convert.ToDouble(x.cash) + Convert.ToDouble(x.card) + Convert.ToDouble(x.exchange) + Convert.ToDouble(x.redemption), 2)),
                                         new JProperty("giftAmount", 0),
                                         new JProperty("accountSchemaId", accountschemaid),
                                        new JProperty("warehouseNo", 0),
                                        new JProperty("cashAmount", x.cash),
                                        new JProperty("salesrepId", x.ad_user_id)
                                                    // new JProperty("isComplement", x.iscomplementary),
                                                    // new JProperty("isCredit", x.iscredit)
                                                    )
                                )
                             );
                        }
                        else if (x.c_status == "Completed")
                        {
                            rss =
                              new JObject(
                                  new JProperty("operation", "POSOrderRelease"),
                                  new JProperty("username", name),
                                  new JProperty("password", password),
                                  new JProperty("clientId", x.ad_client_id),
                                  new JProperty("orgId", x.ad_org_id),
                                  new JProperty("userId", x.ad_user_id),
                                  new JProperty("roleId", x.ad_role_id),
                                  new JProperty("sessionId", 0),
                                  new JProperty("businessPartnerId", x.c_bpartner_id),
                                  new JProperty("warehouseId", x.m_warehouse_id),
                                  new JProperty("SyncedTime", 0),
                                  new JProperty("reason", x.reason),
                                  new JProperty("showImage", "Y"),
                                  new JProperty("macAddress", DeviceMacAdd),//LoginViewModel._DeviceMacAddress
                                  new JProperty("version", 1.0),
                                  new JProperty("appName", "POS"),
                                  new JProperty("remindMe", "Y"),

                                  new JProperty("OrderHeaders",
                                      new JObject(
                                          new JProperty("isReturned", x.is_return),
                                          new JProperty("clientId", x.ad_client_id),
                                          new JProperty("orgId", x.ad_org_id),
                                          new JProperty("warehouseId", x.m_warehouse_id),
                                          new JProperty("userId", x.ad_user_id),
                                          new JProperty("periodId", periodid),
                                          new JProperty("currencyId", currencyid),
                                          new JProperty("cashbookId", cashbookid),
                                          new JProperty("paymentTermId", paymenttermid),
                                          new JProperty("pricelistId", pricelistid),
                                          new JProperty("adTableId", adtableid),
                                          new JProperty("accountSchemaId", accountschemaid),
                                          new JProperty("createdDate", x.created),
                                          new JProperty("posId", x.c_invoice_id),
                                          new JProperty("docNo", x.documentno),
                                          new JProperty("refInvoiceId", 0),
                                          new JProperty("totalLines", Convert.ToUInt32(OrderDetails_items.Count())),
                                          new JProperty("qid", x.qid),
                                          new JProperty("warehouseNo", 0),
                                          new JProperty("customerName", x.c_bpartner_name),
                                          new JProperty("creditName", x.name_id),
                                          new JProperty("businessPartnerId", x.c_bpartner_id),
                                          new JProperty("mobilenumber", x.mobile_numbler),
                                          new JProperty("totalAmount", x.grandtotal),
                                          new JProperty("cashAmount", x.cash),
                                          new JProperty("cardAmount", x.card),
                                          new JProperty("exchangeAmount", x.exchange),
                                          new JProperty("redemptionAmount", x.redemption),
                                          new JProperty("paidAmount", Math.Round(Convert.ToDouble(x.cash) + Convert.ToDouble(x.card) + Convert.ToDouble(x.exchange) + Convert.ToDouble(x.redemption), 2)),
                                          new JProperty("dueAmount", x.balance),
                                          new JProperty("lossAmount", x.lossamount),
                                          new JProperty("extraAmount", x.extraamount),
                                          new JProperty("IsCash", "N"),
                                          new JProperty("IsCard", "N"),
                                          new JProperty("isComplement", x.iscomplementary),
                                          new JProperty("isCredit", x.iscredit)
                                                      )
                                                 ),//OrderHeaders END
                                  new JProperty("OrderDetails",
                                      new JArray(
                                          from p in OrderDetails_items
                                          select new JObject(
                                              new JProperty("isExists", p.isExists),
                                              new JProperty("KotLineID", p.KotLineID),
                                              new JProperty("description", p.description),
                                              new JProperty("uomId", p.uomId),
                                              new JProperty("productUOMValue", p.productUOMValue),
                                              new JProperty("actualPrice", p.actualPrice),
                                              new JProperty("costPrice", p.costPrice),
                                              new JProperty("orderedQty", p.orderedQty),
                                              new JProperty("qty", p.qty),
                                              new JProperty("discountType", p.discountType),
                                              new JProperty("dicountPercent", p.dicountPercent),
                                              new JProperty("discountAmount", p.discountAmount),
                                              new JProperty("price", (Convert.ToDouble(p.actualPrice) - (Convert.ToDouble(p.discountAmount) / Convert.ToDouble(p.qty)))),
                                              new JProperty("productId", p.productId),
                                              new JProperty("productName", p.productName),
                                              new JProperty("productCategoryId", p.productCategoryId)
                                                             )
                                                  )//JArray END
                                             ),//OrderDetails END
                                  new JProperty("PaymentDetails",
                                      new JArray(
                                          new JObject(
                                                  new JProperty("amount", x.exchange),
                                                  new JProperty("paymenttype", "EXCHANGE")
                                              ),
                                              new JObject(
                                                  new JProperty("amount", x.cash),
                                                  new JProperty("paymenttype", "CASH")
                                              ),
                                              new JObject(
                                                  new JProperty("amount", x.card),
                                                  new JProperty("paymenttype", "CARD")
                                              ),
                                              new JObject(
                                                  new JProperty("amount", x.redemption),
                                                  new JProperty("paymenttype", "LOYALTY")
                                              )
                                          )
                                        )//PaymentDetails END
                                 );
                        }
                        #endregion JSON Array Formate

                        #region Posting to Server

                        int CheckApiError = 0;
                        var val = rss.ToString();
                        //log.Info("----------------JSON Request--------------");
                        //log.Info(val);
                        //log.Info("----------------JSON END--------------");
                        try
                        {
                            POSReleaseOrderApiStringResponce = PostgreSQL.ApiCallPost(val);
                            CheckApiError = 1;
                        }
                        catch
                        {
                            CheckApiError = 0;
                            log.Error("POSReleaseOrderApi: Server Error");
                            log.Error("----------------JSON Request--------------");
                            log.Error(val);
                            log.Error("----------------JSON END------------------");
                        }
                        if (CheckApiError == 1)
                        {
                            POSReleaseOrderApiJSONResponce = JsonConvert.DeserializeObject(POSReleaseOrderApiStringResponce);
                            log.Info("POSReleaseOrderApiJSONResponce: " + POSReleaseOrderApiJSONResponce + "");
                            if (POSReleaseOrderApiJSONResponce.responseCode == "200")
                            {
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_Update_post_flag_c_invoic = new NpgsqlCommand("UPDATE c_invoice " +
                                    "SET is_posted = 'Y' " +
                                    "WHERE c_invoice_id = " + x.c_invoice_id + "; ", connection);
                                cmd_Update_post_flag_c_invoic.ExecuteReader();
                                connection.Close();
                                log.Info("Posted Invoice Flag Updated|#invoice number: " + x.c_invoice_id + "");
                            }
                            else
                            {
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_Update_post_ERROR_ = new NpgsqlCommand("UPDATE c_invoice " +
                                    "SET is_posted = 'N' , is_posterror = 'Y' " +
                                    "WHERE c_invoice_id = " + x.c_invoice_id + "; ", connection);
                                cmd_Update_post_ERROR_.ExecuteNonQuery();
                                connection.Close();

                                log.Error("Posting Invoice Failed|Responce Code: " + POSReleaseOrderApiJSONResponce.responseCode);
                                log.Error("----------------JSON Request--------------");
                                log.Error(val);
                                log.Error("----------------JSON END--------------");
                            }
                        }

                        OrderDetails_items.Clear();

                        #endregion Posting to Server

                        #endregion Posting Each Invoice to Sever and Updating the is_posted flag to 'Y'
                    });
                });

                //Clearing List Memory
                OrderHeaders_items.Clear();
                Invoice_Post_items.Clear();
                OrderDetails_items.Clear();
                connection.Close();

                #endregion Posting Background Function

                Console.WriteLine("Invoice Timmer Function Ended");
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Invoic Posting Timmer Function

        #endregion Initial Run

        #region KeyPad Function
        /// <summary>
        /// KeyPad_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPad_Click(object sender, RoutedEventArgs e)
        {
            if (Check_keyboard_Focus != null)
            {
                if (print_Barcode_Popup.IsOpen)
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && txtNoofBarcodePrint.Text.ToString().Contains("."))
                        return;
                    txtNoofBarcodePrint.Text = $"{txtNoofBarcodePrint.Text}{((Button)e.OriginalSource).Content.ToString()}";
                    txtNoofBarcodePrint.CaretIndex = txtNoofBarcodePrint.Text.Length;
                    txtNoofBarcodePrint.ScrollToEnd();
                }
                if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && productSearch_cart.Text.ToString().Contains("."))
                        return;
                    productSearch_cart.Text = $"{productSearch_cart.Text}{((Button)e.OriginalSource).Content.ToString()}";
                    productSearch_cart.CaretIndex = productSearch_cart.Text.Length;
                    productSearch_cart.ScrollToEnd();
                }
                if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && ValueChanger_key_pad.Text.ToString().Contains("."))
                        return;
                    if (_selectedtext_valuecharger == true)
                    {
                        ValueChanger_key_pad.Text = $"{((Button)e.OriginalSource).Content.ToString()}";
                        _selectedtext_valuecharger = false;
                    }
                    else
                    {
                        ValueChanger_key_pad.Text = $"{ValueChanger_key_pad.Text}{((Button)e.OriginalSource).Content.ToString()}";

                    }


                }
                if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && quick_ValueChanger_key_pad.Text.ToString().Contains("."))
                        return;

                    if (_selectedtext_quickvaluecharger == true)
                    {
                        quick_ValueChanger_key_pad.Text = $"{((Button)e.OriginalSource).Content.ToString()}";
                        _selectedtext_quickvaluecharger = false;
                    }
                    else
                    {
                        quick_ValueChanger_key_pad.Text = $"{quick_ValueChanger_key_pad.Text}{((Button)e.OriginalSource).Content.ToString()}";

                    }
                }
                if (Check_keyboard_Focus == "Exchange_Invoice_Number_Search_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Exchange_Invoice_Number_Search.Text.ToString().Contains("."))
                        return;
                    Exchange_Invoice_Number_Search.Text = $"{Exchange_Invoice_Number_Search.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Payment_Cash_Only_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Payment_Cash_Only_tx.Text.ToString().Contains("."))
                        return;
                    Payment_Cash_Only_tx.Text = $"{Payment_Cash_Only_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";

                }
                if (Check_keyboard_Focus == "Payment_Card_Only_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Payment_Card_Only_tx.Text.ToString().Contains("."))
                        return;
                    Payment_Card_Only_tx.Text = $"{Payment_Card_Only_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Payment_Complementary_Mobile_No_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Payment_Complementary_Mobile_No_tx.Text.ToString().Contains("."))
                        return;
                    Payment_Complementary_Mobile_No_tx.Text = $"{Payment_Complementary_Mobile_No_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Payment_Credit_Mobile_No_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Payment_Credit_Mobile_No_tx.Text.ToString().Contains("."))
                        return;
                    Payment_Credit_Mobile_No_tx.Text = $"{Payment_Credit_Mobile_No_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Split_Payment_Cash_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Split_Payment_Cash_tx.Text.ToString().Contains("."))
                        return;
                    Split_Payment_Cash_tx.Text = $"{Split_Payment_Cash_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Split_Payment_Card_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Split_Payment_Card_tx.Text.ToString().Contains("."))
                        return;
                    Split_Payment_Card_tx.Text = $"{Split_Payment_Card_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Split_Payment_GiftCash_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Split_Payment_GiftCash_tx.Text.ToString().Contains("."))
                        return;
                    Split_Payment_GiftCash_tx.Text = $"{Split_Payment_GiftCash_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
                if (Check_keyboard_Focus == "Split_Payment_Exchange_tx_GotFocus")
                {
                    if (((Button)e.OriginalSource).Content.ToString() == "." && Split_Payment_Exchange_tx.Text.ToString().Contains("."))
                        return;
                    Split_Payment_Exchange_tx.Text = $"{Split_Payment_Exchange_tx.Text}{((Button)e.OriginalSource).Content.ToString()}";
                }
            }
        }

        private void KeyPadErase_Click(object sender, RoutedEventArgs e)
        {
            if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus" && productSearch_cart.Text != String.Empty)
            {
                productSearch_cart.Text = productSearch_cart.Text.Remove(productSearch_cart.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus" && ValueChanger_key_pad.Text != String.Empty)
            {
                ValueChanger_key_pad.Text = ValueChanger_key_pad.Text.Remove(ValueChanger_key_pad.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Exchange_Invoice_Number_Search_GotFocus" && Exchange_Invoice_Number_Search.Text != String.Empty)
            {
                Exchange_Invoice_Number_Search.Text = Exchange_Invoice_Number_Search.Text.Remove(Exchange_Invoice_Number_Search.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Payment_Cash_Only_tx_GotFocus" && Payment_Cash_Only_tx.Text != String.Empty)
            {
                Payment_Cash_Only_tx.Text = Payment_Cash_Only_tx.Text.Remove(Payment_Cash_Only_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Payment_Card_Only_tx_GotFocus" && Payment_Card_Only_tx.Text != String.Empty)
            {
                Payment_Card_Only_tx.Text = Payment_Card_Only_tx.Text.Remove(Payment_Card_Only_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Payment_Complementary_Mobile_No_tx_GotFocus" && Payment_Complementary_Mobile_No_tx.Text != String.Empty)
            {
                Payment_Complementary_Mobile_No_tx.Text = Payment_Complementary_Mobile_No_tx.Text.Remove(Payment_Complementary_Mobile_No_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Payment_Credit_Mobile_No_tx_GotFocus" && Payment_Credit_Mobile_No_tx.Text != String.Empty)
            {
                Payment_Credit_Mobile_No_tx.Text = Payment_Credit_Mobile_No_tx.Text.Remove(Payment_Credit_Mobile_No_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Split_Payment_Cash_tx_GotFocus" && Split_Payment_Cash_tx.Text != String.Empty)
            {
                Split_Payment_Cash_tx.Text = Split_Payment_Cash_tx.Text.Remove(Split_Payment_Cash_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Split_Payment_Card_tx_GotFocus" && Split_Payment_Card_tx.Text != String.Empty)
            {
                Split_Payment_Card_tx.Text = Split_Payment_Card_tx.Text.Remove(Split_Payment_Card_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Split_Payment_GiftCash_tx_GotFocus" && Split_Payment_GiftCash_tx.Text != String.Empty)
            {
                Split_Payment_GiftCash_tx.Text = Split_Payment_GiftCash_tx.Text.Remove(Split_Payment_GiftCash_tx.Text.Length - 1);
            }
            if (Check_keyboard_Focus == "Split_Payment_Exchange_tx_GotFocus" && Split_Payment_Exchange_tx.Text != String.Empty)
            {
                Split_Payment_Exchange_tx.Text = Split_Payment_Exchange_tx.Text.Remove(Split_Payment_Exchange_tx.Text.Length - 1);
            }
        }

        private void KeyPad_clear_Click(object sender, RoutedEventArgs e)
        {
            if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
            {

                productSearch_cart.Text = String.Empty;
                // BarcodeSearch_cart.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
            {
                ValueChanger_key_pad.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
            {
                quick_ValueChanger_key_pad.SelectAll();
                _selectedtext_quickvaluecharger = true;
            }
            if (Check_keyboard_Focus == "Exchange_Invoice_Number_Search_GotFocus")
            {
                Exchange_Invoice_Number_Search.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Payment_Cash_Only_tx_GotFocus")
            {
                Payment_Cash_Only_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Payment_Card_Only_tx_GotFocus")
            {
                Payment_Card_Only_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Payment_Complementary_Mobile_No_tx_GotFocus")
            {
                Payment_Complementary_Mobile_No_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Payment_Credit_Mobile_No_tx_GotFocus")
            {
                Payment_Credit_Mobile_No_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Split_Payment_Cash_tx_GotFocus")
            {
                Split_Payment_Cash_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Split_Payment_Card_tx_GotFocus")
            {
                Split_Payment_Card_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Split_Payment_GiftCash_tx_GotFocus")
            {
                Split_Payment_GiftCash_tx.Text = String.Empty;
            }
            if (Check_keyboard_Focus == "Split_Payment_Exchange_tx_GotFocus")
            {
                Split_Payment_Exchange_tx.Text = String.Empty;
            }
        }
        private List<Product> Calculate_OVerAllDiscount(List<Product> items, double Actual_Amount, double _Cart_OverAllDiscount)
        {
            items.ToList().ForEach(x =>
            {
                // if (x.Line_Discount == "N")
                // {
                x.Percentpercentage_OR_Price = Percentage_OR_Price;
                x.Amount = (Math.Round(((Convert.ToDouble(x.Quantity)) * (Convert.ToDouble(x.Sopricestd))), 2)).ToString("0.00");
                if (x.Percentpercentage_OR_Price == "%")
                {
                    x.Discount = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                    double Total_Price = Convert.ToDouble(x.Amount);
                    double Discount_Amt = x.Discount / 100 * Total_Price;
                    double Selling_Price = Total_Price - Discount_Amt;

                    x.Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                }
                else
                {
                    //double Selling_Price = Total_Price - Discount_Amt;
                    double Total_Price = Convert.ToDouble(x.Amount);
                    double Discount_Amt = x.Discount;
                    double Selling_Price = Total_Price;
                    double Eachpercentgediscount = 0;
                    //if (_Cart_OverAllDiscount >= Total_Price)
                    //{
                    //    Selling_Price = 0;
                    //    _Cart_OverAllDiscount -= Total_Price;
                    //    x.Discount = Math.Round(Total_Price, 2);
                    //}
                    //else
                    //{
                    Total_Price = Convert.ToDouble(x.Amount);
                    if (Total_Price > 0)
                    {
                        double EachproductPercent = ((Convert.ToDouble(x.Sopricestd) * Convert.ToDouble(x.Quantity)) / Actual_Amount) * 100;
                        Eachpercentgediscount = (_Cart_OverAllDiscount * EachproductPercent) / 100;
                        x.Discount = Math.Round(Convert.ToDouble(Eachpercentgediscount.ToString()), 2);
                        x.Amount = (Math.Round(Total_Price - Eachpercentgediscount, 2)).ToString("0.00");

                    }

                }
                // }
            });
            return items;

        }
        private void KeyPadEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
                {
                    if (ValueChanger_key_pad.Text.ToString() != String.Empty)
                    {
                        if (items.Count > 0)
                        {
                            double Actual_Amount = 0.00;
                            foreach (var data in items)
                            {
                                // if (data.Line_Discount == "N")
                                // {
                                Actual_Amount = Actual_Amount + (Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity);
                                // }
                            }

                            if (LineIteam_Up_Rf == "Discount_Cart_LineIteam")
                            {
                                items[ListView_Index_No].Discount = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                items[ListView_Index_No].Line_Discount = "Y";
                                items[ListView_Index_No].Percentpercentage_OR_Price = Percentage_OR_Price;
                                items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");
                                if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                {
                                    if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) >= 100)
                                    {
                                        ValueChanger_key_pad.Clear();
                                        return;
                                    }
                                    double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                    double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                    double Selling_Price = Total_Price - Discount_Amt;

                                    items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                }
                                else
                                {

                                    double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                    double Discount_Amt = items[ListView_Index_No].Discount;
                                    double Selling_Price = Total_Price - Discount_Amt;
                                    if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > Total_Price)
                                    {
                                        ValueChanger_key_pad.Clear();
                                        return;
                                    }
                                    items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                }

                                ProductIteams = items;
                                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                view.Refresh();
                                ValueChanger_key_pad.Text = String.Empty;
                                Keyboard.Focus(productSearch_cart);

                                addAmount = 0.00;
                                foreach (var data in items)
                                {
                                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                }
                                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                Grand_Cart_Total = addAmount;
                                MaintainActual_AMount();
                            }

                            if (LineIteam_Up_Rf == "Quantity_Cart_LineIteam")
                            {
                                if (Convert.ToDouble(ValueChanger_key_pad.Text.ToString()) == 0)
                                {
                                    // var item = (sender as FrameworkElement).DataContext;
                                    //  int index = iteamProduct.Items.IndexOf(item);
                                    // ListView_Index_No = index;
                                    #region Customer display 
                                    double lessedamt = 0 - Convert.ToDouble(items[ListView_Index_No].Amount);
                                    string lessedSellingPrice = lessedamt.ToString("0.00");
                                    string CustDispspace1 = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                                    int TotalSpace1 = Convert.ToInt32(CustDispspace1);
                                    string strproductname1 = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                                    int productnamelen1 = strproductname1.Length;
                                    int sellingPricelen1 = lessedSellingPrice.Length;

                                    items.RemoveAt(ListView_Index_No);
                                    ICollectionView view1 = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view1.Refresh();
                                    Grand_Total_cart_price.Text = String.Empty;
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    Product_Each_Item_Count = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                    });
                                    if (items.Count() == 0)
                                    {
                                        Grand_Total_cart_price.Text = "0.00";
                                        Cart_Iteam_Count.Text = "0";
                                        Grand_Cart_Total = 0;
                                        SubToTal_Balance_Amount = 0;
                                        OverAllDiscount_tx.Text = "0";
                                        payment_method_selected = 0;
                                        Cart_OverAllDiscount = 0;
                                        Product_Each_Item_Count = 0;
                                        OverAllDiscount_button.IsEnabled = false;
                                        OrderComplected_button.IsEnabled = false;
                                        OrderCancel_button.IsEnabled = false;
                                    }
                                    Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                    Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                                    Keyboard.Focus(productSearch_cart);
                                    int totallen2 = "Total".Length;


                                    string strtotalprice2 = Grand_Cart_Total.ToString("0.00");
                                    int totalpricelen2 = strtotalprice2.Length;
                                    int space3 = 0;
                                    if (productnamelen1 > sellingPricelen1)
                                    {
                                        space3 = TotalSpace1 - productnamelen1 - sellingPricelen1;

                                    }
                                    int space4 = 0;
                                    if (productnamelen1 > sellingPricelen1)
                                    {
                                        space4 = TotalSpace1 - totallen2 - totalpricelen2;

                                    }

                                    string strspace3 = new string(' ', space3);
                                    string strspace4 = new string(' ', space4);
                                    SerialPort.display(strproductname1 + strspace3, lessedSellingPrice, "Total" + strspace4, strtotalprice2);
                                    #endregion Customer display
                                    ValueChanger_key_pad.Text = "";
                                    MaintainActual_AMount();
                                    return;
                                }
                                else
                                {
                                    items[ListView_Index_No].Quantity = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                    items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");

                                    if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    else
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    ValueChanger_key_pad.Text = String.Empty;
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    ValueChanger_key_pad.Text = String.Empty;
                                    Keyboard.Focus(productSearch_cart);

                                    Product_Each_Item_Count = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                    });
                                    Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                    MaintainActual_AMount();
                                }
                            }

                            if (LineIteam_Up_Rf == "OverAllDiscount")
                            {
                                Cart_OverAllDiscount = Convert.ToDouble(ValueChanger_key_pad.Text);
                                double _Cart_OverAllDiscount = Convert.ToDouble(ValueChanger_key_pad.Text);



                                double TotalDiscountApplied = 0;
                                double Balance_if_any = 0;
                                bool Exit = false;
                                items.ToList().ForEach(x =>
                                {
                                    // if (x.Line_Discount == "N")
                                    // {
                                    x.Percentpercentage_OR_Price = Percentage_OR_Price;
                                    x.Amount = (Math.Round(((Convert.ToDouble(x.Quantity)) * (Convert.ToDouble(x.Sopricestd))), 2)).ToString("0.00");

                                    if (x.Percentpercentage_OR_Price == "%")
                                    {
                                        if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > 100)
                                        {
                                            ValueChanger_key_pad.Clear();
                                            Exit = true;
                                        }
                                        if (!Exit)
                                        {
                                            x.Discount = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                            double Total_Price = Convert.ToDouble(x.Amount);

                                            double Discount_Amt = x.Discount / 100 * Total_Price;
                                            double Selling_Price = Total_Price - Discount_Amt;

                                            x.Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                        }
                                    }
                                    else
                                    {
                                        if (_Cart_OverAllDiscount > Actual_Amount)
                                        {
                                            ValueChanger_key_pad.Clear();
                                            Exit = true;
                                        }
                                        if (!Exit)
                                        {
                                            //double Selling_Price = Total_Price - Discount_Amt;
                                            double Total_Price = Convert.ToDouble(x.Amount);
                                            double Discount_Amt = x.Discount;
                                            double Selling_Price = Total_Price;
                                            double Eachpercentgediscount = 0;
                                            //if (_Cart_OverAllDiscount >= Total_Price)
                                            //{
                                            //    Selling_Price = 0;
                                            //    _Cart_OverAllDiscount -= Total_Price;
                                            //    x.Discount = Math.Round(Total_Price, 2);
                                            //}
                                            //else
                                            //{
                                            Total_Price = Convert.ToDouble(x.Amount);
                                            double EachproductPercent = ((Convert.ToDouble(x.Sopricestd) * Convert.ToDouble(x.Quantity)) / Actual_Amount) * 100;
                                            Eachpercentgediscount = (_Cart_OverAllDiscount * EachproductPercent) / 100;
                                            x.Discount = Math.Round(Convert.ToDouble(Eachpercentgediscount.ToString()), 2);
                                            TotalDiscountApplied += x.Discount;

                                            // Selling_Price = Total_Price - _Cart_OverAllDiscount;
                                            //  x.Discount = Math.Round(_Cart_OverAllDiscount, 2);
                                            // _Cart_OverAllDiscount -= _Cart_OverAllDiscount;
                                            //_Cart_OverAllDiscount =
                                            //}

                                            x.Amount = (Math.Round(Total_Price - Eachpercentgediscount, 2)).ToString("0.00");
                                            //Adjust remaing discount if any after calculationequal discount incase of decimals
                                            Balance_if_any = Cart_OverAllDiscount - TotalDiscountApplied;
                                        }
                                    }
                                    // }
                                });
                                if (Exit)
                                {
                                    return;
                                }
                                _Cart_OverAllDiscount = 0;
                                List<Product> tempview = items;
                                string max_amount = items.Select(i => i.Sopricestd).Max();
                                int Indexval = items.IndexOf(items.Where(x => x.Sopricestd == max_amount).FirstOrDefault());
                                items[Indexval].Discount += Balance_if_any;
                                //----
                                ProductIteams = items;
                                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                view.Refresh();
                                OverAllDiscount_tx.Text = Convert.ToDouble(ValueChanger_key_pad.Text).ToString("0.00");
                                OverAllDiscount_sy.Text = Percentage_OR_Price;
                                ValueChanger_key_pad.Text = String.Empty;
                                addAmount = 0.00;
                                foreach (var data in items)
                                {
                                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                }
                                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                Grand_Cart_Total = addAmount;
                                ValueChanger_key_pad.Text = String.Empty;
                                Keyboard.Focus(productSearch_cart);
                                MaintainActual_AMount();
                            }

                            if (LineIteam_Up_Rf == "Return_Price_Cart_LineIteam")
                            {
                                //double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                //if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > Total_Price)
                                //{
                                //    ValueChanger_key_pad.Clear();
                                //    return;
                                //}
                                items[ListView_Index_No].Amount = (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2)).ToString("0.00");


                                ProductIteams = items;
                                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                view.Refresh();
                                ValueChanger_key_pad.Text = String.Empty;
                                Keyboard.Focus(productSearch_cart);

                                addAmount = 0.00;
                                foreach (var data in items)
                                {
                                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                }
                                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                Grand_Cart_Total = addAmount;
                                MaintainActual_AMount();

                            }
                            #region Customer display 
                            string SellingPrice = Convert.ToDecimal(items[ListView_Index_No].Amount).ToString("0.00");
                            string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                            int TotalSpace = Convert.ToInt32(CustDispspace);
                            string strproductname = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                            int productnamelen = strproductname.Length;
                            int sellingPricelen = SellingPrice.Length;
                            int totallen = "Total".Length;
                            string strtotalprice = Grand_Cart_Total.ToString("0.00");
                            int totalpricelen = strtotalprice.Length;

                            int space1 = 0;
                            if (productnamelen > sellingPricelen)
                            {
                                space1 = TotalSpace - productnamelen - sellingPricelen;

                            }
                            int space2 = 0;
                            if (totallen > totalpricelen)
                            {
                                space2 = TotalSpace - totallen - totalpricelen;

                            }


                            // int space1 = TotalSpace - productnamelen - sellingPricelen;
                            // int space2 = TotalSpace - totallen - totalpricelen;
                            string strspace1 = new string(' ', space1);
                            string strspace2 = new string(' ', space2);
                            SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                            #endregion Customer display 

                        }
                        else
                        {
                            ValueChanger_key_pad.Clear();
                            Back_OR_Esc();
                            return;

                        }


                    }
                    else
                    {
                        ValueChanger_key_pad.Text = String.Empty;
                        Keyboard.Focus(productSearch_cart);
                    }

                }
                if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                {
                    if (productSearch_cart.Text != "")
                    {
                        add_Product();
                        //BarcodeSearch_products();
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Button_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
            {

                //  BarcodeSearch_cart.Select(BarcodeSearch_cart.Text.Length, 0); 
                Keyboard.Focus(productSearch_cart);
            }
            if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
            {
                ValueChanger_key_pad.Select(ValueChanger_key_pad.Text.Length, 0);

                Keyboard.Focus(ValueChanger_key_pad);
            }
            if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
            {
                quick_ValueChanger_key_pad.Select(quick_ValueChanger_key_pad.Text.Length, 0);
                Keyboard.Focus(quick_ValueChanger_key_pad);
            }
            if (Check_keyboard_Focus == "Exchange_Invoice_Number_Search_GotFocus")
            {
                Exchange_Invoice_Number_Search.Select(Exchange_Invoice_Number_Search.Text.Length, 0);
                Keyboard.Focus(Exchange_Invoice_Number_Search);
            }
            if (Check_keyboard_Focus == "Payment_Cash_Only_tx_GotFocus")
            {
                Payment_Cash_Only_tx.Select(Payment_Cash_Only_tx.Text.Length, 0);
                Keyboard.Focus(Payment_Cash_Only_tx);
            }
            if (Check_keyboard_Focus == "Payment_Card_Only_tx_GotFocus")
            {
                Payment_Card_Only_tx.Select(Payment_Card_Only_tx.Text.Length, 0);
                Keyboard.Focus(Payment_Card_Only_tx);
            }
            if (Check_keyboard_Focus == "Payment_Complementary_Mobile_No_tx_GotFocus")
            {
                Payment_Complementary_Mobile_No_tx.Select(Payment_Complementary_Mobile_No_tx.Text.Length, 0);
                Keyboard.Focus(Payment_Complementary_Mobile_No_tx);
            }
            if (Check_keyboard_Focus == "Payment_Credit_Mobile_No_tx_GotFocus")
            {
                Payment_Credit_Mobile_No_tx.Select(Payment_Credit_Mobile_No_tx.Text.Length, 0);
                Keyboard.Focus(Payment_Credit_Mobile_No_tx);
            }
            if (Check_keyboard_Focus == "Split_Payment_Cash_tx_GotFocus")
            {
                Split_Payment_Cash_tx.Select(Split_Payment_Cash_tx.Text.Length, 0);
                Keyboard.Focus(Split_Payment_Cash_tx);
            }
            if (Check_keyboard_Focus == "Split_Payment_Card_tx_GotFocus")
            {
                Split_Payment_Card_tx.Select(Split_Payment_Card_tx.Text.Length, 0);
                Keyboard.Focus(Split_Payment_Card_tx);
            }
            if (Check_keyboard_Focus == "Split_Payment_GiftCash_tx_GotFocus")
            {
                Split_Payment_GiftCash_tx.Select(Split_Payment_GiftCash_tx.Text.Length, 0);
                Keyboard.Focus(Split_Payment_GiftCash_tx);
            }
            if (Check_keyboard_Focus == "Split_Payment_Exchange_tx_GotFocus")
            {
                Split_Payment_Exchange_tx.Select(Split_Payment_Exchange_tx.Text.Length, 0);
                Keyboard.Focus(Split_Payment_Exchange_tx);
            }
        }

        #endregion KeyPad Function

        #region Cart Screen Focus Assist

        private void ValueChanger_key_pad_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "ValueChanger_key_pad_GotFocus";
            ValueChanger_key_pad.Text = String.Empty;
        }
        private void quick_ValueChanger_key_pad_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
            quick_ValueChanger_key_pad.SelectAll();
            _selectedtext_quickvaluecharger = true;
        }
        private void BarcodeSearch_cart_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            productSearch_cart.Text = String.Empty;
        }

        #endregion Cart Screen Focus Assist
        double price_short = 0;
        #region Product Barcode Search Bar
        private void productSearch_cart_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (items.Count == 0)
                    {
                        e.Handled = true;
                    }

                }
                if (Session_Check.Visibility == Visibility.Hidden && Error_page.Visibility == Visibility.Hidden)
                {


                    if (e.Key == Key.Enter)
                    {

                        if (productSearch_cart.Text != "")
                        {
                            add_Product();

                            //string Checkpriceeditable = "SELECT coalesce(m_product.ispriceeditable::varchar(10), 'N') ispriceeditable,m_product.sopricestd ,m_product.isquick " +
                            //" FROM m_product, m_product_price, ad_sys_config" +
                            //" WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey = @_searchkey" +
                            //" AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

                            //NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                            //connection.Open();
                            //NpgsqlCommand cmd_clientId_Read = new NpgsqlCommand(Checkpriceeditable, connection);
                            //if (productSearch_cart.Text.Contains('-'))
                            //{
                            //    searchkey_Short = productSearch_cart.Text.Split('-')[0];
                            //    if (searchkey_Short == "")
                            //    {
                            //        MessageBox.Show("  Invalid Product Search");
                            //        Keyboard.Focus(productSearch_cart);
                            //        return;
                            //    }
                            //    try
                            //    {
                            //        price_short = Convert.ToDouble(productSearch_cart.Text.Trim().Split('-')[1]);
                            //        cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", searchkey_Short);
                            //    }
                            //    catch
                            //    {
                            //        MessageBox.Show("  Invalid Product Search");
                            //        Keyboard.Focus(productSearch_cart);
                            //        return;
                            //    }
                            //}
                            //else
                            //{

                            //    cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", productSearch_cart.Text);
                            //}

                            //NpgsqlDataReader _clientId_Read = cmd_clientId_Read.ExecuteReader();
                            //if (_clientId_Read.Read())
                            //{
                            //    _isPriceeditable = _clientId_Read.GetString(0);
                            //    _pricestd = _clientId_Read.GetInt64(1);
                            //    _isQuickprod = _clientId_Read.GetString(2);
                            //}
                        }
                        else
                        {
                            if (e.Key == Key.OemPlus || e.Key == Key.OemMinus)
                            {
                                e.Handled = true;
                            }

                        }

                        /////////////////////


                    }
                    if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
                    {
                        open_Prod_Search();
                    }
                }
                else
                {

                    productSearch_cart.Text = "";
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void open_Prod_Search()
        {

            Bind_Product_Search();

            product_Search_Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(placementForPopup);
            product_Search_Popup.IsOpen = true;
            txtProdSearch.Focusable = true;
            MainPage.IsEnabled = false;
            txtProdSearch.Focus();
            Keyboard.Focus(txtProdSearch);
        }
        public CustomPopupPlacement[] placementForPopup(Size popupSize,
                                           Size targetSize,
                                           Point offset)
        {
            CustomPopupPlacement customPlacement1 =
               new CustomPopupPlacement(new Point(-40, 90), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement customPlacement2 =
                new CustomPopupPlacement(new Point(20, 30), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] customPlacements =
                    new CustomPopupPlacement[] { customPlacement1, customPlacement2 };
            return customPlacements;
        }


        private void OnGotFocusHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = e.Source as Button;
                if (b.Tag != null)
                {
                    btn_pruduct_id = Convert.ToInt32(b.Tag.ToString().Split('|')[0]);
                    quick_ValueChanger_key_pad.Text = Convert.ToDouble(b.Tag.ToString().Split('|')[1]).ToString("0.00");
                    Keyboard.Focus(quick_ValueChanger_key_pad);
                }

                b.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                b.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void OnLostFocusHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = e.Source as Button;
                if (b.Tag != null)
                {
                    b.Background = Brushes.White;
                    b.Foreground = Brushes.Black;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }



        }
        // Raised when Button losses focus. 
        // Changes the color of the Button back to white.
        private void MouseDoubleHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = e.Source as Button;
                if (b.Tag != null)
                {
                    btn_pruduct_id = Convert.ToInt32(b.Tag.ToString().Split('|')[0]);

                    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("update m_product set isquick='N',updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'  where m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.m_product_id =" + btn_pruduct_id + ";", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();
                    load_Quick_Products_Btn();
                    Error_page.Visibility = Visibility.Hidden;
                    paymentPopup.IsOpen = false;
                }

            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void load_Quick_Products_Btn()
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            quickdataSource.Clear();
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select  name as productname,m_product_id as prodid,m_product.sopricestd,m_product.searchkey from public.m_product  WHERE m_product.isquick='Y' and  m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + "  order by updated desc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            List<QuickProductList> prddataSource = new List<QuickProductList>();
            while (_get_c_products.Read())
            {
                //prddataSource.Insert.(Convert.ToInt32(_get_c_products.GetString(1)), _get_c_products.GetString(0));
                prddataSource.Add(new QuickProductList { ProdID = _get_c_products.GetInt64(1), Product = _get_c_products.GetString(0), ProdPrice = _get_c_products.GetDouble(2), SearchKey = _get_c_products.GetString(3) });


            }
            MainGrid.Children.Clear();
            if (prddataSource.Count == 0)
            {
                Button btnTemp = new Button();
                btnTemp.Name = "btnprod";
                btnTemp.Style = (Style)Application.Current.Resources["quick_KeypadBtn"];
                btnTemp.Height = 70;
                btnTemp.FontSize = 14;
                StackPanel stackPnl = new StackPanel();
                stackPnl.Name = "stkpnl";
                Thickness margin = stackPnl.Margin;
                margin.Left = 8;
                margin.Bottom = 8;
                stackPnl.Margin = margin;
                stackPnl.Children.Add(btnTemp);
                btnTemp.Content = "+ [Ctrl+N]";
                // btnTemp.FontWeight = FontWeights.Bold;
                btnTemp.Click += Button_Click_1;

                Grid.SetColumn(stackPnl, 0);
                Grid.SetRow(stackPnl, 0);
                MainGrid.Children.Add(stackPnl);
                //if(FrameworkElement.reg)
                //UnregisterName("GridButton_" + 0);
                //RegisterName("GridButton_" + 0, btnTemp);
                return;
            }

            int remainder = prddataSource.Count % MainGrid.ColumnDefinitions.Count;
            int no_of_rows = prddataSource.Count / MainGrid.ColumnDefinitions.Count;
            if (remainder > 0)
            {
                no_of_rows++;
            }
            for (int i = 0; i < no_of_rows; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                MainGrid.RowDefinitions.Add(rowDef);
            }
            int idx = 0;
            for (int row = 0; row < no_of_rows; row++)
            {
                for (int column = 0; column < MainGrid.ColumnDefinitions.Count; column++)
                {
                    if (idx < prddataSource.Count)
                    {

                        if (row == 0 && column == 0)
                        {
                            Button btnTemp = new Button();
                            btnTemp.Name = "btnprod";
                            btnTemp.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                            btnTemp.Content = "+ [Ctrl+N]";
                            btnTemp.Style = (Style)Application.Current.Resources["quick_KeypadBtn"];
                            btnTemp.Height = 70;
                            btnTemp.FontSize = 14;
                            btnTemp.Click += Button_Click_1;
                            btnTemp.TabIndex = idx;
                            btnTemp.Focus();

                            StackPanel stackPnl = new StackPanel();
                            stackPnl.Name = "stkpnl";
                            Thickness margin = stackPnl.Margin;
                            margin.Left = 8;
                            margin.Bottom = 8;
                            stackPnl.Margin = margin;
                            stackPnl.Children.Add(btnTemp);
                            Grid.SetColumn(stackPnl, column);
                            Grid.SetRow(stackPnl, row);
                            MainGrid.Children.Add(stackPnl);
                            //UnregisterName("GridButton_" + idx);
                            //RegisterName("GridButton_"+ idx, btnTemp);

                        }
                        else
                        {
                            Button btnTemp = new Button();
                            btnTemp.Name = "btnprod_" + idx;
                            btnTemp.Style = (Style)Application.Current.Resources["quick_KeypadBtn"];
                            btnTemp.Height = 70;
                            btnTemp.FontSize = 14;
                            btnTemp.GotFocus += OnGotFocusHandler;
                            btnTemp.LostFocus += OnLostFocusHandler;
                            StackPanel stackPnl = new StackPanel();
                            stackPnl.Name = "stkpnl";
                            Thickness margin = stackPnl.Margin;
                            margin.Left = 8;
                            margin.Bottom = 8;
                            stackPnl.Margin = margin;
                            stackPnl.Children.Add(btnTemp);
                            btnTemp.Foreground = Brushes.Black;

                            btnTemp.MouseDoubleClick += MouseDoubleHandler;
                            btnTemp.Content = SerialPort.Truncate(prddataSource[idx].Product.ToUpper(), 12) + "\n" + prddataSource[idx].SearchKey.ToUpper();
                            btnTemp.Tag = prddataSource[idx].ProdID.ToString() + '|' + (prddataSource[idx].ProdPrice.ToString());
                            btnTemp.ToolTip = prddataSource[idx].Product.ToUpper() + "\n" + prddataSource[idx].ProdID.ToString();
                            btnTemp.Click += Button_Click_2;
                            btnTemp.TabIndex = idx;
                            if (row == 0 && column == 1)
                            {
                                btnTemp.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                                quick_ValueChanger_key_pad.Text = Convert.ToDouble(prddataSource[idx].ProdPrice).ToString("0.00");
                                btn_pruduct_id = Convert.ToInt32(btnTemp.Tag.ToString().Split('|')[0]);
                                btnTemp.Loaded += OnLoaded;
                            }
                            Grid.SetColumn(stackPnl, column);
                            Grid.SetRow(stackPnl, row);
                            MainGrid.Children.Add(stackPnl);
                            idx++;
                            //UnregisterName("GridButton_" + idx);
                            //RegisterName("GridButton_"+idx, btnTemp);
                        }

                    }

                }
            }


            connection.Close();
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            Keyboard.Focus(Btn);
        }
        public class POSGetProduct
        {
            public int costPrice { get; set; }
            public string categoryValue { get; set; }
            public string scanbyWeight { get; set; }
            public Boolean isSynced { get; set; }
            public Boolean isBomAvailable { get; set; }
            public int categoryId { get; set; }
            public string productValue { get; set; }
            public int productUOMId { get; set; }
            public string[] productMultiImage { get; set; }
            public int productId { get; set; }
            public int sellingPrice { get; set; }
            public string categoryName { get; set; }
            public string productUOMValue { get; set; }
            public Boolean isPriceEditable { get; set; }
            public string description { get; set; }
            public string productArabicName { get; set; }
            public string productName { get; set; }
            public string scanbyPrice { get; set; }
            // Fill the missing properties for your data
        }
        private void lstprod_Bind_Qucik_Product_Search()
        {

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            quickdataSource.Clear();
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select  name as product,m_product_id as prodid,searchkey,(m_product.name || '|' || searchkey) as searchfield from public.m_product  WHERE isquick='N' and m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + "  order by product asc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                quickdataSource.Add(new QuickProductList { ProdID = Convert.ToInt64(_get_c_products.GetInt64(1)), Product = _get_c_products.GetString(0), SearchKey = _get_c_products.GetString(2), SearchField = _get_c_products.GetString(3) });

            }
            connection.Close();
            lstprod.ItemsSource = quickdataSource;
            // lstitems.DataContext = quickdataSource;
        }
        private void lstcust_Bind_customer_Search()
        {

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            CustomerList_dataSource.Clear();
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select  c_bpartner_id as number,name,coalesce(lastname, '') as lastname,coalesce(mobile_number, '') as mobilenumber,(c_bpartner_id || '    |    ' || name ) as searchfield from c_bpartner  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                CustomerList_dataSource.Add(new CustomerList { Bp_partner_id = _get_c_products.GetInt32(0), firstName = _get_c_products.GetString(1), lastName = _get_c_products.GetString(2), mobilenummber = _get_c_products.GetString(3).ToString(), SearchField = _get_c_products.GetString(4) });

            }
            connection.Close();
            lstcustomer.ItemsSource = CustomerList_dataSource;
            txtcustomerSearch.Text = string.Empty;
            // txtprodTotalsearchcount.Text = dataSource.Count.ToString();
            ICollectionView view1 = CollectionViewSource.GetDefaultView(CustomerList_dataSource);

            new TextSearchFilter(view1, this.txtcustomerSearch, lstcustomer);
            // lstitems.DataContext = quickdataSource;
        }

        private void fetch_all_products_details()
        {
            if (ddlCategory.SelectedValue != null)
            {
                if (ddlCategory.SelectedValue.ToString() != "0")
                {
                    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                    connection.Open();
                    quickdataSource.Clear();
                    NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select distinct  name as product, m_product_id as prodid, searchkey,(m_product.name || '|' || searchkey) as searchfield from public.m_product  WHERE   m_product_category_id=" + ddlCategory.SelectedValue + " and isquick='N' and  ad_client_id = " + AD_Client_ID + " AND  ad_org_id = " + AD_ORG_ID + "  order by product asc  ;", connection);
                    NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
                    while (_get_c_products.Read())
                    {
                        quickdataSource.Add(new QuickProductList { ProdID = _get_c_products.GetInt64(1), Product = _get_c_products.GetString(0), SearchKey = _get_c_products.GetString(2), SearchField = _get_c_products.GetString(2) });

                    }
                    connection.Close();
                    lstproducts.ItemsSource = null;
                    lstproducts.ItemsSource = quickdataSource;
                    // lstitems.DataContext = quickdataSource;
                }
                else
                {
                    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                    connection.Open();
                    quickdataSource.Clear();
                    NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select distinct name as product, m_product_id as prodid, searchkey,(m_product.name || '|' || searchkey) as searchfield from public.m_product   WHERE   isquick='N' and ad_client_id = " + AD_Client_ID + " AND  ad_org_id = " + AD_ORG_ID + "  order by product asc  ;", connection);
                    NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
                    while (_get_c_products.Read())
                    {
                        quickdataSource.Add(new QuickProductList { ProdID = _get_c_products.GetInt64(1), Product = _get_c_products.GetString(0), SearchKey = _get_c_products.GetString(2), SearchField = _get_c_products.GetString(3) });

                    }
                    connection.Close();
                    lstproducts.ItemsSource = null;
                    lstproducts.ItemsSource = quickdataSource;

                }
            }
        }
        private void Load_CategoryList(Boolean isListview)
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            CategoryList_quickdataSource.Clear();
            CategoryList_quickdataSource1.Clear();
            if (isListview == false)
            {

                CategoryList_quickdataSource.Add(new CategoryList { categoryName = "All Category", categoryID = 0 });
                CategoryList_quickdataSource1.Add(new CategoryList { categoryName = "+ Add Category", categoryID = 0 });

            }



            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select m_product_category_id,name,(name || '|' || searchkey) as  SearchField from m_product_category  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                CategoryList_quickdataSource.Add(new CategoryList { categoryName = _get_c_products.GetString(1), categoryID = Convert.ToInt64(_get_c_products.GetInt64(0)), SearchField = _get_c_products.GetString(2) });
                CategoryList_quickdataSource1.Add(new CategoryList { categoryName = _get_c_products.GetString(1), categoryID = Convert.ToInt64(_get_c_products.GetInt64(0)) });

            }
            connection.Close();
            if (isListview == false)
            {
                ddlCategory.ItemsSource = CategoryList_quickdataSource;
                ddlitem.ItemsSource = CategoryList_quickdataSource1;
                ddlItem1.ItemsSource = CategoryList_quickdataSource1;
            }
            if (isListview == true)
            {
                lstCategory.ItemsSource = CategoryList_quickdataSource;
                txtCategorySearch.Text = string.Empty;
                // txtprodTotalsearchcount.Text = dataSource.Count.ToString();
                ICollectionView view1 = CollectionViewSource.GetDefaultView(CategoryList_quickdataSource);

                new TextSearchFilter(view1, this.txtCategorySearch, lstCategory);
            }
            // lstitems.DataContext = quickdataSource;
        }
        private void Load_uomList()
        {
            //if (UomList_dataSource.Count == 0)
            // {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();

            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select distinct c_uom_id,name from m_product_units  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                UomList_dataSource.Add(new UomList { uomName = _get_c_products.GetString(1), uomID = _get_c_products.GetInt64(0) });

            }
            connection.Close();
            txtUom1.ItemsSource = null;
            txtUom1.ItemsSource = UomList_dataSource;
            txtUom.ItemsSource = null;
            txtUom.ItemsSource = UomList_dataSource;
            //  }
            // lstitems.DataContext = quickdataSource;
        }
        private void Bind_Quick_Product_Search()
        {

            string productValue = "";
            if (txtProduct.Text != "")
                productValue = txtProduct.Text;
            else
                productValue = productSearch_cart.Text;
            try
            {
                JObject rss = new JObject(
                                 new JProperty("remindMe", "N"),
                                    new JProperty("costElementId", AD_CostelementID.ToString()),
                                    new JProperty("showImage", "Y"),
                                    new JProperty("productValue", productValue),
                                    new JProperty("warehouseId", AD_Warehouse_Id.ToString()),
                                    new JProperty("macAddress", DeviceMacAdd),
                                    new JProperty("businessPartnerId", AD_bpartner_Id.ToString()),
                                    new JProperty("password", AD_UserPassword),
                                    new JProperty("clientId", AD_Client_ID.ToString()),
                                    new JProperty("version", "1.0"),
                                    new JProperty("appName", "POS"),
                                    new JProperty("orgId", AD_ORG_ID.ToString()),
                                    new JProperty("operation", "POSGetProduct"),
                                    new JProperty("username", AD_UserName),
                                    new JProperty("pricelistId", AD_PricelistID.ToString()),
                                    new JProperty("sessionId", AD_SessionID.ToString()),
                                    new JProperty("userId", AD_USER_ID.ToString()),
                                    new JProperty("roleId", AD_ROLE_ID.ToString())
                                           );

                POSGetProductApiJSONResponce = PostgreSQL.ApiCallPost(rss.ToString());

                POSGetProductApiJSONResponce = JsonConvert.DeserializeObject(POSGetProductApiJSONResponce);

                string jsonpr = JsonConvert.SerializeObject(POSGetProductApiJSONResponce);
                dynamic original = JsonConvert.DeserializeObject(jsonpr, typeof(object));
                int index = 0;
                foreach (var prod in original.products)
                {
                    if (index == 0)
                    {
                        if (original.products[0] != null)
                            original.products[0].shortkey = "F3";

                    }
                    if (index == 1)
                    {
                        if (original.products[1] != null)
                            original.products[1].shortkey = "F4";

                    }
                    if (index == 2)
                    {
                        if (original.products[2] != null)
                            original.products[2].shortkey = "F5";

                    }

                    index++;
                }
                POSGetProductApiJSONResponce = original;
                if (POSGetProductApiJSONResponce.responseCode == "200")
                {
                    //CheckApiError = 1;
                    lstprodadd_quickdataSource.Clear();
                    lstprodadd.ItemsSource = POSGetProductApiJSONResponce.products;
                }
            }
            catch
            {
                // CheckApiError = 0;
                log.Error("POSGetProduct: Server Error");
                log.Error("----------------JSON Request--------------");
                // log.Error(rss);
                log.Error("----------------JSON END------------------");
            }

            // lstitems.DataContext = quickdataSource;
        }
        private void add_Product()
        {
            if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
            {
                string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
                "m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1" +
                " FROM m_product, ad_sys_config" +
                " WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey = @_searchkey";
                // "   AND m_product_price.pricelistid = " + AD_PricelistID + ";";

                BarcodeSearch_products(SelectProduct, txtBarcode.Text, 2, false);
                txtBarcode.Text = "";
                return;
            }
            // if (productSearch_cart.SelectedValue != null)
            //  {
            //      string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
            //"m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1, m_product_price.pricestd" +
            //" FROM m_product, m_product_price, ad_sys_config" +
            //" WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.m_product_id = @_searchkey" +
            //" AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

            //      BarcodeSearch_products(SelectProduct, productSearch_cart.SelectedValue.ToString(), 1, false);
            //  }
            //  else
            if (productSearch_cart.Text != string.Empty)
            {
                string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
           "m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1 " +
           " FROM m_product , ad_sys_config" +
           " WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey = @_searchkey";
                //    " AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";
                if (productSearch_cart.Text.Contains('-'))
                {
                    BarcodeSearch_products(SelectProduct, productSearch_cart.Text.Split('-')[0], 2, false);
                    return;
                }
                else
                {

                    BarcodeSearch_products(SelectProduct, productSearch_cart.Text, 2, false);
                    return;
                }
            }
            if (lstsearchprodid != 0)
            {
                string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
          "m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1" +
          " FROM m_product , ad_sys_config" +
          " WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.m_product_id = @_searchkey";
                // " AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

                BarcodeSearch_products(SelectProduct, lstsearchprodid.ToString(), 1, false);
                lstsearchprodid = 0;
                return;
            }


        }
        //private void BarcodeSearch_cart_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {

        //        string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
        //      "m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1, m_product_price.pricestd" +
        //      "m_product.ispriceeditable,coalesce(m_product.ispriceeditable::varchar(10), 'N') ispriceeditable" +
        //      " FROM m_product, m_product_price, ad_sys_config" +
        //      " WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey = @_searchkey" +
        //      " AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

        //        BarcodeSearch_products(SelectProduct, BarcodeSearch_cart.Text,2);
        //    }

        //    if (e.Key == Key.Back)
        //    {
        //        string objTextBox = BarcodeSearch_cart.Text;
        //        if (objTextBox != String.Empty)
        //        {
        //            BarcodeSearch_cart.Text = BarcodeSearch_cart.Text.Remove(BarcodeSearch_cart.Text.Length - 1);
        //        }
        //    }

        //    if (items.Count() > 0)
        //    {
        //        OverAllDiscount_button.IsEnabled = true;
        //        OrderComplected_button.IsEnabled = true;
        //        OrderCancel_button.IsEnabled = true;
        //    }
        //}
        //private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        //{


        //    object j = this.FindName("GridButton_1");

        //    if (j.GetType() == typeof(Button))
        //    { 
        //        ((Button)j).Focus();
        //        Keyboard.Focus(((Button)j)); 
        //    }

        //}

        private void BarcodeSearch_products(string SelectProduct, string searchKey, int flag, Boolean is_Quick)
        {

            try
            {
                log.Error(SelectProduct);
                string objTextBox = searchKey;
                if (objTextBox != String.Empty)
                {
                    long _m_product_id, _currentcostprice, _sopricestd, _uomid, _m_product_category_id;
                    string _name, _uomname, _searchkey, _image, _arabicname, _scanbyweight, _scanbyprice, _productMultiUOM;
                    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                    connection.Open();
                    string searchkey_Short = "";
                    NpgsqlCommand cmd_clientId_Read = new NpgsqlCommand(SelectProduct, connection);
                    if (quick_ValueChanger_key_pad.Text.Contains('-'))
                    {
                        searchkey_Short = quick_ValueChanger_key_pad.Text.Split('-')[0];
                        if (searchkey_Short == "")
                        {
                            MessageBox.Show("  Invalid Product Search");
                            quick_ValueChanger_key_pad.SelectAll();
                            _selectedtext_quickvaluecharger = true;
                            Keyboard.Focus(quick_ValueChanger_key_pad);
                            return;
                        }
                        try
                        {
                            SelectProduct = "SELECT m_product.ad_client_id,m_product.ad_org_id,m_product.m_product_id,m_product.m_product_category_id," +
                            " m_product.name,m_product.searchkey,m_product.arabicname,m_product.image,m_product.scanbyweight, m_product.scanbyprice, " +
                           " m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1" +
                             " FROM m_product,  ad_sys_config WHERE m_product.ad_client_id = " + AD_Client_ID +
                            " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey ='" + searchkey_Short + "' AND m_product.isquick = 'Y' " +
                            " LIMIT 1";
                            log.Error(SelectProduct);
                            cmd_clientId_Read = new NpgsqlCommand(SelectProduct, connection);
                            price_short = Convert.ToDouble(quick_ValueChanger_key_pad.Text.Trim().Split('-')[1]);
                            quick_ValueChanger_key_pad.SelectAll();
                            _selectedtext_quickvaluecharger = true;
                            //cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", searchkey_Short);
                        }
                        catch
                        {
                            MessageBox.Show("  Invalid Product Search");
                            Keyboard.Focus(quick_ValueChanger_key_pad);
                            return;
                        }
                    }
                    if (productSearch_cart.Text.Contains('-'))
                    {
                        searchkey_Short = productSearch_cart.Text.Split('-')[0];
                        if (searchkey_Short == "")
                        {
                            MessageBox.Show("  Invalid Product Search");
                            Keyboard.Focus(productSearch_cart);
                            return;
                        }
                        try
                        {
                            price_short = Convert.ToDouble(productSearch_cart.Text.Trim().Split('-')[1]);
                            cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", searchkey_Short);
                        }
                        catch
                        {
                            MessageBox.Show("  Invalid Product Search");
                            Keyboard.Focus(productSearch_cart);
                            return;
                        }
                    }
                    else
                    {
                        ;
                        if (flag == 1)
                        {
                            cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", Convert.ToInt32(objTextBox));
                        }
                        else if (flag == 2)
                        {
                            cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", objTextBox);
                        }

                    }

                    NpgsqlDataReader _clientId_Read = cmd_clientId_Read.ExecuteReader();
                    if (_clientId_Read.Read())
                    {
                        _m_product_id = _clientId_Read.GetInt64(2);
                        _m_product_category_id = _clientId_Read.GetInt64(3);
                        _name = _clientId_Read.GetString(4);
                        _searchkey = _clientId_Read.GetString(5);
                        _arabicname = _clientId_Read.GetString(6);
                        _image = _clientId_Read.GetString(7);
                        _scanbyweight = _clientId_Read.GetString(8);
                        _scanbyprice = _clientId_Read.GetString(9);
                        _uomid = _clientId_Read.GetInt64(10);
                        _uomname = _clientId_Read.GetString(11);
                        _sopricestd = _clientId_Read.GetInt64(12);
                        _currentcostprice = _clientId_Read.GetInt64(13);
                        _productMultiUOM = _clientId_Read.GetString(14);
                        connection.Close();
                    }
                    else
                    {
                        //  productSearch_cart.Text = String.Empty;  

                        System.Media.SystemSounds.Exclamation.Play();
                        __Side_quickMenu_Page.Visibility = Visibility.Visible;
                        POSGetProductApiJSONResponce_Bom = null;
                        quick_Check_windows_Focus = "Quick_Product_Window_GotFocus";
                        Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                        InvoiceReSync.Visibility = Visibility.Hidden;
                        txtProduct.Text = productSearch_cart.Text;
                        Bind_Quick_Product_Search();
                        ICollectionView view1 = CollectionViewSource.GetDefaultView(quickdataSource);
                        new TextSearchFilter(view1, this.txtSearch, lstprodadd);
                        load_Quick_Products_Btn();
                        txtProduct.Text = productSearch_cart.Text;
                        quick_ValueChanger_key_pad.Focus();
                        quick_ValueChanger_key_pad.Focusable = true;
                        Keyboard.Focus(quick_ValueChanger_key_pad);
                        productSearch_cart.Text = string.Empty;
                        connection.Close();
                        //  return;
                        //   MessageBox.Show("Product Not Found                                             ", "Product Search");
                        // btnaddquickitem.Focus();


                        return;
                    }
                    string _product_name = _name;
                    string __product_id = _m_product_id.ToString();
                    string _product_barcode = _searchkey;
                    string _product_ad_client_id = AD_Client_ID.ToString();
                    string _product_ad_org_id = AD_ORG_ID.ToString();
                    string _product_m_product_category_id = _m_product_category_id.ToString();
                    string _product_arabicname = _arabicname;
                    string _product_image = _image;
                    string _product_scanbyweight = _scanbyweight;
                    string _product_scanbyprice = _scanbyprice;
                    string _product_uomid = _uomid.ToString();
                    string _product_uomname = _uomname;
                    string _product_SellingPricestd = "";
                    if (price_short > 0)
                    {
                        _product_SellingPricestd = price_short.ToString();
                        price_short = 0;
                    }
                    else
                    {
                        _product_SellingPricestd = _sopricestd.ToString();

                    }

                    if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                    {
                        if (quick_ValueChanger_key_pad.Text.Contains("-"))
                        {
                            price_short = Convert.ToDouble(quick_ValueChanger_key_pad.Text.Trim().Split('-')[1]);
                        }
                        else
                        {
                            try
                            {
                                _product_SellingPricestd = (Convert.ToDouble(quick_ValueChanger_key_pad.Text)).ToString("0.00");
                            }
                            catch
                            {
                                MessageBox.Show("Invalid Product Search");
                                quick_ValueChanger_key_pad.SelectAll();
                                _selectedtext_quickvaluecharger = true;
                                Keyboard.Focus(quick_ValueChanger_key_pad);
                                return;
                            }

                        }
                    }
                    // if (txt_Price.Text != "")
                    //     _product_SellingPricestd = txt_Price.Text;
                    //if (is_Quick)
                    //{
                    //    if (quick_ValueChanger_key_pad.Text == string.Empty || quick_ValueChanger_key_pad.Text == "0.00" || quick_ValueChanger_key_pad.Text == "0" || quick_ValueChanger_key_pad.Text == "00" || quick_ValueChanger_key_pad.Text == "0.0" || quick_ValueChanger_key_pad.Text == ".00" || quick_ValueChanger_key_pad.Text == "0.")
                    //        _product_SellingPricestd = _sopricestd.ToString();
                    //    else
                    //        _product_SellingPricestd = quick_ValueChanger_key_pad.Text;
                    //}
                    //else
                    //    _product_SellingPricestd = _sopricestd.ToString();

                    string _product_currentcostprice = _currentcostprice.ToString();
                    string _is_productMultiUOM = _productMultiUOM;

                    int item_count = items.Count();
                    if (item_count == 0)
                    {
                        double product_discount = 0;
                        double product_totalAmount = Convert.ToDouble(_product_SellingPricestd);

                        items.Insert(0, new Product()
                        {
                            #region Display Fields

                            Discount = product_discount,
                            Quantity = 1,
                            Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                            Amount = product_totalAmount.ToString("0.00"),
                            Percentpercentage_OR_Price = Percentage_OR_Price,
                            Product_Name = _product_name,
                            Iteam_Barcode = _product_barcode,

                            #endregion Display Fields

                            #region Hidden Fields

                            Ad_client_id = _product_ad_client_id,
                            Ad_org_id = _product_ad_org_id,
                            Is_productMultiUOM = _is_productMultiUOM,
                            Product_Arabicname = _product_arabicname,
                            Product_category_id = _product_m_product_category_id,
                            Product_ID = __product_id,
                            Product_Image = _product_image,
                            Scanby_Price = _product_scanbyprice,
                            Scanby_Weight = _product_scanbyweight,
                            Current_costprice = _product_currentcostprice,
                            Sopricestd = _product_SellingPricestd,
                            Uom_Id = _product_uomid,
                            Uom_Name = _product_uomname

                            #endregion Hidden Fields
                        });
                    }
                    else
                    {
                        bool result = false;
                        var item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode).FirstOrDefault());
                        if (txt_Price.Text != "")
                        {
                            result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00");
                            item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00").FirstOrDefault());
                        }
                        else
                        {
                            result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00");
                            item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00").FirstOrDefault());
                        }

                        if (result == true)
                        {

                            var _Quantity = items[item_index].Quantity + 1;
                            items[item_index].Quantity = Math.Round(Convert.ToDouble(_Quantity.ToString()), 2);
                            //items[item_index].Amount = Math.Round((Convert.ToDouble(_Quantity.ToString()) * items[item_index].Amount), 2);
                            items[item_index].Amount = (Math.Round(((Convert.ToDouble(items[item_index].Quantity)) * (Convert.ToDouble(items[item_index].Sopricestd))), 2)).ToString("0.00");
                            if (items[item_index].Percentpercentage_OR_Price == "%")
                            {
                                double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                double Discount_Amt = items[item_index].Discount / 100 * Total_Price;
                                double Selling_Price = Total_Price - Discount_Amt;

                                items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                            }
                            else
                            {
                                double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                double Discount_Amt = items[item_index].Discount;
                                double Selling_Price = Total_Price - Discount_Amt;

                                items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                            }
                            var item_reorder = items[item_index];
                            items.RemoveAt(item_index);
                            items.Insert(0, item_reorder);
                        }
                        else
                        {
                            double product_discount = 0;
                            double product_totalAmount = Convert.ToDouble(_product_SellingPricestd.ToString());


                            items.Insert(0, new Product()
                            {
                                #region Display Fields

                                Discount = product_discount,
                                Quantity = 1,
                                Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                                Amount = product_totalAmount.ToString("0.00"),
                                Percentpercentage_OR_Price = Percentage_OR_Price,
                                Product_Name = _product_name,
                                Iteam_Barcode = _product_barcode,

                                #endregion Display Fields

                                #region Hidden Fields

                                Ad_client_id = _product_ad_client_id,
                                Ad_org_id = _product_ad_org_id,
                                Is_productMultiUOM = _is_productMultiUOM,
                                Product_Arabicname = _product_arabicname,
                                Product_category_id = _product_m_product_category_id,
                                Product_ID = __product_id,
                                Product_Image = _product_image,
                                Scanby_Price = _product_scanbyprice,
                                Scanby_Weight = _product_scanbyweight,
                                Current_costprice = _product_currentcostprice,
                                Sopricestd = _product_SellingPricestd,
                                Uom_Id = _product_uomid,
                                Uom_Name = _product_uomname

                                #endregion Hidden Fields
                            });
                        }
                    }

                    ProductIteams = items;
                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                    view.Refresh();
                    Grand_Total_cart_price.Text = String.Empty;
                    addAmount = 0.00;
                    foreach (var data in items)
                    {
                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                    }
                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                    Grand_Cart_Total = addAmount;

                    productSearch_cart.Text = String.Empty;
                    #region Customer display 
                    string SellingPrice = Convert.ToDouble(_product_SellingPricestd).ToString("0.00");
                    string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                    int TotalSpace = Convert.ToInt32(CustDispspace);
                    string strproductname = SerialPort.Truncate(_product_name, 7);
                    int productnamelen = strproductname.Length;
                    int sellingPricelen = SellingPrice.Length;
                    int totallen = "Total".Length;
                    string strtotalprice = Grand_Cart_Total.ToString("0.00");
                    int totalpricelen = strtotalprice.Length;
                    int space1 = 0;
                    if (productnamelen > sellingPricelen)
                    {
                        space1 = TotalSpace - productnamelen - sellingPricelen;

                    }
                    int space2 = 0;
                    if (totallen > totalpricelen)
                    {
                        space2 = TotalSpace - totallen - totalpricelen;

                    }

                    // int space1 = TotalSpace - productnamelen - sellingPricelen;
                    // int space2 = TotalSpace - totallen - totalpricelen;
                    string strspace1 = new string(' ', space1);
                    string strspace2 = new string(' ', space2);
                    SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                    if (items.Count() > 0)
                    {
                        OrderCancel_button.IsEnabled = true;
                        OrderComplected_button.IsEnabled = true;
                        OverAllDiscount_button.Visibility = Visibility.Visible;
                        OverAllDiscount_button_short.Visibility = Visibility.Visible;
                    }
                    #endregion Customer display
                }
                MaintainActual_AMount();
                Product_Each_Item_Count = 0;
                items.ToList().ForEach(x =>
                {
                    Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                });
                Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();

                if (invoice_number == 0 && items.Count() > 0)
                {
                    #region Get Invoice & Doc No

                    //Checking and Getting Invoice POS Number
                    var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                    int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                    int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                    string _responce_code = Check_POS_Number_rs.Item3;
                    bool _network_status_ = Check_POS_Number_rs.Item4;
                    if (_responce_code == "0" || _responce_code == "200")
                    {
                        invoice_number = Check_POS_Number_rs.Item1;
                        document_no = Check_POS_Number_rs.Item2;
                        InvoiceNo.Text = invoice_number.ToString();
                    }
                    else if (_network_status_ != true || _responce_code == "500")
                    {
                        Error_page.Visibility = Visibility.Visible;
                        NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                        return;
                    }

                    #endregion Get Invoice & Doc No
                }

                Back_OR_Esc();
                if (iteamProduct.Items.Count > 0)
                {
                    iteamProduct.SelectedIndex = 0;
                    iteamProduct.Focus();
                    iteamProduct.SelectedItem = items.FirstOrDefault();
                    iteamProduct.SelectedIndex = 0;
                    OverAllDiscount_button.IsEnabled = true;
                    OverAllDiscount_button_short.IsEnabled = true;
                    quick_ValueChanger_key_pad.Text = "0.00";
                }
                lstProdSearch.ItemsSource = null;
                txtprodSearch.Text = string.Empty;
                product_Search_Popup.IsOpen = false;
                MainPage.IsEnabled = true;
                Back_OR_Esc();


            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }
        private void MaintainActual_AMount()
        {
            double Actual_Amount = 0.00;  
            bool _Is_discount = false;
            double eachDisountAmount = 0.00;
            if (LineIteam_Up_Rf == "Discount_Cart_LineIteam")
            {
                foreach (var data in items)
                {
                    if (data.Percentpercentage_OR_Price == "%")
                    {
                        eachDisountAmount += ((Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity) * Convert.ToDouble(data.Discount.ToString())) / 100;
                        //  OverAlldisc = Convert.ToDouble(data.Discount.ToString());
                        _Is_discount = true;
                    }
                    else
                    {
                        eachDisountAmount += Convert.ToDouble(data.Discount.ToString());
                        // OverAlldisc = OverAlldisc + Convert.ToDouble(data.Discount.ToString());
                        _Is_discount = false;
                    }

                    Actual_Amount = Actual_Amount + (Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity);
                }
            }
            else
            {
                foreach (var data in items)
                {
                    if (data.Percentpercentage_OR_Price == "%")
                    {
                        //eachDisountAmount += ((Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity) * Convert.ToDouble(data.Discount.ToString())) / 100;
                        eachDisountAmount = Convert.ToDouble(data.Discount.ToString());
                        _Is_discount = true;
                    }
                    else
                    {
                        // eachDisountAmount += (Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity) - Convert.ToDouble(data.Discount.ToString());
                        eachDisountAmount = eachDisountAmount + Convert.ToDouble(data.Discount.ToString());
                        _Is_discount = false;
                    }

                    Actual_Amount = Actual_Amount + (Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity);
                }
            }
         
            if (_Is_discount)
            {
                if (eachDisountAmount == 0)
                {
                    OverAllDiscount_tx.Text = "0";
                    OverAllDiscount_sy.Text = "";
                }
                else
                {
                    OverAllDiscount_tx.Text = Convert.ToDouble(eachDisountAmount).ToString("0.00");
                    if (LineIteam_Up_Rf == "Discount_Cart_LineIteam") 
                        OverAllDiscount_sy.Text = "QR"; 
                    else
                        OverAllDiscount_sy.Text = Percentage_OR_Price; 
                }


            }
            else
            {

                OverAllDiscount_tx.Text = Convert.ToDouble(eachDisountAmount).ToString("0.00");
                if (LineIteam_Up_Rf == "Discount_Cart_LineIteam")
                    OverAllDiscount_sy.Text = "QR";
                else
                    OverAllDiscount_sy.Text = Percentage_OR_Price;
            }

            if (Convert.ToDouble(Grand_Total_cart_price.Text) < Actual_Amount)
            {
                txtActualtotal.Text = Actual_Amount.ToString("0.00");
                txtActualtotal.TextDecorations = TextDecorations.Strikethrough;
                txtactualamountqr.Visibility = Visibility.Visible;

            }
            else
            {
                txtActualtotal.Text = string.Empty;
                txtactualamountqr.Visibility = Visibility.Hidden;

            }
        }
        private void ValueChanger_key_pad_keyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    e.Handled = true;
                }

                if (e.Key == Key.Enter)
                {
                    if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                    {
                        string objTextBox = "";
                        if (productSearch_cart.Text != string.Empty)
                            objTextBox = productSearch_cart.Text;

                        if (objTextBox != String.Empty)
                        {
                            string _Product_result = RetailViewModel.Product_barcode_search(objTextBox, AD_Client_ID.ToString(), AD_ORG_ID.ToString(), AD_PricelistID.ToString());

                            if (_Product_result == "N")
                            {
                                System.Media.SystemSounds.Exclamation.Play();
                                MessageBox.Show("Product Not Found", "Product Search");
                                productSearch_cart.Text = String.Empty;
                                Keyboard.Focus(productSearch_cart);
                                return;
                            }
                            dynamic _Product_resultString = JsonConvert.DeserializeObject(_Product_result);
                            string _product_name = _Product_resultString.name;
                            string __product_id = _Product_resultString.m_product_id;
                            string _product_barcode = _Product_resultString.searchkey;
                            string _product_ad_client_id = _Product_resultString.ad_client_id;
                            string _product_ad_org_id = _Product_resultString.ad_org_id;
                            string _product_m_product_category_id = _Product_resultString.m_product_category_id;
                            string _product_arabicname = _Product_resultString.arabicname;
                            string _product_image = _Product_resultString.image;
                            string _product_scanbyweight = _Product_resultString.scanbyweight;
                            string _product_scanbyprice = _Product_resultString.scanbyprice;
                            string _product_uomid = _Product_resultString.uomid;
                            string _product_uomname = _Product_resultString.uomname;
                            string _product_SellingPricestd = _Product_resultString.sopricestd;
                            string _product_currentcostprice = _Product_resultString.currentcostprice;
                            string _is_productMultiUOM = _Product_resultString.attribute1;

                            int item_count = items.Count();
                            if (item_count == 0)
                            {
                                double product_discount = 0;
                                double product_totalAmount = Convert.ToDouble(_product_SellingPricestd.ToString());
                                if (Cart_OverAllDiscount == 0)
                                {
                                    product_discount = 0;
                                }
                                else
                                {
                                    if (Percentage_OR_Price == "%")
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        product_totalAmount = Math.Round(Selling_Price, 2);
                                        product_discount = Cart_OverAllDiscount;
                                    }
                                    else
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        product_totalAmount = Math.Round(Selling_Price, 2);
                                        product_discount = Cart_OverAllDiscount;
                                    }
                                }
                                items.Insert(0, new Product()
                                {
                                    #region Display Fields

                                    Discount = product_discount,
                                    Quantity = 1,
                                    Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                                    Amount = product_totalAmount.ToString("0.00"),
                                    Percentpercentage_OR_Price = Percentage_OR_Price,
                                    Product_Name = _product_name,
                                    Iteam_Barcode = _product_barcode,

                                    #endregion Display Fields

                                    #region Hidden Fields

                                    Ad_client_id = _product_ad_client_id,
                                    Ad_org_id = _product_ad_org_id,
                                    Is_productMultiUOM = _is_productMultiUOM,
                                    Product_Arabicname = _product_arabicname,
                                    Product_category_id = _product_m_product_category_id,
                                    Product_ID = __product_id,
                                    Product_Image = _product_image,
                                    Scanby_Price = _product_scanbyprice,
                                    Scanby_Weight = _product_scanbyweight,
                                    Current_costprice = _product_currentcostprice,
                                    Sopricestd = _product_SellingPricestd,
                                    Uom_Id = _product_uomid,
                                    Uom_Name = _product_uomname

                                    #endregion Hidden Fields
                                });
                            }
                            else
                            {
                                bool result = items.Exists(x => x.Iteam_Barcode == _product_barcode);
                                if (result == true)
                                {
                                    var item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode).FirstOrDefault());
                                    var _Quantity = items[item_index].Quantity + 1;
                                    items[item_index].Quantity = Math.Round(Convert.ToDouble(_Quantity.ToString()), 2);
                                    items[item_index].Amount = (Math.Round(((Convert.ToDouble(items[item_index].Quantity)) * (Convert.ToDouble(items[item_index].Sopricestd))), 2)).ToString("0.00");
                                    if (items[item_index].Percentpercentage_OR_Price == "%")
                                    {
                                        double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                        double Discount_Amt = items[item_index].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    else
                                    {
                                        double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                        double Discount_Amt = items[item_index].Discount;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    var item_reorder = items[item_index];
                                    items.RemoveAt(item_index);
                                    items.Insert(0, item_reorder);
                                }
                                else
                                {
                                    double product_discount = 0;
                                    double product_totalAmount = Convert.ToDouble(_product_SellingPricestd.ToString());
                                    items.Insert(0, new Product()
                                    {
                                        #region Display Fields

                                        Discount = product_discount,
                                        Quantity = 1,
                                        Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                                        Amount = product_totalAmount.ToString("0.00"),
                                        Percentpercentage_OR_Price = Percentage_OR_Price,
                                        Product_Name = _product_name,
                                        Iteam_Barcode = _product_barcode,

                                        #endregion Display Fields

                                        #region Hidden Fields

                                        Ad_client_id = _product_ad_client_id,
                                        Ad_org_id = _product_ad_org_id,
                                        Is_productMultiUOM = _is_productMultiUOM,
                                        Product_Arabicname = _product_arabicname,
                                        Product_category_id = _product_m_product_category_id,
                                        Product_ID = __product_id,
                                        Product_Image = _product_image,
                                        Scanby_Price = _product_scanbyprice,
                                        Scanby_Weight = _product_scanbyweight,
                                        Current_costprice = _product_currentcostprice,
                                        Sopricestd = _product_SellingPricestd,
                                        Uom_Id = _product_uomid,
                                        Uom_Name = _product_uomname

                                        #endregion Hidden Fields
                                    });
                                }
                            }

                            ProductIteams = items;
                            DataContext = this;
                            ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                            view.Refresh();
                            Grand_Total_cart_price.Text = String.Empty;
                            addAmount = 0.00;
                            foreach (var data in items)
                            {
                                addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                            }
                            Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                            Grand_Cart_Total = addAmount;
                            productSearch_cart.Text = String.Empty;
                            SerialPort.display(SerialPort.Truncate(_product_name, 10), _product_SellingPricestd + ".00", "Total", Grand_Cart_Total.ToString() + ".00");
                        }
                        MaintainActual_AMount();
                        Product_Each_Item_Count = 0;
                        items.ToList().ForEach(x =>
                        {
                            Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                        });
                        Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                        if (invoice_number == 0 && items.Count() > 0)
                        {
                            #region Get Invoice & Doc No

                            //Checking and Getting Invoice POS Number
                            var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                            int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                            int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                            string _responce_code = Check_POS_Number_rs.Item3;
                            bool _network_status_ = Check_POS_Number_rs.Item4;
                            if (_responce_code == "0" || _responce_code == "200")
                            {
                                invoice_number = Check_POS_Number_rs.Item1;
                                document_no = Check_POS_Number_rs.Item2;
                                InvoiceNo.Text = invoice_number.ToString();
                            }
                            else if (_network_status_ != true || _responce_code == "500")
                            {
                                Error_page.Visibility = Visibility.Visible;
                                NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                                return;
                            }

                            #endregion Get Invoice & Doc No
                        }
                        iteamProduct.SelectedItem = items.FirstOrDefault();
                    }
                    if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
                    {

                        if (ValueChanger_key_pad.Text.ToString() != String.Empty)
                        {
                            if (items.Count > 0)
                            {
                                double Actual_Amount = 0.00;
                                foreach (var data in items)
                                {
                                    // if (data.Line_Discount == "N")
                                    // {
                                    Actual_Amount = Actual_Amount + (Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity);
                                    // }
                                }

                                if (LineIteam_Up_Rf == "Discount_Cart_LineIteam")
                                {
                                    items[ListView_Index_No].Discount = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                    items[ListView_Index_No].Line_Discount = "Y";
                                    items[ListView_Index_No].Percentpercentage_OR_Price = Percentage_OR_Price;
                                    items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");
                                    if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                    {
                                        if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > 100)
                                        {
                                            ValueChanger_key_pad.Clear();
                                            return;
                                        }
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    else
                                    {

                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount;
                                        if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > Total_Price)
                                        {
                                            ValueChanger_key_pad.Clear();
                                            return;
                                        }
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }

                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    ValueChanger_key_pad.Text = String.Empty;
                                    Keyboard.Focus(productSearch_cart);

                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    MaintainActual_AMount();
                                }

                                if (LineIteam_Up_Rf == "Quantity_Cart_LineIteam")
                                {
                                    if (Convert.ToDouble(ValueChanger_key_pad.Text.ToString()) == 0)
                                    {
                                        // var item = (sender as FrameworkElement).DataContext;
                                        //  int index = iteamProduct.Items.IndexOf(item);
                                        // ListView_Index_No = index;
                                        #region Customer display 
                                        double lessedamt = 0 - Convert.ToDouble(items[ListView_Index_No].Amount);
                                        string lessedSellingPrice = lessedamt.ToString("0.00");
                                        string CustDispspace1 = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                                        int TotalSpace1 = Convert.ToInt32(CustDispspace1);
                                        string strproductname1 = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                                        int productnamelen1 = strproductname1.Length;
                                        int sellingPricelen1 = lessedSellingPrice.Length;

                                        items.RemoveAt(ListView_Index_No);
                                        ICollectionView view1 = CollectionViewSource.GetDefaultView(ProductIteams);
                                        view1.Refresh();
                                        Grand_Total_cart_price.Text = String.Empty;
                                        addAmount = 0.00;
                                        foreach (var data in items)
                                        {
                                            addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                        }
                                        Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                        Grand_Cart_Total = addAmount;
                                        Product_Each_Item_Count = 0;
                                        items.ToList().ForEach(x =>
                                        {
                                            Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                        });
                                        if (items.Count() == 0)
                                        {
                                            Grand_Total_cart_price.Text = "0.00";
                                            Cart_Iteam_Count.Text = "0";
                                            Grand_Cart_Total = 0;
                                            SubToTal_Balance_Amount = 0;
                                            OverAllDiscount_tx.Text = "0";
                                            payment_method_selected = 0;
                                            Cart_OverAllDiscount = 0;
                                            Product_Each_Item_Count = 0;
                                            OverAllDiscount_button.IsEnabled = false;
                                            OrderComplected_button.IsEnabled = false;
                                            OrderCancel_button.IsEnabled = false;
                                        }
                                        Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                        Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                                        Keyboard.Focus(productSearch_cart);
                                        int totallen2 = "Total".Length;


                                        string strtotalprice2 = Grand_Cart_Total.ToString("0.00");
                                        int totalpricelen2 = strtotalprice2.Length;

                                        int space3 = 0;
                                        if (productnamelen1 > sellingPricelen1)
                                        {
                                            space3 = TotalSpace1 - productnamelen1 - sellingPricelen1;

                                        }
                                        int space4 = 0;
                                        if (productnamelen1 > sellingPricelen1)
                                        {
                                            space4 = TotalSpace1 - totallen2 - totalpricelen2;

                                        }


                                        //   int space3 = TotalSpace1 - productnamelen1 - sellingPricelen1;
                                        //  int space4 = TotalSpace1 - totallen2 - totalpricelen2;
                                        string strspace3 = new string(' ', space3);
                                        string strspace4 = new string(' ', space4);
                                        SerialPort.display(strproductname1 + strspace3, lessedSellingPrice, "Total" + strspace4, strtotalprice2);
                                        #endregion Customer display
                                        ValueChanger_key_pad.Text = "";
                                        MaintainActual_AMount();
                                        return;
                                    }
                                    else
                                    {
                                        items[ListView_Index_No].Quantity = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                        items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");

                                        if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                        {

                                            double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                            double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                            double Selling_Price = Total_Price - Discount_Amt;

                                            items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                        }
                                        else
                                        {
                                            double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                            double Discount_Amt = items[ListView_Index_No].Discount;
                                            double Selling_Price = Total_Price - Discount_Amt;

                                            items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                        }
                                        ProductIteams = items;
                                        ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                        view.Refresh();
                                        ValueChanger_key_pad.Text = String.Empty;
                                        addAmount = 0.00;
                                        foreach (var data in items)
                                        {
                                            addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                        }
                                        Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                        Grand_Cart_Total = addAmount;
                                        ValueChanger_key_pad.Text = String.Empty;
                                        Keyboard.Focus(productSearch_cart);

                                        Product_Each_Item_Count = 0;
                                        items.ToList().ForEach(x =>
                                        {
                                            Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                        });
                                        Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                        MaintainActual_AMount();
                                    }
                                }

                                if (LineIteam_Up_Rf == "OverAllDiscount")
                                {
                                    Cart_OverAllDiscount = Convert.ToDouble(ValueChanger_key_pad.Text);
                                    double _Cart_OverAllDiscount = Convert.ToDouble(ValueChanger_key_pad.Text);

                                    bool Exit = false;
                                    double TotalDiscountApplied = 0;
                                    double Balance_if_any = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        // if (x.Line_Discount == "N")
                                        // {
                                        x.Percentpercentage_OR_Price = Percentage_OR_Price;
                                        x.Amount = (Math.Round(((Convert.ToDouble(x.Quantity)) * (Convert.ToDouble(x.Sopricestd))), 2)).ToString("0.00");

                                        if (x.Percentpercentage_OR_Price == "%")
                                        {
                                            if (_Cart_OverAllDiscount > 100)
                                            {
                                                ValueChanger_key_pad.Clear();
                                                Exit = true;
                                            }
                                            if (!Exit)
                                            {
                                                x.Discount = Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2);
                                                double Total_Price = Convert.ToDouble(x.Amount);
                                                double Discount_Amt = x.Discount / 100 * Total_Price;
                                                double Selling_Price = Total_Price - Discount_Amt;

                                                x.Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                            }

                                        }
                                        else
                                        {

                                            if (_Cart_OverAllDiscount > Actual_Amount)
                                            {
                                                ValueChanger_key_pad.Clear();
                                                Exit = true;
                                            }
                                            //double Selling_Price = Total_Price - Discount_Amt;
                                            if (!Exit)
                                            {
                                                double Total_Price = Convert.ToDouble(x.Amount);
                                                double Discount_Amt = x.Discount;
                                                double Selling_Price = Total_Price;
                                                double Eachpercentgediscount = 0;
                                                //if (_Cart_OverAllDiscount >= Total_Price)
                                                //{
                                                //    Selling_Price = 0;
                                                //    _Cart_OverAllDiscount -= Total_Price;
                                                //    x.Discount = Math.Round(Total_Price, 2);
                                                //}
                                                //else
                                                //{
                                                Total_Price = Convert.ToDouble(x.Amount);
                                                double EachproductPercent = ((Convert.ToDouble(x.Sopricestd) * Convert.ToDouble(x.Quantity)) / Actual_Amount) * 100;
                                                Eachpercentgediscount = (_Cart_OverAllDiscount * EachproductPercent) / 100;
                                                x.Discount = Math.Round(Convert.ToDouble(Eachpercentgediscount.ToString()), 2);
                                                TotalDiscountApplied += x.Discount;

                                                // Selling_Price = Total_Price - _Cart_OverAllDiscount;
                                                //  x.Discount = Math.Round(_Cart_OverAllDiscount, 2);
                                                // _Cart_OverAllDiscount -= _Cart_OverAllDiscount;
                                                //_Cart_OverAllDiscount =
                                                //}

                                                x.Amount = (Math.Round(Total_Price - Eachpercentgediscount, 2)).ToString("0.00");
                                                //Adjust remaing discount if any after calculationequal discount incase of decimals
                                                Balance_if_any = Cart_OverAllDiscount - TotalDiscountApplied;
                                            }
                                        }
                                        // }
                                    });
                                    if (Exit)
                                    {
                                        return;
                                    }
                                    _Cart_OverAllDiscount = 0;
                                    List<Product> tempview = items;
                                    string max_amount = items.Select(i => i.Sopricestd).Max();
                                    int Indexval = items.IndexOf(items.Where(x => x.Sopricestd == max_amount).FirstOrDefault());
                                    items[Indexval].Discount += Balance_if_any;
                                    //----
                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    OverAllDiscount_tx.Text = Convert.ToDouble(ValueChanger_key_pad.Text).ToString("0.00");
                                    OverAllDiscount_sy.Text = Percentage_OR_Price;
                                    ValueChanger_key_pad.Text = String.Empty;
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    ValueChanger_key_pad.Text = String.Empty;
                                    Keyboard.Focus(productSearch_cart);
                                    MaintainActual_AMount();
                                }
                                if (LineIteam_Up_Rf == "Return_Price_Cart_LineIteam")
                                {
                                    //double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount); 
                                    //if (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2) > Total_Price)
                                    //{
                                    //    ValueChanger_key_pad.Clear();
                                    //    return;
                                    //}

                                    items[ListView_Index_No].Amount = (Math.Round(Convert.ToDouble(ValueChanger_key_pad.Text.ToString()), 2)).ToString("0.00");


                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    ValueChanger_key_pad.Text = String.Empty;
                                    Keyboard.Focus(productSearch_cart);

                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    MaintainActual_AMount();
                                    Pmt_Return_Click(sender, e);
                                }
                                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                                GeneralTextPopUp.Visibility = Visibility.Hidden;
                                popUpText.Visibility = Visibility.Hidden;
                                #region Customer display 
                                string SellingPrice = Convert.ToDecimal(items[ListView_Index_No].Amount).ToString("0.00");
                                string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                                int TotalSpace = Convert.ToInt32(CustDispspace);
                                string strproductname = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                                int productnamelen = strproductname.Length;
                                int sellingPricelen = SellingPrice.Length;
                                int totallen = "Total".Length;
                                string strtotalprice = Grand_Cart_Total.ToString("0.00");
                                int totalpricelen = strtotalprice.Length;

                                int space1 = 0;
                                if (productnamelen > sellingPricelen)
                                {
                                    space1 = TotalSpace - productnamelen - sellingPricelen;

                                }
                                int space2 = 0;
                                if (totallen > totalpricelen)
                                {
                                    space2 = TotalSpace - totallen - totalpricelen;

                                }



                                // int space1 = TotalSpace - productnamelen - sellingPricelen;
                                // int space2 = TotalSpace - totallen - totalpricelen;
                                string strspace1 = new string(' ', space1);
                                string strspace2 = new string(' ', space2);
                                SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                                #endregion Customer display 
                            }
                            else
                            {
                                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                                GeneralTextPopUp.Visibility = Visibility.Hidden;
                                popUpText.Visibility = Visibility.Hidden;
                                ValueChanger_key_pad.Clear();
                                Back_OR_Esc();
                                return;

                            }

                        }
                        else
                        {
                            ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                            ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                            GeneralTextPopUp.Visibility = Visibility.Hidden;
                            popUpText.Visibility = Visibility.Hidden;
                            ValueChanger_key_pad.Text = String.Empty;
                            Keyboard.Focus(productSearch_cart);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void quick_ValueChanger_key_pad_keyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Enter)
                {

                    if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                    {
                        if (quick_ValueChanger_key_pad.Text != "")
                            Add_Product_toCart_From_Quick();
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        //private void BarcodeSearch_cart_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    BarcodeSearch_cart.Select(BarcodeSearch_cart.Text.Length, 0);
        //}

        private void ValueChanger_key_pad_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValueChanger_key_pad.Select(ValueChanger_key_pad.Text.Length, 0);
        }
        private void quick_ValueChanger_key_pad_TextChanged(object sender, TextChangedEventArgs e)
        {
            quick_ValueChanger_key_pad.Select(quick_ValueChanger_key_pad.Text.Length, 0);
        }
        #endregion Product Barcode Search Bar

        #region List View for Product Iteams

        private void IteamProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object index = e.Source;
            int val = ((System.Windows.Controls.Primitives.Selector)index).SelectedIndex;
            ListView_Index_No = val;
        }

        private void DeleteCart_LineIteam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (sender as FrameworkElement).DataContext;
                int index = iteamProduct.Items.IndexOf(item);
                ListView_Index_No = index;
                #region Customer display 
                double lessedamt = 0 - Convert.ToDouble(items[ListView_Index_No].Amount);
                string lessedSellingPrice = lessedamt.ToString("0.00");
                string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                int TotalSpace = Convert.ToInt32(CustDispspace);
                string strproductname = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                int productnamelen = strproductname.Length;
                int sellingPricelen = lessedSellingPrice.Length;

                items.RemoveAt(ListView_Index_No);
                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                view.Refresh();
                Grand_Total_cart_price.Text = String.Empty;
                addAmount = 0.00;
                foreach (var data in items)
                {
                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                }
                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                Grand_Cart_Total = addAmount;
                Product_Each_Item_Count = 0;
                items.ToList().ForEach(x =>
                {
                    Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                });
                if (items.Count() == 0)
                {
                    Grand_Total_cart_price.Text = "0.00";
                    Cart_Iteam_Count.Text = "0";
                    Grand_Cart_Total = 0;
                    SubToTal_Balance_Amount = 0;
                    OverAllDiscount_tx.Text = "0";
                    payment_method_selected = 0;
                    Cart_OverAllDiscount = 0;
                    Product_Each_Item_Count = 0;
                    OverAllDiscount_button.IsEnabled = false;
                    OrderComplected_button.IsEnabled = false;
                    OrderCancel_button.IsEnabled = false;
                }
                MaintainActual_AMount();

                Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                Keyboard.Focus(productSearch_cart);
                int totallen = "Total".Length;


                string strtotalprice = Grand_Cart_Total.ToString("0.00");
                int totalpricelen = strtotalprice.Length;

                int space1 = 0;
                if (productnamelen > sellingPricelen)
                {
                    space1 = TotalSpace - productnamelen - sellingPricelen;

                }
                int space2 = 0;
                if (totallen > totalpricelen)
                {
                    space2 = TotalSpace - totallen - totalpricelen;

                }


                // int space1 = TotalSpace - productnamelen - sellingPricelen;
                // int space2 = TotalSpace - totallen - totalpricelen;

                // int space1 = TotalSpace - productnamelen - sellingPricelen;
                //int space2 = TotalSpace - totallen - totalpricelen;
                string strspace1 = new string(' ', space1);
                string strspace2 = new string(' ', space2);
                SerialPort.display(strproductname + strspace1, lessedSellingPrice, "Total" + strspace2, strtotalprice);
                #endregion Customer display
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Discount_Cart_LineIteam_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var item = (sender as Button).DataContext;
                int index = iteamProduct.Items.IndexOf(item);
                ListView_Index_No = index;
                iteamProduct.SelectedIndex = ListView_Index_No;
                Button button = (Button)sender;
                StackPanel StackPanel = (StackPanel)button.Content;
                TextBox _TextBlock = new TextBox();
                foreach (var child in StackPanel.Children)
                {
                    if (child.GetType().ToString() == "System.Windows.Controls.TextBox")
                    {
                        _TextBlock = (TextBox)child;
                        if (_TextBlock.Text != "")
                        {
                            ValueChanger_key_pad.Text = _TextBlock.Text;
                        }
                    }

                }

                if (Percentage_OR_Price == "%")
                {

                    LineIteam_Up_Rf = "Discount_Cart_LineIteam";

                    ValueChanger_key_pad.Focusable = true;
                    Keyboard.Focus(ValueChanger_key_pad);

                    iteamProduct.SelectedIndex = ListView_Index_No;

                    popUpText.Visibility = Visibility.Visible;
                    popUpText.Text = "Line Discount";
                    ToggleButton_for_DisAndPrice.Visibility = Visibility.Visible;
                    ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                }
                else
                {
                    //if (Convert.ToDouble(OverAllDiscount_tx.Text) > 0)
                    //{
                    //    if (MessageBox.Show("OverAll Discount Need To Clear?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    //    {
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        var item = (sender as FrameworkElement).DataContext;
                    //        int index = iteamProduct.Items.IndexOf(item);
                    //        ListView_Index_No = index;

                    //        LineIteam_Up_Rf = "Discount_Cart_LineIteam";

                    //        ValueChanger_key_pad.Focusable = true;
                    //        Keyboard.Focus(ValueChanger_key_pad);

                    //        iteamProduct.SelectedIndex = ListView_Index_No;

                    //        popUpText.Visibility = Visibility.Visible;
                    //        popUpText.Text = "Line Discount";
                    //        Cart_OverAllDiscount = Convert.ToDouble(0);
                    //        double _Cart_OverAllDiscount = Convert.ToDouble(0);
                    //        double Actual_Amount = 0.00;
                    //        foreach (var data in items)
                    //        {
                    //            Actual_Amount = Actual_Amount + Convert.ToDouble(data.Sopricestd.ToString());
                    //        }

                    //        items = Calculate_OVerAllDiscount(items, Actual_Amount, _Cart_OverAllDiscount);
                    //        _Cart_OverAllDiscount = 0;
                    //        ProductIteams = items;
                    //        ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                    //        view.Refresh();
                    //        OverAllDiscount_tx.Text = "0";
                    //        OverAllDiscount_sy.Text = Percentage_OR_Price;

                    //        addAmount = 0.00;
                    //        foreach (var data in items)
                    //        {
                    //            addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                    //        }
                    //        Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                    //        Grand_Cart_Total = addAmount;
                    //        ValueChanger_key_pad.Text = String.Empty;
                    //        ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                    //        ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                    //        #region Customer display 
                    //        string SellingPrice = Convert.ToDouble(items[ListView_Index_No].Amount).ToString("0.00");
                    //        string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                    //        int TotalSpace = Convert.ToInt32(CustDispspace);
                    //        string strproductname = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                    //        int productnamelen = strproductname.Length;
                    //        int sellingPricelen = SellingPrice.Length;
                    //        int totallen = "Total".Length;
                    //        string strtotalprice = Grand_Cart_Total.ToString("0.00");
                    //        int totalpricelen = strtotalprice.Length;

                    //        int space1 = TotalSpace - productnamelen - sellingPricelen;
                    //        int space2 = TotalSpace - totallen - totalpricelen;
                    //        string strspace1 = new string(' ', space1);
                    //        string strspace2 = new string(' ', space2);
                    //        SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                    //        #endregion Customer display
                    //    }
                    //}
                    //else
                    //{
                    LineIteam_Up_Rf = "Discount_Cart_LineIteam";

                    ValueChanger_key_pad.Focusable = true;
                    Keyboard.Focus(ValueChanger_key_pad);

                    iteamProduct.SelectedIndex = ListView_Index_No;
                    ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Discount.ToString();
                    ValueChanger_key_pad.SelectAll();
                    _selectedtext_valuecharger = true;
                    popUpText.Visibility = Visibility.Visible;
                    popUpText.Text = "Line Discount";
                    ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                    ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                    // }

                }
                GeneralTextPopUp.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Quantity_Cart_LineIteam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (sender as FrameworkElement).DataContext;
                int index = iteamProduct.Items.IndexOf(item);
                ListView_Index_No = index;

                LineIteam_Up_Rf = "Quantity_Cart_LineIteam";
                ValueChanger_key_pad.Focusable = true;
                Keyboard.Focus(ValueChanger_key_pad);
                iteamProduct.SelectedIndex = ListView_Index_No;
                Button button = (Button)sender;
                StackPanel StackPanel = (StackPanel)button.Content;
                TextBox _TextBlock = new TextBox();
                foreach (var child in StackPanel.Children)
                {
                    if (child.GetType().ToString() == "System.Windows.Controls.TextBox")
                    {
                        _TextBlock = (TextBox)child;
                    }

                }

                ValueChanger_key_pad.Text = _TextBlock.Text;
                ValueChanger_key_pad.SelectAll();
                _selectedtext_valuecharger = true;
                GeneralTextPopUp.Visibility = Visibility.Visible;
                GeneralTextPopUp.Text = "Enter the Quantity ";
                popUpText.Visibility = Visibility.Hidden;
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void OverAllDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                overAllDiscount();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void overAllDiscount()
        {
            LineIteam_Up_Rf = "OverAllDiscount";

            ValueChanger_key_pad.Focusable = true;
            Keyboard.Focus(ValueChanger_key_pad);
            popUpText.Visibility = Visibility.Visible;
            popUpText.Text = "Over All Discount";
            if (Percentage_OR_Price == "%")
            {
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Visible;
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
            }
            else
            {
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;

            }

            GeneralTextPopUp.Visibility = Visibility.Hidden;
        }
        private void ValueFinderINPercentage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Visible;
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                OverAllDiscount_sy.Text = "%";

                ValueChanger_key_pad.Focusable = true;
                Keyboard.Focus(ValueChanger_key_pad);
                Percentage_OR_Price = "%";
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void ValueFinderINQA_Click(object sender, RoutedEventArgs e)
        {
            try

            {
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                OverAllDiscount_sy.Text = AD_CurrencyCode;

                ValueChanger_key_pad.Focusable = true;
                Keyboard.Focus(ValueChanger_key_pad);
                Percentage_OR_Price = AD_CurrencyCode;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion List View for Product Iteams

        #region Validations

        /// <summary>
        /// Allowes Number with Decimal Points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_dot_ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);

            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");//allow decimal points
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Allowes Only Numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);

            var regex = new Regex("^[0-9]*$");//only numbers
            if (regex.IsMatch(e.Text))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void Number_Card_ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");//only numbers
                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;
                else
                {
                    string card_val = "0.00";
                    switch (payment_method_selected)
                    {
                        case 2:
                            card_val = Payment_Card_Only_tx.Text;
                            if (card_val == String.Empty)
                            {
                                card_val = "0.00";
                            }
                            break;

                        case 5:
                            card_val = Split_Payment_Card_tx.Text;
                            if (card_val == String.Empty)
                            {
                                card_val = "0.00";
                            }
                            break;

                        default:
                            break;
                    }
                    bool v = card_val.All(char.IsNumber);
                    if (v)
                    {
                        double validate = Convert.ToDouble(card_val);
                        if (validate <= Grand_Cart_Total)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Validations

        #region Shortcut Keys

        private void Retail_Cart_KeyDown(object sender, KeyEventArgs e)
        {


        }

        #endregion Shortcut Keys

        #region Payment End Process |Cancel|Hold|Complected

        #region Cancel

        private void OrderCancel_Click(object sender, RoutedEventArgs e)
        {
            SessionHead.Visibility = Visibility.Visible;
            SessionHead.Text = "Cancel Order Alert";
            SessionDescription.Text = "Are You Sure To Cancel?";
            SessionResume.Visibility = Visibility.Hidden;
            SessionClose.Visibility = Visibility.Hidden;
            SessionCreateNew.Visibility = Visibility.Hidden;
            txt_openingBal.Visibility = Visibility.Hidden;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;

            Session_Check.Visibility = Visibility.Visible;
            Error_page.Visibility = Visibility.Visible;
            CancelYes.Visibility = Visibility.Visible;
            CancelNo.Visibility = Visibility.Visible;



        }

        public void Sale_Cancel(string reason)
        {

            if (invoice_number == 0)
            {
                Keyboard.Focus(productSearch_cart);
                return;
            }
            Store_Cart_Items_Locally("Y", "N", "N", reason);
            GetOrderHold_Count();
            Cart_Hold_Count.Text = ProductHold_list_items.Count().ToString();
            items.Clear();
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
            view.Refresh();
            OrderComplected_button.IsEnabled = false;
            OverAllDiscount_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
            Grand_Total_cart_price.Text = "0.00";
            Cart_Iteam_Count.Text = "0";
            Grand_Cart_Total = 0;
            SubToTal_Balance_Amount = 0;
            OverAllDiscount_tx.Text = "0";
            Change_Activity_of_Payment_buttons("null");
            payment_method_selected = 0;
            Cart_OverAllDiscount = 0;
            Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
            BarcodeSearch_bar.Visibility = Visibility.Visible;
            productSearch_cart.Visibility = Visibility.Visible;
            Payment_Screen.Visibility = Visibility.Hidden;
            iteamProduct.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            Keyboard.Focus(productSearch_cart);
            invoice_number = 0;
            document_no = 0;
            Product_Each_Item_Count = 0;
            InvoiceNo.Text = String.Empty;
            MaintainActual_AMount();
        }

        #endregion Cancel

        #region Complete

        private void OrderComplected_Click(object sender, RoutedEventArgs e)
        {
            Sale_complete();
        }

        public void Sale_complete()
        {

            string name_id = "";
            string reason = "";
            string mobile_number = "";
            double cash = 0;
            double card = 0;
            double redemption = 0;
            double exchange = 0;
            string iscomplementary = "N";
            string iscredit = "N";
            if (payment_method_selected == 0)
            {
                _Payment_Cash();
                return;
            }
            if (payment_method_selected == 1)
            {
                if (Payment_Cash_Only_tx.Text != String.Empty)
                {
                    cash = Convert.ToDouble(Payment_Cash_Only_tx.Text);
                }
                else
                {
                    MessageBox.Show("Cash Can't Be Empty");
                    Payment_Cash_Only_tx.Focus();
                    return;
                }

            }
            if (payment_method_selected == 2)
            {
                if (Payment_Card_Only_tx.Text != String.Empty)
                {
                    card = Convert.ToDouble(Payment_Card_Only_tx.Text);
                }
                else
                {
                    MessageBox.Show("Card Value Can't Be Empty");
                    Payment_Card_Only_tx.Focus();
                    return;
                }

            }
            if (payment_method_selected == 3)
            {

                if (Payment_Complementary_Name_tx.Text != String.Empty)
                {
                    name_id = Payment_Complementary_Name_tx.Text;
                }
                else
                {

                    MessageBox.Show("Name Can't Be Empty");
                    Payment_Complementary_Name_tx.Focus();
                    return;
                }

                if (Payment_Complementary_Mobile_No_tx.Text != String.Empty)
                {
                    mobile_number = Payment_Complementary_Mobile_No_tx.Text;
                }
                else
                {
                    MessageBox.Show("Mobile Number Can't Be Empty");
                    Payment_Complementary_Mobile_No_tx.Focus();
                    return;
                }


                reason = Complementary_Reason_tx.Text;
                iscomplementary = "Y";
                cash = Grand_Cart_Total;
            }
            if (payment_method_selected == 4)
            {

                if (Payment_Credit_Name_tx.Text != String.Empty)
                {
                    name_id = Payment_Credit_Name_tx.Text;
                }
                else
                {
                    MessageBox.Show("Name Can't Be Empty");
                    Payment_Credit_Name_tx.Focus();
                    return;
                }

                if (Payment_Credit_Mobile_No_tx.Text != String.Empty)
                {
                    mobile_number = Payment_Credit_Mobile_No_tx.Text;
                }
                else
                {
                    MessageBox.Show("Mobile Number Can't Be Empty");
                    Payment_Credit_Mobile_No_tx.Focus();
                    return;
                }
                iscredit = "Y";
                cash = Grand_Cart_Total;
            }
            if (payment_method_selected == 5)
            {
                if (Split_Payment_Exchange_tx.Text != String.Empty)
                    exchange = Convert.ToDouble(Split_Payment_Exchange_tx.Text);
                if (Split_Payment_GiftCash_tx.Text != String.Empty)
                    redemption = Convert.ToDouble(Split_Payment_GiftCash_tx.Text);
                if (Split_Payment_Cash_tx.Text != String.Empty)
                    cash = Convert.ToDouble(Split_Payment_Cash_tx.Text);
                if (Split_Payment_Card_tx.Text != String.Empty)
                    card = Convert.ToDouble(Split_Payment_Card_tx.Text);
            }
            if (payment_method_selected == 6)
            {
                if (items.Count() < 1)
                    return;

                Change_Activity_of_Payment_buttons("Return");
                cash = Convert.ToDouble(Grand_Total_cart_price.Text);
                #region Customer display 
                string SellingPrice = "";
                string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                int TotalSpace = Convert.ToInt32(CustDispspace);
                string strproductname = "Thank you..!!";
                int productnamelen = strproductname.Length;
                int sellingPricelen = SellingPrice.Length;
                int totallen = "Total".Length;
                string strtotalprice = Grand_Cart_Total.ToString("0.00");
                int totalpricelen = strtotalprice.Length;

                int space1 = 0;
                if (productnamelen > sellingPricelen)
                {
                    space1 = TotalSpace - productnamelen - sellingPricelen;

                }
                int space2 = 0;
                if (totallen > totalpricelen)
                {
                    space2 = TotalSpace - totallen - totalpricelen;

                }

                string strspace1 = new string(' ', space1);
                string strspace2 = new string(' ', space2);
                SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                #endregion Customer display

                MaintainActual_AMount();

                Keyboard.Focus(Payment_Cash_Only_tx);
            }
            string[] Grand_Cart_Total_round_off = Grand_Cart_Total.ToString("0.00").Split('.');
            int _Grand_Cart_Total_round_off = int.Parse(Grand_Cart_Total_round_off[0]);
            if ((cash + card + redemption + exchange) < _Grand_Cart_Total_round_off)
            {
                MessageBox.Show("Insufficient Funds To Processed                              ", "Payment");
                return;
            }
            //load_Process();
            Store_Cart_Items_Locally("N", "N", "Y", "");
            GetOrderHold_Count();
            Cart_Hold_Count.Text = ProductHold_list_items.Count().ToString();
            isDuplicateCopy = Side_menu_Duplicate_Copy.IsChecked.Equals(true);
            #region Invoice Payment Details

            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            connection.Open();

            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'c_invoicepaymentdetails';", connection);
            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

            if (_get__Ad_sequenc_no.Read())
            {
                _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
            }
            connection.Close();

            connection.Open();
            NpgsqlCommand INSERT_c_invoicepaymentdetails = new NpgsqlCommand("INSERT INTO c_invoicepaymentdetails(" +
            "c_invoicepaymentdetails_id, ad_client_id, ad_org_id, c_invoice_id," +
            "cash, card, exchange, redemption, iscomplementary, iscredit, createdby, updatedby,name_id,mobile_numbler,reason)" +
            "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + invoice_number + "," + cash + "," + card + "," + exchange + "," + redemption + ",'" + iscomplementary + "','" + iscredit + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + name_id + "','" + mobile_number + "','" + reason + "'); ", connection);
            INSERT_c_invoicepaymentdetails.ExecuteNonQuery();
            connection.Close();

            connection.Open();
            NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'c_invoicepaymentdetails';", connection);
            NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
            connection.Close();

            #endregion Invoice Payment Details
            //--------------------------------------------------------------------------------------------------------------------------------//
            //-----------------------------                          PRINTING                                 --------------------------------//
            /*-----------------------------*/
            RetailViewModel.Get_invoice_And_print(invoice_number.ToString(), isDuplicateCopy);/*------------------------------*/
            //--------------------------------------------------------------------------------------------------------------------------------//

            if (isDuplicateCopy)
            {
                //--------------------------------------------------------------------------------------------------------------------------------//
                //---------------------CompleteSessionClose_Click--------                       Duplicate Copy PRINTING                     --------------------------------//
                /*--------------------*/
                RetailViewModel.Get_invoice_And_print(invoice_number.ToString(), isDuplicateCopy);/*------------------------*/
                //--------------------------------------------------------------------------------------------------------------------------------//
            }

            items.Clear();
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
            view.Refresh();
            OrderComplected_button.IsEnabled = false;
            OverAllDiscount_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
            Grand_Total_cart_price.Text = "0.00";
            Cart_Iteam_Count.Text = "0";
            Grand_Cart_Total = 0;
            SubToTal_Balance_Amount = 0;
            OverAllDiscount_tx.Text = "0";
            Change_Activity_of_Payment_buttons("null");
            payment_method_selected = 0;
            Cart_OverAllDiscount = 0;
            Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
            BarcodeSearch_bar.Visibility = Visibility.Visible;
            productSearch_cart.Visibility = Visibility.Visible;
            Payment_Screen.Visibility = Visibility.Hidden;
            iteamProduct.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";

            invoice_number = 0;
            document_no = 0;
            Product_Each_Item_Count = 0;
            InvoiceNo.Text = String.Empty;
            MaintainActual_AMount();
            // Thread.Sleep(5000);
            SerialPort.display(AD_ORG_name, "", "                       ", "QSale.Qa");
            // unLoad_Process();
            productSearch_cart.Focus();
            Keyboard.Focus(productSearch_cart);
            Back_OR_Esc();
            // unLoad_Process();
        }

        #endregion Complete

        #region Hold

        private void OrderHold_Click(object sender, RoutedEventArgs e)
        {
            Sale_hold();
        }

        public void Sale_hold()
        {
            Cart_Main_Page.Visibility = Visibility.Hidden;
            Cart_Hold_Page.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "Cart_Hold_Check_keyboard_Focus";
            OrderCancel_button.IsEnabled = false;
            OrderComplected_button.IsEnabled = false;
            //await Task.Run(() =>
            //{
            GetOrderHold_Count();
            //});
            if (Hold_iteamProduct.Items.Count == 0)
            {
                New_Sale.Focus();
                Keyboard.Focus(New_Sale);
            }
            else
            {
                Hold_iteamProduct.SelectedIndex = 0;
                Hold_iteamProduct.Focus();

            }

            //this.Dispatcher.Invoke(() =>
            //{
            Cart_Hold_Count.Text = ProductHold_list_items.Count().ToString();
            //Hold_iteamProduct.ItemsSource = ProductHold_list_items;

            //});
            Hold_iteamProduct.SelectedItem = ProductHold_list_items.FirstOrDefault();
            Keyboard.Focus(Hold_iteamProduct);
        }

        public int Hold_iteamProduct_SelectedIndex;

        /// <summary>
        /// One click of the holded items , take all selected items to the cart page If cart has
        /// existing item then move it to hold. Clear the cart items list. Add the Hold Item list to
        /// the cart. Reset All the Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hold_iteamProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object index = e.Source;
            int val = ((System.Windows.Controls.Primitives.Selector)index).SelectedIndex;
            Hold_iteamProduct_SelectedIndex = val;
        }

        /// <summary>
        ///  Open The Hold Sale Invoiced on MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hold_iteamProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Hold_Invoice_to_Cart_Main_Page();
        }

        private void Hold_iteamProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Hold_Invoice_to_Cart_Main_Page();
            }
        }

        public void Hold_Invoice_to_Cart_Main_Page()
        {
            if (ProductHold_list_items.Count() == 0)
            {
                return;
            }

            if (MessageBox.Show("Do You Want To Open The Hold Sale Invoiced(#" + ProductHold_list_items[Hold_iteamProduct_SelectedIndex].C_invoice_id + ")?",
                        "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (invoice_number != 0 && items.Count() > 0)
                {
                    New_Sale_hold();
                }

                var selected_hold_items = ProductHold_list_items[Hold_iteamProduct_SelectedIndex];

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_c_invoic_on_hold = new NpgsqlCommand("SELECT " +
                    "t1.c_invoice_id," +                //0
                    "t2.m_product_id," +                //1
                    "t2.productname," +                 //2
                    "t2.paroductarabicname," +          //3
                    "t2.productbarcode," +              //4
                    "t2.c_uom_id," +                    //5
                    "t2.uomname," +                     //6
                    "t2.qtyinvoiced," +                 //7
                    "t2.qtyentered," +                  //8
                    "t2.saleprice," +                   //9
                    "t2.costprice," +                   //10
                    "t2.discounttype," +                //11
                    "t2.discountvalue," +               //12
                    "t2.linetotalamt," +                //13
                    "t2.pricelistid," +                 //14
                    "t2.islinediscounted," +            //15
                    "t3.m_product_category_id, " +      //16
                    "t3.image, " +                      //17
                    "t3.scanbyweight, " +               //18
                    "t3.scanbyprice, " +                 //19
                    "t3.attribute1 as Is_productMultiUOM " + //20
                    "FROM c_invoice t1 ,c_invoiceline t2 , m_product t3 " +
                    "WHERE t1.is_onhold = 'Y'" +
                    "AND t1.ad_client_id = " + AD_Client_ID + " " +
                    "AND t1.ad_org_id = " + AD_ORG_ID + " " +
                    "AND t1.ad_user_id = " + AD_USER_ID + " " +
                    "AND t1.c_invoice_id = t2.c_invoice_id AND t2.m_product_id = t3.m_product_id " +
                    "AND t1.c_invoice_id =  " + selected_hold_items.C_invoice_id + " ;", connection);
                NpgsqlDataReader _get_c_invoic_on_hold = cmd_c_invoic_on_hold.ExecuteReader();

                while (_get_c_invoic_on_hold.Read())
                {
                    long discount_type = _get_c_invoic_on_hold.GetInt64(11);
                    string Percentage_OR_Price_line;
                    if (discount_type == 0)
                    { Percentage_OR_Price_line = "%"; }
                    else
                    { Percentage_OR_Price_line = AD_CurrencyCode; }
                    items.Add(new Product()
                    {
                        Product_Name = _get_c_invoic_on_hold.GetString(2),
                        Product_ID = _get_c_invoic_on_hold.GetDouble(1).ToString(),
                        Discount = _get_c_invoic_on_hold.GetDouble(12),
                        Quantity = _get_c_invoic_on_hold.GetDouble(8),
                        Price = _get_c_invoic_on_hold.GetDouble(9).ToString("0.00"),
                        Amount = _get_c_invoic_on_hold.GetDouble(13).ToString("0.00"),
                        Iteam_Barcode = _get_c_invoic_on_hold.GetString(4),
                        Percentpercentage_OR_Price = Percentage_OR_Price_line,
                        Ad_client_id = AD_Client_ID.ToString(),
                        Ad_org_id = AD_ORG_ID.ToString(),
                        Product_category_id = _get_c_invoic_on_hold.GetDouble(16).ToString(),
                        Product_Arabicname = _get_c_invoic_on_hold.GetString(3),
                        Product_Image = _get_c_invoic_on_hold.GetString(17),
                        Scanby_Weight = _get_c_invoic_on_hold.GetString(18),
                        Scanby_Price = _get_c_invoic_on_hold.GetString(19),
                        Uom_Id = _get_c_invoic_on_hold.GetDouble(5).ToString(),
                        Uom_Name = _get_c_invoic_on_hold.GetString(6),
                        Sopricestd = _get_c_invoic_on_hold.GetDouble(9).ToString(),
                        Current_costprice = _get_c_invoic_on_hold.GetDouble(10).ToString(),
                        Is_productMultiUOM = _get_c_invoic_on_hold.GetString(20),
                        Line_Discount = _get_c_invoic_on_hold.GetString(15)
                    });
                }

                connection.Close();
                ProductIteams = items;
                iteamProduct.ItemsSource = items;
                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                view.Refresh();
                OrderComplected_button.IsEnabled = true;
                OverAllDiscount_button.IsEnabled = true;
                OrderCancel_button.IsEnabled = true;
                Grand_Total_cart_price.Text = selected_hold_items.Grandtotal;
                Cart_Iteam_Count.Text = selected_hold_items.Total_items_count;
                Grand_Cart_Total = Convert.ToDouble(selected_hold_items.Grandtotal);
                SubToTal_Balance_Amount = Convert.ToDouble(selected_hold_items.Grandtotal);
                int discounttype = Convert.ToInt32(selected_hold_items.Discounttype);
                if (discounttype == 0)
                { Percentage_OR_Price = "%"; }
                else
                { Percentage_OR_Price = AD_CurrencyCode; }
                OverAllDiscount_tx.Text = Convert.ToDouble(selected_hold_items.Discountvalue).ToString("0.00");
                payment_method_selected = 0;
                invoice_number = Convert.ToInt32(selected_hold_items.C_invoice_id);
                document_no = Convert.ToInt32(selected_hold_items.Documentno);
                InvoiceNo.Text = selected_hold_items.C_invoice_id;
                Product_Each_Item_Count = Convert.ToInt32(selected_hold_items.Total_items_count);
                Cart_OverAllDiscount = Convert.ToDouble(selected_hold_items.Discountvalue);

                Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
                BarcodeSearch_bar.Visibility = Visibility.Visible;
                productSearch_cart.Visibility = Visibility.Visible;
                Payment_Screen.Visibility = Visibility.Hidden;
                iteamProduct.Visibility = Visibility.Visible;
                Cart_Hold_Page.Visibility = Visibility.Hidden;
                Cart_Main_Page.Visibility = Visibility.Visible;
                Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                Keyboard.Focus(productSearch_cart);
                MaintainActual_AMount();
                //ProductHold_list_items.RemoveAt(Hold_iteamProduct_SelectedIndex);
                //ICollectionView ProductHold_list_items_view = CollectionViewSource.GetDefaultView(ProductHold_list);
                //ProductHold_list_items_view.Refresh();
            }
            else
            {
                // Do not close the window
            }
        }

        /// <summary>
        /// Creates a New Sale or Addes all items in the cart to hold
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Sale_Click(object sender, RoutedEventArgs e)
        {
            //await Task.Run(() =>
            //{
            New_Sale_hold();

            //});
        }

        public void New_Sale_hold()
        {
            if (invoice_number == 0)
            {
                //this.Dispatcher.Invoke(() =>
                //{
                Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
                BarcodeSearch_bar.Visibility = Visibility.Visible;
                productSearch_cart.Visibility = Visibility.Visible;
                Payment_Screen.Visibility = Visibility.Hidden;
                iteamProduct.Visibility = Visibility.Visible;
                Cart_Hold_Page.Visibility = Visibility.Hidden;
                Cart_Main_Page.Visibility = Visibility.Visible;
                Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                Keyboard.Focus(productSearch_cart);
                //});
                return;
            }

            Store_Cart_Items_Locally("N", "Y", "N", "hold");
            GetOrderHold_Count();

            //this.Dispatcher.Invoke(() =>
            //{
            Cart_Hold_Count.Text = ProductHold_list_items.Count().ToString();

            items.Clear();
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
            view.Refresh();
            OrderComplected_button.IsEnabled = false;
            OverAllDiscount_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
            Grand_Total_cart_price.Text = "0.00";
            Cart_Iteam_Count.Text = "0";
            Grand_Cart_Total = 0;
            SubToTal_Balance_Amount = 0;
            OverAllDiscount_tx.Text = "0";
            Change_Activity_of_Payment_buttons("null");
            payment_method_selected = 0;
            invoice_number = 0;
            document_no = 0;
            InvoiceNo.Text = String.Empty;
            Product_Each_Item_Count = 0;
            Cart_OverAllDiscount = 0;
            Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
            BarcodeSearch_bar.Visibility = Visibility.Visible;
            productSearch_cart.Visibility = Visibility.Visible;
            Payment_Screen.Visibility = Visibility.Hidden;
            iteamProduct.Visibility = Visibility.Visible;
            Cart_Hold_Page.Visibility = Visibility.Hidden;
            Cart_Main_Page.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            Keyboard.Focus(productSearch_cart);
            iteamProduct.SelectedIndex = 0;
            MaintainActual_AMount();
            //});
        }

        /// <summary>
        /// GEt Data form Table c_invoice where in_hold flage is 'Y'
        /// </summary>
        private void GetOrderHold_Count()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            connection.Open();
            NpgsqlCommand cmd_c_invoic_on_hold = new NpgsqlCommand("SELECT c_invoice_id, ad_client_id, ad_org_id, ad_role_id, ad_user_id," +
            "documentno, m_warehouse_id, c_bpartner_id, qid, mobilenumber," +
            "discounttype, discountvalue, grandtotal, orderid, reason," +
            "is_posted, is_onhold, is_canceled, is_completed, grandtotal_round_off,total_items_count" +
            " FROM c_invoice Where ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + " AND ad_user_id = " + AD_USER_ID + " AND m_warehouse_id = " + AD_Warehouse_Id + " " +
            "AND is_onhold = 'Y' Order By c_invoice_id Desc ; ", connection);
            NpgsqlDataReader _get_c_invoic_on_hold = cmd_c_invoic_on_hold.ExecuteReader();
            int Same_loop = 0;
            string Same_loop_invoice = "";
            if (ProductHold_list_items.Count() != 0)
            {
                Same_loop = 1;
                var temp_selected_hold_items = ProductHold_list_items[Hold_iteamProduct_SelectedIndex];
                Same_loop_invoice = temp_selected_hold_items.C_invoice_id;
            }

            ProductHold_list_items.Clear();

            while (_get_c_invoic_on_hold.Read())
            {
                //On Iteration Run, Check The list to remove Duplicates entries
                //double count = Convert.ToDouble(ProductHold_list_items.Where(s => s.C_invoice_id == _get_c_invoic_on_hold.GetInt64(0).ToString()).Count());
                //if (count == 0)
                //{
                ProductHold_list_items.Add(new Product_Hold()
                {
                    C_invoice_id = _get_c_invoic_on_hold.GetDouble(0).ToString(),
                    Ad_client_id = _get_c_invoic_on_hold.GetDouble(1).ToString(),
                    Ad_org_id = _get_c_invoic_on_hold.GetDouble(2).ToString(),
                    Ad_role_id = _get_c_invoic_on_hold.GetDouble(3).ToString(),
                    Ad_user_id = _get_c_invoic_on_hold.GetDouble(4).ToString(),
                    Documentno = _get_c_invoic_on_hold.GetString(5),
                    M_warehouse_id = _get_c_invoic_on_hold.GetDouble(6).ToString(),
                    C_bpartner_id = _get_c_invoic_on_hold.GetDouble(7).ToString(),
                    Qid = _get_c_invoic_on_hold.GetString(8),
                    Mobilenumber = _get_c_invoic_on_hold.GetString(9),
                    Discounttype = _get_c_invoic_on_hold.GetDouble(10).ToString(),
                    Discountvalue = _get_c_invoic_on_hold.GetDouble(11).ToString(),
                    Grandtotal = _get_c_invoic_on_hold.GetDouble(12).ToString("0.00"),
                    Orderid = _get_c_invoic_on_hold.GetDouble(13).ToString(),
                    Reason = _get_c_invoic_on_hold.GetString(14).ToString(),
                    Is_posted = _get_c_invoic_on_hold.GetString(15),
                    Is_onhold = _get_c_invoic_on_hold.GetString(16),
                    Is_canceled = _get_c_invoic_on_hold.GetString(17),
                    Is_completed = _get_c_invoic_on_hold.GetString(18),
                    Grandtotal_round_off = _get_c_invoic_on_hold.GetDouble(19).ToString("0.00"),
                    Total_items_count = _get_c_invoic_on_hold.GetDouble(20).ToString(),
                });
                //}
            }

            connection.Close();

            ProductHold_list = ProductHold_list_items;
            ICollectionView ProductHold_list_items_view = CollectionViewSource.GetDefaultView(ProductHold_list);
            ProductHold_list_items_view.Refresh();
            if (Same_loop == 1)
            {
                int check_loop_hold = ProductHold_list_items.Where(x => x.C_invoice_id == Same_loop_invoice).Count();
                if (check_loop_hold != 0)
                {
                    Hold_iteamProduct_SelectedIndex = ProductHold_list_items.IndexOf(ProductHold_list_items.Where(x => x.C_invoice_id == Same_loop_invoice).FirstOrDefault());
                }
                else
                {
                    Hold_iteamProduct_SelectedIndex = 0;
                }
            }
        }

        #endregion Hold

        /// <summary>
        /// Store Cart Items Locally
        /// </summary>
        /// <param name="is_canceled"></param>
        /// <param name="is_onhold"></param>
        /// <param name="is_completed"></param>
        public void Store_Cart_Items_Locally(string is_canceled, string is_onhold, string is_completed, string reason)
        {
            try
            {
                int discounttype = 0;
                if (Percentage_OR_Price == "%")
                { discounttype = 0; }
                else
                { discounttype = 1; }
                double overallDis = 0;
                if (OverAllDiscount_tx.Text != "")
                    overallDis = Convert.ToDouble(OverAllDiscount_tx.Text);

                double Grand_Cart_Total_Round_Off = Grand_Cart_Total;
                double change = 0, balance = 0;
                if (SubToTal_Balance_Amount < 0) { change = SubToTal_Balance_Amount; }
                else { balance = SubToTal_Balance_Amount; }
                string[] parts = Grand_Cart_Total.ToString("0.00").Split('.');
                int Grand_Cart_Total1 = int.Parse(parts[0]);
                int Grand_Cart_Total2 = int.Parse(parts[1]);
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Close();
                connection.Open();
                NpgsqlCommand check_c_invoice_ = new NpgsqlCommand("SELECT is_posted, is_onhold, is_canceled, is_completed " +
                    "FROM c_invoice WHERE c_invoice_id = " + invoice_number + ";", connection);
                NpgsqlDataReader _check_c_invoice_ = check_c_invoice_.ExecuteReader();
                if (_check_c_invoice_.Read() && _check_c_invoice_.HasRows == true)
                {
                    #region Update Invoice Header

                    connection.Close();
                    connection.Open();

                    NpgsqlCommand uPDATE_c_invoice_ = new NpgsqlCommand("UPDATE c_invoice SET " +
                        "discounttype =" + discounttype + ", " +
                        "discountvalue =" + overallDis.ToString("0.00") + ", " +
                        "grandtotal =" + Grand_Cart_Total1 + ", " +
                        "updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', " +
                        "is_onhold='" + is_onhold + "', is_canceled='" + is_canceled + "'," +
                        " is_completed='" + is_completed + "', " +
                        "grandtotal_round_off=" + Grand_Cart_Total_Round_Off + ", " +
                        "total_items_count=" + Product_Each_Item_Count + ", " +
                        "balance =" + balance + ", " +
                        "change = " + change + ", " +
                        "lossamount = ." + Grand_Cart_Total2 + ", " +
                        "extraamount = 0, " +
                        "reason='" + reason + "', " +
                        "is_return='" + is_Return + "' WHERE c_invoice_id = " + invoice_number + ";", connection);
                    uPDATE_c_invoice_.ExecuteReader();
                    connection.Close();

                    #endregion Update Invoice Header

                    #region Delete Invoiceline to Insert New Updated Line

                    connection.Open();
                    NpgsqlCommand Delete_c_invoiceliine_ = new NpgsqlCommand("DELETE FROM c_invoiceline WHERE c_invoice_id = " + invoice_number + ";", connection);
                    Delete_c_invoiceliine_.ExecuteReader();
                    connection.Close();

                    #endregion Delete Invoiceline to Insert New Updated Line
                }
                else
                {
                    #region Creating Invoice Header
                    long Session_StartTimeMilliseconds = new DateTimeOffset(Convert.ToDateTime(AD_Session_Started_at)).ToUnixTimeMilliseconds();

                    connection.Close();
                    connection.Open();
                    NpgsqlCommand INSERT_c_invoice = new NpgsqlCommand("INSERT INTO c_invoice(" +
                    "c_invoice_id, ad_client_id, ad_org_id, ad_role_id, ad_user_id," +
                    "documentno, m_warehouse_id, c_bpartner_id, qid, mobilenumber," +
                    "discounttype, discountvalue, grandtotal, orderid, reason, createdby, updatedby,is_canceled,is_onhold,is_completed," +
                    "grandtotal_round_off,balance,change,total_items_count,lossamount,session_id,openingbalance,is_return)" +
                    "VALUES(" + invoice_number + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_ROLE_ID + "," + AD_USER_ID + "," + document_no + "," + AD_Warehouse_Id + "," + AD_bpartner_Id + ",'" + txtCustomer_CR.Text + "','" + txtCustomermobile.Text + "'," + discounttype +
                    "," + overallDis.ToString("0.00") + "," + Grand_Cart_Total1 + ",0,''," + AD_USER_ID + "," + AD_USER_ID + ",'" + is_canceled + "','" + is_onhold + "','" + is_completed + "'," + Grand_Cart_Total_Round_Off + "," + balance + "," + change + "," + Product_Each_Item_Count +
                    " ,." + Grand_Cart_Total2 + "," + Session_StartTimeMilliseconds + ",0,'" + is_Return + "');", connection);

                    INSERT_c_invoice.ExecuteNonQuery();
                    connection.Close();

                    #endregion Creating Invoice Header
                }

                #region Inserting Invoice Line Items

                items.ToList().ForEach(x =>
                {
                    if (x.Percentpercentage_OR_Price == "%")
                    { discounttype = 0; }
                    else
                    { discounttype = 1; }
                    connection.Close();

                    connection.Open();

                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'c_invoiceline';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                    if (_get__Ad_sequenc_no.Read())
                    {
                        _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                    }
                    connection.Close();

                    connection.Open();
                    string INSERT_c_invoiceline_string = "INSERT INTO c_invoiceline(" +
                    "c_invoiceline_id, ad_client_id, ad_org_id, c_invoice_id, ad_user_id," +
                    "m_product_id, productname, paroductarabicname, productbarcode, " +
                    "c_uom_id, uomname, qtyinvoiced, qtyentered, saleprice, costprice, " +
                    "discounttype, discountvalue, linetotalamt, pricelistid, islinediscounted, createdby, updatedby)" +
                    "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + invoice_number + "," + AD_USER_ID +
                    "," + x.Product_ID + ",@_xProduct_Name,@_xProduct_Arabicname,@_xIteam_Barcode," + x.Uom_Id + ",@_xUom_Name," + x.Quantity + "," + x.Quantity + "," + x.Sopricestd + "," + x.Current_costprice +
                    ", " + discounttype + "," + x.Discount + "," + x.Amount + "," + AD_PricelistID + ",'" + x.Line_Discount + "'," + AD_USER_ID + "," + AD_USER_ID +
                    "); ";

                    NpgsqlCommand INSERT_c_invoiceline = new NpgsqlCommand(INSERT_c_invoiceline_string, connection);
                    INSERT_c_invoiceline.Parameters.AddWithValue("@_xIteam_Barcode", x.Iteam_Barcode);
                    INSERT_c_invoiceline.Parameters.AddWithValue("@_xProduct_Arabicname", x.Product_Arabicname);
                    INSERT_c_invoiceline.Parameters.AddWithValue("@_xProduct_Name", x.Product_Name);
                    INSERT_c_invoiceline.Parameters.AddWithValue("@_xUom_Name", x.Uom_Name);
                    INSERT_c_invoiceline.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'c_invoiceline';", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();
                });
                connection.Close();

                #endregion Inserting Invoice Line Items
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void BackTOCart_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Back_OR_Esc();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Back_OR_Esc()
        {
            if (Check_keyboard_Focus == "Session_Close_Pannel")
            {
                Check_keyboard_Focus = "Session_Close_Pannel";
                Session_Close_Pannel.Visibility = Visibility.Hidden;
                Cart_Main_Pannel.Visibility = Visibility.Visible;
                //Error_page.Visibility = Visibility.Visible;
                // Session_Check.Visibility = Visibility.Visible;
                txt_Price.Visibility = Visibility.Hidden;
                SessionChangePrice.Visibility = Visibility.Hidden;
                CustLeft.Visibility = Visibility.Hidden;
                WrongOrder.Visibility = Visibility.Hidden;
                CancelYes.Visibility = Visibility.Hidden;
                CancelNo.Visibility = Visibility.Hidden;
            }


            Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
            BarcodeSearch_bar.Visibility = Visibility.Visible;
            productSearch_cart.Visibility = Visibility.Visible;
            Payment_Screen.Visibility = Visibility.Hidden;
            iteamProduct.Visibility = Visibility.Visible;
            Cart_Hold_Page.Visibility = Visibility.Hidden;
            Cart_Main_Page.Visibility = Visibility.Visible;
            __Side_Menu_Page.Visibility = Visibility.Hidden;
            __Side_quickMenu_Page.Visibility = Visibility.Hidden;
            __Side_Menu_POS_Setting_Page.Visibility = Visibility.Hidden;
            Menu_Page_Customer.Visibility = Visibility.Hidden;
            Menu_Page_Category.Visibility = Visibility.Hidden;
            Menu_Page_Customer_Payment.Visibility = Visibility.Hidden;
            Cart_Main_Pannel_Window.Visibility = Visibility.Visible;
            Menu_Page_Product.Visibility = Visibility.Hidden;
            Tittle_Bar_Right_Content.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            if (items.Count() > 0)
            {
                OrderCancel_button.IsEnabled = true;
                OrderComplected_button.IsEnabled = true;
                OverAllDiscount_button.Visibility = Visibility.Visible;
                OverAllDiscount_button_short.Visibility = Visibility.Visible;
            }
            Side_Menu_Bg.Visibility = Visibility.Hidden;
            Side_menu.Visibility = Visibility.Hidden;
            nav_bar_status = 0;
            Change_Activity_of_Payment_buttons("null");
            ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
            ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;

            GeneralTextPopUp.Visibility = Visibility.Hidden;
            popUpText.Visibility = Visibility.Hidden;
            ValueChanger_key_pad.Text = String.Empty;
            quick_Check_windows_Focus = String.Empty;
            Payment_Cash.IsEnabled = true;
            Payment_Card.IsEnabled = true;
            Payment_Credit.IsEnabled = true;
            Payment_Complementry.IsEnabled = true;
            Payment_Split.IsEnabled = true;
            pmt_Return.IsEnabled = true;
            Reset_Pmt_Return_View();
            Keyboard.Focus(productSearch_cart);
        }

        #endregion Payment End Process |Cancel|Hold|Complected

        #region Payment Options

        #region Payment Option Button Click

        private void Payment_Cash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Payment_Cash();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Payment_Card_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Payment_Card();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Payment_Complementry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Payment_Complementry();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Payment_Credit_Click(object sender, RoutedEventArgs e)
        {
            _Payment_Credit();
        }

        private void Payment_Split_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Payment_Split();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        public void _Payment_Cash()
        {
            if (items.Count() < 1)
                return;

            Change_Activity_of_Payment_buttons("Cash");
            Payment_Cash_Only_tx.Text = Grand_Total_cart_price.Text;

            #region Customer display 
            string SellingPrice = "";
            string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

            int TotalSpace = Convert.ToInt32(CustDispspace);
            string strproductname = "Thank you..!!";
            int productnamelen = strproductname.Length;
            int sellingPricelen = SellingPrice.Length;
            int totallen = "Total".Length;
            string strtotalprice = Grand_Cart_Total.ToString("0.00");
            int totalpricelen = strtotalprice.Length;

            int space1 = 0;
            if (productnamelen > sellingPricelen)
            {
                space1 = TotalSpace - productnamelen - sellingPricelen;

            }
            int space2 = 0;
            if (totallen > totalpricelen)
            {
                space2 = TotalSpace - totallen - totalpricelen;

            }


            // int space1 = TotalSpace - productnamelen - sellingPricelen;
            // int space2 = TotalSpace - totallen - totalpricelen;
            //int space1 = TotalSpace - productnamelen - sellingPricelen;
            //int space2 = TotalSpace - totallen - totalpricelen;
            string strspace1 = new string(' ', space1);
            string strspace2 = new string(' ', space2);
            SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
            #endregion Customer display

            MaintainActual_AMount();
            // SerialPort.display("Thanks you..Visit Again!!" , "", "Total" + strspace2, strtotalprice);

            //  SerialPort.display("Thanks for shopping!!!", "", "Total       ", Payment_Cash_Only_tx.Text);

            // Payment_Cash_Only_tx.Select(0, Payment_Cash_Only_tx.Text.Length);
            //  Payment_Cash_Only_tx.SelectAll();
            //(sender as TextBox).Select(0, (sender as TextBox).Text.Length);
            //  Payment_Cash_Only_tx.Focus(); 

            Keyboard.Focus(Payment_Cash_Only_tx);

        }

        public void _Payment_Card()
        {
            if (items.Count() < 1)
                return;

            Change_Activity_of_Payment_buttons("Card");
            Keyboard.Focus(Payment_Card_Only_tx);
        }

        public void _Payment_Complementry()
        {
            if (items.Count() < 1)
                return;

            Change_Activity_of_Payment_buttons("Complementry");
            Keyboard.Focus(Payment_Complementary_Name_tx);
        }

        public void _Payment_Credit()
        {
            if (items.Count() < 1)
                return;

            Change_Activity_of_Payment_buttons("GiftCard");
            Keyboard.Focus(Payment_Credit_Name_tx);
        }

        public void _Payment_Split()
        {
            if (items.Count() < 1)
                return;

            Change_Activity_of_Payment_buttons("Split");
            Keyboard.Focus(Split_Payment_Cash_tx);
        }

        /// <summary>
        /// 1-Cash
        /// 2-Card
        /// 3-GiftCard
        /// 4-Complementry
        /// 5-Split
        /// </summary>
        /// <param name="button_name"></param>
        public void Change_Activity_of_Payment_buttons(string button_name)
        {
            OrderComplected_button.IsEnabled = true;
            OrderCancel_button.IsEnabled = true;
            switch (button_name)
            {
                case "Cash":
                    payment_method_selected = 1;
                    SubToTal_Balance_Amount = Grand_Cart_Total;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    Payment_Cash_Only_tx.Text = SubToTal_Balance_Amount.ToString("0.00");
                    // Exchange_Invoice_Number_Search.Visibility = Visibility.Visible;
                    BarcodeSearch_bar.Visibility = Visibility.Hidden;
                    productSearch_cart.Visibility = Visibility.Hidden;
                    Payment_Screen.Visibility = Visibility.Visible;
                    Split_Payment_Screen.Visibility = Visibility.Hidden;
                    Credit_Payment_Screen.Visibility = Visibility.Hidden;
                    Complementary_Payment_Screen.Visibility = Visibility.Hidden;
                    Card_Payment_Screen.Visibility = Visibility.Hidden;
                    Cash_Payment_Screen.Visibility = Visibility.Visible;
                    iteamProduct.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Visible;
                    OverAllDiscount_button.Visibility = Visibility.Hidden;
                    OverAllDiscount_button_short.Visibility = Visibility.Hidden;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    pmt_Return.IsEnabled = true;
                    break;
                case "Return":
                    payment_method_selected = 6;
                    SubToTal_Balance_Amount = Grand_Cart_Total;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    Payment_Cash_Only_tx.Text = SubToTal_Balance_Amount.ToString("0.00");
                    // Exchange_Invoice_Number_Search.Visibility = Visibility.Visible; 
                    Payment_Cash.IsEnabled = false;
                    Payment_Card.IsEnabled = false;
                    Payment_Credit.IsEnabled = false;
                    Payment_Complementry.IsEnabled = false;
                    Payment_Split.IsEnabled = false;
                    is_Return = "Y";
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);

                    break;

                case "Card":
                    payment_method_selected = 2;
                    SubToTal_Balance_Amount = Grand_Cart_Total;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    Payment_Card_Only_tx.Text = SubToTal_Balance_Amount.ToString("0.00");
                    Payment_Card_Only_tx.IsReadOnly = true;
                    //  Exchange_Invoice_Number_Search.Visibility = Visibility.Visible;
                    BarcodeSearch_bar.Visibility = Visibility.Hidden;
                    productSearch_cart.Visibility = Visibility.Hidden;
                    Payment_Screen.Visibility = Visibility.Visible;
                    Split_Payment_Screen.Visibility = Visibility.Hidden;
                    Credit_Payment_Screen.Visibility = Visibility.Hidden;
                    Complementary_Payment_Screen.Visibility = Visibility.Hidden;
                    Card_Payment_Screen.Visibility = Visibility.Visible;
                    Cash_Payment_Screen.Visibility = Visibility.Hidden;
                    iteamProduct.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Visible;
                    OverAllDiscount_button.Visibility = Visibility.Hidden;
                    OverAllDiscount_button_short.Visibility = Visibility.Hidden;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    pmt_Return.IsEnabled = false;
                    break;

                case "GiftCard":
                    payment_method_selected = 4;
                    Payment_Credit_Name_tx.Text = String.Empty;
                    Payment_Credit_Mobile_No_tx.Text = String.Empty;
                    //Exchange_Invoice_Number_Search.Visibility = Visibility.Visible;
                    BarcodeSearch_bar.Visibility = Visibility.Hidden;
                    productSearch_cart.Visibility = Visibility.Hidden;
                    Payment_Screen.Visibility = Visibility.Visible;
                    Split_Payment_Screen.Visibility = Visibility.Hidden;
                    Credit_Payment_Screen.Visibility = Visibility.Visible;
                    Complementary_Payment_Screen.Visibility = Visibility.Hidden;
                    Card_Payment_Screen.Visibility = Visibility.Hidden;
                    Cash_Payment_Screen.Visibility = Visibility.Hidden;
                    iteamProduct.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Hidden;
                    OverAllDiscount_button.Visibility = Visibility.Hidden;
                    OverAllDiscount_button_short.Visibility = Visibility.Hidden;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    pmt_Return.IsEnabled = false;
                    break;

                case "Complementry":
                    payment_method_selected = 3;
                    Payment_Complementary_Name_tx.Text = String.Empty;
                    Payment_Complementary_Mobile_No_tx.Text = String.Empty;
                    Complementary_Reason_tx.Text = String.Empty;
                    //Exchange_Invoice_Number_Search.Visibility = Visibility.Visible;
                    BarcodeSearch_bar.Visibility = Visibility.Hidden;
                    productSearch_cart.Visibility = Visibility.Hidden;
                    Payment_Screen.Visibility = Visibility.Visible;
                    Split_Payment_Screen.Visibility = Visibility.Hidden;
                    Credit_Payment_Screen.Visibility = Visibility.Hidden;
                    Complementary_Payment_Screen.Visibility = Visibility.Visible;
                    Card_Payment_Screen.Visibility = Visibility.Hidden;
                    Cash_Payment_Screen.Visibility = Visibility.Hidden;
                    iteamProduct.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Hidden;
                    OverAllDiscount_button.Visibility = Visibility.Hidden;
                    OverAllDiscount_button_short.Visibility = Visibility.Hidden;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    pmt_Return.IsEnabled = false;
                    break;

                case "Split":
                    payment_method_selected = 5;
                    Split_Payment_Card_tx.Text = String.Empty;
                    Split_Payment_Cash_tx.Text = String.Empty;
                    Split_Payment_GiftCash_tx.Text = String.Empty;
                    Split_Payment_Exchange_tx.Text = String.Empty;
                    SubToTal_Balance_Amount = Grand_Cart_Total;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    //Exchange_Invoice_Number_Search.Visibility = Visibility.Visible;
                    BarcodeSearch_bar.Visibility = Visibility.Hidden;
                    productSearch_cart.Visibility = Visibility.Hidden;
                    Payment_Screen.Visibility = Visibility.Visible;
                    Split_Payment_Screen.Visibility = Visibility.Visible;
                    Credit_Payment_Screen.Visibility = Visibility.Hidden;
                    Complementary_Payment_Screen.Visibility = Visibility.Hidden;
                    Card_Payment_Screen.Visibility = Visibility.Hidden;
                    Cash_Payment_Screen.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Visible;
                    iteamProduct.Visibility = Visibility.Hidden;
                    OverAllDiscount_button.Visibility = Visibility.Hidden;
                    OverAllDiscount_button_short.Visibility = Visibility.Hidden;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.Green);
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    break;

                case "null":
                    Payment_Cash_Only_tx.Text = String.Empty;
                    Payment_Card_Only_tx.Text = String.Empty;
                    Payment_Credit_Name_tx.Text = String.Empty;
                    Payment_Credit_Mobile_No_tx.Text = String.Empty;
                    Payment_Complementary_Name_tx.Text = String.Empty;
                    Payment_Complementary_Mobile_No_tx.Text = String.Empty;
                    Complementary_Reason_tx.Text = String.Empty;
                    Split_Payment_Card_tx.Text = String.Empty;
                    Split_Payment_Cash_tx.Text = String.Empty;
                    Split_Payment_GiftCash_tx.Text = String.Empty;
                    Split_Payment_Exchange_tx.Text = String.Empty;
                    SubToTal_Balance_Amount = Grand_Cart_Total;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    payment_method_selected = 0;
                    BarcodeSearch_bar.Visibility = Visibility.Visible;
                    productSearch_cart.Visibility = Visibility.Visible;
                    iteamProduct.Visibility = Visibility.Visible;
                    Payment_Screen.Visibility = Visibility.Hidden;
                    Exchange_Invoice_Number_Search.Visibility = Visibility.Hidden;
                    Split_Payment_Screen.Visibility = Visibility.Hidden;
                    Credit_Payment_Screen.Visibility = Visibility.Hidden;
                    Complementary_Payment_Screen.Visibility = Visibility.Hidden;
                    Card_Payment_Screen.Visibility = Visibility.Hidden;
                    Cash_Payment_Screen.Visibility = Visibility.Hidden;
                    Payment_Screen_Show_Balance.Visibility = Visibility.Hidden;
                    OverAllDiscount_button.Visibility = Visibility.Visible;
                    OverAllDiscount_button_short.Visibility = Visibility.Visible;
                    Payment_Cash_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Card_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_GiftCard_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Complementry_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Payment_Split_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Return_Border.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    is_Return = "N";
                    pmt_Return.IsEnabled = false;
                    break;

                default:
                    is_Return = "N";
                    break;
            }
        }

        #endregion Payment Option Button Click

        #region Payment Screen Focus assist

        private void Exchange_Invoice_Number_Search_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Exchange_Invoice_Number_Search_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
        }

        private void Payment_Cash_Only_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Cash_Only_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 1;
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            tb.SelectAll();

            e.Handled = true;
            //Payment_Cash_Only_tx.SelectAll();
        }

        private void Payment_Card_Only_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Card_Only_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 2;
        }

        private void Payment_Complementary_Name_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Complementary_Name_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 3;
        }

        private void Payment_Complementary_Mobile_No_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Complementary_Mobile_No_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 3;
        }

        private void Complementary_Reason_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Complementary_Reason_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 3;
        }

        private void Payment_Credit_Name_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Credit_Name_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 4;
        }

        private void Payment_Credit_Mobile_No_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Payment_Credit_Mobile_No_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = true;
            payment_method_selected = 4;
        }

        private void Split_Payment_Cash_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Split_Payment_Cash_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = false;
            payment_method_selected = 5;
        }

        private void Split_Payment_Card_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Split_Payment_Card_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = false;
            payment_method_selected = 5;
        }

        private void Split_Payment_GiftCash_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Split_Payment_GiftCash_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = false;
            payment_method_selected = 5;
        }

        private void Split_Payment_Exchange_tx_GotFocus(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Split_Payment_Exchange_tx_GotFocus";
            Exchange_Invoice_Number_Search.Text = String.Empty;
            Exchange_Invoice_Number_Search.IsReadOnly = false;
            Split_Payment_Exchange_tx.IsReadOnly = true;
            payment_method_selected = 5;
        }

        #endregion Payment Screen Focus assist

        #region Payment TextChanged

        private void Payment_Cash_Only_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Payment_Cash_Only_tx.Select(Payment_Cash_Only_tx.Text.Length, 0);
                if (Payment_Cash_Only_tx.Text == String.Empty)
                {
                    SubToTal_Balance_Amount = Grand_Cart_Total - 0;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    return;
                }
                double amt = 0; try { amt = Convert.ToDouble(Payment_Cash_Only_tx.Text); } catch { return; }

                SubToTal_Balance_Amount = Grand_Cart_Total - amt;
                Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                if (SubToTal_Balance_Amount < 0) { Payment_Screen_Show_Balance_tx.Text = "Change"; } else { Payment_Screen_Show_Balance_tx.Text = "Balance"; }

            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Payment_Card_Only_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Payment_Card_Only_tx.Select(Payment_Card_Only_tx.Text.Length, 0);
                if (Payment_Card_Only_tx.Text == String.Empty)
                {
                    SubToTal_Balance_Amount = Grand_Cart_Total - 0;
                    Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                    return;
                }
                double validate = Convert.ToDouble(Payment_Card_Only_tx.Text);
                if (Math.Round(validate, 2) > Math.Round(Grand_Cart_Total, 2))
                {
                    Payment_Card_Only_tx.Text = Payment_Card_Only_tx.Text.Remove(Payment_Card_Only_tx.Text.Length - 1);
                    return;
                }

                double amt = 0; try { amt = Convert.ToDouble(Payment_Card_Only_tx.Text); } catch { return; }
                SubToTal_Balance_Amount = Grand_Cart_Total - amt;
                Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                if (SubToTal_Balance_Amount < 0) { Payment_Screen_Show_Balance_tx.Text = "Change"; } else { Payment_Screen_Show_Balance_tx.Text = "Balance"; }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Split_Payment_tx_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double cash = 0;
                double card = 0;
                double giftcard = 0;
                double exchange = 0;
                Split_Payment_Cash_tx.Select(Split_Payment_Cash_tx.Text.Length, 0);
                Split_Payment_Card_tx.Select(Split_Payment_Card_tx.Text.Length, 0);
                Split_Payment_GiftCash_tx.Select(Split_Payment_GiftCash_tx.Text.Length, 0);
                Split_Payment_Exchange_tx.Select(Split_Payment_Exchange_tx.Text.Length, 0);
                if (Split_Payment_Cash_tx.Text != String.Empty)
                    try { cash = Convert.ToDouble(Split_Payment_Cash_tx.Text); } catch { return; }
                if (Split_Payment_Card_tx.Text != String.Empty)
                    try
                    {
                        card = Convert.ToDouble(Split_Payment_Card_tx.Text);
                        double validate = Convert.ToDouble(Split_Payment_Card_tx.Text);
                        double temp = Grand_Cart_Total - (cash + giftcard + exchange);
                        if (validate > temp)
                        {
                            Split_Payment_Card_tx.Text = Split_Payment_Card_tx.Text.Remove(Split_Payment_Card_tx.Text.Length - 1);
                            return;
                        }
                    }
                    catch { return; }
                if (Split_Payment_GiftCash_tx.Text != String.Empty)
                    try { giftcard = Convert.ToDouble(Split_Payment_GiftCash_tx.Text); } catch { return; }
                if (Split_Payment_Exchange_tx.Text != String.Empty)
                    try { exchange = Convert.ToDouble(Split_Payment_Exchange_tx.Text); } catch { return; }

                SubToTal_Balance_Amount = Grand_Cart_Total - (cash + card + giftcard + exchange);
                Payment_Screen_Show_Balance_val.Text = SubToTal_Balance_Amount.ToString("0.00");
                if (SubToTal_Balance_Amount < 0) { Payment_Screen_Show_Balance_tx.Text = "Change"; } else { Payment_Screen_Show_Balance_tx.Text = "Balance"; }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Payment TextChanged

        private void Exchange_Invoice_Number_Search_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Exchange_Invoice_Number_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Exchange_Invoice_Number_Search.Select(Exchange_Invoice_Number_Search.Text.Length, 0);
        }

        #endregion Payment Options

        #region Choose Customers | Default Cash Customers

        private void ChooseCustomers_Click(object sender, RoutedEventArgs e)
        {
            popupCustomerList.IsOpen = true;
            MainPage.IsEnabled = false;
            popup_lstcust_Bind_customer_Search();
            txtpopupcustomerSearch.Focus();
        }

        #endregion Choose Customers | Default Cash Customers

        #region Side Menu

        //private void btnRightMenuHide_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbHideRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
        //}

        //private void btnRightMenuShow_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowHideMenu("sbShowRightMenu", btnRightMenuHide, btnRightMenuShow, pnlRightMenu);
        //}

        //private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        //{
        //    Storyboard sb = Resources[Storyboard] as Storyboard;
        //    sb.Begin(pnl);

        //    if (Storyboard.Contains("Show"))
        //    {
        //        btnHide.Visibility = System.Windows.Visibility.Visible;
        //        btnShow.Visibility = System.Windows.Visibility.Hidden;
        //    }
        //    else if (Storyboard.Contains("Hide"))
        //    {
        //        btnHide.Visibility = System.Windows.Visibility.Hidden;
        //        btnShow.Visibility = System.Windows.Visibility.Visible;
        //    }
        //}

        public int nav_bar_status = 0;
        public int CheckServerErrorPOSAllProducts = 0;
        private string jsonPOSAllProducts;
        private dynamic POSAllProductsApiStringResponce;
        private dynamic POSAllProductsApiJSONResponce;
        private dynamic POSCategoryApiStringResponce;
        private dynamic POSCategoryApiJSONResponce;
        private string jsonPOSCategory;
        private dynamic GetUpdatedDataApiStringResponce;
        private dynamic GetUpdatedDataApiJSONResponce;
        private string jsonGetUpdatedData;

        private void MenuNavigation_but(object sender, RoutedEventArgs e)
        {
            if (nav_bar_status == 0)
            {
                Side_Menu_Bg.Visibility = Visibility.Visible;
                Side_menu.Visibility = Visibility.Visible;
                nav_bar_status = 1;
            }
            else
            {
                Side_Menu_Bg.Visibility = Visibility.Hidden;
                Side_menu.Visibility = Visibility.Hidden;
                Keyboard.Focus(productSearch_cart);
                nav_bar_status = 0;
            }
        }

        private void Side_Menu_Bg_MouseEnter(object sender, MouseEventArgs e)
        {
            Side_Menu_Bg.Visibility = Visibility.Hidden;
            Side_menu.Visibility = Visibility.Hidden;
            nav_bar_status = 0;
            Keyboard.Focus(productSearch_cart);
        }

        private void LogOutBt_Click(object sender, RoutedEventArgs e)
        {
            //string message = "Are you sure want to logout?                                                     ";
            //string caption = "Confirmation";
            //MessageBoxButton buttons = MessageBoxButton.YesNo;
            //MessageBoxImage icon = MessageBoxImage.Question;
            //if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            //{
            //    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            //    connection.Open();
            //    NpgsqlCommand cmd_User_Logout = new NpgsqlCommand("UPDATE ad_user_pos SET isactive='N' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and ad_user_id = " + AD_USER_ID + " and c_bpartner_id = " + AD_bpartner_Id + "; ", connection);
            //    cmd_User_Logout.ExecuteReader();
            //    connection.Close();
            //    ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Login;
            //}
            //else
            //{
            //    Side_Menu_Bg.Visibility = Visibility.Hidden;
            //    Side_menu.Visibility = Visibility.Hidden;
            //    nav_bar_status = 0;
            //    Keyboard.Focus(BarcodeSearch_cart);
            //}
            Error_page.Visibility = Visibility.Visible;
            Logout_Check.Visibility = Visibility.Visible;
        }

        private void SessionLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Check_keyboard_Focus = "Session_Close_Pannel";
                Session_Close_Pannel.Visibility = Visibility.Visible;
                Cart_Main_Pannel.Visibility = Visibility.Hidden;
                Error_page.Visibility = Visibility.Hidden;
                Session_Check.Visibility = Visibility.Hidden;
                Calculate_OpeningBalance();
                SessionClose_grand_total_cal();
                Keyboard.Focus(SessionClose_Pruchase_input);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
            //NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            //connection.Open();
            //NpgsqlCommand cmd_User_Logout = new NpgsqlCommand("UPDATE ad_user_pos SET isactive='N' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and ad_user_id = " + AD_USER_ID + " and c_bpartner_id = " + AD_bpartner_Id + "; ", connection);
            //cmd_User_Logout.ExecuteReader();
            //connection.Close(); 
        }
        private void LogoutCheckClose_Click(object sender, RoutedEventArgs e)
        {
            Error_page.Visibility = Visibility.Hidden;
            Logout_Check.Visibility = Visibility.Hidden;
        }

        private void Side_Menu_About_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_Page.Visibility = Visibility.Visible;
            InvoiceReSync.Visibility = Visibility.Hidden;
            Change_Content_of_Side_Menu_buttons("About");
        }

        private void Side_Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_Page.Visibility = Visibility.Visible;
            InvoiceReSync.Visibility = Visibility.Hidden;
            Change_Content_of_Side_Menu_buttons("Settings");
        }

        private void Side_Menu_Manual_Sync_Click(object sender, RoutedEventArgs e)
        {
            ManualSync();
        }
        private async void ManualSync()
        {
            try
            {
                __Side_Menu_Page.Visibility = Visibility.Visible;
                InvoiceReSync.Visibility = Visibility.Hidden;
                Change_Content_of_Side_Menu_buttons("Manual_Sync");

                if (_NetworkUpStatus != true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Visible;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Hidden;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Hidden;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Visible;
                        //Manual_Sync_ProgressBar.Visibility = Visibility.Visible;
                        //Manual_Sync_ProgressBar_text.Visibility = Visibility.Visible;
                        Side_Menu_Page_Manual_Sync_inner_Content.Visibility = Visibility.Hidden;
                    });

                    string local_itemCount = "0";
                    string itemCount = "0";
                    await Task.Run(() =>
                    {
                        NpgsqlConnection connection = new NpgsqlConnection(connstring);
                        connection.Open();
                        NpgsqlCommand Get_local_product_count = new NpgsqlCommand("SELECT COUNT(m_product_id) FROM m_product where ad_client_id = " + AD_Client_ID + " and ad_org_id = " + AD_ORG_ID + " and attribute2 = '" + AD_Warehouse_Id + "';", connection);
                        NpgsqlDataReader _Get_local_product_count = Get_local_product_count.ExecuteReader();
                        if (_Get_local_product_count.Read())
                        {
                            local_itemCount = _Get_local_product_count.GetInt64(0).ToString();
                        }
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand Get_product_itemCount = new NpgsqlCommand("SELECT CASE WHEN attribute1 is null THEN 0 ELSE CAST (attribute1 AS INTEGER)  END AS itemcount FROM m_warehouse where ad_client_id = " + AD_Client_ID + " and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = '" + AD_Warehouse_Id + "';", connection);
                        NpgsqlDataReader _Get_product_itemCount = Get_product_itemCount.ExecuteReader();
                        if (_Get_product_itemCount.Read())
                        {
                            itemCount = _Get_product_itemCount.GetInt64(0).ToString();
                        }
                        connection.Close();
                    });
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Synced Products:" + local_itemCount + "/" + itemCount + "";
                        //Manual_Sync_ProgressBar.Visibility = Visibility.Hidden;
                        //Manual_Sync_ProgressBar_text.Visibility = Visibility.Hidden;
                        Side_Menu_Page_Manual_Sync_inner_Content.Visibility = Visibility.Visible;
                    });

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void Side_Menu_View_Orders_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_Page.Visibility = Visibility.Visible;
            InvoiceReSync.Visibility = Visibility.Visible;
            Change_Content_of_Side_Menu_buttons("View_Orders");
        }

        private void View_Order_iteamProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object index = e.Source;
            int val = ((System.Windows.Controls.Primitives.Selector)index).SelectedIndex;
            View_Order__Index_No = val;


        }
        private FrameworkElement FindByName(string name, FrameworkElement root)
        {
            Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
            tree.Push(root);

            while (tree.Count > 0)
            {
                FrameworkElement current = tree.Pop();
                if (current.Name == name)
                    return current;

                int count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    if (child is FrameworkElement)
                        tree.Push((FrameworkElement)child);
                }
            }

            return null;
        }
        /// <summary>
        /// -View_Orders
        /// -Manual_Sync
        /// -Settings
        /// -About
        /// </summary>
        /// <param name="button_name"></param>
        public void Change_Content_of_Side_Menu_buttons(string button_name)
        {
            switch (button_name)
            {
                case "View_Orders":
                    Side_Menu_Page_View_Orders.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_View_Orders_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_View_Orders_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Manual_Sync.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Settings_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_About_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    BackTOCart_from_side_menu_page_text.Text = "View Orders";
                    Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Visible;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Hidden;
                    Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                    __Side_quickMenu_Page.Visibility = Visibility.Hidden;
                    Menu_Page_Product.Visibility = Visibility.Hidden;
                    View_Order_Completed_items.Clear();
                    Get_View_Orders();
                    break;

                case "Manual_Sync":
                    Side_Menu_Page_View_Orders.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_View_Orders_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_View_Orders_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Manual_Sync_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Settings.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Settings_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_About_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_Get_All_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_Message_text.Text = "Do You Want To Refresh The Local Database?";
                    BackTOCart_from_side_menu_page_text.Text = "Manual Sync";
                    Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Visible;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Hidden;
                    Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                    __Side_quickMenu_Page.Visibility = Visibility.Hidden;
                    Menu_Page_Product.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Manual_Sync_Content_Check_to_refresh.Visibility = Visibility.Visible;
                    Side_Menu_Page_Manual_Sync_Content_refresh.Visibility = Visibility.Hidden;
                    Check_keyboard_Focus = "Manual_Sync_GotFocus";
                    Side_Menu_Page_Manual_Sync_Yes.Focus();
                    Keyboard.Focus(Side_Menu_Page_Manual_Sync_Yes);
                    break;

                case "Settings":
                    Side_Menu_Page_View_Orders.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_View_Orders_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_View_Orders_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Settings_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Settings_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_About.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_About_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    BackTOCart_from_side_menu_page_text.Text = "Settings";
                    Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Visible;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Hidden;
                    break;

                case "About":
                    Side_Menu_Page_View_Orders.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_View_Orders_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_View_Orders_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Settings_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_About_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_About_text.Foreground = System.Windows.Media.Brushes.White;
                    BackTOCart_from_side_menu_page_text.Text = "About";
                    Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Visible;
                    break;
                case "Add_Products":
                    Side_Menu_Page_View_Orders.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_View_Orders_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_View_Orders_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Settings_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_About_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_About_text.Foreground = System.Windows.Media.Brushes.White;
                    BackTOCart_from_side_menu_page_text.Text = "Add_Products";
                    Check_keyboard_Focus = "Add_Products_View_GotFocus";
                    Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Visible;
                    break;
                default:
                    Side_Menu_Page_View_Orders.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_View_Orders_icon.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_View_Orders_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Manual_Sync.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Manual_Sync_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_Settings_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Settings_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About.Background = new SolidColorBrush(Colors.White);
                    Side_Menu_Page_About_icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_About_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    BackTOCart_from_side_menu_page_text.Text = "View Orders";
                    Side_Menu_Page_View_Orders_Content.Visibility = Visibility.Visible;
                    Side_Menu_Page_Manual_Sync_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_Settings_Content.Visibility = Visibility.Hidden;
                    Side_Menu_Page_About_Content.Visibility = Visibility.Hidden;
                    View_Order_Completed_items.Clear();
                    Get_View_Orders();
                    break;
            }
        }

        public void Get_View_Orders()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            connection.Open();
            NpgsqlCommand cmd_c_invoic_completed = new NpgsqlCommand("SELECT " +
                "t1.c_invoice_id, " +               //0
                "t1.ad_client_id, " +               //1
                "t1.ad_org_id, " +                  //2
                "t1.ad_role_id, " +                 //3
                "t1.ad_user_id, " +                 //4
                "t1.documentno, " +                 //5
                "t1.m_warehouse_id, " +             //6
                "t1.c_bpartner_id, " +              //7
                "t1.qid, " +                        //8
                "t1.mobilenumber, " +               //9
                "t1.discounttype, " +               //10
                "t1.discountvalue, " +              //11
                "t1.grandtotal, " +                 //12
                "t1.orderid, " +                    //13
                "t1.reason, " +                     //14
                "t1.created," +                     //15
                "t1.is_posted, " +                  //16
                "t1.grandtotal_round_off, " +       //17
                "t1.total_items_count, " +          //18
                "t1.balance, " +                    //19
                "t1.change, " +                     //20
                "t1.lossamount, " +                 //21
                "t1.extraamount, " +                //22
                "t2.name as c_bpartner_name, " +    //23
                "t2.searchkey, " +                  //24
                "t2.pricelistid, " +                //25
                "t2.iscredit, " +                   //26
                "t2.creditamount, " +               //27
                "t2.isdefault, " +                  //28
                "t2.iscashcustomer, " +              //29
                 "t1.is_completed, " +              //30
                 "t1.is_canceled " +              //31
                "FROM c_invoice t1 , c_bpartner t2 " +
                "Where is_completed = 'Y'  And t1.c_bpartner_id = t2.c_bpartner_id and t1.ad_client_id = " + AD_Client_ID + " and t1.ad_org_id =" + AD_ORG_ID + " and t1.ad_user_id = " + AD_USER_ID + " " + " Union SELECT " +
                "t1.c_invoice_id, " +               //0
                "t1.ad_client_id, " +               //1
                "t1.ad_org_id, " +                  //2
                "t1.ad_role_id, " +                 //3
                "t1.ad_user_id, " +                 //4
                "t1.documentno, " +                 //5
                "t1.m_warehouse_id, " +             //6
                "t1.c_bpartner_id, " +              //7
                "t1.qid, " +                        //8
                "t1.mobilenumber, " +               //9
                "t1.discounttype, " +               //10
                "t1.discountvalue, " +              //11
                "t1.grandtotal, " +                 //12
                "t1.orderid, " +                    //13
                "t1.reason, " +                     //14
                "t1.created," +                     //15
                "t1.is_posted, " +                  //16
                "t1.grandtotal_round_off, " +       //17
                "t1.total_items_count, " +          //18
                "t1.balance, " +                    //19
                "t1.change, " +                     //20
                "t1.lossamount, " +                 //21
                "t1.extraamount, " +                //22
                "t2.name as c_bpartner_name, " +    //23
                "t2.searchkey, " +                  //24
                "t2.pricelistid, " +                //25
                "t2.iscredit, " +                   //26
                "t2.creditamount, " +               //27
                "t2.isdefault, " +                  //28
                "t2.iscashcustomer, " +              //29
                 "t1.is_completed, " +              //30
                 "t1.is_canceled " +              //31
                "FROM c_invoice t1 , c_bpartner t2 " +
                "Where   is_canceled='Y' And t1.c_bpartner_id = t2.c_bpartner_id and t1.ad_client_id = " + AD_Client_ID + " and t1.ad_org_id =" + AD_ORG_ID + " and t1.ad_user_id = " + AD_USER_ID + " Order by created Desc;", connection);
            NpgsqlDataReader _get_c_invoic_completed = cmd_c_invoic_completed.ExecuteReader();
            if (_get_c_invoic_completed.HasRows == true)
            {
                Side_Menu_Page_View_Orders_Content_Empty.Visibility = Visibility.Hidden;
                while (_get_c_invoic_completed.Read())
                {
                    string _Status;
                    if (_get_c_invoic_completed.GetString(16) == "Y")
                    {
                        _Status = CoustomColors.CartBlueBrush;
                    }
                    else
                    {
                        _Status = CoustomColors.CartRedBrush;
                    }
                    string is_complete_Status = "";
                    string is_StrikeThrough = "";
                    if (_get_c_invoic_completed.GetString(30) == "Y")
                    {
                        is_complete_Status = "True";
                        is_StrikeThrough = "";
                    }
                    else if (_get_c_invoic_completed.GetString(31) == "Y")
                    {
                        is_complete_Status = "False";
                        is_StrikeThrough = "StrikeThrough";
                    }
                    View_Order_Completed_items.Add(new View_Order_Completed()
                    {
                        c_invoice_id = _get_c_invoic_completed.GetDouble(0).ToString(),
                        invoice_id = "#" + _get_c_invoic_completed.GetDouble(0).ToString(),
                        ad_client_id = _get_c_invoic_completed.GetDouble(1).ToString(),
                        ad_org_id = _get_c_invoic_completed.GetDouble(2).ToString(),
                        ad_role_id = _get_c_invoic_completed.GetDouble(3).ToString(),
                        ad_user_id = _get_c_invoic_completed.GetDouble(4).ToString(),
                        documentno = _get_c_invoic_completed.GetString(5),
                        m_warehouse_id = _get_c_invoic_completed.GetDouble(6).ToString(),
                        c_bpartner_id = _get_c_invoic_completed.GetDouble(7).ToString(),
                        qid = _get_c_invoic_completed.GetString(8),
                        mobilenumber = _get_c_invoic_completed.GetString(9),
                        discounttype = _get_c_invoic_completed.GetDouble(10).ToString(),
                        discountvalue = _get_c_invoic_completed.GetDouble(11).ToString(),
                        grandtotal = _get_c_invoic_completed.GetDouble(12).ToString(),
                        grandtotal_tx = "QR " + _get_c_invoic_completed.GetDouble(12).ToString(),
                        orderid = _get_c_invoic_completed.GetDouble(13).ToString(),
                        reason = _get_c_invoic_completed.GetString(14),
                        created = _get_c_invoic_completed.GetDateTime(15).ToString(),
                        is_posted = _get_c_invoic_completed.GetString(16),
                        grandtotal_round_off = _get_c_invoic_completed.GetDouble(17).ToString(),
                        total_items_count = _get_c_invoic_completed.GetDouble(18).ToString(),
                        items_count = _get_c_invoic_completed.GetDouble(18).ToString() + " Items",
                        balance = _get_c_invoic_completed.GetDouble(19).ToString(),
                        change = _get_c_invoic_completed.GetDouble(20).ToString(),
                        lossamount = _get_c_invoic_completed.GetDouble(21).ToString(),
                        extraamount = _get_c_invoic_completed.GetDouble(22).ToString(),
                        c_bpartner_name = _get_c_invoic_completed.GetString(23),
                        searchkey = _get_c_invoic_completed.GetString(24),
                        pricelistid = _get_c_invoic_completed.GetDouble(25).ToString(),
                        iscredit = _get_c_invoic_completed.GetString(26),
                        creditamount = _get_c_invoic_completed.GetDouble(27).ToString(),
                        isdefault = _get_c_invoic_completed.GetString(28),
                        iscashcustomer = _get_c_invoic_completed.GetString(29),
                        Status = _Status,
                        completedStatus = is_complete_Status,
                        StrikeThrough = is_StrikeThrough
                    });
                }
                connection.Close();
                View_Order_Completed_object = View_Order_Completed_items;
                ICollectionView view = CollectionViewSource.GetDefaultView(View_Order_Completed_object);
                view.Refresh();
                View_Order_iteamProduct.SelectedIndex = 0;
                View_Order_iteamProduct.Focus();
                Keyboard.Focus(View_Order_iteamProduct);


            }
            else
            {
                connection.Close();
                Side_Menu_Page_View_Orders_Content_Empty.Visibility = Visibility.Visible;
            }
            connection.Close();
        }

        private int View_Order__Index_No;

        private void View_Order_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isDuplicateCopy = Side_menu_Duplicate_Copy.IsChecked.Equals(true);
                var print_items = (sender as FrameworkElement).DataContext;
                int index = View_Order_iteamProduct.Items.IndexOf(print_items);
                View_Order_iteamProduct.SelectedIndex = index;
                RetailViewModel.Get_invoice_And_print(View_Order_Completed_items[index].c_invoice_id, isDuplicateCopy);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Side Menu

        #region Side Menu Page Manual Sync

        private async void Side_Menu_Page_Manual_Sync_Get_All_Data_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_NetworkUpStatus != true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Visible;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Hidden;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "“This Might Take Several Seconds!”";
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Hidden;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Visible;
                    });
                    Change_Color_of_Side_Menu_Manual_sync_get_data_buttons("Get_All_Data");
                    this.Dispatcher.Invoke(() =>
                    {
                        __Side_Menu_Page.IsEnabled = false;
                        Manual_Sync_ProgressBar.Visibility = Visibility.Visible;
                    });
                    await Task.Run(() =>
                    {
                        Manual_Sync_Get_All_Data();
                        dataSource.Clear();
                        // Bind_Product_Search();

                    });
                    this.Dispatcher.Invoke(() =>
                    {
                        __Side_Menu_Page.IsEnabled = true;
                        Manual_Sync_ProgressBar.Visibility = Visibility.Hidden;
                    });
                }
                Keyboard.Focus(this.Side_Menu_Manual_Sync);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private async void Side_Menu_Page_Manual_Sync_Get_Updated_Data_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_NetworkUpStatus != true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Visible;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Hidden;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "“This Might Take Several Seconds!”";
                        Side_Menu_Page_Manual_Sync_Content_No_Network.Visibility = Visibility.Hidden;
                        Side_Menu_Page_Manual_Sync_Content_with_Network.Visibility = Visibility.Visible;
                    });
                    Change_Color_of_Side_Menu_Manual_sync_get_data_buttons("Get_Updated_Data");
                    this.Dispatcher.Invoke(() =>
                    {
                        __Side_Menu_Page.IsEnabled = false;
                        Manual_Sync_ProgressBar.Visibility = Visibility.Visible;
                    });
                    await Task.Run(() =>
                    {
                        Manual_Sync_Get_Updated_Data();
                        dataSource.Clear();
                        // Bind_Product_Search();

                    });
                    Side_Menu_Page_Manual_Sync_Get_All_Data.Focus();
                    Keyboard.Focus(Side_Menu_Page_Manual_Sync_Get_All_Data);
                    __Side_Menu_Page.IsEnabled = true;
                    Manual_Sync_ProgressBar.Visibility = Visibility.Hidden;
                }
                Keyboard.Focus(this.Side_Menu_Manual_Sync);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Manual_Sync_Get_All_Data()
        {
            log.Info("Manual_Sync_Get_All_Data Method Called");
            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            #region Getting ALL Product CATEGORY for the orgination and client

            log.Info("Getting Product Category");
            POSCategory Product_Category = new POSCategory
            {
                operation = "POSCategory",
                remindMe = "Y",
                macAddress = DeviceMacAdd,//LoginViewModel._DeviceMacAddress;
                version = "1.0",
                appName = "POS",
                clientId = AD_Client_ID.ToString(),
                orgId = AD_ORG_ID.ToString(),
            };

            jsonPOSCategory = JsonConvert.SerializeObject(Product_Category);
            this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ...  "; });
            try
            {
                POSCategoryApiStringResponce = PostgreSQL.ApiCallPost(jsonPOSCategory);
                //this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .... "; });
                CheckServerError = 1;
                //this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ....."; });
            }
            catch
            {
                CheckServerError = 0;
                this.Dispatcher.Invoke(() =>
                {
                    Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error !!!";
                    Side_Menu_Page_Manual_Sync_Message_text.Text = "Get All Data Failed! Contact Admin";
                });
                log.Error("POSCategory Api Server Error");
                log.Info("Manual_Sync_Get_All_Data Method Exit");
                return;
            }
            if (CheckServerError == 1)
            {
                POSCategoryApiJSONResponce = JsonConvert.DeserializeObject(POSCategoryApiStringResponce);

                if (POSCategoryApiJSONResponce.responseCode == "200")
                {
                    //this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                    JArray _category = POSCategoryApiJSONResponce.category;

                    foreach (dynamic m_category in _category)
                    {
                        string _categoryId = m_category.categoryId;
                        string _categoryName = m_category.categoryName;
                        string _categoryValue = m_category.categoryValue;
                        string _categoryNameArabic = m_category.categoryNameArabic;
                        string _categoryImage = m_category.categoryImage;

                        connection.Open();
                        NpgsqlCommand cmd_m_product_category_check = new NpgsqlCommand("SELECT  id FROM m_product_category WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID +
                            "AND m_product_category_id =" + _categoryId + " ; ", connection);
                        NpgsqlDataReader _m_product_category_Read = cmd_m_product_category_check.ExecuteReader();

                        if (_m_product_category_Read.Read() && _m_product_category_Read.HasRows == true)
                        {
                            connection.Close();
                            log.Info("Updating m_product_category ID:" + _categoryId);
                            connection.Open();
                            NpgsqlCommand cmd_Up_ad_sys_config = new NpgsqlCommand("UPDATE m_product_category  SET updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby=" + AD_USER_ID + ", " +
                                "name=:_categoryName, " +
                                "searchkey=:_categoryValue, " +
                                "arabicname=:_categoryNameArabic, " +
                                "image=:_categoryImage " +
                                "WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_category_id =" + _categoryId + "; ", connection);
                            cmd_Up_ad_sys_config.Parameters.AddWithValue("_categoryName", _categoryName);
                            cmd_Up_ad_sys_config.Parameters.AddWithValue("_categoryValue", _categoryValue ?? "");
                            cmd_Up_ad_sys_config.Parameters.AddWithValue("_categoryNameArabic", _categoryNameArabic ?? "");
                            cmd_Up_ad_sys_config.Parameters.AddWithValue("_categoryImage", _categoryImage ?? "");
                            cmd_Up_ad_sys_config.ExecuteNonQuery();
                            connection.Close();
                        }
                        else
                        {
                            connection.Close();
                            //log.Info("Inserting new m_product_category ID:" + _categoryId);
                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_category';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                            if (_get__Ad_sequenc_no.Read())
                            {
                                _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                            }
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand INSERT_cmd_m_product_category = new NpgsqlCommand("INSERT INTO m_product_category(" +
                                "id, ad_client_id, ad_org_id, m_product_category_id," +
                                "createdby, updatedby, name, searchkey, arabicname," +
                                "image)" +
                                "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + _categoryId + "," +
                                AD_USER_ID + "," + AD_USER_ID + "," +
                                " @_categoryName,@_categoryValue,@_categoryNameArabic,@_categoryImage);", connection);
                            INSERT_cmd_m_product_category.Parameters.AddWithValue("@_categoryName", _categoryName ?? "");
                            INSERT_cmd_m_product_category.Parameters.AddWithValue("@_categoryValue", _categoryValue ?? "");
                            INSERT_cmd_m_product_category.Parameters.AddWithValue("@_categoryNameArabic", _categoryNameArabic ?? "");
                            INSERT_cmd_m_product_category.Parameters.AddWithValue("@_categoryImage", _categoryImage ?? "");
                            INSERT_cmd_m_product_category.ExecuteNonQuery();
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_category';", connection);
                            NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                            connection.Close();
                        }
                    }
                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ...  "; });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error: " + POSCategoryApiJSONResponce.responseCode + "!!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Get All Data Failed! Contact Admin";
                    });
                    log.Error("POSCategory Api JSON Responce  Error responseCode:" + POSCategoryApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
            }

            log.Info("Getting All Product");

            #endregion Getting ALL Product CATEGORY for the orgination and client

            POSAllProducts Product = new POSAllProducts
            {
                operation = "POSAllProducts",
                remindMe = "Y",
                macAddress = DeviceMacAdd,//LoginViewModel._DeviceMacAddress;
                version = "1.0",
                appName = "POS",
                clientId = AD_Client_ID.ToString(),
                orgId = AD_ORG_ID.ToString(),
                pricelistId = AD_PricelistID.ToString(),
                costElementId = AD_CostelementID.ToString(),
                warehouseId = AD_Warehouse_Id.ToString()
            };

            jsonPOSAllProducts = JsonConvert.SerializeObject(Product);
            try
            {
                POSAllProductsApiStringResponce = PostgreSQL.ApiCallPost(jsonPOSAllProducts);
                CheckServerErrorPOSAllProducts = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                CheckServerErrorPOSAllProducts = 0;
                this.Dispatcher.Invoke(() =>
                {
                    Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error !!!";
                    Side_Menu_Page_Manual_Sync_Message_text.Text = "Get Updated Data Failed! Contact Admin";
                });
                log.Error("GetUpdatedData Api Server Error");
                log.Info("Manual_Sync_Get_All_Data Method Exit");
                return;
            }
            if (CheckServerErrorPOSAllProducts == 1)
            {
                POSAllProductsApiJSONResponce = JsonConvert.DeserializeObject(POSAllProductsApiStringResponce);
                string _itemCount = _itemCount = POSAllProductsApiJSONResponce.itemCount ?? "0";
                if (POSAllProductsApiJSONResponce.responseCode == "200")
                {
                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .... "; });
                    JArray _products = POSAllProductsApiJSONResponce.products;
                    int _products_count = _products.Count();
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand UPDATE_cmd_m_warehouse = new NpgsqlCommand("UPDATE m_warehouse SET attribute1 ='" + _itemCount + "' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = " + AD_Warehouse_Id + "; ", connection);
                    UPDATE_cmd_m_warehouse.ExecuteNonQuery();
                    connection.Close();

                    //delete all t he  products before insert new product in manual sync
                    if (_products_count > 0)
                    {
                        connection.Open();
                        NpgsqlCommand cmd_m_product_delete = new NpgsqlCommand("delete FROM m_product WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + "; ", connection);
                        cmd_m_product_delete.ExecuteReader();
                        connection.Close();

                    }


                    foreach (dynamic m_product in _products)
                    {
                        string _productId = m_product.productId;
                        string _productName = m_product.productName;
                        string _productValue = m_product.productValue;
                        string _categoryId = m_product.categoryId;
                        string _productArabicName = m_product.productArabicName ?? " ";
                        string _description = m_product.description;
                        string _scanbyWeight = m_product.scanbyWeight;
                        string _scanbyPrice = m_product.scanbyPrice;
                        string _productUOMId = m_product.productUOMId;
                        string _productUOMValue = m_product.productUOMValue;
                        string _sellingPrice = m_product.sellingPrice;
                        string _costprice = m_product.costprice;
                        string _purchaseprice = m_product.costprice;
                        string _productMultiUOM = m_product.productMultiUOM;
                        JArray _productMultiImage = m_product.productMultiImage;
                        JArray _productPriceArray = m_product.productPriceArray;
                        JArray _productsUOMArray = m_product.productsUOMArray;
                        int img_count = 0;
                        if (_productMultiImage != null)
                        {
                            img_count = _productMultiImage.Count;
                        }
                        string _product_image = " ";
                        if (img_count > 0)
                        {
                            _product_image = _productMultiImage[0]["productImage"].ToString();
                        }

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_m_product_check = new NpgsqlCommand("SELECT  * FROM m_product WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " ; ", connection);
                        NpgsqlDataReader _m_product_Read = cmd_m_product_check.ExecuteReader();

                        if (_m_product_Read.Read() && _m_product_Read.HasRows == true)
                        {
                            connection.Close();
                            connection.Open();
                            //NpgsqlCommand UPDATE_cmd_m_product = new NpgsqlCommand("UPDATE m_product SET " +
                            //    "m_product_category_id=" + _categoryId + "," +
                            //    "updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', " +
                            //    "updatedby= " + AD_USER_ID + ", " +
                            //    "name='" + _productName + "', " +
                            //    "searchkey='" + _productValue + "', " +
                            //    "arabicname='" + _productArabicName + "', " +
                            //    "image='" + _product_image + "', " +
                            //    "scanbyweight='" + _scanbyWeight + "', " +
                            //    "scanbyprice='" + _scanbyPrice + "', " +
                            //    "uomid=" + _productUOMId + ", " +
                            //    "uomname='" + _productUOMValue + "', " +
                            //    "sopricestd=" + _sellingPrice + ", " +
                            //    "currentcostprice=" + _costprice + ", " +
                            //    "attribute1='" + _productMultiUOM + "', " +
                            //    "attribute2='" + AD_Warehouse_Id + "' " +
                            //    " WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " ; ", connection);
                            NpgsqlCommand UPDATE_cmd_m_product = new NpgsqlCommand("UPDATE m_product SET " +
                                "m_product_category_id=  :_categoryId  , " +
                                "updated=  ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'   ,  " +
                                "updatedby =   " + AD_USER_ID + "  ,  " +
                                "name=   :_productName   ,  " +
                                "searchkey=   :_productValue   ,  " +
                                "arabicname=   :_productArabicName   ,  " +
                                "image=   :_product_image   ,  " +
                                "scanbyweight=   :_scanbyWeight   ,  " +
                                "scanbyprice=   :_scanbyPrice   ,  " +
                                "uomid =  " + _productUOMId + "  ,  " +
                                "uomname=   :_productUOMValue   ,  " +
                                "sopricestd =  " + _sellingPrice + "  ,  " +
                                "currentcostprice=  " + _costprice + "  ,  " +
                                "purchaseprice=  " + _purchaseprice + "  ,  " +
                                "attribute1=   :_productMultiUOM   ,  " +
                                "attribute2 =   :AD_Warehouse_Id     " +
                                "WHERE ad_client_id =   " + AD_Client_ID + "  AND ad_org_id =   " + AD_ORG_ID + "  AND  m_product_id =   " + _productId + "   ;", connection);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_categoryId", Convert.ToInt64(_categoryId));
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_productName", _productName);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_productValue", _productValue);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_productArabicName", _productArabicName);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_product_image", _product_image);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_scanbyWeight", _scanbyWeight);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_scanbyPrice", _scanbyPrice);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_productUOMValue", _productUOMValue);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("_productMultiUOM", _productMultiUOM);
                            UPDATE_cmd_m_product.Parameters.AddWithValue("AD_Warehouse_Id", AD_Warehouse_Id);
                            UPDATE_cmd_m_product.ExecuteNonQuery();
                            connection.Close();

                            foreach (dynamic m_productPriceArray in _productPriceArray)
                            {
                                string _productId_PriceArray = m_productPriceArray.productId;
                                string _pricelistId_PriceArray = m_productPriceArray.pricelistId;
                                if (string.IsNullOrEmpty(_pricelistId_PriceArray))
                                    _pricelistId_PriceArray = "0";
                                string _pricelistName_PriceArray = m_productPriceArray.pricelistName;
                                string _priceStd_PriceArray = m_productPriceArray.priceStd;
                                if (string.IsNullOrEmpty(_priceStd_PriceArray))
                                    _priceStd_PriceArray = "0";
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_m_product_price_check = new NpgsqlCommand("SELECT  * FROM m_product_price WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND pricelistid = " + _pricelistId_PriceArray + "; ", connection);
                                NpgsqlDataReader _m_product_price_Read = cmd_m_product_check.ExecuteReader();

                                if (_m_product_price_Read.Read() && _m_product_price_Read.HasRows == true)
                                {
                                    connection.Close();
                                    connection.Open();
                                    NpgsqlCommand UPDATE_product_price = new NpgsqlCommand("UPDATE m_product_price SET pricestd = " + _priceStd_PriceArray + ", updated ='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby =" + AD_USER_ID + ", name = '" + _pricelistName_PriceArray + "'  WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND pricelistid = " + _pricelistId_PriceArray + "; ", connection);
                                    UPDATE_product_price.ExecuteReader();
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ....."; });
                                }
                                else
                                {
                                    connection.Close();
                                    log.Info("Inserting New m_product_price  ID " + _pricelistId_PriceArray + " for m_product_id:" + _productId);
                                    connection.Open();
                                    NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                    NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                    if (_get__Ad_sequenc_no_m_product_price.Read())
                                    {
                                        _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                    }
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                            "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_price';", connection);
                                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ....."; });
                                }
                                connection.Close();
                            }
                            if (_productMultiUOM == "Y")
                            {
                                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                foreach (dynamic m_productsUOMArray in _productsUOMArray)
                                {
                                    string _uomId_UOMArray = m_productsUOMArray.uomId;
                                    string _uomValue_UOMArray = m_productsUOMArray.uomValue;
                                    string _uomConvRate_UOMArray = m_productsUOMArray.uomConvRate;
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    connection.Close();
                                    connection.Open();
                                    string cmd_m_product_uom_check_string = "SELECT  * FROM m_product_uom WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND uomid = " + _uomId_UOMArray + "; ";
                                    NpgsqlCommand cmd_m_product_uom_check = new NpgsqlCommand(cmd_m_product_uom_check_string, connection);
                                    NpgsqlDataReader _m_product_uom_Read = cmd_m_product_check.ExecuteReader();

                                    if (_m_product_uom_Read.Read() && _m_product_uom_Read.HasRows == true)
                                    {
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                        connection.Close();
                                        connection.Open();
                                        NpgsqlCommand UPDATE_product_price = new NpgsqlCommand("UPDATE m_product_uom SET updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby=" + AD_USER_ID + ", uomvalue='" + _uomValue_UOMArray + "',uomconvrate=" + _uomConvRate_UOMArray + "  WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND uomid = " + _uomId_UOMArray + "; ", connection);
                                        UPDATE_product_price.ExecuteReader();
                                        connection.Close();
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                        connection.Close();
                                        log.Info("Inserting New m_product_uom  ID " + _uomId_UOMArray + " for m_product_id:" + _productId);
                                        connection.Open();
                                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                        if (_get__Ad_sequenc_no_m_product_price.Read())
                                        {
                                            _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                        }
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                        " VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                        connection.Close();
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    }
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                }
                            }
                        }
                        else
                        {
                            connection.Close();
                            log.Info("Inserting New Product");
                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                            this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                            if (_get__Ad_sequenc_no.Read())
                            {
                                _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                            }
                            connection.Close();
                            if (!check_product_exist(_productId))
                            {
                                connection.Open();
                                NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product';", connection);
                                NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                                connection.Close();

                                connection.Open();
                                NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,purchaseprice, attribute1,attribute2,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@purchaseprice, @attribute1, @attribute2,@created)", connection);

                                INSERT_cmd_m_product.Parameters.AddWithValue("@id", _sequenc_id);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchaseprice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", AD_Warehouse_Id);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                                INSERT_cmd_m_product.ExecuteNonQuery();
                                connection.Close();


                                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                                foreach (dynamic m_productPriceArray in _productPriceArray)
                                {
                                    string _productId_PriceArray = m_productPriceArray.productId;
                                    string _pricelistId_PriceArray = m_productPriceArray.pricelistId;
                                    if (string.IsNullOrEmpty(_pricelistId_PriceArray))
                                        _pricelistId_PriceArray = "0";
                                    string _pricelistName_PriceArray = m_productPriceArray.pricelistName;
                                    string _priceStd_PriceArray = m_productPriceArray.priceStd;
                                    if (string.IsNullOrEmpty(_priceStd_PriceArray))
                                        _priceStd_PriceArray = "0";

                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                    NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                    if (_get__Ad_sequenc_no_m_product_price.Read())
                                    {
                                        _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                    }
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                            "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_price';", connection);
                                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                    connection.Close();
                                }
                                if (_productMultiUOM == "Y")
                                {
                                    foreach (dynamic m_productsUOMArray in _productsUOMArray)
                                    {
                                        string _uomId_UOMArray = m_productsUOMArray.uomId;
                                        string _uomValue_UOMArray = m_productsUOMArray.uomValue;
                                        string _uomConvRate_UOMArray = m_productsUOMArray.uomConvRate;

                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                        if (_get__Ad_sequenc_no_m_product_price.Read())
                                        {
                                            _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                        }
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                        " VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                        connection.Close();
                                    }
                                }
                            }
                            else
                            {
                                log.Info("Product Already exist  ProductId:" + _productId);

                            }
                            this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                        }
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Success !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "All Products Data Synced";
                    });
                }
                else if (POSAllProductsApiJSONResponce.responseCode == "101")
                {
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand UPDATE_cmd_m_warehouse = new NpgsqlCommand("UPDATE m_warehouse SET attribute1 ='" + _itemCount + "' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = " + AD_Warehouse_Id + "; ", connection);
                    UPDATE_cmd_m_warehouse.ExecuteNonQuery();
                    connection.Close();
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Success !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "All Data Are Up-To-Date!";
                    });
                    log.Info("POSAllProducts Api JSON Responce  responseCode:" + POSAllProductsApiJSONResponce.responseCode);
                }
                else if (POSAllProductsApiJSONResponce.responseCode == "301")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Error";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Device Not Registered";
                    });
                    log.Info("POSAllProducts Api JSON Responce  responseCode:" + POSAllProductsApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
                else if (POSAllProductsApiJSONResponce.responseCode == "401")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Error";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Session Expired or No Session";
                    });
                    log.Info("POSAllProducts Api JSON Responce  responseCode:" + POSAllProductsApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Get All Data Failed! Contact Admin";
                    });
                    log.Error("POSAllProducts Api JSON Responce  Error responseCode:" + POSAllProductsApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
            }
            log.Info("Manual_Sync_Get_All_Data Method Success! Exiting");
        }

        private void Manual_Sync_Get_Updated_Data()
        {
            log.Info("Manual_Sync_Get_Updated_Data Method Called");
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            log.Info("Getting Updated Product");

            POSAllProducts GetUpdatedData = new POSAllProducts
            {
                operation = "GetUpdatedData",
                remindMe = "Y",
                macAddress = DeviceMacAdd,//LoginViewModel._DeviceMacAddress;
                version = "1.0",
                appName = "POS",
                clientId = AD_Client_ID.ToString(),
                orgId = AD_ORG_ID.ToString(),
                pricelistId = AD_PricelistID.ToString(),
                costElementId = AD_CostelementID.ToString(),
                warehouseId = AD_Warehouse_Id.ToString()
            };

            jsonGetUpdatedData = JsonConvert.SerializeObject(GetUpdatedData);
            this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ...  "; });
            try
            {
                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .... "; });
                GetUpdatedDataApiStringResponce = PostgreSQL.ApiCallPost(jsonGetUpdatedData);
                CheckServerError = 1;
            }
            catch
            {
                CheckServerError = 0;
            }
            if (CheckServerError == 1)
            {
                GetUpdatedDataApiJSONResponce = JsonConvert.DeserializeObject(GetUpdatedDataApiStringResponce);

                if (GetUpdatedDataApiJSONResponce.responseCode == "200")
                {
                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                    JArray _category = GetUpdatedDataApiJSONResponce.category;
                    JArray _products = GetUpdatedDataApiJSONResponce.products;
                    foreach (dynamic m_category in _category)
                    {
                        string _categoryId = m_category.categoryId;
                        string _categoryName = m_category.categoryName;
                        string _categoryValue = m_category.categoryValue;
                        string _categoryNameArabic = m_category.categoryNameArabic;
                        string _categoryImage = m_category.categoryImage;

                        connection.Open();
                        NpgsqlCommand cmd_m_product_category_check = new NpgsqlCommand("SELECT  id FROM m_product_category WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID +
                            "AND m_product_category_id =" + _categoryId + " ; ", connection);
                        NpgsqlDataReader _m_product_category_Read = cmd_m_product_category_check.ExecuteReader();

                        if (_m_product_category_Read.Read() && _m_product_category_Read.HasRows == true)
                        {
                            connection.Close();
                            //log.Info("Updating m_product_category ID:"+ _categoryId);
                            connection.Open();
                            NpgsqlCommand cmd_Up_ad_sys_config = new NpgsqlCommand("UPDATE m_product_category  SET updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby=" + AD_USER_ID + ", name='" + _categoryName + "', searchkey='" + _categoryValue + "', arabicname='" + _categoryNameArabic + "', image='" + _categoryImage + "' " +
                                "WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_category_id =" + _categoryId + "; ", connection);
                            NpgsqlDataReader _Up_ad_sys_config_Read = cmd_Up_ad_sys_config.ExecuteReader();
                            connection.Close();
                        }
                        else
                        {
                            connection.Close();
                            //log.Info("Inserting new m_product_category ID:" + _categoryId);
                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_category';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                            if (_get__Ad_sequenc_no.Read())
                            {
                                _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                            }
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand INSERT_cmd_m_product_category = new NpgsqlCommand("INSERT INTO m_product_category(" +
                                "id, ad_client_id, ad_org_id, m_product_category_id," +
                                "createdby, updatedby, name, searchkey, arabicname," +
                                "image)" +
                                "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + _categoryId + "," +
                                AD_USER_ID + "," + AD_USER_ID + ",'" + _categoryName + "'," + _categoryValue + ",'" + _categoryNameArabic + "','" + _categoryImage + "');", connection);
                            INSERT_cmd_m_product_category.ExecuteNonQuery();
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_category';", connection);
                            NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                            connection.Close();
                        }
                    }
                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ...  "; });
                    int _products_count = _products.Count();
                    string _itemCount = GetUpdatedDataApiJSONResponce.itemCount;
                    foreach (dynamic m_product in _products)
                    {
                        string _productId = m_product.productId;
                        string _productName = m_product.productName;
                        string _productValue = m_product.productValue;
                        string _categoryId = m_product.categoryId;
                        string _productArabicName = m_product.productArabicName ?? " ";
                        string _description = m_product.description;
                        string _scanbyWeight = m_product.scanbyWeight;
                        string _scanbyPrice = m_product.scanbyPrice;
                        string _productUOMId = m_product.productUOMId;
                        string _productUOMValue = m_product.productUOMValue;
                        string _sellingPrice = m_product.sellingPrice;
                        string _costprice = m_product.costprice;
                        string _productMultiUOM = m_product.productMultiUOM;
                        JArray _productMultiImage = m_product.productMultiImage;
                        JArray _productPriceArray = m_product.productPriceArray;
                        JArray _productsUOMArray = m_product.productsUOMArray;
                        int img_count = _productMultiImage.Count;
                        string _product_image = " ";
                        if (img_count > 0)
                        {
                            _product_image = _productMultiImage[0]["productImage"].ToString();
                        }

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_m_product_check = new NpgsqlCommand("SELECT  * FROM m_product WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " ; ", connection);
                        NpgsqlDataReader _m_product_Read = cmd_m_product_check.ExecuteReader();

                        if (_m_product_Read.Read() && _m_product_Read.HasRows == true)
                        {
                            connection.Close();
                            connection.Open();
                            NpgsqlCommand UPDATE_cmd_m_product = new NpgsqlCommand("UPDATE m_product SET " +
                                "m_product_category_id=" + _categoryId + "," +
                                "updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', " +
                                "updatedby= " + AD_USER_ID + ", " +
                                "name='" + _productName + "', " +
                                "searchkey='" + _productValue + "', " +
                                "arabicname='" + _productArabicName + "', " +
                                "image='" + _product_image + "', " +
                                "scanbyweight='" + _scanbyWeight + "', " +
                                "scanbyprice='" + _scanbyPrice + "', " +
                                "uomid=" + _productUOMId + ", " +
                                "uomname='" + _productUOMValue + "', " +
                                "sopricestd=" + _sellingPrice + ", " +
                                "currentcostprice=" + _costprice + ", " +
                                "purchaseprice=" + _costprice + ", " +
                                "attribute1='" + _productMultiUOM + "', " +
                                "attribute2='" + AD_Warehouse_Id + "' " +
                                " WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " ; ", connection);
                            UPDATE_cmd_m_product.ExecuteNonQuery();
                            connection.Close();

                            foreach (dynamic m_productPriceArray in _productPriceArray)
                            {
                                string _productId_PriceArray = m_productPriceArray.productId;
                                string _pricelistId_PriceArray = m_productPriceArray.pricelistId;
                                if (string.IsNullOrEmpty(_pricelistId_PriceArray))
                                    _pricelistId_PriceArray = "0";
                                string _pricelistName_PriceArray = m_productPriceArray.pricelistName;
                                string _priceStd_PriceArray = m_productPriceArray.priceStd;
                                if (string.IsNullOrEmpty(_priceStd_PriceArray))
                                    _priceStd_PriceArray = "0";
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_m_product_price_check = new NpgsqlCommand("SELECT  * FROM m_product_price WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND pricelistid = " + _pricelistId_PriceArray + "; ", connection);
                                NpgsqlDataReader _m_product_price_Read = cmd_m_product_check.ExecuteReader();
                                connection.Close();
                                if (_m_product_price_Read.Read() && _m_product_price_Read.HasRows == true)
                                {
                                    connection.Close();
                                    connection.Open();
                                    NpgsqlCommand UPDATE_product_price = new NpgsqlCommand("UPDATE m_product_price SET pricestd = " + _priceStd_PriceArray + ", updated ='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby =" + AD_USER_ID + ", name = '" + _pricelistName_PriceArray + "'  WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND pricelistid = " + _pricelistId_PriceArray + "; ", connection);
                                    UPDATE_product_price.ExecuteReader();
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ....."; });
                                }
                                else
                                {
                                    connection.Close();
                                    //log.Info("Inserting New m_product_price  ID " + _pricelistId_PriceArray + " for m_product_id:" + _productId);
                                    connection.Open();
                                    NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                    NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                    if (_get__Ad_sequenc_no_m_product_price.Read())
                                    {
                                        _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                    }
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                            "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_price';", connection);
                                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ....."; });
                                }
                                connection.Close();
                            }
                            if (_productMultiUOM == "Y")
                            {
                                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                foreach (dynamic m_productsUOMArray in _productsUOMArray)
                                {
                                    string _uomId_UOMArray = m_productsUOMArray.uomId;
                                    string _uomValue_UOMArray = m_productsUOMArray.uomValue;
                                    string _uomConvRate_UOMArray = m_productsUOMArray.uomConvRate;
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    connection.Close();
                                    connection.Open();
                                    NpgsqlCommand cmd_m_product_uom_check = new NpgsqlCommand("SELECT  * FROM m_product_uom WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND uomid = " + _uomId_UOMArray + "; ", connection);
                                    NpgsqlDataReader _m_product_uom_Read = cmd_m_product_check.ExecuteReader();
                                    connection.Close();
                                    if (_m_product_uom_Read.Read() && _m_product_uom_Read.HasRows == true)
                                    {
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                        connection.Close();
                                        connection.Open();
                                        NpgsqlCommand UPDATE_product_price = new NpgsqlCommand("UPDATE m_product_uom SET updated='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', updatedby=" + AD_USER_ID + ", uomvalue='" + _uomValue_UOMArray + "',uomconvrate=" + _uomConvRate_UOMArray + "  WHERE ad_client_id = " + AD_Client_ID + "AND ad_org_id = " + AD_ORG_ID + " AND m_product_id = " + _productId + " AND uomid = " + _uomId_UOMArray + "; ", connection);
                                        UPDATE_product_price.ExecuteReader();
                                        connection.Close();
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                        connection.Close();
                                        //log.Info("Inserting New m_product_uom  ID " + _uomId_UOMArray + " for m_product_id:" + _productId);
                                        connection.Open();
                                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                        if (_get__Ad_sequenc_no_m_product_price.Read())
                                        {
                                            _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                        }
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                        " VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                        connection.Close();
                                        this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                    }
                                    connection.Close();
                                    this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait .    "; });
                                }
                            }
                        }
                        else
                        {
                            connection.Close();
                            //log.Info("Inserting New Product");
                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                            this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                            if (_get__Ad_sequenc_no.Read())
                            {
                                _sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                            }
                            connection.Close();
                            if (!check_product_exist(_productId))
                            {
                                connection.Open();
                                NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product';", connection);
                                NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(" +
                                "id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey," +
                                "arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,purchaseprice, attribute1,attribute2,created)" +
                                "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + _productId + "," + _categoryId + "," + AD_USER_ID + "," + AD_USER_ID + ",'" + _productName + "','" + _productValue + "'," +
                                " '" + _productArabicName + "','" + _product_image + "','" + _scanbyWeight + "','" + _scanbyPrice + "'," + _productUOMId + ",'" + _productUOMValue + "'," + _sellingPrice + "," + _costprice + "," + _costprice + ",'" + _productMultiUOM + "','" + AD_Warehouse_Id + "','" + DateTime.Now + "'); ", connection);
                                INSERT_cmd_m_product.ExecuteNonQuery();
                                connection.Close();


                                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                                foreach (dynamic m_productPriceArray in _productPriceArray)
                                {
                                    string _productId_PriceArray = m_productPriceArray.productId;
                                    string _pricelistId_PriceArray = m_productPriceArray.pricelistId;
                                    if (string.IsNullOrEmpty(_pricelistId_PriceArray))
                                        _pricelistId_PriceArray = "0";
                                    string _pricelistName_PriceArray = m_productPriceArray.pricelistName;
                                    string _priceStd_PriceArray = m_productPriceArray.priceStd;
                                    if (string.IsNullOrEmpty(_priceStd_PriceArray))
                                        _priceStd_PriceArray = "0";

                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                    NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                    if (_get__Ad_sequenc_no_m_product_price.Read())
                                    {
                                        _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                    }
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                            "VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_price';", connection);
                                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                    connection.Close();
                                }
                                if (_productMultiUOM == "Y")
                                {
                                    foreach (dynamic m_productsUOMArray in _productsUOMArray)
                                    {
                                        string _uomId_UOMArray = m_productsUOMArray.uomId;
                                        string _uomValue_UOMArray = m_productsUOMArray.uomValue;
                                        string _uomConvRate_UOMArray = m_productsUOMArray.uomConvRate;

                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                        if (_get__Ad_sequenc_no_m_product_price.Read())
                                        {
                                            _sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                        }
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                        " VALUES(" + _sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                        connection.Close();
                                    }
                                }
                                this.Dispatcher.Invoke(() => { Side_Menu_Page_Manual_Sync_Message_text.Text = "Please Wait ..   "; });
                            }
                            else
                            {
                                log.Info("Product Already exist  ProductId:" + _productId);

                            }
                        }
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Success !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "All Products Data Synced";
                    });
                }
                else if (GetUpdatedDataApiJSONResponce.responseCode == "101")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Success !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "All Data Are Up-To-Date!";
                    });
                    log.Info("GetUpdatedData Api JSON Responce  responseCode:" + GetUpdatedDataApiJSONResponce.responseCode);
                }
                else if (GetUpdatedDataApiJSONResponce.responseCode == "301")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Error";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Device Not Registered";
                    });
                    log.Info("GetUpdatedData Api JSON Responce  responseCode:" + GetUpdatedDataApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
                else if (GetUpdatedDataApiJSONResponce.responseCode == "401")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Error";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Session Expired or No Session";
                    });
                    log.Info("GetUpdatedData Api JSON Responce  responseCode:" + GetUpdatedDataApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error !!!";
                        Side_Menu_Page_Manual_Sync_Message_text.Text = "Get All Data Failed! Contact Admin";
                    });
                    log.Error("GetUpdatedData Api JSON Responce  Error responseCode:" + GetUpdatedDataApiJSONResponce.responseCode);
                    log.Info("Manual_Sync_Get_All_Data Method Exit");
                    return;
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    Side_Menu_Page_Manual_Sync_Product_text.Text = "Server Error !!!";
                    Side_Menu_Page_Manual_Sync_Message_text.Text = "Get Updated Data Failed! Contact Admin";
                });
                log.Error("GetUpdatedData Api Server Error");
                log.Info("Manual_Sync_Get_All_Data Method Exit");
                return;
            }
            log.Info("Manual_Sync_Get_Updated_Data Method Success! Exiting");
        }

        /// <summary>
        /// 1-Get_All_Data
        /// 2-Get_Updated_Data
        /// </summary>
        /// <param name="button_name"></param>
        public void Change_Color_of_Side_Menu_Manual_sync_get_data_buttons(string button_name)
        {
            switch (button_name)
            {
                case "Get_All_Data":
                    Side_Menu_Page_Manual_Sync_Get_All_Data.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.Background = null;
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    break;

                case "Get_Updated_Data":
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data_text.Foreground = System.Windows.Media.Brushes.White;
                    Side_Menu_Page_Manual_Sync_Get_All_Data.Background = null;
                    Side_Menu_Page_Manual_Sync_Get_All_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    break;

                default:
                    Side_Menu_Page_Manual_Sync_Get_All_Data.Background = null;
                    Side_Menu_Page_Manual_Sync_Get_All_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_All_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.Background = null;
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBorderGreyBrush);
                    Side_Menu_Page_Manual_Sync_Get_Updated_Data_text.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    break;
            }
        }

        private void Side_Menu_Page_Manual_Sync_No_Click(object sender, RoutedEventArgs e)
        {
            Back_OR_Esc();
        }

        private void Side_Menu_Page_Manual_Sync_Yes_Click(object sender, RoutedEventArgs e)
        {
            Side_Menu_Page_Manual_Sync_Content_Check_to_refresh.Visibility = Visibility.Hidden;
            Side_Menu_Page_Manual_Sync_Content_refresh.Visibility = Visibility.Visible;
            Side_Menu_Page_Manual_Sync_Product_text.Text = "“This Might Take Several Seconds!”";
            Side_Menu_Page_Manual_Sync_Message_text.Text = "Choose Your Mode of Refresh?";
            Side_Menu_Page_Manual_Sync_Get_Updated_Data.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
            Side_Menu_Page_Manual_Sync_Get_Updated_Data.Focus();
            Keyboard.Focus(Side_Menu_Page_Manual_Sync_Get_Updated_Data);
        }

        #endregion Side Menu Page Manual Sync

        #region Errors | PopUp

        #region Login Error

        private async void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    BackToLogin.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(_Colors.CartBlueBrush);
                    //    BackToLogin.Foreground = new SolidColorBrush(Colors.White);
                    //});
                    Thread.Sleep(500);
                });
                Error_page.Visibility = Visibility.Hidden;
                LoginError.Visibility = Visibility.Hidden;
                ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Login;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Login Error

        #region Session

        private void SessionResume_Click(object sender, RoutedEventArgs e)
        {
            ResumeSession();
            Error_page.Visibility = Visibility.Hidden;
            Session_Check.Visibility = Visibility.Hidden;
            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
            Back_OR_Esc();
            Keyboard.Focus(productSearch_cart);
        }

        private void SessionCreateNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateNewSession();
                #region Get Invoice & Doc No

                //Checking and Getting Invoice POS Number
                var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                string _responce_code = Check_POS_Number_rs.Item3;
                bool _network_status_ = Check_POS_Number_rs.Item4;
                if (_responce_code == "0" || _responce_code == "200")
                {
                    invoice_number = Check_POS_Number_rs.Item1;
                    document_no = Check_POS_Number_rs.Item2;
                    InvoiceNo.Text = invoice_number.ToString();
                }
                else if (_network_status_ != true || _responce_code == "500")
                {
                    Error_page.Visibility = Visibility.Visible;
                    NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                    return;
                }

                #endregion Get Invoice & Doc No

                string balance = "0";
                if (txt_openingBal.Text != "")
                {
                    balance = txt_openingBal.Text;
                }
                NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                connection.Open();
                long Session_StartTimeMilliseconds = new DateTimeOffset(Convert.ToDateTime(AD_Session_Started_at)).ToUnixTimeMilliseconds();

                NpgsqlCommand INSERT_c_invoice = new NpgsqlCommand("INSERT INTO c_invoice(" +
                "c_invoice_id, ad_client_id, ad_org_id, ad_role_id, ad_user_id," +
                "documentno, m_warehouse_id, c_bpartner_id, qid, mobilenumber," +
                " orderid, reason, createdby, updatedby," +
                "openingbalance,session_id)" +
                "VALUES(" + invoice_number + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_ROLE_ID + "," + AD_USER_ID + "," + document_no + "," + AD_Warehouse_Id + "," + AD_bpartner_Id + ",'" + txtCustomer_CR.Text + "','" + txtCustomermobile.Text + "'" +
                ",0,''," + AD_USER_ID + "," + AD_USER_ID + "," + balance + "," +
                 Session_StartTimeMilliseconds + ");", connection);
                INSERT_c_invoice.ExecuteNonQuery();
                connection.Close();
                Error_page.Visibility = Visibility.Hidden;
                Session_Check.Visibility = Visibility.Hidden;
                txt_openingBal.Visibility = Visibility.Hidden;
                Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                Back_OR_Esc();
                Keyboard.Focus(productSearch_cart);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }

        private void CreateNewSession()
        {

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            SessionCart CreateSession = new SessionCart
            {
                operation = "createSession",
                username = AD_UserName,
                password = AD_UserPassword,
                clientId = AD_Client_ID.ToString(),
                orgId = AD_ORG_ID.ToString(),
                userId = AD_USER_ID.ToString(),
                businessPartnerId = AD_bpartner_Id.ToString(),
                roleId = AD_ROLE_ID.ToString(),
                warehouseId = AD_Warehouse_Id.ToString(),
                remindMe = "Y",
                macAddress = DeviceMacAdd, //LoginViewModel._DeviceMacAddress,
                version = "1.0",
                appName = "POS",
            };
            Back_OR_Esc();
            jsonCreateSession = JsonConvert.SerializeObject(CreateSession);
            try
            {
                CreateSessionApiStringResponce = PostgreSQL.ApiCallPost(jsonCreateSession);
                Percentage_OR_Price = "%";
                OverAllDiscount_button.IsEnabled = false;
                OrderComplected_button.IsEnabled = false;
                OrderCancel_button.IsEnabled = false;
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
                    AD_SessionID = _sessionId;

                    connection.Close();
                    string start_time = DateTime.Now.ToString();
                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("UPDATE ad_user_pos " +
                    "SET sessionid = " + _sessionId + ",attribute1 ='" + start_time + "' " +
                    "WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and ad_user_id = " + AD_USER_ID + " ; ", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                    connection.Close();
                    AD_Session_Started_at = start_time;
                    SessionClose_User_Start_Time.Text = string.Format(new MyCustomDateProvider(), "{0}", Convert.ToDateTime(AD_Session_Started_at));
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
        }

        private void ResumeSession()
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            SessionCart ResumeSession = new SessionCart
            {
                operation = "resumeSession",
                username = AD_UserName,
                password = AD_UserPassword,
                clientId = AD_Client_ID.ToString(),
                orgId = AD_ORG_ID.ToString(),
                userId = AD_USER_ID.ToString(),
                businessPartnerId = AD_bpartner_Id.ToString(),
                roleId = AD_ROLE_ID.ToString(),
                warehouseId = AD_Warehouse_Id.ToString(),
                remindMe = "Y",
                macAddress = DeviceMacAdd, //LoginViewModel._DeviceMacAddress,
                version = "1.0",
                appName = "POS",
                sessionId = AD_SessionID.ToString(),
            };
            Back_OR_Esc();
            jsonResumeSession = JsonConvert.SerializeObject(ResumeSession);
            try
            {
                ResumeSessionApiStringResponce = PostgreSQL.ApiCallPost(jsonResumeSession);
                Percentage_OR_Price = "%";
                OverAllDiscount_button.IsEnabled = false;
                OrderComplected_button.IsEnabled = false;
                OrderCancel_button.IsEnabled = false;
                CheckServerError = 1;
            }
            catch
            {
                CheckServerError = 0;
            }
            if (CheckServerError == 1)
            {
                ResumeSessionApiJSONResponce = JsonConvert.DeserializeObject(ResumeSessionApiStringResponce);

                if (ResumeSessionApiJSONResponce.responseCode == "200")
                {
                    double _sessionId = ResumeSessionApiJSONResponce.sessionId;
                    AD_SessionID = _sessionId;

                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("UPDATE ad_user_pos " +
                    "SET sessionid = " + _sessionId +
                    "WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and ad_user_id = " + AD_USER_ID + " ; ", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                    connection.Close();

                }
                else if (ResumeSessionApiJSONResponce.responseCode == "401")
                {
                    CreateNewSession();
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
        }

        #endregion Session

        #region Getting Invoice Error

        private async void NetworkRetry_for_getting_invoice_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    NetworkRetry_for_getting_invoice_bt.Content = "";
                    NetworkRetry_for_getting_invoice_ProgressBar.Visibility = Visibility.Visible;
                });
                //Thread.Sleep(5000);
                await Task.Run(() =>
                {
                    if (_NetworkUpStatus != true)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            NetworkRetry_for_getting_invoice_bt.IsEnabled = true;
                            NetworkRetry_for_getting_invoice_bt.Content = "Retry";
                            NetworkRetry_for_getting_invoice_ProgressBar.Visibility = Visibility.Hidden;
                        });
                        return;
                    }
                    else
                    {
                        var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                        int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                        int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                        string _responce_code = Check_POS_Number_rs.Item3;
                        bool _network_status_ = Check_POS_Number_rs.Item4;
                        if (_responce_code == "0" || _responce_code == "200")
                        {
                            invoice_number = Check_POS_Number_rs.Item1;
                            document_no = Check_POS_Number_rs.Item2;
                            //InvoiceNo.Text = invoice_number.ToString();
                            this.Dispatcher.Invoke(() =>
                            {
                                InvoiceNo.Text = invoice_number.ToString();

                                NetworkRetry_for_getting_invoice_ProgressBar.Visibility = Visibility.Hidden;
                                //Thread.Sleep(100);
                                NetworkRetry_for_getting_invoice_bt.Content = "Success";
                                //Thread.Sleep(50);
                                Error_page.Visibility = Visibility.Hidden;
                                NetworkError_for_getting_invoice.Visibility = Visibility.Hidden;
                                NetworkRetry_for_getting_invoice_bt.IsEnabled = true;
                                NetworkRetry_for_getting_invoice_bt.Content = "Retry";
                            });

                            return;
                        }
                        else if (_responce_code == "500")
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                NetworkRetry_for_getting_invoice_text_l2.Text = "Server Not Responding";
                                NetworkRetry_for_getting_invoice_bt.IsEnabled = true;
                                NetworkRetry_for_getting_invoice_bt.Content = "Retry";
                                NetworkRetry_for_getting_invoice_ProgressBar.Visibility = Visibility.Hidden;
                            });
                            return;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        #endregion Getting Invoice Error

        #endregion Errors | PopUp

        private void SessionClose_Click(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Session_Close_Pannel";
            Session_Close_Pannel.Visibility = Visibility.Visible;
            Cart_Main_Pannel.Visibility = Visibility.Hidden;
            Error_page.Visibility = Visibility.Hidden;
            Session_Check.Visibility = Visibility.Hidden;
            Calculate_OpeningBalance();
            Keyboard.Focus(SessionClose_Pruchase_input);
        }


        #region Session Close Screens

        private string rb_Name;

        private void R_sessionClose_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb_Name = rb.Name;
        }

        private void R_sessionClose_Click(object sender, RoutedEventArgs e)
        {
            ScessionClose_ScreenChange(rb_Name);
        }

        private void ScessionClose_ScreenChange(string Name)
        {
            switch (Name)
            {
                case "R_sessionClose_Denomination":
                    SessionClose_Denomination.Visibility = Visibility.Visible;
                    SessionClose_OnlyTotal.Visibility = Visibility.Hidden;
                    SessionClose_NoSale.Visibility = Visibility.Hidden;
                    SessionClose_empty_inputs();
                    Keyboard.Focus(SessionClose_Pruchase_input);
                    break;

                case "R_sessionClose_OnlyTotal":
                    SessionClose_Denomination.Visibility = Visibility.Hidden;
                    SessionClose_OnlyTotal.Visibility = Visibility.Visible;
                    SessionClose_NoSale.Visibility = Visibility.Hidden;
                    SessionClose_empty_inputs();
                    Keyboard.Focus(SessionClose_Pruchase_input);
                    break;

                case "R_sessionClose_No_Sale":
                    SessionClose_Denomination.Visibility = Visibility.Hidden;
                    SessionClose_OnlyTotal.Visibility = Visibility.Hidden;
                    SessionClose_NoSale.Visibility = Visibility.Visible;
                    SessionClose_empty_inputs();
                    Keyboard.Focus(SessionClose_Pruchase_input);
                    break;

                default:
                    break;
            }
        }
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public class SortAdorner : Adorner
        {
            private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

            private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
            {
                this.Direction = dir;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                TranslateTransform transform = new TranslateTransform
                    (
                        AdornedElement.RenderSize.Width - 15,
                        (AdornedElement.RenderSize.Height - 5) / 2
                    );
                drawingContext.PushTransform(transform);

                Geometry geometry = ascGeometry;
                if (this.Direction == ListSortDirection.Descending)
                    geometry = descGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);

                drawingContext.Pop();
            }
        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortAdorner != null)
            {
                AdornerLayer.GetAdornerLayer(lstProdSearch).Remove(listViewSortAdorner);
                lstProdSearch.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lstProdSearch.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void Calculate_OpeningBalance()
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            long Session_StartTimeMilliseconds = new DateTimeOffset(Convert.ToDateTime(AD_Session_Started_at)).ToUnixTimeMilliseconds();
            NpgsqlCommand cmd_c_invoic_SalesSummery_op = new NpgsqlCommand("select   COALESCE(sum(openingbalance),0)   from c_invoice where    session_id  = " + Session_StartTimeMilliseconds.ToString() + ";", connection);

            NpgsqlDataReader _get_c_invoic_SalesSummery_op = cmd_c_invoic_SalesSummery_op.ExecuteReader();
            if (_get_c_invoic_SalesSummery_op.HasRows == true)
            {
                _get_c_invoic_SalesSummery_op.Read();
                OpeningBalance_total.Text = Convert.ToDouble(_get_c_invoic_SalesSummery_op.GetDouble(0)).ToString("0.00");
            }
            else
            {
                OpeningBalance_total.Text = "0.00";
            }
            connection.Close();
        }
        private void SessionClose_empty_inputs()
        {

            SessionClose_Pruchase_input.Text = String.Empty;
            SessionClose_purchase_total.Text = "0.00";
            SessionClose_Expenses_input.Text = String.Empty;
            SessionClose_exp_total.Text = "0.00";
            SessionClose_card_input.Text = String.Empty;
            SessionClose_card_total.Text = "0.00";
            SessionClose_500x_input.Text = String.Empty;
            SessionClose_500x_total.Text = "0.00";
            SessionClose_100x_input.Text = String.Empty;
            SessionClose_100x_total.Text = "0.00";
            SessionClose_50x_input.Text = String.Empty;
            SessionClose_50x_total.Text = "0.00";
            SessionClose_10x_input.Text = String.Empty;
            SessionClose_10x_total.Text = "0.00";
            SessionClose_5x_input.Text = String.Empty;
            SessionClose_5x_total.Text = "0.00";
            SessionClose_1x_input.Text = String.Empty;
            SessionClose_1x_total.Text = "0.00";
            SessionClose_50dx_input.Text = String.Empty;
            SessionClose_50dx_total.Text = "0.00";
            SessionClose_25dx_input.Text = String.Empty;
            SessionClose_25dx_total.Text = "0.00";
            SessionClose_only_total_input.Text = String.Empty;
            SessionClose_grand_total.Text = "0.00";
        }

        private string SessionClose_input_GotFocus_field;

        private void SessionClose_input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox s_text_input = sender as TextBox;
            SessionClose_input_GotFocus_field = s_text_input.Name;

            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_Pruchase_input":
                    SessionClose_Pruchase_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;
                case "SessionClose_Expenses_input":
                    SessionClose_Expenses_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;
                case "SessionClose_card_input":
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    break;

                case "SessionClose_500x_input":
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    break;

                case "SessionClose_100x_input":
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    break;

                case "SessionClose_50x_input":
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    break;

                case "SessionClose_10x_input":
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    break;

                case "SessionClose_5x_input":
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    break;

                case "SessionClose_1x_input":
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    break;

                case "SessionClose_50dx_input":
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    break;

                case "SessionClose_25dx_input":
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    break;

                case "SessionClose_only_total_input":
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    break;

                default:
                    break;
            }
        }



        private void SessionClose_KeyPad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)e.OriginalSource;

                string s = btn.Content.ToString();
                switch (SessionClose_input_GotFocus_field)
                {
                    case "SessionClose_Pruchase_input":
                        if (SessionClose_Pruchase_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_Pruchase_input.Text += s;

                        SessionClose_purchase_total.Text = Convert.ToDouble(SessionClose_Pruchase_input.Text).ToString("0.00");
                        SessionClose_Pruchase_input.Select(SessionClose_Pruchase_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;
                    case "SessionClose_Expenses_input":
                        if (SessionClose_Expenses_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_Expenses_input.Text += s;

                        SessionClose_exp_total.Text = Convert.ToDouble(SessionClose_Expenses_input.Text).ToString("0.00");
                        SessionClose_Expenses_input.Select(SessionClose_Expenses_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;
                    case "SessionClose_card_input":

                        if ((((Button)e.OriginalSource).Content.ToString() == "." && SessionClose_card_input.Text.ToString().Contains(".")) || (((Button)e.OriginalSource).Content.ToString() == "." && SessionClose_card_input.Text == String.Empty))
                        {
                            return;
                        }


                        SessionClose_card_input.Text += s;

                        SessionClose_card_total.Text = Convert.ToDouble(SessionClose_card_input.Text).ToString("0.00");
                        SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_500x_input":
                        if (SessionClose_500x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_500x_input.Text += s;

                        SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 500).ToString("0.00");
                        SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_100x_input":
                        if (SessionClose_100x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_100x_input.Text += s;

                        SessionClose_100x_total.Text = (Convert.ToDouble(SessionClose_100x_input.Text) * 100).ToString("0.00");
                        SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_50x_input":
                        if (SessionClose_50x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_50x_input.Text += s;

                        SessionClose_50x_total.Text = (Convert.ToDouble(SessionClose_50x_input.Text) * 50).ToString("0.00");
                        SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_10x_input":
                        if (SessionClose_10x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_10x_input.Text += s;

                        SessionClose_10x_total.Text = (Convert.ToDouble(SessionClose_10x_input.Text) * 10).ToString("0.00");
                        SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_5x_input":
                        if (SessionClose_5x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_5x_input.Text += s;

                        SessionClose_5x_total.Text = (Convert.ToDouble(SessionClose_5x_input.Text) * 5).ToString("0.00");
                        SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_1x_input":
                        if (SessionClose_1x_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_1x_input.Text += s;

                        SessionClose_1x_total.Text = (Convert.ToDouble(SessionClose_1x_input.Text) * 1).ToString("0.00");
                        SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_50dx_input":
                        if (SessionClose_50dx_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_50dx_input.Text += s;

                        SessionClose_50dx_total.Text = (Convert.ToDouble(SessionClose_50dx_input.Text) * 50).ToString("0.00");
                        SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_25dx_input":
                        if (SessionClose_25dx_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_25dx_input.Text += s;

                        SessionClose_25dx_total.Text = (Convert.ToDouble(SessionClose_25dx_input.Text) * 25).ToString("0.00");
                        SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_only_total_input":
                        if (((Button)e.OriginalSource).Content.ToString() == "." && SessionClose_only_total_input.Text.ToString().Contains("."))
                            return;
                        SessionClose_only_total_input.Text += s;
                        SessionClose_only_total_input.Text = SessionClose_only_total_input.Text;
                        SessionClose_grand_total.Text = SessionClose_only_total_input.Text;
                        SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionClose_KeyPadErase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionClose_input_GotFocus_field == "SessionClose_Pruchase_input" && SessionClose_Pruchase_input.Text != String.Empty)
                {
                    SessionClose_Pruchase_input.Text = SessionClose_Pruchase_input.Text.Remove(SessionClose_Pruchase_input.Text.Length - 1);
                    if (SessionClose_Pruchase_input.Text == String.Empty)
                    {
                        SessionClose_purchase_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_purchase_total.Text = Convert.ToDouble(SessionClose_Pruchase_input.Text).ToString("0.00");
                    SessionClose_Pruchase_input.Select(SessionClose_Pruchase_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }

                if (SessionClose_input_GotFocus_field == "SessionClose_Expenses_input" && SessionClose_Expenses_input.Text != String.Empty)
                {
                    SessionClose_Expenses_input.Text = SessionClose_Expenses_input.Text.Remove(SessionClose_Expenses_input.Text.Length - 1);
                    if (SessionClose_Expenses_input.Text == String.Empty)
                    {
                        SessionClose_exp_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_exp_total.Text = Convert.ToDouble(SessionClose_Expenses_input.Text).ToString("0.00");
                    SessionClose_Expenses_input.Select(SessionClose_Expenses_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }

                if (SessionClose_input_GotFocus_field == "SessionClose_card_input" && SessionClose_card_input.Text != String.Empty)
                {
                    SessionClose_card_input.Text = SessionClose_card_input.Text.Remove(SessionClose_card_input.Text.Length - 1);
                    if (SessionClose_card_input.Text == String.Empty)
                    {
                        SessionClose_card_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_card_total.Text = Convert.ToDouble(SessionClose_card_input.Text).ToString("0.00");
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_500x_input" && SessionClose_500x_input.Text != String.Empty)
                {
                    SessionClose_500x_input.Text = SessionClose_500x_input.Text.Remove(SessionClose_500x_input.Text.Length - 1);
                    if (SessionClose_500x_input.Text == String.Empty)
                    {
                        SessionClose_500x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 500).ToString("0.00");

                    //SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text) * 25).ToString("0.00");
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_100x_input" && SessionClose_100x_input.Text != String.Empty)
                {
                    SessionClose_100x_input.Text = SessionClose_100x_input.Text.Remove(SessionClose_100x_input.Text.Length - 1);
                    if (SessionClose_100x_input.Text == String.Empty)
                    {
                        SessionClose_100x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_100x_total.Text = (Convert.ToDouble(SessionClose_100x_input.Text) * 100).ToString("0.00");
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_50x_input" && SessionClose_50x_input.Text != String.Empty)
                {
                    SessionClose_50x_input.Text = SessionClose_50x_input.Text.Remove(SessionClose_50x_input.Text.Length - 1);
                    if (SessionClose_50x_input.Text == String.Empty)
                    {
                        SessionClose_50x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_50x_total.Text = (Convert.ToDouble(SessionClose_50x_input.Text) * 50).ToString("0.00");
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_10x_input" && SessionClose_10x_input.Text != String.Empty)
                {
                    SessionClose_10x_input.Text = SessionClose_10x_input.Text.Remove(SessionClose_10x_input.Text.Length - 1);
                    if (SessionClose_10x_input.Text == String.Empty)
                    {
                        SessionClose_10x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_10x_total.Text = (Convert.ToDouble(SessionClose_10x_input.Text) * 10).ToString("0.00");
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_5x_input" && SessionClose_5x_input.Text != String.Empty)
                {
                    SessionClose_5x_input.Text = SessionClose_5x_input.Text.Remove(SessionClose_5x_input.Text.Length - 1);
                    if (SessionClose_5x_input.Text == String.Empty)
                    {
                        SessionClose_5x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_5x_total.Text = (Convert.ToDouble(SessionClose_5x_input.Text) * 5).ToString("0.00");
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_1x_input" && SessionClose_1x_input.Text != String.Empty)
                {
                    SessionClose_1x_input.Text = SessionClose_1x_input.Text.Remove(SessionClose_1x_input.Text.Length - 1);
                    if (SessionClose_1x_input.Text == String.Empty)
                    {
                        SessionClose_1x_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_1x_total.Text = (Convert.ToDouble(SessionClose_1x_input.Text) * 1).ToString("0.00");
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_50dx_input" && SessionClose_50dx_input.Text != String.Empty)
                {
                    SessionClose_50dx_input.Text = SessionClose_50dx_input.Text.Remove(SessionClose_50dx_input.Text.Length - 1);
                    if (SessionClose_50dx_input.Text == String.Empty)
                    {
                        SessionClose_50dx_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_50dx_total.Text = (Convert.ToDouble(SessionClose_50dx_input.Text) * 50).ToString("0.00");
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_25dx_input" && SessionClose_25dx_input.Text != String.Empty)
                {
                    SessionClose_25dx_input.Text = SessionClose_25dx_input.Text.Remove(SessionClose_25dx_input.Text.Length - 1);
                    if (SessionClose_25dx_input.Text == String.Empty)
                    {
                        SessionClose_25dx_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_25dx_total.Text = (Convert.ToDouble(SessionClose_25dx_input.Text) * 25).ToString("0.00");
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                }
                if (SessionClose_input_GotFocus_field == "SessionClose_only_total_input" && SessionClose_only_total_input.Text != String.Empty)
                {
                    SessionClose_only_total_input.Text = SessionClose_only_total_input.Text.Remove(SessionClose_only_total_input.Text.Length - 1);
                    if (SessionClose_only_total_input.Text == String.Empty)
                    {
                        SessionClose_only_total_total.Text = "0.00";
                        SessionClose_grand_total_cal();
                        return;
                    }
                    SessionClose_only_total_total.Text = Convert.ToDouble(SessionClose_only_total_input.Text).ToString("0.00");
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    SessionClose_grand_total.Text = SessionClose_only_total_input.Text;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionClose_KeyPad_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (SessionClose_input_GotFocus_field)
                {
                    case "SessionClose_Pruchase_input":
                        SessionClose_Pruchase_input.Text = String.Empty;
                        SessionClose_purchase_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_Pruchase_input);
                        SessionClose_grand_total_cal();
                        break;
                    case "SessionClose_Expenses_input":
                        SessionClose_Expenses_input.Text = String.Empty;
                        SessionClose_exp_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_Expenses_input);
                        SessionClose_grand_total_cal();
                        break;
                    case "SessionClose_card_input":
                        SessionClose_card_input.Text = String.Empty;
                        SessionClose_card_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_card_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_500x_input":
                        SessionClose_500x_input.Text = String.Empty;
                        SessionClose_500x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_500x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_100x_input":
                        SessionClose_100x_input.Text = String.Empty;
                        SessionClose_100x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_100x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_50x_input":
                        SessionClose_50x_input.Text = String.Empty;
                        SessionClose_50x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_50x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_10x_input":
                        SessionClose_10x_input.Text = String.Empty;
                        SessionClose_10x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_10x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_5x_input":
                        SessionClose_5x_input.Text = String.Empty;
                        SessionClose_5x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_5x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_1x_input":
                        SessionClose_1x_input.Text = String.Empty;
                        SessionClose_1x_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_1x_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_50dx_input":
                        SessionClose_50dx_input.Text = String.Empty;
                        SessionClose_50dx_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_50dx_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_25dx_input":
                        SessionClose_25dx_input.Text = String.Empty;
                        SessionClose_25dx_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_25dx_input);
                        SessionClose_grand_total_cal();
                        break;

                    case "SessionClose_only_total_input":
                        SessionClose_only_total_input.Text = String.Empty;
                        SessionClose_only_total_total.Text = "0.00";
                        Keyboard.Focus(SessionClose_only_total_input);
                        SessionClose_grand_total.Text = "0.00";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionClose_KeyPadEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (SessionClose_input_GotFocus_field)
                {
                    case "SessionClose_Pruchase_input":
                        Keyboard.Focus(SessionClose_Expenses_input);
                        break;

                    case "SessionClose_Expenses_input":
                        Keyboard.Focus(SessionClose_card_input);
                        break;

                    case "SessionClose_card_input":
                        Keyboard.Focus(SessionClose_500x_input);
                        break;

                    case "SessionClose_500x_input":
                        Keyboard.Focus(SessionClose_100x_input);
                        break;

                    case "SessionClose_100x_input":
                        Keyboard.Focus(SessionClose_50x_input);
                        break;

                    case "SessionClose_50x_input":
                        Keyboard.Focus(SessionClose_10x_input);
                        break;

                    case "SessionClose_10x_input":
                        Keyboard.Focus(SessionClose_5x_input);
                        break;

                    case "SessionClose_5x_input":
                        Keyboard.Focus(SessionClose_1x_input);
                        break;

                    case "SessionClose_1x_input":
                        Keyboard.Focus(SessionClose_50dx_input);
                        break;

                    case "SessionClose_50dx_input":
                        Keyboard.Focus(SessionClose_25dx_input);
                        break;

                    case "SessionClose_25dx_input":
                        //Keyboard.Focus(SessionClose_25dx_input);
                        break;

                    case "SessionClose_only_total_input":
                        //Keyboard.Focus(SessionClose_only_total_input);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionClose_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                switch (SessionClose_input_GotFocus_field)
                {
                    case "SessionClose_Pruchase_input":
                        Keyboard.Focus(SessionClose_Pruchase_input);
                        SessionClose_Pruchase_input.Select(SessionClose_Pruchase_input.Text.Length, 0);
                        break;

                    case "SessionClose_Expenses_input":
                        Keyboard.Focus(SessionClose_Expenses_input);
                        SessionClose_Expenses_input.Select(SessionClose_Expenses_input.Text.Length, 0);
                        break;

                    case "SessionClose_card_input":
                        Keyboard.Focus(SessionClose_card_input);
                        SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                        break;

                    case "SessionClose_500x_input":
                        Keyboard.Focus(SessionClose_500x_input);
                        SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                        break;

                    case "SessionClose_100x_input":
                        Keyboard.Focus(SessionClose_100x_input);
                        SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);

                        break;

                    case "SessionClose_50x_input":
                        Keyboard.Focus(SessionClose_50x_input);
                        SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);

                        break;

                    case "SessionClose_10x_input":
                        Keyboard.Focus(SessionClose_10x_input);
                        SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);

                        break;

                    case "SessionClose_5x_input":
                        Keyboard.Focus(SessionClose_5x_input);
                        SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);

                        break;

                    case "SessionClose_1x_input":
                        Keyboard.Focus(SessionClose_1x_input);
                        SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);

                        break;

                    case "SessionClose_50dx_input":
                        Keyboard.Focus(SessionClose_50dx_input);
                        SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);

                        break;

                    case "SessionClose_25dx_input":
                        Keyboard.Focus(SessionClose_25dx_input);
                        SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);

                        break;

                    case "SessionClose_only_total_input":
                        Keyboard.Focus(SessionClose_only_total_input);
                        SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionClose_input_TextChanged(object sender, TextChangedEventArgs e)
        {

            //switch (SessionClose_input_GotFocus_field)
            //{
            //    case "SessionClose_card_input":
            //        SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_500x_input":
            //        SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_100x_input":
            //        SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_50x_input":
            //        SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_10x_input":
            //        SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_5x_input":
            //        SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_1x_input":
            //        SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_50dx_input":
            //        SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_25dx_input":
            //        SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
            //        break;

            //    case "SessionClose_only_total_input":
            //        SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
            //        break;

            //    default:
            //        break;
            //}

            switch (SessionClose_input_GotFocus_field)
            {
                case "SessionClose_Pruchase_input":

                    if (SessionClose_Pruchase_input.Text.ToString().Contains("."))
                        return;
                    SessionClose_purchase_total.Text = Convert.ToDouble(SessionClose_Pruchase_input.Text != String.Empty ? SessionClose_Pruchase_input.Text : "0").ToString("0.00");
                    SessionClose_Pruchase_input.Select(SessionClose_Pruchase_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_Expenses_input":

                    if (SessionClose_Expenses_input.Text.ToString().Contains("."))
                        return;
                    SessionClose_exp_total.Text = Convert.ToDouble(SessionClose_Expenses_input.Text != String.Empty ? SessionClose_Expenses_input.Text : "0").ToString("0.00");
                    SessionClose_Expenses_input.Select(SessionClose_Expenses_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_card_input":

                    if (SessionClose_card_input.Text.ToString().Contains("."))
                        return;
                    SessionClose_card_total.Text = Convert.ToDouble(SessionClose_card_input.Text != String.Empty ? SessionClose_card_input.Text : "0").ToString("0.00");
                    SessionClose_card_input.Select(SessionClose_card_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_500x_input":


                    SessionClose_500x_total.Text = (Convert.ToDouble(SessionClose_500x_input.Text != String.Empty ? SessionClose_500x_input.Text : "0") * 500).ToString("0.00");
                    SessionClose_500x_input.Select(SessionClose_500x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_100x_input":


                    SessionClose_100x_total.Text = (Convert.ToDouble(SessionClose_100x_input.Text != String.Empty ? SessionClose_100x_input.Text : "0") * 100).ToString("0.00");
                    SessionClose_100x_input.Select(SessionClose_100x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50x_input":


                    SessionClose_50x_total.Text = (Convert.ToDouble(SessionClose_50x_input.Text != String.Empty ? SessionClose_50x_input.Text : "0") * 50).ToString("0.00");
                    SessionClose_50x_input.Select(SessionClose_50x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_10x_input":


                    SessionClose_10x_total.Text = (Convert.ToDouble(SessionClose_10x_input.Text != String.Empty ? SessionClose_10x_input.Text : "0") * 10).ToString("0.00");
                    SessionClose_10x_input.Select(SessionClose_10x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_5x_input":


                    SessionClose_5x_total.Text = (Convert.ToDouble(SessionClose_5x_input.Text != String.Empty ? SessionClose_5x_input.Text : "0") * 5).ToString("0.00");
                    SessionClose_5x_input.Select(SessionClose_5x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_1x_input":


                    SessionClose_1x_total.Text = (Convert.ToDouble(SessionClose_1x_input.Text != String.Empty ? SessionClose_1x_input.Text : "0") * 1).ToString("0.00");
                    SessionClose_1x_input.Select(SessionClose_1x_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_50dx_input":


                    SessionClose_50dx_total.Text = (Convert.ToDouble(SessionClose_50dx_input.Text != String.Empty ? SessionClose_50dx_input.Text : "0") * 0.50).ToString("0.00");
                    SessionClose_50dx_input.Select(SessionClose_50dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_25dx_input":


                    SessionClose_25dx_total.Text = (Convert.ToDouble(SessionClose_25dx_input.Text != String.Empty ? SessionClose_25dx_input.Text : "0") * 0.25).ToString("0.00");
                    SessionClose_25dx_input.Select(SessionClose_25dx_input.Text.Length, 0);
                    SessionClose_grand_total_cal();
                    break;

                case "SessionClose_only_total_input":

                    SessionClose_only_total_input.Text = SessionClose_only_total_input.Text;
                    SessionClose_grand_total.Text = SessionClose_only_total_input.Text;
                    SessionClose_only_total_input.Select(SessionClose_only_total_input.Text.Length, 0);
                    break;

                default:
                    break;
            }
        }

        private void SessionClose_grand_total_cal()
        {
            double Amount_In_Hand =
                Convert.ToDouble(SessionClose_card_total.Text == String.Empty ? "0" : SessionClose_card_total.Text) +
                Convert.ToDouble(SessionClose_500x_total.Text == String.Empty ? "0" : SessionClose_500x_total.Text) +
                Convert.ToDouble(SessionClose_100x_total.Text == String.Empty ? "0" : SessionClose_100x_total.Text) +
                Convert.ToDouble(SessionClose_50x_total.Text == String.Empty ? "0" : SessionClose_50x_total.Text) +
                Convert.ToDouble(SessionClose_10x_total.Text == String.Empty ? "0" : SessionClose_10x_total.Text) +
                Convert.ToDouble(SessionClose_5x_total.Text == String.Empty ? "0" : SessionClose_5x_total.Text) +
                Convert.ToDouble(SessionClose_1x_total.Text == String.Empty ? "0" : SessionClose_1x_total.Text) +
                Convert.ToDouble(SessionClose_50dx_total.Text == String.Empty ? "0" : SessionClose_50dx_total.Text) +
                Convert.ToDouble(SessionClose_25dx_total.Text == String.Empty ? "0" : SessionClose_25dx_total.Text);
            //+
            //Convert.ToDouble(OpeningBalance_total.Text == String.Empty ? "0" : OpeningBalance_total.Text);

            //double Amount_Total_exp = Convert.ToDouble(SessionClose_Pruchase_input.Text == String.Empty ? "0" : SessionClose_Pruchase_input.Text) +
            //  Convert.ToDouble(SessionClose_Expenses_input.Text == String.Empty ? "0" : SessionClose_Expenses_input.Text);
            SessionClose_grand_total.Text = (Amount_In_Hand).ToString("0.00");


        }

        private void SessionClose_input_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                switch (SessionClose_input_GotFocus_field)
                {
                    case "SessionClose_Pruchase_input":
                        Keyboard.Focus(SessionClose_Expenses_input);
                        break;
                    case "SessionClose_Expenses_input":
                        Keyboard.Focus(SessionClose_card_input);
                        break;

                    case "SessionClose_card_input":
                        Keyboard.Focus(SessionClose_500x_input);
                        break;

                    case "SessionClose_500x_input":
                        Keyboard.Focus(SessionClose_100x_input);
                        break;

                    case "SessionClose_100x_input":
                        Keyboard.Focus(SessionClose_50x_input);
                        break;

                    case "SessionClose_50x_input":
                        Keyboard.Focus(SessionClose_10x_input);
                        break;

                    case "SessionClose_10x_input":
                        Keyboard.Focus(SessionClose_5x_input);
                        break;

                    case "SessionClose_5x_input":
                        Keyboard.Focus(SessionClose_1x_input);
                        break;

                    case "SessionClose_1x_input":
                        Keyboard.Focus(SessionClose_50dx_input);
                        break;

                    case "SessionClose_50dx_input":
                        Keyboard.Focus(SessionClose_25dx_input);
                        break;

                    case "SessionClose_25dx_input":
                        Keyboard.Focus(CompleteSessionClose);
                        break;

                    case "SessionClose_only_total_input":
                        Keyboard.Focus(CompleteSessionClose);
                        break;

                    default:
                        break;
                }
            }
        }

        private void BackToCart_from_session_close_page_Click(object sender, RoutedEventArgs e)
        {
            Check_keyboard_Focus = "Session_Close_Pannel";
            Session_Close_Pannel.Visibility = Visibility.Hidden;
            Cart_Main_Pannel.Visibility = Visibility.Visible;
            //  Error_page.Visibility = Visibility.Visible;
            //  Session_Check.Visibility = Visibility.Visible;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;

            CustLeft.Visibility = Visibility.Hidden;
            WrongOrder.Visibility = Visibility.Hidden;
            CancelYes.Visibility = Visibility.Hidden;
            CancelNo.Visibility = Visibility.Hidden;
        }
        private void Session_Close()
        {
            try
            {
                //  load_Process();
                DateTime CurrentTime = DateTime.Now;
                long Session_StartTimeMilliseconds = new DateTimeOffset(Convert.ToDateTime(AD_Session_Started_at)).ToUnixTimeMilliseconds();
                long Session_EndTimeMilliseconds = new DateTimeOffset(CurrentTime).ToUnixTimeMilliseconds();
                DateTime SessionStart_time = Convert.ToDateTime(AD_Session_Started_at);
                DateTime SessionEnd_time = CurrentTime;
                //return;

                string _SessionClose_grand_total = Convert.ToDouble(SessionClose_grand_total.Text).ToString();
                //Convert.ToDouble(SessionClose_500x_input.Text != String.Empty ? SessionClose_500x_input.Text : "0")
                //#region JSON Array Formate


                //JObject SessionCloseJSON =
                //new JObject(
                //    new JProperty("SyncedTime", 0),
                //    new JProperty("remindMe", "Y"),
                //     new JProperty("showImage", "N"),
                //     new JProperty("warehouseId", AD_Warehouse_Id),
                //     new JProperty("macAddress", DeviceMacAdd),
                //     new JProperty("endTime", Session_EndTimeMilliseconds),
                //     new JProperty("businessPartnerId", AD_bpartner_Id),
                //     new JProperty("password", AD_UserPassword),
                //     new JProperty("clientId", AD_Client_ID),
                //     new JProperty("version", 1.0),
                //     new JProperty("startTime", Session_StartTimeMilliseconds),
                //     new JProperty("appName", "POS"),
                //     new JProperty("orgId", AD_ORG_ID),
                //     new JProperty("operation", "GetSalesSummary"),
                //    new JProperty("Denominations",
                //        new JArray(
                //            new JObject(
                //                    new JProperty("total", (Convert.ToDouble(SessionClose_500x_input.Text != String.Empty ? SessionClose_500x_input.Text : "0") * 500).ToString("0.00")),
                //                    new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_500x_input.Text != String.Empty ? SessionClose_500x_input.Text : "0"))),
                //                    new JProperty("type", "riyal"),
                //                    new JProperty("name", 500)
                //                ),
                //                new JObject(
                //                    new JProperty("total", (Convert.ToDouble(SessionClose_100x_input.Text != String.Empty ? SessionClose_100x_input.Text : "0") * 100).ToString("0.00")),
                //                    new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_100x_input.Text != String.Empty ? SessionClose_100x_input.Text : "0"))),
                //                    new JProperty("type", "riyal"),
                //                    new JProperty("name", 100)
                //                ),
                //                new JObject(
                //                    //new JProperty("total", Convert.ToString(Convert.ToDouble(SessionClose_50x_input.Text != String.Empty ? SessionClose_50x_input.Text : "0") * 50)),
                //                    new JProperty("total", (Convert.ToDouble(SessionClose_50x_input.Text != String.Empty ? SessionClose_50x_input.Text : "0") * 50).ToString("0.00")),
                //                    new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_50x_input.Text != String.Empty ? SessionClose_50x_input.Text : "0"))),
                //                    new JProperty("type", "riyal"),
                //                    new JProperty("name", 50)
                //                ),
                //                new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_10x_input.Text != String.Empty ? SessionClose_10x_input.Text : "0") * 10).ToString("0.00")),
                //                new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_10x_input.Text != String.Empty ? SessionClose_10x_input.Text : "0"))),
                //                new JProperty("type", "riyal"),
                //                new JProperty("name", 10)
                //                 ),
                //                new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_5x_input.Text != String.Empty ? SessionClose_5x_input.Text : "0") * 5).ToString("0.00")),
                //                new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_5x_input.Text != String.Empty ? SessionClose_5x_input.Text : "0"))),
                //                new JProperty("type", "riyal"),
                //                new JProperty("name", 5)
                //                ),
                //                new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_1x_input.Text != String.Empty ? SessionClose_1x_input.Text : "0")).ToString("0.00")),
                //                new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_1x_input.Text != String.Empty ? SessionClose_1x_input.Text : "0"))),
                //                new JProperty("type", "riyal"),
                //                new JProperty("name", 1)
                //                ),
                //                new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_50dx_input.Text != String.Empty ? SessionClose_50dx_input.Text : "0") * 0.50).ToString("0.00")),
                //                new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_50dx_input.Text != String.Empty ? SessionClose_50dx_input.Text : "0"))),
                //                new JProperty("type", "dirhams"),
                //                new JProperty("name", 50)
                //                  ),
                //            new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_50dx_input.Text != String.Empty ? SessionClose_50dx_input.Text : "0") * 0.25).ToString("0.00")),
                //                new JProperty("count", Convert.ToString(Convert.ToDouble(SessionClose_50dx_input.Text != String.Empty ? SessionClose_50dx_input.Text : "0"))),
                //                new JProperty("type", "dirhams"),
                //                new JProperty("name", 25)
                //            ),
                //            new JObject(
                //                new JProperty("total", (Convert.ToDouble(SessionClose_card_input.Text != String.Empty ? SessionClose_card_input.Text : "0")).ToString("0.00")),
                //                new JProperty("count", 1),
                //                new JProperty("type", "cash"),
                //                new JProperty("name", 2)
                //            ),
                //            new JObject(
                //                new JProperty("total", "0.00"),
                //                new JProperty("count", 1),
                //                new JProperty("type", "complement"),
                //                new JProperty("name", 1)
                //            ),
                //            new JObject(
                //                new JProperty("total", Convert.ToDouble(SessionClose_grand_total.Text).ToString("0.00")),
                //                new JProperty("count", 0),
                //                new JProperty("type", "total"),
                //                new JProperty("name", 0)
                //            )
                //            )
                //          ),
                //      new JProperty("username", AD_UserName),
                //      new JProperty("sessionId", AD_SessionID),
                //     new JProperty("cashbookId", AD_CashbookID),
                //     new JProperty("userId", AD_USER_ID),
                //     new JProperty("roleId", AD_ROLE_ID)
                //   );
                //#endregion JSON Array Formate



                string saleType = "";
                if (SessionClose_Denomination.Visibility == Visibility.Visible)
                {
                    saleType = "Denomination";
                }
                if (SessionClose_OnlyTotal.Visibility == Visibility.Visible)
                {
                    saleType = "OnlyTotal";
                }
                if (SessionClose_NoSale.Visibility == Visibility.Visible)
                {
                    saleType = "NoSale";
                }
                NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                connection.Open();
                NpgsqlCommand cmd_User_Logout = new NpgsqlCommand("UPDATE ad_user_pos SET isactive='N',sessionid='0' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and ad_user_id = " + AD_USER_ID + " and c_bpartner_id = " + AD_bpartner_Id + "; ", connection);
                cmd_User_Logout.ExecuteReader();
                connection.Close();
                RetailViewModel.Get_SalesSummery_And_print(Session_StartTimeMilliseconds.ToString(), SessionStart_time.ToString("dd/MM/yyyy hh:mm:ss"), Session_EndTimeMilliseconds.ToString(), SessionEnd_time.ToString("dd/MM/yyyy  hh:mm:ss"), false, SessionClose_Pruchase_input.Text, SessionClose_Expenses_input.Text, AD_Client_ID.ToString(), AD_ORG_ID.ToString(), AD_Warehouse_Id.ToString(), AD_USER_ID.ToString(), OpeningBalance_total.Text,
                    SessionClose_500x_total.Text, SessionClose_100x_total.Text, SessionClose_50x_total.Text, SessionClose_10x_total.Text, SessionClose_5x_total.Text, SessionClose_1x_total.Text, SessionClose_50dx_total.Text, SessionClose_25dx_total.Text, SessionClose_grand_total.Text, saleType, SessionClose_only_total_input.Text, SessionClose_500x_input.Text, SessionClose_100x_input.Text, SessionClose_50x_input.Text, SessionClose_10x_input.Text, SessionClose_5x_input.Text, SessionClose_1x_input.Text, SessionClose_50dx_input.Text, SessionClose_25dx_input.Text);
                ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Login;
                //  unLoad_Process();
            }
            catch (Exception ex)
            {
                // unLoad_Process();
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void CompleteSessionClose_Click(object sender, RoutedEventArgs e)
        {
            Session_Close();
        }
        #endregion Session Close Screens

        private void Exchange_items_Click(object sender, RoutedEventArgs e)
        {
            Bind_Product_Search();
            txtProdSearch.Text = string.Empty;
            product_Search_Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(placementForPopup);
            product_Search_Popup.IsOpen = true;

            MainPage.IsEnabled = false;
            txtProdSearch.Focus();
            Keyboard.Focus(txtProdSearch);
        }

        private void Barcode_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void InvoiceReSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<OrderHeaders> OrderHeaders_items = new List<OrderHeaders>();
                List<Invoice_Post> Invoice_Post_items = new List<Invoice_Post>();
                List<OrderDetails> OrderDetails_items = new List<OrderDetails>();
                this.Dispatcher.Invoke(() =>
                {
                    __Side_Menu_Page.IsEnabled = false;
                    ReSyncProgressbar.Visibility = Visibility.Visible;
                    ReSyncProgressbarText.Visibility = Visibility.Visible;
                });
                //
                //ReSyncProgressbarText
                double totalcount = 0;
                double syncedcount = 0;
                if (_NetworkUpStatus == false)
                {
                    Console.WriteLine("No Network");
                    this.Dispatcher.Invoke(() =>
                    {
                        ReSyncProgressbar.Visibility = Visibility.Hidden;
                        ReSyncProgressbarText.Visibility = Visibility.Hidden;
                        __Side_Menu_Page.IsEnabled = true;
                    });
                    NotifierViewModel.Notifier.ShowError("     No Network");
                    return;
                }

                Console.WriteLine("Invoice Re-Sync Function Running");

                #region Posting Background Function

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand CheckTotalUnPostedInvoice = new NpgsqlCommand("SELECT COUNT(1) FROM c_invoice t1 ,c_invoicepaymentdetails t2,c_bpartner t3 " +
                    "WHERE is_canceled = 'Y' AND is_completed = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'Y' AND t1.c_invoice_id = t2.c_invoice_id AND t1.c_bpartner_id = t3.c_bpartner_id ;", connection);
                NpgsqlDataReader _CheckTotalUnPostedInvoice = CheckTotalUnPostedInvoice.ExecuteReader();
                while (_CheckTotalUnPostedInvoice.Read())
                {
                    totalcount = _CheckTotalUnPostedInvoice.GetInt64(0);
                }
                connection.Close();
                //connection.Open();
                //  CheckTotalUnPostedInvoice = new NpgsqlCommand("SELECT COUNT(1) FROM c_invoice t1 ,c_invoicepaymentdetails t2,c_bpartner t3 " +
                //    "WHERE is_canceled = 'Y' AND is_completed = 'N' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'Y' AND t1.c_invoice_id = t2.c_invoice_id AND t1.c_bpartner_id = t3.c_bpartner_id ;", connection);
                //  _CheckTotalUnPostedInvoice = CheckTotalUnPostedInvoice.ExecuteReader();
                //while (_CheckTotalUnPostedInvoice.Read())
                //{
                //    totalcount += _CheckTotalUnPostedInvoice.GetInt64(0);
                //}
                //connection.Close();

                if (totalcount < 1)
                {
                    Console.WriteLine("All Invoice Posted");
                    this.Dispatcher.Invoke(() =>
                    {
                        ReSyncProgressbar.Visibility = Visibility.Hidden;
                        ReSyncProgressbarText.Visibility = Visibility.Hidden;
                        __Side_Menu_Page.IsEnabled = true;
                    });
                    NotifierViewModel.Notifier.ShowInformation("     All Invoice Synced");
                    return;
                }
                this.Dispatcher.Invoke(() =>
                {
                    ReSyncProgressbarText.Text = "Total " + syncedcount + "/" + totalcount + " Resyncing";
                });

                //Getting Last 5 Invoice Which is Complected and Not Posted
                connection.Open();

                NpgsqlCommand cmd_c_invoic_iPaymentD = new NpgsqlCommand("SELECT t1.c_invoice_id, t1.ad_client_id, t1.ad_org_id,t1.ad_user_id," +
                   "t1.ad_role_id,t1.documentno, t1.m_warehouse_id, t1.c_bpartner_id, t1.qid, t1.mobilenumber, t1.discounttype, t1.discountvalue, " +
                   "t1.grandtotal, t1.orderid, t1.reason as invoice_reason,t1.created, t1.grandtotal_round_off, t1.total_items_count, t1.balance, t1.change, t1.lossamount, " +
                   "t1.extraamount,t2.cash, t2.card, t2.exchange, t2.redemption, t2.iscomplementary, t2.iscredit,t2.name_id, t2.mobile_numbler,t2.reason,t3.name,'Completed' as status " +
                   "FROM c_invoice t1 ,c_invoicepaymentdetails t2,c_bpartner t3 " +
                   "WHERE is_completed = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'N' AND t1.c_invoice_id = t2.c_invoice_id AND t1.c_bpartner_id = t3.c_bpartner_id " +
                   "Union SELECT t1.c_invoice_id, t1.ad_client_id, t1.ad_org_id,t1.ad_user_id," +
                   "t1.ad_role_id,t1.documentno, t1.m_warehouse_id, t1.c_bpartner_id, t1.qid, t1.mobilenumber, t1.discounttype, t1.discountvalue, " +
                   "t1.grandtotal, t1.orderid, t1.reason as invoice_reason,t1.created, t1.grandtotal_round_off, t1.total_items_count, t1.balance, t1.change, t1.lossamount, " +
                   "t1.extraamount,0 as cash, 0 as card, 0 as exchange, 0 as redemption, 'N' as iscomplementary, 'N' as iscredit,'NA' as name_id, 'NA' as mobile_numbler,reason,t3.name ,'Canceled' as status " +
                   "FROM c_invoice t1 ,c_bpartner t3 " +
                   "WHERE is_canceled = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'N' AND t1.c_bpartner_id = t3.c_bpartner_id ", connection);


                //NpgsqlCommand cmd_c_invoic_iPaymentD = new NpgsqlCommand("SELECT t1.c_invoice_id, t1.ad_client_id, t1.ad_org_id,t1.ad_user_id," +
                //    "t1.ad_role_id,t1.documentno, t1.m_warehouse_id, t1.c_bpartner_id, t1.qid, t1.mobilenumber, t1.discounttype, t1.discountvalue, " +
                //    "t1.grandtotal, t1.orderid, t1.reason as invoice_reason,t1.created, t1.grandtotal_round_off, t1.total_items_count, t1.balance, t1.change, t1.lossamount, " +
                //    "t1.extraamount,t2.cash, t2.card, t2.exchange, t2.redemption, t2.iscomplementary, t2.iscredit,t2.name_id, t2.mobile_numbler,t2.reason,t3.name " +
                //    "FROM c_invoice t1 ,c_invoicepaymentdetails t2,c_bpartner t3 " +
                //    "WHERE  is_completed = 'Y' AND is_posted = 'N' AND is_onsync = 'N' AND is_posterror = 'Y' AND t1.c_invoice_id = t2.c_invoice_id AND t1.c_bpartner_id = t3.c_bpartner_id ;", connection);

                NpgsqlDataReader _get_c_invoic_iPaymentD = cmd_c_invoic_iPaymentD.ExecuteReader();
                if (_get_c_invoic_iPaymentD.HasRows == false)
                {
                    Console.WriteLine("All Complected Invoice Posted");
                    //log.Info("All Complected Invoice Posted");
                    connection.Close();
                    return;
                }
                while (_get_c_invoic_iPaymentD.Read())
                {
                    Invoice_Post_items.Add(new Invoice_Post()
                    {
                        c_invoice_id = _get_c_invoic_iPaymentD.GetDouble(0).ToString(),
                        ad_client_id = _get_c_invoic_iPaymentD.GetDouble(1).ToString(),
                        ad_org_id = _get_c_invoic_iPaymentD.GetDouble(2).ToString(),
                        ad_user_id = _get_c_invoic_iPaymentD.GetDouble(3).ToString(),
                        ad_role_id = _get_c_invoic_iPaymentD.GetDouble(4).ToString(),
                        documentno = _get_c_invoic_iPaymentD.GetString(5),
                        m_warehouse_id = _get_c_invoic_iPaymentD.GetDouble(6).ToString(),
                        c_bpartner_id = _get_c_invoic_iPaymentD.GetDouble(7).ToString(),
                        qid = _get_c_invoic_iPaymentD.GetString(8),
                        mobilenumber = _get_c_invoic_iPaymentD.GetString(9),
                        discounttype = _get_c_invoic_iPaymentD.GetDouble(10).ToString(),
                        discountvalue = _get_c_invoic_iPaymentD.GetDouble(11).ToString(),
                        grandtotal = _get_c_invoic_iPaymentD.GetDouble(12).ToString(),
                        orderid = _get_c_invoic_iPaymentD.GetDouble(13).ToString(),
                        invoice_reason = _get_c_invoic_iPaymentD.GetString(14),
                        created = _get_c_invoic_iPaymentD.GetDateTime(15).ToString("yyyy-MM-dd HH:mm:ss"),
                        grandtotal_round_off = _get_c_invoic_iPaymentD.GetDouble(16).ToString(),
                        total_items_count = _get_c_invoic_iPaymentD.GetDouble(17).ToString(),
                        balance = _get_c_invoic_iPaymentD.GetDouble(18).ToString(),
                        change = _get_c_invoic_iPaymentD.GetDouble(19).ToString(),
                        lossamount = _get_c_invoic_iPaymentD.GetDouble(20).ToString(),
                        extraamount = _get_c_invoic_iPaymentD.GetDouble(21).ToString(),
                        cash = _get_c_invoic_iPaymentD.GetDouble(22).ToString(),
                        card = _get_c_invoic_iPaymentD.GetDouble(23).ToString(),
                        exchange = _get_c_invoic_iPaymentD.GetDouble(24).ToString(),
                        redemption = _get_c_invoic_iPaymentD.GetDouble(25).ToString(),
                        iscomplementary = _get_c_invoic_iPaymentD.GetString(26),
                        iscredit = _get_c_invoic_iPaymentD.GetString(27),
                        name_id = _get_c_invoic_iPaymentD.GetString(28),
                        mobile_numbler = _get_c_invoic_iPaymentD.GetString(29),
                        reason = _get_c_invoic_iPaymentD.GetString(30),
                        c_bpartner_name = _get_c_invoic_iPaymentD.GetString(31),
                        c_status = _get_c_invoic_iPaymentD.GetString(32),
                    });
                }
                connection.Close();

                await Task.Run(() =>
                {
                    Invoice_Post_items.ToList().ForEach(x =>
                    {
                        #region Re-Posting Each Invoice to Sever and Updating the is_posted flag to 'N'

                        string pricelistid, costelementid, currencyid, currencycode, cashbookid, periodid, paymenttermid, adtableid,
                        accountschemaid, paymentrule, printsalessummary, printprebill, showcomplement, isdiscount,
                        discpercent, name, password, islogged, isactive, sessionid;
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_ad_sys_config = new NpgsqlCommand("SELECT pricelistid, c_bpartner_id,costelementid, currencyid, currencycode, " +
                            "cashbookid, periodid,paymenttermid, adtableid, accountschemaid, paymentrule, printsalessummary, printprebill, showcomplement, " +
                            "isdiscount, discpercent  " +
                            "FROM ad_sys_config " +
                            "WHERE " +
                            "ad_client_id = " + x.ad_client_id + " AND ad_org_id = " + x.ad_org_id + " AND ad_user_id = " + x.ad_user_id + " ;", connection);
                        NpgsqlDataReader _get_c_invoic_ad_sys_config = cmd_c_invoic_ad_sys_config.ExecuteReader();

                        _get_c_invoic_ad_sys_config.Read();
                        pricelistid = _get_c_invoic_ad_sys_config.GetInt64(0).ToString();
                        costelementid = _get_c_invoic_ad_sys_config.GetInt64(2).ToString();
                        currencyid = _get_c_invoic_ad_sys_config.GetInt64(3).ToString();
                        currencycode = _get_c_invoic_ad_sys_config.GetString(4);
                        cashbookid = _get_c_invoic_ad_sys_config.GetInt64(5).ToString();
                        periodid = _get_c_invoic_ad_sys_config.GetInt64(6).ToString();
                        paymenttermid = _get_c_invoic_ad_sys_config.GetInt64(7).ToString();
                        adtableid = _get_c_invoic_ad_sys_config.GetInt64(8).ToString();
                        accountschemaid = _get_c_invoic_ad_sys_config.GetInt64(9).ToString();
                        paymentrule = _get_c_invoic_ad_sys_config.GetString(10);
                        printsalessummary = _get_c_invoic_ad_sys_config.GetString(11);
                        printprebill = _get_c_invoic_ad_sys_config.GetString(12);
                        showcomplement = _get_c_invoic_ad_sys_config.GetString(13);
                        isdiscount = _get_c_invoic_ad_sys_config.GetString(14);
                        discpercent = _get_c_invoic_ad_sys_config.GetInt64(15).ToString();

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_c_invoiceline = new NpgsqlCommand("SELECT " +
                            "t1.m_product_id," +          //0
                            "t1.productname," +           //1
                            "t1.paroductarabicname," +    //2
                            "t1.productbarcode," +        //3
                            "t1.c_uom_id," +              //4
                            "t1.uomname," +               //5
                            "t1.qtyinvoiced," +           //6
                            "t1.qtyentered," +            //7
                            "t1.saleprice," +             //8
                            "t1.costprice," +             //9
                            "t1.discounttype," +          //10
                            "t1.discountvalue," +         //11
                            "t1.linetotalamt," +          //12
                            "t1.pricelistid," +           //13
                            "t1.islinediscounted," +      //14
                            "t2.m_product_category_id " + //15
                            "FROM c_invoiceline t1 , m_product t2 " +
                            "WHERE t1.m_product_id = t2.m_product_id " +
                            "AND t1.ad_client_id = " + x.ad_client_id + " " +
                            "AND t1.ad_org_id =" + x.ad_org_id + "  " +
                            "AND t1.c_invoice_id = " + x.c_invoice_id + " " +
                            "AND t1.ad_user_id = " + x.ad_user_id + " ;", connection);
                        NpgsqlDataReader _get_c_invoic_c_invoiceline = cmd_c_invoic_c_invoiceline.ExecuteReader();

                        while (_get_c_invoic_c_invoiceline.Read())
                        {
                            double _dicount = _get_c_invoic_c_invoiceline.GetDouble(11),
                            PdiscountType = _get_c_invoic_c_invoiceline.GetDouble(10),
                            _price = _get_c_invoic_c_invoiceline.GetDouble(12),
                            _actualPrice = _get_c_invoic_c_invoiceline.GetDouble(8),
                            _qty = _get_c_invoic_c_invoiceline.GetInt64(7);
                            string _discountType, _dicountPercent, _discountAmount;
                            if (PdiscountType == 0)
                            {
                                _discountType = "P";
                                _dicountPercent = _dicount.ToString();
                                _discountAmount = Math.Round((((_actualPrice * _qty) * _dicount) / 100), 2).ToString();
                            }
                            else
                            {
                                _discountType = "A";
                                _dicountPercent = Math.Round(((_dicount / _price) * 100), 2).ToString();
                                _discountAmount = _dicount.ToString();
                            }
                            OrderDetails_items.Add(new OrderDetails()
                            {
                                isExists = "N",
                                KotLineID = "0",
                                description = "",
                                uomId = _get_c_invoic_c_invoiceline.GetInt64(4).ToString(),
                                productUOMValue = _get_c_invoic_c_invoiceline.GetString(5),
                                actualPrice = _get_c_invoic_c_invoiceline.GetDouble(8).ToString(),
                                costPrice = _get_c_invoic_c_invoiceline.GetInt64(9).ToString(),
                                orderedQty = _get_c_invoic_c_invoiceline.GetInt64(6).ToString(),
                                qty = _get_c_invoic_c_invoiceline.GetInt64(7).ToString(),
                                discountType = _discountType,
                                dicountPercent = _dicountPercent,
                                discountAmount = _discountAmount,
                                price = _price.ToString(),
                                productId = _get_c_invoic_c_invoiceline.GetInt64(0).ToString(),
                                productName = _get_c_invoic_c_invoiceline.GetString(1),
                                productCategoryId = _get_c_invoic_c_invoiceline.GetDouble(15).ToString(),
                            });
                        }
                        //Console.WriteLine("OrderDetails_items  "+ OrderDetails_items.Count);
                        //log.Info("OrderDetails_items  " + OrderDetails_items.Count);
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_c_invoic_ad_user_pos = new NpgsqlCommand("SELECT " +
                            "name, " +             //0
                            "password, " +         //1
                            "islogged, " +         //2
                            "isactive," +          //3
                            "sessionid  " +        //4
                            "FROM ad_user_pos " +
                            "WHERE ad_client_id = " + x.ad_client_id + " AND ad_org_id = " + x.ad_org_id + " AND ad_user_id = " + x.ad_user_id + " ; ", connection);
                        NpgsqlDataReader _get_c_invoic_ad_user_pos = cmd_c_invoic_ad_user_pos.ExecuteReader();
                        _get_c_invoic_ad_user_pos.Read();
                        name = _get_c_invoic_ad_user_pos.GetString(0);
                        password = _get_c_invoic_ad_user_pos.GetString(1);
                        islogged = _get_c_invoic_ad_user_pos.GetString(2);
                        isactive = _get_c_invoic_ad_user_pos.GetString(3);
                        sessionid = _get_c_invoic_ad_user_pos.GetInt64(4).ToString();
                        connection.Close();

                        #region JSON Array Formate 
                        JObject rss = new JObject();
                        if (x.c_status == "Canceled")
                        {
                            rss =
                          new JObject(

                              new JProperty("authorizedBy", accountschemaid),
                              new JProperty("SyncedTime", name),
                              new JProperty("remindMe", password),
                              new JProperty("reason", x.ad_client_id),
                              new JProperty("OrderDetails",
                                    new JArray(
                                        from p in OrderDetails_items
                                        select new JObject(
                                            new JProperty("costPrice", p.costPrice),
                                            new JProperty("orderedQty", p.orderedQty),
                                            new JProperty("dicountPercent", p.dicountPercent),
                                            new JProperty("qty", p.qty),
                                            new JProperty("isExists", p.isExists),
                                            new JProperty("uomId", p.uomId),
                                            new JProperty("discountAmount", p.discountAmount),
                                            new JProperty("productId", p.productId),
                                            new JProperty("actualPrice", p.actualPrice),
                                            new JProperty("KotLineID", p.KotLineID),
                                            new JProperty("price", (Convert.ToDouble(p.actualPrice) - (Convert.ToDouble(p.discountAmount) / Convert.ToDouble(p.qty)))),
                                            new JProperty("description", p.description),
                                            new JProperty("productUOMValue", p.productUOMValue),
                                           new JProperty("discountType", p.discountType),
                                            new JProperty("productName", p.productName),
                                            new JProperty("productCategoryId", p.productCategoryId)
                                                           )
                                                )//JArray END
                                           ),//OrderDetails END 
                                              new JProperty("showImage", "Y"),
                                             new JProperty("warehouseId", x.card),
                                              new JProperty("macAddress", DeviceMacAdd),
                                              new JProperty("businessPartnerId", periodid),
                                              new JProperty("password", password),
                                              new JProperty("clientId", x.redemption),
                                             new JProperty("version", 1.0),
                                              new JProperty("appName", "POS"),
                                              new JProperty("orgId", currencyid),
                                              new JProperty("operation", "POSOrderCancel"),
                                              new JProperty("username", name),
                                            new JProperty("sessionId", sessionid),
                                             new JProperty("userId", accountschemaid),
                              new JProperty("PaymentDetails",
                                  new JArray(
                                      new JObject(
                                              new JProperty("amount", x.exchange),
                                              new JProperty("paymenttype", "EXCHANGE")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.cash),
                                              new JProperty("paymenttype", "CASH")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.card),
                                              new JProperty("paymenttype", "CARD")
                                          ),
                                          new JObject(
                                              new JProperty("amount", x.redemption),
                                              new JProperty("paymenttype", "LOYALTY")
                                          )
                                      )
                                    ),//PaymentDetails END
                              new JProperty("OrderHeaders",
                                    new JObject(
                                         new JProperty("IsCash", "N"),
                                         new JProperty("periodId", periodid),
                                         new JProperty("docNo", x.documentno),
                                         new JProperty("currencyId", currencyid),
                                         new JProperty("creditName", x.name_id),
                                         new JProperty("businessPartnerId", x.c_bpartner_id),
                                         new JProperty("orgId", x.ad_org_id),
                                         new JProperty("cardAmount", x.card),
                                         new JProperty("mobilenumber", x.mobile_numbler),
                                         new JProperty("cashbookId", cashbookid),
                                         new JProperty("dueAmount", x.balance),
                                         new JProperty("userId", x.ad_user_id),
                                         new JProperty("totalAmount", x.grandtotal),
                                         new JProperty("posId", x.c_invoice_id),
                                         new JProperty("isReturned", x.is_return),
                                        new JProperty("redemptionAmount", x.redemption),
                                        new JProperty("createdDate", x.created),
                                        new JProperty("customerName", x.c_bpartner_name),
                                        new JProperty("lossAmount", x.lossamount),
                                        new JProperty("exchangeAmount", x.exchange),
                                        new JProperty("extraAmount", x.extraamount),
                                        new JProperty("refInvoiceId", 0),
                                        new JProperty("totalLines", Convert.ToUInt32(OrderDetails_items.Count())),
                                        new JProperty("qid", x.qid),
                                        new JProperty("IsCard", "N"),
                                        new JProperty("warehouseId", x.m_warehouse_id),
                                        new JProperty("paymentTermId", paymenttermid),
                                        new JProperty("clientId", x.ad_client_id),
                                        new JProperty("pricelistId", pricelistid),
                                        new JProperty("adTableId", adtableid),
                                         new JProperty("paidAmount", Math.Round(Convert.ToDouble(x.cash) + Convert.ToDouble(x.card) + Convert.ToDouble(x.exchange) + Convert.ToDouble(x.redemption), 2)),
                                         new JProperty("giftAmount", 0),
                                         new JProperty("accountSchemaId", accountschemaid),
                                        new JProperty("warehouseNo", 0),
                                        new JProperty("cashAmount", x.cash),
                                        new JProperty("salesrepId", x.ad_user_id)
                                                    // new JProperty("isComplement", x.iscomplementary),
                                                    // new JProperty("isCredit", x.iscredit)
                                                    )
                                )
                             );
                        }
                        else if (x.c_status == "Completed")
                        {
                            rss =
                     new JObject(
                         new JProperty("operation", "POSOrderRelease"),
                         new JProperty("username", name),
                         new JProperty("password", password),
                         new JProperty("clientId", x.ad_client_id),
                         new JProperty("orgId", x.ad_org_id),
                         new JProperty("userId", x.ad_user_id),
                         new JProperty("roleId", x.ad_role_id),
                         new JProperty("sessionId", 0),
                         new JProperty("businessPartnerId", x.c_bpartner_id),
                         new JProperty("warehouseId", x.m_warehouse_id),
                         new JProperty("SyncedTime", 0),
                         new JProperty("reason", x.reason),
                         new JProperty("showImage", "Y"),
                         new JProperty("macAddress", DeviceMacAdd),//LoginViewModel._DeviceMacAddress
                         new JProperty("version", 1.0),
                         new JProperty("appName", "POS"),
                         new JProperty("remindMe", "Y"),

                         new JProperty("OrderHeaders",
                             new JObject(
                                  new JProperty("isReturned", x.is_return),
                                 new JProperty("clientId", x.ad_client_id),
                                 new JProperty("orgId", x.ad_org_id),
                                 new JProperty("warehouseId", x.m_warehouse_id),
                                 new JProperty("userId", x.ad_user_id),
                                 new JProperty("periodId", periodid),
                                 new JProperty("currencyId", currencyid),
                                 new JProperty("cashbookId", cashbookid),
                                 new JProperty("paymentTermId", paymenttermid),
                                 new JProperty("pricelistId", pricelistid),
                                 new JProperty("adTableId", adtableid),
                                 new JProperty("accountSchemaId", accountschemaid),
                                 new JProperty("createdDate", x.created),
                                 new JProperty("posId", x.c_invoice_id),
                                 new JProperty("docNo", x.documentno),
                                 new JProperty("refInvoiceId", 0),
                                 new JProperty("totalLines", Convert.ToUInt32(OrderDetails_items.Count())),
                                 new JProperty("qid", x.qid),
                                 new JProperty("warehouseNo", 0),
                                 new JProperty("customerName", x.c_bpartner_name),
                                 new JProperty("creditName", x.name_id),
                                 new JProperty("businessPartnerId", x.c_bpartner_id),
                                 new JProperty("mobilenumber", x.mobile_numbler),
                                 new JProperty("totalAmount", x.grandtotal),
                                 new JProperty("cashAmount", x.cash),
                                 new JProperty("cardAmount", x.card),
                                 new JProperty("exchangeAmount", x.exchange),
                                 new JProperty("redemptionAmount", x.redemption),
                                 new JProperty("paidAmount", Math.Round(Convert.ToDouble(x.cash) + Convert.ToDouble(x.card) + Convert.ToDouble(x.exchange) + Convert.ToDouble(x.redemption), 2)),
                                 new JProperty("dueAmount", x.balance),
                                 new JProperty("lossAmount", x.lossamount),
                                 new JProperty("extraAmount", x.extraamount),
                                 new JProperty("IsCash", "N"),
                                 new JProperty("IsCard", "N"),
                                 new JProperty("isComplement", x.iscomplementary),
                                 new JProperty("isCredit", x.iscredit)
                                             )
                                        ),//OrderHeaders END
                         new JProperty("OrderDetails",
                             new JArray(
                                 from p in OrderDetails_items
                                 select new JObject(
                                     new JProperty("isExists", p.isExists),
                                     new JProperty("KotLineID", p.KotLineID),
                                     new JProperty("description", p.description),
                                     new JProperty("uomId", p.uomId),
                                     new JProperty("productUOMValue", p.productUOMValue),
                                     new JProperty("actualPrice", p.actualPrice),
                                     new JProperty("costPrice", p.costPrice),
                                     new JProperty("orderedQty", p.orderedQty),
                                     new JProperty("qty", p.qty),
                                     new JProperty("discountType", p.discountType),
                                     new JProperty("dicountPercent", p.dicountPercent),
                                     new JProperty("discountAmount", p.discountAmount),
                                     new JProperty("price", (Convert.ToDouble(p.actualPrice) - (Convert.ToDouble(p.discountAmount) / Convert.ToDouble(p.qty)))),
                                     new JProperty("productId", p.productId),
                                     new JProperty("productName", p.productName),
                                     new JProperty("productCategoryId", p.productCategoryId)
                                                    )
                                         )//JArray END
                                    ),//OrderDetails END
                         new JProperty("PaymentDetails",
                             new JArray(
                                 new JObject(
                                         new JProperty("amount", x.exchange),
                                         new JProperty("paymenttype", "EXCHANGE")
                                     ),
                                     new JObject(
                                         new JProperty("amount", x.cash),
                                         new JProperty("paymenttype", "CASH")
                                     ),
                                     new JObject(
                                         new JProperty("amount", x.card),
                                         new JProperty("paymenttype", "CARD")
                                     ),
                                     new JObject(
                                         new JProperty("amount", x.redemption),
                                         new JProperty("paymenttype", "LOYALTY")
                                     )
                                 )
                               )//PaymentDetails END
                        );
                        }
                        #endregion JSON Array Formate 

                        #region Posting to Server

                        int CheckApiError = 0;
                        var val = rss.ToString();
                        //log.Info("----------------JSON Request--------------");
                        //log.Info(val);
                        //log.Info("----------------JSON END--------------");
                        try
                        {
                            POSReleaseOrderApiStringResponce = PostgreSQL.ApiCallPost(val);
                            CheckApiError = 1;
                        }
                        catch
                        {
                            CheckApiError = 0;
                            log.Error("POSReleaseOrderApi: Server Error");
                            log.Error("----------------JSON Request--------------");
                            log.Error(val);
                            log.Error("----------------JSON END------------------");
                        }
                        if (CheckApiError == 1)
                        {
                            syncedcount += 1;
                            POSReleaseOrderApiJSONResponce = JsonConvert.DeserializeObject(POSReleaseOrderApiStringResponce);
                            log.Info("POSReleaseOrderApiJSONResponce: " + POSReleaseOrderApiJSONResponce + "");
                            if (POSReleaseOrderApiJSONResponce.responseCode == "200")
                            {
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_Update_post_flag_c_invoic = new NpgsqlCommand("UPDATE c_invoice " +
                                    "SET is_posted = 'Y' " +
                                    "WHERE c_invoice_id = " + x.c_invoice_id + "; ", connection);
                                cmd_Update_post_flag_c_invoic.ExecuteReader();
                                connection.Close();
                                log.Info("Posted Invoice Flag Updated|#invoice number: " + x.c_invoice_id + "");
                                this.Dispatcher.Invoke(() =>
                                {
                                    ReSyncProgressbarText.Text = $"Total {syncedcount}/{totalcount} Resyncing";
                                });
                            }
                            else
                            {
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_Update_post_ERROR_ = new NpgsqlCommand("UPDATE c_invoice " +
                                    "SET is_posted = 'N' , is_posterror = 'Y' " +
                                    "WHERE c_invoice_id = " + x.c_invoice_id + "; ", connection);
                                cmd_Update_post_ERROR_.ExecuteNonQuery();
                                connection.Close();

                                log.Error("RePosting Invoice Failed|Responce Code: " + POSReleaseOrderApiJSONResponce.responseCode);
                                log.Error("----------------JSON Request--------------");
                                log.Error(val);
                                log.Error("----------------JSON END--------------");
                            }
                        }

                        OrderDetails_items.Clear();

                        #endregion Posting to Server

                        #endregion Posting Each Invoice to Sever and Updating the is_posted flag to 'Y'
                    });
                });

                //Clearing List Memory
                OrderHeaders_items.Clear();
                Invoice_Post_items.Clear();
                OrderDetails_items.Clear();
                connection.Close();

                #endregion Posting Background Function

                Console.WriteLine("Invoice Timmer Function Ended");
                this.Dispatcher.Invoke(() =>
                {
                    ReSyncProgressbarText.Text = "Total Invoice Resynced : " + syncedcount + " out of " + totalcount;
                    __Side_Menu_Page.IsEnabled = true;
                    ReSyncProgressbar.Visibility = Visibility.Hidden;
                    ReSyncProgressbarText.Visibility = Visibility.Hidden;
                });

                if (!(totalcount == syncedcount))
                {
                    NotifierViewModel.Notifier.ShowWarning("Total Invoice Resynced : " + syncedcount + " out of " + totalcount);
                }
                else
                {
                    NotifierViewModel.Notifier.ShowInformation("     All Invoice Synced");
                }
                // Bind_Product_Search();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void SessionChangePrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Error_page.Visibility = Visibility.Hidden;
                Session_Check.Visibility = Visibility.Hidden;

                add_Product();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void txt_Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");//allow decimal points
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else

                e.Handled = true;
        }


        private void txt_Price_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Price.Text != "")
                {
                    Error_page.Visibility = Visibility.Hidden;
                    Session_Check.Visibility = Visibility.Hidden;
                    add_Product();
                    txt_Price.Text = "";
                }
                else
                {
                    Error_page.Visibility = Visibility.Hidden;
                    Session_Check.Visibility = Visibility.Hidden;
                    txt_Price.Text = "";
                    add_Product();
                    txt_Price.Text = "";
                }

            }

        }

        private void txt_Price_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
            //(sender as TextBox).Select(0, (sender as TextBox).Text.Length);
            (sender as TextBox).Focus();
            e.Handled = true;
        }

        private void CustLeft_Click(object sender, RoutedEventArgs e)
        {
            SessionResume.Visibility = Visibility.Hidden;
            SessionClose.Visibility = Visibility.Hidden;
            SessionCreateNew.Visibility = Visibility.Hidden;
            txt_openingBal.Visibility = Visibility.Hidden;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;


            Session_Check.Visibility = Visibility.Hidden;
            Error_page.Visibility = Visibility.Hidden;
            CustLeft.Visibility = Visibility.Hidden;
            WrongOrder.Visibility = Visibility.Hidden;
            CancelYes.Visibility = Visibility.Hidden;
            CancelNo.Visibility = Visibility.Hidden;
            Sale_Cancel("Customer Left");

            Keyboard.Focus(productSearch_cart);
            Percentage_OR_Price = "%";
            OverAllDiscount_button.IsEnabled = false;
            OrderComplected_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
            Back_OR_Esc();
        }

        private void WrongOrder_Click(object sender, RoutedEventArgs e)
        {
            SessionResume.Visibility = Visibility.Hidden;
            SessionClose.Visibility = Visibility.Hidden;
            SessionCreateNew.Visibility = Visibility.Hidden;
            txt_openingBal.Visibility = Visibility.Hidden;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;


            Session_Check.Visibility = Visibility.Hidden;
            Error_page.Visibility = Visibility.Hidden;
            CustLeft.Visibility = Visibility.Hidden;
            WrongOrder.Visibility = Visibility.Hidden;
            CancelYes.Visibility = Visibility.Hidden;
            CancelNo.Visibility = Visibility.Hidden;
            Sale_Cancel("Wrong Order");

            Keyboard.Focus(productSearch_cart);
            Percentage_OR_Price = "%";
            OverAllDiscount_button.IsEnabled = false;
            OrderComplected_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;
            Back_OR_Esc();
        }

        private void CancelYes_Click(object sender, RoutedEventArgs e)
        {
            SessionHead.Visibility = Visibility.Visible;
            SessionHead.Text = "Cancel Order Alert";
            SessionDescription.Text = "Reason for Cancel!!";
            SessionResume.Visibility = Visibility.Hidden;
            SessionClose.Visibility = Visibility.Hidden;
            SessionCreateNew.Visibility = Visibility.Hidden;
            txt_openingBal.Visibility = Visibility.Hidden;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;

            Session_Check.Visibility = Visibility.Visible;
            Error_page.Visibility = Visibility.Visible;
            CustLeft.Visibility = Visibility.Visible;
            WrongOrder.Visibility = Visibility.Visible;
            CancelYes.Visibility = Visibility.Hidden;
            CancelNo.Visibility = Visibility.Hidden;

            Keyboard.Focus(productSearch_cart);
            Percentage_OR_Price = "%";
            OverAllDiscount_button.IsEnabled = false;
            OrderComplected_button.IsEnabled = false;
            OrderCancel_button.IsEnabled = false;

        }

        private void CancelNo_Click(object sender, RoutedEventArgs e)
        {
            SessionResume.Visibility = Visibility.Hidden;
            SessionClose.Visibility = Visibility.Hidden;
            SessionCreateNew.Visibility = Visibility.Hidden;
            txt_openingBal.Visibility = Visibility.Hidden;
            txt_Price.Visibility = Visibility.Hidden;
            SessionChangePrice.Visibility = Visibility.Hidden;


            Session_Check.Visibility = Visibility.Hidden;
            Error_page.Visibility = Visibility.Hidden;
            CustLeft.Visibility = Visibility.Hidden;
            WrongOrder.Visibility = Visibility.Hidden;
            CancelYes.Visibility = Visibility.Hidden;
            CancelNo.Visibility = Visibility.Hidden;

            Keyboard.Focus(productSearch_cart);
            Percentage_OR_Price = "%";

            if (items.Count > 0)
            {
                OverAllDiscount_button.IsEnabled = true;
                OrderComplected_button.IsEnabled = true;
                OrderCancel_button.IsEnabled = true;
            }
            else
            {
                OverAllDiscount_button.IsEnabled = false;
                OrderComplected_button.IsEnabled = false;
                OrderCancel_button.IsEnabled = false;
            }

        }

        private void BackTOCart_from_quick_menu_page_Click(object sender, RoutedEventArgs e)
        {

        }

        private void View_quick_iteamProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Load_CategoryList(false);
                Load_uomList();
                fetch_all_products_details();
                ICollectionView view =
             CollectionViewSource.GetDefaultView(quickdataSource);
                new TextSearchFilter(view, this.txtprodSearch, lstproducts);
                Menu_Page_Product.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Error_page.Visibility = Visibility.Visible;
                paymentPopup.IsOpen = true;

                lstprod_Bind_Qucik_Product_Search();
                ICollectionView view =
              CollectionViewSource.GetDefaultView(quickdataSource);
                new TextSearchFilter(view, this.txtSearch, lstprod);

                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        int btn_pruduct_id;
        // string btn_produ_ct = ""; 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (UIElement ele in MainGrid.Children)
                {
                    if (ele.GetType() == typeof(StackPanel))
                    {
                        StackPanel stk = (StackPanel)ele;
                        //Do the Work on label like below mentioned
                        Button btn = (Button)stk.Children[0];
                        btn.Background = Brushes.White;
                        btn.Foreground = Brushes.Black;
                        //Button btn = (Button)ele;
                        //Do the Work on label like below mentioned
                        // btn.Background = Brushes.White;

                    }
                }
                Button b = sender as Button;
                btn_pruduct_id = Convert.ToInt32(b.Tag.ToString().Split('|')[0]);
                b.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                b.Foreground = Brushes.White;
                quick_ValueChanger_key_pad.Text = Convert.ToDouble(b.Tag.ToString().Split('|')[1]).ToString("0.00");
                //Button b = sender as Button;
                //TextBlock textBox = null;
                //if (b != null)
                //{
                //    var frameworkElement = ((StackPanel)b.Content).Children[1];
                //    textBox = (TextBlock)frameworkElement;
                //    btn_pruduct_id = Convert.ToInt32(textBox.Text);
                //    (sender as Button).Background = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                //}
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }
        long Productid = 0;
        private void lstprod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (lstprod.SelectedItem != null)
                {
                    Productid = (lstprod.SelectedItem as QuickProductList).ProdID;

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            paymentPopup.IsOpen = false;
            Error_page.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Product_toCart_From_Quick();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void Add_Product_toCart_From_Quick()
        {
            string SelectProduct = "SELECT  m_product.ad_client_id,m_product.ad_org_id,m_product. m_product_id,m_product.m_product_category_id,m_product.name,m_product.searchkey,m_product. arabicname,m_product.image," +
      "m_product.scanbyweight, m_product.scanbyprice, m_product.uomid, m_product.uomname, m_product.sopricestd, m_product.currentcostprice, m_product.attribute1 " +
      " FROM m_product , ad_sys_config" +
      " WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.m_product_id = @_searchkey";
            //" AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

            BarcodeSearch_products(SelectProduct, btn_pruduct_id.ToString(), 1, true);

        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            quick_ValueChanger_key_pad.Text = "0.00";
            Back_OR_Esc();
            Keyboard.Focus(productSearch_cart);

        }


        private async void Load_Products(object sender, EventArgs e)
        {
            try
            {
                lstprodadd.Dispatcher.Invoke(() =>
                {
                    ReSyncProgressbar_quick.Visibility = Visibility.Visible;
                    Qick_ReSyncProgressbarText.Visibility = Visibility.Visible;
                    lstprodadd.Visibility = Visibility.Hidden;
                    Bind_Quick_Product_Search();
                    ICollectionView view =
                     CollectionViewSource.GetDefaultView(lstprodadd_quickdataSource);
                    new TextSearchFilter(view, this.txtSearch, lstprodadd);

                });

                await Task.Run(() =>
                {
                    // prodCount.Text = lstprodadd_quickdataSource.Count().ToString() + " Product Found in Qsale";
                });
                ReSyncProgressbar_quick.Visibility = Visibility.Hidden;
                Qick_ReSyncProgressbarText.Visibility = Visibility.Hidden;
                lstprodadd.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void BackTOCart_from_side_menu_Add_Product_page_Click(object sender, RoutedEventArgs e)
        {
            Menu_Page_Product.Visibility = Visibility.Hidden;
        }


        public long Sequenc_id { get; set; }
        public int WarehouseId_Selected { get; set; }
        private void lstprodadd_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = this.lstprodadd.SelectedIndex;

            select_lstproductAgrreegator(index);
        }
        //private void lstprodadd_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    lstprodadd.SelectedItems.Clear();

        //    ListViewItem item = sender as ListViewItem;
        //    if (item != null)
        //    {
        //        item.IsSelected = true;
        //        lstprodadd.SelectedItem = item;
        //    }
        //}

        public static Visual GetChildrenByType(Visual visualElement, Type typeElement, string nameElement)
        {
            if (visualElement == null) return null;
            if (visualElement.GetType() == typeElement)
            {
                FrameworkElement fe = visualElement as FrameworkElement;
                if (fe != null)
                {
                    if (fe.Name == nameElement)
                    {
                        return fe;
                    }
                }
            }
            Visual foundElement = null;
            if (visualElement is FrameworkElement)
                (visualElement as FrameworkElement).ApplyTemplate();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visualElement); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(visualElement, i) as Visual;
                foundElement = GetChildrenByType(visual, typeElement, nameElement);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        private void PackIcon_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Add_Products_List_Window()
        {
            txtprodSearch.Text = "";
            Menu_Page_Product.Visibility = Visibility.Visible;
            grdprodviewlist.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "Add_Products_View_GotFocus";
            txtadddeleteunits.Visibility = Visibility.Hidden;
            Keyboard.Focus(txtprodSearch);
            lstuomList.ItemsSource = null;
            lstuomList.Items.Clear();
            grdprodadd.Visibility = Visibility.Hidden;

            quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
            btnaddprod.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
            dock_save_cancel_btn.Visibility = Visibility.Hidden;
            Change_Content_of_Side_Menu_buttons("Add_Products");
            ddlCategory.SelectedIndex = 0;
            Load_uomList();
            Load_CategoryList(false);
            txtprodSearch.Text = "";
            ddlCategory.SelectedIndex = 0;
            fetch_all_products_details();
            ICollectionView view =
          CollectionViewSource.GetDefaultView(quickdataSource);
            new TextSearchFilter(view, this.txtprodSearch, lstproducts);
            quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
            quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
            bind_unitlist();
            Keyboard.Focus(txtprodSearch);
            lstproducts.SelectedIndex = 0;
            var MyList = (ListView)lstproducts;
            //standard
            if (MyList.Items.Count > 0)
            {
                foreach (var product in MyList.ItemsSource)
                {

                    view_Product(((QuickProductList)product).ProdID.ToString());
                    return;
                    //do stuff with it modify the entry text whatever you want
                }
            }
        }
        private void Side_Menu_Add_products_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Products_List_Window();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
            //  ICollectionView view =
            //  CollectionViewSource.GetDefaultView(lstproduct_quickdataSource);
            //    new TextSearchFilter(view, this.txtproductSearch); 
        }

        private void BackTOCart_from_side_menu__Product_page_Click(object sender, RoutedEventArgs e)
        {
            //Menu_Page_Product.Visibility = Visibility.Hidden;
            //Tittle_Bar_Right_Content.Visibility = Visibility.Visible;
            //btnaddprod.Visibility = Visibility.Visible;
            //btnEdit.Visibility = Visibility.Visible 
            try
            {
                if (Check_keyboard_Focus == "Add_Products_View_GotFocus" && quick_Check_windows_Focus == "")
                {
                    Back_OR_Esc();
                    return;
                }
                if (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" && quick_Check_windows_Focus == "")
                {
                    Menu_Page_Product.Visibility = Visibility.Visible;
                    grdprodviewlist.Visibility = Visibility.Visible;
                    txtprodSearch.Text = "";
                    ddlCategory.SelectedIndex = 0;
                    fetch_all_products_details();
                    Keyboard.Focus(txtprodSearch);
                    lstuomList.ItemsSource = null;
                    lstuomList.Items.Clear();
                    grdprodadd.Visibility = Visibility.Hidden;
                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
                    btnaddprod.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    dock_save_cancel_btn.Visibility = Visibility.Hidden;
                    Change_Content_of_Side_Menu_buttons("Add_Products");
                    ddlCategory.SelectedIndex = 0;
                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                    Check_keyboard_Focus = "Add_Products_View_GotFocus";
                    bind_unitlist();
                    return;
                }
                if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                {
                    __Side_quickMenu_Page.Visibility = Visibility.Visible;
                    Menu_Page_Product.Visibility = Visibility.Hidden;
                    Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                    load_Quick_Products_Btn();
                    Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                    return;
                }
                if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "")
                {
                    if (quick_Side_Menu_Page_Edit_Unit_Content.Visibility == Visibility.Visible)
                    {
                        quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        Check_keyboard_Focus = "Add_Products_View_Unit_GotFocus";
                        bind_unitlist();
                        quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        btnaddprod.Visibility = Visibility.Visible;
                        btnEdit.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        Menu_Page_Product.Visibility = Visibility.Visible;
                        grdprodviewlist.Visibility = Visibility.Visible;
                        txtprodSearch.Text = "";
                        ddlCategory.SelectedIndex = 0;
                        fetch_all_products_details();
                        Keyboard.Focus(txtprodSearch);
                        lstuomList.ItemsSource = null;
                        lstuomList.Items.Clear();
                        grdprodadd.Visibility = Visibility.Hidden;

                        quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
                        btnaddprod.Visibility = Visibility.Visible;
                        btnEdit.Visibility = Visibility.Visible;
                        dock_save_cancel_btn.Visibility = Visibility.Hidden;
                        Change_Content_of_Side_Menu_buttons("Add_Products");
                        ddlCategory.SelectedIndex = 0;
                        quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        bind_unitlist();
                        quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        Check_keyboard_Focus = "Add_Products_View_GotFocus";
                        return;

                    }

                }
                if (Check_keyboard_Focus == "Add_Products_View_Unit_GotFocus" && quick_Check_windows_Focus == "")
                {
                    Menu_Page_Product.Visibility = Visibility.Visible;
                    grdprodviewlist.Visibility = Visibility.Visible;
                    txtprodSearch.Text = "";
                    ddlCategory.SelectedIndex = 0;
                    fetch_all_products_details();
                    Keyboard.Focus(txtprodSearch);
                    lstuomList.ItemsSource = null;
                    lstuomList.Items.Clear();
                    grdprodadd.Visibility = Visibility.Hidden;
                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
                    btnaddprod.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    dock_save_cancel_btn.Visibility = Visibility.Hidden;
                    Change_Content_of_Side_Menu_buttons("Add_Products");
                    ddlCategory.SelectedIndex = 0;
                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;

                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                    Check_keyboard_Focus = "Add_Products_View_GotFocus";
                    bind_unitlist();
                    return;

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void update_productview_listitem()
        {
            try
            {
                if (Menu_Page_Product.Visibility == Visibility.Visible)
                {
                    fetch_all_products_details();

                    ICollectionView view =
                 CollectionViewSource.GetDefaultView(quickdataSource);
                    new TextSearchFilter(view, this.txtprodSearch, lstproducts);
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void ddlCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update_productview_listitem();
        }
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }
        private void lstproducts_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in lstproducts.Items)
                {
                    ListViewItem i = (ListViewItem)lstproducts.ItemContainerGenerator.ContainerFromItem(item);
                    if (i != null)
                    {
                        //Seek out the ContentPresenter that actually presents our DataTemplate
                        ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                        TextBlock txtprodId = (TextBlock)i.ContentTemplate.FindName("txtProdid", contentPresenter);
                        TextBlock txtProdName = (TextBlock)i.ContentTemplate.FindName("txtProd", contentPresenter);
                        txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    }
                }

                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {


                    TextBlock txtprodId = GetChildrenByType(selectedItem, typeof(TextBlock), "txtProdid") as TextBlock;
                    TextBlock txtProdName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtProd") as TextBlock;
                    txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    if (txtprodId != null)
                    {
                        view_Product(txtprodId.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }
        private void view_Product(string productId)
        {

            lblprodid_.Text = productId;
            string query = " select p.searchkey as  barcode,p.name as productname,c.m_product_category_id as categoryid,p.uomname, " +
" p.purchasePrice as purchaseprice, p.sopricestd as salesprice, 10 as minqty, 20 as maxqty,'Y' as case,ispriceeditable,'Y' as sellonline,c.name as categoryname,p.uomid as uomid,currentcostprice as costprice" +
" from m_product p,m_product_category c where p.m_product_category_id = c.m_product_category_id and p.ad_client_id =" + AD_Client_ID + " AND p.ad_org_id =" + AD_ORG_ID + "and p.m_product_id =" + productId;

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {

                lblprodBarcode_.Text = _get_c_products.GetString(0);
                //  Back_with_product.Text = _get_c_products.GetString(0);
                lblprodName_.Text = _get_c_products.GetString(1);
                long categoryid = _get_c_products.GetInt64(2);
                ddlitem.SelectedValue = categoryid;
                lblprodItem_.Text = _get_c_products.GetString(11);
                // txtUom1.SelectedValue= _get_c_products.GetString(12);
                lblprodUom_.Text = _get_c_products.GetString(3);
                long produomid = _get_c_products.GetInt64(12);
                lblprodUomid_.Text = produomid.ToString();


                //if (uom_dataSource.Count == 0)
                {
                    chkprodcase.IsChecked = false;
                }

                // if (_get_c_products.GetInt64(4) != null)
                // {
                lblprodPurchasePrice_.Text = _get_c_products.GetInt64(4).ToString();
                // }


                lblprodCostPrice_.Text = _get_c_products.GetInt64(13).ToString();
                lblprodSalesPrice_.Text = _get_c_products.GetInt64(5).ToString();
                lblprodMinimumQty_.Text = _get_c_products.GetInt32(6).ToString();
                lblprodMaximumQty_.Text = _get_c_products.GetInt32(7).ToString();
                chkprodPriceedit.IsChecked = false;
                if (_get_c_products.GetString(9) == "Y")
                {
                    chkprodPriceedit.IsChecked = true;
                }
                else
                {
                    chkprodPriceedit.IsChecked = false;
                }
                chkprodsellonline.IsChecked = false;
                if (_get_c_products.GetString(10) == "Y")
                {
                    chkprodsellonline.IsChecked = true;
                }
                else
                {
                    chkprodsellonline.IsChecked = false;
                }
            }
            connection.Close();
            quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "Add_Products_View_GotFocus";
            txtadddeleteunits.Visibility = Visibility.Hidden;
            bind_unitlist();
            quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;

            btnaddprod.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
        }
        private void lstproducts_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                lstproducts.SelectedItems.Clear();

                ListViewItem item = sender as ListViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    lstproducts.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }

        public void bind_unitlist()
        {
            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            //  string query = "select p.searchkey,p.name,c.name as categoryname,u.uomid,u.uomvalue,p.sopricestd as salesprice,p.purchasePrice as purchasePrice,u.uomconvrate,p.m_product_category_id,b.m_product_id  from m_product_uom u,m_product p,m_product_category c,m_product_bom b where p.m_product_id=b.m_parent_product_id and  p.m_product_category_id=c.m_product_category_id and u.m_product_id = p.m_product_id and u.ad_client_id= " + AD_Client_ID + " and u.ad_org_id=" + AD_ORG_ID + " and b.m_parent_product_id =" + lblprodid_.Text;
            string query = "";
            uom_dataSource.Clear();
            chkcase.IsChecked = false;
            chkprodcase.IsChecked = false;
            if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus" && POSGetProductApiJSONResponce_Bom != null)
            {
                if (POSGetProductApiJSONResponce_Bom != null)
                {
                    foreach (var productbomLst in POSGetProductApiJSONResponce_Bom)
                    {
                        chkcase.IsChecked = true;
                        chkprodcase.IsChecked = true;

                        uom_dataSource.Add(new Product_Uom
                        {

                            barCode = productbomLst.productValue,
                            productName = productbomLst.productName,
                            uomType = productbomLst.categoryName,
                            uomValue = productbomLst.productUOMValue,
                            categoryid = productbomLst.categoryId,
                            uomid = productbomLst.productUOMId,
                            salesPrice = Convert.ToDecimal(productbomLst.sellingPrice).ToString("0.00"),
                            purchasePrice = Convert.ToDecimal(productbomLst.costPrice).ToString("0.00"),
                            currency = AD_CurrencyCode,
                            productId = productbomLst.productId,
                            costprice = productbomLst.costPrice,
                            categoryName = productbomLst.categoryName,
                            noofpcs = productbomLst.bomQty
                        });

                    }
                }

                btnaddprod.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
                dock_save_cancel_btn.Visibility = Visibility.Visible;
                txtadddeleteunits.Visibility = Visibility.Hidden;
                txtSalesPrice.Focus();
                Keyboard.Focus(txtSalesPrice);
                lstuomList.ItemsSource = null;
                lstuomList.ItemsSource = uom_dataSource;
                txtunitdetails.Text = "Unit details (" + uom_dataSource.Count.ToString() + ")";
                Check_keyboard_Focus = "Add_Products_View_From_No_Sync_GotFocus";
            }
            if (lblprodid_.Text != "")
            {


                query = "select p.searchkey,p.name,c.name as categoryname, p.uomid,p.uomname,p.sopricestd as salesprice, " +
                " p.purchasePrice as purchasePrice,p.m_product_category_id,b.m_product_id,p.m_product_id,p.ispriceeditable," +
                "b.no_of_pcs ,p.purchasePrice as purchasePrice, p.currentcostprice from m_product p, m_product_bom b , m_product_category c where " +
                " p.m_product_category_id = c.m_product_category_id  and p.m_product_id = b.m_product_id and " +
                " b.m_parent_product_id  = " + lblprodid_.Text + " order by p.name asc;";
                connection.Open();
                NpgsqlCommand cmd_fetch_uom = new NpgsqlCommand(query, connection);
                NpgsqlDataReader _get_c_uom = cmd_fetch_uom.ExecuteReader();
                uom_dataSource.Clear();
                while (_get_c_uom.Read())
                {
                    chkcase.IsChecked = true;
                    chkprodcase.IsChecked = true;
                    uom_dataSource.Add(new Product_Uom
                    {
                        barCode = _get_c_uom.GetString(0),
                        productName = _get_c_uom.GetString(1),
                        uomType = _get_c_uom.GetString(2),
                        uomValue = _get_c_uom.GetString(4),
                        categoryid = _get_c_uom.GetInt64(7).ToString(),
                        uomid = _get_c_uom.GetInt64(3).ToString(),
                        salesPrice = Convert.ToDecimal(_get_c_uom.GetDouble(5)).ToString("0.00"),
                        purchasePrice = Convert.ToDecimal(_get_c_uom.GetDouble(6)).ToString("0.00"),
                        currency = AD_CurrencyCode,
                        productId = _get_c_uom.GetInt64(8).ToString(),
                        costprice = Convert.ToDecimal(_get_c_uom.GetDouble(13)).ToString("0.00"),
                        categoryName = _get_c_uom.GetString(2),
                        noofpcs = _get_c_uom.GetInt32(11).ToString()
                    });
                    if (txtunitprodIdbefadd.Text == "")
                    {
                        txtunitprodIdbefadd.Text = _get_c_uom.GetInt64(8).ToString();
                    }
                    else
                    {
                        txtunitprodIdbefadd.Text += "," + _get_c_uom.GetInt64(8).ToString();
                    }
                }
                connection.Close();
                lstuomList.ItemsSource = null;
                lstuomList.ItemsSource = uom_dataSource;
                if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus")
                {

                    dock_save_cancel_btn.Visibility = Visibility.Visible;
                    btnaddprod.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;
                    txtadddeleteunits.Visibility = Visibility.Visible;
                }
                else
                {
                    btnaddprod.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    dock_save_cancel_btn.Visibility = Visibility.Hidden;
                    txtadddeleteunits.Visibility = Visibility.Hidden;
                }

            }

            txtunitdetails.Text = "Unit details (" + uom_dataSource.Count.ToString() + ")";

        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                //NpgsqlCommand UPDATE_cmd_m_warehouse = new NpgsqlCommand("UPDATE m_warehouse SET attribute1 ='" + _itemCount + "' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = " + WarehouseId_Selected + "; ", connection);
                //UPDATE_cmd_m_warehouse.ExecuteNonQuery();
                //connection.Close();


                NpgsqlCommand command = new NpgsqlCommand("select productId-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);


                // Execute the query and obtain the value of the first column of the first row
                Int32 productId = Convert.ToInt32(command.ExecuteScalar());
                string _productId = productId.ToString();

                string _productName = txtName.Text;
                string _productValue = txtBarcode.Text;
                string _categoryId = ddlitem.SelectedValue.ToString();
                string _productArabicName = " ";
                string _scanbyWeight = "";
                string _scanbyPrice = "";
                string _ispriceEditable = "N";
                if (chkprodPriceedit.IsChecked == true)
                { _ispriceEditable = "Y"; }
                else
                { _ispriceEditable = "N"; }

                string _isquick = "N";

                string _productUOMId = txtUom.SelectedValue.ToString();
                string _productUOMValue = txtUom.SelectedItem.ToString();
                string _sellingPrice = txtSalesPrice.Text;
                string _costprice = txtSalesPrice.Text;
                string _purchasePrice = txtPurchasePrice.Text;
                string _productMultiUOM = "";
                JArray _productMultiImage = new JArray();
                int img_count = 0;
                string _product_image = " ";
                if (img_count > 0)
                {
                    _product_image = _productMultiImage[0]["productImage"].ToString();
                }

                connection.Close();

                connection.Open();
                NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                if (_get__Ad_sequenc_no.Read())
                {
                    Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                }

                connection.Close();



                if (!check_product_exist(_productId))
                {
                    connection.Open();
                    NpgsqlCommand cmd_update_pr_no_m_product_price = new NpgsqlCommand("UPDATE m_pos_sequence SET productId =" + productId + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                    NpgsqlDataReader _update__pr_sequenc_no = cmd_update_pr_no_m_product_price.ExecuteReader();
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();
                    connection.Open();

                    NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,sellonline,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@sellonline,@created)", connection);

                    INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                    INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);

                    INSERT_cmd_m_product.ExecuteNonQuery();
                    connection.Close();


                    string _productId_PriceArray = _productId;
                    string _pricelistId_PriceArray = AD_PricelistID.ToString();
                    string _pricelistName_PriceArray = _sellingPrice;
                    string _priceStd_PriceArray = _costprice;

                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                    if (_get__Ad_sequenc_no_m_product_price.Read())
                    {
                        Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                    }
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                            "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                    connection.Close();

                    if (_productMultiUOM == "Y")
                    {

                        string _uomId_UOMArray = _productUOMId;
                        string _uomValue_UOMArray = _productUOMValue;
                        string _uomConvRate_UOMArray = _productUOMValue;

                        connection.Close();

                        connection.Open();
                        cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                        _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                        if (_get__Ad_sequenc_no_m_product_price.Read())
                        {
                            Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                        }
                        connection.Close();

                        connection.Open();
                        INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                      " VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_uom';", connection);
                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                        connection.Close();
                    }

                }
                else
                {
                    log.Info("Product Already exist  ProductId:" + _productId);

                }
                int _m_product_id, _uomid, _m_product_category_id;
                double _currentcostprice, _sopricestd;
                string _name, _uomname, _searchkey, _arabicname, _scanbyweight, _scanbyprice;

                _m_product_id = Convert.ToInt32(_productId);
                _m_product_category_id = Convert.ToInt32(_categoryId);
                _name = _productName;
                _searchkey = txtProduct.Text;
                _arabicname = _productArabicName;
                //  _image = _clientId_Read.GetString(7);
                _scanbyweight = _scanbyWeight;
                _scanbyprice = _scanbyPrice;
                _uomid = Convert.ToInt32(_productUOMId);
                _uomname = _productUOMValue;
                _sopricestd = Convert.ToDouble(_sellingPrice);
                _currentcostprice = Convert.ToDouble(_costprice);
                // _productMultiUOM = _clientId_Read.GetString(14); 

                string _product_name = _name;
                string __product_id = _m_product_id.ToString();
                string _product_barcode = _searchkey;
                string _product_ad_client_id = AD_Client_ID.ToString();
                string _product_ad_org_id = AD_ORG_ID.ToString();
                string _product_m_product_category_id = _m_product_category_id.ToString();
                string _product_arabicname = _arabicname;
                //string _product_image = _image;
                string _product_scanbyweight = _scanbyweight;
                string _product_scanbyprice = _scanbyprice;
                string _product_uomid = _uomid.ToString();
                string _product_uomname = _uomname;
                string _product_SellingPricestd = _sellingPrice;
                string _product_currentcostprice = _currentcostprice.ToString();
                // string _is_productMultiUOM = _productMultiUOM;

                int item_count = items.Count();
                if (item_count == 0)
                {
                    double product_discount = 0;
                    double product_totalAmount = Convert.ToDouble(_product_SellingPricestd);

                    items.Insert(0, new Product()
                    {
                        #region Display Fields

                        Discount = product_discount,
                        Quantity = 1,
                        Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                        Amount = product_totalAmount.ToString("0.00"),
                        Percentpercentage_OR_Price = Percentage_OR_Price,
                        Product_Name = _product_name,
                        Iteam_Barcode = _product_barcode,

                        #endregion Display Fields

                        #region Hidden Fields

                        Ad_client_id = _product_ad_client_id,
                        Ad_org_id = _product_ad_org_id,
                        // Is_productMultiUOM = _is_productMultiUOM,
                        Product_Arabicname = _product_arabicname,
                        Product_category_id = _product_m_product_category_id,
                        Product_ID = __product_id,
                        //  Product_Image = _product_image,
                        Scanby_Price = _product_scanbyprice,
                        Scanby_Weight = _product_scanbyweight,
                        Current_costprice = _product_currentcostprice,
                        Sopricestd = _product_SellingPricestd,
                        Uom_Id = _product_uomid,
                        Uom_Name = _product_uomname

                        #endregion Hidden Fields
                    });
                }
                else
                {
                    bool result = false;
                    var item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode).FirstOrDefault());
                    if (txt_Price.Text != "")
                    {
                        result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00");
                        item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00").FirstOrDefault());
                    }
                    else
                    {
                        result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00");
                        item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00").FirstOrDefault());
                    }

                    if (result == true)
                    {

                        var _Quantity = items[item_index].Quantity + 1;
                        items[item_index].Quantity = Math.Round(Convert.ToDouble(_Quantity.ToString()), 2);
                        //items[item_index].Amount = Math.Round((Convert.ToDouble(_Quantity.ToString()) * items[item_index].Amount), 2);
                        items[item_index].Amount = (Math.Round(((Convert.ToDouble(items[item_index].Quantity)) * (Convert.ToDouble(items[item_index].Sopricestd))), 2)).ToString("0.00");
                        if (items[item_index].Percentpercentage_OR_Price == "%")
                        {
                            double Total_Price = Convert.ToDouble(items[item_index].Amount);
                            double Discount_Amt = items[item_index].Discount / 100 * Total_Price;
                            double Selling_Price = Total_Price - Discount_Amt;

                            items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                        }
                        else
                        {
                            double Total_Price = Convert.ToDouble(items[item_index].Amount);
                            double Discount_Amt = items[item_index].Discount;
                            double Selling_Price = Total_Price - Discount_Amt;

                            items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                        }
                        var item_reorder = items[item_index];
                        items.RemoveAt(item_index);
                        items.Insert(0, item_reorder);
                    }
                    else
                    {
                        double product_discount = 0;
                        double product_totalAmount = Convert.ToDouble(_product_SellingPricestd.ToString());


                        items.Insert(0, new Product()
                        {
                            #region Display Fields

                            Discount = product_discount,
                            Quantity = 1,
                            Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                            Amount = product_totalAmount.ToString("0.00"),
                            Percentpercentage_OR_Price = Percentage_OR_Price,
                            Product_Name = _product_name,
                            Iteam_Barcode = _product_barcode,

                            #endregion Display Fields

                            #region Hidden Fields

                            Ad_client_id = _product_ad_client_id,
                            Ad_org_id = _product_ad_org_id,
                            // Is_productMultiUOM = _is_productMultiUOM,
                            Product_Arabicname = _product_arabicname,
                            Product_category_id = _product_m_product_category_id,
                            Product_ID = __product_id,
                            //  Product_Image = _product_image,
                            Scanby_Price = _product_scanbyprice,
                            Scanby_Weight = _product_scanbyweight,
                            Current_costprice = _product_currentcostprice,
                            Sopricestd = _product_SellingPricestd,
                            Uom_Id = _product_uomid,
                            Uom_Name = _product_uomname

                            #endregion Hidden Fields
                        });
                    }
                }

                ProductIteams = items;
                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                view.Refresh();
                Grand_Total_cart_price.Text = String.Empty;
                addAmount = 0.00;
                foreach (var data in items)
                {
                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                }
                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                Grand_Cart_Total = addAmount;

                productSearch_cart.Text = String.Empty;
                #region Customer display 
                string SellingPrice = Convert.ToDouble(_product_SellingPricestd).ToString("0.00");
                string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                int TotalSpace = Convert.ToInt32(CustDispspace);
                string strproductname = SerialPort.Truncate(_product_name, 7);
                int productnamelen = strproductname.Length;
                int sellingPricelen = SellingPrice.Length;
                int totallen = "Total".Length;
                string strtotalprice = Grand_Cart_Total.ToString("0.00");
                int totalpricelen = strtotalprice.Length;

                int space1 = 0;
                if (productnamelen > sellingPricelen)
                {
                    space1 = TotalSpace - productnamelen - sellingPricelen;

                }
                int space2 = 0;
                if (totallen > totalpricelen)
                {
                    space2 = TotalSpace - totallen - totalpricelen;

                }



                //int space1 = TotalSpace - productnamelen - sellingPricelen;
                //int space2 = TotalSpace - totallen - totalpricelen;
                string strspace1 = new string(' ', space1);
                string strspace2 = new string(' ', space2);
                SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                #endregion Customer display 

                Product_Each_Item_Count = 0;
                items.ToList().ForEach(x =>
                {
                    Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                });
                Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();

                if (invoice_number == 0 && items.Count() > 0)
                {
                    #region Get Invoice & Doc No

                    //Checking and Getting Invoice POS Number
                    var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                    int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                    int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                    string _responce_code = Check_POS_Number_rs.Item3;
                    bool _network_status_ = Check_POS_Number_rs.Item4;
                    if (_responce_code == "0" || _responce_code == "200")
                    {
                        invoice_number = Check_POS_Number_rs.Item1;
                        document_no = Check_POS_Number_rs.Item2;
                        InvoiceNo.Text = invoice_number.ToString();
                    }
                    else if (_network_status_ != true || _responce_code == "500")
                    {
                        Error_page.Visibility = Visibility.Visible;
                        NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                        return;
                    }

                    #endregion Get Invoice & Doc No
                }
                iteamProduct.SelectedItem = items.FirstOrDefault();
                Keyboard.Focus(productSearch_cart);
                quick_ValueChanger_key_pad.Text = "0.00";
                Back_OR_Esc();
                MaintainActual_AMount();

                Keyboard.Focus(productSearch_cart);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }



        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            lstuomList.SelectedItems.Clear();

            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                lstuomList.SelectedItem = item;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {
                    Check_keyboard_Focus = "Add_Products_Add_Unit_GotFocus";
                    txtadddeleteunits.Visibility = Visibility.Hidden;
                    TextBlock txtprodid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtprodid") as TextBlock;
                    TextBlock txtbarCode = GetChildrenByType(selectedItem, typeof(TextBlock), "txtbarCode") as TextBlock;
                    TextBlock txtprodName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtprodName") as TextBlock;
                    TextBlock txtuomValue = GetChildrenByType(selectedItem, typeof(TextBlock), "txtuomValue") as TextBlock;
                    TextBlock txtPurchasePrice = GetChildrenByType(selectedItem, typeof(TextBlock), "txtunit2") as TextBlock;
                    TextBlock txtcostprice = GetChildrenByType(selectedItem, typeof(TextBlock), "txtcostprice") as TextBlock;
                    TextBlock txtsalesPrice = GetChildrenByType(selectedItem, typeof(TextBlock), "txtunit") as TextBlock;
                    TextBlock txtcategoryid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtcategoryid") as TextBlock;
                    TextBlock txtcategoryName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtcategoryName") as TextBlock;
                    TextBlock txtuomid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtuomid") as TextBlock;
                    TextBlock txtnoofpcs = GetChildrenByType(selectedItem, typeof(TextBlock), "txtnoofpcs") as TextBlock;
                    txtBarcode1.Text = txtbarCode.Text;
                    txtunitprodId.Text = txtprodid.Text;
                    txtName1.Text = txtprodName.Text;
                    if (txtcategoryid.Text != "")
                    {
                        ddlItem1.SelectedValue = txtcategoryid.Text;
                    }

                    if (ddlItem1.SelectedValue == null)
                    {
                        int indexT = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == txtcategoryName.Text);
                        if (indexT > -1)
                        {
                            ddlItem1.SelectedIndex = indexT;
                        }
                        else
                        {
                            ddlItem1.SelectedIndex = 1;
                        }


                    }
                    if (txtuomid.Text != "")
                    { txtUom1.SelectedValue = txtuomid.Text; }
                    if (txtUom1.SelectedValue == null)
                    {
                        int indexT = UomList_dataSource.FindIndex(r => r.uomName == txtuomValue.Text);
                        if (indexT > -1)
                        {
                            txtUom1.SelectedIndex = indexT;

                        }
                        else
                        {
                            txtUom1.SelectedValue = 100;
                        }


                    }
                    //if (ddlItem1.SelectedValue == null)
                    //{
                    //    int indexT = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == _categoryName);
                    //    ddlItem1.SelectedIndex = indexT;

                    //}

                    //if (txtUom1.SelectedValue == null)
                    //{
                    //    int indexT = UomList_dataSource.FindIndex(r => r.uomName == _productUOMValue);
                    //    txtUom1.SelectedIndex = indexT;

                    //}
                    txtNoofpieces.Text = txtnoofpcs.Text;
                    txtPurchasePrice1.Text = txtPurchasePrice.Text.Trim(new Char[] { '/' }).Trim();
                    txtCostPrice1.Text = txtcostprice.Text;
                    txtSalesPrice1.Text = txtsalesPrice.Text;

                    //NpgsqlConnection connection = new NpgsqlConnection(connstring);
                    //connection.Open();
                    //NpgsqlCommand cmd_select_no_of_pcs = new NpgsqlCommand("SELECT no_of_pcs FROM m_product_bom where m_product_id = '" + txtprodid.Text + "';", connection);
                    //NpgsqlDataReader _get__no_of_pcs = cmd_select_no_of_pcs.ExecuteReader();
                    //txtNoofpieces.Text = "";
                    //if (_get__no_of_pcs.Read())
                    //{
                    //    txtNoofpieces.Text = _get__no_of_pcs.GetString(0);
                    //}
                    //connection.Close();

                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Hidden;
                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Hidden;
                    btnaddprod.Visibility = Visibility.Hidden;
                    dock_save_cancel_btn.Visibility = Visibility.Hidden;
                    Keyboard.Focus(txtBarcode1);
                    txtBarcode1.SelectionStart = txtBarcode1.Text.Length;
                    if (lblprodBarcode_.Text != "")
                    {
                        Back_with_product.Text = lblprodBarcode_.Text;
                    }
                    if (txtBarcode.Text != "")
                    {
                        Back_with_product.Text = txtBarcode.Text;

                    }
                    // do stuff
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        //private void lstuomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ItemContainerGenerator generator = lstuomList.ItemContainerGenerator;
        //    ListBoxItem selectedItem = (ListBoxItem)generator.ContainerFromIndex(lstuomList.SelectedIndex);


        //}
        private void Add_New_Product_Window()
        {

            Check_keyboard_Focus = "Add_Products_NewEdit_GotFocus";
            txtunitdetails.Text = "Unit details (0)";
            lblprodid_.Text = "";
            Load_CategoryList(false);
            Load_uomList();
            txtName.Text = "";
            ddlitem.SelectedIndex = 0;
            txtUom.SelectedValue = 100;
            chkcase.IsChecked = false;
            txtadddeleteunits.Visibility = Visibility.Hidden;
            txtPurchasePrice.Text = "";
            txtSalesPrice.Text = "";
            txtMinimumQty.Text = "";
            txtMaximumQty.Text = "";
            chkPriceedit.IsChecked = false;
            chksellonline.IsChecked = false;
            lstuomList.ItemsSource = null;

            grdprodviewlist.Visibility = Visibility.Hidden;
            txtadddeleteunits.Visibility = Visibility.Visible;
            grdprodadd.Visibility = Visibility.Visible;
            quick_Side_Menu_Page_View_Unit_Content_Empty.Visibility = Visibility.Visible;
            Menu_Page_Product.Visibility = Visibility.Visible;
            __Side_quickMenu_Page.Visibility = Visibility.Hidden;
            POSGetProductApiJSONResponce_Bom = null;
            bind_unitlist();

            dock_save_cancel_btn.Visibility = Visibility.Visible;
            if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
            {
                txtBarcode.Text = txtProduct.Text;
                Keyboard.Focus(txtName);
            }
            else
            {
                txtBarcode.Text = "";
                Keyboard.Focus(txtBarcode);

            }



            txtName.SelectionStart = txtName.Text.Length;
            btnaddprod.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;

        }
        private void btnaddprod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_New_Product_Window();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void btncancel_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnunitcancel_Click(object sender, RoutedEventArgs e)
        {
            txtBarcode1.Text = "";
            Back_with_product.Text = "";
            txtName1.Text = "";
            txtUom1.SelectedValue = 100;
            txtPurchasePrice1.Text = "";
            txtCostPrice1.Text = "";
            txtSalesPrice1.Text = "";
            txtNoofpieces.Text = "";
            quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
            POSGetProductApiJSONResponce_Bom = null;
            bind_unitlist();
            quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
            //btnunitsave.Visibility = Visibility.Hidden;
            //btnunitcancel.Visibility = Visibility.Hidden;

            if (grdprodadd.Visibility == Visibility.Visible)
            {
                Check_keyboard_Focus = "Add_Products_NewEdit_GotFocus";
                dock_save_cancel_btn.Visibility = Visibility.Visible;
                btnaddprod.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;

            }
            else if (grdprodviewlist.Visibility == Visibility.Visible)
            {
                Check_keyboard_Focus = "Add_Products_View_GotFocus";
                dock_save_cancel_btn.Visibility = Visibility.Hidden;
                btnaddprod.Visibility = Visibility.Visible;
                btnEdit.Visibility = Visibility.Visible;
                txtadddeleteunits.Visibility = Visibility.Hidden;
            }

        }
        private void Edit_Product_Window()
        {
            Check_keyboard_Focus = "Add_Products_NewEdit_GotFocus";
            txtBarcode.Text = lblprodBarcode_.Text;
            txtName.Text = lblprodName_.Text;


            int indexT = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == lblprodItem_.Text);
            if (indexT > -1)
            {
                ddlitem.SelectedIndex = indexT;
            }
            else
            {
                ddlitem.SelectedIndex = 1;
            }

            if (lblprodUomid_.Text != "")
            { txtUom.SelectedValue = lblprodUomid_.Text; }

            if (txtUom.SelectedValue == null)
            {
                int UomindexT = UomList_dataSource.FindIndex(r => r.uomName == txtUom.Text);
                if (UomindexT > -1)
                {
                    txtUom.SelectedValue = UomindexT;
                }
                else
                {
                    txtUom.SelectedValue = 100;
                }


            }


            chkcase.IsChecked = chkprodcase.IsChecked;
            txtadddeleteunits.Visibility = Visibility.Visible;
            txtPurchasePrice.Text = lblprodPurchasePrice_.Text;
            txtCostPrice.Text = lblprodCostPrice_.Text;
            txtSalesPrice.Text = lblprodSalesPrice_.Text;
            txtMinimumQty.Text = lblprodMinimumQty_.Text;
            txtMaximumQty.Text = lblprodMaximumQty_.Text;
            chkPriceedit.IsChecked = chkprodPriceedit.IsChecked;
            chksellonline.IsChecked = chkprodsellonline.IsChecked;
            grdprodviewlist.Visibility = Visibility.Hidden;
            grdprodadd.Visibility = Visibility.Visible;
            quick_Side_Menu_Page_View_Unit_Content_Empty.Visibility = Visibility.Visible;
            Check_keyboard_Focus = "Add_Products_Add_Unit_GotFocus";
            bind_unitlist();
            dock_save_cancel_btn.Visibility = Visibility.Visible;
            btnaddprod.Visibility = Visibility.Hidden;
            btnEdit.Visibility = Visibility.Hidden;
            Keyboard.Focus(txtBarcode);
            txtBarcode.SelectionStart = txtBarcode.Text.Length;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Edit_Product_Window();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void btneditsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dock_save_cancel_btn.Visibility == Visibility.Visible)
                { Post_Product_Details(); }
                else
                {
                    MessageBox.Show("Product Cannot Post Until Not Complete and Save Unit Details.");
                    return;
                }

                if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                {
                    add_Product();
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void Post_Product_Details()
        {

            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            //NpgsqlCommand UPDATE_cmd_m_warehouse = new NpgsqlCommand("UPDATE m_warehouse SET attribute1 ='" + _itemCount + "' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = " + WarehouseId_Selected + "; ", connection);
            //UPDATE_cmd_m_warehouse.ExecuteNonQuery();
            //connection.Close();

            string _productName = "";
            string _productValue = "";
            string _categoryId = "";
            string _productArabicName = " ";
            string _scanbyWeight = "";
            string _scanbyPrice = "";
            string _ispriceEditable = "N";
            string categoryNAme = "";
            string _isquick = "N";

            string _productUOMId = "";
            string _productUOMValue = "";
            string _sellingPrice = "";
            string _costprice = "";
            string _purchasePrice = "";
            string _productMultiUOM = "";
            JArray _productMultiImage = new JArray();
            int img_count = 0;
            string _product_image = " ";
            string query = "";
            string _productId = "";

            if (grdprodadd.Visibility == Visibility.Visible)
            {
                if (txtBarcode.Text == "")
                {
                    MessageBox.Show("Barcode Can't Be Empty!");
                    Keyboard.Focus(txtBarcode);
                    return;
                }
                if (txtName.Text == "")
                {
                    MessageBox.Show("Product Name Can't Be Empty!");
                    Keyboard.Focus(txtName);
                    return;
                }
                if (ddlitem.SelectedIndex == 0 || ddlitem.SelectedValue.ToString() == "0")
                {
                    MessageBox.Show("Please Select Category!");
                    Keyboard.Focus(ddlitem);
                    return;

                }
                if (txtPurchasePrice.Text == "")
                {
                    MessageBox.Show("Purchase Price Can't Be Empty!");
                    Keyboard.Focus(txtPurchasePrice);
                    return;
                }
                if (txtCostPrice.Text == "")
                {
                    MessageBox.Show("Cost Price Can't Be Empty!");
                    Keyboard.Focus(txtCostPrice);
                    return;
                }

                if (txtSalesPrice.Text == "")
                {
                    MessageBox.Show("Sales Price Can't Be Empty!");
                    Keyboard.Focus(txtSalesPrice);
                    return;
                }

                _productName = txtName.Text;
                _productValue = txtBarcode.Text;
                _categoryId = ddlitem.SelectedValue.ToString();
                categoryNAme = ddlitem.Text;
                _productArabicName = " ";

                _scanbyWeight = "";
                _scanbyPrice = "";
                _ispriceEditable = "N";
                if (chkPriceedit.IsChecked == true)
                {
                    _ispriceEditable = "Y";
                }
                else
                {
                    _ispriceEditable = "N";
                }

                _isquick = "N";

                _productUOMId = txtUom.SelectedValue.ToString();
                _productUOMValue = txtUom.Text;
                _sellingPrice = txtSalesPrice.Text;
                _costprice = txtCostPrice.Text;
                _purchasePrice = txtPurchasePrice.Text;
                _productMultiUOM = "";
                if (img_count > 0)
                {
                    _product_image = _productMultiImage[0]["productImage"].ToString();
                }
               
                if (lblprodid_.Text != "")
                {
                    connection.Open();
                    NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("UPDATE  m_product SET m_product_category_id=@m_product_category_id,name=@name,searchkey=@searchkey, arabicname=@arabicname, uomid=@uomid, uomname=@uomname, sopricestd=@sopricestd, currentcostprice=@currentcostprice,ispriceeditable=@ispriceeditable,isquick=@isquick, attribute1=@attribute1,purchaseprice=@purchaseprice,sellonline=@sellonline where m_product_id=@m_product_id", connection);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(lblprodid_.Text));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                    INSERT_cmd_m_product.ExecuteNonQuery();
                    connection.Close();
                }
                else
                { 
                    connection.Open();
                    NpgsqlCommand cmd_check_barcode = new NpgsqlCommand("select searchkey from m_product where searchkey='" + txtBarcode.Text + "'  and ad_client_id=" + AD_Client_ID + " and ad_org_id=" + AD_ORG_ID + ";", connection);

                    NpgsqlDataReader _get_cmd_barcode = cmd_check_barcode.ExecuteReader();
                    if (_get_cmd_barcode.HasRows)
                    {
                        MessageBox.Show("Barcode value exist already");
                        //log.Info("All Complected Invoice Posted");
                        connection.Close();
                        return;
                    }
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand("select productId-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);

                    // Execute the query and obtain the value of the first column of the first row
                    Int32 productId = Convert.ToInt32(command.ExecuteScalar());
                    _productId = productId.ToString();
                    connection.Close();




                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                    if (_get__Ad_sequenc_no.Read())
                    {
                        Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                    }
                    connection.Close();



                    if (!check_product_exist(_productId))
                    {
                        connection.Open();
                        NpgsqlCommand cmd_update_pr_no_m_product_price = new NpgsqlCommand("UPDATE m_pos_sequence SET productId =" + _productId + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                        NpgsqlDataReader _update__pr_product_no = cmd_update_pr_no_m_product_price.ExecuteReader();
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                        NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                        connection.Close();
                        connection.Open();

                        NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,sellonline,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@sellonline,@created)", connection);

                        INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                        INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                        INSERT_cmd_m_product.ExecuteNonQuery();
                        connection.Close();


                        string _productId_PriceArray = _productId;
                        string _pricelistId_PriceArray = AD_PricelistID.ToString();
                        string _pricelistName_PriceArray = _sellingPrice;
                        string _priceStd_PriceArray = _costprice;

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                        if (_get__Ad_sequenc_no_m_product_price.Read())
                        {
                            Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                        }
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                        connection.Close();
                    }
                    else
                    {
                        log.Info("Product Already exist  ProductId:" + _productId);

                    }
                }
            }
            else
            {
                MessageBox.Show("Please Edit Main Product And Save");
                return;
            }

            uom_erp_dataSource.Clear();
            string parent_prod_id;
            if (lblprodid_.Text != "")
            {
                parent_prod_id = lblprodid_.Text;
                query = "select p.searchkey,p.name,c.name as categoryname, p.uomid,p.uomname,p.sopricestd as salesprice, " +
              " p.purchasePrice as purchasePrice,p.m_product_category_id,b.m_product_id,p.m_product_id,p.ispriceeditable," +
               "  p.currentcostprice,b.no_of_pcs from m_product p, m_product_bom b , m_product_category c where " +
                  " p.m_product_category_id = c.m_product_category_id  and p.m_product_id = b.m_product_id and " +
                  " b.m_parent_product_id =" + parent_prod_id;

                connection.Open();
                NpgsqlCommand cmd_fetch_uom = new NpgsqlCommand(query, connection);
                NpgsqlDataReader _get_c_uom = cmd_fetch_uom.ExecuteReader();
                while (_get_c_uom.Read())
                {
                    uom_erp_dataSource.Add(new Product_Uom_Erp
                    {
                        sellingPrice = _get_c_uom.GetDouble(5).ToString(),
                        categoryName = _get_c_uom.GetString(2),
                        costPrice = _get_c_uom.GetDouble(11).ToString(),
                        isPriceEditable = _get_c_uom.GetString(10),
                        categoryId = _get_c_uom.GetInt64(7).ToString(),
                        productValue = _get_c_uom.GetString(0),
                        uomName = _get_c_uom.GetString(4),
                        bomQty = _get_c_uom.GetInt32(12).ToString(),
                        uomId = _get_c_uom.GetInt64(3).ToString(),
                        productName = _get_c_uom.GetString(1),
                        purchasePrice = _get_c_uom.GetDouble(6).ToString(),
                        productId = _get_c_uom.GetInt64(8).ToString()
                    });

                }

            }
            else
            {
                parent_prod_id = _productId;
                if (POSGetProductApiJSONResponce_Bom != null)
                {
                    Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, parent_prod_id);


                    foreach (var productbomLst in POSGetProductApiJSONResponce_Bom)
                    {
                        uom_erp_dataSource.Add(new Product_Uom_Erp
                        {
                            sellingPrice = Convert.ToDecimal(productbomLst.sellingPrice).ToString("0.00"),
                            categoryName = productbomLst.categoryName,
                            costPrice = productbomLst.costPrice,
                            isPriceEditable = productbomLst.isPriceEditable,
                            categoryId = productbomLst.categoryId,
                            productValue = productbomLst.productValue,
                            uomName = productbomLst.categoryName,
                            bomQty = productbomLst.bomQty,
                            uomId = productbomLst.productUOMId,
                            productName = productbomLst.productName,
                            purchasePrice = Convert.ToDecimal(productbomLst.costPrice).ToString("0.00"),
                            productId = productbomLst.productId,
                        });


                    }
                    POSGetProductApiJSONResponce_Bom = null;
                }


            }

            connection.Close();
            string _IsBOMAvail = "";
            if (chkprodcase.IsChecked == true)
            {
                _IsBOMAvail = "Y";
            }
            else
            {
                _IsBOMAvail = "N";

            }

            JObject rss =
                  new JObject(

                      new JProperty("sessionId", AD_SessionID.ToString()),
                      new JProperty("costElementId", AD_CostelementID.ToString()),
                      new JProperty("showImage", "Y"),
                      new JProperty("macAddress", DeviceMacAdd),
                      new JProperty("businessPartnerId", AD_bpartner_Id.ToString()),
                      new JProperty("password", AD_UserPassword),
                      new JProperty("version", "1.1"),
                      new JProperty("orgId", AD_ORG_ID.ToString()),
                      new JProperty("username", AD_UserName),
                      new JProperty("userId", AD_USER_ID.ToString()),
                      new JProperty("roleId", AD_ROLE_ID.ToString()),
                      new JProperty("SyncedTime", 0),
                      new JProperty("clientId", AD_Client_ID.ToString()),
                      new JProperty("pricelistId", AD_PricelistID.ToString()),
                      new JProperty("warehouseId", AD_Warehouse_Id.ToString()),
                      new JProperty("accountSchemaId", AD_AccountSchemaid),
                      new JProperty("appName", "POS"),
                      new JProperty("remindMe", "N"),
                      new JProperty("operation", "POSAddProduct"),
                      new JProperty("productId", parent_prod_id),
                      new JProperty("productName", _productName),
                      new JProperty("productArabicName", _productArabicName),
                      new JProperty("productValue", _productValue),
                      new JProperty("categoryName", categoryNAme),
                      new JProperty("productCategoryId", _categoryId),
                      new JProperty("uomId", _productUOMId),
                      new JProperty("uomName", _productUOMValue),
                      new JProperty("sellingPrice", _sellingPrice),
                      new JProperty("purchasePrice", _purchasePrice),
                      new JProperty("costPrice", _costprice),
                      new JProperty("isPriceEditable", _ispriceEditable),
                      new JProperty("isBomAvailable", _IsBOMAvail),
                      new JProperty("bomDetails",
                            new JArray(
                                from p in uom_erp_dataSource
                                select new JObject(
                                    new JProperty("sellingPrice", p.sellingPrice),
                                    new JProperty("categoryName", p.categoryName),
                                    new JProperty("costPrice", p.costPrice),
                                    new JProperty("isPriceEditable", p.isPriceEditable),
                                    new JProperty("categoryId", p.categoryId),
                                    new JProperty("productValue", p.productValue),
                                    new JProperty("uomName", p.uomName),
                                    new JProperty("bomQty", p.bomQty),
                                    new JProperty("uomId", p.uomId),
                                    new JProperty("productName", p.productName),
                                    new JProperty("purchasePrice", p.purchasePrice),
                                    new JProperty("productId", p.productId)
                                                   )
                                        )

                                   ));
            var val = rss.ToString();
            int CheckApiError = 0;
            try
            {
                POSAddproductApiStringResponce = PostgreSQL.ApiCallPost(val);
                CheckApiError = 1;
            }
            catch
            {

                CheckApiError = 0;
                log.Error("POSAddProduct: Server Error");
                log.Error("----------------JSON Request--------------");
                log.Error(val);
                log.Error("----------------JSON END------------------");
            }
            if (CheckApiError == 1)
            {
                POSAddproductApiStringResponce = JsonConvert.DeserializeObject(POSAddproductApiStringResponce);
                log.Info("POSAddproductApiStringResponce: " + POSAddproductApiStringResponce + "");
                if (POSAddproductApiStringResponce.responseCode == "200")
                {
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_Update_post_flag_product_id = new NpgsqlCommand("UPDATE m_product " +
                       "SET m_product_id =  " + POSAddproductApiStringResponce.productId + ", updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'" +
                       "WHERE m_product_id = " + POSAddproductApiStringResponce.oldProductId + "; ", connection);
                    cmd_Update_post_flag_product_id.ExecuteReader();
                    connection.Close();
                    connection.Open();
                    cmd_Update_post_flag_product_id = new NpgsqlCommand("UPDATE m_product_price " +
                    "SET m_product_id =  " + POSAddproductApiStringResponce.productId +
                    "WHERE m_product_id = " + POSAddproductApiStringResponce.oldProductId + "; ", connection);
                    cmd_Update_post_flag_product_id.ExecuteReader();
                    connection.Close();
                    if (POSAddproductApiStringResponce.bomReturnDetails != null)
                    {
                        foreach (var productLst in POSAddproductApiStringResponce.bomReturnDetails)
                        {
                            connection.Open();
                            cmd_Update_post_flag_product_id = new NpgsqlCommand("UPDATE m_product " +
                             "SET m_product_id =  " + productLst.productId + ", updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'" +
                             "WHERE m_product_id = " + productLst.oldProductId + "; ", connection);
                            cmd_Update_post_flag_product_id.ExecuteReader();
                            connection.Close();
                            connection.Open();
                            cmd_Update_post_flag_product_id = new NpgsqlCommand("UPDATE m_product_price " +
                             "SET m_product_id =  " + productLst.productId + ", updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'" +
                             "WHERE m_product_id = " + productLst.oldProductId + "; ", connection);
                            cmd_Update_post_flag_product_id.ExecuteReader();
                            connection.Close();
                            connection.Open();
                            cmd_Update_post_flag_product_id = new NpgsqlCommand("UPDATE m_product_bom " +
                             "SET m_product_id =  " + productLst.productId + " , m_parent_product_id = " + POSAddproductApiStringResponce.productId +
                             "WHERE m_product_id = " + productLst.oldProductId + "; ", connection);
                            cmd_Update_post_flag_product_id.ExecuteReader();
                            connection.Close();
                        }
                    }
                    log.Info("Posted Add New Product  Flag Updated|#prodcutId: " + POSAddproductApiStringResponce.productId + "");
                }
                else
                {
                    connection.Close();
                    connection.Open();

                    MessageBox.Show("Problem in Posting Product Details!!");
                    log.Error("Posting Invoice Failed|Responce Code: " + POSAddproductApiStringResponce.responseCode);
                    log.Error("----------------JSON Request--------------");
                    log.Error(val);
                    log.Error("----------------JSON END--------------");
                }
            }


            fetch_all_products_details();

            //Bind_Product_Search();


            txtName.Text = "";
            ddlitem.SelectedIndex = 0;
            txtUom.SelectedValue = 100;
            chkcase.IsChecked = false;
            txtadddeleteunits.Visibility = Visibility.Hidden;
            txtPurchasePrice.Text = "";
            txtCostPrice.Text = "";
            txtSalesPrice.Text = "";
            txtMinimumQty.Text = "";
            txtMaximumQty.Text = "";
            chkPriceedit.IsChecked = false;
            chksellonline.IsChecked = false;
            lstuomList.ItemsSource = null;
            lstuomList.Items.Clear();
            grdprodviewlist.Visibility = Visibility.Visible;
            txtprodSearch.Text = "";
            ddlCategory.SelectedIndex = 0;
            if (quick_Check_windows_Focus == "")
            {
                txtBarcode.Text = "";

            }
            Check_keyboard_Focus = "Add_Products_View_GotFocus";
            grdprodadd.Visibility = Visibility.Hidden;
            dock_save_cancel_btn.Visibility = Visibility.Visible;
            Keyboard.Focus(txtprodSearch);
            txtprodSearch.Focus();
            Add_Products_List_Window();
            if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
            {
                add_Product();
            }
            //   unLoad_Process();
        }
        private void btneditcancel_Click(object sender, RoutedEventArgs e)
        {
            if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
            {
                __Side_quickMenu_Page.Visibility = Visibility.Visible;
                Menu_Page_Product.Visibility = Visibility.Hidden;
                Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                load_Quick_Products_Btn();
                Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                return;
            }
            txtBarcode.Text = "";
            txtName.Text = "";
            ddlitem.SelectedIndex = 0;
            txtUom.SelectedValue = 100;
            chkcase.IsChecked = false;
            txtadddeleteunits.Visibility = Visibility.Hidden;
            txtPurchasePrice.Text = "";
            txtSalesPrice.Text = "";
            txtMinimumQty.Text = "";
            txtMaximumQty.Text = "";
            chkPriceedit.IsChecked = false;
            chksellonline.IsChecked = false;
            grdprodviewlist.Visibility = Visibility.Visible;
            txtprodSearch.Text = "";
            ddlCategory.SelectedIndex = 0;
            fetch_all_products_details();
            Check_keyboard_Focus = "Add_Products_View_GotFocus";
            Keyboard.Focus(txtprodSearch);
            lstuomList.ItemsSource = null;
            lstuomList.Items.Clear();
            grdprodadd.Visibility = Visibility.Hidden;
            dock_save_cancel_btn.Visibility = Visibility.Hidden;
            btnaddprod.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Visible;
        }
        private bool check_product_exist(string product_id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            connection.Open();
            NpgsqlCommand cmd_check_prodid = new NpgsqlCommand("select m_product_Id  from m_product  where  m_product_Id='" + product_id + "'; ", connection);

            NpgsqlDataReader _get_cmd_prodid = cmd_check_prodid.ExecuteReader();
            if (_get_cmd_prodid.Read() && _get_cmd_prodid.HasRows == true)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;

            }
        }
        private bool check_customer_exist(string customer_id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            connection.Open();

            string query = "SELECT c_bpartner_id FROM  c_bpartner where  ad_client_id =" + AD_Client_ID + " AND  ad_org_id =" + AD_ORG_ID + " and c_bpartner_id = " + txtaddcrnumber_.Text;
            NpgsqlCommand cmd_check_custid = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_cmd_custid = cmd_check_custid.ExecuteReader();
            if (_get_cmd_custid.Read() && _get_cmd_custid.HasRows == true)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;

            }
        }
        private bool check_category_exist(string categoryname)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            connection.Open();

            string query = "SELECT name FROM  m_product_category where  ad_client_id =" + AD_Client_ID + " AND  ad_org_id =" + AD_ORG_ID + " and lower(name) = '" + categoryname.ToLower() + "'";
            NpgsqlCommand cmd_check_categoryid = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_cmd_categoryid = cmd_check_categoryid.ExecuteReader();
            if (_get_cmd_categoryid.Read() && _get_cmd_categoryid.HasRows == true)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;

            }
        }
        private void Open_New_Unit()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);

                //NpgsqlCommand UPDATE_cmd_m_warehouse = new NpgsqlCommand("UPDATE m_warehouse SET attribute1 ='" + _itemCount + "' WHERE ad_client_id = " + AD_Client_ID + "  and ad_org_id = " + AD_ORG_ID + " and m_warehouse_id = " + WarehouseId_Selected + "; ", connection);
                //UPDATE_cmd_m_warehouse.ExecuteNonQuery();
                //connection.Close();

                string _productName = "";
                string _productValue = "";
                string _categoryId = "";
                string _productArabicName = " ";
                string _scanbyWeight = "";
                string _scanbyPrice = "";
                string _ispriceEditable = "N";
                string categoryNAme = "";
                string _isquick = "N";

                string _productUOMId = "";
                string _productUOMValue = "";
                string _sellingPrice = "";
                string _costprice = "";
                string _purchasePrice = "";
                string _productMultiUOM = "";
                JArray _productMultiImage = new JArray();
                int img_count = 0;
                string _product_image = " ";

                if (lblprodid_.Text == "")
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand("select productId-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);

                    // Execute the query and obtain the value of the first column of the first row
                    Int32 productId = Convert.ToInt32(command.ExecuteScalar());
                    string _productId = productId.ToString();

                    lblprodid_.Text = _productId;
                    connection.Close();
                }
                if (grdprodadd.Visibility == Visibility.Visible)
                {
                    if (txtBarcode.Text == "")
                    {
                        MessageBox.Show("Product's Barcode Can't Be Empty!");
                        Keyboard.Focus(txtBarcode);
                        return;
                    }
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Product's Name Can't Be Empty!");
                        Keyboard.Focus(txtName);
                        return;
                    }
                    if (ddlitem.SelectedIndex == 0 || ddlitem.SelectedValue.ToString() == "0")
                    {
                        MessageBox.Show("Please Select Category of Product!");
                        Keyboard.Focus(ddlitem);
                        return;

                    }
                    if (txtPurchasePrice.Text == "")
                    {
                        MessageBox.Show("Product's Purchase Price Can't Be Empty!");
                        Keyboard.Focus(txtPurchasePrice);
                        return;
                    }
                    if (txtCostPrice.Text == "")
                    {
                        MessageBox.Show("Product's Cost Price Can't Be Empty!");
                        Keyboard.Focus(txtCostPrice);
                        return;
                    }
                    if (txtSalesPrice.Text == "")
                    {
                        MessageBox.Show("Product'sSales Price Can't Be Empty!");
                        Keyboard.Focus(txtSalesPrice);
                        return;
                    }
                    _productName = txtName.Text;
                    _productValue = txtBarcode.Text;
                    _categoryId = ddlitem.SelectedValue.ToString();
                    categoryNAme = ddlitem.Text;
                    _productArabicName = " ";

                    _scanbyWeight = "";
                    _scanbyPrice = "";
                    _ispriceEditable = "N";
                    chkcase.IsChecked = true;
                    if (chkPriceedit.IsChecked == true)
                    {
                        _ispriceEditable = "Y";
                    }
                    else
                    {
                        _ispriceEditable = "N";
                    }

                    _isquick = "N";

                    _productUOMId = txtUom.SelectedValue.ToString();
                    _productUOMValue = txtUom.Text;
                    _sellingPrice = txtSalesPrice.Text;
                    _costprice = txtCostPrice.Text;
                    _purchasePrice = txtPurchasePrice.Text;
                    _productMultiUOM = "";
                    if (img_count > 0)
                    {
                        _product_image = _productMultiImage[0]["productImage"].ToString();
                    }
                    //Boolean prodexist = false;
                    //if (Convert.ToInt32(lblprodid_.Text) < 0)
                    //{
                    //    connection.Open();
                    //    NpgsqlCommand cmd_check_prodid = new NpgsqlCommand("select m_product_Id  from m_product  where  m_product_Id='" + lblprodid_.Text + "'; ", connection);

                    //    NpgsqlDataReader _get_cmd_prodid = cmd_check_prodid.ExecuteReader();
                    //    if (_get_cmd_prodid.HasRows)
                    //    {
                    //        prodexist = true;
                    //    }
                    //    connection.Close();
                    //}
                    //else
                    //{
                    //    prodexist = true;
                    //}
                    if (!check_product_exist(lblprodid_.Text))
                    {
                        connection.Open();
                        NpgsqlCommand cmd_check_barcode = new NpgsqlCommand("select searchkey from m_product where searchkey='" + txtBarcode.Text + "' and ad_client_id=" + AD_Client_ID + " and ad_org_id=" + AD_ORG_ID + ";", connection);

                        NpgsqlDataReader _get_cmd_barcode = cmd_check_barcode.ExecuteReader();
                        if (_get_cmd_barcode.HasRows)
                        {
                            MessageBox.Show("Barcode value exist already");
                            //log.Info("All Complected Invoice Posted");
                            connection.Close();
                            return;
                        }
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                        NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                        if (_get__Ad_sequenc_no.Read())
                        {
                            Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                        }
                        connection.Close();
                        if (!check_product_exist(lblprodid_.Text))
                        {
                            connection.Open();
                            NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE m_pos_sequence SET productId =" + lblprodid_.Text + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                            NpgsqlDataReader _update__no_sequenc_no = cmd_update_sequenc_no_m_product_price.ExecuteReader();
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                            NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                            connection.Close();

                            connection.Open();

                            NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,sellonline,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@sellonline,@created)", connection);

                            INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(lblprodid_.Text));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                            INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                            INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                            INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                            INSERT_cmd_m_product.ExecuteNonQuery();
                            connection.Close();


                            string _productId_PriceArray = lblprodid_.Text;
                            string _pricelistId_PriceArray = AD_PricelistID.ToString();
                            string _pricelistName_PriceArray = _sellingPrice;
                            string _priceStd_PriceArray = _costprice;

                            connection.Close();

                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                            if (_get__Ad_sequenc_no_m_product_price.Read())
                            {
                                Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                            }
                            connection.Close();

                            connection.Open();
                            cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                            cmd_update_sequenc_no_m_product_price.ExecuteReader();
                            connection.Close();

                            connection.Open();
                            NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                    "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                            INSERT_cmd_m_product_price.ExecuteNonQuery();
                            connection.Close();


                            if (_productMultiUOM == "Y")
                            {

                                string _uomId_UOMArray = _productUOMId;
                                string _uomValue_UOMArray = _productUOMValue;
                                string _uomConvRate_UOMArray = _productUOMValue;

                                connection.Close();

                                connection.Open();
                                cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                if (_get__Ad_sequenc_no_m_product_price.Read())
                                {
                                    Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                }
                                connection.Close();

                                connection.Open();
                                INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                              " VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + lblprodid_.Text + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                INSERT_cmd_m_product_price.ExecuteNonQuery();
                                connection.Close();

                                connection.Open();
                                cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                connection.Close();
                            }
                        }
                        else
                        {
                            log.Info("Product Already exist  ProductId:" + lblprodid_.Text);

                        }
                        if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                        {

                            if (POSGetProductApiJSONResponce_Bom != null)
                            {

                                Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, lblprodid_.Text);

                                POSGetProductApiJSONResponce_Bom = null;
                            }
                        }

                    }
                    else
                    {
                        connection.Open();

                        NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("UPDATE  m_product SET m_product_category_id=@m_product_category_id,name=@name,searchkey=@searchkey, arabicname=@arabicname, uomid=@uomid, uomname=@uomname, sopricestd=@sopricestd, currentcostprice=@currentcostprice,ispriceeditable=@ispriceeditable,isquick=@isquick, attribute1=@attribute1,purchaseprice=@purchaseprice,sellonline=@sellonline where m_product_id=@m_product_id", connection);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(lblprodid_.Text));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                        INSERT_cmd_m_product.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please Edit to add units");
                    return;
                }
                txtunitprodId.Text = "";
                txtBarcode1.Text = "";

                txtName1.Text = "";
                txtNoofpieces.Text = "";
                ddlItem1.SelectedIndex = ddlitem.SelectedIndex;

                txtUom1.SelectedValue = 100;

                txtPurchasePrice1.Text = "";
                txtCostPrice1.Text = "";
                txtSalesPrice1.Text = "";
                quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Hidden;
                quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Visible;
                Check_keyboard_Focus = "Add_Products_Add_Unit_GotFocus";
                if (lblprodBarcode_.Text != "")
                {
                    Back_with_product.Text = lblprodBarcode_.Text;
                }
                if (txtBarcode.Text != "")
                {
                    Back_with_product.Text = txtBarcode.Text;

                }

                // Bind_Product_Search();
                btnEdit.Visibility = Visibility.Hidden;
                btnaddprod.Visibility = Visibility.Hidden;
                dock_save_cancel_btn.Visibility = Visibility.Hidden;
                Keyboard.Focus(txtBarcode1);
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                MessageBox.Show(ex.ToString());
            }

        }
        private void BackTOCart_Back_with_product_Click(object sender, RoutedEventArgs e)
        {
            Back_From_Edit_Unit();


        }
        private void Back_From_Edit_Unit()
        {
            if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus")
            {


                quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                bind_unitlist();
                Check_keyboard_Focus = "Add_Products_View_GotFocus";
            }
            else
            {

                quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                bind_unitlist();
                Check_keyboard_Focus = "Add_Products_View_GotFocus";
            }
        }
        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Back_From_Edit_Unit();
            //btnunitsave.Visibility = Visibility.Hidden;
            // btnunitcancel.Visibility = Visibility.Hidden; 

        }

        private void unitsaveclick_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Save_Unit_Details();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void Save_Unit_Details()
        {
            try
            {


                if (txtBarcode1.Text == "")
                {
                    MessageBox.Show("Barcode Can't Be Empty!");
                    Keyboard.Focus(txtBarcode1);
                    return;
                }
                if (txtName1.Text == "")
                {
                    MessageBox.Show("Product Name Can't Be Empty!");
                    Keyboard.Focus(txtName1);
                    return;
                }
                if (ddlItem1.SelectedIndex == 0 || ddlItem1.SelectedValue.ToString() == "0")
                {
                    MessageBox.Show("Please Select Category!");
                    Keyboard.Focus(ddlItem1);
                    return;

                }
                if (txtNoofpieces.Text == "")
                {
                    MessageBox.Show("No Of Pieces Can't Be Empty!");
                    Keyboard.Focus(txtNoofpieces);
                    return;
                }
                if (txtPurchasePrice1.Text == "")
                {
                    MessageBox.Show("Purchase Price Can't Be Empty!");
                    Keyboard.Focus(txtPurchasePrice1);
                    return;
                }
                if (txtCostPrice1.Text == "")
                {
                    MessageBox.Show("Cost Price Can't Be Empty!");
                    Keyboard.Focus(txtCostPrice1);
                    return;
                }
                if (txtSalesPrice1.Text == "")
                {
                    MessageBox.Show("Sales Price Can't Be Empty!");
                    Keyboard.Focus(txtSalesPrice1);
                    return;
                }
                if (POSGetProductApiJSONResponce_Bom != null)
                {
                    Open_New_Unit();

                    Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, lblprodid_.Text);

                    POSGetProductApiJSONResponce_Bom = null;
                }
                //   load_Process();
                NpgsqlConnection connection = new NpgsqlConnection(connstring);

                string _productName = txtName1.Text;
                string _productValue = txtBarcode1.Text;
                string _categoryId = ddlItem1.SelectedValue.ToString();
                string _productArabicName = " ";
                string _scanbyWeight = "";
                string _scanbyPrice = "";
                string _ispriceEditable = "N";
                //if (chkprodPriceedit.IsChecked == true)
                //{ _ispriceEditable = "Y"; }
                //else
                //{ _ispriceEditable = "N"; }

                string _isquick = "N";

                string _productUOMId = txtUom1.SelectedValue.ToString();
                string _productUOMValue = txtUom1.Text;
                string _sellingPrice = txtSalesPrice1.Text;
                string _costprice = txtCostPrice1.Text;
                string _purchasePrice = txtPurchasePrice1.Text;
                int _no_of_pcs = Convert.ToInt32(txtNoofpieces.Text);
                string _productMultiUOM = "";
                JArray _productMultiImage = new JArray();
                int img_count = 0;
                string _product_image = " ";
                if (img_count > 0)
                {

                    _product_image = _productMultiImage[0]["productImage"].ToString();
                }

                Boolean prodexist = false;
                if (txtunitprodId.Text != "")
                {
                    if (Convert.ToInt64(txtunitprodId.Text) < 0)
                    {
                        connection.Open();
                        NpgsqlCommand cmd_check_prodid = new NpgsqlCommand("select m_product_Id  from m_product  where  m_product_Id='" + txtunitprodId.Text + "'; ", connection);

                        NpgsqlDataReader _get_cmd_prodid = cmd_check_prodid.ExecuteReader();
                        if (_get_cmd_prodid.HasRows)
                        {
                            prodexist = true;
                        }
                        connection.Close();
                    }
                    else
                    {
                        prodexist = true;
                    }
                }

                if (prodexist == false)
                {

                    connection.Open();
                    NpgsqlCommand cmd_check_barcode = new NpgsqlCommand("select searchkey from m_product where searchkey='" + _productValue + "'  and ad_client_id=" + AD_Client_ID + " and ad_org_id=" + AD_ORG_ID + ";", connection);

                    NpgsqlDataReader _get_cmd_barcode = cmd_check_barcode.ExecuteReader();
                    if (_get_cmd_barcode.HasRows)
                    {
                        MessageBox.Show("Barcode value exist already");
                        //log.Info("All Complected Invoice Posted");
                        connection.Close();
                        // unLoad_Process();
                        return;
                    }
                    connection.Close();
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand("select productId-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);
                    // Execute the query and obtain the value of the first column of the first row
                    Int32 productId = Convert.ToInt32(command.ExecuteScalar());
                    string _productId = productId.ToString();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                    if (_get__Ad_sequenc_no.Read())
                    {
                        Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                    }
                    connection.Close();

                    if (!check_product_exist(_productId))
                    {
                        connection.Open();
                        NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("UPDATE m_pos_sequence SET productId =" + _productId + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                        NpgsqlDataReader _update__pr_sequenc_no = INSERT_cmd_m_product.ExecuteReader();
                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                        connection.Close();

                        connection.Open();

                        INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,sellonline,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@sellonline,@created)", connection);

                        INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                        INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                        INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                        INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                        INSERT_cmd_m_product.ExecuteNonQuery();
                        connection.Close();




                        connection.Open();
                        NpgsqlCommand INSERT_cmd_m_product_Bom = new NpgsqlCommand("INSERT INTO m_product_bom(ad_client_id, ad_org_id, m_product_id, no_of_pcs, m_parent_product_id)VALUES(@ad_client_id,@ad_org_id,@m_product_id, @no_of_pcs, @m_parent_product_id)", connection);

                        INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                        INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                        INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@m_product_id", Convert.ToInt64(_productId));
                        INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@no_of_pcs", _no_of_pcs);
                        INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@m_parent_product_id", Convert.ToInt64(lblprodid_.Text));
                        INSERT_cmd_m_product_Bom.ExecuteNonQuery();
                        connection.Close();
                        if (txtunitprodIdbefadd.Text == "")
                        {
                            txtunitprodIdbefadd.Text = _productId;
                        }
                        else
                        {
                            txtunitprodIdbefadd.Text += "," + _productId;
                        }



                        string _productId_PriceArray = _productId;
                        string _pricelistId_PriceArray = AD_PricelistID.ToString();
                        string _pricelistName_PriceArray = _sellingPrice;
                        string _priceStd_PriceArray = _costprice;

                        connection.Close();

                        connection.Open();
                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                        if (_get__Ad_sequenc_no_m_product_price.Read())
                        {
                            Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                        }
                        connection.Close();
                        connection.Open();
                        cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                        connection.Close();





                        string _uomId_UOMArray = _productUOMId;
                        string _uomValue_UOMArray = _productUOMValue;
                        int _uomConvRate_UOMArray = 1;

                        connection.Close();

                        connection.Open();
                        cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                        _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                        if (_get__Ad_sequenc_no_m_product_price.Read())
                        {
                            Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                        }
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand cmd_update_sequenc_no_m_product_uom = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_uom';", connection);
                        cmd_update_sequenc_no_m_product_uom.ExecuteReader();
                        connection.Close();
                        connection.Open();
                        NpgsqlCommand INSERT_cmd_m_product_uom = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                          " VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                        INSERT_cmd_m_product_uom.ExecuteNonQuery();
                        connection.Close();


                    }
                    else
                    {
                        log.Info("Product Already exist  ProductId:" + _productId);

                    }
                }
                else
                {
                    connection.Open();

                    NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("UPDATE  m_product SET m_product_category_id=@m_product_category_id,name=@name,searchkey=@searchkey, arabicname=@arabicname, uomid=@uomid, uomname=@uomname, sopricestd=@sopricestd, currentcostprice=@currentcostprice,ispriceeditable=@ispriceeditable,isquick=@isquick, attribute1=@attribute1,purchaseprice=@purchaseprice,sellonline=@sellonline where m_product_id=@m_product_id", connection);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(txtunitprodId.Text));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                    INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                    INSERT_cmd_m_product.Parameters.AddWithValue("@sellonline", 'N');
                    INSERT_cmd_m_product.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_update_uom = new NpgsqlCommand("UPDATE m_product_uom SET uomid =" + Convert.ToInt32(_productUOMId) + " WHERE m_product_id=" + Convert.ToInt32(txtunitprodId.Text) + ";", connection);
                    NpgsqlDataReader _get_cmd_uom = cmd_update_uom.ExecuteReader();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand cmd_update_no_of_pcs = new NpgsqlCommand("UPDATE m_product_bom SET no_of_pcs =" + _no_of_pcs + " WHERE m_product_id=" + Convert.ToInt32(txtunitprodId.Text) + ";", connection);
                    NpgsqlDataReader _get_no_of_pcs = cmd_update_no_of_pcs.ExecuteReader();
                    connection.Close();

                }

                connection.Close();
                bind_unitlist();
                //Bind_Product_Search();

                txtBarcode1.Text = "";
                Back_with_product.Text = "";
                txtName1.Text = "";
                txtUom1.SelectedValue = 100;
                txtPurchasePrice1.Text = "";
                txtCostPrice1.Text = "";
                txtSalesPrice1.Text = "";
                txtNoofpieces.Text = "";

                if (grdprodadd.Visibility == Visibility.Visible)
                {
                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                    bind_unitlist();
                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                    grdprodadd.Visibility = Visibility.Visible;
                    Check_keyboard_Focus = "Add_Products_NewEdit_GotFocus";
                    dock_save_cancel_btn.Visibility = Visibility.Visible;
                    btnaddprod.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Hidden;
                }
                else
                {
                    //  grdprodadd.Visibility = Visibility.Hidden;
                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                    Check_keyboard_Focus = "Add_Products_View_GotFocus";
                    dock_save_cancel_btn.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    btnaddprod.Visibility = Visibility.Visible;


                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                    //btnunitsave.Visibility = Visibility.Hidden;
                    //  btnunitcancel.Visibility = Visibility.Hidden;

                }
                fetch_all_products_details();
                btneditsave.Focus();
                Keyboard.Focus(btneditsave);
                // unLoad_Process();
            }
            catch (Exception ex)
            {
                //unLoad_Process();
                CrashApp_Alert();
                MessageBox.Show(ex.ToString());
            }
        }
        private void Prod_Barcode_gen_Click(object sender, RoutedEventArgs e)
        {
            txtBarcode.Text = DateTime.Now.ToString("ddmmyyyyhhmmss");
            txtName.Focus();
        }

        private void Unit_Barcode_gen_Click(object sender, RoutedEventArgs e)
        {
            txtBarcode1.Text = DateTime.Now.ToString("ddmmyyyyhhmmss");
            txtName1.Focus();
        }

        private void chkcase_Click(object sender, RoutedEventArgs e)
        {
            if (chkcase.IsChecked == true)
            {
                Check_keyboard_Focus = "Add_Unit_KeyBoard_Got_Focus";
                txtadddeleteunits.Visibility = Visibility.Visible;
            }
            else
            {
                txtadddeleteunits.Visibility = Visibility.Hidden;
            }
        }
        private void Add_Prod_From_Search()
        {
            if (lstProdSearch.Items.Count == 0)
            {
                lstsearchprodid = 0;
            }
            if (lstsearchprodid != 0)
            {
                // product_Search_Popup.IsOpen = false;

                SessionResume.Visibility = Visibility.Hidden;
                SessionClose.Visibility = Visibility.Hidden;
                SessionCreateNew.Visibility = Visibility.Hidden;
                txt_openingBal.Visibility = Visibility.Hidden;
                txt_Price.Visibility = Visibility.Hidden;
                Session_Check.Visibility = Visibility.Hidden;
                Error_page.Visibility = Visibility.Hidden;
                SessionChangePrice.Visibility = Visibility.Hidden;
                add_Product();
                txt_Price.Text = "";
                lstsearchprodid = 0;
            }
            else
            {
                ////string Checkpriceeditable = "SELECT coalesce(m_product.ispriceeditable::varchar(10), 'N') ispriceeditable,m_product.sopricestd ,m_product.isquick " +
                ////" FROM m_product, m_product_price, ad_sys_config" +
                ////" WHERE m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.searchkey = @_searchkey" +
                ////" AND m_product.m_product_id = m_product_price.m_product_id  AND m_product_price.pricelistid = " + AD_PricelistID + ";";

                ////NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                ////connection.Open();
                ////NpgsqlCommand cmd_clientId_Read = new NpgsqlCommand(Checkpriceeditable, connection);
                ////if (productSearch_cart.Text.Contains('-'))
                ////{
                ////    searchkey_Short = productSearch_cart.Text.Split('-')[0];
                ////    if (searchkey_Short == "")
                ////    {
                ////        MessageBox.Show("  Invalid Product Search");
                ////        Keyboard.Focus(productSearch_cart);
                ////        return;
                ////    }
                ////    try
                ////    {
                ////        price_short = Convert.ToDouble(productSearch_cart.Text.Trim().Split('-')[1]);
                ////        cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", searchkey_Short);
                ////    }
                ////    catch
                ////    {
                ////        MessageBox.Show("  Invalid Product Search");
                ////        Keyboard.Focus(productSearch_cart);
                ////        return;
                ////    }
                ////}
                ////else
                ////{

                ////    cmd_clientId_Read.Parameters.AddWithValue("@_searchkey", productSearch_cart.Text);
                ////}

                ////NpgsqlDataReader _clientId_Read = cmd_clientId_Read.ExecuteReader();
                ////if (_clientId_Read.Read())
                ////{
                ////    _isPriceeditable = _clientId_Read.GetString(0);
                ////    _pricestd = _clientId_Read.GetInt64(1);
                ////    _isQuickprod = _clientId_Read.GetString(2);
                ////}
            }

        }
        private void load_Process()
        {
            RetailProgressBar.Visibility = Visibility.Visible;
            RetailGrid.Opacity = 50;
            RetailGrid.IsEnabled = false;
        }
        private void unLoad_Process()
        {
            RetailProgressBar.Visibility = Visibility.Hidden;
            RetailGrid.Opacity = 100;
            RetailGrid.IsEnabled = true;
        }
        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Menu_Page_Category.Visibility == Visibility.Visible)
                {
                    if (e.SystemKey == Key.F10)
                    {
                        Btncategorysave_Click(sender, e);
                        return;
                    }
                    if (grdCategoryviewlist.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F3)
                        {
                            BtnEdit_Category_Click(sender, e);
                            return;
                        }
                        if (e.Key == Key.F2)
                        {
                            Btnaddcategory_Click(sender, e);
                            return;
                        }

                    }
                    if (e.Key == Key.Escape)
                    {
                        if (grdCategoryviewlist.Visibility == Visibility.Visible)
                        {
                            Back_OR_Esc();
                        }
                        else
                        {
                            View_Category_List_Page();
                        }
                        return;
                    }
                }
                if (Menu_Page_Customer.Visibility == Visibility.Visible)
                {
                    if (e.SystemKey == Key.F10)
                    {
                        Btncustsave_Click(sender, e);
                        return;
                    }
                    if (grdcustomerviewlist.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F3)
                        {
                            BtnEdit_cust_Click(sender, e);
                            return;
                        }
                        if (e.Key == Key.F2)
                        {
                            Btnaddcust_Click(sender, e);
                            return;
                        }

                    }
                    if (e.Key == Key.Escape)
                    {
                        if (grdcustomerviewlist.Visibility == Visibility.Visible)
                        {
                            Back_OR_Esc();
                        }
                        else
                        {
                            View_Customer_List_Page();
                        }
                        return;
                    }
                }
                if (popupCustomerList.IsOpen)
                {
                    if (e.Key == Key.Escape)
                    {
                        popupCustomerList.IsOpen = false;
                        MainPage.IsEnabled = true;
                    }
                }
                if (__Side_Menu_POS_Setting_Page.Visibility == Visibility.Visible)
                {
                    if (e.SystemKey == Key.F10)
                    {
                        btnsettingsave_Click(sender, e);
                        return;
                    }

                    if (e.Key == Key.Escape)
                    {

                        Back_OR_Esc();
                        return;
                    }
                }

                if (print_Barcode_Popup.IsOpen)
                {
                    if (e.Key == Key.Escape)
                    {
                        print_Barcode_Popup.IsOpen = false;
                        txtNoofBarcodePrint.Text = String.Empty;
                        return;

                    }

                }
                if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                {
                    if (Error_page.Visibility == Visibility.Visible && txt_openingBal.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.Escape || e.Key == Key.Enter)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                    if (product_Search_Popup.IsOpen == true)
                    {
                        if (e.Key == Key.Escape || e.Key == Key.Enter)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                    if (Error_page.Visibility == Visibility.Visible && txt_openingBal.Visibility == Visibility.Hidden)
                    {
                        if (e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.F4 || e.Key == Key.F5 || e.Key == Key.Y || e.Key == Key.N || e.Key == Key.F1 || e.Key == Key.F2)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                }
                if (Check_keyboard_Focus == "Manual_Sync_GotFocus")
                {
                    if (e.Key == Key.Y)
                    {
                        Side_Menu_Page_Manual_Sync_Yes_Click(sender, e);
                        return;
                    }
                    if (e.Key == Key.N)
                    {
                        Side_Menu_Page_Manual_Sync_No_Click(sender, e);
                        return;
                    }
                    if (Side_Menu_Page_Manual_Sync_Get_All_Data.Visibility == Visibility.Visible && Side_Menu_Page_Manual_Sync_Get_Updated_Data.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.A)
                        {
                            Side_Menu_Page_Manual_Sync_Get_All_Data_Click(sender, e);
                            return;
                        }
                        if (e.Key == Key.U)
                        {
                            Side_Menu_Page_Manual_Sync_Get_Updated_Data_Click(sender, e);
                            return;
                        }

                    }

                }
                if (paymentPopup.IsOpen == true)
                {
                    if (e.Key == Key.F2)
                    {
                        Button_Click_3(sender, e);
                        return;
                    }

                }
                if (product_Search_Popup.IsOpen == true)
                {

                    if (e.Key == Key.Escape)
                    {
                        lstProdSearch.ItemsSource = null;
                        txtprodSearch.Text = string.Empty;
                        product_Search_Popup.IsOpen = false;
                        MainPage.IsEnabled = true;
                        Back_OR_Esc();
                        return;
                    }
                    if (e.Key == Key.Enter)
                    {
                        Add_Prod_From_Search();
                        return;

                    }
                    if (e.Key == Key.F6)
                    {
                        txtProdSearch.Focus();
                        Keyboard.Focus(txtProdSearch);
                        return;
                    }
                }
                if (Check_keyboard_Focus == "Cart_Hold_Check_keyboard_Focus")
                {
                    if (e.Key == Key.F2)
                    {
                        New_Sale_hold();
                        return;
                    }
                }
                if (Check_keyboard_Focus == "Session_Close_Pannel")
                {
                    if (e.Key == Key.F5)
                    {
                        Session_Close();
                        return;

                    }

                }
                if (Check_keyboard_Focus == "ValueChanger_key_pad_GotFocus")
                {
                    if (LineIteam_Up_Rf == "Quantity_Cart_LineIteam" || LineIteam_Up_Rf == "Discount_Cart_LineIteam" || LineIteam_Up_Rf == "OverAllDiscount")
                    {
                        if (LineIteam_Up_Rf == "Discount_Cart_LineIteam" || LineIteam_Up_Rf == "OverAllDiscount")
                        {
                            if (e.Key == Key.P)
                            {
                                try
                                {
                                    ToggleButton_for_DisAndPrice.Visibility = Visibility.Visible;
                                    ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                                    OverAllDiscount_sy.Text = "%";

                                    ValueChanger_key_pad.Focusable = true;
                                    Keyboard.Focus(ValueChanger_key_pad);
                                    Percentage_OR_Price = "%";
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    CrashApp_Alert();
                                    log.Error(" ===================  Error In Retail POS  =========================== ");
                                    log.Error(DateTime.Now.ToString());
                                    log.Error(ex.ToString());
                                    log.Error(" ===================  End of Error  =========================== ");
                                    if (MessageBox.Show(ex.ToString(),
                                            "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                                    {
                                        Environment.Exit(0);
                                    }
                                    Environment.Exit(0);
                                }
                                return;
                            }
                            if (e.Key == Key.A)
                            {
                                try

                                {
                                    ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                                    ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                                    OverAllDiscount_sy.Text = AD_CurrencyCode;

                                    ValueChanger_key_pad.Focusable = true;
                                    Keyboard.Focus(ValueChanger_key_pad);
                                    Percentage_OR_Price = AD_CurrencyCode;
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    CrashApp_Alert();
                                    log.Error(" ===================  Error In Retail POS  =========================== ");
                                    log.Error(DateTime.Now.ToString());
                                    log.Error(ex.ToString());
                                    log.Error(" ===================  End of Error  =========================== ");
                                    if (MessageBox.Show(ex.ToString(),
                                            "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                                    {
                                        Environment.Exit(0);
                                    }
                                    Environment.Exit(0);
                                }
                            }
                        }
                        if (LineIteam_Up_Rf == "Quantity_Cart_LineIteam")
                        {
                            if (e.Key == Key.OemPlus || e.Key == Key.Add)
                            {
                                int qty = 0;
                                if (ValueChanger_key_pad.Text == "")
                                {
                                    ValueChanger_key_pad.Text = (qty + 1).ToString();
                                    ValueChanger_key_pad.SelectAll();
                                    _selectedtext_valuecharger = true;
                                    ValueChanger_key_pad.Focus();
                                }
                                else if (ValueChanger_key_pad.Text != "")
                                {
                                    qty = Convert.ToInt32(ValueChanger_key_pad.Text);
                                    ValueChanger_key_pad.Text = (qty + 1).ToString();
                                    ValueChanger_key_pad.SelectAll();
                                    _selectedtext_valuecharger = true;
                                    ValueChanger_key_pad.Focus();


                                }
                                return;
                            }
                            if (productSearch_cart.Text == "")
                            {
                                if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                                {
                                    if (ValueChanger_key_pad.Text != "")
                                    {
                                        if (Convert.ToInt32(ValueChanger_key_pad.Text) > 0)
                                        {
                                            int qty = Convert.ToInt32(ValueChanger_key_pad.Text);
                                            ValueChanger_key_pad.Text = (qty - 1).ToString();
                                            ValueChanger_key_pad.SelectAll();
                                            _selectedtext_valuecharger = true;
                                            ValueChanger_key_pad.Focus();
                                        }

                                    }
                                    return;
                                }
                            }
                        }
                    }

                }
                if (Check_keyboard_Focus == "Payment_Card_Only_tx_GotFocus" || Check_keyboard_Focus == "Payment_Cash_Only_tx_GotFocus" || Check_keyboard_Focus == "Payment_Credit_Name_tx_GotFocus" || Check_keyboard_Focus == "Payment_Complementary_Name_tx_GotFocus")
                {
                    if (e.Key == Key.F1)
                    {
                        _Payment_Cash();
                        return;
                    }

                    if (e.Key == Key.F12)
                    {
                        _Payment_Card();
                        return;
                    }

                    //if (e.Key == Key.F2)
                    //{
                    //    New_Sale_hold();
                    //    return;
                    //}


                    //  if (e.Key == Key.D && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F7)
                    {
                        _Payment_Credit();
                        return;
                    }
                    //if (e.Key == Key.L && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F9)
                    {
                        _Payment_Complementry();
                        return;
                    }
                    if (e.Key == Key.X && (Keyboard.Modifiers == ModifierKeys.Control))
                    {
                        _Payment_Split();
                        return;
                    }
                    if (e.Key == Key.F8)
                    {
                        Sale_hold();
                        return;
                    }
                    if (OrderComplected_button.IsEnabled == true && e.SystemKey == Key.F10)
                    {
                        Sale_complete();
                        return;

                    }
                }
                if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                {
                    if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
                    {
                        open_Prod_Search();
                        return;
                    }
                    if (e.Key == Key.Delete)
                    {
                        if (productSearch_cart.Text == "")
                        {
                            if (items.Count > 0)
                            {
                                #region Customer display 
                                double lessedamt = 0 - Convert.ToDouble(items[ListView_Index_No].Amount);
                                string lessedSellingPrice = lessedamt.ToString("0.00");
                                string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                                int TotalSpace = Convert.ToInt32(CustDispspace);
                                string strproductname = SerialPort.Truncate(items[ListView_Index_No].Product_Name, 7);
                                int productnamelen = strproductname.Length;
                                int sellingPricelen = lessedSellingPrice.Length;

                                items.RemoveAt(ListView_Index_No);
                                ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                view.Refresh();
                                Grand_Total_cart_price.Text = String.Empty;
                                addAmount = 0.00;
                                foreach (var data in items)
                                {
                                    addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                }
                                Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                double Actual_Amount = 0.00;
                                foreach (var data in items)
                                {
                                    // if (data.Line_Discount == "N")
                                    // {
                                    Actual_Amount = Actual_Amount + Convert.ToDouble(data.Sopricestd.ToString()) * data.Quantity;
                                    // }
                                }
                                if (Convert.ToDouble(Grand_Total_cart_price.Text) < Actual_Amount)
                                {
                                    txtActualtotal.Text = Actual_Amount.ToString("0.00");
                                    txtActualtotal.TextDecorations = TextDecorations.Strikethrough;
                                    txtactualamountqr.Visibility = Visibility.Visible;

                                }
                                else
                                {
                                    txtActualtotal.Text = string.Empty;
                                    txtactualamountqr.Visibility = Visibility.Hidden;

                                }
                                Grand_Cart_Total = addAmount;
                                Product_Each_Item_Count = 0;
                                items.ToList().ForEach(x =>
                                {
                                    Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                });
                                if (items.Count() == 0)
                                {
                                    Grand_Total_cart_price.Text = "0.00";
                                    Cart_Iteam_Count.Text = "0";
                                    Grand_Cart_Total = 0;
                                    SubToTal_Balance_Amount = 0;
                                    OverAllDiscount_tx.Text = "0";
                                    payment_method_selected = 0;
                                    Cart_OverAllDiscount = 0;
                                    Product_Each_Item_Count = 0;
                                    OverAllDiscount_button.IsEnabled = false;
                                    OrderComplected_button.IsEnabled = false;
                                    OrderCancel_button.IsEnabled = false;
                                    txtActualtotal.Text = string.Empty;
                                    txtactualamountqr.Visibility = Visibility.Hidden;
                                }
                                Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                                Keyboard.Focus(productSearch_cart);
                                iteamProduct.SelectedIndex = 0;
                                int totallen = "Total".Length;

                                MaintainActual_AMount();
                                string strtotalprice = Grand_Cart_Total.ToString("0.00");
                                int totalpricelen = strtotalprice.Length;
                                int space1 = 0;
                                if (productnamelen > sellingPricelen)
                                {
                                    space1 = TotalSpace - productnamelen - sellingPricelen;

                                }
                                int space2 = 0;
                                if (totallen > totalpricelen)
                                {
                                    space2 = TotalSpace - totallen - totalpricelen;

                                }


                                // int space1 = TotalSpace - productnamelen - sellingPricelen;
                                // int space2 = TotalSpace - totallen - totalpricelen;
                                // int space1 = TotalSpace - productnamelen - sellingPricelen;
                                // int space2 = TotalSpace - totallen - totalpricelen;
                                string strspace1 = new string(' ', space1);
                                string strspace2 = new string(' ', space2);
                                SerialPort.display(strproductname + strspace1, lessedSellingPrice, "Total" + strspace2, strtotalprice);
                                #endregion Customer display
                            }
                            return;

                        }


                    }
                    if (e.Key == Key.F3)
                    {
                        if (items.Count > 0)
                        {
                            SessionHead.Visibility = Visibility.Visible;
                            SessionHead.Text = "Cancel Order Alert";
                            SessionDescription.Text = "Are You Sure To Cancel?";
                            SessionResume.Visibility = Visibility.Hidden;
                            SessionClose.Visibility = Visibility.Hidden;
                            SessionCreateNew.Visibility = Visibility.Hidden;
                            txt_openingBal.Visibility = Visibility.Hidden;
                            txt_Price.Visibility = Visibility.Hidden;
                            SessionChangePrice.Visibility = Visibility.Hidden;

                            Session_Check.Visibility = Visibility.Visible;
                            Error_page.Visibility = Visibility.Visible;
                            CancelYes.Visibility = Visibility.Visible;
                            CancelNo.Visibility = Visibility.Visible;
                            CancelYes.Focus();
                            Keyboard.Focus(CancelYes);
                        }
                        return;
                        //Sale_Cancel();
                    }
                    if (CustLeft.Visibility == Visibility.Visible && WrongOrder.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F1)
                        {
                            SessionResume.Visibility = Visibility.Hidden;
                            SessionClose.Visibility = Visibility.Hidden;
                            SessionCreateNew.Visibility = Visibility.Hidden;
                            txt_openingBal.Visibility = Visibility.Hidden;
                            txt_Price.Visibility = Visibility.Hidden;
                            SessionChangePrice.Visibility = Visibility.Hidden;


                            Session_Check.Visibility = Visibility.Hidden;
                            Error_page.Visibility = Visibility.Hidden;
                            CustLeft.Visibility = Visibility.Hidden;
                            WrongOrder.Visibility = Visibility.Hidden;
                            CancelYes.Visibility = Visibility.Hidden;
                            CancelNo.Visibility = Visibility.Hidden;
                            Sale_Cancel("Customer Left");
                            return;
                        }
                        if (e.Key == Key.F2)
                        {
                            SessionResume.Visibility = Visibility.Hidden;
                            SessionClose.Visibility = Visibility.Hidden;
                            SessionCreateNew.Visibility = Visibility.Hidden;
                            txt_openingBal.Visibility = Visibility.Hidden;
                            txt_Price.Visibility = Visibility.Hidden;
                            SessionChangePrice.Visibility = Visibility.Hidden;


                            Session_Check.Visibility = Visibility.Hidden;
                            Error_page.Visibility = Visibility.Hidden;
                            CustLeft.Visibility = Visibility.Hidden;
                            WrongOrder.Visibility = Visibility.Hidden;
                            CancelYes.Visibility = Visibility.Hidden;
                            CancelNo.Visibility = Visibility.Hidden;
                            Sale_Cancel("Wrong Order");
                            return;
                        }
                    }
                    if (SessionCreateNew.Visibility == Visibility.Visible && Error_page.Visibility == Visibility.Visible && Session_Check.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F3)
                        {
                            CreateNewSession();
                            Error_page.Visibility = Visibility.Hidden;
                            Session_Check.Visibility = Visibility.Hidden;
                            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                            Keyboard.Focus(productSearch_cart);
                            return;
                        }
                    }


                    if (SessionResume.Visibility == Visibility.Visible && Error_page.Visibility == Visibility.Visible && Session_Check.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F4)
                        {
                            ResumeSession();
                            Error_page.Visibility = Visibility.Hidden;
                            Session_Check.Visibility = Visibility.Hidden;
                            Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                            Keyboard.Focus(productSearch_cart);
                            return;
                        }
                    }
                    else
                    {
                        if (items.Count > 0)
                        {
                            if (e.Key == Key.F4 && OverAllDiscount_button.IsEnabled == true)
                            {

                                overAllDiscount();
                                return;
                            }
                        }
                    }
                    if (SessionClose.Visibility == Visibility.Visible && Error_page.Visibility == Visibility.Visible && Session_Check.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.F5)
                        {
                            Check_keyboard_Focus = "Session_Close_Pannel";
                            Session_Close_Pannel.Visibility = Visibility.Visible;
                            Cart_Main_Pannel.Visibility = Visibility.Hidden;
                            Error_page.Visibility = Visibility.Hidden;
                            Session_Check.Visibility = Visibility.Hidden;
                            Calculate_OpeningBalance();
                            Keyboard.Focus(SessionClose_Pruchase_input);
                            return;
                        }
                    }
                    if (SessionChangePrice.Visibility == Visibility.Visible && Error_page.Visibility == Visibility.Visible && Session_Check.Visibility == Visibility.Visible)
                    {

                        if (e.Key == Key.F5)
                        {
                            Error_page.Visibility = Visibility.Hidden;
                            Session_Check.Visibility = Visibility.Hidden;

                            add_Product();
                            return;
                        }
                    }
                    //  if (e.Key == Key.C && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F1)
                    {
                        _Payment_Cash();
                        return;
                    }
                    if (e.Key == Key.F8)
                    {
                        Sale_hold();
                        return;
                    }

                    if (e.Key == Key.F6)
                    {
                        Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                        Keyboard.Focus(productSearch_cart);
                        return;
                    }

                    if (CancelYes.Visibility == Visibility.Visible && CancelNo.Visibility == Visibility.Visible && Error_page.Visibility == Visibility.Visible && Session_Check.Visibility == Visibility.Visible)
                    {
                        if (e.Key == Key.Y)
                        {
                            SessionHead.Visibility = Visibility.Visible;
                            SessionHead.Text = "Cancel Order Alert";
                            SessionDescription.Text = "Reason for Cancel!!";
                            SessionResume.Visibility = Visibility.Hidden;
                            SessionClose.Visibility = Visibility.Hidden;
                            SessionCreateNew.Visibility = Visibility.Hidden;
                            txt_openingBal.Visibility = Visibility.Hidden;
                            txt_Price.Visibility = Visibility.Hidden;
                            SessionChangePrice.Visibility = Visibility.Hidden;

                            Session_Check.Visibility = Visibility.Visible;
                            Error_page.Visibility = Visibility.Visible;
                            CustLeft.Visibility = Visibility.Visible;
                            WrongOrder.Visibility = Visibility.Visible;
                            CustLeft.Focus();
                            CancelYes.Visibility = Visibility.Hidden;
                            CancelNo.Visibility = Visibility.Hidden;
                            return;
                        }

                    }
                    if (items.Count > 0)
                    {
                        if (e.Key == Key.F11)
                        {
                            if (iteamProduct.SelectedItem != null)
                            {
                                LineIteam_Up_Rf = "Quantity_Cart_LineIteam";
                                ValueChanger_key_pad.Focusable = true;
                                Keyboard.Focus(ValueChanger_key_pad);
                                iteamProduct.SelectedIndex = ListView_Index_No;
                                //Button button = (Button)sender;
                                //TextBox TextBox = (TextBox)button.Content;
                                ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Quantity.ToString();
                                ValueChanger_key_pad.SelectAll();
                                _selectedtext_valuecharger = true;
                                GeneralTextPopUp.Visibility = Visibility.Visible;
                                GeneralTextPopUp.Text = "Enter the Quantity ";
                                popUpText.Visibility = Visibility.Hidden;
                                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                            }

                            return;
                        }

                        //if (e.Key == Key.F12)
                        //{
                        //    if (iteamProduct.SelectedItem != null)
                        //    {
                        //        LineIteam_Up_Rf = "Return_Price_Cart_LineIteam";
                        //        ValueChanger_key_pad.Focusable = true;
                        //        Keyboard.Focus(ValueChanger_key_pad);
                        //        iteamProduct.SelectedIndex = ListView_Index_No;
                        //        //Button button = (Button)sender;
                        //        //TextBox TextBox = (TextBox)button.Content;
                        //        ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Amount.ToString();
                        //        ValueChanger_key_pad.SelectAll();
                        //        _selectedtext_valuecharger = true;
                        //        GeneralTextPopUp.Visibility = Visibility.Visible;
                        //        GeneralTextPopUp.Text = "Return Price";
                        //        popUpText.Visibility = Visibility.Hidden;
                        //        ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                        //        ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                        //    }

                        //    return;
                        //}
                        if (productSearch_cart.Text == "")
                        {
                            if (e.Key == Key.OemPlus || e.Key == Key.Add)
                            {

                                int qty = 0;

                                if (items[ListView_Index_No].Quantity >= 0)
                                {
                                    qty = Convert.ToInt32(items[ListView_Index_No].Quantity);
                                    string str_qty = (qty + 1).ToString();
                                    items[ListView_Index_No].Quantity = Math.Round(Convert.ToDouble(str_qty.ToString()), 2);
                                    items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");
                                    if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    else
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }

                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    Product_Each_Item_Count = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                    });
                                    Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                    MaintainActual_AMount();

                                }
                                return;
                            }
                            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                            {

                                int qty = Convert.ToInt32(items[ListView_Index_No].Quantity);
                                if (qty > 0)
                                {
                                    string str_qty = (qty - 1).ToString();
                                    items[ListView_Index_No].Quantity = Math.Round(Convert.ToDouble(str_qty.ToString()), 2);
                                    items[ListView_Index_No].Amount = (Math.Round(((Convert.ToDouble(items[ListView_Index_No].Quantity)) * (Convert.ToDouble(items[ListView_Index_No].Sopricestd))), 2)).ToString("0.00");
                                    if (items[ListView_Index_No].Percentpercentage_OR_Price == "%")
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount / 100 * Total_Price;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }
                                    else
                                    {
                                        double Total_Price = Convert.ToDouble(items[ListView_Index_No].Amount);
                                        double Discount_Amt = items[ListView_Index_No].Discount;
                                        double Selling_Price = Total_Price - Discount_Amt;

                                        items[ListView_Index_No].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                    }

                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;
                                    Product_Each_Item_Count = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                    });
                                    Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();
                                    MaintainActual_AMount();
                                }
                                return;
                            }
                        }
                        if (e.Key == Key.F5)
                        {
                            //var item = (sender as FrameworkElement).DataContext;
                            //int index = iteamProduct.Items.IndexOf(item);
                            //ListView_Index_No = index;


                            if (Percentage_OR_Price == "%")
                            {
                                LineIteam_Up_Rf = "Discount_Cart_LineIteam";

                                ValueChanger_key_pad.Focusable = true;
                                Keyboard.Focus(ValueChanger_key_pad);

                                iteamProduct.SelectedIndex = ListView_Index_No;
                                ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Discount.ToString();
                                ValueChanger_key_pad.SelectAll();
                                _selectedtext_valuecharger = true;
                                popUpText.Visibility = Visibility.Visible;
                                popUpText.Text = "Line Discount";
                                ToggleButton_for_DisAndPrice.Visibility = Visibility.Visible;
                                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                LineIteam_Up_Rf = "Discount_Cart_LineIteam";

                                ValueChanger_key_pad.Focusable = true;
                                Keyboard.Focus(ValueChanger_key_pad);

                                iteamProduct.SelectedIndex = ListView_Index_No;
                                ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Discount.ToString();
                                ValueChanger_key_pad.SelectAll();
                                _selectedtext_valuecharger = true;
                                popUpText.Visibility = Visibility.Visible;
                                popUpText.Text = "Line Discount";
                                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Visible;
                                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                                // }

                            }
                            GeneralTextPopUp.Visibility = Visibility.Hidden;
                            return;
                        }

                    }
                    // if (e.Key == Key.R && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F12)
                    {
                        _Payment_Card();
                        return;
                    }




                    //  if (e.Key == Key.D && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F7)
                    {
                        _Payment_Credit();
                        return;
                    }
                    //if (e.Key == Key.L && (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)))
                    if (e.Key == Key.F9)
                    {
                        _Payment_Complementry();
                        return;
                    }
                    if (e.Key == Key.X && (Keyboard.Modifiers == ModifierKeys.Control))
                    {
                        _Payment_Split();
                        return;
                    }
                    if (e.Key == Key.F8)
                    {
                        Sale_hold();
                        return;
                    }
                    if (OrderComplected_button.IsEnabled == true && e.SystemKey == Key.F10)
                    {
                        Sale_complete();
                        return;

                    }
                    if (e.Key == Key.F6)
                    {
                        productSearch_cart.Focus();
                        return;
                    }
                }
                if (e.Key == Key.F2)
                {

                    if (Check_keyboard_Focus == "Add_Products_View_GotFocus")
                    {
                        Add_New_Product_Window();
                        return;
                    }
                    if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                    {
                        Add_New_Product_Window();
                        return;
                    }
                    if (Check_keyboard_Focus == "BarcodeSearch_cart_GotFocus")
                    {
                        Add_Products_List_Window();
                        return;
                    }

                }
                if (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                {

                    if (e.Key == Key.F3)
                    {
                        if (lstprodadd.Items.Count > 0)
                            select_lstproductAgrreegator(0);

                        return;
                    }
                    if (e.Key == Key.F4)
                    {
                        if (lstprodadd.Items.Count > 1)
                            select_lstproductAgrreegator(1);
                        return;
                    }
                    if (e.Key == Key.F5)
                    {
                        if (lstprodadd.Items.Count > 2)
                            select_lstproductAgrreegator(2);
                        return;
                    }
                    if (e.Key == Key.N && (Keyboard.Modifiers == ModifierKeys.Control))
                    {
                        Button_Click_1(sender, e);
                        return;

                    }
                }
                if (e.Key == Key.F3)
                {
                    if (Check_keyboard_Focus == "Add_Products_View_GotFocus")
                    {
                        Edit_Product_Window();
                        return;
                    }

                }
                if (e.Key == Key.O && (Keyboard.Modifiers == ModifierKeys.Control))
                {
                    __Side_Menu_Page.Visibility = Visibility.Visible;
                    InvoiceReSync.Visibility = Visibility.Visible;
                    Change_Content_of_Side_Menu_buttons("View_Orders");
                    return;
                }
                if (e.Key == Key.M && (Keyboard.Modifiers == ModifierKeys.Control))
                {
                    ManualSync();
                    return;
                }

                if (e.Key == Key.Back)
                {
                    return;
                }
                if (e.Key == Key.Escape)
                {
                    if (SessionHead.Visibility == Visibility.Visible)
                    {
                        Session_Check.Visibility = Visibility.Hidden;
                        Error_page.Visibility = Visibility.Hidden;
                        CustLeft.Visibility = Visibility.Hidden;
                        WrongOrder.Visibility = Visibility.Hidden;
                        CancelYes.Visibility = Visibility.Hidden;
                        CancelNo.Visibility = Visibility.Hidden;
                        if (txt_openingBal.Visibility == Visibility.Visible)
                        {
                            txt_openingBal.Text = "0";
                            SessionCreateNew_Click(sender, e);
                        }
                    }
                    if (paymentPopup.IsOpen)
                    {
                        paymentPopup.IsOpen = false;
                        Error_page.Visibility = Visibility.Hidden;
                        quick_ValueChanger_key_pad.Focus();
                        quick_ValueChanger_key_pad.Focusable = true;
                        Keyboard.Focus(quick_ValueChanger_key_pad);
                        return;
                    }
                    if (Check_keyboard_Focus == "Add_Products_View_GotFocus")
                    {
                        Back_OR_Esc();
                        return;
                    }
                    if (Check_keyboard_Focus == "Manual_Sync_GotFocus")
                    {
                        Back_OR_Esc();
                        return;

                    }
                    if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus" && (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_View_From_No_Sync_GotFocus"))
                    {

                        __Side_quickMenu_Page.Visibility = Visibility.Visible;
                        Menu_Page_Product.Visibility = Visibility.Hidden;
                        Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                        Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                        load_Quick_Products_Btn();
                        return;
                    }
                    if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus" && Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                    {
                        Back_OR_Esc();
                        return;
                    }
                    if (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_View_Unit_GotFocus")
                    {
                        Add_Products_List_Window();
                        //Menu_Page_Product.Visibility = Visibility.Visible;
                        //grdprodviewlist.Visibility = Visibility.Visible;
                        //txtprodSearch.Text = "";
                        //ddlCategory.SelectedIndex = 0;
                        //fetch_all_products_details();
                        //lstuomList.ItemsSource = null;
                        //lstuomList.Items.Clear();
                        //grdprodadd.Visibility = Visibility.Hidden;
                        //btnaddprod.Visibility = Visibility.Visible;
                        //btnEdit.Visibility = Visibility.Visible;
                        //dock_save_cancel_btn.Visibility = Visibility.Hidden;
                        //Change_Content_of_Side_Menu_buttons("Add_Products");
                        //ddlCategory.SelectedIndex = 0;
                        //quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        //bind_unitlist();
                        //quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        //Check_keyboard_Focus = "Add_Products_View_GotFocus";
                        //Keyboard.Focus(txtprodSearch);
                        return;
                    }
                    if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus" && Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                    {
                        __Side_quickMenu_Page.Visibility = Visibility.Visible;
                        Menu_Page_Product.Visibility = Visibility.Hidden;
                        Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                        Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                        load_Quick_Products_Btn();
                        return;
                    }
                    if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus")
                    {
                        quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        bind_unitlist();
                        quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        //Check_keyboard_Focus = "Add_Products_View_Unit_GotFocus";
                        btnaddprod.Focus();
                        Keyboard.Focus(btnaddprod);
                        return;

                    }
                    if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "")
                    {

                        Add_Products_List_Window();
                        //Menu_Page_Product.Visibility = Visibility.Visible;
                        //grdprodviewlist.Visibility = Visibility.Visible;
                        //txtprodSearch.Text = "";
                        //ddlCategory.SelectedIndex = 0;
                        //fetch_all_products_details();
                        //lstuomList.ItemsSource = null;
                        //lstuomList.Items.Clear();
                        //grdprodadd.Visibility = Visibility.Hidden;
                        //btnaddprod.Visibility = Visibility.Visible;
                        //btnEdit.Visibility = Visibility.Visible;
                        //dock_save_cancel_btn.Visibility = Visibility.Hidden;
                        //Change_Content_of_Side_Menu_buttons("Add_Products");
                        //ddlCategory.SelectedIndex = 0;
                        //quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        //bind_unitlist();
                        //quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        //Check_keyboard_Focus = "Add_Products_View_GotFocus";
                        //Keyboard.Focus(txtprodSearch);
                        //return; 
                        //quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                        //bind_unitlist();
                        //quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                        //Check_keyboard_Focus = "Add_Products_View_Unit_GotFocus";
                        //btnaddprod.Focus();
                        //Keyboard.Focus(btnaddprod);
                        return;


                    }

                    Back_OR_Esc();
                    return;
                }
                if (quick_Check_windows_Focus == "Quick_Product_Window_GotFocus" && Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus")
                {
                    if (e.Key == Key.Q)
                    {
                        quick_ValueChanger_key_pad.SelectAll();
                        _selectedtext_quickvaluecharger = true;
                        Keyboard.Focus(quick_ValueChanger_key_pad);
                    }
                    return;

                }

                if (e.Key == Key.Escape && (Keyboard.Modifiers == ModifierKeys.Shift))
                {
                    //Error_page.Visibility = Visibility.Visible;
                    //Logout_Check.Visibility = Visibility.Visible;
                    //SessionLogout.IsDefault = true;
                    Check_keyboard_Focus = "Session_Close_Pannel";
                    Session_Close_Pannel.Visibility = Visibility.Visible;
                    Cart_Main_Pannel.Visibility = Visibility.Hidden;
                    Error_page.Visibility = Visibility.Hidden;
                    Session_Check.Visibility = Visibility.Hidden;
                    Calculate_OpeningBalance();
                    Keyboard.Focus(SessionClose_card_input);
                    return;
                }
                if (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_View_GotFocus")
                {
                    if (e.Key == Key.F12)
                    {
                        openPrintBarcode();
                        //  MessageBox.Show("Print Barcode in progress");
                        return;
                    }
                }

                if ((Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus") || (Check_keyboard_Focus == "quick_ValueChanger_key_pad_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus") || (Check_keyboard_Focus == "Add_Products_View_From_No_Sync_GotFocus" || Check_keyboard_Focus == "Add_Unit_KeyBoard_Got_Focus" || Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_View_Unit_GotFocus"))
                {

                    if (e.Key == Key.F2)
                    {
                        Open_New_Unit();
                        if ((Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus") || (Check_keyboard_Focus == "Add_Products_View_From_No_Sync_GotFocus"))
                        {

                            if (POSGetProductApiJSONResponce_Bom != null)
                            {

                                Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, lblprodid_.Text);

                                POSGetProductApiJSONResponce_Bom = null;
                            }
                        }
                        return;

                    }
                    if (e.SystemKey == Key.F10)
                    {
                        if (dock_save_cancel_btn.Visibility == Visibility.Visible)
                        { Post_Product_Details(); }
                        else
                        {
                            MessageBox.Show("Product Cannot Post Until Not Complete and Save Unit Details.");
                        }

                        return;

                    }
                    if (e.Key == Key.Escape)
                    {
                        txtBarcode.Text = "";
                        txtName.Text = "";
                        ddlitem.SelectedIndex = 0;
                        txtUom.SelectedValue = 100;
                        chkcase.IsChecked = false;
                        txtadddeleteunits.Visibility = Visibility.Hidden;
                        txtPurchasePrice.Text = "";
                        txtSalesPrice.Text = "";
                        txtMinimumQty.Text = "";
                        txtMaximumQty.Text = "";
                        chkPriceedit.IsChecked = false;
                        chksellonline.IsChecked = false;
                        txtprodSearch.Text = "";
                        ddlCategory.SelectedIndex = 0;
                        fetch_all_products_details();
                        grdprodviewlist.Visibility = Visibility.Visible;
                        txtprodSearch.Text = "";
                        ddlCategory.SelectedIndex = 0;
                        fetch_all_products_details();
                        Keyboard.Focus(txtprodSearch);
                        lstuomList.ItemsSource = null;
                        lstuomList.Items.Clear();
                        grdprodadd.Visibility = Visibility.Hidden;
                        dock_save_cancel_btn.Visibility = Visibility.Hidden;
                        btnaddprod.Visibility = Visibility.Visible;
                        btnEdit.Visibility = Visibility.Visible;
                        if (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_View_Unit_GotFocus")
                        {
                            Check_keyboard_Focus = "Add_Products_View_GotFocus";

                        }
                        else
                        {
                            Check_keyboard_Focus = "Add_Products_View_From_No_Sync_GotFocus";
                            __Side_quickMenu_Page.Visibility = Visibility.Visible;
                            Check_keyboard_Focus = "quick_ValueChanger_key_pad_GotFocus";
                            Menu_Page_Product.Visibility = Visibility.Hidden;
                            Cart_Main_Pannel_Window.Visibility = Visibility.Hidden;
                            load_Quick_Products_Btn();
                            return;
                        }
                        return;

                    }
                }
                if (Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus")
                {
                    if (e.Key == Key.S && (Keyboard.Modifiers == ModifierKeys.Control))
                    {
                        Save_Unit_Details();
                        return;

                    }
                    if (e.Key == Key.F2)
                    {
                        Open_New_Unit();
                        return;
                    }
                    if (e.SystemKey == Key.F10)
                    {
                        if (dock_save_cancel_btn.Visibility == Visibility.Visible)
                        { Post_Product_Details(); }
                        else
                        {
                            MessageBox.Show("Product Cannot Post Until Not Complete and Save Unit Details.");
                        }

                        return;

                    }
                }

            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void openPrintBarcode()
        {
            print_Barcode_Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(placementForPopup);
            print_Barcode_Popup.IsOpen = true;
            txtNoofBarcodePrint.Focusable = true;
            MainPage.IsEnabled = false;
            txtBarcodeNumber.Text = lblprodBarcode_.Text;
            txtNoofBarcodePrint.Focus();
            Keyboard.Focus(txtNoofBarcodePrint);
            txtNoofBarcodePrint.SelectAll();
        }
        private void Insert_Product_Bom(dynamic POSGetProductApiJSONResponce, string parent_Product_id)
        {
            if (POSGetProductApiJSONResponce != null)
            {
                try
                {

                    string _productId = "";
                    string _productName = "";
                    string _productPrice = "";
                    string isSynced = "";
                    string _productValue = "";
                    string _categoryId = "";
                    string _categoryName = "";
                    string _productArabicName = " ";
                    string _scanbyWeight = "";
                    string _scanbyPrice = "";
                    string _ispriceEditable = "N";
                    string _isquick = "N";
                    string _productUOMId = "";
                    string _productUOMValue = "";
                    string _sellingPrice = "";
                    string _purchasePrice = "";
                    string _costprice = "";
                    string _productMultiUOM = "";
                    JArray _productMultiImage = new JArray();
                    int img_count = 0;
                    string _product_image = " ";
                    int _bomQty = 0;
                    foreach (var productLst in POSGetProductApiJSONResponce)
                    {

                        _productId = productLst.productId;
                        _productName = productLst.productName;
                        _productPrice = productLst.sellingPrice;
                        isSynced = productLst.isSynced;

                        _categoryName = productLst.categoryName;
                        int indexTcat = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == _categoryName);
                        if (indexTcat > -1)
                        {
                            _categoryId = CategoryList_quickdataSource1[indexTcat].categoryID.ToString();
                        }
                        else
                        {
                            _categoryId = CategoryList_quickdataSource1[1].categoryID.ToString();
                        }


                        _productArabicName = productLst.productArabicName ?? " "; ;
                        _scanbyWeight = productLst.scanbyWeight;
                        _scanbyPrice = productLst.scanbyPrice;

                        _productUOMValue = productLst.productUOMValue;
                        int indexTuom = UomList_dataSource.FindIndex(r => r.uomName == _productUOMValue);
                        if (indexTuom > -1)
                        {
                            _productUOMId = UomList_dataSource[indexTuom].uomID.ToString();
                        }
                        else
                        {
                            _productUOMId = "100";
                        }

                        _sellingPrice = productLst.sellingPrice;
                        _purchasePrice = productLst.costPrice;
                        _productValue = productLst.productValue;
                        _costprice = productLst.costPrice;

                        _bomQty = productLst.bomQty;
                        if (img_count > 0)
                        {
                            _product_image = _productMultiImage[0]["productImage"].ToString();
                        }
                        if (_productId != null)
                        {

                            //  Check_keyboard_Focus = "Add_Products_View_From_Sync_GotFocus";
                            NpgsqlConnection connection = new NpgsqlConnection(connstring);
                            connection.Open();
                            NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                            NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                            if (_get__Ad_sequenc_no.Read())
                            {
                                Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                            }
                            connection.Close();
                            if (!check_product_exist(_productId))
                            {
                                connection.Open();
                                NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                                NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                                connection.Close();
                                connection.Open();

                                NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@created)", connection);

                                INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                                INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                                INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                                INSERT_cmd_m_product.ExecuteNonQuery();
                                connection.Close();

                                connection.Open();
                                NpgsqlCommand INSERT_cmd_m_product_Bom = new NpgsqlCommand("INSERT INTO m_product_bom(ad_client_id, ad_org_id, m_product_id, no_of_pcs, m_parent_product_id)VALUES(@ad_client_id,@ad_org_id,@m_product_id, @no_of_pcs, @m_parent_product_id)", connection);

                                INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                                INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                                INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@m_product_id", Convert.ToInt64(_productId));
                                INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@no_of_pcs", _bomQty);
                                INSERT_cmd_m_product_Bom.Parameters.AddWithValue("@m_parent_product_id", Convert.ToInt64(parent_Product_id));
                                INSERT_cmd_m_product_Bom.ExecuteNonQuery();
                                connection.Close();




                                string _productId_PriceArray = _productId;
                                string _pricelistId_PriceArray = AD_PricelistID.ToString();
                                string _pricelistName_PriceArray = _sellingPrice;
                                string _priceStd_PriceArray = _costprice;

                                connection.Close();

                                connection.Open();
                                NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                if (_get__Ad_sequenc_no_m_product_price.Read())
                                {
                                    Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                }
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                                cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                connection.Close();
                                connection.Open();
                                NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                                                                        "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + _pricelistId_PriceArray + "','" + _priceStd_PriceArray + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + _pricelistName_PriceArray + "'); ", connection);
                                INSERT_cmd_m_product_price.ExecuteNonQuery();
                                connection.Close();

                                if (_productMultiUOM == "Y")
                                {

                                    string _uomId_UOMArray = _productUOMId;
                                    string _uomValue_UOMArray = _productUOMValue;
                                    string _uomConvRate_UOMArray = _productUOMValue;

                                    connection.Close();

                                    connection.Open();
                                    cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                    _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                    if (_get__Ad_sequenc_no_m_product_price.Read())
                                    {
                                        Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                    }
                                    connection.Close();
                                    connection.Open();
                                    cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                    cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                    connection.Close();
                                    connection.Open();
                                    INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                  " VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                    INSERT_cmd_m_product_price.ExecuteNonQuery();
                                    connection.Close();
                                }
                            }
                            else
                            {
                                log.Info("Product Already exist  ProductId:" + _productId);

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    CrashApp_Alert();
                    log.Error(" ===================  Error In Retail POS  =========================== ");
                    log.Error(DateTime.Now.ToString());
                    log.Error(ex.ToString());
                    log.Error(" ===================  End of Error  =========================== ");
                    if (MessageBox.Show(ex.ToString(),
                            "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        Environment.Exit(0);
                    }
                    Environment.Exit(0);
                }
            }
        }
        public static string ZeroIfEmpty(string s)
        {
            return string.IsNullOrEmpty(s) ? "0" : s;
        }
        private void select_lstproductAgrreegator(int row)
        {
            // ListViewItem i = (ListViewItem)lstprodadd.ItemContainerGenerator.Items(row);
            //  var i = (lstprodadd.ItemContainerGenerator.Items)[0];
            //load_Process();
            if (POSGetProductApiJSONResponce.products != null)
            {
                try
                {
                    Load_CategoryList(false);
                    Load_uomList();
                    POSGetProductApiJSONResponce_Bom = null;
                    txtunitdetails.Text = "Unit details (0)";
                    lblprodid_.Text = "";
                    string _productId = "";
                    string _productName = "";
                    string _productPrice = "";
                    string isSynced = "";
                    string _productValue = "";
                    string _categoryId = "";
                    string _categoryName = "";
                    string _productArabicName = " ";
                    string _scanbyWeight = "";
                    string _scanbyPrice = "";
                    string _ispriceEditable = "N";
                    string _isquick = "N";
                    string _productUOMId = "";
                    string _productUOMValue = "";
                    string _sellingPrice = "";
                    string _purchasePrice = "";
                    string _costprice = "";
                    string _productMultiUOM = "";
                    JArray _productMultiImage = new JArray();
                    int img_count = 0;
                    string _product_image = " ";
                    int index = 0;
                    lblprodid_.Text = "";
                    txtadddeleteunits.Visibility = Visibility.Visible;
                    foreach (var productLst in POSGetProductApiJSONResponce.products)
                    {
                        if (index == row)
                        {
                            _productId = productLst.productId;
                            _productName = productLst.productName;
                            _productPrice = productLst.sellingPrice;
                            isSynced = productLst.isSynced;

                            _categoryName = productLst.categoryName;
                            int indexTcat = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == _categoryName);
                            if (indexTcat > -1)
                            {
                                _categoryId = CategoryList_quickdataSource1[indexTcat].categoryID.ToString();

                            }
                            else
                            {
                                _categoryId = CategoryList_quickdataSource1[1].categoryID.ToString();
                            }


                            _productArabicName = productLst.productArabicName ?? " "; ;
                            _scanbyWeight = productLst.scanbyWeight;
                            _scanbyPrice = productLst.scanbyPrice;

                            _productUOMValue = productLst.productUOMValue;
                            int indexTuom = UomList_dataSource.FindIndex(r => r.uomName == _productUOMValue);
                            if (indexTuom > -1)
                            {
                                _productUOMId = UomList_dataSource[indexTuom].uomID.ToString(); ;
                            }
                            else
                            {
                                _productUOMId = UomList_dataSource[1].uomID.ToString(); ;

                            }


                            _sellingPrice = productLst.sellingPrice;
                            _purchasePrice = productLst.costPrice;
                            _productValue = productLst.productValue;
                            _costprice = productLst.costPrice;
                            if (productLst.isBomAvailable == "Y")
                            {
                                POSGetProductApiJSONResponce_Bom = productLst.bomDetails;
                            }
                            if (img_count > 0)
                            {
                                _product_image = _productMultiImage[0]["productImage"].ToString();
                            }
                            if (_productId != null && isSynced != null)
                            {
                                if (isSynced == "Y")
                                {
                                    //  Check_keyboard_Focus = "Add_Products_View_From_Sync_GotFocus";
                                    NpgsqlConnection connection = new NpgsqlConnection(connstring);
                                    connection.Open();

                                    connection.Close();

                                    connection.Open();
                                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product';", connection);
                                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                                    if (_get__Ad_sequenc_no.Read())
                                    {
                                        Sequenc_id = _get__Ad_sequenc_no.GetInt64(4) + 1;
                                    }
                                    connection.Close();
                                    if (!check_product_exist(_productId))
                                    {
                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product';", connection);
                                        NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                                        connection.Close();
                                        connection.Open();

                                        NpgsqlCommand INSERT_cmd_m_product = new NpgsqlCommand("INSERT INTO m_product(id, ad_client_id, ad_org_id, m_product_id, m_product_category_id, createdby, updatedby, name, searchkey, arabicname, image, scanbyweight, scanbyprice, uomid, uomname, sopricestd, currentcostprice,ispriceeditable,isquick, attribute1,attribute2,purchaseprice,created)VALUES(@id, @ad_client_id, @ad_org_id, @m_product_id, @m_product_category_id, @createdby, @updatedby, @name, @searchkey, @arabicname, @image, @scanbyweight, @scanbyprice, @uomid, @uomname, @sopricestd, @currentcostprice,@isPriceEditable,@isquick, @attribute1, @attribute2,@purchaseprice,@created)", connection);

                                        INSERT_cmd_m_product.Parameters.AddWithValue("@id", Sequenc_id);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_client_id", AD_Client_ID);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@ad_org_id", AD_ORG_ID);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_id", Convert.ToInt32(_productId));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@m_product_category_id", Convert.ToInt32(_categoryId));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@createdby", Convert.ToInt32(AD_USER_ID));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@updatedby", Convert.ToInt32(AD_USER_ID));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@name", _productName);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@searchkey", _productValue);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@arabicname", _productArabicName);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@image", _product_image);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyweight", _scanbyWeight);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@scanbyprice", _scanbyPrice);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomid", Convert.ToInt32(_productUOMId));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@uomname", _productUOMValue);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@sopricestd", Convert.ToDouble(_sellingPrice));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@currentcostprice", Convert.ToDouble(_costprice));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@purchaseprice", Convert.ToDouble(_purchasePrice));
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@isPriceEditable", _ispriceEditable);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@isquick", _isquick);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute1", _productMultiUOM);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@attribute2", WarehouseId_Selected);
                                        INSERT_cmd_m_product.Parameters.AddWithValue("@created", DateTime.Now);
                                        INSERT_cmd_m_product.ExecuteNonQuery();
                                        connection.Close();

                                        string _productId_PriceArray = _productId;
                                        string _pricelistId_PriceArray = AD_PricelistID.ToString();
                                        string _pricelistName_PriceArray = _sellingPrice;
                                        string _priceStd_PriceArray = _costprice;

                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_price';", connection);
                                        NpgsqlDataReader _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                        if (_get__Ad_sequenc_no_m_product_price.Read())
                                        {
                                            Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                        }
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_price(id, ad_client_id, ad_org_id, m_product_id, pricelistid, pricestd,createdby,updatedby, name)" +
                                        "VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + ",'" + _productId_PriceArray + "','" + ZeroIfEmpty(_pricelistId_PriceArray) + "','" + ZeroIfEmpty(_priceStd_PriceArray) + "'," + AD_USER_ID + "," + AD_USER_ID + ",'" + ZeroIfEmpty(_pricelistName_PriceArray) + "'); ", connection);
                                        INSERT_cmd_m_product_price.ExecuteNonQuery();
                                        connection.Close();

                                        connection.Open();
                                        NpgsqlCommand cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_price';", connection);
                                        cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                        connection.Close();

                                        if (_productMultiUOM == "Y")
                                        {

                                            string _uomId_UOMArray = _productUOMId;
                                            string _uomValue_UOMArray = _productUOMValue;
                                            string _uomConvRate_UOMArray = _productUOMValue;

                                            connection.Close();

                                            connection.Open();
                                            cmd_select_sequenc_no_m_product_price = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'm_product_uom';", connection);
                                            _get__Ad_sequenc_no_m_product_price = cmd_select_sequenc_no_m_product_price.ExecuteReader();

                                            if (_get__Ad_sequenc_no_m_product_price.Read())
                                            {
                                                Sequenc_id = _get__Ad_sequenc_no_m_product_price.GetInt64(4) + 1;
                                            }
                                            connection.Close();
                                            connection.Open();
                                            cmd_update_sequenc_no_m_product_price = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'm_product_uom';", connection);
                                            cmd_update_sequenc_no_m_product_price.ExecuteReader();
                                            connection.Close();
                                            connection.Open();
                                            INSERT_cmd_m_product_price = new NpgsqlCommand("INSERT INTO m_product_uom(m_product_uom_id, ad_client_id, ad_org_id, createdby,updatedby, m_product_id, uomid, uomvalue, uomconvrate)" +
                                                                                                          " VALUES(" + Sequenc_id + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_USER_ID + "," + AD_USER_ID + "," + _productId + "," + _uomId_UOMArray + ",'" + _uomValue_UOMArray + "'," + _uomConvRate_UOMArray + ");", connection);
                                            INSERT_cmd_m_product_price.ExecuteNonQuery();
                                            connection.Close();


                                        }
                                    }
                                    else
                                    {
                                        log.Info("Product Already exist  ProductId:" + _productId);

                                    }
                                    if (productLst.isBomAvailable == "Y" && POSGetProductApiJSONResponce_Bom != null)
                                    {
                                        Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, _productId);
                                        POSGetProductApiJSONResponce_Bom = null;
                                    }




                                    int _m_product_id, _uomid, _m_product_category_id;
                                    double _currentcostprice, _sopricestd;
                                    string _name, _uomname, _searchkey, _arabicname, _scanbyweight, _scanbyprice;

                                    _m_product_id = Convert.ToInt32(_productId);
                                    _m_product_category_id = Convert.ToInt32(_categoryId);
                                    _name = _productName;
                                    _searchkey = _productValue;
                                    _arabicname = _productArabicName;
                                    //  _image = _clientId_Read.GetString(7);
                                    _scanbyweight = _scanbyWeight;
                                    _scanbyprice = _scanbyPrice;
                                    _uomid = Convert.ToInt32(_productUOMId);

                                    txtUom1.SelectedValue = _uomid;
                                    _uomname = _productUOMValue;
                                    _sopricestd = Convert.ToDouble(_sellingPrice);
                                    _currentcostprice = Convert.ToDouble(_costprice);
                                    // _productMultiUOM = _clientId_Read.GetString(14); 

                                    string _product_name = _name;
                                    string __product_id = _m_product_id.ToString();
                                    string _product_barcode = _searchkey;
                                    string _product_ad_client_id = AD_Client_ID.ToString();
                                    string _product_ad_org_id = AD_ORG_ID.ToString();
                                    string _product_m_product_category_id = _m_product_category_id.ToString();
                                    string _product_arabicname = _arabicname;
                                    //string _product_image = _image;
                                    string _product_scanbyweight = _scanbyweight;
                                    string _product_scanbyprice = _scanbyprice;
                                    string _product_uomid = _uomid.ToString();
                                    string _product_uomname = _uomname;
                                    string _product_SellingPricestd = _sellingPrice;
                                    string _product_currentcostprice = _currentcostprice.ToString();
                                    // string _is_productMultiUOM = _productMultiUOM;

                                    int item_count = items.Count();
                                    if (item_count == 0)
                                    {
                                        double product_discount = 0;
                                        double product_totalAmount = Convert.ToDouble(_product_SellingPricestd);

                                        items.Insert(0, new Product()
                                        {
                                            #region Display Fields

                                            Discount = product_discount,
                                            Quantity = 1,
                                            Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                                            Amount = product_totalAmount.ToString("0.00"),
                                            Percentpercentage_OR_Price = Percentage_OR_Price,
                                            Product_Name = _product_name,
                                            Iteam_Barcode = _product_barcode,

                                            #endregion Display Fields

                                            #region Hidden Fields

                                            Ad_client_id = _product_ad_client_id,
                                            Ad_org_id = _product_ad_org_id,
                                            // Is_productMultiUOM = _is_productMultiUOM,
                                            Product_Arabicname = _product_arabicname,
                                            Product_category_id = _product_m_product_category_id,
                                            Product_ID = __product_id,
                                            //  Product_Image = _product_image,
                                            Scanby_Price = _product_scanbyprice,
                                            Scanby_Weight = _product_scanbyweight,
                                            Current_costprice = _product_currentcostprice,
                                            Sopricestd = _product_SellingPricestd,
                                            Uom_Id = _product_uomid,
                                            Uom_Name = _product_uomname

                                            #endregion Hidden Fields
                                        });
                                    }
                                    else
                                    {
                                        bool result = false;
                                        var item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode).FirstOrDefault());
                                        if (txt_Price.Text != "")
                                        {
                                            result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00");
                                            item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == txt_Price.Text + ".00").FirstOrDefault());
                                        }
                                        else
                                        {
                                            result = items.Exists(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00");
                                            item_index = items.IndexOf(items.Where(x => x.Iteam_Barcode == _product_barcode && x.Price == _product_SellingPricestd + ".00").FirstOrDefault());
                                        }

                                        if (result == true)
                                        {

                                            var _Quantity = items[item_index].Quantity + 1;
                                            items[item_index].Quantity = Math.Round(Convert.ToDouble(_Quantity.ToString()), 2);
                                            //items[item_index].Amount = Math.Round((Convert.ToDouble(_Quantity.ToString()) * items[item_index].Amount), 2);
                                            items[item_index].Amount = (Math.Round(((Convert.ToDouble(items[item_index].Quantity)) * (Convert.ToDouble(items[item_index].Sopricestd))), 2)).ToString("0.00");
                                            if (items[item_index].Percentpercentage_OR_Price == "%")
                                            {
                                                double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                                double Discount_Amt = items[item_index].Discount / 100 * Total_Price;
                                                double Selling_Price = Total_Price - Discount_Amt;

                                                items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                            }
                                            else
                                            {
                                                double Total_Price = Convert.ToDouble(items[item_index].Amount);
                                                double Discount_Amt = items[item_index].Discount;
                                                double Selling_Price = Total_Price - Discount_Amt;

                                                items[item_index].Amount = (Math.Round(Selling_Price, 2)).ToString("0.00");
                                            }
                                            var item_reorder = items[item_index];
                                            items.RemoveAt(item_index);
                                            items.Insert(0, item_reorder);
                                        }
                                        else
                                        {
                                            double product_discount = 0;
                                            double product_totalAmount = Convert.ToDouble(_product_SellingPricestd.ToString());


                                            items.Insert(0, new Product()
                                            {
                                                #region Display Fields

                                                Discount = product_discount,
                                                Quantity = 1,
                                                Price = (Convert.ToDouble(_product_SellingPricestd)).ToString("0.00"),
                                                Amount = product_totalAmount.ToString("0.00"),
                                                Percentpercentage_OR_Price = Percentage_OR_Price,
                                                Product_Name = _product_name,
                                                Iteam_Barcode = _product_barcode,

                                                #endregion Display Fields

                                                #region Hidden Fields

                                                Ad_client_id = _product_ad_client_id,
                                                Ad_org_id = _product_ad_org_id,
                                                // Is_productMultiUOM = _is_productMultiUOM,
                                                Product_Arabicname = _product_arabicname,
                                                Product_category_id = _product_m_product_category_id,
                                                Product_ID = __product_id,
                                                //  Product_Image = _product_image,
                                                Scanby_Price = _product_scanbyprice,
                                                Scanby_Weight = _product_scanbyweight,
                                                Current_costprice = _product_currentcostprice,
                                                Sopricestd = _product_SellingPricestd,
                                                Uom_Id = _product_uomid,
                                                Uom_Name = _product_uomname

                                                #endregion Hidden Fields
                                            });
                                        }
                                    }

                                    ProductIteams = items;
                                    ICollectionView view = CollectionViewSource.GetDefaultView(ProductIteams);
                                    view.Refresh();
                                    Grand_Total_cart_price.Text = String.Empty;
                                    addAmount = 0.00;
                                    foreach (var data in items)
                                    {
                                        addAmount = addAmount + Convert.ToDouble(data.Amount.ToString());
                                    }
                                    Grand_Total_cart_price.Text = addAmount.ToString("0.00");
                                    Grand_Cart_Total = addAmount;

                                    productSearch_cart.Text = String.Empty;
                                    #region Customer display 
                                    string SellingPrice = Convert.ToDouble(_product_SellingPricestd).ToString("0.00");
                                    string CustDispspace = ConfigurationManager.AppSettings.Get("CusterDisplaytotalspace");

                                    int TotalSpace = Convert.ToInt32(CustDispspace);
                                    string strproductname = SerialPort.Truncate(_product_name, 7);
                                    int productnamelen = strproductname.Length;
                                    int sellingPricelen = SellingPrice.Length;
                                    int totallen = "Total".Length;
                                    string strtotalprice = Grand_Cart_Total.ToString("0.00");
                                    int totalpricelen = strtotalprice.Length;


                                    int space1 = 0;
                                    if (productnamelen > sellingPricelen)
                                    {
                                        space1 = TotalSpace - productnamelen - sellingPricelen;

                                    }
                                    int space2 = 0;
                                    if (totallen > totalpricelen)
                                    {
                                        space2 = TotalSpace - totallen - totalpricelen;

                                    }

                                    //    int space1 = TotalSpace - productnamelen - sellingPricelen;
                                    //   int space2 = TotalSpace - totallen - totalpricelen;

                                    string strspace1 = new string(' ', space1);
                                    string strspace2 = new string(' ', space2);
                                    SerialPort.display(strproductname + strspace1, SellingPrice, "Total" + strspace2, strtotalprice);
                                    #endregion Customer display 

                                    Product_Each_Item_Count = 0;
                                    items.ToList().ForEach(x =>
                                    {
                                        Product_Each_Item_Count += Convert.ToInt32(x.Quantity);
                                    });
                                    Cart_Iteam_Count.Text = Product_Each_Item_Count.ToString();

                                    if (invoice_number == 0 && items.Count() > 0)
                                    {
                                        #region Get Invoice & Doc No

                                        //Checking and Getting Invoice POS Number
                                        var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                                        int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                                        int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                                        string _responce_code = Check_POS_Number_rs.Item3;
                                        bool _network_status_ = Check_POS_Number_rs.Item4;
                                        if (_responce_code == "0" || _responce_code == "200")
                                        {
                                            invoice_number = Check_POS_Number_rs.Item1;
                                            document_no = Check_POS_Number_rs.Item2;
                                            InvoiceNo.Text = invoice_number.ToString();
                                        }
                                        else if (_network_status_ != true || _responce_code == "500")
                                        {
                                            Error_page.Visibility = Visibility.Visible;
                                            NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                                            return;
                                        }

                                        #endregion Get Invoice & Doc No
                                    }
                                    iteamProduct.SelectedItem = items.FirstOrDefault();
                                    Keyboard.Focus(productSearch_cart);
                                    quick_ValueChanger_key_pad.Text = "0.00";
                                    Back_OR_Esc();
                                    MaintainActual_AMount();
                                    //Bind_Product_Search();
                                    Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                                    Keyboard.Focus(productSearch_cart);
                                }
                                else
                                {
                                    // Check_keyboard_Focus = "Add_Products_View_From_No_Sync_GotFocus";
                                    Load_CategoryList(false);
                                    Load_uomList();
                                    fetch_all_products_details();
                                    Menu_Page_Product.Visibility = Visibility.Visible;

                                    grdprodviewlist.Visibility = Visibility.Hidden;
                                    grdprodadd.Visibility = Visibility.Visible;
                                    quick_Side_Menu_Page_View_Unit_Content_Empty.Visibility = Visibility.Visible;
                                    // bind_unitlist();

                                    uom_dataSource.Clear();
                                    chkcase.IsChecked = false;
                                    if (productLst.bomDetails != null)
                                    {
                                        chkcase.IsChecked = true;

                                    }
                                    dock_save_cancel_btn.Visibility = Visibility.Visible;
                                    quick_Side_Menu_Page_Edit_Unit_Content.Visibility = Visibility.Hidden;
                                    btnaddprod.Visibility = Visibility.Hidden;
                                    btnEdit.Visibility = Visibility.Hidden;
                                    txtBarcode.Text = _productValue;
                                    // Back_with_product.Text = _productValue;
                                    txtName.Text = _productName;
                                    string categoryid = _categoryId;
                                    ddlitem.SelectedValue = categoryid;
                                    // txtUom1.SelectedValue= _get_c_products.GetString(12);
                                    txtUom.SelectedValue = _productUOMId;

                                    if (ddlitem.SelectedValue == null)
                                    {
                                        int indexT = CategoryList_quickdataSource1.FindIndex(r => r.categoryName == _categoryName);
                                        if (indexT > -1)
                                        {
                                            ddlitem.SelectedIndex = indexT;
                                        }
                                        else
                                        {
                                            ddlitem.SelectedIndex = 1;
                                        }


                                    }
                                    // txtUom1.SelectedValue= _get_c_products.GetString(12);
                                    txtUom.SelectedValue = _productUOMId;
                                    if (txtUom.SelectedValue == null)
                                    {
                                        int indexT = UomList_dataSource.FindIndex(r => r.uomName == _productUOMValue);
                                        if (indexT > -1)
                                        {
                                            txtUom.SelectedIndex = indexT;

                                        }
                                        else
                                        {
                                            txtUom.SelectedValue = 100;
                                        }

                                    }
                                    if (uom_dataSource.Count == 0)
                                    {
                                        chkprodcase.IsChecked = false;
                                    }


                                    txtPurchasePrice.Text = _purchasePrice;
                                    txtCostPrice.Text = _costprice;
                                    txtSalesPrice.Text = _sellingPrice;
                                    txtSalesPrice.Focus();
                                    txtSalesPrice.SelectAll();
                                    chkPriceedit.IsChecked = false;
                                    if (_ispriceEditable == "Y")
                                    {
                                        chkPriceedit.IsChecked = true;
                                    }
                                    else
                                    {
                                        chkPriceedit.IsChecked = false;
                                    }
                                    if (productLst.bomDetails != null)
                                    {
                                        foreach (var productbomLst in productLst.bomDetails)
                                        {
                                            chkprodcase.IsChecked = true;
                                            uom_dataSource.Add(new Product_Uom
                                            {

                                                barCode = productbomLst.productValue,
                                                productName = productbomLst.productName,
                                                uomType = productbomLst.categoryName,
                                                uomValue = productbomLst.productUOMValue,
                                                categoryid = productbomLst.categoryId,
                                                uomid = productbomLst.productUOMId,
                                                salesPrice = Convert.ToDecimal(productbomLst.sellingPrice).ToString("0.00"),
                                                purchasePrice = Convert.ToDecimal(productbomLst.costPrice).ToString("0.00"),
                                                currency = AD_CurrencyCode,
                                                productId = productbomLst.productId,
                                                costprice = productbomLst.costPrice,
                                                categoryName = productbomLst.categoryName,
                                                noofpcs = productbomLst.bomQty
                                            });

                                        }

                                        lstuomList.ItemsSource = null;
                                        lstuomList.ItemsSource = uom_dataSource;

                                    }

                                    quick_Side_Menu_Page_View_Unit_Content.Visibility = Visibility.Visible;
                                    txtunitdetails.Text = "Unit details (" + uom_dataSource.Count.ToString() + ")";
                                    Check_keyboard_Focus = "Add_Products_View_From_No_Sync_GotFocus";


                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    // unLoad_Process();
                    CrashApp_Alert();
                    log.Error(" ===================  Error In Retail POS  =========================== ");
                    log.Error(DateTime.Now.ToString());
                    log.Error(ex.ToString());
                    log.Error(" ===================  End of Error  =========================== ");
                    if (MessageBox.Show(ex.ToString(),
                            "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        Environment.Exit(0);
                    }
                    Environment.Exit(0);
                }
            }
            //unLoad_Process();
        }
        public static Visual GetDescendantByType(Visual element, Type type, string name)
        {
            if (element == null) return null;
            if (element.GetType() == type)
            {
                FrameworkElement fe = element as FrameworkElement;
                if (fe != null)
                {
                    if (fe.Name == name)
                    {
                        return fe;
                    }
                }
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
                (element as FrameworkElement).ApplyTemplate();
            for (int i = 0;
                i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type, name);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }
        private void btnlstunitdelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //  load_Process();
                var curItem = ((ListBoxItem)lstuomList.ContainerFromElement((Button)sender));
                string Prodid = ((Restaurant_Pos.Product_Uom)curItem.Content).productId;
                if (Prodid != "")
                {
                    NpgsqlConnection connection = new NpgsqlConnection(connstring);
                    List<String> Items = txtunitprodIdbefadd.Text.Split(',').Select(i => i.Trim()).Where(i => i != string.Empty).ToList(); //Split them all and remove spaces
                    connection.Open();
                    NpgsqlCommand cmd_delete_unitprodid = new NpgsqlCommand("delete from m_product_bom where m_product_id=" + Prodid, connection);
                    NpgsqlDataReader _get_delete_prodid = cmd_delete_unitprodid.ExecuteReader();
                    connection.Close();
                    connection.Open();
                    cmd_delete_unitprodid = new NpgsqlCommand("delete from m_product where m_product_id=" + Prodid, connection);
                    _get_delete_prodid = cmd_delete_unitprodid.ExecuteReader();
                    connection.Close();
                    Items.Remove(Prodid);
                    txtunitprodIdbefadd.Text = String.Join(",", Items.ToArray());
                    bind_unitlist();
                    dock_save_cancel_btn.Visibility = Visibility.Visible;
                    //  unLoad_Process();  
                }
            }
            catch (Exception ex)
            {
                // unLoad_Process();
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void txtPurchasePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPurchasePrice.Text))
            {
                txtCostPrice.Clear();
                return;
            }
            txtCostPrice.Text = txtPurchasePrice.Text;
        }

        private void txtPurchasePrice1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPurchasePrice1.Text))
            {
                txtCostPrice1.Clear();
                return;
            }
            txtCostPrice1.Text = txtPurchasePrice1.Text;

        }

        private void Side_Menu_Add_category_Click(object sender, RoutedEventArgs e)
        {
            View_Category_List_Page();
        }


        private void imgbarcodegen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Print Barcode in progress");
            return;
        }

        private void BtnAddNewProd_Quick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_New_Product_Window();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void lstproducts_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    foreach (var item in lstproducts.Items)
                    {
                        ListViewItem i = (ListViewItem)lstproducts.ItemContainerGenerator.ContainerFromItem(item);
                        if (i != null)
                        {
                            //Seek out the ContentPresenter that actually presents our DataTemplate
                            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                            TextBlock txtprodId = (TextBlock)i.ContentTemplate.FindName("txtProdid", contentPresenter);
                            TextBlock txtProdName = (TextBlock)i.ContentTemplate.FindName("txtProd", contentPresenter);
                            txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        }
                    }

                    ListViewItem selectedItem = e.OriginalSource as ListViewItem;

                    if (selectedItem.Content != null && selectedItem.IsSelected)
                    {


                        TextBlock txtprodId = GetChildrenByType(selectedItem, typeof(TextBlock), "txtProdid") as TextBlock;
                        TextBlock txtProdName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtProd") as TextBlock;
                        txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        if (txtprodId != null)
                        {
                            view_Product(txtprodId.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void lstproducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = this.lstproducts.SelectedIndex;
                int CurrentRow = 0;
                foreach (var item in lstproducts.Items)
                {
                    if (index == CurrentRow)
                    {
                        long prodid = ((Restaurant_Pos.Pages.RetailPage.QuickProductList)item).ProdID;
                        view_Product(prodid.ToString());
                        return;
                    }
                    CurrentRow++;
                }
                //foreach (var item in lstproducts.Items)
                //  {
                //      ListViewItem i = (ListViewItem)lstproducts.ItemContainerGenerator.ContainerFromItem(item);
                //      if (i != null)
                //      {
                //          if(index==CurrentRow)
                //          {
                //              //Seek out the ContentPresenter that actually presents our DataTemplate
                //              ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                //              TextBlock txtprodId = (TextBlock)i.ContentTemplate.FindName("txtProdid", contentPresenter);
                //              TextBlock txtProdName = (TextBlock)i.ContentTemplate.FindName("txtProd", contentPresenter);
                //              txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                //              txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);

                //          }
                //          else
                //          {
                //              //Seek out the ContentPresenter that actually presents our DataTemplate
                //              ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                //              TextBlock txtprodId = (TextBlock)i.ContentTemplate.FindName("txtProdid", contentPresenter);
                //              TextBlock txtProdName = (TextBlock)i.ContentTemplate.FindName("txtProd", contentPresenter);
                //              txtprodId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                //              txtProdName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);

                //          }

                //          CurrentRow++;
                //      }
                //  }

                //    ListViewItem selectedItem = sender as ListViewItem;

                //    if (selectedItem.Content != null && selectedItem.IsSelected)
                //    { 

                //        TextBlock txtprodId = GetChildrenByType(lstproducts, typeof(TextBlock), "txtProdid") as TextBlock;
                //    TextBlock txtProdName = GetChildrenByType(lstproducts, typeof(TextBlock), "txtProd") as TextBlock;

                //}
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void grditemuom_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)e.OriginalSource;
            Button btnedelete = GetChildrenByType(grid, typeof(Button), "btnlstunitdelete") as Button;
            if (Check_keyboard_Focus == "Add_Products_NewEdit_GotFocus" || Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus")
            {
                btnedelete.Visibility = Visibility.Visible;
            }
            else
            {
                btnedelete.Visibility = Visibility.Hidden;
            }

        }

        private void iteamProduct_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                object index = e.Source;
                int val = ((System.Windows.Controls.Primitives.Selector)index).SelectedIndex;
                ListView_Index_No = val;
            }


        }

        private void Cancel_Quick_Cart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Back_OR_Esc();
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // productSearch_cart.IsDropDownOpen = true;
        }

        private void productSearch_cart_TextChanged(object sender, TextChangedEventArgs e)
        {

            // if (productSearch_cart.Text=="")
            // productSearch_cart.IsDropDownOpen = false;
            //else
            //    productSearch_cart.IsDropDownOpen = true; 
        }


        private void ComboBoxItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Up || e.Key == System.Windows.Input.Key.Down)
            {
                VisualStateManager.GoToState(this, "KeyNavigation", true);
                e.Handled = true;
                return;
            }
            base.OnPreviewKeyDown(e);
        }

        private void productSearch_cart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //productSearch_cart.IsDropDownOpen = false;
        }

        private void txt_openingBal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Enter)
            {
                if (txt_openingBal.Text != "")
                {
                    try
                    {
                        CreateNewSession();
                        #region Get Invoice & Doc No

                        //Checking and Getting Invoice POS Number
                        var Check_POS_Number_rs = RetailViewModel.Check_POS_Number(AD_UserName, AD_UserPassword, AD_Client_ID, AD_ORG_ID, AD_USER_ID, AD_bpartner_Id, AD_ROLE_ID, AD_Warehouse_Id, DeviceMacAdd);
                        int _InvoiceNo_ = Check_POS_Number_rs.Item1;
                        int _doc_no_or_error_code = Check_POS_Number_rs.Item2;
                        string _responce_code = Check_POS_Number_rs.Item3;
                        bool _network_status_ = Check_POS_Number_rs.Item4;
                        if (_responce_code == "0" || _responce_code == "200")
                        {
                            invoice_number = Check_POS_Number_rs.Item1;
                            document_no = Check_POS_Number_rs.Item2;
                            InvoiceNo.Text = invoice_number.ToString();
                        }
                        else if (_network_status_ != true || _responce_code == "500")
                        {
                            Error_page.Visibility = Visibility.Visible;
                            NetworkError_for_getting_invoice.Visibility = Visibility.Visible;
                            return;
                        }

                        #endregion Get Invoice & Doc No

                        string balance = "0";
                        if (txt_openingBal.Text != "")
                        {
                            balance = txt_openingBal.Text;
                        }
                        NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                        connection.Open();
                        long Session_StartTimeMilliseconds = new DateTimeOffset(Convert.ToDateTime(AD_Session_Started_at)).ToUnixTimeMilliseconds();

                        NpgsqlCommand INSERT_c_invoice = new NpgsqlCommand("INSERT INTO c_invoice(" +
                        "c_invoice_id, ad_client_id, ad_org_id, ad_role_id, ad_user_id," +
                        "documentno, m_warehouse_id, c_bpartner_id, qid, mobilenumber," +
                        " orderid, reason, createdby, updatedby," +
                        "openingbalance,session_id)" +
                        "VALUES(" + invoice_number + "," + AD_Client_ID + "," + AD_ORG_ID + "," + AD_ROLE_ID + "," + AD_USER_ID + "," + document_no + "," + AD_Warehouse_Id + "," + AD_bpartner_Id + ",'" + txtCustomer_CR.Text + "','" + txtCustomermobile.Text + "'" +
                        ",0,''," + AD_USER_ID + "," + AD_USER_ID + "," + balance + "," +
                         Session_StartTimeMilliseconds + ");", connection);
                        INSERT_c_invoice.ExecuteNonQuery();
                        connection.Close();
                        Error_page.Visibility = Visibility.Hidden;
                        Session_Check.Visibility = Visibility.Hidden;
                        txt_openingBal.Visibility = Visibility.Hidden;
                        Check_keyboard_Focus = "BarcodeSearch_cart_GotFocus";
                        Keyboard.Focus(productSearch_cart);
                    }
                    catch (Exception ex)
                    {
                        CrashApp_Alert();
                        log.Error(" ===================  Error In Retail POS  =========================== ");
                        log.Error(DateTime.Now.ToString());
                        log.Error(ex.ToString());
                        log.Error(" ===================  End of Error  =========================== ");
                        if (MessageBox.Show(ex.ToString(),
                                "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                        {
                            Environment.Exit(0);
                        }
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Error_page.Visibility = Visibility.Hidden;
                    Session_Check.Visibility = Visibility.Hidden;
                    txt_openingBal.Text = "";
                    add_Product();
                    txt_Price.Text = "";
                }

            }
        }

        private void productSearch_cart_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                e.Handled = true;
                return; // do not call the base class method OnPreviewKeyDown()
            }
            if (e.Key == Key.F4)
            {
                e.Handled = true;
                return;

            }
        }

        private void imgbarcodegen_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Print Barcode in progress");
            return;
        }

        private void imgbarcodegeninview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Print Barcode in progress");
            return;
        }

        private void quick_ValueChanger_key_pad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[a-zA-Z-]*[0-9]*(?:\.[0-9]*)?$");//allow decimal points
            if (regex.IsMatch(e.Text) && !(e.Text == "." && e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtadddeleteunits_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtadddeleteunits.Text == "+ Add Unit [F2]")
                {
                    Open_New_Unit();
                    if ((Check_keyboard_Focus == "Add_Products_Add_Unit_GotFocus" && quick_Check_windows_Focus == "Quick_Product_Window_GotFocus") || (Check_keyboard_Focus == "Add_Products_View_From_No_Sync_GotFocus"))
                    {

                        if (POSGetProductApiJSONResponce_Bom != null)
                        {

                            Insert_Product_Bom(POSGetProductApiJSONResponce_Bom, lblprodid_.Text);

                            POSGetProductApiJSONResponce_Bom = null;
                        }
                    }
                }
                else
                {


                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Payment_Cash_Only_tx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F10)
            {
                OrderComplected_Click(sender, e);
            }
        }

        private void lstprodadd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // int index = this.lstprodadd.SelectedIndex;

            //  select_lstproductAgrreegator(index);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Productid > 0)
                {

                    NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("update m_product set isquick='Y',updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'  where m_product.ad_client_id = " + AD_Client_ID + " AND m_product.ad_org_id = " + AD_ORG_ID + " AND m_product.m_product_id =" + Productid + ";", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();
                    load_Quick_Products_Btn();
                    Error_page.Visibility = Visibility.Hidden;
                    paymentPopup.IsOpen = false;
                    Productid = 0;
                }

            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        long lstsearchprodid = 0;
        private void lstProdSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (lstProdSearch.SelectedItem != null)
                {
                    lstsearchprodid = (lstProdSearch.SelectedItem as ProductList).ProdID;

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }
        private void CrashApp_Alert()
        {
            if (items.Count > 0)
            {
                if (MessageBox.Show("Technical Problem!! Appliation has been Closed.. All  Items Will Be Moved To Hold List?",
                   "Error In Retail POS", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    New_Sale_hold();
                }
            }
        }

        private void SessionClose_input_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);

        }
        //private void lstProdSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    ListView _ListView = sender as ListView;
        //    GridView _GridView = _ListView.View as GridView;

        //    var _ActualWidth1 = _ListView.Width * 50/100;
        //    var _ActualWidth2 = _ListView.Width * 25 / 100; ;
        //    var _ActualWidth3 = _ListView.Width * 25 / 100; ;
        //    _GridView.Columns[1].Width = _ActualWidth1;
        //    _GridView.Columns[0].Width = _ActualWidth2;
        //    _GridView.Columns[2].Width = _ActualWidth3; 

        //}










        private void PopupClosing(object sender, EventArgs e)
        {
            lstProdSearch.ItemsSource = null;
            txtprodSearch.Text = string.Empty;
            product_Search_Popup.IsOpen = false;
            MainPage.IsEnabled = true;
            txtProdSearch.Focusable = true;
            Keyboard.Focus(productSearch_cart);
            productSearch_cart.Focus();
            //Back_OR_Esc();
            return;
            //Code to execute when Popup closes
        }

        private void lstProdSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // int index = this.lstprodadd.SelectedIndex;
            try
            {

                if (lstProdSearch.SelectedItem != null)
                {
                    lstsearchprodid = (lstProdSearch.SelectedItem as ProductList).ProdID;
                    Add_Prod_From_Search();
                    return;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }


        }

        private void lstprod_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_3(sender, e);
                return;
            }
        }

        private void paymentPopup_Closed(object sender, EventArgs e)
        {
            lstprod.ItemsSource = null;
            txtSearch.Text = string.Empty;
            paymentPopup.IsOpen = false;
            MainPage.IsEnabled = true;
            quick_ValueChanger_key_pad.Focusable = true;
            Error_page.Visibility = Visibility.Hidden;
            Keyboard.Focus(quick_ValueChanger_key_pad);
            quick_ValueChanger_key_pad.Focus();
            //Back_OR_Esc();
            return;
        }

        private void btnPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            string noofbarcode = "";
            if (txtNoofBarcodePrint.Text == "")
            {
                noofbarcode = "1";
            }
            else
            {
                noofbarcode = txtNoofBarcodePrint.Text;
            }
            RetailViewModel.Pring_No_of_Barcode(noofbarcode, lblprodBarcode_.Text, lblprodName_.Text, lblprodSalesPrice_.Text, lblprodUom_.Text);
            print_Barcode_Popup.IsOpen = false;

        }



        private void btCancelPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            print_Barcode_Popup.IsOpen = false;


        }

        private void print_Barcode_Popup_Closed(object sender, EventArgs e)
        {
            txtNoofBarcodePrint.Text = String.Empty;
            MainPage.IsEnabled = true;
            txtprodSearch.Focus();
            Keyboard.Focus(txtprodSearch);
        }

        private void imgbarcodegeninview_Click(object sender, RoutedEventArgs e)
        {
            openPrintBarcode();
        }

        private void lblprintbarcodeinview_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            openPrintBarcode();
        }

        private void Side_Menu_Pos_Settings_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_POS_Setting_Page.Visibility = Visibility.Visible;
            Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
            Side_Menu_Page_View_Pos_Setting_General_Click(sender, e);
        }

        private void Side_Menu_Page_View_Pos_Setting_General_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_POS_Setting_Page.Visibility = Visibility.Visible;
            Side_Menu_Page_POS_General_Content.Visibility = Visibility.Visible;
            Side_Menu_Page_POS_Printer_Content.Visibility = Visibility.Hidden;
            fetch_General_settings();
        }

        private void Side_Menu_Page_Printer_Setting_Click(object sender, RoutedEventArgs e)
        {
            __Side_Menu_POS_Setting_Page.Visibility = Visibility.Visible;
            Side_Menu_Page_POS_General_Content.Visibility = Visibility.Hidden;
            Side_Menu_Page_POS_Printer_Content.Visibility = Visibility.Visible;
            fetch_printer_settings();
        }
        private void fetch_General_settings()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var POSLanguage = configuration.AppSettings.Settings["POSLanguage"];
            if (POSLanguage == null)
            {
                rdblanguageenglish.IsChecked = false;
                rdblanguageArabic.IsChecked = false;
            }
            else
            {
                if (POSLanguage.Value == "English")
                {
                    rdblanguageenglish.IsChecked = true;
                }
                if (POSLanguage.Value == "Arabic")
                {
                    rdblanguageArabic.IsChecked = true;
                }
            }
            var prdViewOption = configuration.AppSettings.Settings["prdViewOption"];
            if (prdViewOption == null)
            {
                rdbimage.IsChecked = false;
                rdbtext.IsChecked = false;
            }
            else
            {
                if (prdViewOption.Value == "Image")
                {
                    rdbimage.IsChecked = true;
                }
                if (prdViewOption.Value == "OnlyText")
                {
                    rdbtext.IsChecked = true;
                }
            }
            var BillFormat = configuration.AppSettings.Settings["BillFormat"];
            if (BillFormat == null)
            {
                chkprintshorbill.IsChecked = false;
                chkprintA4.IsChecked = false;
                chkprintA5.IsChecked = false;
            }
            else
            {
                if (BillFormat.Value == "ShortBill")
                {
                    chkprintshorbill.IsChecked = true;
                }
                if (BillFormat.Value == "A4")
                {
                    chkprintA4.IsChecked = true;
                }
                if (BillFormat.Value == "A5")
                {
                    chkprintA5.IsChecked = true;
                }
                if (BillFormat.Value == "NormalBill")
                {
                    chkNormaBill.IsChecked = true;
                }
            }
            var NeedDupCopy = configuration.AppSettings.Settings["NeedDupCopy"];
            if (NeedDupCopy == null)
            {
                chkdupcopy.IsChecked = false;

            }
            else
            {
                if (NeedDupCopy.Value == "Yes")
                {
                    chkdupcopy.IsChecked = true;
                }
                if (NeedDupCopy.Value == "No")
                {
                    chkdupcopy.IsChecked = false;
                }

            }
            var RoundOffDiscount = configuration.AppSettings.Settings["RoundOffDiscount"];
            if (RoundOffDiscount == null)
            {
                chkroundoff.IsChecked = false;

            }
            else
            {
                if (RoundOffDiscount.Value == "Yes")
                {
                    chkroundoff.IsChecked = true;
                }
                if (RoundOffDiscount.Value == "No")
                {
                    chkroundoff.IsChecked = false;
                }

            }
        }
        private void fetch_printer_settings()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var PrinterType = configuration.AppSettings.Settings["PrinterType"];
            if (PrinterType == null)
            {
                rdbRio.IsChecked = false;
                rdbInbuild.IsChecked = false;
            }
            else
            {
                if (PrinterType.Value == "Rio")
                {
                    rdbRio.IsChecked = true;
                }
                if (PrinterType.Value == "InBuild")
                {
                    rdbInbuild.IsChecked = true;
                }
            }
            var PrinterOption = configuration.AppSettings.Settings["PrinterOption"];
            if (PrinterOption == null)
            {
                rdbUsb.IsChecked = false;
                rdbWifi.IsChecked = false;
                rdbSerial.IsChecked = false;
            }
            else
            {
                if (PrinterOption.Value == "USB")
                {
                    rdbUsb.IsChecked = true;
                }
                if (PrinterOption.Value == "Wifi")
                {
                    rdbWifi.IsChecked = true;
                }
                if (PrinterOption.Value == "Serial")
                {
                    rdbSerial.IsChecked = true;
                }
            }
        }
        private static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var entry = configuration.AppSettings.Settings[key];
            if (entry == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void btnsettingsave_Click(object sender, RoutedEventArgs e)
        {
            if (Side_Menu_Page_POS_General_Content.Visibility == Visibility.Visible)
            {
                if (rdblanguageArabic.IsChecked == true)
                {
                    SetSetting("POSLanguage", "Arabic");
                }
                if (rdblanguageenglish.IsChecked == true)
                {
                    SetSetting("POSLanguage", "English");
                }
                if (rdbimage.IsChecked == true)
                {
                    SetSetting("prdViewOption", "Image");
                }
                if (rdbtext.IsChecked == true)
                {
                    SetSetting("prdViewOption", "OnlyText");
                }
                if (chkprintshorbill.IsChecked == true)
                {
                    SetSetting("BillFormat", "ShortBill");
                }
                if (chkprintA4.IsChecked == true)
                {
                    SetSetting("BillFormat", "A4");
                }
                if (chkprintA5.IsChecked == true)
                {
                    SetSetting("BillFormat", "A5");
                }
                if (chkNormaBill.IsChecked == true)
                {
                    SetSetting("BillFormat", "NormalBill");
                }
                if (chkdupcopy.IsChecked == true)
                {
                    SetSetting("NeedDupCopy", "Yes");
                }
                if (chkdupcopy.IsChecked == true)
                {
                    SetSetting("NeedDupCopy", "Yes");
                }
                else
                {
                    SetSetting("NeedDupCopy", "No");
                }
                if (chkroundoff.IsChecked == true)
                {
                    SetSetting("RoundOffDiscount", "Yes");
                }
                else
                {
                    SetSetting("RoundOffDiscount", "No");
                }
                Back_OR_Esc();
            }
            if (Side_Menu_Page_POS_Printer_Content.Visibility == Visibility.Visible)
            {
                if (rdbRio.IsChecked == true)
                {
                    SetSetting("PrinterType", "Rio");
                }
                if (rdbInbuild.IsChecked == true)
                {
                    SetSetting("PrinterType", "InBuild");
                }
                if (rdbUsb.IsChecked == true)
                {
                    SetSetting("PrinterOption", "USB");
                }
                if (rdbWifi.IsChecked == true)
                {
                    SetSetting("PrinterOption", "Wifi");
                }
                if (rdbSerial.IsChecked == true)
                {
                    SetSetting("PrinterOption", "Serial");
                }
                Back_OR_Esc();
            }

        }
        private void btnsettingcancel_Click(object sender, RoutedEventArgs e)
        {
            Back_OR_Esc();
        }

        private void Side_Menu_Add_Edit_Customer_Click(object sender, RoutedEventArgs e)
        {
            View_Customer_List_Page();
        }
        private void View_Customer_List_Page()
        {
            Menu_Page_Customer.Visibility = Visibility.Visible;
            grdcustomerviewlist.Visibility = Visibility.Visible;
            Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
            grdcustomeradd.Visibility = Visibility.Hidden;
            dock_edit_add_buttons_cust.Visibility = Visibility.Visible;
            lstcust_Bind_customer_Search();
            txtcustomerSearch.Focus();
            Keyboard.Focus(txtcustomerSearch);
            if (lstcustomer.Items.Count > 0)
            {

                lstcustomer.SelectedIndex = 0;
            }
            var MyList = (ListView)lstcustomer;
            //standard
            if (MyList.Items.Count > 0)
            {
                foreach (var customer in MyList.ItemsSource)
                {

                    view_Customer(((CustomerList)customer).Bp_partner_id.ToString());
                    return;
                    //do stuff with it modify the entry text whatever you want
                }
            }
        }
        private void Add_New_Cust_Page()
        {
            clearcustfields();
            Menu_Page_Customer.Visibility = Visibility.Visible;
            grdcustomerviewlist.Visibility = Visibility.Hidden;
            grdcustomeradd.Visibility = Visibility.Visible;
            dock_edit_add_buttons_cust.Visibility = Visibility.Hidden;
            btnSavetxt.Text = "Save";
            txtaddcrnumber_.Focus();

        }
        private void Update_Cust_Page()
        {
            Menu_Page_Customer.Visibility = Visibility.Visible;
            grdcustomerviewlist.Visibility = Visibility.Hidden;
            grdcustomeradd.Visibility = Visibility.Visible;
            dock_edit_add_buttons_cust.Visibility = Visibility.Hidden;
            btnSavetxt.Text = "Update";

            string query = "SELECT searchkey, name,coalesce(lastname, '') AS lastname , coalesce(mobile_number, '') AS mobile_number ,coalesce(emailaddress, '') AS emailaddress ,coalesce(allow_credit_limit, 'N') AS allow_credit_limit ,coalesce(creditamount, '0') AS creditamount , coalesce(address, '0') AS address ,coalesce(city, '0') AS city ,coalesce(country, '0') AS country  ,coalesce(zipcode, '0') AS zipcode  " +

                " FROM  c_bpartner where  ad_client_id =" + AD_Client_ID + " AND  ad_org_id =" + AD_ORG_ID + " and c_bpartner_id = " + txttmpBpartnerid.Text;


            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_customer = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_customer = cmd_fetch_customer.ExecuteReader();
            while (_get_c_customer.Read())
            {

                txtaddcrnumber_.Text = _get_c_customer.GetString(0);
                txtaddcrnumber_.IsEnabled = false;
                //  Back_with_product.Text = _get_c_products.GetString(0);
                txtaddfirstname_.Text = _get_c_customer.GetString(1);
                txtaddfirstname_.Focus();
                txtaddlastname_.Text = _get_c_customer.GetString(2);

                txtaddmobileNumber_.Text = _get_c_customer.GetString(3);
                // txtUom1.SelectedValue= _get_c_products.GetString(12);
                txtaddemailAddress_.Text = _get_c_customer.GetString(4);
                if (_get_c_customer.GetString(5) == "Y")
                    chkaddallowcreditlmt.IsChecked = true;
                else
                    chkaddallowcreditlmt.IsChecked = false;

                txtaddcreditLimit_.Text = _get_c_customer.GetString(6);
                txtaddaddress_.Text = _get_c_customer.GetString(7);
                txtaddcity_.Text = _get_c_customer.GetString(8);
                txtaddcountry_.Text = _get_c_customer.GetString(9);
                txtaddzipcode_.Text = _get_c_customer.GetString(10);

            }
            connection.Close();
        }
        private void clearcustfields()
        {
            txtaddcrnumber_.Text = "";
            txtaddcrnumber_.IsEnabled = true;
            //  Back_with_product.Text = _get_c_products.GetString(0);
            txtaddfirstname_.Text = "";
            txtaddlastname_.Text = "";

            txtaddmobileNumber_.Text = "";
            // txtUom1.SelectedValue= _get_c_products.GetString(12);
            txtaddemailAddress_.Text = "";

            chkaddallowcreditlmt.IsChecked = false;

            txtaddcreditLimit_.Text = "";
            txtaddaddress_.Text = "";
            txtaddcity_.Text = "";
            txtaddcountry_.Text = "";
            txtaddzipcode_.Text = "";
        }

        private void view_Customer(string bpartnerid)
        {
            txttmpBpartnerid.Text = bpartnerid;
            string query = "SELECT searchkey, name,coalesce(lastname, '') AS lastname , coalesce(mobile_number, '') AS mobile_number ,coalesce(emailaddress, '') AS emailaddress ,coalesce(allow_credit_limit, 'N') AS allow_credit_limit ,coalesce(creditamount, '0') AS creditamount , coalesce(address, '0') AS address ,coalesce(city, '0') AS city ,coalesce(country, '0') AS country  ,coalesce(zipcode, '0') AS zipcode  " +

                " FROM  c_bpartner where  ad_client_id =" + AD_Client_ID + " AND  ad_org_id =" + AD_ORG_ID + " and c_bpartner_id = " + bpartnerid;

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_customer = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_customer = cmd_fetch_customer.ExecuteReader();
            while (_get_c_customer.Read())
            {

                lblcrNumber_.Text = _get_c_customer.GetString(0);
                //  Back_with_product.Text = _get_c_products.GetString(0);
                lblfirstName_.Text = _get_c_customer.GetString(1);
                lbllastName_.Text = _get_c_customer.GetString(2);

                lblmobileno_.Text = _get_c_customer.GetString(3);
                // txtUom1.SelectedValue= _get_c_products.GetString(12);
                lblemailid_.Text = _get_c_customer.GetString(4);
                if (_get_c_customer.GetString(5) == "Y")
                    chkallowcreditlimit.IsChecked = true;
                else
                    chkallowcreditlimit.IsChecked = false;

                lblcreditlimit_.Text = _get_c_customer.GetString(6);
                lbladdress_.Text = _get_c_customer.GetString(7);
                lblcity_.Text = _get_c_customer.GetString(8);
                lblcountry_.Text = _get_c_customer.GetString(9);
                lblzipcode_.Text = _get_c_customer.GetString(10);

            }
            connection.Close();
        }
        private void edit_Customer(string bpartnerid)
        {
            string query = "SELECT searchkey, name,coalesce(lastname, '') AS lastname , coalesce(mobile_number, '') AS mobile_number ,coalesce(emailaddress, '') AS emailaddress ,coalesce(allow_credit_limit, 'N') AS allow_credit_limit ,coalesce(creditamount, '0') AS creditamount , coalesce(address, '0') AS address ,coalesce(city, '0') AS city ,coalesce(country, '0') AS country  ,coalesce(zipcode, '0') AS zipcode  " +

                " FROM  c_bpartner where  ad_client_id =" + AD_Client_ID + " AND  ad_org_id =" + AD_ORG_ID + " and c_bpartner_id = " + bpartnerid;

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_customer = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_customer = cmd_fetch_customer.ExecuteReader();
            while (_get_c_customer.Read())
            {

                txtaddcrnumber_.Text = _get_c_customer.GetString(0);
                txtaddcrnumber_.IsEnabled = false;
                //  Back_with_product.Text = _get_c_products.GetString(0);
                txtaddfirstname_.Text = _get_c_customer.GetString(1);
                txtaddlastname_.Text = _get_c_customer.GetString(2);

                txtaddmobileNumber_.Text = _get_c_customer.GetString(3);
                // txtUom1.SelectedValue= _get_c_products.GetString(12);
                txtaddemailAddress_.Text = _get_c_customer.GetString(4);
                if (_get_c_customer.GetString(5) == "Y")
                    chkaddallowcreditlmt.IsChecked = true;
                else
                    chkaddallowcreditlmt.IsChecked = false;

                txtaddcreditLimit_.Text = _get_c_customer.GetString(6);
                txtaddaddress_.Text = _get_c_customer.GetString(7);
                txtaddcity_.Text = _get_c_customer.GetString(8);
                txtaddcountry_.Text = _get_c_customer.GetString(9);
                txtaddzipcode_.Text = _get_c_customer.GetString(10);

            }
            connection.Close();
        }
        private void Lstcustomer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                lstcustomer.SelectedItems.Clear();

                ListViewItem item = sender as ListViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    lstcustomer.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Lstcustomer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in lstcustomer.Items)
                {
                    ListViewItem i = (ListViewItem)lstcustomer.ItemContainerGenerator.ContainerFromItem(item);
                    if (i != null)
                    {
                        //Seek out the ContentPresenter that actually presents our DataTemplate
                        ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                        TextBlock txtBpartnerid = (TextBlock)i.ContentTemplate.FindName("txtBpartnerid", contentPresenter);
                        TextBlock txtFirstName = (TextBlock)i.ContentTemplate.FindName("txtFirstName", contentPresenter);
                        txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    }
                }

                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {


                    TextBlock txtBpartnerid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtBpartnerid") as TextBlock;
                    TextBlock txtFirstName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtFirstName") as TextBlock;
                    txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    if (txtBpartnerid != null)
                    {
                        view_Customer(txtBpartnerid.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void BtnEdit_cust_Click(object sender, RoutedEventArgs e)
        {
            Update_Cust_Page();

        }

        private void Btnaddcust_Click(object sender, RoutedEventArgs e)
        {
            Add_New_Cust_Page();
        }

        private void Btncustsave_Click(object sender, RoutedEventArgs e)
        {

            save_update_customer_Data();

        }
        private void save_update_customer_Data()
        {
            string str_crNumber = "";
            string str_firstName = "";
            string str_lastName = "";
            string mobileNo = "";
            string emailAddress = "";
            string AllowCreditLimit = "N";
            double creditLimit = 0;
            string addresss = "";
            string city = "";
            string country = "";
            string zipcode = "";
            if (txtaddcrnumber_.Text == "")
            {
                MessageBox.Show("CR-Nummber Cant Be Empty");
                txtaddcrnumber_.Focus();
                return;
            }
            else
            {
                str_crNumber = txtaddcrnumber_.Text;
            }
            if (txtaddfirstname_.Text == "")
            {
                MessageBox.Show("First Name Cant Be Empty");
                txtaddfirstname_.Focus();
                return;
            }
            else
            {
                str_firstName = txtaddfirstname_.Text;
            }
            if (txtaddlastname_.Text == "")
            {
                MessageBox.Show("Last Name Cant Be Empty");
                txtaddlastname_.Focus();
                return;
            }
            else
            {
                str_lastName = txtaddlastname_.Text;
            }
            if (txtaddmobileNumber_.Text == "")
            {
                MessageBox.Show("Mobile Number Cant Be Empty");
                txtaddmobileNumber_.Focus();
                return;
            }
            else
            {
                mobileNo = txtaddmobileNumber_.Text;
            }
            emailAddress = txtaddemailAddress_.Text;
            if (chkaddallowcreditlmt.IsChecked == true)
            {
                AllowCreditLimit = "Y";
            }
            if (txtaddcreditLimit_.Text == "")
            {
                MessageBox.Show("Credit Limit Cant Be Empty");
                txtaddcreditLimit_.Focus();
                return;
            }
            else
            {
                creditLimit = Convert.ToDouble(txtaddcreditLimit_.Text);

            }
            if (txtaddaddress_.Text == "")
            {
                MessageBox.Show("Address Cant Be Empty");
                txtaddaddress_.Focus();
            }
            else
            {
                addresss = txtaddaddress_.Text;
            }
            if (txtaddcity_.Text == "")
            {
                MessageBox.Show("City Can Be Empty");
                txtaddcity_.Focus();
                return;

            }
            else
            {
                city = txtaddcity_.Text;
            }
            if (txtaddcountry_.Text == "")
            {
                MessageBox.Show("Country Can Be Empty");
                txtaddcountry_.Focus();
                return;

            }
            else
            {
                country = txtaddcountry_.Text;
            }
            if (txtaddzipcode_.Text == "")
            {
                MessageBox.Show("Postal Code Cant Be Empty");
                txtaddzipcode_.Focus();
                return;
            }
            else
            {
                zipcode = txtaddzipcode_.Text;
            }
            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            if (btnSavetxt.Text == "Save")
            {
                if (check_customer_exist(txtaddcrnumber_.Text))
                {
                    MessageBox.Show("Qid/CR_Number Already Exist");
                    return;

                }
                else
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand("select bpid-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);


                    // Execute the query and obtain the value of the first column of the first row
                    Int32 bpid = Convert.ToInt32(command.ExecuteScalar());
                    txttmpBpartnerid.Text = bpid.ToString();
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_update_pr_no_m_product_price = new NpgsqlCommand("UPDATE m_pos_sequence SET bpid =" + txttmpBpartnerid.Text + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                    NpgsqlDataReader _update__pr_sequenc_no = cmd_update_pr_no_m_product_price.ExecuteReader();
                    connection.Close();
                    int Sequenc_id = 9;
                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'ee_msrmanager';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                    if (_get__Ad_sequenc_no.Read())
                    {
                        Sequenc_id = _get__Ad_sequenc_no.GetInt32(4) + 1;
                    }
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'ad_client';", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand INSERT_customerdetails = new NpgsqlCommand("INSERT INTO c_bpartner(" +
                         "id,c_bpartner_id,searchkey, name, lastname,mobile_number, emailaddress, allow_credit_limit,creditamount, address, city, country, zipcode,ad_org_id,ad_client_id,createdby,updatedby)" +
                         " VALUES(" + Sequenc_id + ",'" + txttmpBpartnerid.Text + "','" + str_crNumber + "','" + str_firstName + "','" + str_lastName + "','" + mobileNo + "','" + emailAddress + "','" + AllowCreditLimit + "','" + creditLimit + "','" + addresss + "','" + city + "','" + country + "','" + zipcode + "','" + AD_ORG_ID + "','" + AD_Client_ID + "','" + AD_USER_ID + "','" + AD_USER_ID + "'); ", connection);
                    INSERT_customerdetails.ExecuteNonQuery();
                    MessageBox.Show("Customer Details Has Been Saved");
                    txtaddcrnumber_.Focus();
                    connection.Close();
                }

            }
            if (btnSavetxt.Text == "Update")
            {
                connection.Open();
                NpgsqlCommand UPDATE_c_customerdetails = new NpgsqlCommand("UPDATE c_bpartner set " +
                             " name='" + str_firstName + "', lastname='" + str_lastName + "'," +
                             "mobile_number='" + mobileNo + "', emailaddress='" + emailAddress + "', allow_credit_limit='" + AllowCreditLimit + "'," +
                             "creditamount='" + creditLimit + "', address='" + addresss + "', city='" + city + "', country='" + country + "', zipcode='" + zipcode + "', createdby='" + AD_USER_ID + "'" +
                             " WHERE c_bpartner_id = '" + txttmpBpartnerid.Text + "'", connection);
                UPDATE_c_customerdetails.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Customer Details Has Been Updated");
                View_Customer_List_Page();

            }

            connection.Close();
            JObject json = new JObject(
                               new JProperty("username", AD_UserName),
                                      new JProperty("operation", "POSAddEditCustomer"),
                                     new JProperty("macAddress", DeviceMacAdd),
                                      new JProperty("clientId", AD_Client_ID),
                                     new JProperty("orgId", AD_ORG_ID),
                                     new JProperty("roleId", AD_ROLE_ID),
                                    new JProperty("warehouseId", AD_Warehouse_Id),
                                    new JProperty("userId", AD_USER_ID),
                                    new JProperty("pricelistId", AD_PricelistID),
                                    new JProperty("costElementId", AD_CostelementID),
                                   new JProperty("accountSchemaId", AD_AccountSchemaid),
                                    new JProperty("businessPartnerId", txttmpBpartnerid.Text),
                                    new JProperty("showImage", "Y"),
                                    new JProperty("version", "1.6"),
                                    new JProperty("appName", "POS"),
                                    new JProperty("remindMe", "N"),
                                   new JProperty("customerName", str_firstName),
                                    new JProperty("lastName", str_lastName),
                                    new JProperty("customerValue", str_crNumber),
                                    new JProperty("customerEmail", emailAddress),
                                    new JProperty("customerNumber", mobileNo),
                                    new JProperty("isCredit", AllowCreditLimit),
                                    new JProperty("creditLimit", creditLimit),
                                   new JProperty("openingBalance", creditLimit),
                                   new JProperty("isCustomer", "Y"),
                                    new JProperty("isVendor", "N"),
                                    new JProperty("address1", addresss),
                                    new JProperty("city", city),
                                   new JProperty("country", country),
                                    new JProperty("postal", zipcode)
                                    );
            var val = json.ToString();
            int CheckApiError = 0;
            try
            {
                POSAddcustomerApiStringResponce = PostgreSQL.ApiCallPost(val);
                CheckApiError = 1;
            }
            catch
            {

                CheckApiError = 0;
                log.Error("POSAddProduct: Server Error");
                log.Error("----------------JSON Request--------------");
                log.Error(val);
                log.Error("----------------JSON END------------------");
            }
            if (CheckApiError == 1)
            {
                POSAddcustomerApiStringResponce = JsonConvert.DeserializeObject(POSAddcustomerApiStringResponce);
                log.Info("POSAddcustomerApiStringResponce: " + POSAddcustomerApiStringResponce + "");
                if (POSAddcustomerApiStringResponce.responseCode == "200")
                {
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_Update_post_flag_customer_id = new NpgsqlCommand("UPDATE c_bpartner " +
                       "SET c_bpartner_id =  " + POSAddcustomerApiStringResponce.businessPartnerId + ", updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'" +
                       "WHERE c_bpartner_id = " + POSAddcustomerApiStringResponce.oldBusinessPartnerId + "; ", connection);
                    cmd_Update_post_flag_customer_id.ExecuteReader();
                    connection.Close();
                    log.Info("Posted Add New customer  Flag Updated|#partnerid: " + POSAddcustomerApiStringResponce.businessPartnerId + "");
                }
                else
                {
                    connection.Close();
                    connection.Open();

                    MessageBox.Show("Problem in Posting Product Details!!");
                    log.Error("Posting Invoice Failed|Responce Code: " + POSAddcustomerApiStringResponce.responseCode);
                    log.Error("----------------JSON Request--------------");
                    log.Error(val);
                    log.Error("----------------JSON END--------------");
                }
            }

            clearcustfields();
            //            {
            //                "responseCode": "200",
            //    "oldBusinessPartnerId": -101,
            //    "businessPartnerId": 1015810
            //}

        }
        private void Btncustcancel_Click(object sender, RoutedEventArgs e)
        {
            clearcustfields();
            Back_OR_Esc();
        }

        private void BackTOCart_from_side_menu__Customer_Click(object sender, RoutedEventArgs e)
        {
            clearcustfields();
            Back_OR_Esc();
        }

        private void popupCustomerList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                txttmpBpartnerid.Text = lstbpid.ToString();
                txtCustomer.Text = customeername;
                txtCustomermobile.Text = mobile;
                lstbpid = 0;
                customeername = "";
                mobile = "";
                MainPage.IsEnabled = true;
                popupCustomerList.IsOpen = false;
                return;
            }
        }
        int lstbpid = 0;
        string customeername = "";
        string mobile = "";
        private void popupCustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                if (popuplstcustomer.SelectedItem != null)
                {
                    lstbpid = (popuplstcustomer.SelectedItem as CustomerList).Bp_partner_id;
                    customeername = (popuplstcustomer.SelectedItem as CustomerList).firstName;
                    mobile = (popuplstcustomer.SelectedItem as CustomerList).mobilenummber;

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

            //int index = this.popuplstcustomer.SelectedIndex;
            //int CurrentRow = 0;
            //foreach (var item in popuplstcustomer.Items)
            //{
            //    if (index == CurrentRow)
            //    {
            //        // string CR_Number = ((Restaurant_Pos.Pages.RetailPage.CustomerList)item).CR_Number;
            //        txttmpBpartnerid.Text=((Restaurant_Pos.Pages.RetailPage.CustomerList)item).Bp_partner_id.ToString();
            //        txtCustomer.Text = ((Restaurant_Pos.Pages.RetailPage.CustomerList)item).firstName;
            //        txtCustomermobile.Text = ((Restaurant_Pos.Pages.RetailPage.CustomerList)item).mobilenummber;
            //        MainPage.IsEnabled = true;
            //        popupCustomerList.IsOpen = false;
            //        return;
            //    }
            //}
        }

        private void popup_lstcust_Bind_customer_Search()
        {

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            CustomerList_dataSource.Clear();
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand("select  c_bpartner_id as number,name,coalesce(lastname, '') as lastname,coalesce(mobile_number, '') as mobilenumber,searchkey||c_bpartner_id || name|| COALESCE(mobile_number, '') as searchfield from c_bpartner  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;", connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                CustomerList_dataSource.Add(new CustomerList { Bp_partner_id = _get_c_products.GetInt32(0), firstName = _get_c_products.GetString(1), lastName = _get_c_products.GetString(2), mobilenummber = _get_c_products.GetString(3).ToString(), SearchField = _get_c_products.GetString(4) });

            }
            connection.Close();
            popuplstcustomer.ItemsSource = CustomerList_dataSource;
            txtpopupcustomerSearch.Text = string.Empty;
            ICollectionView view1 = CollectionViewSource.GetDefaultView(CustomerList_dataSource);

            new TextSearchFilter(view1, this.txtpopupcustomerSearch, popuplstcustomer);
            // lstitems.DataContext = quickdataSource;
        }

        private void PopupCustomerList_Closed(object sender, EventArgs e)
        {
            popupCustomerList.IsOpen = false;
            MainPage.IsEnabled = true;
            Back_OR_Esc();
        }

        private void popupCustomerList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                popuplstcustomer.SelectedItems.Clear();

                ListViewItem item = sender as ListViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    popuplstcustomer.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }

        private void popupCustomerList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in popuplstcustomer.Items)
                {
                    ListViewItem i = (ListViewItem)lstproducts.ItemContainerGenerator.ContainerFromItem(item);
                    if (i != null)
                    {
                        //Seek out the ContentPresenter that actually presents our DataTemplate
                        ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                        TextBlock txtBpartnerid = (TextBlock)i.ContentTemplate.FindName("txtBpartnerid", contentPresenter);
                        TextBlock txtFirstName = (TextBlock)i.ContentTemplate.FindName("txtFirstName", contentPresenter);
                        TextBlock txLastName = (TextBlock)i.ContentTemplate.FindName("txLastName", contentPresenter);
                        TextBlock txtMobileNo = (TextBlock)i.ContentTemplate.FindName("txtMobileNo", contentPresenter);
                        txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    }
                }

                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {


                    TextBlock txtBpartnerid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtBpartnerid") as TextBlock;
                    TextBlock txtFirstName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtFirstName") as TextBlock;
                    TextBlock txLastName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtLastName") as TextBlock;
                    TextBlock txtMobileNo = GetChildrenByType(selectedItem, typeof(TextBlock), "txtMobileNo") as TextBlock;
                    txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    if (txtBpartnerid != null)
                    {
                        AD_bpartner_Id = long.Parse(txtBpartnerid.Text);
                        txtCustomer.Text = txtFirstName.Text;
                        txtCustomermobile.Text = txtMobileNo.Text;
                        MainPage.IsEnabled = true;
                        popupCustomerList.IsOpen = false;
                        return;
                        // view_Product(txtprodId.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }

        private void Lstcustomer_PreviewKeyUp_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    foreach (var item in lstcustomer.Items)
                    {
                        ListViewItem i = (ListViewItem)lstcustomer.ItemContainerGenerator.ContainerFromItem(item);
                        if (i != null)
                        {
                            //Seek out the ContentPresenter that actually presents our DataTemplate
                            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                            TextBlock txtBpartnerid = (TextBlock)i.ContentTemplate.FindName("txtBpartnerid", contentPresenter);
                            TextBlock txtFirstName = (TextBlock)i.ContentTemplate.FindName("txtFirstName", contentPresenter);
                            TextBlock txtLastName = (TextBlock)i.ContentTemplate.FindName("txtLastName", contentPresenter);
                            TextBlock txtMobileNo = (TextBlock)i.ContentTemplate.FindName("txtMobileNo", contentPresenter);
                            txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        }
                    }

                    ListViewItem selectedItem = e.OriginalSource as ListViewItem;

                    if (selectedItem.Content != null && selectedItem.IsSelected)
                    {


                        TextBlock txtBpartnerid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtBpartnerid") as TextBlock;
                        TextBlock txtFirstName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtFirstName") as TextBlock;
                        TextBlock txtLastName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtLastName") as TextBlock;
                        TextBlock txtMobileNo = GetChildrenByType(selectedItem, typeof(TextBlock), "txtMobileNo") as TextBlock;
                        txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        if (txtBpartnerid != null)
                        {
                            // txttmpBpartnerid.Text = txtBpartnerid.Text;
                            view_Customer(txtBpartnerid.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Lstcustomer_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int index = lstcustomer.SelectedIndex;
            int CurrentRow = 0;
            foreach (var item in lstcustomer.Items)
            {
                if (index == CurrentRow)
                {
                    int bpartnerid = ((Restaurant_Pos.Pages.RetailPage.CustomerList)item).Bp_partner_id;
                    view_Customer(bpartnerid.ToString());
                    return;
                }
                CurrentRow++;
            }
        }

        private void TxtProdSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            popuplstcustomer.Focus();
            popuplstcustomer.SelectedIndex = 0;
        }

        private void Pmt_Return_Click(object sender, RoutedEventArgs e)
        {
            if (iteamProduct.Items.Count > 0)
            {
                Change_Activity_of_Payment_buttons("Return");
            }
            Pmt_Return_View();
        }
        private void Pmt_Return_View()
        {
            UpdateLayout();
            foreach (var item in iteamProduct.Items)
            {
                ListViewItem i = (ListViewItem)iteamProduct.ItemContainerGenerator.ContainerFromItem(item);
                if (i != null)
                {
                    //Seek out the ContentPresenter that actually presents our DataTemplate
                    ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                    StackPanel stkpriceedit = (StackPanel)i.ContentTemplate.FindName("stkpriceedit", contentPresenter);
                    stkpriceedit.Visibility = Visibility.Visible;
                    TextBox Amount_Cart_LineIteam = (TextBox)i.ContentTemplate.FindName("Amount_Cart_LineIteam", contentPresenter);
                    Amount_Cart_LineIteam.Visibility = Visibility.Hidden;
                    TextBox Cart_LineIteam_Price_tx = (TextBox)i.ContentTemplate.FindName("Cart_LineIteam_Price_tx", contentPresenter);
                    Cart_LineIteam_Price_tx.Text = Amount_Cart_LineIteam.Text;
                }
            }
            if (iteamProduct.Items.Count > 0)
            {
                iteamProduct.SelectedIndex = 0;
                iteamProduct.Focus();
                iteamProduct.SelectedItem = items.FirstOrDefault();
                iteamProduct.SelectedIndex = 0;
            }
        }
        private void Reset_Pmt_Return_View()
        {
            UpdateLayout();
            foreach (var item in iteamProduct.Items)
            {
                ListViewItem i = (ListViewItem)iteamProduct.ItemContainerGenerator.ContainerFromItem(item);
                if (i != null)
                {
                    //Seek out the ContentPresenter that actually presents our DataTemplate
                    ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                    StackPanel stkpriceedit = (StackPanel)i.ContentTemplate.FindName("stkpriceedit", contentPresenter);
                    stkpriceedit.Visibility = Visibility.Hidden;
                    TextBox Amount_Cart_LineIteam = (TextBox)i.ContentTemplate.FindName("Amount_Cart_LineIteam", contentPresenter);
                    Amount_Cart_LineIteam.Visibility = Visibility.Visible;
                    // TextBox Cart_LineIteam_Price_tx = (TextBox)i.ContentTemplate.FindName("Cart_LineIteam_Price_tx", contentPresenter);
                    // Cart_LineIteam_Price_tx.Text = Amount_Cart_LineIteam.Text;
                }
            }
            if (iteamProduct.Items.Count > 0)
            {
                iteamProduct.SelectedIndex = 0;
                iteamProduct.Focus();
                iteamProduct.SelectedItem = items.FirstOrDefault();
                iteamProduct.SelectedIndex = 0;
            }
        }
        private void Price_Cart_LineIteam_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var item = (sender as Button).DataContext;
                int index = iteamProduct.Items.IndexOf(item);
                ListView_Index_No = index;
                iteamProduct.SelectedIndex = ListView_Index_No;
                Button button = (Button)sender;
                StackPanel StackPanel = (StackPanel)button.Content;
                TextBox _TextBlock = new TextBox();

                LineIteam_Up_Rf = "Return_Price_Cart_LineIteam";
                ValueChanger_key_pad.Focusable = true;
                Keyboard.Focus(ValueChanger_key_pad);
                iteamProduct.SelectedIndex = ListView_Index_No;
                foreach (var child in StackPanel.Children)
                {
                    if (child.GetType().ToString() == "System.Windows.Controls.TextBox")
                    {
                        _TextBlock = (TextBox)child;
                        if (_TextBlock.Text != "")
                        {
                            ValueChanger_key_pad.Text = _TextBlock.Text;
                        }
                    }

                }
                // ValueChanger_key_pad.Text = ((Restaurant_Pos.Product)iteamProduct.SelectedItem).Discount.ToString();
                ValueChanger_key_pad.SelectAll();
                _selectedtext_valuecharger = true;
                GeneralTextPopUp.Visibility = Visibility.Visible;
                GeneralTextPopUp.Text = "Return Price";
                popUpText.Visibility = Visibility.Hidden;
                ToggleButton_for_DisAndPrice.Visibility = Visibility.Hidden;
                ToggleButton_for_DisAndPrice2.Visibility = Visibility.Hidden;
                // } 
                //GeneralTextPopUp.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void View_Category_List_Page()
        {
            Menu_Page_Category.Visibility = Visibility.Visible;
            grdCategoryviewlist.Visibility = Visibility.Visible;
            Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
            grdCategoryadd.Visibility = Visibility.Hidden;
            dock_edit_add_buttons_Category.Visibility = Visibility.Visible;
            Load_CategoryList(true);
            txtCategorySearch.Focus();
            Keyboard.Focus(txtCategorySearch);
            if (lstCategory.Items.Count > 0)
            {

                lstCategory.SelectedIndex = 0;
            }
            var MyList = (ListView)lstCategory;
            //standard
            if (MyList.Items.Count > 0)
            {
                foreach (var category in MyList.ItemsSource)
                {

                    view_Category(((CategoryList)category).categoryID.ToString());
                    return;
                    //do stuff with it modify the entry text whatever you want
                }
            }
        }
        private void Add_New_category_Page()
        {
            clearcategoryfields();
            Menu_Page_Category.Visibility = Visibility.Visible;
            grdCategoryviewlist.Visibility = Visibility.Hidden;
            grdCategoryadd.Visibility = Visibility.Visible;
            dock_edit_add_buttons_Category.Visibility = Visibility.Hidden;
            btncategorySavetxt.Text = "Save";
            txtaddCategoryname_.Focus();
            //txtaddcrnumber_.Focus();

        }

        private void clearcategoryfields()
        {
            txtaddCategoryname_.Text = "";
            txtaddCategoydesc_.Text = "";

        }

        private void view_Category(string categoryId)
        {
            txttmpCategoryid.Text = categoryId;


            string query = "select m_product_category_id,name,searchkey from m_product_category  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + " and m_product_category_id= '" + txttmpCategoryid.Text + "';";

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_category = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_category = cmd_fetch_category.ExecuteReader();
            while (_get_c_category.Read())
            {

                lblCategoryid_.Text = _get_c_category.GetString(0);
                //  Back_with_product.Text = _get_c_products.GetString(0);
                lblCategoryName_.Text = _get_c_category.GetString(1);
                lblCategoryDesc_.Text = _get_c_category.GetString(2);
            }
            connection.Close();
        }
        private void edit_category()
        {
            Menu_Page_Category.Visibility = Visibility.Visible;
            grdCategoryviewlist.Visibility = Visibility.Hidden;
            grdCategoryadd.Visibility = Visibility.Visible;
            dock_edit_add_buttons_Category.Visibility = Visibility.Hidden;
            btncategorySavetxt.Text = "Update";
            string query = "select name,searchkey from m_product_category  WHERE ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + " and m_product_category_id= '" + txttmpCategoryid.Text + "';";


            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            NpgsqlCommand cmd_fetch_customer = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_customer = cmd_fetch_customer.ExecuteReader();
            while (_get_c_customer.Read())
            {

                txtaddCategoryname_.Text = _get_c_customer.GetString(0);
                txtaddCategoydesc_.Text = _get_c_customer.GetString(1);
                //  Back_with_product.Text = _get_c_products.GetString(0);


            }
            connection.Close();
        }
        private void BackTOCart_from_side_menu__Category_Click(object sender, RoutedEventArgs e)
        {
            clearcategoryfields();
            Back_OR_Esc();
        }

        private void BtnEdit_Category_Click(object sender, RoutedEventArgs e)
        {
            edit_category();
        }

        private void LstCategory_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    foreach (var item in lstCategory.Items)
                    {
                        ListViewItem i = (ListViewItem)lstCategory.ItemContainerGenerator.ContainerFromItem(item);
                        if (i != null)
                        {
                            //Seek out the ContentPresenter that actually presents our DataTemplate
                            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);


                            TextBlock txtCategorysearchkey = (TextBlock)i.ContentTemplate.FindName("txtCategorysearchkey", contentPresenter);
                            TextBlock txtCategoryName = (TextBlock)i.ContentTemplate.FindName("txtCategoryName", contentPresenter);
                            txtCategorysearchkey.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtCategoryName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        }
                    }

                    ListViewItem selectedItem = e.OriginalSource as ListViewItem;

                    if (selectedItem.Content != null && selectedItem.IsSelected)
                    {

                        TextBlock txtCategoryId = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategoryId") as TextBlock;
                        TextBlock txtCategorysearchkey = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategorysearchkey") as TextBlock;
                        TextBlock txtCategoryName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategoryName") as TextBlock;
                        txtCategorysearchkey.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtCategoryName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        if (txtCategoryId != null)
                        {
                            // txttmpBpartnerid.Text = txtBpartnerid.Text;
                            view_Category(txtCategoryId.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void LstCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                int index = this.lstCategory.SelectedIndex;
                int CurrentRow = 0;
                foreach (var item in lstCategory.Items)
                {
                    if (index == CurrentRow)
                    {
                        long categoryID = ((Restaurant_Pos.Pages.RetailPage.CategoryList)item).categoryID;
                        view_Category(categoryID.ToString());
                        return;
                    }
                    CurrentRow++;
                }

            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void categoryListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                lstCategory.SelectedItems.Clear();

                ListViewItem item = sender as ListViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    lstCategory.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void categoryListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in lstCategory.Items)
                {
                    ListViewItem i = (ListViewItem)lstCategory.ItemContainerGenerator.ContainerFromItem(item);
                    if (i != null)
                    {
                        //Seek out the ContentPresenter that actually presents our DataTemplate
                        ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                        TextBlock txtCategorysearchkey = (TextBlock)i.ContentTemplate.FindName("txtCategorysearchkey", contentPresenter);
                        TextBlock txtCategoryName = (TextBlock)i.ContentTemplate.FindName("txtCategoryName", contentPresenter);
                        txtCategorysearchkey.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtCategoryName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    }
                }

                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {

                    TextBlock txtCategoryid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategoryid") as TextBlock;
                    TextBlock txtCategorysearchkey = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategorysearchkey") as TextBlock;
                    TextBlock txtCategoryName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtCategoryName") as TextBlock;
                    txtCategorysearchkey.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtCategoryName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    if (txtCategoryid != null)
                    {
                        view_Category(txtCategoryid.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void Btnaddcategory_Click(object sender, RoutedEventArgs e)
        {
            Add_New_category_Page();
        }

        private void Btncategorysave_Click(object sender, RoutedEventArgs e)
        {
            save_update_category_Data();
        }

        private void Btncategorycancel_Click(object sender, RoutedEventArgs e)
        {
            clearcategoryfields();
            grdCategoryviewlist.Visibility = Visibility.Visible;
            grdCategoryadd.Visibility = Visibility.Hidden;
        }
        private void save_update_category_Data()
        {
            string str_category_Name = "";
            string str_category_desc = "";

            if (txtaddCategoryname_.Text == "")
            {
                MessageBox.Show("Category Name Cant Be Empty");
                txtaddCategoryname_.Focus();
                return;
            }
            else
            {
                str_category_Name = txtaddCategoryname_.Text;
            }

            str_category_desc = txtaddCategoydesc_.Text;

            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            if (btncategorySavetxt.Text == "Save")
            {
                if (check_category_exist(txtaddCategoryname_.Text))
                {
                    MessageBox.Show("Category Already Exist");
                    txtaddCategoryname_.Focus();
                    return;

                }
                else
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand("select categoryid-1 from m_pos_sequence WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + "", connection);


                    // Execute the query and obtain the value of the first column of the first row
                    Int32 cpid = Convert.ToInt32(command.ExecuteScalar());
                    txttmpCategoryid.Text = cpid.ToString();
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_update_pr_no_m_product_price = new NpgsqlCommand("UPDATE m_pos_sequence SET categoryid =" + txttmpCategoryid.Text + " WHERE clientId=" + AD_Client_ID + " and orgId=" + AD_ORG_ID + ";", connection);
                    NpgsqlDataReader _update__pr_sequenc_no = cmd_update_pr_no_m_product_price.ExecuteReader();
                    connection.Close();
                    int Sequenc_id = 9;
                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("SELECT ad_sequence_id,name, incrementno, startno,currentnext FROM ad_sequence where name = 'ee_msrmanager';", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();

                    if (_get__Ad_sequenc_no.Read())
                    {
                        Sequenc_id = _get__Ad_sequenc_no.GetInt32(4) + 1;
                    }
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + Sequenc_id + " WHERE name = 'ad_client';", connection);
                    NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    connection.Close();

                    connection.Open();
                    NpgsqlCommand INSERTCategorydetails = new NpgsqlCommand("INSERT INTO public.m_product_category(id, ad_client_id, ad_org_id," +
                        " m_product_category_id, isactive, created, createdby,updated,updatedby, name, searchkey,arabicname, image, isdefault)" +
                         " VALUES(" + Sequenc_id + ",'" + AD_Client_ID + "','" + AD_ORG_ID + "','" + txttmpCategoryid.Text + "','Y','" + DateTime.Now + "','" + AD_USER_ID + "','" + DateTime.Now + "','" + AD_USER_ID + "','" + str_category_Name + "','" + str_category_desc + "','','','Y'); ", connection);
                    INSERTCategorydetails.ExecuteNonQuery();
                    MessageBox.Show("Category Details Has Been Saved");
                    txtaddCategoryname_.Focus();
                    connection.Close();
                }

            }
            if (btncategorySavetxt.Text == "Update")
            {
                connection.Open();
                NpgsqlCommand UPDATE_c_categorydetails = new NpgsqlCommand("UPDATE public.m_product_category set " +
                " name='" + str_category_Name + "', searchkey='" + str_category_desc + "', updated='" + DateTime.Now + "', updatedby='" + AD_USER_ID + "' " +
                " WHERE m_product_category_id = '" + txttmpCategoryid.Text + "'", connection);
                UPDATE_c_categorydetails.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Category Details Has Been Updated");
                View_Category_List_Page();

            }

            connection.Close();
            JObject json = new JObject(
                               new JProperty("macAddress", DeviceMacAdd),
                                      new JProperty("username", AD_UserName),
                                     new JProperty("password", AD_UserPassword),
                                      new JProperty("userId", AD_USER_ID),
                                     new JProperty("clientId", AD_Client_ID),
                                     new JProperty("roleId", AD_ROLE_ID),
                                    new JProperty("orgId", AD_ORG_ID),
                                    new JProperty("warehouseId", AD_Warehouse_Id),
                                    new JProperty("businessPartnerId", AD_bpartner_Id),
                                    new JProperty("sessionId", AD_SessionID),
                                   new JProperty("remindMe", "N"),
                                    new JProperty("version", "1.0"),
                                    new JProperty("appName", "POS"),
                                    new JProperty("showImage", "Y"),
                                    new JProperty("SyncedTime", "0"),
                                    new JProperty("operation", "POSAddCategory"),
                                   new JProperty("categoryId", txttmpCategoryid.Text),
                                    new JProperty("categoryName", str_category_Name)
                                    //,new JProperty("categoryImage", str_crNumber) 
                                    );
            var val = json.ToString();
            int CheckApiError = 0;
            try
            {
                POSAddcategoryApiStringResponce = PostgreSQL.ApiCallPost(val);
                CheckApiError = 1;
            }
            catch
            {

                CheckApiError = 0;
                log.Error("POSAddCategory: Server Error");
                log.Error("----------------JSON Request--------------");
                log.Error(val);
                log.Error("----------------JSON END------------------");
            }
            if (CheckApiError == 1)
            {
                POSAddcategoryApiStringResponce = JsonConvert.DeserializeObject(POSAddcategoryApiStringResponce);
                log.Info("POSAdddCategoryApiStringResponce: " + POSAddcategoryApiStringResponce + "");
                if (POSAddcategoryApiStringResponce.responseCode == "200")
                {
                    connection.Close();
                    connection.Open();
                    NpgsqlCommand cmd_Update_post_flag_category_id = new NpgsqlCommand("UPDATE public.m_product_category " +
                       "SET m_product_category_id =  " + POSAddcategoryApiStringResponce.categoryId + ", updated = ' " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'" +
                       "WHERE m_product_category_id = " + POSAddcategoryApiStringResponce.oldCategoryId + "; ", connection);
                    cmd_Update_post_flag_category_id.ExecuteReader();
                    connection.Close();
                    log.Info("Posted Add New category Flag Updated|#categoryid: " + POSAddcategoryApiStringResponce.businessPartnerId + "");
                }
                else
                {
                    connection.Close();
                    connection.Open();

                    MessageBox.Show("Problem in Posting category Details!!");
                    log.Error("Posting category Failed|Responce Code: " + POSAddcategoryApiStringResponce.responseCode);
                    log.Error("----------------JSON Request--------------");
                    log.Error(val);
                    log.Error("----------------JSON END--------------");
                }
            }
            clearcategoryfields();
            //            {
            //                "responseCode": "200",
            //    "oldBusinessPartnerId": -101,
            //    "businessPartnerId": 1015810
            //}

        }

        private void Side_Menu_Customer_Credit_Pay_Click(object sender, RoutedEventArgs e)
        {
            View_Customer_Pay_List_Page("C");
        }
        private void Side_Menu_Vendor_Credit_Pay_Click(object sender, RoutedEventArgs e)
        {
            View_Customer_Pay_List_Page("V");
        }
        private void BackTOCart_from_side_menu__Customer_Payment_page_Click(object sender, RoutedEventArgs e)
        {
            Back_OR_Esc();
        }
        private void lstcust_Bind_customer_pay_Search(string type)
        {

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            connection.Open();
            CustomerList_dataSource.Clear();
            string query = "";
            if(type=="C")
            {
                query = "select  c_bpartner_id as number,name,coalesce(lastname, '') as lastname,coalesce(mobile_number, '') as mobilenumber,(c_bpartner_id || '    |    ' || name ) as searchfield from c_bpartner  WHERE iscustomer='Y' AND ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;";
            }
            if (type == "V")
            {
                query = "select  c_bpartner_id as number,name,coalesce(lastname, '') as lastname,coalesce(mobile_number, '') as mobilenumber,(c_bpartner_id || '    |    ' || name ) as searchfield from c_bpartner  WHERE isvendor='Y' AND ad_client_id = " + AD_Client_ID + " AND ad_org_id = " + AD_ORG_ID + "  order by name asc  ;";
            }
            NpgsqlCommand cmd_fetch_products = new NpgsqlCommand(query, connection);
            NpgsqlDataReader _get_c_products = cmd_fetch_products.ExecuteReader();
            while (_get_c_products.Read())
            {
                CustomerList_dataSource.Add(new CustomerList { Bp_partner_id = _get_c_products.GetInt32(0), firstName = _get_c_products.GetString(1), lastName = _get_c_products.GetString(2), mobilenummber = _get_c_products.GetString(3).ToString(), SearchField = _get_c_products.GetString(4) });

            }
            connection.Close();
            lstcustomer_pay.ItemsSource = CustomerList_dataSource;
            txtcustomerSearch.Text = string.Empty;
            // txtprodTotalsearchcount.Text = dataSource.Count.ToString();
            ICollectionView view1 = CollectionViewSource.GetDefaultView(CustomerList_dataSource);

            new TextSearchFilter(view1, this.txtcustomerSearch, lstcustomer_pay);
            // lstitems.DataContext = quickdataSource;
        }
        private void View_Customer_Pay_List_Page(string Type)
        {
            if(Type=="C")
            {
                BackTOCart_from_side_menu_Customer_Payment_page.Text = "Customer Credit Pay";
            }
            if (Type == "V")
            {
                BackTOCart_from_side_menu_Customer_Payment_page.Text = "Vendor Credit Pay";
            }
            Menu_Page_Customer_Payment.Visibility = Visibility.Visible;
            grdcust_pay_viewlist.Visibility = Visibility.Visible;
            Tittle_Bar_Right_Content.Visibility = Visibility.Hidden;
            grdcust_payment_details.Visibility = Visibility.Hidden;
            lstcust_Bind_customer_pay_Search(Type);
            txtcust_pay_Search.Focus();
            Keyboard.Focus(txtcust_pay_Search);
            if (lstcustomer_pay.Items.Count > 0)
            {

                lstcustomer_pay.SelectedIndex = 0;
            }
            var MyList = (ListView)lstcustomer_pay;
            //standard
            if (MyList.Items.Count > 0)
            {
                foreach (var customer in MyList.ItemsSource)
                {

                    view_invoice(((CustomerList)customer).Bp_partner_id.ToString(), Type);
                    return;
                    //do stuff with it modify the entry text whatever you want
                }
            }
            else
            {
                lstcustomer_invoice.ItemsSource = null;
            }
        }
        private void view_invoice(string Bp_partner_id,string Type)
        {
            cust_invoiceList.Clear();
          //  dockpnlinvoiceheader.Visibility = Visibility.Hidden;
           // dockpnlselectallinvoice.Visibility = Visibility.Hidden;
            grdcust_payment_details.Visibility = Visibility.Hidden;
            txtinvoiceAmt.Text = "";
            string isCust = "N";
            string isVend = "N";
            if(Type=="C")
            {
                isCust = "Y";
            }
            if (Type == "V")
            {
                isVend = "Y";
            }
            JObject rss = new JObject(
                new JProperty("operation", "GetCreditInvoices"),
                new JProperty("username", AD_UserName),
                new JProperty("password", AD_UserPassword),
                new JProperty("clientId", AD_Client_ID.ToString()),
                new JProperty("orgId", AD_ORG_ID.ToString()),
                new JProperty("userId", AD_USER_ID.ToString()),
                new JProperty("warehouseId", AD_Warehouse_Id.ToString()),
                new JProperty("macAddress", DeviceMacAdd),
                new JProperty("version", "1.0"),
                new JProperty("appName", "POS"),
                new JProperty("businessPartnerId", Bp_partner_id),
                new JProperty("isCustomer", isCust),
                new JProperty("isVendor", isVend)
            );
            #region Posting to Server

            int CheckApiError = 0;
            var val = rss.ToString();
            //log.Info("----------------JSON Request--------------");
            //log.Info(val);
            //log.Info("----------------JSON END--------------");
            try
            {
                POSGetOrderListCustomerApiJSONResponce = PostgreSQL.ApiCallPost(val);
                CheckApiError = 1;
            }
            catch(Exception e)
            {
                CheckApiError = 0;
                log.Error("POSReleaseOrderApi: Server Error");
                log.Error("----------------JSON Request--------------");
                log.Error(e);
                log.Error(val);
                log.Error("----------------JSON END------------------");
            }
            if (CheckApiError == 1)
            {
                POSGetOrderListCustomerApiJSONResponce = JsonConvert.DeserializeObject(POSGetOrderListCustomerApiJSONResponce);
                log.Info("POSReleaseOrderApiJSONResponce: " + POSGetOrderListCustomerApiJSONResponce + "");
                if (POSGetOrderListCustomerApiJSONResponce.responseCode == "200")
                {
                    if (POSGetOrderListCustomerApiJSONResponce.invoiceList != null)
                    {
                        foreach (var invoiceList in POSGetOrderListCustomerApiJSONResponce.invoiceList)
                        {

                            cust_invoiceList.Add(new invoiceList
                            {
                                invoiceId = invoiceList.invoiceId,
                                documentNo = invoiceList.documentNo,
                                posId = invoiceList.posId,
                                invoiceDate = Convert.ToDateTime(invoiceList.invoiceDate).ToString("dd MMM yyyy"),
                                grandTotal =  Convert.ToDecimal(invoiceList.grandTotal).ToString("0.00"),
                                lineCount = invoiceList.lineCount + " items"
                            });

                        }

                        lstcustomer_invoice.ItemsSource = null;
                        lstcustomer_invoice.ItemsSource = cust_invoiceList;
                        if (cust_invoiceList.Count > 0)
                        {
                           // dockpnlinvoiceheader.Visibility = Visibility.Visible;
                            //dockpnlselectallinvoice.Visibility = Visibility.Visible;
                            grdcust_payment_details.Visibility = Visibility.Visible;
                            txtinvoiceAmt.Focus();
                            Keyboard.Focus(txtinvoiceAmt);
                        }


                    }
                    else
                    {
                        log.Error("Posting Invoice Failed|Responce Code: " + POSGetOrderListCustomerApiJSONResponce.responseCode);
                        log.Error("----------------JSON Request--------------");
                        log.Error(val);
                        log.Error("----------------JSON END--------------");
                    }
                }

                #endregion Posting to Server

            }
            else
            {
                MessageBox.Show("Error in Fethcing Invoice");
                lstcustomer_invoice.ItemsSource = null;
            }
        }
        private void Lstcustomer_pay_list_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    foreach (var item in lstcustomer_pay.Items)
                    {
                        ListViewItem i = (ListViewItem)lstcustomer_pay.ItemContainerGenerator.ContainerFromItem(item);
                        if (i != null)
                        {
                            //Seek out the ContentPresenter that actually presents our DataTemplate
                            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                            //   TextBlock txtinvoiceId = (TextBlock)i.ContentTemplate.FindName("txtinvoiceId", contentPresenter);
                            TextBlock txtBpartnerid = (TextBlock)i.ContentTemplate.FindName("txtBpartnerid", contentPresenter);
                            TextBlock txtFirstName = (TextBlock)i.ContentTemplate.FindName("txtFirstName", contentPresenter);
                            TextBlock txtLastName = (TextBlock)i.ContentTemplate.FindName("txtLastName", contentPresenter);
                            TextBlock txtMobileNo = (TextBlock)i.ContentTemplate.FindName("txtMobileNo", contentPresenter);
                            txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        }
                    }

                    ListViewItem selectedItem = e.OriginalSource as ListViewItem;

                    if (selectedItem.Content != null && selectedItem.IsSelected)
                    {

                        TextBlock txtBpartnerid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtBpartnerid") as TextBlock;
                        TextBlock txtFirstName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtFirstName") as TextBlock;
                        TextBlock txtLastName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtLastName") as TextBlock;
                        TextBlock txtgrandTotal = GetChildrenByType(selectedItem, typeof(TextBlock), "txtgrandTotal") as TextBlock;
                        TextBlock txtMobileNo = GetChildrenByType(selectedItem, typeof(TextBlock), "txtMobileNo") as TextBlock;

                        // txtinvoiceId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtLastName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtMobileNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        if (txtBpartnerid != null)
                        {
                            // txttmpBpartnerid.Text = txtBpartnerid.Text;
                            if(BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
                            {
                                view_invoice(txtBpartnerid.Text,"C");
                            }
                            if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
                            {
                                view_invoice(txtBpartnerid.Text,"V");
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }
        private void Lstcustomer_pay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int index = lstcustomer_pay.SelectedIndex;
            int CurrentRow = 0;
            foreach (var item in lstcustomer_pay.Items)
            {
                if (index == CurrentRow)
                {
                    int bpartnerid = ((Restaurant_Pos.Pages.RetailPage.CustomerList)item).Bp_partner_id;
                    if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
                    {
                        view_invoice(bpartnerid.ToString(), "C");
                    }
                    if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
                    {
                        view_invoice(bpartnerid.ToString(), "V");
                    }
                    
                    return;
                }
                CurrentRow++;
            }
        }




        private void lstcustomer_pay__PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                lstcustomer_pay.SelectedItems.Clear();

                ListViewItem item = sender as ListViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    lstcustomer_pay.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void lstcustomer_pay_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in lstcustomer_pay.Items)
                {
                    ListViewItem i = (ListViewItem)lstcustomer_pay.ItemContainerGenerator.ContainerFromItem(item);
                    if (i != null)
                    {
                        //Seek out the ContentPresenter that actually presents our DataTemplate
                        ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                        TextBlock txtBpartnerid = (TextBlock)i.ContentTemplate.FindName("txtBpartnerid", contentPresenter);
                        TextBlock txtFirstName = (TextBlock)i.ContentTemplate.FindName("txtFirstName", contentPresenter);
                        txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                    }
                }

                ListViewItem selectedItem = sender as ListViewItem;
                if (selectedItem != null && selectedItem.IsSelected)
                {


                    TextBlock txtBpartnerid = GetChildrenByType(selectedItem, typeof(TextBlock), "txtBpartnerid") as TextBlock;
                    TextBlock txtFirstName = GetChildrenByType(selectedItem, typeof(TextBlock), "txtFirstName") as TextBlock;
                    txtBpartnerid.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    txtFirstName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                    if (txtBpartnerid != null)
                    {
                        if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
                        {
                            view_invoice(txtBpartnerid.Text, "C");
                        }
                        if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
                        {
                            view_invoice(txtBpartnerid.Text, "V");
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

       

        private void Lstcustomer_invoice_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Down || e.Key == Key.Up)
                {
                    foreach (var item in lstcustomer_invoice.Items)
                    {
                        ListViewItem i = (ListViewItem)lstcustomer_invoice.ItemContainerGenerator.ContainerFromItem(item);
                        if (i != null)
                        {
                            //Seek out the ContentPresenter that actually presents our DataTemplate
                            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

                            //   TextBlock txtinvoiceId = (TextBlock)i.ContentTemplate.FindName("txtinvoiceId", contentPresenter);
                            TextBlock txtdocumentNo = (TextBlock)i.ContentTemplate.FindName("txtdocumentNo", contentPresenter);
                            TextBlock txtinvoiceDate = (TextBlock)i.ContentTemplate.FindName("txtinvoiceDate", contentPresenter);
                            TextBlock txtgrandTotal = (TextBlock)i.ContentTemplate.FindName("txtgrandTotal", contentPresenter);
                            TextBlock txtlineCount = (TextBlock)i.ContentTemplate.FindName("txtlineCount", contentPresenter);
                            txtdocumentNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtinvoiceDate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtgrandTotal.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                            txtlineCount.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.TextBrush);
                        }
                    }

                    ListViewItem selectedItem = e.OriginalSource as ListViewItem;

                    if (selectedItem.Content != null && selectedItem.IsSelected)
                    {

                        TextBlock txtinvoiceId = GetChildrenByType(selectedItem, typeof(TextBlock), "txtinvoiceId") as TextBlock;
                        TextBlock txtdocumentNo = GetChildrenByType(selectedItem, typeof(TextBlock), "txtdocumentNo") as TextBlock;
                        TextBlock txtinvoiceDate = GetChildrenByType(selectedItem, typeof(TextBlock), "txtinvoiceDate") as TextBlock;
                        TextBlock txtgrandTotal = GetChildrenByType(selectedItem, typeof(TextBlock), "txtgrandTotal") as TextBlock;
                        TextBlock txtlineCount = GetChildrenByType(selectedItem, typeof(TextBlock), "txtlineCount") as TextBlock;

                        // txtinvoiceId.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtdocumentNo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtinvoiceDate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtgrandTotal.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        txtlineCount.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(CoustomColors.CartBlueBrush);
                        if (txtinvoiceId != null)
                        {
                            // txttmpBpartnerid.Text = txtBpartnerid.Text;
                           // view_invoice(txtinvoiceId.Text);
                            if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
                            {
                                view_invoice(txtinvoiceId.Text, "C");
                            }
                            if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
                            {
                                view_invoice(txtinvoiceId.Text, "V");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }

        }

        private void lstcustomer_invoice__PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lstcustomer_invoice_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        //private void Chkselectallinvoice_Click(object sender, RoutedEventArgs e)
        //{

        //    bool newBool = chkselectallinvoice.IsChecked ?? false;
        //    SelectAll(newBool);
        //}
        //private void SelectAll(bool select)
        //{
        //    Decimal grandTotal = 0;

        //    foreach (var item in lstcustomer_invoice.Items)
        //    {
        //        ListViewItem i = (ListViewItem)lstcustomer_invoice.ItemContainerGenerator.ContainerFromItem(item);
        //        if (i != null)
        //        {
        //            //Seek out the ContentPresenter that actually presents our DataTemplate
        //            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(i);

        //            TextBlock txtgrandTotal = (TextBlock)i.ContentTemplate.FindName("txtgrandTotal", contentPresenter);
        //            CheckBox chkselectinvoice = (CheckBox)i.ContentTemplate.FindName("chkselectinvoice", contentPresenter);
        //            chkselectinvoice.IsChecked = select;
        //            if (select == true)
        //            {
        //                grandTotal += Convert.ToDecimal(txtgrandTotal.Text); 
        //            }
        //        }
        //    }
        //    txtusedamt.Text = "QR "+grandTotal.ToString("0.00");

        //}
        private void KeyPad_invoice_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).Content.ToString() == "." && txtinvoiceAmt.Text.ToString().Contains("."))
                return;

            if (_selectedtext_quickvaluecharger == true)
            {
                txtinvoiceAmt.Text = $"{((Button)e.OriginalSource).Content.ToString()}";
                _selectedtext_quickvaluecharger = false;
            }
            else
            {
                txtinvoiceAmt.Text = $"{txtinvoiceAmt.Text}{((Button)e.OriginalSource).Content.ToString()}";

            }
        }

        private void Invoice_KeyPadErase_Click(object sender, RoutedEventArgs e)
        {
            if(txtinvoiceAmt.Text.Length>0)
            txtinvoiceAmt.Text = txtinvoiceAmt.Text.Remove(txtinvoiceAmt.Text.Length - 1);
        }
    
        //private void Chkselectinvoice_Click(object sender, RoutedEventArgs e)
        //{

        //}
        string totalreturnamout = "";

        private void Lstcustomer_invoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (lstcustomer_invoice.SelectedItem != null)
                {
                    totalreturnamout = (lstcustomer_invoice.SelectedItem as invoiceList).grandTotal;

                }
            }
            catch (Exception ex)
            {
                CrashApp_Alert();
                log.Error(" ===================  Error In Retail POS  =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Retail POS", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }
        }

        private void BtninvoicePay_Click(object sender, RoutedEventArgs e)
        {
              //invoiceList_items = lstcustomer_invoice.SelectedItem as List<invoiceList>;
            if(txtinvoiceAmt.Text=="")
            {
                MessageBox.Show("Enter Payable Amount");
                return;
            }
            invoiceList_items = new List<invoiceList>();
            double payamount = Convert.ToDouble(txtinvoiceAmt.Text);

            foreach (invoiceList item in lstcustomer_invoice.SelectedItems)
            {
                invoiceList_items.Add(item);
            }
            invoiceList_items = invoiceList_items.OrderBy(x => x.invoiceDate).ToList();
            foreach(var item in invoiceList_items)
            {
                if(Convert.ToDouble(item.grandTotal) < payamount)
                {
                    item.payAmount = item.grandTotal;
                    payamount = payamount - Convert.ToDouble(item.grandTotal);

                }
                else
                {
                    item.payAmount = payamount.ToString("0.00");
                    payamount = 0;
                }
            }
            //foreach (var item in lstcustomer_invoice.SelectedItems)
            //{
            //    invoiceList_items.Add(item);
            //}
            string postinvoiceApi = "";
            string IsCust = "C";
            if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
            {
                postinvoiceApi = "PostCreditInvoicePayments";
                IsCust = "Y";
            }
            if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
            {
                postinvoiceApi = "PostVendorInvoicePayments";
                IsCust = "N";
            }
            JObject rss = new JObject();
            rss =    new JObject( 
                              new JProperty("operation", postinvoiceApi),
                              new JProperty("username", AD_UserName),
                              new JProperty("password", AD_UserPassword),
                              new JProperty("clientId", AD_Client_ID),
                               new JProperty("orgId", AD_ORG_ID),
                              new JProperty("cashbookId", AD_CashbookID),
                              new JProperty("userId", AD_USER_ID),
                              new JProperty("businessPartnerId", AD_bpartner_Id),
                               new JProperty("payAmount", txtinvoiceAmt.Text),
                              new JProperty("tenderType", "X"),
                              new JProperty("warehouseId", AD_Warehouse_Id),
                               new JProperty("description", "payment"),
                                new JProperty("macAddress", DeviceMacAdd),
                              new JProperty("version", "1.0"),
                               new JProperty("appName", "POS"),
                              new JProperty("isCustomer", IsCust),
                             new JProperty("invoiceList",
                            new JArray(
                                from p in invoiceList_items
                                select new JObject(
                                    new JProperty("invoiceId", p.invoiceId),
                                    new JProperty("documentNo", p.documentNo),
                                    new JProperty("posId", p.posId),
                                    new JProperty("invoiceDate", p.invoiceDate),
                                    new JProperty("grandTotal", p.grandTotal),
                                    new JProperty("payAmount", p.payAmount)
                                                  )
                                     )//JArray END
            ));

            #region Posting to Server

            int CheckApiError = 0;
            var val = rss.ToString();
            //log.Info("----------------JSON Request--------------");
            //log.Info(val);
            //log.Info("----------------JSON END--------------");
            try
            {
                POSPostInvoiceApiStringResponce = PostgreSQL.ApiCallPost(val);
                CheckApiError = 1;
            }
            catch
            {
                CheckApiError = 0;
                log.Error("PostCreditInvoicePayments: Server Error");
                log.Error("----------------JSON Request--------------");
                log.Error(val);
                log.Error("----------------JSON END------------------");
            }
            if (CheckApiError == 1)
            {
                POSPostInvoiceApiJSONResponce = JsonConvert.DeserializeObject(POSPostInvoiceApiStringResponce);
                log.Info("POSReleaseOrderApiJSONResponce: " + POSPostInvoiceApiJSONResponce + "");
                if (POSPostInvoiceApiJSONResponce.responseCode == "200")
                {
                    if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("CUSTOMER"))
                    {
                        view_invoice(AD_bpartner_Id.ToString(),"C");
                        MessageBox.Show("Customer Invoice has been updated");
                        log.Info("Posted Customer Payment Invoice | receoptNo: " + POSPostInvoiceApiJSONResponce.receiptNo);
                    }
                    if (BackTOCart_from_side_menu_Customer_Payment_page.Text.ToUpper().Contains("VENDOR"))
                    {
                        view_invoice(AD_bpartner_Id.ToString(), "V");
                        MessageBox.Show("Vendor Invoice has been updated");
                        log.Info("Posted Vendor Payment Invoice | receoptNo: " + POSPostInvoiceApiJSONResponce.receiptNo);
                    }
                    
                   
                }
                else
                {
                   

                    log.Error("Posting Invoice Failed|Responce Code: " + POSPostInvoiceApiJSONResponce.responseCode);
                    log.Error("----------------JSON Request--------------");
                    log.Error(val);
                    log.Error("----------------JSON END--------------");
                }
            }

            OrderDetails_items.Clear();

            #endregion Posting to Server

        }

        


        //private void ddlitem_ComboBoxItem_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //        if (e.Key == System.Windows.Input.Key.Enter)
        //    {
        //        Btnaddcategory_Click(sender, e);
        //      //  VisualStateManager.GoToState(this, "KeyNavigation", true);
        //     //   e.Handled = true;
        //     //   return;
        //    }

        //}

        //private void Ddlitem_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (e.OriginalSource.GetType() != typeof(ComboBoxItem))
        //    {
        //        ComboBox cb = (ComboBox)sender;
        //        cb.IsDropDownOpen = true;
        //    }
        //}

        //private void Ddlitem_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (e.OriginalSource.GetType() != typeof(ComboBoxItem))
        //    {
        //        ComboBox cb = (ComboBox)sender;
        //        cb.IsDropDownOpen = false;
        //    }
        //}


        //private void Grid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Grid grd = sender as Grid;
        //    Keyboard.Focus(grd);
        //}
    }

}