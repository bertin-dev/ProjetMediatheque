using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProjetMediatheque.ViewModels
{
    class VideoViewModel : BaseViewModel
    {

        public VideoViewModel()
        {
            Title = "Video";
           // OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }

    }
}
