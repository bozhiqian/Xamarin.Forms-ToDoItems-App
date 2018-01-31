using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace TodoAzure.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : IAuthenticate
    {
        // Define a authenticated user.
        MobileServiceUser user;

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize the authenticator before loading the app.
            TodoAzure.App.Init(this);

            this.LoadApplication(new TodoAzure.App());
        }

        #region IAuthenticate
        
        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;

            try
            {
                // Sign in with Twitter login using a server-managed flow.
                if (user == null)
                {
                    user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Twitter, Constants.URLScheme);
                    if (user != null)
                    {
                        var dialog = new MessageDialog(string.Format("You are now logged in - {0}", user.UserId), "Authentication");
                        await dialog.ShowAsync();
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Authentication Failed");
                await dialog.ShowAsync();
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
                    var dialog = new MessageDialog(string.Format("You are now logged out - {0}", user.UserId), "Logout");
                    await dialog.ShowAsync();
                }

                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Logout failed");
                await dialog.ShowAsync();
            }
            return success;
        }
        #endregion
    }
}