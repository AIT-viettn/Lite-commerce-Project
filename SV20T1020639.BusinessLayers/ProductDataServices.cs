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
            return productDB.List().ToList();    
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", 
            int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue);
            return productDB.List(page, pageSize, searchValue,categoryId,supplierId,minPrice,maxPrice).ToList();
        }

        public static Product GetProduct(int productId) 
        {
            return productDB.Get(productId);
        }
    }
}
