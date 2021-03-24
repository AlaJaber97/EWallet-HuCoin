using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public ICommand OpenRegisterPageCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public LoginPageViewModel()
        {
            OpenRegisterPageCommand = new Command(OpenRegisterPage);
            SignInCommand = new Command(SingIn);
        }
        private void OpenRegisterPage()
        {
            OpenPage(new Views.RegisterPage());
        }
        private void SingIn()
        {
            OpenPageAsMainPage(new Views.MainPage());
        }
    }
}
