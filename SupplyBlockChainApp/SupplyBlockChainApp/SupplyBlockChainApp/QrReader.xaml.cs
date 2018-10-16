using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SupplyBlockChainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QrReader : ContentPage
	{
		public QrReader ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void ScanQrCodeButton_Clicked(object sender, EventArgs e)
        {
            var ScannerPage = new ZXingScannerPage();
            if (!Application.Current.Properties.ContainsKey("ScannedProductID"))
            {
                Application.Current.Properties.Add("ScannedProductID", string.Empty);
            }
            Application.Current.Properties["ScannedProductID"] = string.Empty;
            ScannerPage.OnScanResult += (result) => {
                ScannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    Application.Current.Properties["ScannedProductID"] = result.Text;
                    LoadingOverlay.IsVisible = true;
                    LoadingIndicatorText.Text = "Fetching Product's Transactions";
                    ScanQrCodeButton.IsVisible = false;
                    MainLayout.IsVisible = false;
                    await Task.Run(async () =>
                    {
                        string url = "http://test.sudeshkumar.me/BlockChain/GetBlockChain";
                        HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
                        using (var httpClient = new HttpClient())
                        {
                            try
                            {
                                Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                                HttpResponseMessage response = await getResponse;
                                if (response.IsSuccessStatusCode)
                                {
                                    var myContent = await response.Content.ReadAsStringAsync();
                                    {
                                        Device.BeginInvokeOnMainThread( () =>
                                        {
                                            BlockChain SupplyBlockChain = JsonConvert.DeserializeObject<BlockChain>(myContent);

                                            StackLayout newStackLayout = new StackLayout()
                                            {
                                                VerticalOptions = LayoutOptions.StartAndExpand,
                                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                            };

                                            double TotalCost = 0;

                                            foreach (var block in SupplyBlockChain.Chain)
                                            {
                                                foreach (var Transaction in block.Transactions)
                                                {
                                                    if(Transaction.ProductID == Application.Current.Properties["ScannedProductID"].ToString())
                                                    {
                                                        Label TransactionTime = new Label()
                                                        {
                                                            Text = "Transaction Time : " + Transaction.TransactionTimeStamp,
                                                            FontAttributes = FontAttributes.Bold,
                                                            FontSize=14,
                                                            TextColor=Color.Black
                                                        };
                                                        Label ProcessDone = new Label()
                                                        {
                                                            Text = "Process Done : " + Transaction.ProcessDone,
                                                            FontAttributes = FontAttributes.Italic,
                                                            FontSize = 13,
                                                            TextColor = Color.Gray
                                                        };
                                                        Label ProcessDoneBy = new Label()
                                                        {
                                                            Text = "Process Done By: " + Transaction.ProcessDoneBy,
                                                            FontAttributes = FontAttributes.Italic,
                                                            FontSize = 13,
                                                            TextColor = Color.Gray
                                                        };
                                                        Label ProcessCost = new Label()
                                                        {
                                                            Text = "Process Cost : " + Transaction.CostOfProcess,
                                                            FontAttributes = FontAttributes.Italic,
                                                            FontSize = 13,
                                                            TextColor = Color.Gray
                                                        };
                                                        newStackLayout.Children.Add(TransactionTime);
                                                        newStackLayout.Children.Add(ProcessDone);
                                                        newStackLayout.Children.Add(ProcessDoneBy);
                                                        newStackLayout.Children.Add(ProcessCost);
                                                        TotalCost += Transaction.CostOfProcess;
                                                    }
                                                }
                                            }
                                            Label TotalCostLabel = new Label()
                                            {
                                                Text = "Total Cost Till Now : " + TotalCost.ToString(),
                                                FontAttributes = FontAttributes.Bold,
                                                FontSize = 15,
                                                TextColor = Color.CornflowerBlue
                                            };
                                            newStackLayout.Children.Add(TotalCostLabel);
                                            MainLabel.Text = "Transaction Details";
                                            this.Title = "Transaction Details";
                                            MainScrollView.Content = newStackLayout;
                                            MainScrollView.IsVisible = true;
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
                });
            };
            await Navigation.PushAsync(ScannerPage);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}