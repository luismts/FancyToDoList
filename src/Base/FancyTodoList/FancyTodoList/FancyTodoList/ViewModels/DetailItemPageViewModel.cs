using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using FancyTodoList.Models;

namespace FancyTodoList.ViewModels
{
    public class DetailItemPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        public DetailItemPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public DelegateCommand CompletedItemCommand => new DelegateCommand(CompletedItem);

        private async void CompletedItem()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            Item.Completed = true;
            var navParameters = new NavigationParameters { { "itemCompleted", Item } };
            await _navigationService.GoBackAsync(navParameters);

            IsBusy = false;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("selecttedItem"))
            {
                Item = (Item)parameters["selecttedItem"];
            }
        }
    }
}
