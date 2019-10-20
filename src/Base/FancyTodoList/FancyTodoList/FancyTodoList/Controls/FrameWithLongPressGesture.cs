using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FancyTodoList.Models;
using Xamarin.Forms;

namespace FancyTodoList.Controls
{
    public class FrameWithLongPressGesture : StackLayout
    {
        public static readonly BindableProperty HandleLongPressCommandProperty =
            BindableProperty.Create("HandleLongPressCommand", typeof(ICommand), typeof(FrameWithLongPressGesture));

        public ICommand HandleLongPressCommand
        {
            get => (ICommand)GetValue(HandleLongPressCommandProperty);
            set => SetValue(HandleLongPressCommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(FrameWithLongPressGesture));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler<LongPressEventArgs> LongPressActivated;
        public void HandleLongPress(object sender, LongPressEventArgs e)
        {
            //Handle LongPressActivated Event
            HandleLongPressCommand?.Execute(CommandParameter);
            LongPressActivated?.Invoke(sender, e);
        }
    }

    public class LongPressEventArgs : EventArgs
    {
        public bool LongPress { get; set; }
    }
}
