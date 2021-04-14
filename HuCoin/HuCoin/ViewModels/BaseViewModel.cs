using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BaseViewModel
    {
        public ICommand BackPageCommand { get; set; }
        public BaseViewModel()
        {
            BackPageCommand = new Command(CloseCurrentPage);
        }

        public void OpenPageAsMainPage(Page page)
        {
            App.Current.MainPage = new NavigationPage(page);
        }

        public void OpenPage(Page page)
        {
            App.Current.MainPage.Navigation.PushAsync(page);
        }
        public void OpenModal(Page page)
        {
            App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public void CloseCurrentPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        public void CloseCurrentModal()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task DisplayAlert(string title, string message, string cancelButton)
        {
            if(Xamarin.Essentials.MainThread.IsMainThread)
                await App.Current.MainPage.DisplayAlert(title, message, cancelButton);
            else 
                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.DisplayAlert(title, message, cancelButton));

        }
        public async Task<bool> DisplayAlert(string title, string message, string okButton, string cancelButton)
        {
            bool isAproved;
            if (Xamarin.Essentials.MainThread.IsMainThread)
                isAproved = await App.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton);
            else
                isAproved = await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton));
            return isAproved;
        }
    }
}
