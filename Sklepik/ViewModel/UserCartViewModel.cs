﻿using Domain;
using Domain.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Sklepik.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public class UserCartViewModel : BaseObservableObject
    {
        private readonly OrderHeaderModel _orderHeaderModel;
        private readonly IOrderRepository _orderRepository;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserCartViewModel(OrderHeaderModel orderHeaderModel, IOrderRepository orderRepository, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderHeaderModel = orderHeaderModel;
            _orderRepository = orderRepository;
            _authenticationStateProvider = authenticationStateProvider;
            ItemsInCart = new TrulyObservableCollection<OrderLineModel>(_orderHeaderModel.AvailableItems.Where(x => x.IsInCart == true).ToList());

            
            RecalculateOrder();

            ItemsInCart.CollectionChanged += ItemsInCart_CollectionChanged;
        }

      
        #region PROPERTIES
        private TrulyObservableCollection<OrderLineModel> _itemsInCart;

        public TrulyObservableCollection<OrderLineModel> ItemsInCart
        {
            get { return _itemsInCart; }
            set 
            { 
                _itemsInCart = value;
                NotifyPropertyChanged(nameof(ItemsInCart));
                
            }
        }

        private double _summaryOrderValue;

        public double SummaryOrderValue
        {
            get { return _summaryOrderValue; }
            set 
            { 
                _summaryOrderValue = value;
                NotifyPropertyChanged(nameof(SummaryOrderValue));
            }
        }

        public string Notification { get; set; }
        #endregion

        #region Public Methods

        public void RecalculateOrder()
        {
            SummaryOrderValue = Math.Round(ItemsInCart.Sum(x => x.ValueGross), 2);
        }

        public void RemoveFromCart(OrderLineModel model)
        {
            foreach (var item in _orderHeaderModel.AvailableItems.Where(x => x.ItemId == model.ItemId))
            {
                item.IsInCart = false;
                item.Quantity = 1;
            }
            ItemsInCart.Remove(model);
        }

        public async Task SaveOrder()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string user = authState.User.Identity.Name;

            List<OrderLineModelDto> list = new List<OrderLineModelDto>();
            foreach (var item in ItemsInCart)
            {
                list.Add(new OrderLineModelDto { 
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    PriceNet = item.PriceNet,
                    PriceGross = item.PriceGross,
                    Tax = item.Tax,
                    Quantity = item.Quantity
                });
            }

            try
            {
                _orderRepository.SaveOrder(user, Notification, ItemsInCart.Sum(x => x.ValueGross), list);
            }
            catch
            {
                new Exception("Save order failed");
            }
            finally
            {
                ClearDataAfterOrder();
            }
        }

        #endregion

        #region Private Methods

        private void ClearDataAfterOrder()
        {
            ItemsInCart.Clear();
            _orderHeaderModel.AvailableItems.Clear();
            Notification = string.Empty;
            SummaryOrderValue = 0;
        }

        private void ItemsInCart_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RecalculateOrder();
        }
        #endregion
    }
}