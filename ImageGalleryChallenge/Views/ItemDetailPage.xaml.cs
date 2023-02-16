using ImageGalleryChallenge.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ImageGalleryChallenge.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}