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
    public partial class ListSongsDetailPage : ContentPage
    {
        public ListSongsDetailPage()
        {
            InitializeComponent();
            BindingContext = new ListSongsDetailViewModel();
        }
    }
}