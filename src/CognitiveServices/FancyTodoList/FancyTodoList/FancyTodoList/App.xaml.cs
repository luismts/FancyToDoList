using Prism.Unity;
using Microsoft.Practices.Unity;
using FancyTodoList.Data;
using FancyTodoList.Interfaces;
using FancyTodoList.Views;
using Xamarin.Forms;
using FancyTodoList.Services;

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
            //Container.RegisterTypeForNavigation<AddCategoryItemPage>();

            //types
            Container.RegisterType<ICacheData, CacheData>();
            //Container.RegisterType<IAuthenticationService, AuthenticationService>();
            //Container.RegisterType<IBingSpeechService, BingSpeechService>(new InjectionConstructor(new AuthenticationService(Constants.BingSpeechApiKey), Device.RuntimePlatform));
            //Container.RegisterType<IBingSpellCheckService, BingSpellCheckService>();
            //Container.RegisterType<ITextTranslationService, TextTranslationService>(new InjectionConstructor(Constants.TextTranslatorApiKey));
            Container.RegisterTypeForNavigation<RateAppPage>();
        }
    }
}
