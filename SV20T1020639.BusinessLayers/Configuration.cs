

namespace SV20T1020639.BusinessLayers
{
    public static class Configuration
    {
        /// <summary>
        /// chuỗi thông số kết nối CSDL
        /// </summary>
        public static string ConnectionString { get; private set; } = "";
        /// <summary>
        /// hàm khởi tạo cấu hình cho BusinessLayer
        /// Hàm này phải được gọi trước khi chạy ứng dụng   
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}