using AutoMapper.Configuration.Annotations;
using Domain.Model;
using Domain.States;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Model
{
    public class OrderHeaderModel : BaseObservableObject
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
            set { }
        }

        public string StatusFormatted
        {
            get
            {
                string result = string.Empty;

                switch(OrderStatusDictionary.GetStatus.FirstOrDefault(x => x.Value == Status).Key)
                {
                    case OrderStatus.Submitted:
                        result = "10 - Złożone";
                        break;
                    case OrderStatus.InReview:
                        result = "20 - W weryfikacji";
                        break;
                    case OrderStatus.Accepted:
                        result = "30 - Zaakceptowane";
                        break;
                    case OrderStatus.Rejected:
                        result = "40 - Odrzucone";
                        break;
                }
                return result;
            }
        }


        private List<OrderLineModel> _availableItems = new List<OrderLineModel>();
        
        [SourceMember("Items")]
        public List<OrderLineModel> AvailableItems
        {
            get { return _availableItems; }
            set
            {
                _availableItems = value;
                NotifyPropertyChanged(nameof(AvailableItems));
            }
        }

    }
}
