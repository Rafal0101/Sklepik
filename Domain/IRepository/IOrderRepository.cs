using Domain.Model;
using Domain.Statuses;
using System.Collections.Generic;

namespace Domain
{
    public interface IOrderRepository
    {
        void SaveOrder(string user, string notification, double summaryOrderValue, List<OrderLineModelDto> list);
        List<OrderHeaderModelDto> GetUserOrderList(StatusEnum[] statuses, string buyer = null, string seller = null, int id = 0);
        List<OrderSummaryModel> OrderHeadersInStatusGet(StatusEnum[] statuses);
        void ChangeUserOrdersStatus(string BuyerId, StatusEnum fromStatus, StatusEnum intoStatus);
        void Delete(int id);
    }
}