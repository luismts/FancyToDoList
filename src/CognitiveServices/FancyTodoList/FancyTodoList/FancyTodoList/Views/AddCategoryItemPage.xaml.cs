using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FancyTodoList.ViewModels;
using Xamarin.Forms;

namespace FancyTodoList.Views
{
    public partial class AddCategoryItemPage : PopupPage
    {
        private MasterPageViewModel _context;
        public AddCategoryItemPage(MasterPageViewModel context)
        {
            InitializeComponent();
            _context = context;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryCategoryName.Text))
            {
                await DisplayAlert("", "Complete el campo requerido!", "Ok");
                return;
            }
                
            await _context.AddCategory(entryCategoryName.Text);
            await PopupNavigation.PopAsync();
        }
    }
}
