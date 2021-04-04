using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class SettingsPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand SetCulterLanguageCommand { get; set; }
        public ICommand OpenChangePasswordCommand { get; set; }
        public ICommand OpenPrivacyPolicyCommand { get; set; }
        public ICommand OpenContactUsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public SettingsPageViewModel()
        {
            SetCulterLanguageCommand = new Command(SetCulterLanguage);
            OpenChangePasswordCommand = new Command(OpenChangePassword);
            OpenPrivacyPolicyCommand = new Command(OpenPrivacyPolicy);
            OpenContactUsCommand = new Command(OpenContactUs);
            LogoutCommand = new Command(Logout);
        }
        private void SetCulterLanguage()
        {

        }
        private void OpenChangePassword()
        {
            OpenPage(new Views.ChangePasswordPage());
        }
        private void OpenPrivacyPolicy()
        {
            Xamarin.Essentials.Launcher.TryOpenAsync("www.google.com");
        }
        private void OpenContactUs()
        {
            OpenPage(new Views.ContactUsPage());
        }
        private void Logout()
        {

        }
    }
}
