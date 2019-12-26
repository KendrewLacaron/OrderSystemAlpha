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
	public partial class ViewTransacSignPage : ContentPage
	{
        public string PONum;
        public ViewTransacSignPage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var e = await App.K5OrderingDatabase.GetSignatureByPO(PONum);
            BindingContext = e;
        }
    }
}