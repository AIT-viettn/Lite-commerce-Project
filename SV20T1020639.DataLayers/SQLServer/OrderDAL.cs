using Dapper;
using SV20T1020639.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SV20T1020639.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL, IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Order data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Orders(CustomerID, OrderTime, DeliveryProvince, DeliveryAddress, EmployeeID, Status)
                            VALUES(@CustomerID, GETDATE(), @DeliveryProvince, @DeliveryAddress, @EmployeeID, @Status);
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";
                var parameters = new
                {
                    CustomerID = data.CustomerID ,
                    OrderTime = data.OrderTime,
                    DeliveryProvince = data.DeliveryProvince,
                    DeliveryAddress = data.DeliveryAddress,
                    EmployeeID = data.EmployeeID,
                    Status = data.Status
                    
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT COUNT(*)
                            FROM Orders AS o
                            LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID
                            LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID
                            WHERE (@Status = 0 OR o.Status = @Status)
                            AND (@FromTime IS NULL OR o.OrderTime >= @FromTime)
                            AND (@ToTime IS NULL OR o.OrderTime <= @ToTime)
                            AND (@SearchValue = N'' OR c.CustomerName LIKE @SearchValue OR e.FullName LIKE @SearchValue OR s.ShipperName LIKE @SearchValue)";
                var parameters = new { Status = status, FromTime = fromTime, ToTime = toTime, SearchValue = searchValue };
                count = connection.ExecuteScalar<int>(sql, parameters);
            }
            return count;
        }

        public bool Delete(int orderID)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM OrderDetails WHERE OrderID = @OrderID;
                            DELETE FROM Orders WHERE OrderID = @OrderID";
                var parameters = new { OrderID = orderID };
                return connection.Execute(sql, parameters) > 0;
            }
        }

        public bool DeleteDetail(int orderID, int productID)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM OrderDetails WHERE OrderID = @OrderID AND ProductID = @ProductID";
                var parameters = new { OrderID = orderID, ProductID = productID };
                return connection.Execute(sql, parameters) > 0;
            }
        }

        public Order? Get(int orderID)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT o.*, c.CustomerName, c.ContactName AS CustomerContactName, c.Address AS CustomerAddress,
                            c.Phone AS CustomerPhone, c.Email AS CustomerEmail, e.FullName AS EmployeeName,
                            s.ShipperName, s.Phone AS ShipperPhone
                            FROM Orders AS o
                            LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID
                            LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID
                            WHERE o.OrderID = @OrderID";
                var parameters = new { OrderID = orderID };
                return connection.QueryFirstOrDefault<Order>(sql, parameters);
            }
        }

        public OrderDetail? GetDetail(int orderID, int productID)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT od.*, p.ProductName, p.Photo, p.Unit
                            FROM OrderDetails AS od
                            JOIN Products AS p ON od.ProductID = p.ProductID
                            WHERE od.OrderID = @OrderID AND od.ProductID = @ProductID";
                var parameters = new { OrderID = orderID, ProductID = productID };
                return connection.QueryFirstOrDefault<OrderDetail>(sql, parameters);
            }
        }

        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"WITH cte AS (
                            SELECT ROW_NUMBER() OVER(ORDER BY o.OrderTime DESC) AS RowNumber, o.*, c.CustomerName,
                            c.ContactName AS CustomerContactName, c.Address AS CustomerAddress, c.Phone AS CustomerPhone,
                            c.Email AS CustomerEmail, e.FullName AS EmployeeName, s.ShipperName, s.Phone AS ShipperPhone
                            FROM Orders AS o
                            LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID
                            LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID
                            WHERE (@Status = 0 OR o.Status = @Status)
                            AND (@FromTime IS NULL OR o.OrderTime >= @FromTime)
                            AND (@ToTime IS NULL OR o.OrderTime <= @ToTime)
                            AND (@SearchValue = N'' OR c.CustomerName LIKE @SearchValue OR e.FullName LIKE @SearchValue OR s.ShipperName LIKE @SearchValue)
                            )
                            SELECT * FROM cte
                            WHERE (@PageSize = 0) OR (RowNumber BETWEEN (@Page - 1) * @PageSize + 1 AND @Page * @PageSize)
                            ORDER BY RowNumber";
                var parameters = new { Status = status, FromTime = fromTime, ToTime = toTime, SearchValue = searchValue, Page = page, PageSize = pageSize };
                return connection.Query<Order>(sql, parameters).ToList();
            }
        }

        public IList<OrderDetail> ListDetails(int orderID)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT od.*, p.ProductName, p.Photo, p.Unit
                            FROM OrderDetails AS od
                            JOIN Products AS p ON od.ProductID = p.ProductID
                            WHERE od.OrderID = @OrderID";
                var parameters = new { OrderID = orderID };
                return connection.Query<OrderDetail>(sql, parameters).ToList();
            }
        }

        public bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS(SELECT * FROM OrderDetails WHERE OrderID = @OrderID AND ProductID = @ProductID)
                            BEGIN
                                UPDATE OrderDetails
                                SET Quantity = @Quantity, SalePrice = @SalePrice
                                WHERE OrderID = @OrderID AND ProductID = @ProductID
                            END
                            ELSE
                            BEGIN
                                INSERT INTO OrderDetails(OrderID, ProductID, Quantity, SalePrice)
                                VALUES(@OrderID, @ProductID, @Quantity, @SalePrice)
                            END";
                var parameters = new { OrderID = orderID, ProductID = productID, Quantity = quantity, SalePrice = salePrice };
                return connection.Execute(sql, parameters) > 0;
            }
        }

        public bool Update(Order data)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Orders
                            SET CustomerID = @CustomerID, OrderTime = @OrderTime, DeliveryProvince = @DeliveryProvince,
                            DeliveryAddress = @DeliveryAddress, EmployeeID = @EmployeeID, AcceptTime = @AcceptTime,
                            ShipperID = @ShipperID, ShippedTime = @ShippedTime, FinishedTime = @FinishedTime, Status = @Status
                            WHERE OrderID = @OrderID";
                return connection.Execute(sql, data) > 0;
            }
        }
    }
}
