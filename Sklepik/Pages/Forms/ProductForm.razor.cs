using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.Forms
{
    public partial class ProductForm
    {
        [Inject]
        IModalService ModalService { get; set; }
        [CascadingParameter] ModalParameters Parameters { get; set; }


        bool ShowForm { get; set; } = true;
        ProductModel product { get; set; }
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
       
        protected override void OnInitialized()
        {
            product = Parameters.Get<ProductModel>("product");
            Categories = Parameters.Get<List<CategoryModel>>("categories");
        }

        void SubmitForm()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Ok<ProductModel>(product));
        }


        void Cancel()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Cancel());
        }
        void CalculateGross()
        {
            product.PriceGross = product.PriceNet + (product.PriceNet * product.Tax / 100);
        }

        void CategorySelectionChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value.ToString(), out int id))
            {
                product.CategoryId = id;
            }
        }
    }
}
