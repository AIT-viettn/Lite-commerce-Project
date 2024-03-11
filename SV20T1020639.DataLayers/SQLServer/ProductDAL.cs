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
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where ProductName = @ProductName)
                                select -1
                            else
                                begin
                                    insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                                    values(@ProductName,@ProductDescription,@SupplierID,@CategoryID,@Unit,@Price,@Photo,@IsSelling);

                                    select @@identity;
                                end";

                var parameters = new
                {
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }

        public long AddAtrribute(ProductAttribute data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from ProductAttributes where AttributeName = @AttributeName)
                                select -1
                            else
                                begin
                                    insert into ProductAttributes(ProductID,AttributeName,AttributeValue,DisplayOrder)
                                    values(@ProductID,@AttributeName,@AttributeValue,@DisplayOrder);

                                    select @@identity;
                                end";

                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from ProductPhotos where Photo = @Photo)
                                select -1
                            else
                                begin
                                    insert into ProductPhotos(ProductID,Photo,Description,DisplayOrder,IsHidden)
                                    values(@ProductID,@Photo,@Description,@DisplayOrder,@IsHidden);

                                    select @@identity;
                                end";

                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"SELECT COUNT(*) 
                    FROM Products 
                    WHERE (ProductName LIKE @SearchValue OR @SearchValue = N'')
                        AND (CategoryID = @CategoryID OR @CategoryID = 0)
                        AND (SupplierID = @SupplierID OR @SupplierID = 0)
                        AND (Price >= @MinPrice OR @MinPrice = 0)
                        AND (Price <= @MaxPrice OR @MaxPrice = 0)";

                var parameters = new
                {
                    SearchValue = searchValue ?? "",
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }


        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products where ProductID = @productID";
                var parameters = new
                {
                    ProductID = productID
                };
                // thực thi câu lệnh
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;

        }

        public bool DeleteAtrribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AttributeID = @attributeID";
                var parameters = new
                {
                    AttributeID = attributeID
                };
                // thực thi câu lệnh
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where PhotoID = @photoID";
                var parameters = new
                {
                    PhotoID = photoID
                };
                // thực thi câu lệnh
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductId = @productId";
                var parameters = new
                {
                    ProductID = productID
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where AttributeID = @attributeID";
                var parameters = new
                {
                    AttributeID = attributeID
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where PhotoID = @photoID";
                var parameters = new
                {
                    PhotoID = photoID
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails where ProductID = @ProductId)
                                select 1
                            else 
                                select 0";
                var parameters = new
                {
                    ProductID = productID
                };
                // thực thi câu lệnh
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
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
            List<ProductAttribute> list = new List<ProductAttribute>();

            using (var connection = OpenConnection())
            {
                var sql = @"WITH cte AS (
                        SELECT *,
                            ROW_NUMBER() OVER (ORDER BY AttributeName) AS RowNumber
                        FROM ProductAttributes
                        WHERE ProductID = @ProductID
                    )
                    SELECT * FROM cte";
                var parameters = new
                {
                    ProductID = productID
                };
                list = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> list = new List<ProductPhoto>();

            using (var connection = OpenConnection())
            {
                var sql = @"WITH cte AS (
                        SELECT *,
                            ROW_NUMBER() OVER (ORDER BY Photo) AS RowNumber
                        FROM ProductPhotos
                        WHERE ProductID = @ProductID
                    )
                    SELECT * FROM cte";
                var parameters = new
                {
                    ProductID = productID
                };
                list = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }

            return list;
        }

        public bool Update(Product data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Products where ProductID <> @ProductId and ProductName = @ProductName)
                                begin
                                    update Products 
                                    set ProductName = @ProductName,
                                        ProductDescription = @productDescription,
                                        SupplierID = @supplierID,
                                        CategoryID = @categoryID,
                                        Unit = @unit,
                                        Price = @price,
                                        Photo = @photo,
                                        IsSelling = @isSelling
                                        where ProductID = @productId
                               end";

                var parameters = new
                {
                    ProductId = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        public bool UpdateAtrribute(ProductAttribute data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from ProductAttributes where AttributeID <> @attributeId and AttributeName = @attributeName)
                                begin
                                    update ProductAttributes 
                                    set ProductID = @productId,
                                        AttributeName = @attributeName,
                                        AttributeValue = @attributeValue,
                                        DisplayOrder = @displayOrder
                                        where AttributeID = @attributeId
                               end";

                var parameters = new
                {
                    AttributeID = data.AttributeID,
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from ProductPhotos where PhotoID <> @photoId and Photo = @photo)
                                begin
                                    update ProductPhotos 
                                    set ProductID = @productId,
                                        Photo = @photo,
                                        Description = @description,
                                        DisplayOrder = @displayOrder,
                                        IsHidden = @isHidden
                                        where PhotoID = @photoId
                               end";

                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }
    }
}
