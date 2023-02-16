using ImageGalleryChallenge.Models;
using ImageGalleryChallenge.ViewModels;
using ImageGalleryChallenge.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageGalleryChallenge.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            //try
            //{
            //    BindingContext = _viewModel = new ItemsViewModel();
            //} catch (Exception e)
            //{
            //    Debug.WriteLine(e.InnerException);
            //}
            BindingContext = _viewModel = new ItemsViewModel();

            Console.WriteLine("aaaaa");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}