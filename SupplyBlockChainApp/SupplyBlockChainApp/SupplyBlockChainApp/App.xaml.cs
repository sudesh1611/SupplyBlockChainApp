using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SupplyBlockChainApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if(Application.Current.Properties.ContainsKey("UserName"))
            {
                if(Application.Current.Properties["UserName"].ToString() != string.Empty)
                {
                    MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage());
                }
            }
            else
            {
                Application.Current.Properties["Username"] = string.Empty;
                Application.Current.Properties["Password"] = string.Empty;
                Application.Current.Properties["FullName"] = string.Empty;
                Application.Current.Properties["EmailID"] = string.Empty;
                Application.Current.Properties["AccessRights"] = string.Empty;
                Application.Current.Properties["AssociatedProductTypes"] = string.Empty;
                Application.Current.SavePropertiesAsync();
                MainPage = new NavigationPage(new MainPage());
            }
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
