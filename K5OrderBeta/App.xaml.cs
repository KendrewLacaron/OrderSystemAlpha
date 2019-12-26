using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using K5OrderBeta.Database;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace K5OrderBeta
{
    public partial class App : Application
    {


        static K5OrderingDatabase k5OrderingDatabase;

        public App()
        {
            InitializeComponent();

            var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            //DLToolkit.Forms.Controls.FlowListView.Init();
            MainPage = new NavigationPage(new LoginPage()); 
            //MainPage = new NavigationPage(new MainPage());
        }

        public static K5OrderingDatabase K5OrderingDatabase
        {
            get
            {
                if (k5OrderingDatabase == null)
                {

                    //var d = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Avatars");
                    //Directory.CreateDirectory(d);
                    k5OrderingDatabase = new K5OrderingDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RPIOrderingSQLite.db3"));
                    //EmployeeDatabase.AddUser();
                }
                return k5OrderingDatabase;
            }
        }

        public int ResumeAtEmpId { get; set; }



        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
