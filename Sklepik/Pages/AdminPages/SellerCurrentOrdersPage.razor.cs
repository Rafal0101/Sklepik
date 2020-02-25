using Domain.States;
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
            _sellerCurrentOrderViewModel.UpdateUserOrdersStatus(buyerId, OrderStatus.Submitted, OrderStatus.InReview);
            _sellerCurrentOrderViewModel.LoadOrdersList(buyerId);
        }
        void ClickDel(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _sellerCurrentOrderViewModel.DeleteOrder(sellerOrderHeaderModel);
        }

        void ClickDelOrderPosition(SellerOrderLineModel sellerOrderLineModel)
        {
            _sellerCurrentOrderViewModel.DeleteOrderPosition(sellerOrderLineModel);
        }
    }
}
