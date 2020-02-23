using MatBlazor;
using Microsoft.AspNetCore.Components;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.AdminPages
{
    public partial class CategoryPage : ComponentBase
    {
        [Inject]
        public CategoryViewModel _categoryViewModel { get; set; }

        protected override void OnInitialized()
        {

            _categoryViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }
   }
}
