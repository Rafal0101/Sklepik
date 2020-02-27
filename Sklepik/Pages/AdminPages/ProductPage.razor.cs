using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Sklepik.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.AdminPages
{
    public partial class ProductPage : ComponentBase
    {
        [Inject]
        public ProductViewModel _productViewModel { get; set; }

        protected override void OnInitialized()
        {
            _productViewModel.PropertyChanged += (obj, args) => StateHasChanged();
        }

        async Task ReadExcelPriceList(IFileListEntry[] files)
        {
            IFileListEntry file;
            file = files.FirstOrDefault();
            var ms = new MemoryStream();
            await file.Data.CopyToAsync(ms);
            _productViewModel.PriceListFileBody = ms;
            file = null;
        }
    }
}
