using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Xamarin.Forms;
using FancyTodoList.Droid.Controls;
using Xamarin.Forms.Platform.Android;
using FancyTodoList.Controls;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]
namespace FancyTodoList.Droid.Controls
{
    /// <summary>
    /// Class CheckBoxRenderer.
    /// </summary>
    public class CheckBoxRenderer : ViewRenderer<CheckBox, Android.Widget.CheckBox>
    {
        private ColorStateList defaultTextColor;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var checkBox = new Android.Widget.CheckBox(this.Context);
                checkBox.CheckedChange += CheckBoxCheckedChange;

                defaultTextColor = checkBox.TextColors;
                this.SetNativeControl(checkBox);
            }

            //En nuestro caso usaremos los estilos de android para dar el color a todos los controles
            #region Colores para los estados del checkBox
            /*
            //Array de estados del checkBox
            int[][] states = {
                new int[] { Android.Resource.Attribute.StateEnabled}, // enabled
                new int[] { Android.Resource.Attribute.StateEnabled}, // disabled
                new int[] { Android.Resource.Attribute.StateChecked}, // unchecked
                new int[] { Android.Resource.Attribute.StatePressed}  // pressed
            };

            //Extraccion del color
            var checkBoxColor = (int)e.NewElement.Color.ToAndroid();

            //Array de colores para los estados
            int[] colors = { checkBoxColor, checkBoxColor, checkBoxColor, checkBoxColor };

            //Lista que contiene los estados con sus resprectivos colores
            var myList = new Android.Content.Res.ColorStateList(states, colors);

            if (int.Parse(Android.OS.Build.VERSION.Sdk) >= 21)
                Control.ButtonTintList = myList;
                */
            #endregion

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;
            UpdateTextColor();

            if (e.NewElement.FontSize > 0)
            {
                Control.TextSize = (float)e.NewElement.FontSize;
            }

            if (!string.IsNullOrEmpty(e.NewElement.FontName))
            {
                Control.Typeface = TrySetFont(e.NewElement.FontName);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            //CheckboxPropertyChanged((CheckBox)sender, e.PropertyName); //-- incrustacion

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Text = Element.Text;
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    UpdateTextColor();
                    break;
                case "FontName":
                    if (!string.IsNullOrEmpty(Element.FontName))
                    {
                        Control.Typeface = TrySetFont(Element.FontName);
                    }
                    break;
                case "FontSize":
                    if (Element.FontSize > 0)
                    {
                        Control.TextSize = (float)Element.FontSize;
                    }
                    break;
                case "CheckedText":
                case "UncheckedText":
                    Control.Text = Element.Text;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    break;
            }
        }

        private void CheckboxPropertyChanged(CheckBox model, String propertyName)
        {

            if (propertyName == null || CheckBox.ColorProperty.PropertyName == propertyName)
            {
                int[][] states = {
                new int[] { Android.Resource.Attribute.StateEnabled}, // enabled
                new int[] {Android.Resource.Attribute.StateEnabled}, // disabled
                new int[] {Android.Resource.Attribute.StateChecked}, // unchecked
                new int[] { Android.Resource.Attribute.StatePressed}  // pressed
                };

                var checkBoxColor = (int)model.Color.ToAndroid();

                int[] colors = {
                checkBoxColor,
                checkBoxColor,
                checkBoxColor,
                checkBoxColor
                };

                var myList = new Android.Content.Res.ColorStateList(states, colors);
                Control.ButtonTintList = myList;
            }
        }

        /// <summary>
        /// CheckBoxes the checked change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Widget.CompoundButton.CheckedChangeEventArgs"/> instance containing the event data.</param>
        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        /// <summary>
        /// Tries the set font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <returns>Typeface.</returns>
        private Typeface TrySetFont(string fontName)
        {
            Typeface tf = Typeface.Default;
            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, fontName);
                return tf;
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);
                    return tf;
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);
                    return Typeface.Default;
                }
            }
        }

        /// <summary>
        /// Updates the color of the text
        /// </summary>
        private void UpdateTextColor()
        {
            if (Control == null || Element == null)
                return;

            if (Element.TextColor == Xamarin.Forms.Color.Default)
                Control.SetTextColor(defaultTextColor);
            else
                Control.SetTextColor(Element.TextColor.ToAndroid());
        }
    }
}