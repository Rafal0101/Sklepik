using Microsoft.AspNetCore.Components;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.UserPages
{
    public partial class UserOrderPage : ComponentBase
    {
        [Inject]
        public UserOrderViewModel _userOrderViewModel { get; set; }

       
        protected override void OnInitialized()
        {
            _userOrderViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }
    }
}
