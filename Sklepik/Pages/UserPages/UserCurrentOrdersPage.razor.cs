using Microsoft.AspNetCore.Components;
using Sklepik.Model;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.UserPages
{
    public partial class UserCurrentOrdersPage : ComponentBase
    {
        [Inject]
        public UserCurrentOrdersViewModel _myCurrentOrdersViewModel { get; set; }

        protected override void OnInitialized()
        {
            _myCurrentOrdersViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await _myCurrentOrdersViewModel.LoadOrdersList();
        }

        void ClickDel(UserOrderHeaderModel userOrderHeaderModel)
        {
            _myCurrentOrdersViewModel.DeleteOrder(userOrderHeaderModel);
        }
    }
}
