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
    public partial class CategoryForm
    {
        [Inject]
        IModalService ModalService { get; set; }

        [CascadingParameter]
        public ModalParameters Parameters { get; set; }
        bool ShowForm { get; set; } = true;
        CategoryModel category { get; set; }


        protected override void OnInitialized()
        {
            category = Parameters.Get<CategoryModel>("category");
        }

        void SubmitForm()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Ok<CategoryModel>(category));
        }


        void Cancel()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Cancel());
        }
    }
}
