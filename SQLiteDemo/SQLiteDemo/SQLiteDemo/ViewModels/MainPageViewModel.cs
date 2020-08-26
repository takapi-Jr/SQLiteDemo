using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SQLiteDemo.Data;
using SQLiteDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
using System.Reactive.Disposables;
using System.Text;

namespace SQLiteDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        public ReactiveProperty<ObservableCollection<Person>> Persons { get; } = new ReactiveProperty<ObservableCollection<Person>>();
        public ReactiveProperty<Person> SelectedPerson { get; } = new ReactiveProperty<Person>();
        public ReactiveProperty<string> EntryName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> EntryAddress { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand PageAppearingCommand { get; } = new AsyncReactiveCommand();
        //public AsyncReactiveCommand PageDisappearingCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SaveCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand DeleteCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            this.PageAppearingCommand.Subscribe(async () =>
            {
                var list = await App.Database.GetPersonsAsync();
                Persons.Value = new ObservableCollection<Person>(list);
                SelectedPerson.Value = null;
            }).AddTo(this.Disposable);

            this.SaveCommand.Subscribe(async () =>
            {
                var person = new Person 
                {
                    Id = (SelectedPerson.Value != null) ? SelectedPerson.Value.Id : 0,
                    Name = EntryName.Value,
                    Address = EntryAddress.Value,
                    UpdateDate = DateTime.Now,
                };
                var ret = await App.Database.SavePersonAsync(person);
                if (ret > 0)
                {
                    var list = await App.Database.GetPersonsAsync();
                    Persons.Value = new ObservableCollection<Person>(list);
                }
            }).AddTo(this.Disposable);

            this.DeleteCommand.Subscribe(async () =>
            {
                if (SelectedPerson.Value == null)
                {
                    return;
                }
                var ret = await App.Database.DeletePersonAsync(SelectedPerson.Value);
                if (ret > 0)
                {
                    Persons.Value.Remove(SelectedPerson.Value);
                    SelectedPerson.Value = null;
                }
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
