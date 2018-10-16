using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace SupplyBlockChainApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn(), true);
        }

        private void ScanButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QrReader(), true);
        }
    }
}
