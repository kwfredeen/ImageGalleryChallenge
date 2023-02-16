using ImageGalleryChallenge.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ImageGalleryChallenge.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel _vm;

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _vm = new ItemDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.OnAppearing();
            ImageScroller.Position = _vm.Position;
        }
    }
}