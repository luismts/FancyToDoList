using Prism.Unity;
using Microsoft.Practices.Unity;
using FancyTodoList.Data;
using FancyTodoList.Interfaces;
using FancyTodoList.Views;
using Xamarin.Forms;

namespace FancyTodoList
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("MyMasterDetailPage/NavigationPage/MainPage");
            //NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MasterPage>();
            Container.RegisterTypeForNavigation<MyMasterDetailPage>();
            Container.RegisterTypeForNavigation<MasterPage>();
            Container.RegisterTypeForNavigation<MasterPage>();
            Container.RegisterTypeForNavigation<AddPage>();
            Container.RegisterTypeForNavigation<DetailItemPage>();
            Container.RegisterTypeForNavigation<NotificationPage>();

            //types
            Container.RegisterType<ICacheData, CacheData>();
            //Container.RegisterTypeForNavigation<AddCategoryItemPage>();
        }
    }
}
