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
    public class DishCollectionViewModel : BaseViewModel
    {
        public Command NavigateToAddDishCommand { get; }
        public Command LoadDishesCommand { get; }
        public Command<Dish> ItemTapped { get; }

        private IEnumerable<Dish> _dishCollection;
        public IEnumerable<Dish> DishCollection
        {
            get { return _dishCollection; }
            set { SetProperty(ref _dishCollection, value); }
        }

        public DishCollectionViewModel()
        {
            Title = "Dishes";

            NavigateToAddDishCommand = new Command(NavigateToAddDish);
            LoadDishesCommand = new Command(async () => await ExecuteLoadDishesCommand());
            ItemTapped = new Command<Dish>(OnItemSelected);
        }

        async Task ExecuteLoadDishesCommand()
        {
            IsBusy = true;

            try
            {
                var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                DishCollection = PhoneContext.All<Dish>();
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

        private async void NavigateToAddDish(object obj)
        {
            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync(nameof(UpsertDishViewModel));
            await Shell.Current.GoToAsync($"{nameof(UpsertDishPage)}?{nameof(UpsertDishViewModel.DishId)}=0");
        }

        async void OnItemSelected(Dish dish)
        {
            if (dish == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            // throws exception if the objectId is passed
            await Shell.Current.GoToAsync($"{nameof(UpsertDishPage)}?{nameof(UpsertDishViewModel.DishId)}={dish.Id.ToString()}");
        }
    }
}