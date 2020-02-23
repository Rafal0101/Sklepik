using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class OrderSummaryModel
    {
        public string BuyerId { get; set; }
        public int Qty { get; set; }
        public double SummaryValue { get; set; }

        public double SummaryValueFormatted { 
            get 
            {
                return Math.Round(SummaryValue, 2);
            }
            //set { } 
        }
    }
}
