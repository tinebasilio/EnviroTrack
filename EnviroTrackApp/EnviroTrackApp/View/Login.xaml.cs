using EnviroTrackApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnviroTrackApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btnLogin(object sender, EventArgs e)
        {
            if (txtUsername.Text == "user" && txtPassword.Text == "123")
            {
                Navigation.PushAsync(new LandingPage());
            }
            else
            {
                DisplayAlert("Please re-enter right credentials", "Username or Password is Incorrect", "Ok");
            }

        }
    }
}