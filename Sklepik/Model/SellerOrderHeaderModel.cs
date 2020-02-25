using Domain.States;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Model
{
    public class SellerOrderHeaderModel : BaseObservableObject
    {
        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SummaryValue = Math.Round(Items.Sum(x => x.NewValueGross), 2);
        }

        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public int Status { get; set; }
        public string Notification { get; set; }

        private double _summaryValue;

        public double SummaryValue
        {
            get { return _summaryValue; }
            set 
            { 
                _summaryValue = value;
                NotifyPropertyChanged(nameof(SummaryValue));
            }
        }

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

                switch (OrderStatusDictionary.GetStatus.FirstOrDefault(x => x.Value == Status).Key)
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

        private TrulyObservableCollection<SellerOrderLineModel> _items;
        public TrulyObservableCollection<SellerOrderLineModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyPropertyChanged(nameof(Items));

                if (_items != null)
                    Items.CollectionChanged += Items_CollectionChanged;
            }
        }

    }
}
