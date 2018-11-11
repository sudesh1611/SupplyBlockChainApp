using Newtonsoft.Json;
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
	public partial class MineTransaction : ContentPage
	{
		public MineTransaction ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void StartMiningButton_Clicked(object sender, EventArgs e)
        {
            LoadingOverlay.IsVisible = true;
            var FullName = FullNameEntry.Text;
            var Username = UserNameEntry.Text;
            var EmailID = EmailIDEntry.Text;
            var Password = PasswordEntry.Text;
            var ConfirmPassword = ConfirmPasswordEntry.Text;

            if(String.IsNullOrWhiteSpace(FullName) || String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(EmailID) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(ConfirmPassword))
            {
                var Message = "Fill out every entry";
                await DisplayAlert("Error", Message, "OK");
                LoadingOverlay.IsVisible = false;
                return;
            }
            if(Username.Contains(" "))
            {
                var Message = "Username can't contain whitespace";
                await DisplayAlert("Error", Message, "OK");
                UserNameEntry.Focus();
                LoadingOverlay.IsVisible = false;
                return;
            }
            if(Username.Length<6)
            {
                var Message = "Username length should be atleast 6";
                await DisplayAlert("Error", Message, "OK");
                UserNameEntry.Focus();
                LoadingOverlay.IsVisible = false;
                return;
            }
            if (Password.Length < 6)
            {
                var Message = "Password length should be atleast 6";
                await DisplayAlert("Error", Message, "OK");
                PasswordEntry.Focus();
                LoadingOverlay.IsVisible = false;
                return;
            }
            if(Password!=ConfirmPassword)
            {
                var Message = "Passwords don't match";
                await DisplayAlert("Error", Message, "OK");
                PasswordEntry.Focus();
                LoadingOverlay.IsVisible = false;
                return;
            }
            List<string> AccessRights = new List<string>();
            if(AdminCheck.Checked)
            {
                AccessRights.Add("Admin");
            }
            if(CreateAccountCheck.Checked)
            {
                AccessRights.Add("CreateAccount");
            }
            if(CreateTransactionCheck.Checked)
            {
                AccessRights.Add("CreateTransaction");
            }
            var newUser = new User()
            {
                FullName = FullName,
                UserName = Username.ToLower(),
                EmailID = EmailID.ToLower(),
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                AccessRights = JsonConvert.SerializeObject(AccessRights),
                AssociatedProductTypes = JsonConvert.SerializeObject(new List<string>())
            };
            MainLayout.IsVisible = false;
            await Task.Run(async () =>
            {
                string url = "http://supplyblockchain.sudeshkumar.me/UserAccount/CreateAccount";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("userName", Application.Current.Properties["UserName"].ToString()), new KeyValuePair<string, string>("password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("user", JsonConvert.SerializeObject(newUser)) });
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
                                    MainScrollView.Content = new Label()
                                    {
                                        Text = $"Account Successfully Created.\n\n UserName : {newUser.UserName} \n Password : {newUser.Password}",
                                        TextColor = Color.Green,
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold
                                    };
                                    CreateAccountButton.IsVisible = false;
                                    BackButton.IsVisible = true;
                                    MainScrollView.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                            else if(myContent=="UserName")
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    var Message = "Username already exists!";
                                    DisplayAlert("Error", Message, "OK");
                                    UserNameEntry.Focus();
                                    MainLayout.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    MainScrollView.Content = new Label()
                                    {
                                        Text = "Account Creation Failed!!",
                                        TextColor = Color.Red,
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold
                                    };
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

        private void FullNameEntry_Completed(object sender, EventArgs e)
        {
            UserNameEntry.Focus();
        }

        private void UserNameEntry_Completed(object sender, EventArgs e)
        {
            EmailIDEntry.Focus();
        }

        private void EmailIDEntry_Completed(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            ConfirmPasswordEntry.Focus();
        }

        private void ConfirmPasswordEntry_Completed(object sender, EventArgs e)
        {

        }
    }
}