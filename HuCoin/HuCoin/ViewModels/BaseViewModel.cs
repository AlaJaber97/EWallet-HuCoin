using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using localizer = HuCoin.Utils.LocalizationResourceManager;

namespace HuCoin.ViewModels
{
    public class BaseViewModel: Services.NotifyPropertyChanged
    {
        public ICommand BackPageCommand { get; set; }
        public ICommand BackModalCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public BaseViewModel()
        {
            BackPageCommand = new Command(CloseCurrentPage);
            BackModalCommand = new Command(CloseCurrentModal);
            ChangeLanguageCommand = new Command<string>(culture => ChangeLanguage(culture));
        }

        private void ChangeLanguage(string culture)=>
            localizer.Instance.SetCulture(new System.Globalization.CultureInfo(culture));

        public void OpenPageAsMainPage(Page page)
        {
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(path: nameof(localizer.FlowDirection), source: localizer.Instance));
            Application.Current.MainPage = new NavigationPage(page);
        }

        public void OpenPage(Page page)
        {
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(path: nameof(localizer.FlowDirection), source: localizer.Instance));
            Application.Current.MainPage.Navigation.PushAsync(page);
        }
        public void OpenModal(Page page)
        {
            page.SetBinding(VisualElement.FlowDirectionProperty, new Binding(path: nameof(localizer.FlowDirection), source: localizer.Instance));
            Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public virtual void CloseCurrentPage()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        public void CloseCurrentModal()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task DisplayAlert(string title, string message, string cancelButton)
        {
            if(Xamarin.Essentials.MainThread.IsMainThread)
                await Application.Current.MainPage.DisplayAlert(title, message, cancelButton);
            else 
                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, cancelButton));

        }
        public async Task<bool> DisplayAlert(string title, string message, string okButton, string cancelButton)
        {
            bool isAproved;
            if (Xamarin.Essentials.MainThread.IsMainThread)
                isAproved = await Application.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton);
            else
                isAproved = await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton));
            return isAproved;
        }
    }
}
