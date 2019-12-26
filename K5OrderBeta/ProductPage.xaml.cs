using K5OrderBeta.Models;
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
	public partial class ProductPage : ContentPage
	{
        public string pm;
        public int rowID;
        public int productRowCart;
        public bool isEdit = false;

        public ProductPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = await App.K5OrderingDatabase.GetProductByCode(pm);

            if (Convert.ToInt32(productIB.Text) <= 0)
            {
                txtIB.IsEnabled = false;
                productIB.IsEnabled = false;
            }
        }

        private async void BtnAddToCart_Clicked(object sender, EventArgs e)
        {
            if (true)
            {
                bool confirm = await DisplayAlert("Order Confirmation", "IB  : " + txtIB.Text + " \r\nPC : " + txtPC.Text + " \r\nCS : " + txtCS.Text + " \r\n", "Confirm", "Cancel");
                if (confirm)
                {
                    //string curDocm = DateTime.Now.Day + App.CurrentUserCode + App.CurrentCustomerCode + App.CurrentTransaction;
                    //var q = await App.RPIOrderingMainDatabase.GetOrderByDocnum(curDocm);

                    //if (q == null)
                    //{
                    //OrderMain orderMain = new OrderMain()
                    //{
                    //    //docNum = curDocm,
                    //    custCode = App.CurrentCustomerCode,
                    //    userCode = App.CurrentUserCode,
                    //    poNumber = DateTime.Now.Day + App.CurrentCustomerCode,
                    //    tDate = DateTime.Now
                    //};

                    //await App.RPIOrderingMainDatabase.SaveOrderMain(orderMain);
                    //}


                    CartService cart = new CartService()
                    {
                        //docNum = curDocm,

                        productCode = productCode.Text,
                        IB = Convert.ToInt32(txtIB.Text),
                        PC = Convert.ToInt32(txtPC.Text),
                        CS = Convert.ToInt32(txtCS.Text),
                        productName = productName.Text,
                        productImage = productImage.Source.ToString()

                    };

                    if (isEdit == true)
                    {
                        cart.productRowCart = productRowCart;
                        await DisplayAlert("Operation", "Order Updated", "Got It!");
                    }

                    await App.K5OrderingDatabase.AddtoCart(cart);
                    await DisplayAlert("Success", "Order added to Cart", "Got It!");
                    await Navigation.PopAsync();

                }
            }
           
        }

    }
}