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
    public partial class TransactionLogPage : TabbedPage
    {
        public TransactionLogPage ()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            App.K5OrderingDatabase.LoadStatus = "Loading Transactions";
            await PopupNavigation.Instance.PushAsync(new LoadingPopup(), true);
            // Reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtEmpId = -1;
            var e = await App.K5OrderingDatabase.GetAllMainTransaction(0);
            var d = await App.K5OrderingDatabase.GetAllMainTransaction(1);
            mainTransacList.ItemsSource = e;
            cancelTransacList.ItemsSource = d;
            await PopupNavigation.Instance.PopAsync(true);
        }

        //async void OnUserAdded(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new RegisterPageUser
        //    {
        //        BindingContext = new User()
        //    });
        //}

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
            //Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new Cart
                {
                    BindingContext = e.SelectedItem as Customer
                });
            }
        }

        private async void MainTransacList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TransactionDetailView
                {
                    pm = (e.SelectedItem as GlobalReport).docNum
                });
            }
        }

        private void CancelTransacList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var u = await App.K5OrderingDatabase.GetAllMainTransaction(0);
                mainTransacList.ItemsSource = u;
            }

            else
            {
                List<GlobalReport> ls;
                ls = await App.K5OrderingDatabase.GetAllMainTransaction(0);
                var cs = ls.Where(x => x.customerName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
                mainTransacList.ItemsSource = cs;
            }
        }

        private async void SearchBar_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var u = await App.K5OrderingDatabase.GetAllMainTransaction(1);
                cancelTransacList.ItemsSource = u;
            }

            else
            {
                List<GlobalReport> ls;
                ls = await App.K5OrderingDatabase.GetAllMainTransaction(1);
                var cs = ls.Where(x => x.customerName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase));
                cancelTransacList.ItemsSource = cs;
            }
        }

    }
}