using K5OrderBeta.Models;
using K5OrderBeta.Popups;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace K5OrderBeta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserActivityPage : ContentPage
	{

        public UserActivityPage ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UserPrefs pref = await App.K5OrderingDatabase.GetLogPref();

            if(pref == null)
            {
                // Make Error
            }
            else
            {
                if (pref.prefType == 5 && pref.customerCode != null)
                {
                    App.K5OrderingDatabase.LoadStatus = "Resuming Transaction";
                    App.K5OrderingDatabase.CurrentCustomerCode = pref.customerCode;
                    await Navigation.PushAsync(new MainPage());
                }
            }

            //if (pref.customerCode == null)
            //{
            //    // Make Error
            //}
            //else
            //{
            //    if (pref.prefType == 5)
            //    {
            //        App.K5OrderingDatabase.CurrentCustomerCode = pref.customerCode;
            //        await Navigation.PushAsync(new MainPage());
            //    }
            //}
            User e = await App.K5OrderingDatabase.GetUserByCode(App.K5OrderingDatabase.CurrentUserCode);
            UserPrefs import = await App.K5OrderingDatabase.GetLogPref(3);
            UserPrefs export = await App.K5OrderingDatabase.GetLogPref(4);
            lblCurrentUser.Text = e.userName;
            if (import != null)
            {
                lblImportTime.Text = import.timeLog.ToString();
            }
            if (export != null)
            {
                lblExportTime.Text = export.timeLog.ToString();
            }

            
            txtCustomer.Text = null;
            txtCustomerCode.Text = null;
            custList.ItemsSource = null;

        }

        private async void BtnStartTransac_Clicked(object sender, EventArgs e)
        {
            if (txtCustomer.Text != null || txtCustomerCode.Text != null)
            {
                App.K5OrderingDatabase.LoadStatus = "Starting Transaction";
                App.K5OrderingDatabase.CurrentCustomerCode = txtCustomerCode.Text;
                await App.K5OrderingDatabase.SaveLogPref(5,txtCustomerCode.Text);
                await Navigation.PushAsync(new MainPage());
                //DisplayAlert("Baka", "Customer Code : " + App.RPIOrderingMainDatabase.CurrentCustomerCode +"\r\nUser Code : "+ App.RPIOrderingMainDatabase.CurrentUserCode, "Soka");

            }
            else
            {

                await DisplayAlert("Baka", "Please Select a Customer", "Okay");
            }

        }

        private void CustList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            txtCustomer.Text = (e.SelectedItem as Customer)?.customerName;
            txtCustomerCode.Text = (e.SelectedItem as Customer)?.customerCode;
            App.K5OrderingDatabase.CurrentCustomerCode = (e.SelectedItem as Customer)?.customerCode;

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {

            }
            else
            {
                List<Customer> ls;
                ls = await App.K5OrderingDatabase.GetAllCustomer();
                custList.ItemsSource = ls.Where(x => x.customerName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
            }
        }

        private async void BtnFunc_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("More", "Cancel", null, "Import Data", "Export Data", "Transactions","Logout");
            if (action == "Transactions")
            {
                await Navigation.PushAsync(new TransactionLogPage(), true);
            }
            if(action == "Logout")
            {
                bool oo = await DisplayAlert("Hey", "You will be logged out", "Alright", "Cancel That");
                if (oo)
                {
                    App.K5OrderingDatabase.LoadStatus = "Signing Out";
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    await App.K5OrderingDatabase.SaveLogPref(2);
                    await App.K5OrderingDatabase.Logout();
                    await Navigation.PopToRootAsync();
                    await PopupNavigation.Instance.PopAsync(true);
                }

            }
            if (action == "Export Data")
            {

                DataTable dtMain = App.K5OrderingDatabase.ConvertToDataTable(await App.K5OrderingDatabase.GetAllOrderMain());
                DataTable dtSub = App.K5OrderingDatabase.ConvertToDataTable(await App.K5OrderingDatabase.GetAllOrderSub());

                bool oo = await DisplayAlert("Hey", "This will update the table\r\n\r\nAre Previous Records Cleared?", "Alright", "Cancel That");
                if (oo)
                {
                    IPConfig ipconf = await App.K5OrderingDatabase.GetLatestConfig();
                    App.K5OrderingDatabase.LoadStatus = "Exporting Data";
                    App.K5OrderingDatabase.AdditionalLoadInfo = ipconf.ConfigIP + ":" + ipconf.ConfigPort;

                    var jsonMain = JsonConvert.SerializeObject(dtMain);
                    var contentMain = new StringContent(jsonMain, Encoding.UTF8, "application/json");

                    var jsonSub = JsonConvert.SerializeObject(dtSub);
                    var contentSub = new StringContent(jsonSub, Encoding.UTF8, "application/json");

                    //await DisplayAlert("Hey", "Processing...\r\n\r\nPlease wait for application feedback before proceeding!", "Alright");

                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    HttpClient client = new HttpClient();
                    try
                    {
                        var resulttMain = await client.PostAsync(await App.K5OrderingDatabase.IPMaker() +"/API/Order/K5/Export/OrderMain", contentMain);
                        App.K5OrderingDatabase.LoadStatus = resulttMain.StatusCode.ToString();

                        var resultSub = await client.PostAsync(await App.K5OrderingDatabase.IPMaker() + "/API/Order/K5/Export/OrderSub", contentSub);
                        App.K5OrderingDatabase.LoadStatus = resultSub.StatusCode.ToString();
                        if (resulttMain.StatusCode == HttpStatusCode.Created && resultSub.StatusCode == HttpStatusCode.Created)
                        {
                            await App.K5OrderingDatabase.SaveLogPref(4);
                            await DisplayAlert("Hey", "All Transactions Exported", "Alright");
                            //await DisplayAlert("Hey", "Main Status " + resulttMain.StatusCode.ToString() + "\r\n\r\nSub Status " + resultSub.StatusCode.ToString(), "Alright");
                            await App.K5OrderingDatabase.ResetAfterExport();
                            OnAppearing();
                        }
                        else
                        {

                            // await DisplayAlert("MessageBox",result+"","Ok");
                            await DisplayAlert("Hey", "Transactions has not been added", "Alright");
                            await DisplayAlert("Hey", "Main Status " + resulttMain.StatusCode.ToString() + "\r\n\r\nSub Status " + resultSub.StatusCode.ToString(), "Alright");
                        }
                        //await Navigation.PopAsync();

                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Baka..", ex.Message, "Taskowarimashta");
                        await DisplayAlert("Baka..", "No Connection Found", "Taskowarimashta");

                    }
                    finally
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        App.K5OrderingDatabase.AdditionalLoadInfo = "";


                    }

                }
                else
                {
                    await DisplayAlert("Hey", "Operation Canceled", "Alright");
                }
            }

            if (action == "Import Data")
            {

                bool oo = await DisplayAlert("Hey", "This will refresh your application data and log you out \r\n\r\n Continue?", "Alright", "Cancel That");

                if (oo)
                {
                    IPConfig ipconf = await App.K5OrderingDatabase.GetLatestConfig();

                    App.K5OrderingDatabase.LoadStatus = "Importing Data";
                    App.K5OrderingDatabase.AdditionalLoadInfo = ipconf.ConfigIP + ":" + ipconf.ConfigPort;

                    //await DisplayAlert("Hey", "Processing...\r\n\r\nPlease wait for application feedback before proceeding!", "Alright");
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    try
                    {

                        if (await App.K5OrderingDatabase.SaveAllImportedDataToSQLite() == 0)
                        {
                            await App.K5OrderingDatabase.SaveLogPref(3);
                            await DisplayAlert("Hey", "All Data Successfully Imported", "Alright");
                            await Navigation.PopToRootAsync();

                        }
                        else
                        {

                            // await DisplayAlert("MessageBox",result+"","Ok");
                            await DisplayAlert("Hey", "Data has not been added", "Alright");
                        }
                        //var a = App.K5OrderingDatabase.database;

                        //HttpClient client = new HttpClient();
                        //var response = await client.GetStringAsync("http://" + ngrok + ".ngrok.io/API/Order/K5/Import/User");
                        //var data = JsonConvert.DeserializeObject<List<User>>(response);
                        //await a.InsertAllAsync(data);

                        //HttpClient client1 = new HttpClient();
                        //var response1 = await client.GetStringAsync("http://" + ngrok + ".ngrok.io/API/Order/K5/Import/ProductMain");
                        //var data1 = JsonConvert.DeserializeObject<List<ProductMain>>(response);
                        //await a.InsertAllAsync(data1);

                        //HttpClient client2 = new HttpClient();
                        //var response2 = await client.GetStringAsync("http://" + ngrok + ".ngrok.io/API/Order/K5/Import/Customer");
                        //var data2 = JsonConvert.DeserializeObject<List<Customer>>(response);
                        //await a.InsertAllAsync(data2);

                        //await Navigation.PopAsync();
                        //await DisplayAlert("Hey", "All Data Successfully Imported", "Alright");
                        //await Navigation.PopToRootAsync();  


                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Baka..", ex.Message, "Taskowarimashta");
                        //await DisplayAlert("Baka..", "No Connection Found", "Taskowarimashta");

                    }
                    finally
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        App.K5OrderingDatabase.AdditionalLoadInfo = "";
                    }

                }
                else
                {
                    await DisplayAlert("Hey", "Operation Canceled", "Alright");
                }
            }
        }

    }
}