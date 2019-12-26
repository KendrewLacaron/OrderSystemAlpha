using K5OrderBeta.Models;
using K5OrderBeta.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K5OrderBeta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            IPConfig a = await App.K5OrderingDatabase.GetLatestConfig();

            if(a == null)
            {
                //No Config Found || First Installation
            }
            else
            {
                App.K5OrderingDatabase.IP = a.ConfigIP;
                App.K5OrderingDatabase.Port = a.ConfigPort;
            }
           
            //foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            //{
            //    App.K5OrderingDatabase.IP = address.ToString();

            //    break;
            //}
            //UserPrefs log = new UserPrefs();

            //await App.K5OrderingDatabase.AddAccessPref();

            //bool henji = App.K5OrderingDatabase.GetLogPref().IsFaulted;

            //if (henji)
            //{
            var log = await App.K5OrderingDatabase.GetLogPref();

            if(log == null)
            {
                //No Pref Found || First Installation
            }
            else
            {
                //if (App.K5OrderingDatabase.CurrentUserCode != null)
                //{
                //    App.K5OrderingDatabase.LoadStatus = "Signing In";
                //    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                //    await Navigation.PushAsync(new UserActivityPage());

                //}
                if (log.prefType == 1 || /*log.prefType == 3 || */log.prefType == 4 || log.prefType == 5 || log.prefType == 6 || log.prefType == 7) /*log.prefType == 1 || log.prefType == 3 || log.prefType == 4*/
                {
                    App.K5OrderingDatabase.LoadStatus = "Signing In";
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    App.K5OrderingDatabase.CurrentUserCode = log.userCode;
                    await Navigation.PushAsync(new UserActivityPage());
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }

            //}
        }


        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            User a = await App.K5OrderingDatabase.CheckUserLogin(txtUser.Text, txtPass.Text);
            if (a != null)
            {
                App.K5OrderingDatabase.LoadStatus = "Signing In";
                await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                App.K5OrderingDatabase.CurrentUserCode = a.userCode;
                await App.K5OrderingDatabase.SaveLogPref(1);
                //await DisplayAlert("System...", "Login Success", "Noice");
                await Navigation.PushAsync(new UserActivityPage());
                await PopupNavigation.Instance.PopAsync(true);


            }
            else
            {
                await DisplayAlert("Login Failed", "Username or Password is Incorrect", "Noice");
            }
        }

        private async void BtnAddUser_Clicked(object sender, EventArgs e)
        {
            App.K5OrderingDatabase.AddUser();
            await DisplayAlert("Debug", "New User Added", "Noice");
        }

        private async void BtnAddProduct_Clicked(object sender, EventArgs e)
        {
            //App.K5OrderingDatabase.AddProduct();

            IPConfig a = await App.K5OrderingDatabase.GetLatestConfig();

            if (a == null)
            {
                //No Config Found || First Installation
                await DisplayAlert("Baka", "No Config Found", "Yata");

            }
            else
            {
            await DisplayAlert("Baka",a.ConfigIP + " " + a.ConfigPort, "Yata");

            }

        }

        private async void BtnAddCustomer_Clicked(object sender, EventArgs e)
        {
            App.K5OrderingDatabase.AddCustomer();
            await DisplayAlert("Debug", "New Customer Added", "Noice");
        }

        private async void BtnConnect_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ChangeConnectionPopup());
        }
    }
}