using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K5OrderBeta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderCheckoutPage : ContentPage
	{
		public OrderCheckoutPage ()
		{
			InitializeComponent ();
		}

        private async void BtnFinalCheckOut_Clicked(object sender, EventArgs e)
        {
            bool he = await DisplayAlert("Baka...", "Confirm Checkout", "Hai", "Nai");

            //if(he && txtPONumber.Text != null)
            //{
            //await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);

            //    byte[] a;
            //    var img = signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            //    var SignatureMemoryStream = (MemoryStream)img;

            //    byte[] sigdata = SignatureMemoryStream.ToArray();

            //    // write the signature to the orderheader.
            //    using (BinaryReader binaryreader = new BinaryReader(img))
            //    {
            //        binaryreader.BaseStream.Position = 0;
            //        a = binaryreader.ReadBytes((int)img.Length);
            //    }

            //   if (await App.K5OrderingDatabase.ProccessAllTransacAsync(txtPONumber.Text,a))
            //{
            //    await App.K5OrderingDatabase.SaveLogPref(6, App.K5OrderingDatabase.CurrentCustomerCode);
            //    await PopupNavigation.Instance.PopAsync(true);
            //    await App.K5OrderingDatabase.ResetTransaction();
            //    await DisplayAlert("Checkout", "Order Successfully Completed", "Nice");
            //    await Navigation.PopToRootAsync();
            //}
            //    else 
            //    {
            //    await PopupNavigation.Instance.PopAsync(true);
            //    await DisplayAlert("Checkout", "Transaction Saving Failed", "Nice");

            //}

            //    } else
            //{
            //    await DisplayAlert("Checkout", "Note: PONumber must not be null ", "Sure");
            //}



        }
    }
}