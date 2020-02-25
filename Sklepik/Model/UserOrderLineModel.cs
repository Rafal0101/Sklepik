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


        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set 
            {
                _quantity = value;
                NotifyPropertyChanged(nameof(Quantity));
            }
        }

        public bool IsInCart { get; set; }
        public double ValueGross
        {
            get
            {
                 return Math.Round((Quantity * PriceGross), 2);
            }
        }
    }
}
