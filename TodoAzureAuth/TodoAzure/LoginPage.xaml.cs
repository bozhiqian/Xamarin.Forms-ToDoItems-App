using System;
using Xamarin.Forms;

namespace TodoAzure
{
    public partial class LoginPage : ContentPage
    {
        private bool _authenticated;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.Authenticator != null) _authenticated = await App.Authenticator.AuthenticateAsync();

                if (_authenticated) Application.Current.MainPage = new TodoList();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                    messageLabel.Text = "Authentication cancelled by the user";
            }
            catch (Exception)
            {
                messageLabel.Text = "Authentication failed";
            }
        }
    }
}