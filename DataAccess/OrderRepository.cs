using Dapper;
using Domain;
using Domain.Model;
using Domain.Statuses;
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

        public void SaveOrder(string buyer, string buyerNotification, double summaryOrderValue, List<OrderLineModelDto> list)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                cnn.Open();
                using (var transaction = cnn.BeginTransaction())
                {

                    string sqlHeader = $"INSERT INTO OrderHeader (Number, BuyerId, Status, BuyerNotification, SummaryValue) " +
                        $"VALUES ((select dbo.GetOrderNumber()), @BuyerId, @Status, @BuyerNotification, @SummaryValue);" +
                        $"SELECT CAST(SCOPE_IDENTITY() as int)";

                    var result = cnn.ExecuteScalar(sqlHeader,
                        new
                        {
                            BuyerId = buyer
                        ,
                            Status = Const.StatusesList.Where(x => x.Status == StatusEnum.Submitted).FirstOrDefault().StatusId
                        ,
                            BuyerNotification = buyerNotification
                        ,
                            SummaryValue = summaryOrderValue
                        },
                        transaction: transaction);

                    int OrderHeaderId = Convert.ToInt32(result);

                    foreach (var line in list)
                    {
                        string sqlLine = $"INSERT INTO OrderLine ([OrderHeaderId], [ItemId], [ItemName], [PriceNet], [PriceGross], [Tax], [SubmittedQty]) " +
                                        $"VALUES (@OrderHeaderId, @ItemId, @ItemName, @PriceNet, @PriceGross, @Tax, @SubmittedQty)";

                        cnn.Execute(sqlLine,
                            new { OrderHeaderId, line.ItemId, line.ItemName, line.PriceNet, line.PriceGross, line.Tax, line.SubmittedQty },
                            transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }


        public List<OrderHeaderModelDto> GetUserOrderList(StatusEnum[] statuses, string buyer = null, string seller = null, int id = 0)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                List<OrderHeaderModelDto> headers = new List<OrderHeaderModelDto>();
                foreach (var status in statuses)
                {
                    //int testStatus = Const.StatusesList.Where(x => x.Status == StatusEnum.Submitted).FirstOrDefault().StatusId;

                    headers.AddRange(cnn.Query<OrderHeaderModelDto>("dbo.OrderHeaderGet"
                        , param: new
                        {
                            BuyerId = buyer
                        ,
                            Status = Const.StatusesList.Where(x => x.Status == status).FirstOrDefault().StatusId
                        ,
                            SellerId = seller
                        ,
                            Id = id
                        }
                        , commandType: CommandType.StoredProcedure).ToList());

                    foreach (var item in headers)
                    {
                        string sql = "SELECT [Id], [OrderHeaderId], [ItemId], [ItemName], [PriceNet], [PriceGross], [Tax], [SubmittedQty], [AcceptedQty]" +
                            "FROM [Sklepik].[dbo].[OrderLine] WHERE OrderHeaderId = @OrderHeaderId";
                        item.Items = cnn.Query<OrderLineModelDto>(sql, param: new { OrderHeaderId = item.Id }).ToList();
                    }
                }
                return headers;
            }
        }

        public List<OrderSummaryModel> OrderHeadersInStatusGet(StatusEnum[] statuses)
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
                    sql += $"Status = {Const.StatusesList.Where(x => x.Status == statuses[i]).FirstOrDefault().StatusId}";
                }
                sql += " GROUP BY BuyerId";

                List<OrderSummaryModel> headers = cnn.Query<OrderSummaryModel>(sql).ToList();

                return headers;
            }
        }

        public void ChangeOrderStatus(OrderHeaderModelDto dtoModel, StatusEnum newStatus)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sqlHead = $"UPDATE [dbo].[OrderHeader] SET Status = @Status WHERE Id = @Id";

                cnn.Execute(sqlHead, new { Status = Const.StatusesList.Where(x => x.Status == newStatus).FirstOrDefault().StatusId, dtoModel.Id });
            }
        }

        public void ChangeOrderStatusAsAccepted(OrderHeaderModelDto dtoModel, string SellerId)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sqlHead = $"UPDATE [dbo].[OrderHeader] SET Status = @Status, SellerId = @SellerId, " +
                    $"SummaryValue = @SummaryValue, SellerNotification = @SellerNotification WHERE Id = @Id";

                cnn.Execute(sqlHead, new
                {
                    Status = Const.StatusesList.Where(x => x.Status == StatusEnum.Accepted).FirstOrDefault().StatusId,
                    SellerId,
                    dtoModel.SummaryValue,
                    dtoModel.SellerNotification,
                    dtoModel.Id
                });

                foreach (var item in dtoModel.Items)
                {
                    string sqlLine = $"UPDATE [dbo].[OrderLine] SET AcceptedQty = {item.AcceptedQty} " +
                                $"WHERE Id = {item.Id}";
                    cnn.Execute(sqlLine);
                }

            }
        }

        public void ChangeUserOrdersStatus(string BuyerId, StatusEnum fromStatus, StatusEnum intoStatus)
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sql = $"UPDATE [dbo].[OrderHeader] SET Status = {Const.StatusesList.Where(x => x.Status == intoStatus).FirstOrDefault().StatusId} " +
                    $"WHERE BuyerId = '{BuyerId}' AND Status = {Const.StatusesList.Where(x => x.Status == fromStatus).FirstOrDefault().StatusId}";

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
