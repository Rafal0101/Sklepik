using Domain.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public static class Const
    {
        public static List<OrderStatus> StatusesList = new List<OrderStatus>
        {
            new OrderStatus { Status = StatusEnum.Submitted, StatusId = 10, StatusName = "10 - Złożone" },
            new OrderStatus { Status = StatusEnum.InReview, StatusId = 20, StatusName  = "20 - W weryfikacji" },
            new OrderStatus { Status = StatusEnum.Rejected, StatusId = 30, StatusName  = "30 - Odrzucone" },
            new OrderStatus { Status = StatusEnum.PartiallyAccepted, StatusId = 40, StatusName = "40 - Zaakceptowane częściowo" },
            new OrderStatus { Status = StatusEnum.Accepted, StatusId = 50, StatusName  = "50 - Zaakceptowane" },
            new OrderStatus { Status = StatusEnum.Paid, StatusId = 60, StatusName  = "60 - Opłacone" },
            new OrderStatus { Status = StatusEnum.Delivered, StatusId = 70, StatusName  = "60 - Dostarczone" }

        };
    }
}
