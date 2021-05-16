using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin.Views.Walkthorugh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalkthorughPage : CarouselPage
    {
        public ICommand SkipCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand OpenStartUpPageCommand { get; set; }
        public WalkthorughPage()
        {
            InitializeComponent();
            SkipCommand = new Command(SkipOpration);
            SkipCommand = new Command(NextOpration);
            OpenStartUpPageCommand = new Command(OpenStartUpPage);
            foreach (var child in this.Children)
                child.BindingContext = this;
        }

        private void OpenStartUpPage()
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        private void SkipOpration()
        {
            this.CurrentPage = this.Children.Last();
        }
        private void NextOpration()
        {
            var CurrentIndex = GetIndex(this.CurrentPage);
            this.CurrentPage = GetPageByIndex(CurrentIndex + 1);
        }
    }
}