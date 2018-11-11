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
    public partial class ScanOldProduct : ContentPage
    {
        public ScanOldProduct()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void CreateTransactionButton_Clicked(object sender, EventArgs e)
        {
            LoadingOverlay.IsVisible = true;
            LoadingIndicatorText.Text = "Creating Transaction";
            string ProcessDone = ProcessDoneEditor.Text;
            string ProcessDoneBy = Application.Current.Properties["FullName"].ToString();
            if ( string.IsNullOrEmpty(ProcessDone))
            {
                await DisplayAlert("Error", "Fill out all the fields", "Okay");
                LoadingOverlay.IsVisible = false;
                return;
            }
            double ProcessCost = 0;
            if (double.TryParse(ProductCostEntry.Text, out ProcessCost))
            {

            }
            else
            {
                await DisplayAlert("Error", "Product Cost can only be number", "Okay");
                LoadingOverlay.IsVisible = false;
                return;
            }
            string ProductID = Application.Current.Properties["ScannedProductID"].ToString();
            Location location = new Location();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "Device doesn't support gps location", "Okay");
                LoadingOverlay.IsVisible = false;
                return;
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Error", "Give Location Access to application", "Okay");
                LoadingOverlay.IsVisible = false;
                return;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Can't Get Location", "Okay");
                LoadingOverlay.IsVisible = false;
                return;
            }
            Transaction newTransaction = new Transaction(ProductID, ProcessDone, ProcessDoneBy, location.Latitude.ToString(), location.Longitude.ToString(), ProcessCost);

            await Task.Run(async () =>
            {
                string url = "http://supplyblockchain.sudeshkumar.me/BlockChain/CreateTransaction";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("transaction", JsonConvert.SerializeObject(newTransaction)), new KeyValuePair<string, string>("userName", Application.Current.Properties["UserName"].ToString()), new KeyValuePair<string, string>("password", Application.Current.Properties["Password"].ToString()) });
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
                                    var Message = "Can't create transactions at present. Try again after some time.";
                                    DisplayAlert("Error", Message, "OK");
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread( () =>
                                {
                                    Label label = new Label()
                                    {
                                        Text = "Transaction Committed Successfully",
                                        TextColor = Color.Green,
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold
                                    };
                                    MainScrollView.Content = label;
                                    MainScrollView.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    //var Message = "Transaction Added Successfully. Save this QrCode for further Transactions.";
                                    //await DisplayAlert("Success", Message, "OK");
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

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void ScanQrCodeButton_Clicked(object sender, EventArgs e)
        {
            var ScannerPage = new ZXingScannerPage();
            if (!Application.Current.Properties.ContainsKey("ScannedProductID"))
            {
                Application.Current.Properties.Add("ScannedProductID", string.Empty);
            }
            Application.Current.Properties["ScannedProductID"] = string.Empty;
            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    Application.Current.Properties["ScannedProductID"] = result.Text;
                    LoadingOverlay.IsVisible = true;
                    LoadingIndicatorText.Text = "Fetching Product's Details";
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
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            ProductIDEntry.Text = Application.Current.Properties["ScannedProductID"].ToString();
                                            MainScrollView.IsVisible = true;
                                            LoadingOverlay.IsVisible = false;
                                            return;
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
                                    MainLayout.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    Navigation.PopAsync(true);
                                    return;
                                });
                            }
                        }
                    });
                });
            };
            await Navigation.PushAsync(ScannerPage);
        }
    }
}