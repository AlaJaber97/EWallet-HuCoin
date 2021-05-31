using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace HuCoin.Utils
{
    public class LocalizationResourceManager : Services.NotifyPropertyChanged
    {
        private static LocalizationResourceManager _Instance;
        public static LocalizationResourceManager Instance => _Instance ??= new LocalizationResourceManager();
        private CultureInfo CultureInfo { get; set; }
        public FlowDirection FlowDirection { get; set; }
        private LocalizationResourceManager() { }
        public void SetCulture(CultureInfo culture)
        {
            try
            {
                CultureInfo = culture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo;
                FlowDirection = CultureInfo.TextInfo.IsRightToLeft ? Xamarin.Forms.FlowDirection.RightToLeft : Xamarin.Forms.FlowDirection.LeftToRight;
                OnPropertyChanged();
                if (!Application.Current.Properties.ContainsKey(AppStatic.LanguageKey))
                    Application.Current.Properties.Add(AppStatic.LanguageKey, culture.Name);
                else
                    Application.Current.Properties[AppStatic.LanguageKey] = culture.Name;
                Application.Current.SavePropertiesAsync();
            }
            catch(Exception)
            {

            }
        }
        public string this[string key]=>
            Resource.Lang.Resource.ResourceManager.GetString(key, CultureInfo);
    }
}
