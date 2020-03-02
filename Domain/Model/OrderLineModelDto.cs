using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class OrderLineModelDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double PriceNet { get; set; }
        public double PriceGross { get; set; }
        public int Tax { get; set; }
        public int SubmittedQty { get; set; }
        public int AcceptedQty { get; set; }
    }
}
