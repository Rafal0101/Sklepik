using Dapper;
using Domain;
using Domain.Model;
using Domain.States;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public OrderRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public void SaveOrder(string buyer, string notification, double summaryOrderValue, List<OrderLineModelDto> list)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                cnn.Open();
                using (var transaction = cnn.BeginTransaction())
                {

                    string sqlHeader = $"INSERT INTO OrderHeader (BuyerId, Status, Notification, SummaryValue) " +
                        $"VALUES (@BuyerId, @Status, @Notification, @SummaryValue);" +
                        $"SELECT CAST(SCOPE_IDENTITY() as int)";

                    var result = cnn.ExecuteScalar(sqlHeader,
                        new { BuyerId = buyer, Status = OrderStatusDictionary.GetStatus[OrderStatus.Submitted], Notification = notification, SummaryValue = summaryOrderValue },
                        transaction: transaction);

                    int OrderHeaderId = Convert.ToInt32(result);

                    foreach (var line in list)
                    {
                        string sqlLine = $"INSERT INTO OrderLine ([OrderHeaderId], [ItemId], [ItemName], [PriceNet], [PriceGross], [Tax], [Quantity]) " +
                                        $"VALUES (@OrderHeaderId, @ItemId, @ItemName, @PriceNet, @PriceGross, @Tax, @Quantity)";

                        cnn.Execute(sqlLine,
                            new { OrderHeaderId, line.ItemId, line.ItemName, line.PriceNet, line.PriceGross, line.Tax, line.Quantity },
                            transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }


        public List<OrderHeaderModelDto> GetUserOrderList(OrderStatus[] statuses, string buyer = null, string seller = null, int id = 0)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                List<OrderHeaderModelDto> headers = new List<OrderHeaderModelDto>();
                foreach (var status in statuses)
                {
                    headers.AddRange(cnn.Query<OrderHeaderModelDto>("dbo.OrderHeaderGet"
                        , param: new { BuyerId = buyer, Status = OrderStatusDictionary.GetStatus[status], SellerId = seller, Id = id }
                        , commandType: CommandType.StoredProcedure).ToList());

                    foreach (var item in headers)
                    {
                        string sql = "SELECT [Id], [OrderHeaderId], [ItemId], [ItemName], [PriceNet], [PriceGross], [Tax], [Quantity]" +
                            "FROM [Sklepik].[dbo].[OrderLine] WHERE OrderHeaderId = @OrderHeaderId";
                        item.Items = cnn.Query<OrderLineModelDto>(sql, param: new { OrderHeaderId = item.Id }).ToList();
                    }
                }
                return headers;
            }
        }

        public List<OrderSummaryModel> OrderHeadersInStatusGet(OrderStatus[] statuses)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sql = $"SELECT BuyerId, COUNT(Id) Qty, SUM(SummaryValue) SummaryValue FROM [dbo].[OrderHeader] WHERE ";

                for (int i = 0; i < statuses.Length; i++)
                {
                    if (i > 0)
                    {
                        sql += " OR ";
                    }
                    sql += $"Status = {OrderStatusDictionary.GetStatus[statuses[i]]}";
                }
                sql += " GROUP BY BuyerId";

                List<OrderSummaryModel> headers = cnn.Query<OrderSummaryModel>(sql).ToList();

                return headers;
            }
        }

        public void ChangeUserOrdersStatus(string BuyerId, OrderStatus fromStatus, OrderStatus intoStatus)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sql = $"UPDATE [dbo].[OrderHeader] SET Status = {OrderStatusDictionary.GetStatus[intoStatus]} " +
                    $"WHERE BuyerId = '{BuyerId}' AND Status = {OrderStatusDictionary.GetStatus[fromStatus]}";

                cnn.Execute(sql);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sql = $"DELETE FROM [Sklepik].[dbo].[OrderHeader] WHERE Id = {id}";
                cnn.Execute(sql);
            }
        }

        //public List<OrderHeaderModel> GetOrderList(string buyer = null, OrderStatus status = OrderStatus.Submitted, string seller = null, int id = 0)
        //{
        //    using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
        //    {
        //        var orderDictionary = new Dictionary<int, OrderHeaderModel>();


        //        var result = cnn.Query<OrderHeaderModel, List<OrderLineModel>, OrderHeaderModel>("dbo.OrderHeaderGet",
        //            (header, lines) =>
        //            {
        //                OrderHeaderModel orderEntry;

        //                if (!orderDictionary.TryGetValue(header.Id, out orderEntry))
        //                {
        //                    orderEntry = header;
        //                    orderEntry.Items = new List<OrderLineModel>();
        //                    orderDictionary.Add(orderEntry.Id, orderEntry);
        //                }
        //                orderEntry.Items.AddRange(lines);
        //                return orderEntry;

        //            }
        //            , splitOn: "Id, OrderHeaderId"
        //            , param: new { BuyerId = buyer, Status = OrderStatusDictionary.GetStatus[status], SellerId = seller, Id = id }
        //            , commandType: CommandType.StoredProcedure);

        //        return null;
        //    }
        //}



        //public List<OrderHeaderModel> GetOrderList(string buyer = null, OrderStatus status = OrderStatus.Submitted, string seller = null, int id = 0)
        //{
        //    using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
        //    {
        //        var result = cnn.Query<OrderHeaderModel, List<OrderLineModel>, OrderHeaderModel>("dbo.OrderHeaderGet",
        //            (header, lines) => { header.Items = lines; return header; }
        //            , splitOn: "Id, OrderHeaderId"
        //            , param: new { BuyerId = buyer, Status = OrderStatusDictionary.GetStatus[status], SellerId = seller, Id = id}
        //            , commandType: CommandType.StoredProcedure);

        //        return null;
        //    }
        //}

    }
}
