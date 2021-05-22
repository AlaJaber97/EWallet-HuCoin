using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HuCoin.Services
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propretyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propretyname));
        }
    }
}
