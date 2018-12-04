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
	public partial class SignIn : ContentPage
	{
		public SignIn ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void UsernameEntry_Focused(object sender, FocusEventArgs e)
        {
            if(UsernameEntry.Text==null)
            {
                UsernameEntry.Placeholder = "";
                await UsernameLabel.TranslateTo(5, 25, 0, null);
                UsernameLabel.TextColor = Color.FromHex("#1C405B");
                await UsernameLabel.TranslateTo(5, 0, 100, Easing.Linear);
            }
            else if(UsernameEntry.Text=="")
            {
                UsernameEntry.Placeholder = "";
                await UsernameLabel.TranslateTo(5, 25, 0, null);
                UsernameLabel.TextColor = Color.FromHex("#1C405B");
                await UsernameLabel.TranslateTo(5, 0, 100, Easing.Linear);
            }
        }

        private async void UsernameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if(UsernameEntry.Text==null)
            {
                await UsernameLabel.TranslateTo(5, 25, 100, Easing.Linear);
                UsernameLabel.TextColor = Color.Transparent;
                UsernameEntry.Placeholder = "Username";
            }
            else if(UsernameEntry.Text=="")
            {
                await UsernameLabel.TranslateTo(5, 25, 100, Easing.Linear);
                UsernameLabel.TextColor = Color.Transparent;
                UsernameEntry.Placeholder = "Username";
            }
        }

        private void UsernameEntry_Completed(object sender, EventArgs e)
        {
            var input = UsernameEntry.Text;
            if(input==null)
            {
                DisplayAlert("Error", "Username is too short (minimum is 6 characters)", "OK");
                UsernameEntry.Focus();
            }
            else
            {
                
                input = input.ToLower();
                if (input.Length >20)
                {
                    DisplayAlert("Error", "Username is too long (maximum is 20 characters)", "OK");
                    UsernameEntry.Focus();
                    return;
                }
                if (input.Length < 6)
                {
                    DisplayAlert("Error", "Username is too short (minimum is 6 characters)", "OK");
                    UsernameEntry.Focus();
                    return;
                }
                if(input.Contains(' '))
                {
                    DisplayAlert("Error", "Username Can't Have Whitespaces", "OK");
                    UsernameEntry.Focus();
                    return;
                }
                bool temp = true;
                foreach(var item in input)
                {
                    if(!char.IsLetterOrDigit(item))
                    {
                        temp = false;
                        break;
                    }
                }
                if(!temp)
                {
                    DisplayAlert("Error", "Username Can Only Contain Alphabets or Digits", "OK");
                    UsernameEntry.Focus();
                    return;
                }
                PasswordEntry.Focus();
            }
        }

        private async void PasswordEntry_Focused(object sender, FocusEventArgs e)
        {
            if (PasswordEntry.Text == null)
            {
                PasswordEntry.Placeholder = "";
                await PasswordLabel.TranslateTo(5, 25, 0, null);
                PasswordLabel.TextColor = Color.FromHex("#1C405B");
                await PasswordLabel.TranslateTo(5, 0, 100, Easing.Linear);
            }
            else if(PasswordEntry.Text=="")
            {
                PasswordEntry.Placeholder = "";
                await PasswordLabel.TranslateTo(5, 25, 0, null);
                PasswordLabel.TextColor = Color.FromHex("#1C405B");
                await PasswordLabel.TranslateTo(5, 0, 100, Easing.Linear);
            }
        }

        private async void PasswordEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (PasswordEntry.Text == null)
            {
                await PasswordLabel.TranslateTo(5, 25, 100, Easing.Linear);
                PasswordLabel.TextColor = Color.Transparent;
                PasswordEntry.Placeholder = "Password";
            }
            else if (PasswordEntry.Text == "")
            {
                await PasswordLabel.TranslateTo(5, 25, 100, Easing.Linear);
                PasswordLabel.TextColor = Color.Transparent;
                PasswordEntry.Placeholder = "Password";
            }
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            var input = PasswordEntry.Text;
            if (input == null)
            {
                DisplayAlert("Error", "Password is too short (minimum is 6 characters)", "OK");
                PasswordEntry.Focus();
            }
            else
            {
                input = input.ToLower();
                if (input.Length > 20)
                {
                    DisplayAlert("Error", "Password is too long (maximum is 20 characters)", "OK");
                    PasswordEntry.Focus();
                    return;
                }
                if (input.Length < 6)
                {
                    DisplayAlert("Error", "Password is too short (minimum is 6 characters)", "OK");
                    PasswordEntry.Focus();
                    return;
                }
                if (input.Contains(' '))
                {
                    DisplayAlert("Error", "Password Can't Have Whitespaces", "OK");
                    PasswordEntry.Focus();
                    return;
                }
                LogInButton_Clicked(null, null);
            }
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            var Username = UsernameEntry.Text;
            var Password = PasswordEntry.Text;
            if (Username == null)
            {
                await DisplayAlert("Error", "Username is too short (minimum is 6 characters)", "OK");
                UsernameEntry.Focus();
                return;
            }
            if (Password == null)
            {
                var Message = "Password is too short (minimum is 6 characters)";
                await DisplayAlert("Error", Message, "OK");
                PasswordEntry.Focus();
                return;
            }
            if (Username.Length < 6)
            {
                var Message = "Username is too short (minimum is 6 characters)";
                await DisplayAlert("Error", Message, "OK");
                UsernameEntry.Focus();
                return;
            }
            if (Password.Length < 6)
            {
                var Message = "Password is too short (minimum is 6 characters)";
                await DisplayAlert("Error", Message, "OK");
                PasswordEntry.Focus();
                return;
            }
            if (Username.Length > 20)
            {
                var Message = "Username is too long (maximum is 20 characters)";
                await DisplayAlert("Error", Message, "OK");
                UsernameEntry.Focus();
                return;
            }
            if (Password.Length > 20)
            {
                var Message = "Password is too long (maximum is 20 characters)";
                await DisplayAlert("Error", Message, "OK");
                PasswordEntry.Focus();
                return;
            }
            MainFrame.IsVisible = false;
            LoadingOverlay.IsVisible = true;
            LoadingIndicatorText.Text = "Checking Credentials";
            Username = Username.ToLower();
            await Task.Run(async () =>
            {
                string url = "http://supplyblockchain.sudeshkumar.me/UserAccount/LogIn";
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("userName",Username), new KeyValuePair<string, string>("password", Password) });
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
                                    var Message = "Invalid Login Attempt!!!";
                                    DisplayAlert("Error", Message, "OK");
                                    UsernameEntry.Focus();
                                    MainFrame.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    User user = JsonConvert.DeserializeObject<User>(myContent);
                                    Application.Current.Properties["UserName"] = user.UserName;
                                    Application.Current.Properties["Password"] = user.Password;
                                    Application.Current.Properties["FullName"] = user.FullName;
                                    Application.Current.Properties["EmailID"] = user.EmailID;
                                    Application.Current.Properties["AccessRights"] = user.AccessRights;
                                    Application.Current.Properties["AssociatedProductTypes"] = user.AssociatedProductTypes;
                                    await Application.Current.SavePropertiesAsync();
                                    MainFrame.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    Application.Current.MainPage = new NavigationPage(new HomePage());
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
                                MainFrame.IsVisible = true;
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
                            MainFrame.IsVisible = true;
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
    }
}