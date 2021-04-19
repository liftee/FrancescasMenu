using FrancescasMenu.Views;
using Realms;
using Realms.Sync;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrancescasMenu
{
    public partial class App : Application
    {
        public static Realms.Sync.App RealmApp;
        private const string appId = "";
        public static string adminAKey = "";
      

        public App()
        {
            InitializeComponent();

            // Register Services
            //
            //DependencyService.Register<MockDataStore>();
            
            // Create the RealmApp
            //
            RealmApp = Realms.Sync.App.Create(appId);

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
