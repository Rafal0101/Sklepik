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
        private readonly UserOrderHeaderModel _userOrderHeaderModel;


        public UserOrderViewModel(IProductRepository productRepository, UserOrderHeaderModel userOrderHeaderModel)
        {
            _productRepository = productRepository;
            _userOrderHeaderModel = userOrderHeaderModel;


            GetAllProducts();
        }

        private void GetAllProducts()
        {
            if (_userOrderHeaderModel.AvailableItems.Count == 0)
            {
                foreach (var item in _productRepository.GetAll())
                {
                    _userOrderHeaderModel.AvailableItems.Add(new UserOrderLineModel
                    {
                        CategoryName = item.PrimaryCategory.Name,
                        ItemId = item.ItemId,
                        ItemName = item.Name,
                        PriceNet = item.PriceNet,
                        PriceGross = item.PriceGross,
                        Tax = item.Tax,
                        SubmittedQty = 1,
                        IsInCart = false
                    });
                }
            }

            ProductList = _userOrderHeaderModel.AvailableItems;
        }

        public List<UserOrderLineModel> ProductList { get; set; }

        private string _searchingPattern;
        public string SearchingPattern
        {
            get { return _searchingPattern; }
            set 
            { 
                _searchingPattern = value;
                NotifyPropertyChanged(nameof(SearchingPattern));
                for (int i = 0; i < ProductList.Count; i++)
                {
                    if (!ProductList[i].ItemId.ToLower().Contains(_searchingPattern.ToLower()) 
                        && !ProductList[i].ItemName.ToLower().Contains(_searchingPattern.ToLower()))
                    {
                        ProductList[i].IsVisible = false;
                    }
                    else
                    {
                        ProductList[i].IsVisible = true;
                    }
                }
            }
        }

    }
}

