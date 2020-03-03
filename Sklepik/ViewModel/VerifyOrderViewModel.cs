using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Domain.Model;
using Domain.Statuses;
using Microsoft.AspNetCore.Components.Authorization;
using Sklepik.Model;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public partial class VerifyOrderViewModel : BaseObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IModalService _modalService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public VerifyOrderViewModel(IOrderRepository orderRepository, IModalService modalService, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderRepository = orderRepository;
            _modalService = modalService;
            _authenticationStateProvider = authenticationStateProvider;
            LoadSummaryOrdersList();
        }

        private void LoadSummaryOrdersList()
        {
            StatusEnum[] statuses = new StatusEnum[] { StatusEnum.Submitted, StatusEnum.InReview };
            SummaryOrdersList = _orderRepository.OrderHeadersInStatusGet(statuses);
        }


        public void LoadOrdersList(string buyerId)
        {
             var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLineModelDto, SellerOrderLineModel>();
                cfg.CreateMap<OrderHeaderModelDto, SellerOrderHeaderModel>()
                .ForMember(dest => dest.Items, map => map.MapFrom(src => src.Items));
            });
            IMapper iMapper = config.CreateMapper();

            StatusEnum[] statuses = new StatusEnum[] { StatusEnum.Submitted, StatusEnum.InReview };
            SellerOrdersList = iMapper.Map<List<OrderHeaderModelDto>, TrulyObservableCollection<SellerOrderHeaderModel>>(_orderRepository.GetUserOrderList(statuses, buyerId));
            SellerOrdersList.ToList().ForEach(x => { x.Items.ToList().ForEach(y => { y.AcceptedQty = y.SubmittedQty; }); });
        }



        internal void UpdateUserOrdersStatus(string buyerId, StatusEnum submitted, StatusEnum inReview)
        {
            CurrentBuyerId = buyerId;
            _orderRepository.ChangeUserOrdersStatus(buyerId, submitted, inReview);
        }

        public void RejectOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            UserOrderHeaderModel userOrderHeaderModel = new UserOrderHeaderModel
            {
                Id = sellerOrderHeaderModel.Id,
                CreationDateFormatted = sellerOrderHeaderModel.CreationDateFormatted,
                SummaryValue = sellerOrderHeaderModel.SummaryValue,
                BuyerId = sellerOrderHeaderModel.BuyerId
            };
            parameters.Add("order", userOrderHeaderModel);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<DeleteIndyvidualOrderForm>("Czy na pewno musisz odrzucić zamówienie?", parameters, options);
        }

        private void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                UserOrderHeaderModel updated = (UserOrderHeaderModel)result.Data;
                
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserOrderLineModel, OrderLineModelDto>();
                    cfg.CreateMap<UserOrderHeaderModel, OrderHeaderModelDto>();
                });
                IMapper iMapper = config.CreateMapper();

                OrderHeaderModelDto modelDto = iMapper.Map<UserOrderHeaderModel, OrderHeaderModelDto>(updated);
                _orderRepository.ChangeOrderStatus(modelDto, StatusEnum.Rejected);
                LoadOrdersList(updated.BuyerId);
            }
            _modalService.OnClose -= _modalService_OnClose;
        }

        internal async void ChangeOrderStatus(SellerOrderHeaderModel sellerOrderHeaderModel, StatusEnum status)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string seller = authState.User.Identity.Name;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SellerOrderLineModel, OrderLineModelDto>()
                .ForMember(dest => dest.SubmittedQty, map => map.MapFrom(src => src.AcceptedQty));
                cfg.CreateMap<SellerOrderHeaderModel, OrderHeaderModelDto>();
            });
            IMapper iMapper = config.CreateMapper();

            OrderHeaderModelDto modelDto = iMapper.Map<SellerOrderHeaderModel, OrderHeaderModelDto>(sellerOrderHeaderModel);
            if (status == StatusEnum.Accepted)
            {
                _orderRepository.ChangeOrderStatusAsAccepted(modelDto, seller);
            }
            else
            {
                _orderRepository.ChangeOrderStatus(modelDto, status);
            }
            LoadOrdersList(CurrentBuyerId);
        }
    }
}
