using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Domain.Model;
using Domain.States;
using Sklepik.Model;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public class SellerCurrentOrderViewModel : BaseObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IModalService _modalService;

        public SellerCurrentOrderViewModel(IOrderRepository orderRepository, IModalService modalService)
        {
            _orderRepository = orderRepository;
            _modalService = modalService;

            LoadSummaryOrdersList();
        }

        private void LoadSummaryOrdersList()
        {
            SummaryOrdersList = _orderRepository.OrderHeadersInStatusGet(OrderStatus.Submitted);
            SummaryOrdersList.AddRange(_orderRepository.OrderHeadersInStatusGet(OrderStatus.InReview));
        }

        private List<OrderSummaryModel> _summaryOrdersList;

        public List<OrderSummaryModel> SummaryOrdersList
        {
            get { return _summaryOrdersList; }
            set 
            { 
                _summaryOrdersList = value;
                NotifyPropertyChanged(nameof(SummaryOrdersList));
            }
        }
        public void LoadOrdersList(string buyerId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLineModelDto, OrderLineModel>();
                cfg.CreateMap<OrderHeaderModelDto, OrderHeaderModel>()
                .ForMember(dest => dest.AvailableItems, map => map.MapFrom(src => src.Items));
            });
            IMapper iMapper = config.CreateMapper();

            OrdersList = iMapper.Map<List<OrderHeaderModelDto>, List<OrderHeaderModel>>(_orderRepository.GetUserOrderList(buyerId, OrderStatus.Submitted));
            OrdersList.AddRange(iMapper.Map<List<OrderHeaderModelDto>, List<OrderHeaderModel>>(_orderRepository.GetUserOrderList(buyerId, OrderStatus.InReview)));
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

        internal void UpdateUserOrdersStatus(string buyerId, OrderStatus submitted, OrderStatus inReview)
        {
            _orderRepository.ChangeUserOrdersStatus(buyerId, submitted, inReview);
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
                LoadOrdersList(updated.BuyerId);
            }
            _modalService.OnClose -= _modalService_OnClose;
        }
    }
}
