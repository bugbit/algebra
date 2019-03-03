using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace Algebra.ViewsXaml
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            //Debug.WriteLine("Provide: " + Text);
            // Do your translation lookup here, using whatever method you require
            var translated = Core.Algebra_Resources.ResourceManager.GetString(Text, Core.Algebra_Resources.Culture);

            return translated;
        }
    }
}
