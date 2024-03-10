using SV20T1020639.DataLayers;
using SV20T1020639.DataLayers.SQLServer;
using SV20T1020639.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020639.BusinessLayers
{
    /// <summary>
    /// Cung cấp các chức năng nghiệp vụ liên quan đến các dữ liệu "chung"
    /// (tỉnh / thành , khách hàng , nhà cung cấp , loại hàng , người giao hàng , nhân viên ) 
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Category> categoryDB;

        /// <summary>
        /// Ctor ( Câu hỏi static constructor hoạt động như thế nào ? cách viết ? )
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;
            provinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
        }
        //Province
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }

        //Customer
        /// <summary>
        /// Tìm kiếm và lấy danh sách
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được </param>
        /// <param name="page">Trang cần hiển thị </param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ khách hàng ) </param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của một khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 khách hàng mới . Hàm trả về mã của khách hàng mới được bổ sung 
        /// (Hàm trả về -1 nếu email bị trùng , trả về giá trị 0 nếu lỗi)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin khách hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 khách hàng (nếu khách hàng đó dữ liệu liên quan) (bảng Order)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.IsUsed(id))
            {
                return false;
            }
            return customerDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem 1 khách hàng hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }


        //Supplier
        /// <summary>
        /// Tìm kiếm và lấy danh sách Nhà Cung Cấp
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được </param>
        /// <param name="page">Trang cần hiển thị </param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ nhà cung cấp ) </param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách Nhà Cung Cấp không phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers( string searchValue = "")
        {
            return supplierDB.List().ToList();
        }
        /// <summary>
        /// Lấy thông tin 1 nhà cung cấp thông qua mã nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }

        /// <summary>
        /// Bổ sung nhà cung cấp mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        /// <summary>
        /// Cập nhật nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }

        /// <summary>
        /// Xóa nhà cung cấp có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            if (supplierDB.IsUsed(id))
            {
                return false;
            }
            else
            {
                return supplierDB.Delete(id);
            }
        }

        /// <summary>
        /// Kiểm tra xem có mã id có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

        //Shipper
        /// <summary>
        /// Tìm kiếm và lấy danh sách Người giao hàng
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được</param>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ người giao hàng )</param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        ///  Lấy thông tin của một người giao hàng theo mã giao hàng
        /// </summary>
        /// <param name="id">mã giao hàng</param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1  người giao hàng mới . Hàm trả về mã của người giao hàng mới được bổ sung 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Xóa người giao hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            /*if(shipperDB.IsUsed(id))
            {
                return false;
            }    */
            return shipperDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem 1 khách hàng hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }


        // Employees    
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được </param>
        /// <param name="page">Trang cần hiển thị </param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ nhân viên ) </param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của một nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Thêm nhân viên mới . Trả về nhân viên mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Xóa nhân viên có mã id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.IsUsed(id))
            {
                return false;
            }
            return employeeDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem nhân viên có mã id hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }

        // Category
        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng không phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories( string searchValue = "")
        {
            return categoryDB.List().ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 loại hàng thông qua mã loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }

        /// <summary>
        /// Bổ sung loại hàng mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        /// <summary>
        /// Cập nhật loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        /// <summary>
        /// Xóa loại hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.IsUsed(id))
            {
                return false;
            }
            else
            {
                return categoryDB.Delete(id);
            }
        }

        /// <summary>
        /// Kiểm tra loại hàng có mã id có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }

    }
}
