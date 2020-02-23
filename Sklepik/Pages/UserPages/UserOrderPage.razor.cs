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
        public UserOrderViewModel _myOrderViewModel { get; set; }

       
        protected override void OnInitialized()
        {
            _myOrderViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }
    }
}
