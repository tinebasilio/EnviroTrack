using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnviroTrackApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            var profileTapGestureRecognizer = new TapGestureRecognizer();
            profileTapGestureRecognizer.Tapped += OnProfileClicked;

            var aboutTapGestureRecognizer = new TapGestureRecognizer();
            aboutTapGestureRecognizer.Tapped += OnAboutClicked;

            ProfileLabel.GestureRecognizers.Add(profileTapGestureRecognizer);
            AboutLabel.GestureRecognizers.Add(aboutTapGestureRecognizer);
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            string res = await DisplayActionSheet("Profile", "Cancel", null, "Username: user");
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            string res = await DisplayActionSheet("About", "Cancel", null, "EnviroTrack is a mobile application built on Xamarin that encourages sustainable living through gamification with ecological awareness to inspire users towards adopting and maintaining eco-friendly habits.");
        }
    }
}