using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppSpotify.ViewModels
{
    public class LocationViewModel :BaseViewModel
    {
        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        // Propiedades
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
        public LocationViewModel()
        { }

        // Métodos
        private async void GetLocationAction()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(); //GetLastKnownLocationAsync();
                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;

                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("AppTriLocCam", "No se pudo obtener la ubicación", "Ok");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
}

