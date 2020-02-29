using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Statuses
{
    public class OrderStatus
    {
        public StatusEnum Status { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
