using Microsoft.AspNetCore.Components;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.UserPages
{
    public partial class UserCartPage : ComponentBase
    {
        [Inject]
        public UserCartViewModel _userCartViewModel { get; set; }


        protected override void OnInitialized()
        {
            _userCartViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }
        
    }
}
