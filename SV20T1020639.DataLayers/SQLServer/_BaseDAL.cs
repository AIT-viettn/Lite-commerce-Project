using Microsoft.Data.SqlClient;

namespace SV20T1020639.DataLayers.SQLServer
{
    /// <summary>
    /// Lớp cha của các lớp cài đặt các phép xử lí dự liệu trên SQLServer
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _ConnetionString = " ";
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connetionString"></param>
        public _BaseDAL(string connetionString)
        {
            _ConnetionString = connetionString;
        }
        /// <summary>
        /// tao va mo ket noi
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _ConnetionString;
            connection.Open();
            return connection;
        }
    }
}
