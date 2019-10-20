using System;
using Xamarin.Forms;
using Internals = Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FancyTodoList.Helpers
{
    [Internals.Preserve(AllMembers = true)]
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);

            return imageSource;
        }
    }
}
