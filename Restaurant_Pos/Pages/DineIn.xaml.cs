﻿using log4net;
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
    /// Interaction logic for DineIn.xaml
    /// </summary>
    public partial class DineIn : Page
    {

        #region 
        int _clientId = Int32.Parse(Application.Current.Properties["clientId"].ToString());
        int _OrgId = Int32.Parse(Application.Current.Properties["OrgId"].ToString());
        int _UserID = Int32.Parse(Application.Current.Properties["UserID"].ToString());
        int _sessionID = Int32.Parse(Application.Current.Properties["SessionID"].ToString());
        int _Bpartner_Id = Int32.Parse(Application.Current.Properties["BpartnerId"].ToString());
        int _Warehouse_Id = Int32.Parse(Application.Current.Properties["WarehouseId"].ToString());
        public string selectItemForLineDiscount = null;
        int totalItems = 0;
        double payableAmount = 0;
        bool totalDiscountApplied = false;
        float totalAmount = 0;
        int selectedTable = 0;
        int? invoiceno = null;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<m_TakeAwayProduct> _m_TakeAwayProduct = new List<m_TakeAwayProduct>();
        public List<m_TakeAwayProductCAT> m_TakeAwayProductCAT = new List<m_TakeAwayProductCAT>();
        public List<m_TakeAwayProductRS> _m_TakeAwayProductRS = new List<m_TakeAwayProductRS>();
        public List<m_TakeAwayProductValue> _m_TakeAwayProductValue = new List<m_TakeAwayProductValue>();
        public List<m_floorTables> _m_floor2Tables = new List<m_floorTables>();
        public List<m_floorTables> _m_floor1Tables = new List<m_floorTables>();
        public List<m_TakeAwayProductRS> splitbill = new List<m_TakeAwayProductRS>();
        public string connstring = PostgreSQL.ConnectionString;

        public object PopupCancelOrder { get; private set; }

        #endregion

        #region Public Function
        public DineIn()
        {
            InitializeComponent();
            BindTakeAwayProductCAT();
            calculateSum();
            getTables();

        }
        #endregion

        #region Bind Data Function

        public void BindTakeAwayProduct(int ID)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TakeAwayProduct_GetData = new NpgsqlCommand("select m_product_id,name,image from M_product  where ad_org_id=" + _OrgId + " and m_product_category_id='" + ID + "'  ;", connection);//
                NpgsqlDataReader _TakeAwayProduct_GetData = cmd_TakeAwayProduct_GetData.ExecuteReader();
                _m_TakeAwayProduct.Clear();
                productCat_ListBox.ItemsSource = "";
                while (_TakeAwayProduct_GetData.Read())
                {

                    _m_TakeAwayProduct.Add(new m_TakeAwayProduct()
                    {
                        id = _TakeAwayProduct_GetData.GetInt32(0),
                        Name = _TakeAwayProduct_GetData.GetString(1),
                        ImgPath = "/Restaurant_Pos;component/Resources/Images/Icons/" + _TakeAwayProduct_GetData.GetString(2)

                    });
                }

                connection.Close();

                if (_m_TakeAwayProduct.Count == 0)
                {
                    MessageBox.Show("Please Add Product !");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        productCat_ListBox.ItemsSource = _m_TakeAwayProduct;
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

        public void BindTakeAwayProductCAT()
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TakeAwayProductCAT_GetData = new NpgsqlCommand("select m_product_category_id,Name,image from m_product_category  where ad_org_id=" + _OrgId + "  ;", connection);//
                NpgsqlDataReader _TakeAwayProductCAT_GetData = cmd_TakeAwayProductCAT_GetData.ExecuteReader();
                m_TakeAwayProductCAT.Clear();
                Product_ListBox.ItemsSource = "";
                while (_TakeAwayProductCAT_GetData.Read())
                {

                    m_TakeAwayProductCAT.Add(new m_TakeAwayProductCAT()
                    {
                        id = _TakeAwayProductCAT_GetData.GetInt32(0),
                        Name = _TakeAwayProductCAT_GetData.GetString(1),
                        ImgPath = "/Restaurant_Pos;component/Resources/Images/Icons/" + _TakeAwayProductCAT_GetData.GetString(2)

                    });
                }

                connection.Close();

                if (m_TakeAwayProductCAT.Count == 0)
                {
                    MessageBox.Show("Product Category Not Found!");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Product_ListBox.ItemsSource = m_TakeAwayProductCAT;
                    });
                }
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product Category Not Found!   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product Category Not Found!", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }

        public void BindTakeAwayProductRS(int ID)
        {

            try
            {

                NpgsqlConnection connection = new NpgsqlConnection(connstring);
                connection.Open();
                NpgsqlCommand cmd_TakeAwayProductCAT_GetData = new NpgsqlCommand("select m_product_id,name,currentcostprice from m_product  where ad_org_id=" + _OrgId + " and m_product_id='" + ID + "' ;", connection);//
                NpgsqlDataReader _TakeAwayProductCAT_GetData = cmd_TakeAwayProductCAT_GetData.ExecuteReader();

                price_ListBox.ItemsSource = "";
                while (_TakeAwayProductCAT_GetData.Read())
                {


                    _m_TakeAwayProductRS.Add(new m_TakeAwayProductRS()
                    {
                        id = _TakeAwayProductCAT_GetData.GetInt32(0),
                        Name = _TakeAwayProductCAT_GetData.GetString(1),
                        Price = _TakeAwayProductCAT_GetData.GetInt32(2),
                        ItemCount = 1,

                    });

                }

                connection.Close();

                if (_m_TakeAwayProductRS.Count == 0)
                {
                    MessageBox.Show("Product Category Rate Not Found!");

                    return;

                }
                else
                {


                    this.Dispatcher.Invoke(() =>
                    {

                        price_ListBox.ItemsSource = _m_TakeAwayProductRS;
                    });
                }
            }
            catch (Exception ex)
            {

                log.Error(" ===================  Error In Product Category Not Found!   =========================== ");
                log.Error(DateTime.Now.ToString());
                log.Error(ex.ToString());
                log.Error(" ===================  End of Error  =========================== ");
                if (MessageBox.Show(ex.ToString(),
                        "Error In Product Category Rates Not Found!", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                Environment.Exit(0);
            }




        }

        #endregion

        #region Event Popup Payment
        private void One_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "1";
        }

        private void Two_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "2";
        }

        private void Three_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "3";
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "4";
        }

        private void Mul_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "*";
        }

        private void Five_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "5";
        }

        private void Six_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "6";
        }

        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "8";
        }

        private void Ziro_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "0";
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "9";
        }

        private void Point_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += ".";
        }

        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            txtBal.Text += "7";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            paymentPopup.IsOpen = false;
        }

        private void Grid_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToUpper() == "F10")
            {
                BtnComplete_Click(sender, e);
            }
            else if (e.Key.ToString().ToUpper() == "ESC")
            {
                paymentPopup.IsOpen = false;
            }
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            paymentPopup.IsOpen = false;
            //paymentPopup.Visibility = Visibility.Collapsed;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connstring))
                {
                    connection.Open();
                    NpgsqlCommand INSERT_c_invoicepaymentdetails = new NpgsqlCommand("INSERT INTO c_invoicepaymentdetails(" +
                                                    " ad_client_id, ad_org_id, c_invoice_id," +
                                                    "cash, card, exchange, redemption, iscomplementary, iscredit, createdby, updatedby)" +
                                                    "VALUES(" + _clientId + "," + _OrgId + "," + invoiceno + "," + payableAmount + ",0,0,0,'N','N'," + _UserID + "," + _UserID + "); ", connection);
                    INSERT_c_invoicepaymentdetails.ExecuteNonQuery();

                    //NpgsqlCommand cmd_update_sequenc_no = new NpgsqlCommand("UPDATE ad_sequence SET currentnext =" + _sequenc_id + " WHERE name = 'c_invoicepaymentdetails';", connection);
                    //NpgsqlDataReader _update__Ad_sequenc_no = cmd_update_sequenc_no.ExecuteReader();
                    MessageBox.Show("cash paymen completed successfully");
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }



        }

        private void PreBill_Click(object sender, RoutedEventArgs e)
        {
            invoive_No p = new invoive_No();
           invoiceno = p.invoice_no();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connstring))
                {
                    connection.Open();
                    var dateTimeOffset = DateTimeOffset.Parse(DateTime.Now.ToUniversalTime().ToString(), null);
                    var dt = (dateTimeOffset.DateTime);

                    string command_c_invoice = "insert into c_invoice(c_invoice_id,ad_client_id,ad_org_id,created,createdby,ad_user_id,mobile,discounttype,discountvalue,grandtotal,reason,grandtotal_round_off,total_items_count,balance,order_type,m_warehouse_id,c_bpartner_id,session_id)" +
                                                                   "OVERRIDING SYSTEM VALUE values('" + invoiceno + "'," + _clientId + "," + _OrgId + ",'" + dt + "'," + _UserID + "," + _UserID + ",0000000,1," + (totalAmount - payableAmount) + "," + payableAmount + ",'" + txtReason.Text + "'," + payableAmount + "," + totalItems + ",0,'Take away'," + _Warehouse_Id + "," + _Bpartner_Id + "," + _sessionID + ")";
                    NpgsqlCommand cmd = new NpgsqlCommand(command_c_invoice, connection);
                    cmd.ExecuteNonQuery();
                    foreach (m_TakeAwayProductRS element in _m_TakeAwayProductRS)
                    {
                        double discountValue = element.Price * element.ItemCount - element.TotalPrice;
                        string c_invoiceline = "insert into c_invoiceline(ad_client_id,ad_org_id,created,createdby,ad_user_id,c_invoice_id,productname,saleprice,qtyinvoiced,discountvalue,linetotalamt)" +
                        " values(" + _clientId + "," + _OrgId + ",'" + dt + "'," + _UserID + "," + _UserID + "," + invoiceno + ",'" + element.Name + "'," + element.Price + "," + element.ItemCount + "," + discountValue + "," + element.TotalPrice + ")";
                        cmd = new NpgsqlCommand(c_invoiceline, connection);
                        int rowInvoice = cmd.ExecuteNonQuery();
                    }

                }
                MessageBox.Show("Pre Bill Generated");
            }
            catch
            {

            }
        }
    


private void TxtBal_KeyUp(object sender, KeyEventArgs e)
        {
            decimal BalAmt = int.Parse(txtBal.Text) - int.Parse(PayableAmt.Content.ToString());
            BalanceAmt.Content = BalAmt;
        }

        #endregion

        #region Event Popup Notes
        private void BtnPopupNotesVancel_Click(object sender, RoutedEventArgs e)
        {
            MyPopupNotes.IsOpen = false;
        }

        private void Grid_KeyUp_2(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToUpper() == "F4")
            {
                BtnPopupNotesSubmit_Click(sender, e);
            }
            else if (e.Key.ToString().ToUpper() == "ESCAPE")
            {
                MyPopupNotes.IsOpen = false;
            }
        }

        private void BtnPopupNotesSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Event Line Discount

        private void Grid_KeyUp_3(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToUpper() == "ENTER")
            {
                MyPopupLineDiscSubmit_Click(sender, e);
            }
            else if (e.Key.ToString().ToUpper() == "ESCAPE")
            {
                MyPopupLineDisc.IsOpen = false;
            }
        }

        private void MyPopupLineDiscSubmit_Click(object sender, RoutedEventArgs e)
        {
            MyPopupLineDisc.IsOpen = false;
            if (!totalDiscountApplied)
            {
                m_TakeAwayProductRS SelectedItem = _m_TakeAwayProductRS.Find(i => i.Name == selectItemForLineDiscount);
                int SelectedItemIndex = _m_TakeAwayProductRS.FindIndex(i => i.Name == selectItemForLineDiscount);
                _m_TakeAwayProductRS.RemoveAt(SelectedItemIndex);
                if (qar_line_radio.IsChecked == true && float.Parse(discount.Text) < SelectedItem.Price)
                {
                    SelectedItem.discountPer = Math.Round((float.Parse(discount.Text) / SelectedItem.Price) * 100, 2);
                }
                else if (per_line_radio.IsChecked == true)
                {
                    SelectedItem.discountPer = Math.Round(float.Parse(discount.Text), 2);
                }
                _m_TakeAwayProductRS.Insert(SelectedItemIndex, SelectedItem);
                price_ListBox.Items.Refresh();
                calculateSum();

                this.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Total discount already applied please set it to 0");
                this.Visibility = Visibility.Visible;
            }

        }

        private void BtnMyPopupLineDiscCancel_Click(object sender, RoutedEventArgs e)
        {
            MyPopupLineDisc.IsOpen = false;
            this.Visibility = Visibility.Visible;
        }

        #endregion

        #region Event Total Discount

        private void BtnMyPopupTotalDiscOK_Click(object sender, RoutedEventArgs e)
        {
            if (float.Parse(txtOpeningBalanceDisc.Text) > 0)
                totalDiscountApplied = true;
            else
            {
                totalDiscountApplied = false;
            }

            MyPopupTotalDisc.IsOpen = false;
            double discountPer = 0;


            if (qar_total_radio.IsChecked == true)
            {
                discountPer = Math.Round(float.Parse(txtOpeningBalanceDisc.Text) / float.Parse(payableAmount.ToString()) * 100, 2);
            }
            else if (per_total_radio.IsChecked == true)
            {
                discountPer = float.Parse(txtOpeningBalanceDisc.Text);
            }

            foreach (m_TakeAwayProductRS element in _m_TakeAwayProductRS)
            {

                element.discountPer = discountPer;
            }
            price_ListBox.Items.Refresh();
            calculateSum();
            //MyPopupLineDisc.IsOpen = false;

            this.Visibility = Visibility.Visible;


        }

        private void BtnMyPopupTotalDiscCancel_Click(object sender, RoutedEventArgs e)
        {
            MyPopupTotalDisc.IsOpen = false;
        }

        private void Grid_KeyUp_4(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToUpper() == "ENTER")
            {
                BtnMyPopupTotalDiscOK_Click(sender, e);
            }
            else if (e.Key.ToString().ToUpper() == "ESCAPE")
            {
                MyPopupTotalDisc.IsOpen = false;
            }
        }

        #endregion

        #region Event

        public void calculateSum()
        {
            payableAmount = 0;
            totalItems = 0;
            totalAmount = 0;
            double total_discount_percent = 0;
            int itemTypes = 0;
            foreach (m_TakeAwayProductRS element in _m_TakeAwayProductRS)
            {
                itemTypes++;
                totalItems += element.ItemCount;
                payableAmount += element.TotalPrice;
                totalAmount += element.Price * element.ItemCount;
                total_discount_percent += element.discountPer;
            }

            lblRS.Content = payableAmount.ToString();
            lblitems.Content = totalItems.ToString() + " ITEMS";
            DiscountAmt.Content = totalAmount - payableAmount;
            txtTotalDiscountPer.Text = (total_discount_percent / itemTypes).ToString();
            //txtOpeningBalanceDisc.Text = txtTotalDiscountPer.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Product_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic PC = Product_ListBox.SelectedItem as dynamic;
            int id = PC.id;
            BindTakeAwayProduct(id);

        }

        private void ProductCat_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic PC = productCat_ListBox.SelectedItem as dynamic;
            int id = PC.id;

            string Name = PC.Name;

            BindTakeAwayProductRS(id);
            calculateSum();

        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToUpper() == "F9")
            {
                if (selectedTable > 0)
                {
                    paymentPopup.IsOpen = true;
                    TotalAmt.Content = totalAmount;
                    //DiscountAmt.Content =totalAmount- payableAmount;
                    double PayAmt = payableAmount;
                    PayableAmt.Content = payableAmount;
                    BalanceAmt.Content = payableAmount;
                    txtBal.Text = payableAmount.ToString();
                }

                else
                {
                    MessageBox.Show("Please select the table");
                }

            }
            if (e.Key.ToString().ToUpper() == "F5")
            {
                MyPopupNotes.IsOpen = true;

            }
            if (e.Key.ToString().ToUpper() == "F4")
            {
                MyPopupTotalDisc.IsOpen = true;
                billAmt.Content = "QR" + lblRS.Content;
            }
            if (e.Key.ToString().ToUpper() == "F3")
            {


            }
            if (e.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {

                _m_TakeAwayProductRS.ForEach((x) => x.checkbox = "Visible");
                price_ListBox.Items.Refresh();
                // checkboxVisible = "Visible";
                //checkBoxsplitBill.Visibility = "Visible";
                cancel_Order.Visibility = Visibility.Hidden;
                KOT.Visibility = Visibility.Hidden;
                Spit_Back.Visibility = Visibility.Visible;
                Pre_bil.Visibility = Visibility.Hidden;
                HR.Visibility = Visibility.Hidden;
                completed.Visibility = Visibility.Hidden;
                split.Visibility = Visibility.Visible;
            }
        }

        private void BtnDec_Click(object sender, RoutedEventArgs e)
        {
            string ItemName = (sender as Button).ToolTip.ToString();

            m_TakeAwayProductRS SelectedItem = _m_TakeAwayProductRS.Find(i => i.Name == ItemName);
            int SelectedItemIndex = _m_TakeAwayProductRS.FindIndex(i => i.Name == ItemName);

            _m_TakeAwayProductRS.RemoveAt(SelectedItemIndex);

            if (SelectedItem.ItemCount > 1)
            {
                SelectedItem.ItemCount -= 1;

                _m_TakeAwayProductRS.Insert(SelectedItemIndex, SelectedItem);

            }
            price_ListBox.Items.Refresh();

            calculateSum();

        }

        private void BtnInc_Click(object sender, RoutedEventArgs e)
        {
            string ItemName = (sender as Button).ToolTip.ToString();
            m_TakeAwayProductRS SelectedItem = _m_TakeAwayProductRS.Find(i => i.Name == ItemName);
            int SelectedItemIndex = _m_TakeAwayProductRS.FindIndex(i => i.Name == ItemName);
            _m_TakeAwayProductRS.RemoveAt(SelectedItemIndex);

            SelectedItem.ItemCount += 1;

            _m_TakeAwayProductRS.Insert(SelectedItemIndex, SelectedItem);
            price_ListBox.Items.Refresh();
            calculateSum();
        }

        private void ViewProduct_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new ViewProduct());
        }

        private void ViewUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewUser());

        }

        private void ViewProductCat_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new ViewProductCAT());
        }

        private void ViewTerminal_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new ViewTerminals());
        }

        private void POS_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new POSsystem());
        }

        private void DineIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DineIn());

        }

        private void BtnNotesEdit_Click(object sender, RoutedEventArgs e)
        {
            MyPopupNotes.IsOpen = true;
            lblItemName.Content = "items";
        }

        private void BtnPer_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            MyPopupLineDisc.IsOpen = true;

            string ItemName = (sender as Button).ToolTip.ToString();
            lblitemName.Content = ItemName;
            selectItemForLineDiscount = ItemName;

        }
        #endregion
        private void getTables()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connstring))
            {
                connection.Open();
                string command_floortables = "select * from floorTables";
                NpgsqlCommand cmd = new NpgsqlCommand(command_floortables, connection);
                NpgsqlDataReader _floortable_GetData = cmd.ExecuteReader();
                _m_floor2Tables.Clear();
                floor2tables.ItemsSource = "";
                while (_floortable_GetData.Read())
                {

                    if (_floortable_GetData.GetString(5) == "2")
                    {
                        _m_floor2Tables.Add(new m_floorTables()
                        {
                            id = _floortable_GetData.GetInt32(0),
                            name = _floortable_GetData.GetString(1),
                            description = _floortable_GetData.GetString(2),
                            tablegroup = _floortable_GetData.GetString(3),
                            coversLevel = _floortable_GetData.GetBoolean(4),
                            floor_No = _floortable_GetData.GetString(5)
                        });
                    }
                    else
                    {
                        _m_floor1Tables.Add(new m_floorTables()
                        {
                            id = _floortable_GetData.GetInt32(0),
                            name = _floortable_GetData.GetString(1),
                            description = _floortable_GetData.GetString(2),
                            tablegroup = _floortable_GetData.GetString(3),
                            coversLevel = _floortable_GetData.GetBoolean(4),
                            floor_No = _floortable_GetData.GetString(5)
                        });
                    }
                }
                if (_m_floor2Tables.Count == 0)
                {
                    MessageBox.Show("No Tables Found");

                    return;

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        floor1tables.ItemsSource = _m_floor1Tables;
                        floor2tables.ItemsSource = _m_floor2Tables;
                    });
                }
            }
        }

        private void Floor2tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic table = floor2tables.SelectedItem as dynamic;
            selectedTable = table.id;

        }

        private void Split_Click(object sender, RoutedEventArgs e)
        {
            invoive_No p = new invoive_No();
            int? splitinvoiceNo = p.invoice_no();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connstring))
                {
                    connection.Open();
                    var dateTimeOffset = DateTimeOffset.Parse(DateTime.Now.ToUniversalTime().ToString(), null);
                    var dt = (dateTimeOffset.DateTime);
                    NpgsqlCommand cmd = null;
                    double totalDiscount = 0;
                    double splitPayableamount = 0;
                    double splitItemCount = 0;
                    //Updating  c_invoiceline table
                    foreach (m_TakeAwayProductRS element in splitbill)
                    {
                        double discountValue = element.Price * element.ItemCount - element.TotalPrice;
                        totalDiscount += discountValue;
                        splitPayableamount += element.TotalPrice;
                        splitItemCount += element.ItemCount;
                        string c_invoiceline = "update c_invoiceline set c_invoice_id = " + splitinvoiceNo + " where c_invoice_id =" + invoiceno + " and productname ='"+ element.Name + "'";
                       
                        cmd = new NpgsqlCommand(c_invoiceline, connection);
                        int rowInvoice = cmd.ExecuteNonQuery();
                    }

                    //Updating and adding record to c_invoice table
                    string c_invoice_Update = "update c_invoice set discountvalue= discountvalue - "+totalDiscount+",grandtotal=grandtotal- "+splitPayableamount+",grandtotal_round_off=grandtotal_round_off - "+splitPayableamount+",total_items_count= total_items_count- "+splitItemCount+ " where c_invoice_id="+invoiceno+ "";
                    cmd = new NpgsqlCommand(c_invoice_Update, connection);
                    cmd.ExecuteNonQuery();
                    string command_c_invoice = "insert into c_invoice(c_invoice_id,ad_client_id,ad_org_id,created,createdby,ad_user_id,mobile,discounttype,discountvalue,grandtotal,reason,grandtotal_round_off,total_items_count,balance,order_type,m_warehouse_id,c_bpartner_id,session_id)" +
                                           "OVERRIDING SYSTEM VALUE values" +
                                             "('" + splitinvoiceNo + "'," + _clientId + "," + _OrgId + ",'" + dt + "'," +
                                             "" + _UserID + "," + _UserID + ",0000000,1," + totalDiscount + "," +
                                             splitPayableamount + ",'" + txtReason.Text + "'," + splitPayableamount + "," + 
                                             splitItemCount + 
                                             ",0,'Take away'," + _Warehouse_Id + "," + _Bpartner_Id + "," + _sessionID + ")";
                     cmd= new NpgsqlCommand(command_c_invoice, connection);
                    cmd.ExecuteNonQuery();

                    //updating and adding record to Payment table
                    
                    //INSERT_c_invoicepaymentdetails.ExecuteNonQuery();

                    string c_invoicepaymentdetails_update = "update c_invoicepaymentdetails set cash= cash-"+ splitPayableamount + "where c_invoice_id = "+invoiceno+"";
                       cmd = new NpgsqlCommand(c_invoicepaymentdetails_update, connection);
                    cmd.ExecuteNonQuery();

                    string c_invoicepaymentdetails_insert = "INSERT INTO c_invoicepaymentdetails(" +
                                                  " ad_client_id, ad_org_id, c_invoice_id," +
                                                "cash, card, exchange, redemption, iscomplementary, iscredit, createdby, updatedby)" +
                                              "VALUES(" + _clientId + "," + _OrgId + "," + splitinvoiceNo + "," + splitPayableamount + ",0,0,0,'N','N'," + _UserID + "," + _UserID + ")";
                    cmd = new NpgsqlCommand(c_invoicepaymentdetails_insert, connection);
                    cmd.ExecuteNonQuery();


                    //END  updating and adding record to Payment table
                    MessageBox.Show("Split Successful");
                }
                
            }
            catch
            {

            }

        }

        private void Spit_Back_Click(object sender, RoutedEventArgs e)
        {
            _m_TakeAwayProductRS.ForEach((x) => x.checkbox = "Hidden");
            price_ListBox.Items.Refresh();
            // checkboxVisible = "Visible";
            //checkBoxsplitBill.Visibility = "Visible";
            cancel_Order.Visibility = Visibility.Visible;
            KOT.Visibility = Visibility.Visible;
            Spit_Back.Visibility = Visibility.Hidden;
            Pre_bil.Visibility = Visibility.Visible;
            HR.Visibility = Visibility.Visible;
            completed.Visibility = Visibility.Visible;
            split.Visibility = Visibility.Hidden;
        }
        //List<string> splitbill = new List<string>();

       

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = (sender as CheckBox).IsChecked;
            string itemName = (sender as CheckBox).ToolTip.ToString();

            m_TakeAwayProductRS SelectedItem = _m_TakeAwayProductRS.Find(i => i.Name == itemName);
            int SelectedItemIndex = _m_TakeAwayProductRS.FindIndex(i => i.Name == itemName);
           if (isChecked==true)
                splitbill.Add(SelectedItem);

            if (isChecked == false)
                splitbill.RemoveAt(SelectedItemIndex);

        }

    }
}
