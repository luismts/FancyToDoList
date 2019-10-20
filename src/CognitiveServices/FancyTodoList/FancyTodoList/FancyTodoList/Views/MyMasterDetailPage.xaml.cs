using Xamarin.Forms;

namespace FancyTodoList.Views
{
    public partial class MyMasterDetailPage : MasterDetailPage
    {
        public static MasterDetailPage Current;
        public MyMasterDetailPage()
        {
            InitializeComponent();
            Current = this;
        }
    }
}