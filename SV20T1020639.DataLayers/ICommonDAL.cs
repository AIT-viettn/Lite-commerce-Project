using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020639.DataLayers
{
    /// <summary>
    /// mô tả các phép xử lí dữ liệu " chung chung " (generic)
    /// </summary>
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tim kiem, lay danh sach du lieu duoi dang phan trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số lượng dòng dữ liệu tìm kiếm được
        /// </summary>
        /// <param name="searchValue">giá trị tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Lấy một bản ghi/ dòng dữ liệu trên mã id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);
        /// <summary>
        /// Bổ sung dữ liệu vào trong CSDL. Hàm trả về
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa một bản ghi dữ liệu dựa vào id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// kiểm tra xem một bản ghi dữ liệu có mã id hiện đang có được sử dụng hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        bool IsUsed(int id);
    }
}
// thuan tuy//hoan toan là abstract