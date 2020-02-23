using Domain.Model;
using Domain.States;
using System.Collections.Generic;

namespace Domain
{
    public interface IOrderRepository
    {
        void SaveOrder(string user, string notification, double summaryOrderValue, List<OrderLineModelDto> list);
        List<OrderHeaderModelDto> GetUserOrderList(string buyer = null, OrderStatus status = OrderStatus.Submitted, string seller = null, int id = 0);
        List<OrderSummaryModel> OrderHeadersInStatusGet(OrderStatus status = OrderStatus.Submitted);
        void ChangeUserOrdersStatus(string BuyerId, OrderStatus fromStatus, OrderStatus intoStatus);
        void Delete(int id);
    }
}