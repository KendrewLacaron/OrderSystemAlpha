using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using K5OrderBeta.Models;

namespace K5OrderBeta.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeConnectionPopup : PopupPage
    {
        public ChangeConnectionPopup()
        {
            InitializeComponent();

        }

        //protected  async override void ChangeVisualState()
        //{
        //    base.ChangeVisualState();

        //    IPConfig a = await App.K5OrderingDatabase.GetLatestConfig();

        //    if (a == null)
        //    {
        //        //No Config Found || First Installation
        //    }
        //    else
        //    {
        //        App.K5OrderingDatabase.IP = a.ConfigIP;
        //        App.K5OrderingDatabase.Port = a.ConfigPort;
        //    }

        //}
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = await App.K5OrderingDatabase.GetLatestConfig();
            //IPConfig a = await App.K5OrderingDatabase.GetLatestConfig();

            //if (a == null)
            //{
            //    //No Config Found || First Installation
            //}
            //else
            //{
            //    App.K5OrderingDatabase.IP = a.ConfigIP;
            //    App.K5OrderingDatabase.Port = a.ConfigPort;
            //}
        }

        private async void BtnSaveIP_Clicked(object sender, EventArgs e)
        {
            if(txtPort.Text != null && txtPort.Text != string.Empty && txtIP.Text != null && txtIP.Text != string.Empty)
            {
                //App.K5OrderingDatabase.Port = txtPort.Text;
                //App.K5OrderingDatabase.IP = txtIP.Text;
                await App.K5OrderingDatabase.SaveIPConfiguration(txtIP.Text, txtPort.Text);
                await DisplayAlert("Baka...", "IP Saved", "Wakata");
            }
            else
            {
                await DisplayAlert("Baka...", "Enrty must not be null", "Wakata");
            }
        }

        private async void BtnImport_Clicked(object sender, EventArgs e)
        {
            bool oo = await DisplayAlert("Hey", "This will delete the application data and add a new one \r\n\r\n Continue?", "Alright", "Cancel That");

            if (oo && txtIP != null && txtIP.Text != string.Empty)
            {
                await App.K5OrderingDatabase.SaveIPConfiguration(txtIP.Text, txtPort.Text);
                //App.K5OrderingDatabase.Port = txtPort.Text;
                //App.K5OrderingDatabase.IP = txtIP.Text;
                IPConfig ipconf = await App.K5OrderingDatabase.GetLatestConfig();
                App.K5OrderingDatabase.LoadStatus = "Importing Data";
                App.K5OrderingDatabase.AdditionalLoadInfo = ipconf.ConfigIP + ":" + ipconf.ConfigPort ;

                await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                try
                {

                    if (await App.K5OrderingDatabase.SaveAllImportedDataToSQLite() == 0)
                    {
                        await App.K5OrderingDatabase.SaveLogPref(3);
                        await DisplayAlert("Hey", "All Data Successfully Imported", "Alright");
                    }
                    else
                    {

                        await DisplayAlert("Hey", "Data has not been added", "Alright");
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Baka..", ex.Message, "Taskowarimashta");

                }
                finally
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    App.K5OrderingDatabase.AdditionalLoadInfo = "";
                }

            }
            else
            {
                await DisplayAlert("Hey", "Operation Canceled | Complete Entry", "Alright");
            }
        }
    }
     
}