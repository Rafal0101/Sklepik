using Domain;
using Domain.Statuses;
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
        public string Number { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public int Status { get; set; }
        public string BuyerNotification { get; set; }
        public string SellerNotification { get; set; }

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

        public string StatusName
        {
            get
            {
                return Const.StatusesList.Where(x => x.StatusId == Status).FirstOrDefault().StatusName;
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
