using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Prism.Navigation;
using FancyTodoList.Helpers;
using FancyTodoList.ViewModels;
using Xamarin.Forms;
using FancyTodoList.Models;

namespace FancyTodoList.Views
{
    public partial class MainPage : ContentPage, INavigationAware
    {
        public static MainPage CurrentMainPage;
        
        public MainPage()
        {
            InitializeComponent();
            CurrentMainPage = this;
        }
        
        void RefreshList()
        {
            var src = listItems.ItemsSource;
            listItems.ItemsSource = null;
            listItems.ItemsSource = src;
        }

        //private void Cell_OnTapped(object sender, EventArgs e)
        //{
        //    (BindingContext as MainPageViewModel)?.SelectedItemCommand.Execute((Models.Item)selectedItem);
        //}

        //private void ListItems_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if(e.SelectedItem != null)
        //        selectedItem = e.SelectedItem as Item;

        //    ((ListView)sender).SelectedItem = null;
        //}

        private void CheckBox_OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            //(BindingContext as MainPageViewModel)?.CheckColletion();
            RefreshList();
        }

        private async void MainCheckBox_OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            ((Controls.CheckBox)sender).Checked = false;
            await Task.Delay(200);

            RefreshList();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("itemCompleted"))
            {
                RefreshList();
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        private void CompletedTaskButton_Clicked(object sender, EventArgs e)
        {
            completedTaskView.IsVisible = !completedTaskView.IsVisible;
        }
    }
}
