using AutoMapper.Configuration.Annotations;
using Domain;
using Domain.Model;
using Domain.Statuses;
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

        public string Number { get; set; }


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
        public string BuyerNotification { get; set; }
        public string SellerNotification { get; set; }
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

        public string StatusName
        {
            get
            {
                return Const.StatusesList.Where(x => x.StatusId == Status).FirstOrDefault().StatusName;
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

        public string Visibility
        {
            get
            {
                switch (Const.StatusesList.Where(x => x.StatusId == Status).FirstOrDefault().Status)
                {
                    case StatusEnum.Submitted:
                        return "visible";
                    default:
                        return "invisible";
                }
            }
        }
        public bool CanBeDeleted
        {
            get
            {
                bool result = false;

                switch (Const.StatusesList.Where(x => x.StatusId == Status).FirstOrDefault().Status)
                {
                    case StatusEnum.InReview:
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
