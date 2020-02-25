using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Domain.Model;
using Domain.States;
using Microsoft.AspNetCore.Components.Authorization;
using Sklepik.Model;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public partial class UserCurrentOrdersViewModel : BaseObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IModalService _modalService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserCurrentOrdersViewModel(IOrderRepository orderRepository, IModalService modalService, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderRepository = orderRepository;
            _modalService = modalService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        #region PUBLIC METHODS

        public async Task LoadOrdersList()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string user = authState.User.Identity.Name;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLineModelDto, UserOrderLineModel>();
                cfg.CreateMap<OrderHeaderModelDto, UserOrderHeaderModel>()
                .ForMember(dest => dest.AvailableItems, map => map.MapFrom(src => src.Items));
            });
            IMapper iMapper = config.CreateMapper();

            OrderStatus[] statuses = new OrderStatus[] { OrderStatus.Submitted, OrderStatus.InReview };
            OrdersList = iMapper.Map<List<OrderHeaderModelDto>, List<UserOrderHeaderModel>>(_orderRepository.GetUserOrderList(statuses, user));
        }


        public void DeleteOrder(UserOrderHeaderModel userOrderHeaderModel)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            parameters.Add("order", userOrderHeaderModel);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<DeleteIndyvidualOrderForm>("Czy na pewno chcesz usunąć zamówienie?", parameters, options);
        }


        #endregion


        #region PRIVATE METHODS
        private async void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                UserOrderHeaderModel updated = (UserOrderHeaderModel)result.Data;
                _orderRepository.Delete(updated.Id);
                await LoadOrdersList();
            }
            _modalService.OnClose -= _modalService_OnClose;
        }

        #endregion
    }
}
