﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public class ProductViewModel : BaseObservableObject
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IModalService _modalService;

        public ProductViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository, IModalService modalService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _modalService = modalService;
        
            ProductList = new List<ProductModel>(_productRepository.GetAll());
            CategoryList = _categoryRepository.GetAll();
        }
        private ActionState currentAction;

        private List<ProductModel> _productsList = new List<ProductModel>();
        public List<ProductModel> ProductList
        {
            get { return _productsList; }
            set 
            { 
                _productsList = value;
                NotifyPropertyChanged(nameof(ProductList));
            }
        }


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
                        && !ProductList[i].Name.ToLower().Contains(_searchingPattern.ToLower()))
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


        public List<CategoryModel> CategoryList { get; private set; } = new List<CategoryModel>();

        private MemoryStream _priceListFileBody;

        public MemoryStream PriceListFileBody
        {
            get { return _priceListFileBody; }
            set
            {
                _priceListFileBody = value;
                NotifyPropertyChanged(nameof(PriceListFileBody));
                _productRepository.ImportProductFromFile(_priceListFileBody);
                ProductList = new List<ProductModel>(_productRepository.GetAll());
            }
        }


        public void AddProduct()
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            parameters.Add("product", new ProductModel { Tax = 23, CategoryId = CategoryList[0].Id});
            parameters.Add("categories", CategoryList);
            _modalService.Show<ProductForm>("Dodawanie nowego produktu", parameters, options);

            currentAction = ActionState.Adding;
        }

        private void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                ProductModel updated = (ProductModel)result.Data;
                if (currentAction == ActionState.Adding)
                {
                    _productRepository.AddProduct(updated);
                }
                else if (currentAction == ActionState.Editing)
                {
                    _productRepository.EditProduct(updated);
                }
                else
                {
                    _productRepository.DeleteProduct(updated);
                }

                ProductList = new List<ProductModel>(_productRepository.GetAll());
            }

            _modalService.OnClose -= _modalService_OnClose;
        }

        public void EditProduct(ProductModel product)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            parameters.Add("product", product);
            parameters.Add("categories", CategoryList);
            _modalService.Show<ProductForm>("Edycja produktu", parameters, options);

            currentAction = ActionState.Editing;
        }
        public void DeleteProduct(ProductModel product)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            parameters.Add("product", product);
            parameters.Add("categories", CategoryList);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<ProductForm>("Czy na pewno chcesz usunąć produkt?", parameters, options);
 
            currentAction = ActionState.Deleting;
        }

    }
}
