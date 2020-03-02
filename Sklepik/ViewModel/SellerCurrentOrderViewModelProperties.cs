using Domain.Model;
using Domain.Statuses;
using Sklepik.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public partial class SellerCurrentOrderViewModel
    {
        private List<OrderSummaryModel> _summaryOrdersList;

        public List<OrderSummaryModel> SummaryOrdersList
        {
            get { return _summaryOrdersList; }
            set
            {
                _summaryOrdersList = value;
                NotifyPropertyChanged(nameof(SummaryOrdersList));
            }
        }

        private TrulyObservableCollection<SellerOrderHeaderModel> _sellerOrdersList;
        public TrulyObservableCollection<SellerOrderHeaderModel> SellerOrdersList
        {
            get { return _sellerOrdersList; }
            set 
            { 
                _sellerOrdersList = value;
                NotifyPropertyChanged(nameof(SellerOrdersList));
            }
        }

        public string CurrentBuyerId { get; set; }
    }
}
