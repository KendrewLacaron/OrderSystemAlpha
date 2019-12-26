using K5OrderBeta.Models;
using K5OrderBeta.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K5OrderBeta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionDetailView : ContentPage
	{
        public string pm;

        public TransactionDetailView ()
		{
			InitializeComponent ();
		}
        

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            App.K5OrderingDatabase.LoadStatus = "Loading Transaction Details";
            await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
            var e = await App.K5OrderingDatabase.GetOrderDetailByCode(pm);
            var q = await App.K5OrderingDatabase.GetOrderByDocnum(pm);
            listView.ItemsSource = e;
            txtPONumber.Text = q.poNumber;
            txtCartVal.Text = e.Count.ToString();
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void BtnCancelOrder_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Baka", "Cancel Order?", "Yes", "No");
            if (answer)
            {
                var w = await App.K5OrderingDatabase.GetOrderByDocnum(pm);
                await App.K5OrderingDatabase.RemoveOrderByDocnum(w);
                await DisplayAlert("Baka", "Order Successfully Cancelled", "Yata");
                await Navigation.PopAsync();
            }


        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var u = await App.K5OrderingDatabase.GetOrderDetailByCode(pm);
                listView.ItemsSource = u;
            }

            else
            {
                List<GlobalReport> ls;
                ls = await App.K5OrderingDatabase.GetOrderDetailByCode(pm);
                var cs = ls.Where(x => x.productName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
                listView.ItemsSource = cs;
            }
        }

        private void BtnViewSign_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewTransacSignPage { PONum = txtPONumber.Text }, true);
        }

    }
}