using Blazored.Modal;
using Blazored.Modal.Services;
using Domain.Model;
using Microsoft.AspNetCore.Components;
using Sklepik.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.Forms
{
    public partial class DeleteIndyvidualOrderForm
    {
        [Inject]
        IModalService ModalService { get; set; }

        [CascadingParameter]
        public ModalParameters Parameters { get; set; }
        bool ShowForm { get; set; } = true;
        UserOrderHeaderModel OrderModel { get; set; }


        protected override void OnInitialized()
        {
            OrderModel = Parameters.Get<UserOrderHeaderModel>("order");
        }

        void SubmitForm()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Ok<UserOrderHeaderModel>(OrderModel));
        }


        void Cancel()
        {
            ShowForm = false;
            ModalService.Close(ModalResult.Cancel());
        }
    }
}
