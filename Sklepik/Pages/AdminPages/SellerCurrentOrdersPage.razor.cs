using Domain.Statuses;
using Microsoft.AspNetCore.Components;
using Sklepik.Model;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.AdminPages
{
    public partial class SellerCurrentOrdersPage : ComponentBase
    {
        [Inject]
        public SellerCurrentOrderViewModel _sellerCurrentOrderViewModel { get; set; }

        public bool InEditUserOrdersMode { get; set; } = false;
        protected override void OnInitialized()
        {
            _sellerCurrentOrderViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }

        public void ClickEdit(string buyerId)
        {
            InEditUserOrdersMode = true;
            _sellerCurrentOrderViewModel.UpdateUserOrdersStatus(buyerId, StatusEnum.Submitted, StatusEnum.InReview);
            _sellerCurrentOrderViewModel.LoadOrdersList(buyerId);
        }

        void RejectOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _sellerCurrentOrderViewModel.RejectOrder(sellerOrderHeaderModel);
        }
        void AcceptOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _sellerCurrentOrderViewModel.ChangeOrderStatus(sellerOrderHeaderModel, StatusEnum.Accepted);
        }
 
    }
}
