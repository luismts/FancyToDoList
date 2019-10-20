using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyTodoList.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
    {
        protected INavigationService NavigationService { get; }
        protected BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _canExecute;
        public bool CanExecute
        {
            get => _canExecute;
            set => SetProperty(ref _canExecute, value);
        }

        private bool _canNavigate = true;
        public bool CanNavigate
        {
            get => _canNavigate;
            set => SetProperty(ref _canNavigate, value);
        }

        public DelegateCommand<string> NavigateCommand
        {
            get { return new DelegateCommand<string>(x => Navigate(x)); }
        }

        public DelegateCommand NavigateBackCommand
        {
            get { return new DelegateCommand(() => NavigateBack()); }
        }

        public async void Navigate(string name, NavigationParameters navigationParameters = null)
        {
            if (!CanNavigate)
                return;

            CanNavigate = false;

            if (navigationParameters == null)
                await NavigationService.NavigateAsync(name, useModalNavigation: false);
            else
                await NavigationService.NavigateAsync(name, navigationParameters, useModalNavigation: false);

            CanNavigate = true;
        }

        public async void NavigateBack(NavigationParameters parameters = null)
        {
            if (!CanNavigate)
                return;

            CanNavigate = false;

            await NavigationService.GoBackAsync(parameters);

            CanNavigate = true;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
