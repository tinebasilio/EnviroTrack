using EnviroTrackApp;
using System;
using Xamarin.Forms;

namespace EnviroTrackApp.View
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private async void OnGetStartedClicked(object sender, EventArgs e)
        {
            // Navigate to the main page or any other page you want to show after clicking "Get Started"
            await Navigation.PushAsync(new MainPage());
        }
    }
}
