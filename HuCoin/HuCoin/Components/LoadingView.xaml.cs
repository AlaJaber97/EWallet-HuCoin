using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingView : ContentPage, IDisposable
    {
        public LoadingView()
        {
            InitializeComponent();
            Application.Current.MainPage.Navigation.PushModalAsync(this);
        }

        public void Dispose()
        {
            if(Application.Current.MainPage.Navigation.ModalStack.Any())
                Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}