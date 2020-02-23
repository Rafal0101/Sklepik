using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class OrderHeaderModelDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public int Status { get; set; }
        public string Notification { get; set; }
        public double SummaryValue { get; set; }
        public string MergedId { get; set; }
        public DateTime CreationDate { get; set; }

        public string CreationDateFormatted
        {
            get
            {
                return CreationDate.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public List<OrderLineModelDto> Items { get; set; } = new List<OrderLineModelDto>();

    }
}
