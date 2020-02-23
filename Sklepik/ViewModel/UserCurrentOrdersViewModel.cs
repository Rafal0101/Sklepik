﻿using AutoMapper;
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
    public class UserCurrentOrdersViewModel : BaseObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IModalService _modalService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserCurrentOrdersViewModel(IOrderRepository orderRepository, IModalService modalService, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderRepository = orderRepository;
            _modalService = modalService;
            _authenticationStateProvider = authenticationStateProvider;
            LoadOrdersList();
            
        }

        private async Task LoadOrdersList()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string user = authState.User.Identity.Name;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLineModelDto, OrderLineModel>();
                cfg.CreateMap<OrderHeaderModelDto, OrderHeaderModel>()
                .ForMember(dest => dest.AvailableItems, map => map.MapFrom(src => src.Items));
            });
            IMapper iMapper = config.CreateMapper();

            OrdersList = iMapper.Map<List<OrderHeaderModelDto>, List<OrderHeaderModel>>(_orderRepository.GetUserOrderList(user, OrderStatus.Submitted));
            OrdersList.AddRange(iMapper.Map<List<OrderHeaderModelDto>, List<OrderHeaderModel>>(_orderRepository.GetUserOrderList(user, OrderStatus.InReview)));
        }

        private List<OrderHeaderModel> _oredersList = new List<OrderHeaderModel>();

        public List<OrderHeaderModel> OrdersList
        {
            get { return _oredersList; }
            set
            { 
                _oredersList = value;
                NotifyPropertyChanged(nameof(OrdersList));
            }
        }
        public void DeleteOrder(OrderHeaderModel OrderHeaderModel)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            parameters.Add("order", OrderHeaderModel);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<DeleteIndyvidualOrderForm>("Czy na pewno chcesz usunąć zamówienie?", parameters, options);
        }

        private void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                OrderHeaderModel updated = (OrderHeaderModel)result.Data;
                _orderRepository.Delete(updated.Id);
                LoadOrdersList();
            }
            _modalService.OnClose -= _modalService_OnClose;
        }
    }
}