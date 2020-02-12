using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace ProjetMediatheque.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPicture : ContentPage
    {
        private MediaFile _mediaFile;

        public UploadPicture()
        {
            InitializeComponent();
            Title = "Upload d'images";
        }

        public string URL { get; set; }

        //Picture choose from device
        private async void Button_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Votre périphérique n'est pas supporté", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                UploadedUrl.Text = "Image URL:";
            }
        }

        //Upload picture button
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "Une erreur est survenue", "OK");
                return;
            }
            else
            {
                UploadImage(_mediaFile.GetStream());
            }

        }

        //Take picture from camera
        private async void Button_Clicked_2(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Pas de Camera", ":(Aucune camera n'est disponible.)", "OK");
                return;
            }
            else
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });


                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                UploadedUrl.Text = "Image URL:";
            }
        }

        //Upload to blob function
        private async void UploadImage(Stream stream)
        {
            Busy();
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=blobacount;AccountKey=hMZuDKfdz1iGPVsV+W9V52YnsjD6F1tjdH89XIw0QM3J6FB+tdJ9IgQI+OAWHgCRKSYMK0EwGcpB0qCJI8kY+w==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.png");
            await blockBlob.UploadFromStreamAsync(stream);
            URL = blockBlob.Uri.OriginalString;
            UploadedUrl.Text = URL;
            NotBusy();
            await DisplayAlert("Uploaded", "Image uploaded to Blob Storage Successfully!", "OK");
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }
    }
}