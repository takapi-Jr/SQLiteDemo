using Prism;
using Prism.Ioc;
using SQLiteDemo.ViewModels;
using SQLiteDemo.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using SQLiteDemo.Data;
using System.IO;
using System;

namespace SQLiteDemo
{
    public partial class App
    {
        private static PersonDatabase _database;

        public static PersonDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    _database = new PersonDatabase(Path.Combine(dir, "Persons.db3"));
                }
                return _database;
            }
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
    }
}
