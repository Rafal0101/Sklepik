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
    public partial class VerifyOrdersPage : ComponentBase
    {
        [Inject]
        public VerifyOrderViewModel _verifyOrderViewModel { get; set; }

        public bool InEditUserOrdersMode { get; set; } = false;
        protected override void OnInitialized()
        {
            _verifyOrderViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }

        private void ClickEdit(string buyerId)
        {
            InEditUserOrdersMode = true;
            _verifyOrderViewModel.UpdateUserOrdersStatus(buyerId, StatusEnum.Submitted, StatusEnum.InReview);
            _verifyOrderViewModel.LoadOrdersList(buyerId);
        }

        private void RejectOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _verifyOrderViewModel.RejectOrder(sellerOrderHeaderModel);
        }
        private void AcceptOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _verifyOrderViewModel.ChangeOrderStatus(sellerOrderHeaderModel, StatusEnum.Accepted);
        }
 
        private void ReturnBack()
        {
            InEditUserOrdersMode = false;
        }
    }
}
