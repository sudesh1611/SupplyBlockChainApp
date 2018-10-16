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
	public partial class CreateTransaction : ContentPage
	{
		public CreateTransaction ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void NewProductButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNewProduct(), true);
        }

        private async void ScanQrCodeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanOldProduct(), true);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}