using System;
using Xamarin.Forms;

namespace FancyTodoList.Controls
{
    public class FloatingActionButton : Button
    {
        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        public static BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(FloatingActionButton), Color.Accent);

        public Action<double> ShowHideButton { get; set; }

        public FloatingActionButton()
        {
            ShowHideButton += AnimateShowHideButtom;
        }

        private bool _isBusy;
        private async void AnimateShowHideButtom(double showButtom)
        {
            if (_isBusy)
                return;

            _isBusy = true;

            if (showButtom == -60)
                showButtom = showButtom - 5;

            await this.TranslateTo(0, -showButtom, 250, Easing.CubicIn);
            
            _isBusy = false;
        }
    }
}
