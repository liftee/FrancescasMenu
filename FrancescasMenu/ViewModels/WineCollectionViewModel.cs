using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Realms.Sync;
using Realms;
using FrancescasMenu.Model;
using FrancescasMenu.Views;

namespace FrancescasMenu.ViewModels
{
    public class WineCollectionViewModel : BaseViewModel
    {
        public Command NavigateToAddWineCommand { get; }
        public Command LoadWinesCommand { get; }
        public Command<Wine> ItemTapped { get; }

        private IEnumerable<Wine> _wineCollection;
        public IEnumerable<Wine> WineCollection
        {
            get { return _wineCollection; }
            set { SetProperty(ref _wineCollection, value); }
        }

        public WineCollectionViewModel()
        {
            Title = "Wine List";

            NavigateToAddWineCommand = new Command(NavigateToAddWine);
            LoadWinesCommand = new Command(async () => await ExecuteLoadWinesCommand());
            ItemTapped = new Command<Wine>(OnItemSelected);
        }

        async Task ExecuteLoadWinesCommand()
        {
            IsBusy = true;

            try
            {
                var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                WineCollection = PhoneContext.All<Wine>();
            }
            catch (Exception X0)
            {
                Debug.WriteLine(X0);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void NavigateToAddWine(object obj)
        {
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(UpsertWinePage)}?{nameof(UpsertWineViewModel.WineId)}=0");
        }

        async void OnItemSelected(Wine item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            // throws exception if the objectId is passed
            await Shell.Current.GoToAsync($"{nameof(UpsertWinePage)}?{nameof(UpsertWineViewModel.WineId)}={item.Id.ToString()}");
        }
    }
}
