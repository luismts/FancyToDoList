using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FancyTodoList.Data;
using FancyTodoList.Interfaces;
using FancyTodoList.Models;
using Xamarin.Forms;

namespace FancyTodoList.ViewModels
{
    public class NotificationPageViewModel : BaseViewModel
    {
        private readonly ICacheData _cache;
        private ObservableCollection<Item> _items;

        public NotificationPageViewModel(ICacheData cache, INavigationService navigationService) : base(navigationService)
        {
            _cache = cache;
        }

        private ObservableCollection<Item> _notifications = new ObservableCollection<Item>();
        public ObservableCollection<Item> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value); }
        }

        public async void SetNotificatonShowed()
        {
            if (_items == null || _items.Count == 0)
                return;

            foreach (var item in _items)
            {
                if (item.Date.Date <= DateTime.Today)
                    item.Showed = true;
            }
                
            await _cache.InsertLocalObject(CacheKeyDictionary.ItemList, _items);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("itemColletion"))
            {
                _items = (ObservableCollection<Item>)parameters["itemColletion"];

                foreach (var item in _items)
                {
                    if(item.Date.Date <= DateTime.Today)
                        Notifications.Add(item);
                }
            }
        }

    }
}
