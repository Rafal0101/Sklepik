using Domain.Model;
using Domain.States;
using System.Collections.Generic;

namespace Domain
{
    public interface IOrderRepository
    {
        void SaveOrder(string user, string notification, double summaryOrderValue, List<OrderLineModelDto> list);
        List<OrderHeaderModelDto> GetUserOrderList(OrderStatus[] statuses, string buyer = null, string seller = null, int id = 0);
        List<OrderSummaryModel> OrderHeadersInStatusGet(OrderStatus[] statuses);
        void ChangeUserOrdersStatus(string BuyerId, OrderStatus fromStatus, OrderStatus intoStatus);
        void Delete(int id);
    }
}