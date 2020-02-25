using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.States
{
    public static class OrderStatusDictionary
    {
        public static Dictionary<OrderStatus, int> GetStatus = new Dictionary<OrderStatus, int>
        {
            { OrderStatus.All, -1 },
            { OrderStatus.Submitted, 10},
            { OrderStatus.InReview, 20},
            { OrderStatus.Accepted, 30 },
            { OrderStatus.Rejected, 99}

        };
    }
}
