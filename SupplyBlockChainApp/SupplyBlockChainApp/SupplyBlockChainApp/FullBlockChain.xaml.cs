using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupplyBlockChainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FullBlockChain : ContentPage
	{
		public FullBlockChain ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void BlockChainButton_Clicked(object sender, EventArgs e)
        {
            LoadingOverlay.IsVisible = true;
            await Task.Run(async () =>
            {
                string url = "http://supplyblockchain.sudeshkumar.me/BlockChain/GetFullBlockChain";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("userName", Application.Current.Properties["UserName"].ToString()), new KeyValuePair<string, string>("password", Application.Current.Properties["Password"].ToString()) });
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                        HttpResponseMessage response = await getResponse;
                        if (response.IsSuccessStatusCode)
                        {
                            var myContent = await response.Content.ReadAsStringAsync();
                            if (myContent == "False")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    var Message = "Can't get blockchain at present. Try again after some time.";
                                    DisplayAlert("Error", Message, "OK");
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    BlockChainText.Text = myContent;
                                    BlockChainText.IsVisible = true;
                                    BlockChainButton.IsVisible = false;
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                var Message = "Server Is Down. Try Again After Some Time";
                                DisplayAlert("Error", Message, "OK");
                                LoadingOverlay.IsVisible = false;
                                return;
                            });
                        }
                    }
                    catch (Exception)
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var Message = "Check Your Internet Connection and Try Again";
                            DisplayAlert("Error", Message, "OK");
                            LoadingOverlay.IsVisible = false;
                            return;
                        });
                    }
                }
            });
        }
    }
}