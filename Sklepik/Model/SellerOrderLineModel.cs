using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Model
{
    public class SellerOrderLineModel : BaseObservableObject 
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double PriceGross { get; set; }
        public double PriceNet { get; set; }
        public int Tax { get; set; }


        private int _quantity;
        public int SubmittedQty
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyPropertyChanged(nameof(SubmittedQty));
            }
        }

        public double ValueGross
        {
            get
            {
                return Math.Round((SubmittedQty * PriceGross), 2);
            }

        }

        private int _acceptedQty;
        public int AcceptedQty
        {
            get { return _acceptedQty; }
            set
            {

                _acceptedQty = value;

                if (_acceptedQty < 0)
                {
                    _acceptedQty = 0;
                }
                else if (_acceptedQty > SubmittedQty)
                {
                    _acceptedQty = SubmittedQty;
                }

                NotifyPropertyChanged(nameof(AcceptedQty));
            }
        }

        public double NewValueGross
        {
            get
            {
                return Math.Round((AcceptedQty * PriceGross), 2);
            }
        }
    }
}
