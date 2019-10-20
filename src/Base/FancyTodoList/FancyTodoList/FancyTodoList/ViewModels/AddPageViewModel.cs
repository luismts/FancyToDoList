using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using FancyTodoList.Models;
using Prism.Services;
using FancyTodoList.Data;
using FancyTodoList.Interfaces;

namespace FancyTodoList.ViewModels
{
    public class AddPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private readonly ICacheData _cache;

        public AddPageViewModel(ICacheData cache, IPageDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _cache = cache;

            Init();
        }
        
        public DelegateCommand AddItemCommand => new DelegateCommand(AddList);

        private Item _item = new Item() {Description = "Descripcion", Date = DateTime.Now};
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private string _title = "Agregar Tarea";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _indexItemCategory = -1;
        public int IndexItemCategory
        {
            get { return _indexItemCategory; }
            set { SetProperty(ref _indexItemCategory, value); }
        }

        private async void Init()
        {
            var categories = await _cache.GetLocalObject<ObservableCollection<Category>>(CacheKeyDictionary.CategoryItems);

            if (categories?.Count > 0)
            {
                Categories = new ObservableCollection<Category>(categories);
            }
            else
            {
                Categories = new ObservableCollection<Category>
                {
                    new Category() { DisplayName = "Privado" },
                    new Category() { DisplayName = "Familia" },
                    new Category() { DisplayName = "Trabajo" }
                };
                await _cache.InsertLocalObject(CacheKeyDictionary.CategoryItems, Categories);
            }
        }

        private async void AddList()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (string.IsNullOrEmpty(Item.Title) || IndexItemCategory == -1)
            {
                await _dialogService.DisplayAlertAsync("", "Ups! Se te a olvidado llenar algunos campos. :)", "Ok");
                IsBusy = false;
                return;
            }

            Item.Category = Categories[IndexItemCategory]?.DisplayName ?? "Privado";

            if (Item?.Id > 0)
            {
                var navParameters = new NavigationParameters { { "itemEdited", Item } };
                await _navigationService.GoBackAsync(navParameters);
            }
            else
            {
                var navParameters = new NavigationParameters { { "itemCreated", Item } };
                await _navigationService.GoBackAsync(navParameters);
            }
            
            IsBusy = false;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("selecttedItem"))
            {
                Title = "Editar Tarea";

                var item = (Item)parameters["selecttedItem"];
                Item = new Item() { Completed = item.Completed, Date = item.Date, Showed = item.Showed, Title = item.Title, Description = item.Description, Id = item.Id, Category = item.Category};

                var cat = Categories.Single(i => i.DisplayName == Item.Category);

                if(cat != null)
                    IndexItemCategory = Categories.IndexOf(cat);
            }
        }
    }
}
