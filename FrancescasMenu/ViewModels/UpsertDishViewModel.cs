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
    [QueryProperty(nameof(DishId), nameof(DishId))]
    public class UpsertDishViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }

        public UpsertDishViewModel()
        {
            SaveCommand = new Command(executeSave);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command(executeDelete);
        }

        private string dishId;
        public string DishId
        {
            get
            {
                return dishId;
            }
            set
            {
                dishId = value;
                LoadDishId(value);
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public async void LoadDishId(string entreeId)
        {
            try
            {
                if (DishId == "0")
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

                        var objId = MongoDB.Bson.ObjectId.Parse(entreeId);
                        var selected_entree = PhoneContext.Find<Dish>(objId);

                        Name = selected_entree.Name;
                    }
                    catch (Exception X0)
                    {
                        Debug.WriteLine($"-->  LoadDishId Exception: {X0.Message}");
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("--> Failed to Load Dish");
            }
        }

        private async void executeSave()
        {
            try
            {
                var DeviceUser = await App.RealmApp.LogInAsync(Credentials.ApiKey(App.adminAKey));
                var RealmConfiguration = new SyncConfiguration(DeviceUser.Id, DeviceUser);
                var PhoneContext = await Realm.GetInstanceAsync(RealmConfiguration);

                Dish upsertDish = new Dish();
                if (DishId != "0")
                {
                    // Updating
                    upsertDish.Id = MongoDB.Bson.ObjectId.Parse(DishId);
                }
                upsertDish.Partition = DeviceUser.Id;
                upsertDish.Name = Name;
                //upsertDish.Course = "";
                //upsertDish.Description = "";

                PhoneContext.Write(() =>
                {
                    // Add
                    //PhoneContext.Add(addDish);
                    
                    // Upsert
                    PhoneContext.Add(upsertDish, update: true);
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
                    var objId = MongoDB.Bson.ObjectId.Parse(DishId);
                    var selected_dish = PhoneContext.Find<Dish>(objId);


                    // Remove the instance from the realm.
                    PhoneContext.Remove(selected_dish);

                    // Discard the reference.
                    selected_dish = null;
                });
            }
            catch (Exception X0)
            {
                Debug.WriteLine($"-->  executeDelete Exception: {X0.Message}");
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