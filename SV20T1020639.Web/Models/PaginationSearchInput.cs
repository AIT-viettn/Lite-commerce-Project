namespace SV20T1020639.Web.Models
{
    /// <summary>
    /// Đầu vào tìm kiếm dữ liệu để nhận dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public String SearchValue { get; set; } = "";
    }
    /// <summary>
    /// đầu vào tìm kiếm dành cho mặt hàng
    /// </summary>
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int EmployeeID { get; set;} = 0;
        public int ShipperID { get; set; } = 0;
        public int CustomerID { get; set; } = 0;
        public string ProvinceName { get; set; } = "";

    }

}
