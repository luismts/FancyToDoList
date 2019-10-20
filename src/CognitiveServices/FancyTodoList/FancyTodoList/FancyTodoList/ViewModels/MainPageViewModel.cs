using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Notifications;
using Prism.Services;
using FancyTodoList.Data;
using FancyTodoList.Models;
using Xamarin.Forms;
using FancyTodoList.Interfaces;

namespace FancyTodoList.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ICacheData _cache;

        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private Category _currentCategory;
        public MainPageViewModel(ICacheData cache, IPageDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            _cache = cache;
            _navigationService = navigationService;
            _dialogService = dialogService;
            LoadItems();
        }

        #region Properties
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<Item> _itemsByCategory = new ObservableCollection<Item>();
        public ObservableCollection<Item> ItemsByCategory
        {
            get { return _itemsByCategory; }
            set { SetProperty(ref _itemsByCategory, value); }
        }

        private string _title = "Todas las tareas";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _completeAllTask;
        public bool CompleteAllTask
        {
            get { return _completeAllTask; }
            set
            {
                if (value != _completeAllTask && value)
                {
                    foreach (var item in Items)
                    {
                        if (!item.Completed)
                            item.Completed = true;
                    }
                }

                SetProperty(ref _completeAllTask, value);
            }
        }

        private bool _showCompletedTask;
        public bool ShowCompletedTask
        {
            get { return _showCompletedTask; }
            set
            {
                SetProperty(ref _showCompletedTask, value);
            }
        }

        private string _iconNotification = "ic_notification.png";
        public string IconNotification
        {
            get { return _iconNotification; }
            set { SetProperty(ref _iconNotification, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand<Item> EditItemCommand
        {
            get { return new DelegateCommand<Item>(SelectedItem); }
        }

        public DelegateCommand RemoveCategoryCommand
        {
            get { return new DelegateCommand(()=> DeleteCategory(null)); }
        }

        public DelegateCommand<Item> DeleteItemCommand
        {
            get { return new DelegateCommand<Item>(DeleteItem); }
        }

        public DelegateCommand<Item> SelectedItemCommand
        {
            get { return new DelegateCommand<Item>(SelectedItem); }
        }

        public DelegateCommand ShowCompleteTaskCommand
        {
            get { return new DelegateCommand(()=>ShowCompletedTask = !ShowCompletedTask); }
        }

        public DelegateCommand ShowNotificationsCommand
        {
            get { return new DelegateCommand(ShowNotifications); }
        }
        
        public DelegateCommand<Item> MenuActionCommand
        {
            get { return new DelegateCommand<Item>(MenuAction); }
        }
        #endregion

        public void ShowItemsByCategory(Category categoryItem, bool showAll = false)
        {
            ItemsByCategory = new ObservableCollection<Item>();
            _currentCategory = categoryItem;

            if (string.IsNullOrEmpty(categoryItem?.DisplayName) || showAll)
            {
                Title = "Todas las tareas";
                ItemsByCategory = new ObservableCollection<Item>(Items);
                return;
            }

            foreach (var item in Items)
            {
                if(item.Category == categoryItem.DisplayName)
                    ItemsByCategory.Add(item);
            }

            Title = categoryItem.DisplayName;
        }

        private async void ShowNotifications()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var navParameters = new NavigationParameters { { "itemColletion", Items } };
            await _navigationService.NavigateAsync("NotificationPage", navParameters);

            await CrossNotifications.Current.CancelAll();
            await CrossNotifications.Current.SetBadge(0);

            IconNotification = "ic_notification.png";
            await _cache.InsertLocalObject(CacheKeyDictionary.LastCheck, DateTime.Now);

            IsBusy = false;
        }

        async void MenuAction(Item item)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var actions = new[] { "Editar", "Eliminar"};
            var action = await _dialogService.DisplayActionSheetAsync("", "", null, actions);

            switch (action)
            {
                case "Editar":
                    EditItem(item);
                    break;
                case "Eliminar":
                    DeleteItem(item);
                    break;
            }

            IsBusy = false;
        }
        private async void LoadItems()
        {
            int countNewNotification = 0;
            var items = await _cache.GetLocalObject<ObservableCollection<Item>>(CacheKeyDictionary.ItemList);

            if(items == null || items.Count == 0)
                return;

            Items = items;
            ItemsByCategory = new ObservableCollection<Item>(Items);
            DateTime? lastCheck = await _cache.GetLocalObject<DateTime>(CacheKeyDictionary.LastCheck);
           
            if(lastCheck == null || lastCheck.Value.Date == DateTime.MinValue.Date)
                lastCheck = DateTime.Parse("1/1/2017");

            foreach (var item in items)
            {
                if (item.Date == lastCheck?.Date)
                    countNewNotification++;
            }
            
            ShowNotification(countNewNotification);
        }

        async void ShowNotification(int countNewNotification, bool newItem = false)
        {
            if (countNewNotification > 0)
            {
                var not = new Plugin.Notifications.Notification
                {
                    Title = "TODO List",
                    Message = "Tienes nuevas notificacines!"
                };

                await CrossNotifications.Current.CancelAll();
                await CrossNotifications.Current.Send(not);

                IconNotification = "ic_notification_has.png";
                await CrossNotifications.Current.SetBadge(countNewNotification);
            }

            if (newItem)
            {
                var not = new Plugin.Notifications.Notification
                {
                    Title = "TODO List",
                    Message = "Tienes nuevas notificacines!"
                };

                await CrossNotifications.Current.CancelAll();
                await CrossNotifications.Current.Send(not);

                IconNotification = "ic_notification_has.png";
            }
        }

        //private void CompletedTaskShow()
        //{
        //    if (ShowCompletedTask)
        //    {
        //        //var items = new ObservableCollection<Item>();
        //        foreach (var item in Items)
        //        {
        //            if (item.Completed)
        //                CompletedItems.Add(item);
        //        }

        //        //if(items.Count > 0)
        //        //    CompletedItems = new ObservableCollection<Item>(items);
        //    }
        //    else
        //    {
        //        CompletedItems.Clear();
        //    }

        //    ShowCompletedTask = !ShowCompletedTask;
        //}

        void EditItem(Item item)
        {
            var navParameters = new Prism.Navigation.NavigationParameters { { "selecttedItem", item } };
            Navigate("AddPage", navParameters);
        }

        void SelectedItem(Item item)
        {
            var navParameters = new Prism.Navigation.NavigationParameters { { "selecttedItem", item } };
            Navigate("DetailItemPage", navParameters);
        }

        private async void DeleteItem(Item item)
        {
            var resp = await _dialogService.DisplayAlertAsync("", "Desea eliminar este item?", "Ok", "Cancelar");

            if (!resp || item == null)
                return;

            ItemsByCategory.Remove(item);
            Items.Remove(item);
            await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, Items);
        }

        public async void DeleteCategory(Category cat)
        {
            if (cat == null)
                return;

            var resp = await _dialogService.DisplayAlertAsync("", "Desea eliminar esta categoria?", "Ok", "Cancelar");

            if (!resp)
                return;
            
            var categories = (Views.MasterPage.Current.BindingContext as MasterPageViewModel)?.MenuItems;

            if(!categories.Contains(cat))
                return;

            categories.Remove(cat);
            (Views.MasterPage.Current.BindingContext as MasterPageViewModel).MenuItems = categories;

            var items = Items.ToList();
            foreach (var item in items)
            {
                if (item.Category == cat.DisplayName)
                {
                    if (ItemsByCategory.Contains(item))
                        ItemsByCategory.Remove(item);

                    Items.Remove(item);
                }
            }

            ShowItemsByCategory(null, true);
            await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, Items);
            await _cache.InsertLocalObject(CacheKeyDictionary.CategoryItems, categories);
        }


        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("itemCreated"))
            {
                var itemSelected = (Item)parameters["itemCreated"];
                itemSelected.Id = (Items?.Count > 0)? Items.Last().Id + 1 : 1;
                Items.Add(itemSelected);

                if (_currentCategory == null)
                    ItemsByCategory.Add(itemSelected);
                else if (itemSelected.Category == _currentCategory?.DisplayName)
                    ItemsByCategory.Add(itemSelected);

                if(itemSelected.Date.Date == DateTime.Today.Date)
                    ShowNotification(0, true);

                await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, Items);
            }

            if (parameters.ContainsKey("itemEdited"))
            {
                var newItem = (Item)parameters["itemEdited"];

                //by cat
                var iEdited = ItemsByCategory.Single(i => i.Id == newItem.Id);
                var idxNewItem = ItemsByCategory.IndexOf(iEdited);

                ItemsByCategory.Remove(iEdited);
                ItemsByCategory = _itemsByCategory;

                if (_currentCategory == null)
                    ItemsByCategory.Insert(idxNewItem, newItem);
                else if (newItem.Category == _currentCategory.DisplayName)
                    ItemsByCategory.Insert(idxNewItem, newItem);

                //origin
                var itemEdited = Items.Single(i => i.Id == newItem.Id);
                var indexNewItem = Items.IndexOf(itemEdited);

                Items.Remove(itemEdited);
                Items.Insert(indexNewItem, newItem);

                await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, Items);
            }

            if (parameters.ContainsKey("itemCompleted"))
            {
                var newItem = (Item)parameters["itemCompleted"];
                var itemEdited = Items.Single(i => i.Id == newItem.Id);
                itemEdited.Completed = true;

                await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, Items);
            }
        }
    }
}
