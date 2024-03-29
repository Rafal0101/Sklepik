﻿using Domain.Model;
using Domain.Statuses;
using System.Collections.Generic;

namespace Domain
{
    public interface IOrderRepository
    {
        void SaveOrder(string user, string notification, double summaryOrderValue, List<OrderLineModelDto> list);
        List<OrderHeaderModelDto> GetUserOrderList(StatusEnum[] statuses, string buyer = null, string seller = null, int id = 0);
        List<OrderSummaryModel> OrderHeadersInStatusGet(StatusEnum[] statuses);
        void ChangeOrderStatus(OrderHeaderModelDto modelDto, StatusEnum newStatus);
        void ChangeUserOrdersStatus(string BuyerId, StatusEnum fromStatus, StatusEnum intoStatus);
        void ChangeOrderStatusAsAccepted(OrderHeaderModelDto dtoModel, string SellerId);
        void Delete(int id);
    }
}