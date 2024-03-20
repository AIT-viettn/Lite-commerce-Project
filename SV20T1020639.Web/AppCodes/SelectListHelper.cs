using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020639.BusinessLayers;

namespace SV20T1020639.Web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem > list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn Tỉnh/Thành --"
            });
            foreach (var  item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName,
                });
            }    
            return list;
        }
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn loại hàng--"
            });
            foreach (var item in CommonDataService.ListOfCategories(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }
        /// <summary>
        /// Lấy danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn nhà cung cấp--"
            });
            foreach (var item in CommonDataService.ListOfSuppliers(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });
            }
            return list;
        }
        public static List<SelectListItem> Products()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn mặt hàng--"
            });
            foreach (var item in ProductDataServices.ListProducts(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProductID.ToString(),
                    Text = item.ProductName
                });
            }
            return list;
        }
        // <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Customers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn khách hàng--"
            });
            foreach (var item in CommonDataService.ListOfCustomers(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CustomerID.ToString(),
                    Text = item.CustomerName
                });
            }
            return list;
        }
       
        public static List<SelectListItem> Employees()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn nhân viên--"
            });
            foreach (var item in CommonDataService.ListOfEmployees(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.EmployeeID.ToString(),
                    Text = $"{item.FullName}"
                });
            }
            return list;
        }
        public static List<SelectListItem> Shippers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn đơn vị vận chuyển--"
            });
            foreach (var item in CommonDataService.ListOfShippers(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ShipperID.ToString(),
                    Text = $"{item.ShipperName}"
                });
            }
            return list;
        }
    }
}
