using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Restaurant_Pos
{
    public class LoginViewModel : BaseViewModel
    {
        public static string[] _DeviceMacAddress_arr = NetworkInterface
         .GetAllNetworkInterfaces()
         .Where(nic => nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
         .Select(nic => nic.GetPhysicalAddress().ToString()).ToArray();

        //public static string _DeviceMacAddress = NetworkInterface
        // .GetAllNetworkInterfaces()
        // .Where(nic => nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        // .Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();

        public static int _DeviceMacAddress_count = NetworkInterface
         .GetAllNetworkInterfaces()
         .Where(nic => nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
         .Select(nic => nic.GetPhysicalAddress().ToString()).Count();

        public static string DeviceMacAddress()//CPU ID
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
    }

    public class POSorganizationApiModel
    {


#pragma warning disable IDE1006 // Naming Styles
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string operation { get; set; }
        public string username { get; set; }
        public string password { get; set; }




        //public string userId { get; set; }
        //public string businessPartnerId { get; set; }
        //public string clientId { get; set; }
        //public string roleId { get; set; }
        //public string orgId { get; set; }
        //public string warehouseId { get; set; }




#pragma warning restore IDE1006 // Naming Styles
    }

    public class M_UOM
    {
        public int UOMId { get; set; }
        public string Name { get; set; }
    }
    public class M_ItemCategory
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
    }
    public class M_warehouse
    {
        public int WarehouseId { get; set; }
        public int Ad_client_id { get; set; }
        public int Ad_org_id { get; set; }
        public int M_warehouse_id { get; set; }
        public string Isactive { get; set; }
        public string Isdefault { get; set; }
        public string WarehouseName { get; set; }
        public int WarehouePriceListId { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
    public class M_user
    {
        public int UserID { get; set; }
        public string Name { get; set; }

    }

    public class M_Roles
    {
        public int RoleID { get; set; }
        public string Name { get; set; }

    }
    public class M_Country
    {
        public int CountryID { get; set; }
        public string Name { get; set; }

    }
    public class M_City
    {
        public int CityID { get; set; }
        public string Name { get; set; }

    }

    public class m_ProductList
    {
        public int id { get; set; }
        public string Name { get; set; }

    }
    public class m_ProductKey
    {
        public int id { get; set; }
        public string Name { get; set; }

    }


    public class m_ProductLst
    {
        public int id { get; set; }
        public string Name { get; set; }

    }
    public class m_ProductWise
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }


    }


    public class m_ProductCATWise
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class m_onHoldInvoice
    {
        public int c_invoice_id { get; set; }
        public string grandtotal { get; set; }
        public string total_items_count { get; set; }

    }
    public class m_TakeAwayProduct
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }

    }

    public class m_TakeAwayProductCAT
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }

    }
    public class m_floorTables
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string tablegroup { get; set; }
        public bool coversLevel { get; set; }
        public string floor_No { get; set; }

    }

    //Api mode
    public class PostVendorInvoicePayments
    {
  public string macAddress  { get; set; }
  public int clientId {get; set;}
  public string orgId {get; set;}
  public string warehouseId   {get; set;}
  public string userId     {get; set;}
  public string version      {get; set;}
  public string appName      {get; set;}
  public string operation     {get; set;}
  public string businessPartnerId  {get; set;}
  public string description {get; set;}
  public string cashbookId   {get; set;}
  public string tenderType   {get; set;}
  public string payAmount  {get; set;}
  public string isCustomer   {get; set;}
  public string isVendor   {get; set;}
public List<apiInvoiceList> invoiceList { get; set; }
  }

  public class apiInvoiceList { 
  public string payAmount   {get; set;}
  public string grandTotal  {get; set;}
  public string invoiceDate {get; set;}
  public string posId       {get; set;}
  public string invoiceId   {get; set;}
  public string documentNo { get; set; }
    }
    

    //Api model end
    public class m_TakeAwayProductRS
    {
        public string checkbox { get; set; }

        public m_TakeAwayProductRS()
        {
            checkbox = "Hidden";
        }
        public int id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ItemCount { get; set; }
        public double discountPer { get; set; }

        public double TotalPrice
        {
            get
            {
                double x = (ItemCount * (int)Price) - ((discountPer * (ItemCount * (int)Price)) / 100);
                return System.Math.Round(x, 2);
            }

        }

    }
    public class m_TakeAwayProductValue
    {
        public int value { get; set; }


    }

    public class m_TerminalsList
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class m_TerminalsWise
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class m_ProductCATLst
    {
        public int id { get; set; }
        public string Name { get; set; }

    }
    public class ImgPath
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class Img
    {

        public string imgpath { get; set; }
    }
    public class UserData
    {

        public string UserName { get; set; }

    }

    public class POSCashCustomer
    {
#pragma warning disable IDE1006 // Naming Styles
        public string operation { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string userId { get; set; }
        public string clientId { get; set; }
        public string orgId { get; set; }
        public string roleId { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class POSCustomers
    {
#pragma warning disable IDE1006 // Naming Styles
        public string operation { get; set; }
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string clientId { get; set; }
        public string orgId { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class POSCategory
    {
#pragma warning disable IDE1006 // Naming Styles
        public string operation { get; set; }
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string clientId { get; set; }
        public string orgId { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class POSOrderNumber
    {
#pragma warning disable IDE1006 // Naming Styles
        public string operation { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string userId { get; set; }
        public string businessPartnerId { get; set; }
        public string clientId { get; set; }
        public string roleId { get; set; }
        public string orgId { get; set; }
        public string warehouseId { get; set; }
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class POSAllProducts
    {
#pragma warning disable IDE1006 // Naming Styles
        public string operation { get; set; }
        public string remindMe { get; set; }
        public string macAddress { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string clientId { get; set; }
        public string orgId { get; set; }
        public string pricelistId { get; set; }
        public string costElementId { get; set; }

        public string warehouseId { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }


}