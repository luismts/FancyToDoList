using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using FancyTodoList.Data;
using FancyTodoList.Interfaces;
using FancyTodoList.Models;

namespace FancyTodoList.ViewModels
{
    public class MasterPageViewModel : BaseViewModel
    {
        private readonly ICacheData _cache;
        private readonly IPageDialogService _dialogService;

        public MasterPageViewModel(ICacheData cache, INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _cache = cache;
            _dialogService = dialogService;
            LoadMenu();
        }

        private ObservableCollection<Category> menuItems = new ObservableCollection<Category>();
        public ObservableCollection<Category> MenuItems
        {
            get { return menuItems; }
            set { SetProperty(ref menuItems, value); }
        }

        public DelegateCommand ShowAddCategoryPageCommand
        {
            get { return new DelegateCommand(ShowAddCategoryPage); }
        }

        public DelegateCommand<Category> ShowItemsByCategoryCommand
        {
            get { return new DelegateCommand<Category>(ShowItemsByCategory); }
        }

        private void ShowItemsByCategory(Category obj)
        {
            if(IsBusy)
                return;

            IsBusy = true;

            ((MainPageViewModel)Views.MainPage.CurrentMainPage.BindingContext)?.ShowItemsByCategory(obj);

            IsBusy = false;
        }

        

        private async void ShowAddCategoryPage()
        {
            var page = new Views.AddCategoryItemPage(this);
            await PopupNavigation.PushAsync(page);
        }

        public async Task AddCategory(string category)
        {
            if (IsBusy || string.IsNullOrEmpty(category))
                return;

            IsBusy = true;

            var itemCategory = new Category() {DisplayName = category};
            if (!MenuItems.Contains(itemCategory))
            {
                MenuItems.Add(itemCategory);
                await _cache.InsertLocalObject(CacheKeyDictionary.CategoryItems, MenuItems);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("", "Esta categoria ya esta registrada! :(", "Ok");
            }

            IsBusy = false;
        }

        private async void LoadMenu()
        {
            var categories = await _cache.GetLocalObject<ObservableCollection<Category>>(CacheKeyDictionary.CategoryItems);

            if (categories?.Count > 0)
            {
                MenuItems = new ObservableCollection<Category>(categories);
            }
            else
            {
                MenuItems = new ObservableCollection<Category>
                {
                    new Category(){DisplayName = "Privado"},
                    new Category(){DisplayName = "Familia"},
                    new Category(){DisplayName = "Trabajo"}
                };
                await _cache.InsertLocalObject(CacheKeyDictionary.CategoryItems, MenuItems);
            }
        }
    }

    

   
}
