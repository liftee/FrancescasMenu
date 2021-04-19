using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;
using Xamarin.Forms;
using Realms.Sync;
using Realms;
using MongoDB.Bson;
using FrancescasMenu.Model;

namespace FrancescasMenu.ViewModels
{
    [QueryProperty(nameof(WineId), nameof(WineId))]
    public class UpsertWineViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }

        public UpsertWineViewModel()
        {
            SaveCommand = new Command(executeSave);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command(executeDelete);
            //this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private string wineId;
        public string WineId
        {
            get
            {
                return wineId;
            }
            set
            {
                wineId = value;
                LoadWineId(value);
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public async void LoadWineId(string wineId)
        {
            if (WineId == "0")
            {
                // Adding
            }
            else
            {
                // Updating
                try
                {
                    var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                    var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                    var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                    var objId = MongoDB.Bson.ObjectId.Parse(wineId);
                    var selected_wine = PhoneContext.Find<Wine>(objId);

                    Name = selected_wine.Name;
                }
                catch (Exception X0)
                {
                    Debug.WriteLine($"-->  LoadWineId Exception: {X0.Message}");
                }
            }
        }

        private async void executeSave()
        {
            try
            {
                var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                Wine upsertWine = new Wine();
                if (WineId != "0")
                {
                    // Updating
                    upsertWine.Id = MongoDB.Bson.ObjectId.Parse(WineId);
                }
                upsertWine.Partition = DeviceUser.Id;
                upsertWine.Name = Name;
                //upsertWine.Course
                //upsertWine.Description

                PhoneContext.Write(() =>
                {
                    //PhoneContext.Add(addDish);
                    PhoneContext.Add(upsertWine, update: true);
                });
            }
            catch (Exception X0)
            {
                Debug.WriteLine($"-->  executeSave Exception: {X0.Message}");
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void executeDelete()
        {
            try
            {
                var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                PhoneContext.Write(() =>
                {
                    var objId = MongoDB.Bson.ObjectId.Parse(WineId);
                    var selected_wine = PhoneContext.Find<Wine>(objId);

                    // Remove the instance from the realm.
                    PhoneContext.Remove(selected_wine);

                    // Discard the reference.
                    selected_wine = null;
                });
            }
            catch (Exception X0)
            {
                Debug.WriteLine($"-->  executeSave Exception: {X0.Message}");
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
