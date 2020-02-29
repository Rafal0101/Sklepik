using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Domain.Model;
using Domain.Statuses;
using Sklepik.Model;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public partial class SellerCurrentOrderViewModel : BaseObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IModalService _modalService;

        public SellerCurrentOrderViewModel(IOrderRepository orderRepository, IModalService modalService)
        {
            _orderRepository = orderRepository;
            _modalService = modalService;
            LoadSummaryOrdersList();
        }

        private void OrdersList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int test = 1;
        }

        private void LoadSummaryOrdersList()
        {
            StatusEnum[] statuses = new StatusEnum[] { StatusEnum.Submitted, StatusEnum.InReview };
            SummaryOrdersList = _orderRepository.OrderHeadersInStatusGet(statuses);
        }


        public void LoadOrdersList(string buyerId)
        {
            //List<OrderHeaderModelDto> list = _orderRepository.GetUserOrderList(buyerId, OrderStatus.Submitted);

            //foreach (var item in list)
            //{
            //    List<OrderLineModelDto> lines = item.Items;

            //    OrdersList.Add(new SellerOrderHeaderModel
            //    {
            //        Id = item.Id,
            //        BuyerId = item.BuyerId,
            //        SellerId = item.SellerId,
            //        Notification = item.Notification,
            //        Status = item.Status,
            //        SummaryValue = item.SummaryValue
            //    });
            //}

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLineModelDto, SellerOrderLineModel>();
                cfg.CreateMap<OrderHeaderModelDto, SellerOrderHeaderModel>()
                .ForMember(dest => dest.Items, map => map.MapFrom(src => src.Items));
            });
            IMapper iMapper = config.CreateMapper();

            StatusEnum[] statuses = new StatusEnum[] { StatusEnum.Submitted, StatusEnum.InReview };
            SellerOrdersList = iMapper.Map<List<OrderHeaderModelDto>, TrulyObservableCollection<SellerOrderHeaderModel>>(_orderRepository.GetUserOrderList(statuses, buyerId));
            //OrdersList.AddRange(iMapper.Map<List<OrderHeaderModelDto>, TrulyObservableCollection<SellerOrderHeaderModel>>(_orderRepository.GetUserOrderList(buyerId, OrderStatus.InReview)));
            //OrdersList.ForEach(x => { x.AvailableItems.ForEach(y => { y.NewQuantity = y.Quantity; }); });
            //OrdersList = new TrulyObservableCollection<OrderHeaderModel>(RawOrdersList);
            SellerOrdersList.ToList().ForEach(x => { x.Items.ToList().ForEach(y => { y.NewQuantity = y.Quantity; }); });
            SellerOrdersList.CollectionChanged += OrdersList_CollectionChanged;

        }



        internal void UpdateUserOrdersStatus(string buyerId, StatusEnum submitted, StatusEnum inReview)
        {
            _orderRepository.ChangeUserOrdersStatus(buyerId, submitted, inReview);
        }

        public void DeleteOrder(SellerOrderHeaderModel sellerOrderHeaderModel)
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
            _modalService.Show<DeleteIndyvidualOrderForm>("Czy na pewno chcesz usunąć zamówienie?", parameters, options);
        }

        private void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                UserOrderHeaderModel updated = (UserOrderHeaderModel)result.Data;
                _orderRepository.Delete(updated.Id);
                LoadOrdersList(updated.BuyerId);
            }
            _modalService.OnClose -= _modalService_OnClose;
        }

        public void DeleteOrderPosition(SellerOrderLineModel orderLineModel)
        {
            var list = SellerOrdersList.SingleOrDefault(x => x.Id == orderLineModel.OrderHeaderId);
            list.Items.Remove(orderLineModel);
        }
    }
}
