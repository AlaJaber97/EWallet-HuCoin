using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public ICommand BackPageCommand { get; set; }
        public RegisterPageViewModel()
        {
            BackPageCommand = new Command(BackPage);
            
        }
        private void BackPage() => CloseCurrentPage();
    }
}
