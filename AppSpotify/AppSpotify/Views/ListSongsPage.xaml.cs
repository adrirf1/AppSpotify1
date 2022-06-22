using AppSpotify.Models;
using AppSpotify.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppSpotify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListSongsPage : ContentPage
    {
        public ListSongsPage()
        {
            InitializeComponent();
            BindingContext = new ListSongsViewModel();
        }

        public ListSongsPage(SongModel songSelected)
        {
            InitializeComponent();

            BindingContext = new ListSongsViewModel(songSelected);
        }
    }
}