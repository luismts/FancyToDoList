using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyTodoList.Controls
{
    public class CustomViewCell : ViewCell
    {
        public static BindableProperty IsVisibleProperty = BindableProperty.Create("IsVisible", typeof(bool), typeof(CustomViewCell), true);

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
    }
}
