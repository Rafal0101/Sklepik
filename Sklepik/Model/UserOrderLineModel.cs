using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Model
{
    public class UserOrderLineModel : BaseObservableObject
    {
        public string CategoryName { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double PriceGross { get; set; }
        public double PriceNet { get; set; }
        public int Tax { get; set; }


        private int _submittedQty;
        public int SubmittedQty
        {
            get { return _submittedQty; }
            set 
            {
                _submittedQty = value;
                NotifyPropertyChanged(nameof(SubmittedQty));
            }
        }

        private int _acceptedQty;
        public int AcceptedQty
        {
            get { return _acceptedQty; }
            set
            {
                _acceptedQty = value;
                NotifyPropertyChanged(nameof(AcceptedQty));
            }
        }

        public bool IsInCart { get; set; }
        public double ValueGross
        {
            get
            {
                 return Math.Round((SubmittedQty * PriceGross), 2);
            }
        }

        public bool IsVisible { get; set; } = true;
    }
}
