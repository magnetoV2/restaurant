using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Npgsql;
using System.Linq;
using Restaurant_Pos.Pages;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;
using Newtonsoft.Json.Linq;
using Restaurant_Pos.ViewModels.Page;

namespace Restaurant_Pos.Pages.Session
{
    class Sessions
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<OrderDetails> salesSummaryDetails_Print = new List<OrderDetails>();

        //string JSON = "{\"operation\": \"GetSalesSummary\",\"username\": \"gulfcashier\",\"password\": \"123456\",\"clientId\": 1000049,\"orgId\": 1000070,\"userId\": 1005329,\"roleId\": 1000328,\"sessionId\": 1001901.0,\"businessPartnerId\": 1011080,\"warehouseId\": 1000103,\"cashbookId\": 1000092,\"startTime\": 1556543196000,\"endTime\": 1556545753139,\"startTime_Atc\": \"4/29/2019 6:36:36 PM\",\"endTime_Atc\": \"29/04/2019 07:19:13\",\"SyncedTime\": 0,\"showImage\": \"N\",\"macAddress\": \"BFEBFBFF000906EA\",\"version\": 1.0,\"appName\": \"POS\",\"remindMe\": \"Y\",\"UserName\": \"gulfcashier\",\"Denominations\": [{\"total\": 2000.0,\"count\": 4.0,\"type\": \"riyal\",\"name\": 500    },{\"total\": 500.0,\"count\": 5.0,\"type\": \"riyal\",\"name\": 100    },{\"total\": 300.0,\"count\": 6.0,\"type\": \"riyal\",\"name\": 50    },{\"total\": 70.0,\"count\": 7.0,\"type\": \"riyal\",\"name\": 10    },{\"total\": 40.0,\"count\": 8.0,\"type\": \"riyal\",\"name\": 5    },{\"total\": 9.0,\"count\": 9.0,\"type\": \"riyal\",\"name\": 1    },{\"total\": 4.5,\"count\": 9.0,\"type\": \"dirhams\",\"name\": 50    },{\"total\": 2.25,\"count\": 9.0,\"type\": \"dirhams\",\"name\": 25    },{\"total\": 2.0,\"count\": 1,\"type\": \"cash\",\"name\": 2    },{\"total\": 0,\"count\": 1,\"type\": \"complement\",\"name\": 1    },{\"total\": 2.0,\"count\": 0,\"type\": \"total\",\"name\": 0    }  ]}";

        public static void Sales_Summary_APICall(string json)
        {
            dynamic CloseSessionApiStringResponce,
                CloseSessionrApiJSONResponce,
                SalesSummaryJSON = JsonConvert.DeserializeObject(json);

            NpgsqlConnection connection = new NpgsqlConnection(PostgreSQL.ConnectionString);
            try
            {
                CloseSessionApiStringResponce = PostgreSQL.ApiCallPost(json);
                CloseSessionrApiJSONResponce = JsonConvert.DeserializeObject(CloseSessionApiStringResponce);
                Console.WriteLine(CloseSessionrApiJSONResponce);
                if (CloseSessionrApiJSONResponce.responseCode == "200")
                {
                    JArray salesSummaryDetails = CloseSessionrApiJSONResponce.salesSummaryDetails;
                    try
                    {
                        NotifierViewModel.Notifier.ShowSuccess("Session Closed");
                        Sales_Summary_Print(json, salesSummaryDetails);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Sales Summary Details Print Failed");
                        log.Error(ex);

                    }
                    connection.Open();
                    NpgsqlCommand cmd_select_sequenc_no = new NpgsqlCommand("UPDATE ad_user_pos " +
                    "SET sessionid = 0 ,attribute1 =null " +
                    "WHERE ad_client_id = " + SalesSummaryJSON.clientId + "  and ad_org_id = " + SalesSummaryJSON.orgId + " and ad_user_id = " + SalesSummaryJSON.userId + " ; ", connection);
                    NpgsqlDataReader _get__Ad_sequenc_no = cmd_select_sequenc_no.ExecuteReader();
                    connection.Close();
                }
                else
                {

                    log.Error("Posting Invoice Failed|Responce Code: " + CloseSessionrApiJSONResponce.responseCode);
                    log.Error("----------------Server Responce --------------");
                    log.Error(CloseSessionrApiJSONResponce);
                    log.Error("----------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Error(ex);
            }
        }

        private static void Sales_Summary_Print(string json, JArray salesSummaryDetails)
        {
            dynamic SalesSummaryJSON = JsonConvert.DeserializeObject(json);
            JArray Denominations = SalesSummaryJSON.Denominations;
            int PageSpace_sp = 62;
            #region Public

            string baseFont = "./Resources/Fonts/GothamRounded/#GothamRounded-Book";
            PrintDialog printDlg = new PrintDialog();
            FlowDocument doc = new FlowDocument();
            Span s = new Span();
           
            #endregion Public

            #region Sales Summary

            Paragraph SalesSummary_Header = new Paragraph();
            byte[] binaryData = Convert.FromBase64String(Convert.ToString(RetailPage.AD_ORG_logo));
            BitmapImage ad_org_logo_bit = new BitmapImage();
            ad_org_logo_bit.BeginInit();
            ad_org_logo_bit.StreamSource = new MemoryStream(binaryData);
            ad_org_logo_bit.EndInit();
            Image logo = new Image() { Source = ad_org_logo_bit };
            s = new Span(new Run(""));
            s.Inlines.Add(new LineBreak());
            s.Inlines.Add(new LineBreak());
            SalesSummary_Header.Inlines.Add(logo);
            SalesSummary_Header.Inlines.Add(s);

            if (RetailPage.AD_ORG_arabicname != String.Empty)
            {
                s = new Span(new Run(RetailPage.AD_ORG_arabicname));
                s.Inlines.Add(new LineBreak());
                s.FontWeight = FontWeights.Bold;
                SalesSummary_Header.Inlines.Add(s);
            }
            s = new Span(new Run(RetailPage.AD_ORG_name));
            s.Inlines.Add(new LineBreak());
            s.FontWeight = FontWeights.Bold;
            SalesSummary_Header.Inlines.Add(s);

            s = new Span(new Run(Convert.ToString(SalesSummaryJSON.AD_Warehouse_Name) + " - " + RetailPage.AD_ORG_city));
            s.Inlines.Add(new LineBreak());
            SalesSummary_Header.Inlines.Add(s);
            s = new Span(new Run(" Tel: " + RetailPage.AD_ORG_phone));
            s.Inlines.Add(new LineBreak());
            SalesSummary_Header.Inlines.Add(s);

            s = new Span(new Run("Sales Summary"));
            s.Inlines.Add(new LineBreak());
            s.Inlines.Add(new LineBreak());
            s.FontSize = 16;
            SalesSummary_Header.Inlines.Add(s);



            SalesSummary_Header.Inlines.Add(s);
            SalesSummary_Header.FontSize = 14;
            SalesSummary_Header.FontStyle = FontStyles.Normal;
            SalesSummary_Header.TextAlignment = TextAlignment.Center;
            SalesSummary_Header.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            doc.Blocks.Add(SalesSummary_Header);

            #endregion Sales Summary

            #region SalesSummary Header Lines

            Paragraph SalesSummary_Header_Lines = new Paragraph();
            int SalesSummary_Header_Lines_sp = 60;
            string S_StartTime = "Session Start Time : " + Convert.ToString(Convert.ToDateTime(RetailPage.AD_Session_Started_at));
            string S_EndTime = "Session End Time : " + Convert.ToString(DateTime.Now);

            s = new Span(new Run(S_StartTime));
            s.Inlines.Add(new LineBreak());
            SalesSummary_Header.Inlines.Add(s);
            s = new Span(new Run(S_EndTime));
            SalesSummary_Header.Inlines.Add(s);

            string Cashier = "Cashier : " + RetailPage.AD_UserName;
            int Cashier_l = Cashier.Length;
            string Time = "Time : " + DateTime.Now.ToShortTimeString();
            int Time_l = Time.Length;
            int Cashier_Time_sp = SalesSummary_Header_Lines_sp - (Cashier_l + Time_l);
            s = new Span(new Run(Cashier + Time.PadLeft(Cashier_Time_sp)));
            SalesSummary_Header_Lines.Inlines.Add(s);
            SalesSummary_Header_Lines.FontSize = 14;
            SalesSummary_Header_Lines.FontStyle = FontStyles.Normal;
            SalesSummary_Header_Lines.TextAlignment = TextAlignment.Left;
            SalesSummary_Header_Lines.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            SalesSummary_Header_Lines.Margin = new Thickness(0);
            doc.Blocks.Add(SalesSummary_Header_Lines);

            #endregion SalesSummary Header Lines

            #region salesSummaryDetails_HD

            Paragraph salesSummaryDetails_HD = new Paragraph();
            s = new Span(new Run("-----------------------------------------------"));//47
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);
            s = new Span(new Run("Total Revenue Summary"));
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);
            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);

            salesSummaryDetails_HD.FontSize = 14;
            salesSummaryDetails_HD.FontStyle = FontStyles.Normal;
            salesSummaryDetails_HD.TextAlignment = TextAlignment.Center;
            salesSummaryDetails_HD.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            salesSummaryDetails_HD.Margin = new Thickness(0);
            doc.Blocks.Add(salesSummaryDetails_HD);
            #endregion


            #region salesSummaryDetails
            Paragraph salesSummaryDetails_Lines = new Paragraph();
           
            double totat_revenue = 0;
            foreach (dynamic _Summary in salesSummaryDetails )
            {
                string lineTotal = (Convert.ToDouble(_Summary.lineTotal)).ToString("0.00");
                string summaryName = _Summary.summaryName;
                string linePrice = (Convert.ToDouble(_Summary.linePrice)).ToString("0.00");
                string summaryTitle = _Summary.summaryTitle;
                string lineQty = _Summary.lineQty;
                if (summaryTitle == "Summary")
                {
                    if (summaryName == "Settlement Discount" || summaryName == "Discount")
                    {
                        totat_revenue -= Convert.ToDouble(lineTotal);
                    }
                    else
                    {
                        if (summaryName != "Total Sales")
                        {
                            totat_revenue += Convert.ToDouble(lineTotal);
                        }
                
                    }
                    int _sp;
                    switch (summaryName)
                    {
                        case "Total Sales":
                            _sp = 65 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Discount":
                            _sp = 64 - ((summaryName + "(-)").Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + "(-)" + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Cash":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Credit":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Complementary":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Exchange":
                            _sp = 54 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Redeem":
                            _sp = 54 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Settlement Discount":
                            _sp = 56 - ((summaryName + "(-)").Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + "(-)" + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Credit/Debit Card":
                            _sp = 59 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "All":
                            _sp = 60 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        default:
                            break;
                    }

                }
                else if (summaryTitle == "Item Summary")
                {
                    salesSummaryDetails_Print.Add(new OrderDetails()
                    {
                        actualPrice = linePrice,
                        qty = lineQty,
                        productName = summaryName,
                        price = lineTotal,
                    }); 
                }
                else
                {

                }
                //int _sp = _Summary_sp - (summaryName.Length + linePrice.Length);
                //Console.WriteLine(_Summary_sp);
                //Console.WriteLine(summaryName);
                //Console.WriteLine(_sp);
                //Console.WriteLine(summaryName.Length);
                //Console.WriteLine(linePrice.Length);
                //Console.WriteLine(_sp+ summaryName.Length+ linePrice.Length);
                //s = new Span(new Run(summaryName + linePrice.PadLeft(_sp)));
                //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
                //s.Inlines.Add(new LineBreak());
                //salesSummaryDetails_Lines.Inlines.Add(s);
            }
            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);
            int totat_revenue_sp_cal = 58;
            int totat_revenue_sp = totat_revenue_sp_cal - ("Total Revenue".Length + totat_revenue.ToString().Length);
            s = new Span(new Run("Total Revenue" + totat_revenue.ToString("0.00").PadLeft(totat_revenue_sp)));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);

            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);

            salesSummaryDetails_Lines.FontSize = 14;
            salesSummaryDetails_Lines.FontStyle = FontStyles.Normal;
            salesSummaryDetails_Lines.TextAlignment = TextAlignment.Left;
            salesSummaryDetails_Lines.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            salesSummaryDetails_Lines.Margin = new Thickness(0);
            doc.Blocks.Add(salesSummaryDetails_Lines);
            #endregion


            #region customerSummaryDetails_HD

              salesSummaryDetails_HD = new Paragraph();
            s = new Span(new Run("-----------------------------------------------"));//47
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);
            s = new Span(new Run("Cash Customer Summary"));
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);
            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_HD.Inlines.Add(s);

            salesSummaryDetails_HD.FontSize = 14;
            salesSummaryDetails_HD.FontStyle = FontStyles.Normal;
            salesSummaryDetails_HD.TextAlignment = TextAlignment.Center;
            salesSummaryDetails_HD.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            salesSummaryDetails_HD.Margin = new Thickness(0);
            doc.Blocks.Add(salesSummaryDetails_HD);
            #endregion


            #region customerSummaryDetails
            salesSummaryDetails_Lines = new Paragraph();

              totat_revenue = 0;
            foreach (dynamic _Summary in salesSummaryDetails)
            {
                string lineTotal = (Convert.ToDouble(_Summary.lineTotal)).ToString("0.00");
                string summaryName = _Summary.summaryName;
                string linePrice = (Convert.ToDouble(_Summary.linePrice)).ToString("0.00");
                string summaryTitle = _Summary.summaryTitle;
                string lineQty = _Summary.lineQty;
                if (summaryTitle == "Summary")
                {
                    if (summaryName == "Settlement Discount" || summaryName == "Discount")
                    {
                        totat_revenue -= Convert.ToDouble(lineTotal);
                    }
                    else
                    {
                        if (summaryName != "Total Sales")
                        {
                            totat_revenue += Convert.ToDouble(lineTotal);
                        }

                    }
                    int _sp;
                    switch (summaryName)
                    {
                        case "Total Sales":
                            _sp = 65 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Discount":
                            _sp = 64 - ((summaryName + "(-)").Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + "(-)" + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Cash":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Credit":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Complementary":
                            _sp = 58 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Exchange":
                            _sp = 54 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Accountable Redeem":
                            _sp = 54 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Settlement Discount":
                            _sp = 56 - ((summaryName + "(-)").Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + "(-)" + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "Credit/Debit Card":
                            _sp = 59 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        case "All":
                            _sp = 60 - (summaryName.Length + lineTotal.Length);
                            s = new Span(new Run(summaryName + lineTotal.PadLeft(_sp)));
                            s.Inlines.Add(new LineBreak());
                            salesSummaryDetails_Lines.Inlines.Add(s);
                            break;
                        default:
                            break;
                    }

                } 
                else
                {

                }
              
            }
            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);
              totat_revenue_sp_cal = 58;
              totat_revenue_sp = totat_revenue_sp_cal - ("Total Revenue".Length + totat_revenue.ToString().Length);
            s = new Span(new Run("Total Revenue" + totat_revenue.ToString("0.00").PadLeft(totat_revenue_sp)));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);

            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            salesSummaryDetails_Lines.Inlines.Add(s);

            salesSummaryDetails_Lines.FontSize = 14;
            salesSummaryDetails_Lines.FontStyle = FontStyles.Normal;
            salesSummaryDetails_Lines.TextAlignment = TextAlignment.Left;
            salesSummaryDetails_Lines.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            salesSummaryDetails_Lines.Margin = new Thickness(0);
            doc.Blocks.Add(salesSummaryDetails_Lines);
            #endregion



            #region Item Sales Header
            Paragraph ItemSales_Hd = new Paragraph();
            s = new Span(new Run("Item Sales"));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            //s.Inlines.Add(new LineBreak());
            ItemSales_Hd.Inlines.Add(s);
            s.Inlines.Add(new LineBreak());
            s = new Span(new Run("-----------------------------------------------"));//47
            ItemSales_Hd.Inlines.Add(s);

            ItemSales_Hd.FontSize = 14;
            ItemSales_Hd.FontStyle = FontStyles.Normal;
            ItemSales_Hd.TextAlignment = TextAlignment.Center;
            ItemSales_Hd.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            ItemSales_Hd.Margin = new Thickness(0);
            doc.Blocks.Add(ItemSales_Hd);
            #endregion



            #region Item Sales Line
            double subtotal = 0;
            Section OrderDetails_sec = new Section();
            salesSummaryDetails_Print.ForEach(x =>
            {
                Paragraph OrderDetails = new Paragraph();
                s = new Span(new Run(x.productName));

                OrderDetails.Inlines.Add(s);
                OrderDetails.Margin = new Thickness(0);
                OrderDetails.FontSize = 14;
                OrderDetails.FontStyle = FontStyles.Normal;
                OrderDetails.TextAlignment = TextAlignment.Left;
                OrderDetails.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
                OrderDetails_sec.Blocks.Add(OrderDetails);

                Paragraph OrderDetails_price = new Paragraph();
                int print_spacing = 20;
                int _p = Convert.ToDouble(x.price).ToString("0.00").Length;
                string price = Convert.ToDouble(x.price).ToString("0.00");
                int a = print_spacing - _p;
                s = new Span(new Run(x.qty + " " + " x " + Convert.ToDouble(x.actualPrice).ToString("0.00") + "  " + price.PadLeft(a)));//.PadLeft(a)
                //s.Inlines.Add(new LineBreak());
                OrderDetails_price.Inlines.Add(s);
                OrderDetails_price.Margin = new Thickness(0);
                OrderDetails_price.FontSize = 14;
                OrderDetails_price.FontStyle = FontStyles.Normal;
                OrderDetails_price.TextAlignment = TextAlignment.Right;
                OrderDetails_price.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
                OrderDetails_sec.Blocks.Add(OrderDetails_price);
                subtotal += Convert.ToDouble(x.price);
            });
            doc.Blocks.Add(OrderDetails_sec);

            Paragraph OrderDetails_Total = new Paragraph();
            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            OrderDetails_Total.Inlines.Add(s);

            int totat_sales_sp = PageSpace_sp - ("Total Sales".Length + subtotal.ToString().Length);
            s = new Span(new Run("Total Sales" + subtotal.ToString("0.00").PadLeft(totat_sales_sp)));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            s.Inlines.Add(new LineBreak());
            OrderDetails_Total.Inlines.Add(s);

            s = new Span(new Run("-----------------------------------------------"));//47
         

            OrderDetails_Total.Inlines.Add(s);

            OrderDetails_Total.Margin = new Thickness(0);
            OrderDetails_Total.FontSize = 14;
            OrderDetails_Total.FontStyle = FontStyles.Normal;
            OrderDetails_Total.TextAlignment = TextAlignment.Left;
            OrderDetails_Total.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            doc.Blocks.Add(OrderDetails_Total);

            #endregion

            #region Current Cashier Cash Receipts hd
            Paragraph CurrentCashierCashReceipts = new Paragraph();
  
            s = new Span(new Run("CURRENT CASHIER CASH RECEIPTS"));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            s.Inlines.Add(new LineBreak());
            CurrentCashierCashReceipts.Inlines.Add(s);

            s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());

            CurrentCashierCashReceipts.Inlines.Add(s);

            CurrentCashierCashReceipts.Margin = new Thickness(0);
            CurrentCashierCashReceipts.FontSize = 14;
            CurrentCashierCashReceipts.FontStyle = FontStyles.Normal;
            CurrentCashierCashReceipts.TextAlignment = TextAlignment.Center;
            CurrentCashierCashReceipts.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            doc.Blocks.Add(CurrentCashierCashReceipts);

            #endregion

            #region Current Cashier Cash Receipts line
            Paragraph CurrentCashierCashReceipts_l = new Paragraph();
            Paragraph CurrentCashierCashReceipts_ll = new Paragraph();
            s = new Span(new Run("CASH ON HAND"));
            //s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
          //  s.Inlines.Add(new LineBreak());
            CurrentCashierCashReceipts_l.Inlines.Add(s);
            string gTotal="";

            Paragraph pp = new Paragraph();

            foreach (dynamic item in Denominations)
            {
                string type = item.type;
                string count = item.count;
                string total = item.total;
                string name = item.name;

                switch (type)
                {
                    case "riyal":
                       
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);
                        Floater f = new Floater(new Paragraph(new Run(name + " x " + count)), s.ContentStart)
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(f);

                        Floater ff = new Floater(new Paragraph(new Run(total)), s.ContentEnd)
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(ff);
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s); 
                        
                        break;
                    case "dirhams":
                      
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);
                        Floater f2 = new Floater(new Paragraph(new Run("0." + name + " x " + count)), s.ContentStart)
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(f2);

                        Floater ff2 = new Floater(new Paragraph(new Run(total)), s.ContentEnd)
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(ff2);
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);

                        //cashONhand_sp = cashONhand_size - (("0." + name + " x " + count).Length + total.Length);
                        //s = new Span(new Run("0." + name + " x " + count + total.PadLeft(cashONhand_sp)));
                        ////s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
                        //s.Inlines.Add(new LineBreak());
                        //CurrentCashierCashReceipts_l.Inlines.Add(s);
                        break;
                    case "cash" :
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);
                        Floater f3 = new Floater(new Paragraph(new Run("Cash")), s.ContentStart)
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(f3);

                        Floater ff3 = new Floater(new Paragraph(new Run(total)), s.ContentEnd)
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(ff3);
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);

                        //cashONhand_sp = 50 - (type.Length + total.Length)+5;
                        //s = new Span(new Run(type + total.PadLeft(cashONhand_sp)));
                        ////s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
                        //s.Inlines.Add(new LineBreak());
                        //CurrentCashierCashReceipts_l.Inlines.Add(s);
                        break;
                    case "complement":
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);
                        Floater fcomplement = new Floater(new Paragraph(new Run("Complement")), s.ContentStart)
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(fcomplement);

                        Floater ffcomplement = new Floater(new Paragraph(new Run(total)), s.ContentEnd)
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0)
                        };
                        CurrentCashierCashReceipts_l.Inlines.Add(ffcomplement);
                        s = new Span();
                        s.Inlines.Add(new LineBreak());
                        CurrentCashierCashReceipts_l.Inlines.Add(s);

                        //cashONhand_sp = cashONhand_size - (type.Length + total.Length)-2;
                        //s = new Span(new Run(type + total.PadLeft(cashONhand_sp)));
                        ////s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
                        //s.Inlines.Add(new LineBreak());
                        //CurrentCashierCashReceipts_l.Inlines.Add(s);
                        break;
                    case "total":
                        gTotal = total;
                        break;
                        
                    default:
                        break;
                }
            }

            s = new Span(new Run("---------------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            CurrentCashierCashReceipts_l.Inlines.Add(s);

            s = new Span();
            s.Inlines.Add(new LineBreak());
            CurrentCashierCashReceipts_l.Inlines.Add(s);
            Floater f5 = new Floater(new Paragraph(new Run("Total Sales")), s.ContentStart)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0)
            };
            CurrentCashierCashReceipts_l.Inlines.Add(f5);

            Floater ff5 = new Floater(new Paragraph(new Run(gTotal)), s.ContentEnd)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0)
            };
            CurrentCashierCashReceipts_l.Inlines.Add(ff5);
            s = new Span();
            s.Inlines.Add(new LineBreak());
            CurrentCashierCashReceipts_l.Inlines.Add(s);

            //int totat_sales_CashReceipts_l = 67 - ("Total Sales".Length + gTotal.Length);
            //s = new Span(new Run("Total Sales" + gTotal.PadLeft(totat_sales_CashReceipts_l)));
            ////s = new Span(new Run(Cashier + "\t" + "Time : " + DateTime.Now.ToShortTimeString()));
            //s.Inlines.Add(new LineBreak());
            //CurrentCashierCashReceipts_l.Inlines.Add(s);

            s = new Span(new Run("---------------------------------------------------"));//47
            s.Inlines.Add(new LineBreak());

            CurrentCashierCashReceipts_l.Inlines.Add(s);

            CurrentCashierCashReceipts_l.Margin = new Thickness(0);
            CurrentCashierCashReceipts_l.FontSize = 13;
            CurrentCashierCashReceipts_l.FontStyle = FontStyles.Normal;
            CurrentCashierCashReceipts_l.TextAlignment = TextAlignment.Justify;
            CurrentCashierCashReceipts_l.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            doc.Blocks.Add(CurrentCashierCashReceipts_l);

            #endregion

            //#region Payment_Details

            //Paragraph Payment_Details = new Paragraph();
            //s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            //Payment_Details.Inlines.Add(s);
            //int Payment_Details_sp = 40;
            //if (Invoice_Print.iscomplementary != "Y" && Invoice_Print.iscredit != "Y")
            //{
            //    string Sub_Total = "Sub Total";
            //    int Sub_Total_l = Sub_Total.Length;
            //    //string Sub_Total_val = Convert.ToDouble(Invoice_Print.grandtotal_round_off).ToString("0.00");
            //    string Sub_Total_val = subtotal.ToString("0.00");
            //    int Sub_Total_val_l = Sub_Total_val.Length;
            //    int Sub_Total_sp = Payment_Details_sp - (Sub_Total_l + Sub_Total_val_l) - 6;
            //    s = new Span(new Run(Sub_Total + Sub_Total_val.PadLeft(Sub_Total_sp)));
            //    //s = new Span(new Run("Sub Total" + (Convert.ToDouble(Invoice_Print.grandtotal_round_off).ToString("0.00")).PadLeft(20)));
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Details.Inlines.Add(s);

            //    string Discount = "Discount";
            //    int Discount_l = Discount.Length;
            //    //string Discount_val = Convert.ToDouble(Invoice_Print.grandtotal_round_off).ToString("0.00");
            //    string Discount_val = subtotal_dis.ToString("-0.00");
            //    int Discount_val_l = Discount_val.Length;
            //    int Discount_sp = Payment_Details_sp - (Discount_l + Discount_val_l) - 6;
            //    s = new Span(new Run(Discount + Discount_val.PadLeft(Discount_sp)));
            //    //s = new Span(new Run("Sub Total" + (Convert.ToDouble(Invoice_Print.grandtotal_round_off).ToString("0.00")).PadLeft(20)));
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Details.Inlines.Add(s);

            //    string Rounding_Discount = "Rounding Discount";
            //    int Rounding_Discount_l = Rounding_Discount.Length;
            //    string Rounding_Discount_val = "-" + Convert.ToDouble(Invoice_Print.lossamount).ToString("0.00");
            //    int Rounding_Discount_val_l = Rounding_Discount_val.Length;
            //    int Rounding_Discount_sp = Payment_Details_sp - (Rounding_Discount_l + Rounding_Discount_val_l) + 3;
            //    s = new Span(new Run(Rounding_Discount + Rounding_Discount_val.PadLeft(Rounding_Discount_sp)));
            //    //s = new Span(new Run("Rounding Discount" + (Convert.ToDouble(Invoice_Print.lossamount).ToString("-0.00")).PadLeft(20)));
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Details.Inlines.Add(s);
            //}
            //string Grand_Total = "Grand Total";
            //int Grand_Total_l = Grand_Total.Length;
            //string Grand_Total_val = Convert.ToDouble(Invoice_Print.grandtotal).ToString("0.00");
            //int Grand_Total_val_l = Grand_Total_val.Length;
            //int Grand_Total_sp = Payment_Details_sp - (Grand_Total_l + Grand_Total_val_l) - 4;
            //s = new Span(new Run(Grand_Total + Grand_Total_val.PadLeft(Grand_Total_sp)));
            ////s = new Span(new Run("Grand Total" + (Convert.ToDouble(Invoice_Print.grandtotal).ToString("0.00")).PadLeft(20)));
            //s.Inlines.Add(new LineBreak());
            //Payment_Details.Inlines.Add(s);
            //s = new Span(new Run("-----------------------------------------------"));//47
            ////s.Inlines.Add(new LineBreak());
            //Payment_Details.Inlines.Add(s);
            //Payment_Details.Margin = new Thickness(0);
            //Payment_Details.FontSize = 14;
            //Payment_Details.FontWeight = FontWeights.Bold;
            //Payment_Details.FontStyle = FontStyles.Normal;
            //Payment_Details.TextAlignment = TextAlignment.Right;
            //Payment_Details.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            //doc.Blocks.Add(Payment_Details);

            //#endregion Payment_Details

            //#region item_qy

            //Paragraph item_qy = new Paragraph();
            //s = new Span(new Run("No of Items : " + OrderDetails_Print.Count + "\t" + "Total Quantity : " + Invoice_Print.total_items_count));//47
            //s.Inlines.Add(new LineBreak());
            //item_qy.Inlines.Add(s);
            //item_qy.Margin = new Thickness(0);
            //item_qy.FontSize = 14;
            //item_qy.FontStyle = FontStyles.Normal;
            //item_qy.TextAlignment = TextAlignment.Center;
            //item_qy.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            //doc.Blocks.Add(item_qy);

            //#endregion item_qy

            //#region Payment_Method

            //int Payment_Method_sp = 66;
            //Paragraph Payment_Method = new Paragraph();
            //if (Invoice_Print.iscomplementary != "Y" && Invoice_Print.iscredit != "Y")
            //{
            //    if (Invoice_Print.cash != "0")
            //    {
            //        string cash = "Cash";
            //        int cash_l = cash.Length;
            //        string cash_val = Convert.ToDouble(Invoice_Print.cash).ToString("0.00");
            //        int cash_val_l = cash_val.Length;
            //        int cash_sp = Payment_Method_sp - (cash_l + cash_val_l) + 1;
            //        s = new Span(new Run(cash + cash_val.PadLeft(cash_sp)));
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //    if (Invoice_Print.card != "0")
            //    {
            //        string card = "Card";
            //        int card_l = card.Length;
            //        string card_val = Convert.ToDouble(Invoice_Print.card).ToString("0.00");
            //        int card_val_l = card_val.Length;
            //        int card_sp = Payment_Method_sp - (card_l + card_val_l) + 1;
            //        s = new Span(new Run(card + card_val.PadLeft(card_sp)));
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //    if (Invoice_Print.redemption != "0")
            //    {
            //        string redemption = "Redemption";
            //        int redemption_l = redemption.Length;
            //        string redemption_val = Convert.ToDouble(Invoice_Print.redemption).ToString("0.00");
            //        int redemption_val_l = redemption_val.Length;
            //        int redemption_sp = Payment_Method_sp - (redemption_l + redemption_val_l) - 5;
            //        s = new Span(new Run(redemption + redemption_val.PadLeft(redemption_sp)));
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //    if (Invoice_Print.exchange != "0")
            //    {
            //        string exchange = "Exchange";
            //        int exchange_l = exchange.Length;
            //        string exchange_val = Convert.ToDouble(Invoice_Print.exchange).ToString("0.00");
            //        int exchange_val_l = exchange_val.Length;
            //        int exchange_sp = Payment_Method_sp - (exchange_l + exchange_val_l) - 3;
            //        s = new Span(new Run(exchange + exchange_val.PadLeft(exchange_sp)));
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //    s = new Span(new Run("-----------------------------------------------"));//47
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Method.Inlines.Add(s);

            //    string Paid = "Paid";
            //    int Paid_l = Paid.Length;
            //    string Paid_val = (Convert.ToDouble(Invoice_Print.cash) + Convert.ToDouble(Invoice_Print.card) + Convert.ToDouble(Invoice_Print.redemption) + Convert.ToDouble(Invoice_Print.exchange)).ToString("0.00");
            //    int Paid_val_l = Paid_val.Length;
            //    int Paid_sp = Payment_Method_sp - (Paid_l + Paid_val_l) + 1;
            //    s = new Span(new Run(Paid + Paid_val.PadLeft(Paid_sp)));
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Method.Inlines.Add(s);

            //    string change = "Change";
            //    int change_l = change.Length;
            //    string change_val = Convert.ToDouble(Invoice_Print.change).ToString("0.00");
            //    int change_val_l = change_val.Length;
            //    int change_sp = Payment_Method_sp - (change_l + change_val_l) - 2;
            //    s = new Span(new Run(change + change_val.PadLeft(change_sp)));
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Method.Inlines.Add(s);
            //}
            //else
            //{
            //    s = new Span(new Run("-----------------------------------------------"));//47
            //    s.Inlines.Add(new LineBreak());
            //    Payment_Method.Inlines.Add(s);

            //    if (Invoice_Print.iscomplementary == "Y")
            //    {
            //        string Complementary = "COMPLEMENTARY";
            //        //int Complementary_l = Complementary.Length;
            //        //int Complementary_sp = (Payment_Method_sp - Complementary_l)/2;//26
            //        s = new Span(new Run("-------------" + Complementary + "-------------"));//15
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //    if (Invoice_Print.iscredit == "Y")
            //    {
            //        string credit = "CREDIT";
            //        //int credit_l = credit.Length;
            //        //int credit_sp = (Payment_Method_sp - credit_l)/2;//26
            //        s = new Span(new Run("-------------------" + credit + "--------------------"));//15
            //        s.Inlines.Add(new LineBreak());
            //        Payment_Method.Inlines.Add(s);
            //    }
            //}

            //s = new Span(new Run("-----------------------------------------------"));//47
            //s.Inlines.Add(new LineBreak());
            //Payment_Method.Inlines.Add(s);
            //Payment_Method.Margin = new Thickness(0);
            //Payment_Method.FontSize = 14;
            //Payment_Method.FontStyle = FontStyles.Normal;
            //Payment_Method.TextAlignment = TextAlignment.Left;
            //Payment_Method.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            //doc.Blocks.Add(Payment_Method);

            //#endregion Payment_Method

            #region Summary

            Paragraph Summary = new Paragraph();
            if (RetailPage.AD_ORG_footermessage != String.Empty)
            {
                s = new Span(new Run(RetailPage.AD_ORG_footermessage));
                s.Inlines.Add(new LineBreak());
                Summary.Inlines.Add(s);
            }

            if (RetailPage.AD_ORG_arabicfootermessage != String.Empty)
            {
                s = new Span(new Run(RetailPage.AD_ORG_arabicfootermessage));
                s.Inlines.Add(new LineBreak());
                Summary.Inlines.Add(s);
            }
            if (RetailPage.AD_ORG_termsmessage != String.Empty)
            {
                s = new Span(new Run(RetailPage.AD_ORG_termsmessage));
                s.Inlines.Add(new LineBreak());
                Summary.Inlines.Add(s);
            }
            if (RetailPage.AD_ORG_arabictermsmessage != String.Empty)
            {
                s = new Span(new Run(RetailPage.AD_ORG_arabictermsmessage));
                s.Inlines.Add(new LineBreak());
                Summary.Inlines.Add(s);
            }
            Summary.Margin = new Thickness(0);
            Summary.FontSize = 14;
            Summary.FontStyle = FontStyles.Normal;
            Summary.TextAlignment = TextAlignment.Center;
            Summary.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), baseFont);
            doc.Blocks.Add(Summary);

            #endregion Summary

            string docName = "SalesSummary" + DateTime.Now.ToString("ddmmyyyhhMMss");
            doc.Name = docName;
            doc.PageWidth = 270;
            doc.PagePadding = new Thickness(1);

            IDocumentPaginatorSource idpSource = doc;
            try
            {
                
              //  SerialPort.OpenDrawer();
                printDlg.PrintDocument(idpSource.DocumentPaginator, docName);
            }
            catch
            {
                NotifierViewModel.Notifier.ShowError(" Printer Not Connected");
                salesSummaryDetails_Print.Clear();
                return;
            }

            //Clearing Memory
            //OrderDetails_Print.Clear();
        }
    }
}
