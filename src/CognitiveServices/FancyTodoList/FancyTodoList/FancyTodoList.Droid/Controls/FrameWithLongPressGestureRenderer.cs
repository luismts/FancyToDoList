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
using FancyTodoList.Controls;
using FancyTodoList.Droid.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(FrameWithLongPressGesture), typeof(FrameWithLongPressGestureRenderer))]
namespace FancyTodoList.Droid.Controls
{
    public class FrameWithLongPressGestureRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FrameWithLongPressGesture, Android.Views.View>
    {
        FrameWithLongPressGesture view;
        protected override void OnElementChanged(ElementChangedEventArgs<FrameWithLongPressGesture> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                this.Touch -= FrameWithLongPressGestureRenderer_Touch;
            }

            if (e.NewElement != null)
            {
                view = e.NewElement as FrameWithLongPressGesture;
                this.Touch += FrameWithLongPressGestureRenderer_Touch;
            }
        }

        private void FrameWithLongPressGestureRenderer_Touch(object sender, TouchEventArgs e)
        {
            if (view == null)
                return;

            var handler = new LongPressEventArgs();

            if (e.Event.Action == MotionEventActions.Up)
            {
                handler.LongPress = false;
                view.HandleLongPress(view, handler);
            }
            else if (e.Event.Action == MotionEventActions.Move || e.Event.Action == MotionEventActions.Cancel)
            {
                handler.LongPress = true;
                view.HandleLongPress(view, handler);
            }

            //var x = e.Handled;
            //var y = e.Event;
        }
    }
}