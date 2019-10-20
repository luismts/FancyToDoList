using System;
using FancyTodoList.ViewModels;
using Xamarin.Forms;

namespace FancyTodoList.Views
{
    public partial class MasterPage : ContentPage
    {
        public static ContentPage Current;
        public MasterPage()
        {
            InitializeComponent();
            Current = this;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            MyMasterDetailPage.Current.IsPresented = false;
        }


        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            (MainPage.CurrentMainPage.BindingContext as MainPageViewModel)?.ShowItemsByCategory(null, true);
            MyMasterDetailPage.Current.IsPresented = false;
        }
    }
}
