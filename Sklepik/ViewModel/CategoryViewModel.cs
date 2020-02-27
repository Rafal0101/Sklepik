using Blazored.Modal;
using Blazored.Modal.Services;
using Domain;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Sklepik.Pages.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public class CategoryViewModel : BaseObservableObject
    {
        [Inject]
        protected IMatToaster Toaster { get; set; }

        private ActionState currentAction;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IModalService _modalService;

        public CategoryViewModel(ICategoryRepository categoryRepository, IModalService modalService)
        {
            _categoryRepository = categoryRepository;
            _modalService = modalService;
            CategoryList = new List<CategoryModel>(_categoryRepository.GetAll());
        }

        private string _searchingPattern;
        public string SearchingPattern
        {
            get { return _searchingPattern; }
            set 
            { 
                _searchingPattern = value;
                CategoryList = new List<CategoryModel>(_categoryRepository.GetAll(SearchingPattern));
                NotifyPropertyChanged(nameof(SearchingPattern));
             }
        }
   
        private List<CategoryModel> _categorysList = new List<CategoryModel>();
         public List<CategoryModel> CategoryList
        {
            get { return _categorysList; }
            set 
            { 
                _categorysList = value;
                NotifyPropertyChanged(nameof(CategoryList));
            }
        }

        public void AddCategory()
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<CategoryForm>("Dodawanie nowej kategorii", parameters, options);
            parameters.Add("category", new CategoryModel());

            currentAction = ActionState.Adding;
        }

        private void _modalService_OnClose(ModalResult result)
        {
            if (!result.Cancelled)
            {
                CategoryModel updated = (CategoryModel)result.Data;
                if (currentAction == ActionState.Adding)
                {
                    _categoryRepository.AddCategory(updated);
                }
                else if (currentAction == ActionState.Editing)
                {
                    _categoryRepository.EditCategory(updated);
                }
                else
                {
                    try
                    {
                        _categoryRepository.DeleteCategory(updated);
                    }
                    catch(Exception ex)
                    {
                        #warning Pobrac MessageBox z CPEm
                        //_modalService.Show<MessageBox>
                    }
                }

                CategoryList = new List<CategoryModel>(_categoryRepository.GetAll());
            }
            //Show(MatToastType.Success);
            _modalService.OnClose -= _modalService_OnClose;
        }

        public void EditCategory(CategoryModel category)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            parameters.Add("category", category);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<CategoryForm>("Edycja kategorii", parameters, options);
            currentAction = ActionState.Editing;
        }
        public void DeleteCategory(CategoryModel category)
        {
            _modalService.OnClose += _modalService_OnClose;
            var parameters = new ModalParameters();
            parameters.Add("category", category);
            var options = new ModalOptions() { DisableBackgroundCancel = true };
            _modalService.Show<CategoryForm>("Czy na pewno chcesz usunąć kategorię?", parameters, options);
            currentAction = ActionState.Deleting;
        }

        private void Show(MatToastType type, string icon = "")
        {
            Toaster.Add("ok", type, "Ok", icon, config =>
            {
                config.ShowCloseButton = true;
                config.ShowProgressBar = true;
                config.MaximumOpacity = 100;

                config.ShowTransitionDuration = 500;
                config.VisibleStateDuration = 5000;
                config.HideTransitionDuration = 500;

                config.RequireInteraction = false;

            });
        }
    }
}
