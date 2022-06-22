using AppSpotify.Models;
using AppSpotify.Service;
using AppSpotify.Services;
using AppSpotify.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace AppSpotify.ViewModels
{
    public class ListSongsViewModel:BaseViewModel
    {
        public readonly ListSongsViewModel ListViewModel;
        private Command _MapsCommand;
        public Command MapsCommand => _MapsCommand ?? (_MapsCommand = new Command(MapsAction));

        private Command _CancelCommand;
        public Command CancelCommand => _CancelCommand ?? (_CancelCommand = new Command(CancelAction));

        private Command _SaveCommand;
        public Command SaveCommand => _SaveCommand ?? (_SaveCommand = new Command(SaveAction));

        private Command _DeleteCommand;
        public Command DeleteCommand => _DeleteCommand ?? (_DeleteCommand = new Command(DeleteAction));

        private Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        private Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));







        // Propiedades
        private SongModel _SongSelected;
        public SongModel SongSelected
        {
            get => _SongSelected;
            set => SetProperty(ref _SongSelected, value);
        }

        private string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }
        private int _ID;
        public int ID
        {
            get => _ID;
            set => SetProperty(ref _ID, value);
        }
        private string _SongName;
        public string SongName
        {
            get => _SongName;
            set => SetProperty(ref _SongName, value);
        }
        private string _Singer;
        public string Singer
        {
            get => _Singer;
            set => SetProperty(ref _Singer, value);
        }
        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        // Constructor
        public ListSongsViewModel()
        {
            SongSelected = new SongModel();
        }

        public ListSongsViewModel(SongModel songSelected)
        {
            SongSelected = songSelected;
        }

        // Metodos

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void SaveAction()
        {
            ApiResponse response;
            try
            {
                // Iniciamos el spinner
                IsBusy = true;

                // Creamos el modelo con los datos de los controles
                SongModel model = new SongModel
                {
                    ID = ID,
                    SongName = SongName,
                    Singer = Singer,
                    Picture = Picture,
                    Latitude = Latitude,
                    Longitude = Longitude
                };

                if (model.ID == 0)
                {
                    // Crear un nuevo producto
                    response = await new ApiService().PostDataAsync("Song", model);
                }
                else
                {
                    // Actualizar un producto existente
                    response = await new ApiService().PutDataAsync("Song", model);
                }

                // Si no fue satisfactorio enviamos un mensaje y terminamos el método
                if (response == null || !response.IsSucces)
                {
                    IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert("AppSpotify", response.Message, "Ok");
                    return;
                }

             

                // Cerramos la vista actual
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppProducts", $"Se generó un error al guardar el producto: {ex.Message}", "Ok");
            }
        }

        private async void DeleteAction()
        {
         
        }
        

        private void MapsAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new MapsPage(SongSelected.SongName, SongSelected.Latitude, SongSelected.Longitude));

        }


        private async void TakePictureAction()
        {
            // Inicializa la cámara
            await CrossMedia.Current.Initialize();



            // Si la cámara no está disponible o no está soportada lanza un mensaje y termina este método
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }



            // Toma la fotografía y la regresa en el objeto file
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "AppSpotify",
                Name = "camPicture.jpg"
            });



            // Si el objeto file es null termina este método
            if (file == null) return;



            // Asignamos la ruta de la fotografía en la propiedad de la imagen
            Picture = SongSelected.ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path);

        }

        private async void SelectPictureAction()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Pick Photo", ":( No pick photo available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            Picture = SongSelected.ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path); //file.Path;


        }
    }
}

