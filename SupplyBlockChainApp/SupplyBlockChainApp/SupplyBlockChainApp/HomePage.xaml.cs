using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupplyBlockChainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

        private async void ViewDetailsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrReader(), true);
        }

        private async void CreateTransactionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTransaction(), true);
        }

        private async void MineTransactionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MineTransaction(), true);
        }

        private async void CreateAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccount(), true);
        }

        private async void LogOutButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["UserName"] = string.Empty;
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopToRootAsync();
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}