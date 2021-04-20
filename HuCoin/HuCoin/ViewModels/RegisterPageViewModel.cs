using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public ICommand SingUpCommand { get; set; }
        public BLL.Models.User User { get; set; }
        public RegisterPageViewModel()
        {
            User = new BLL.Models.User();
            SingUpCommand = new Command(SingUp);
        }
        private void SingUp()
        {
            OpenPage(new Views.VerificationPhoneCodePage());
        }
    }
}
