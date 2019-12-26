using K5OrderBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using K5OrderBeta.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K5OrderBeta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {   
            return true;
        }

        private async void SrcProducts_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var u = await App.K5OrderingDatabase.GetAllProductsAsync();

                itemList.ItemsSource = u;
            }

            else
            {
                List<ProductMain> ls;
                ls = await App.K5OrderingDatabase.GetAllProductsAsync();
                var cs = ls.Where(x => x.productName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase) || x.productCode.ToString().Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));

                itemList.ItemsSource = cs;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            App.K5OrderingDatabase.LoadStatus = "Loading Products";
            await PopupNavigation.Instance.PushAsync(new LoadingPopup(),true);
            var e = await App.K5OrderingDatabase.GetAllProductsAsync();
            itemList.ItemsSource = e;
            var c = await App.K5OrderingDatabase.NoItemsInCart();
            txtCartVal.Text = c.ToString();
            await PopupNavigation.Instance.PopAsync(true);
        }

        public class ProductPair : Tuple<ProductMain, ProductMain>
        {
            public ProductPair(ProductMain item1, ProductMain item2)
                : base(item1, item2 ?? CreateEmptyModel()) { }

            private static ProductMain CreateEmptyModel()
            {
                return new ProductMain { isVisible = false };
            }
        }

        private async void BtnFProd_Clicked(object sender, EventArgs e)
        {
            string s = (sender as Button).ClassId;
            await Navigation.PushAsync(new ProductPage { BindingContext = await App.K5OrderingDatabase.GetProductByCode(s) });
        }

        private async void ItemList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ProductPage { BindingContext = e.SelectedItem as ProductMain });
        }

        private async void BtnAddProd_Clicked(object sender, EventArgs e)
        {
            App.K5OrderingDatabase.AddProduct();
            await DisplayAlert("Baka", "Added New Dummy", "Yata");
        }

        private async void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string s = (sender as Button).ClassId;
            //await Navigation.PushAsync(new ProductPage { BindingContext = await App.RPIOrderingMainDatabase.GetProductByCode(s) });
            string current = (e.CurrentSelection.FirstOrDefault() as ProductMain)?.productCode;
            //await DisplayAlert("Baka", current, "Yosh");
            //var prod = await App.RPIOrderingMainDatabase.GetProductByCode(current);

            //    var op = new ProductMain()
            //    {
            //        productCode = prod.productCode,
            //        productName = prod.productName,
            //        productDesc = prod.productDesc

            //};

            await Navigation.PushAsync(new ProductPage { pm = current }, true);
        }

        private async void BtnMore_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("More", "Cancel", null, "Show Customer", "Change Customer", "Cancel Transaction");
            if (action == "Show Customer")
            {
                Customer a = await App.K5OrderingDatabase.GetCustomerByCode(App.K5OrderingDatabase.CurrentCustomerCode);
                await DisplayAlert("Customer", a.customerName, "Yata");
            }
            if (action == "Change Customer")
            {
                bool oo = await DisplayAlert("Transaction", "Change Customer ?", "Okay", "Cancel That");
                if (oo)
                {
                    App.K5OrderingDatabase.LoadStatus = "Changing Customer";
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    App.K5OrderingDatabase.CurrentCustomerCode = null;
                    await App.K5OrderingDatabase.SaveLogPref(7, App.K5OrderingDatabase.CurrentCustomerCode);
                    await Navigation.PopAsync();
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
            if(action == "Cancel Transaction")
            {
                bool oo = await DisplayAlert("Transaction","Cancel Current Transaction ?","Okay","Cancel That");
                if (oo)
                {
                    App.K5OrderingDatabase.LoadStatus = "Cancelling Transaction";
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                    await App.K5OrderingDatabase.SaveLogPref(7, App.K5OrderingDatabase.CurrentCustomerCode);
                    await App.K5OrderingDatabase.ResetTransaction();
                    await Navigation.PopAsync();
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
        }

        private void BtnCart_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Cart(), true);
        }

    }
}