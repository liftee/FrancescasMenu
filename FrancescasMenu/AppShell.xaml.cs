using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FrancescasMenu.ViewModels;
using FrancescasMenu.Views;

namespace FrancescasMenu
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(UpsertDishPage), typeof(UpsertDishPage));
            Routing.RegisterRoute(nameof(UpsertWinePage), typeof(UpsertWinePage));
        }
    }
}