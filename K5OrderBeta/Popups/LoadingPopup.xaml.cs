using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K5OrderBeta.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPopup : PopupPage
	{
		public LoadingPopup ()
		{
			InitializeComponent ();
            txtLoadStatus.Text = App.K5OrderingDatabase.LoadStatus;
            txtAdditionalStatus.Text = App.K5OrderingDatabase.AdditionalLoadInfo;
		}

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

    }
}