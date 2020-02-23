using Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sklepik.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public class UserOrderViewModel : BaseObservableObject
    {

        private readonly IProductRepository _productRepository;
        private readonly OrderHeaderModel _orderHeaderModel;


        public UserOrderViewModel(IProductRepository productRepository, OrderHeaderModel orderHeaderModel)
        {
            _productRepository = productRepository;
            _orderHeaderModel = orderHeaderModel;

            GetAllProducts();
        }

        private void GetAllProducts()
        {
            if (_orderHeaderModel.AvailableItems.Count == 0)
            {
                foreach (var item in _productRepository.GetAll())
                {
                    _orderHeaderModel.AvailableItems.Add(new OrderLineModel
                    {
                        CategoryName = item.PrimaryCategory.Name,
                        ItemId = item.ItemId,
                        ItemName = item.Name,
                        PriceNet = item.PriceNet,
                        PriceGross = item.PriceGross,
                        Tax = item.Tax,
                        Quantity = 1,
                        IsInCart = false
                    });
                }
            }

            ProductList = _orderHeaderModel.AvailableItems;
        }

        public List<OrderLineModel> ProductList { get; set; }
        //private List<MyOrderModel> _productsList = new List<MyOrderModel>();
        //public List<MyOrderModel> ProductList
        //{
        //    get { return _productsList; }
        //    set
        //    {
        //        _productsList = value;
        //        NotifyPropertyChanged(nameof(ProductList));
        //    }
        //}

        //public void AddToCart(MyOrderModel product)
        //{
        //    int test = 1;
        //}


    }
}


/*
         //private readonly IPurchaseRepository _purchaseRepository;
        //private readonly AuthenticationStateProvider _authenticationStateProvider;





                //MyPurchases.PurchaseItems.Add(new MyPurchaseModel 
            //{ CategoryName = product.CategoryName,
            //ItemId = product.ItemId,
            //Name = product.Name,
            //PriceGross = product.PriceGross,
            //Quantity = 1,
            //IsInCart = product.IsInCart
            //});




            //var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            //string user = authState.User.Identity.Name;

            //int test = _purchaseRepository.SavePurchaseHeader(user, 10);
     * */
