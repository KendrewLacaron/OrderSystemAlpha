using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K5OrderBeta.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using K5OrderBeta.Popups;
namespace K5OrderBeta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        public Cart()
        {
            InitializeComponent();
        }


        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var u = await App.K5OrderingDatabase.GetCurrentCart();
                listView.ItemsSource = u;
            }

            else
            {
                List<CartService> ls;
                ls = await App.K5OrderingDatabase.GetCurrentCart();
                var cs = ls.Where(x => x.productName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
                listView.ItemsSource = cs;
            }
        }

        protected override async void OnAppearing()
        {

            base.OnAppearing();
            App.K5OrderingDatabase.LoadStatus = "Loading Cart";
            await PopupNavigation.Instance.PushAsync(new LoadingPopup(),true);
            var e = await App.K5OrderingDatabase.GetCurrentCart();
            listView.ItemsSource = e;
            var c = await App.K5OrderingDatabase.NoItemsInCart();
            txtCartVal.Text = c.ToString();
            await PopupNavigation.Instance.PopAsync(true);
        }


        private async void BtnCheckOut_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new OrderCheckoutPage(), true)
            Customer ww = await App.K5OrderingDatabase.GetCustomerByCode(App.K5OrderingDatabase.CurrentCustomerCode);
            bool he = await DisplayAlert("Cart", "Checkout For Customer : " + ww.customerName + " \r\n\r\nNo. of Items : " + txtCartVal.Text + " \r\n\r\nPO Number   : " + txtPONumber.Text + "\r\n\r\n\r\nConfirm Checkout ?", "Sure", "No");

            if (he && txtPONumber.Text != null)
            {
                await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
                if (await App.K5OrderingDatabase.ProccessAllTransacAsync(txtPONumber.Text))
                {
                    await App.K5OrderingDatabase.SaveLogPref(6, App.K5OrderingDatabase.CurrentCustomerCode);
                    await PopupNavigation.Instance.PopAsync(true);
                    await App.K5OrderingDatabase.ResetTransaction();
                    await DisplayAlert("Checkout", "Order Successfully Completed", "Nice");
                    await Navigation.PopToRootAsync();
                }
                else 
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Checkout", "Transaction Saving Failed", "Nice");

                }


            }
            else
            {
                await DisplayAlert("Checkout", "Note: PONumber must not be null ", "Sure");
            }
        }


        //private void BtnEdit_Clicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new ProductPage { pm = (sender as Button).ClassId , productRowCart = Convert.ToInt16((sender as Button).AutomationId) , isEdit=true }, true);
        //}

        //private void BtnRemove_Clicked(object sender, EventArgs e)
        //{
        //    //bool a = await DisplayAlert("Remove","Remove Product ?","Sure","No");
        //    //if (a)
        //    //{
        //    //    int q = Convert.ToInt16((sender as Button).AutomationId);
        //    //    await App.RPIOrderingMainDatabase.RemoveItemInCart(q);
        //    //    await DisplayAlert("Feed", "Product Removed", "Sure");
        //    //}
        //}

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pRowCart = (e.CurrentSelection.FirstOrDefault() as CartService).productRowCart;
            string pCode = (e.CurrentSelection.FirstOrDefault() as CartService)?.productCode;

            string action = await DisplayActionSheet("Commands", "Cancel", null, "Edit Order", "Remove Order");

            if (action == "Edit Order")
            {
                await Navigation.PushAsync(new ProductPage { pm = pCode, productRowCart = pRowCart, isEdit = true }, true);
            }

            if (action == "Remove Order")
            {
                bool a = await DisplayAlert("Remove", "Remove Product ?", "Sure", "No");
                if (a)
                {
                    await App.K5OrderingDatabase.RemoveItemInCart(pRowCart);
                    await DisplayAlert("Feed", "Product Removed", "Sure");
                    OnAppearing();
                }
            }
            else
            {
                OnAppearing();
            }
        }

    }
}