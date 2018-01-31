using Xamarin.Forms;

//https://developer.xamarin.com/samples/xamarin-forms/WebServices/TodoAzureAuth/
// https://docs.microsoft.com/en-gb/azure/app-service-mobile/app-service-mobile-xamarin-forms-get-started-users


namespace TodoAzure
{
    public class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public App()
        {
            MainPage = new LoginPage();
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

