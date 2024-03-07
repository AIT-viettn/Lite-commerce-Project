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
    public static class ProductDataServices
    {
        private static readonly IProductDAL productDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataServices()
        {
            productDB  = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng ( không phân trang )
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List(searchValue).ToList();    
        }
    }
}
