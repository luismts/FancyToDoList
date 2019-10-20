using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Text;
using System.ComponentModel;
using FancyTodoList.Controls;
using FancyTodoList.Droid.Controls;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace FancyTodoList.Droid.Controls
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control?.SetText(FromHtml(Element.Text ?? string.Empty), TextView.BufferType.Spannable);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Control?.SetText(FromHtml(Element.Text ?? string.Empty), TextView.BufferType.Spannable);
            }
        }

#pragma warning disable CS0618, CS0612 // Type or member is obsolete
        protected ISpanned FromHtml(String strHtml)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                return Html.FromHtml(strHtml, Html.FromHtmlModeLegacy);

            return Html.FromHtml(strHtml);
        }
#pragma warning restore CS0618, CS0612 // Type or member is obsolete

    }
}