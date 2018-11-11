using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            ScannerPage.Title = "Scan QrCode";
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
                        string url = "http://supplyblockchain.sudeshkumar.me/BlockChain/CheckTransactionID";
                        HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("ID", Application.Current.Properties["ScannedProductID"].ToString()) });
                        using (var httpClient = new HttpClient())
                        {
                            try
                            {
                                Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                                HttpResponseMessage response = await getResponse;
                                if (response.IsSuccessStatusCode)
                                {
                                    var myContent = await response.Content.ReadAsStringAsync();

                                    if (myContent == "True")
                                    {
                                        await Task.Run(async () =>
                                        {
                                            string url2 = "http://supplyblockchain.sudeshkumar.me/BlockChain/GetBlockChain";
                                            HttpContent q12 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
                                            using (var httpClient2 = new HttpClient())
                                            {
                                                try
                                                {
                                                    Task<HttpResponseMessage> getResponse2 = httpClient2.PostAsync(url2, q12);
                                                    HttpResponseMessage response2 = await getResponse2;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        var myContent2 = await response2.Content.ReadAsStringAsync();
                                                        {
                                                            Device.BeginInvokeOnMainThread(() =>
                                                            {
                                                                BlockChain SupplyBlockChain = new BlockChain();
                                                                SupplyBlockChain.Chain = JsonConvert.DeserializeObject<List<Block>>(myContent2);

                                                                StackLayout newStackLayout = new StackLayout()
                                                                {
                                                                    VerticalOptions = LayoutOptions.StartAndExpand,
                                                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                                                    BackgroundColor=Color.Transparent
                                                                };

                                                                double TotalCost = 0;

                                                                foreach (var block in SupplyBlockChain.Chain)
                                                                {
                                                                    foreach (var Transaction in block.Transactions)
                                                                    {
                                                                        if (Transaction.ProductID == Application.Current.Properties["ScannedProductID"].ToString())
                                                                        {
                                                                            Label TransactionTime = new Label()
                                                                            {
                                                                                Text = "Transaction Time : " + Transaction.TransactionTimeStamp,
                                                                                FontAttributes = FontAttributes.Bold,
                                                                                FontSize = 15,
                                                                                TextColor = Color.FromHex("#1C405B")
                                                                            };
                                                                            Label ProcessDone = new Label()
                                                                            {
                                                                                Text = "Process Done : " + Transaction.ProcessDone,
                                                                                FontSize = 13,
                                                                                TextColor = Color.FromHex("#1C405B")
                                                                            };
                                                                            Label ProcessDoneBy = new Label()
                                                                            {
                                                                                Text = "Process Done By: " + Transaction.ProcessDoneBy,
                                                                                FontSize = 13,
                                                                                TextColor = Color.FromHex("#1C405B")
                                                                            };
                                                                            Label ProcessCost = new Label()
                                                                            {
                                                                                Text = "Process Cost : " + Transaction.CostOfProcess,
                                                                                FontSize = 13,
                                                                                TextColor = Color.FromHex("#1C405B")
                                                                            };
                                                                            Button LocationButton = new Button()
                                                                            {
                                                                                Text = "Location : "+Transaction.Latitude+", "+Transaction.Longitude,
                                                                                FontSize = 12,
                                                                                BackgroundColor = Color.FromHex("#111C405B"),
                                                                                TextColor = Color.FromHex("#1C405B"),
                                                                                StyleId = Transaction.Latitude + ";" + Transaction.Longitude,
                                                                                VerticalOptions=LayoutOptions.CenterAndExpand,
                                                                                HorizontalOptions=LayoutOptions.CenterAndExpand,
                                                                                Margin=new Thickness(0,0,0,20),
                                                                                CornerRadius=30
                                                                            };
                                                                            LocationButton.Clicked += LocationButton_Clicked;
                                                                            newStackLayout.Children.Add(TransactionTime);
                                                                            newStackLayout.Children.Add(ProcessDone);
                                                                            newStackLayout.Children.Add(ProcessDoneBy);
                                                                            newStackLayout.Children.Add(ProcessCost);
                                                                            newStackLayout.Children.Add(LocationButton);
                                                                            TotalCost += Transaction.CostOfProcess;
                                                                        }
                                                                    }
                                                                }
                                                                Label TotalCostLabel = new Label()
                                                                {
                                                                    Text = "Total Cost Till Now : " + TotalCost.ToString(),
                                                                    FontAttributes = FontAttributes.Bold,
                                                                    FontSize = 15,
                                                                    TextColor = Color.FromHex("#1C405B")
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
                                                            ScanQrCodeButton.IsVisible = true;
                                                            MainLayout.IsVisible = true;
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
                                                        ScanQrCodeButton.IsVisible = true;
                                                        MainLayout.IsVisible = true;
                                                        LoadingOverlay.IsVisible = false;
                                                        return;
                                                    });
                                                }
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            Label label = new Label()
                                            {
                                                Text = "Product Is not present in the system",
                                                TextColor = Color.Red
                                            };
                                            MainLayout.Children.Add(label);
                                            MainScrollView.IsVisible = false;
                                            MainLayout.IsVisible = true;
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
                                        ScanQrCodeButton.IsVisible = true;
                                        MainLayout.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
                                        return;
                                    });
                                }
                            }
                            catch (Exception ex)
                            {

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    var Message = "Check Your Internet Connection and Try Again";
                                    DisplayAlert("Error", Message, "OK");
                                    ScanQrCodeButton.IsVisible = true;
                                    MainLayout.IsVisible = true;
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

        private async void LocationButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var tempString = button.StyleId.Split(';');
            double.TryParse(tempString[0], out double Latitude);
            double.TryParse(tempString[1], out double Longitude);
            var location = new Location(Latitude, Longitude);
            var options = new MapsLaunchOptions { Name = "Transaction Location" };
            await Maps.OpenAsync(location, options);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}