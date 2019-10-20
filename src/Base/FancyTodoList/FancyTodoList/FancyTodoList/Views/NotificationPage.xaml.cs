using FancyTodoList.ViewModels;
using Xamarin.Forms;

namespace FancyTodoList.Views
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            ((NotificationPageViewModel)BindingContext)?.SetNotificatonShowed();

            base.OnDisappearing();
        }
    }
}
