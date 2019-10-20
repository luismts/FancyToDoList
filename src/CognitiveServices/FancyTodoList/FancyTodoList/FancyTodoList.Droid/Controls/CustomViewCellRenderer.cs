using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using FancyTodoList.Controls;
using FancyTodoList.Droid.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace FancyTodoList.Droid.Controls
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var view = base.GetCellCore(item, convertView, parent, context);
            var cell = (CustomViewCell)item;
            view.Visibility = cell.IsVisible ? ViewStates.Visible : ViewStates.Gone;
            return view;
        }
    }
}