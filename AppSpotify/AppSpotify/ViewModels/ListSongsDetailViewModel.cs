using System;
using System.Collections.Generic;
using System.Text;
using AppSpotify.Models;
using AppSpotify.Service;
using AppSpotify.Views;
using Newtonsoft.Json;
using Xamarin.Forms;



namespace AppSpotify.ViewModels
{
    public class ListSongsDetailViewModel:BaseViewModel
    {
        static ListSongsDetailViewModel instance;

        // Propiedades
        private List<SongModel> _Songs;
        public List<SongModel> Songs
        {
            get => _Songs;
            set => SetProperty(ref _Songs, value);
        }

        private SongModel _SongSelected;
        public SongModel SongSelected
        {
            get => _SongSelected;
            set
            {
                if (SetProperty(ref _SongSelected, value))
                {
                    SelectSongAction();
                }
            }
        }

        // Constructores

        public ListSongsDetailViewModel()
        {
            instance = this;

            LoadSongs();
        }

        // Metodos

        public static ListSongsDetailViewModel GetInstance()
        {
            return instance;
        }

        private void NewAction(object obj)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ListSongsPage());
        }
        private List<SongModel> _ListSongs;
        public List<SongModel> ListSongs
        {
            get => _ListSongs;
            set => SetProperty(ref _ListSongs, value);
        }
        public async void LoadSongs()
        {
            IsBusy = true;
            ListSongs = null;
            ApiResponse response = await new ApiService().GetDataAsync("Product");
            if (response == null || !response.IsSucces)
            {
                // No hubo una respuesta exitosa
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("AppProducts", $"Error al cargar los productos: {response.Message}", "Ok");
                return;
            }
            ListSongs = JsonConvert.DeserializeObject<List<SongModel>>(response.Result.ToString());
            IsBusy = false;
        }

        private void SelectSongAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ListSongsPage(SongSelected));
        }

        public void RefreshSongs()
        {
            LoadSongs();
        }

    }
}

