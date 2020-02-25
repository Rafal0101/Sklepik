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
    public class UserOrderHeaderModel : BaseObservableObject
    {

        public int Id { get; set; }
        private string _buyerId;

        public string BuyerId
        {
            get { return _buyerId; }
            set 
            { 
                _buyerId = value;
                NotifyPropertyChanged(nameof(BuyerId));
            }
        }

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

        private List<UserOrderLineModel> _availableItems = new List<UserOrderLineModel>();
        [SourceMember("Items")]
        public List<UserOrderLineModel> AvailableItems
        {
            get { return _availableItems; }
            set
            {
                _availableItems = value;
                NotifyPropertyChanged(nameof(AvailableItems));
            }
        }

        public bool CanBeDeleted
        {
            get
            {
                bool result = false;

                switch (OrderStatusDictionary.GetStatus.FirstOrDefault(x => x.Value == Status).Key)
                { 
                    case OrderStatus.InReview:
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
                return result;
            }
        }
    }
}
