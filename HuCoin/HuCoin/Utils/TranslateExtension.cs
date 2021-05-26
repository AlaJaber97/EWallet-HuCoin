using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin.Utils
{
    [ContentProperty(nameof(Key))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        public string Key { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                return ProvideValue(serviceProvider);
            }
            catch (Exception)
            {
                return Key;
            }
        }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Path = $"[{Key}]",
                Source = LocalizationResourceManager.Instance,
            };
            return binding;
        }
    }
}
