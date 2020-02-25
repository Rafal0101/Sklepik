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
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyPropertyChanged(nameof(Quantity));
            }
        }

        public double ValueGross
        {
            get
            {
                return Math.Round((Quantity * PriceGross), 2);
            }

        }

        private int _newQuantity;
        public int NewQuantity
        {
            get { return _newQuantity; }
            set
            {

                _newQuantity = value;

                if (_newQuantity < 1)
                {
                    _newQuantity = 1;
                }
                else if (_newQuantity > Quantity)
                {
                    _newQuantity = Quantity;
                }

                NotifyPropertyChanged(nameof(NewQuantity));
            }
        }

        public double NewValueGross
        {
            get
            {
                return Math.Round((NewQuantity * PriceGross), 2);
            }
        }
    }
}
