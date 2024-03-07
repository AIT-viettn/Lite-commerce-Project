using Dapper;
using SV20T1020639.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020639.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            throw new NotImplementedException();
        }

        public long AddAtrribute(ProductAttribute data)
        {
            throw new NotImplementedException();
        }

        public long AddPhoto(ProductPhoto data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAtrribute(long productID)
        {
            throw new NotImplementedException();
        }

        public bool DeletePhoto(long productID)
        {
            throw new NotImplementedException();
        }

        public Product? Get(int productID)
        {
            throw new NotImplementedException();
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            throw new NotImplementedException();
        }

        public ProductPhoto? GetPhoto(long productID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int productID)
        {
            throw new NotImplementedException();
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> list = new List<Product>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%"; //Viet => %Viet%
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as(
                                select  *,
                                    row_number() over(order by ProductName) as RowNumber
                                from Products
                                where   (@SearchValue = N'' or ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)
                                )
                                select * from cte
                                where   (@PageSize = 0)
                                    or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page *
                                @PageSize) ";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? "",
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                list = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }
        public IList<ProductAttribute> ListAttributes(int productID)
        {
            throw new NotImplementedException();
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Product data)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAtrribute(ProductAttribute data)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            throw new NotImplementedException();
        }
    }
}
