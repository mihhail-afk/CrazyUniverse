using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Core.Models;
using Core.Repositories;
using WPF.Commands;

namespace WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Repository<Penguin> _penguinRepo = new();
        private readonly Repository<Monkey> _monkeyRepo = new();
        private readonly Repository<SuperLion> _lionRepo = new();
        private readonly Repository<Dragon> _dragonRepo = new();
        private readonly Repository<Phoenix> _phoenixRepo = new();
        private readonly Repository<Yeti> _yetiRepo = new();

        private BaseAnimal? _selectedAnimal;
        private string _searchText = string.Empty;

        public ObservableCollection<BaseAnimal> CurrentAnimals { get; } = new();
        public ObservableCollection<string> LogEntries { get; } = new();

        public BaseAnimal? SelectedAnimal
        {
            get => _selectedAnimal;
            set
            {
                _selectedAnimal = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredAnimals));
            }
        }

        public ObservableCollection<BaseAnimal> FilteredAnimals
        {
            get
            {
                var filtered = CurrentAnimals.AsEnumerable();
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filtered = filtered.Where(a => a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                return new ObservableCollection<BaseAnimal>(filtered);
            }
        }

        public ICommand SortCommand { get; }
        public ICommand GroupCommand { get; }

        public ICommand AddPenguinCommand { get; }
        public ICommand AddMonkeyCommand { get; }
        public ICommand AddLionCommand { get; }
        public ICommand AddDragonCommand { get; }
        public ICommand AddPhoenixCommand { get; }
        public ICommand AddYetiCommand { get; }

        public ICommand RemoveCommand { get; }
        public ICommand CrazyActionCommand { get; }

        public MainViewModel()
        {
            SetupEvents(_penguinRepo);
            SetupEvents(_monkeyRepo);
            SetupEvents(_lionRepo);
            SetupEvents(_dragonRepo);
            SetupEvents(_phoenixRepo);
            SetupEvents(_yetiRepo);

            AddPenguinCommand = new RelayCommand(_ => _penguinRepo.Add(new Penguin("Pingu", 100)));
            AddMonkeyCommand = new RelayCommand(_ => _monkeyRepo.Add(new Monkey("Abu", 100)));
            AddLionCommand = new RelayCommand(_ => _lionRepo.Add(new SuperLion("Simba", 100)));
            AddDragonCommand = new RelayCommand(_ => _dragonRepo.Add(new Dragon("Smaug")));
            AddPhoenixCommand = new RelayCommand(_ => _phoenixRepo.Add(new Phoenix("Fawkes")));
            AddYetiCommand = new RelayCommand(_ => _yetiRepo.Add(new Yeti("Abominable")));

            RemoveCommand = new RelayCommand(_ =>
            {
                if (SelectedAnimal is Penguin p) _penguinRepo.Remove(p);
                else if (SelectedAnimal is Monkey m) _monkeyRepo.Remove(m);
                else if (SelectedAnimal is SuperLion l) _lionRepo.Remove(l);
                else if (SelectedAnimal is Dragon d) _dragonRepo.Remove(d);
                else if (SelectedAnimal is Phoenix ph) _phoenixRepo.Remove(ph);
                else if (SelectedAnimal is Yeti y) _yetiRepo.Remove(y);

                RefreshCurrentList();
            }, _ => SelectedAnimal != null);

            CrazyActionCommand = new RelayCommand(_ =>
            {
                if (SelectedAnimal is Penguin p) _penguinRepo.PerformCrazyAction(p);
                else if (SelectedAnimal is Monkey m) _monkeyRepo.PerformCrazyAction(m);
                else if (SelectedAnimal is SuperLion l) _lionRepo.PerformCrazyAction(l);
                else if (SelectedAnimal is Dragon d) _dragonRepo.PerformCrazyAction(d);
                else if (SelectedAnimal is Phoenix ph) _phoenixRepo.PerformCrazyAction(ph);
                else if (SelectedAnimal is Yeti y) _yetiRepo.PerformCrazyAction(y);

                RefreshCurrentList();
            }, _ => SelectedAnimal != null);

            SortCommand = new RelayCommand(_ =>
            {
                var sorted = FilteredAnimals.OrderByDescending(a => a.Energy).ToList();
                CurrentAnimals.Clear();
                foreach (var item in sorted)
                {
                    CurrentAnimals.Add(item);
                }
                OnPropertyChanged(nameof(FilteredAnimals));
            });

            GroupCommand = new RelayCommand(_ =>
            {
                var groups = CurrentAnimals.GroupBy(a => a.GetType().Name);
                AddLog("--- Grouping by types ---");
                foreach (var group in groups)
                {
                    AddLog($"Type: {group.Key}, Count: {group.Count()}");
                }
            });

            _penguinRepo.Add(new Penguin("Kowalski", 90));
            _penguinRepo.Add(new Penguin("Rico", 85));
            _dragonRepo.Add(new Dragon("Smaug"));
            _phoenixRepo.Add(new Phoenix("Fawkes"));
            _yetiRepo.Add(new Yeti("Abominable"));

            RefreshCurrentList();
        }

        private void SetupEvents<T>(Repository<T> repo) where T : BaseAnimal
        {
            repo.OnItemAdded += (newItem, neighbor) =>
            {
                AddLog($"{neighbor.Name} welcomes newcomer {newItem.Name}!");
            };

            repo.OnItemRemoved += (removedItem, neighbor) =>
            {
                AddLog($"{neighbor.Name} says goodbye to departed {removedItem.Name}.");
            };

            repo.OnCrazyActionTriggered += (source, neighbor) =>
            {
                neighbor.Energy -= 10;
                AddLog($"{neighbor.Name} reacted to CrazyAction from {source.Name}! Energy became {neighbor.Energy}.");
                RefreshCurrentList();
            };
        }

        private void RefreshCurrentList()
        {
            CurrentAnimals.Clear();
            foreach (var item in _penguinRepo.GetAll()) CurrentAnimals.Add(item);
            foreach (var item in _monkeyRepo.GetAll()) CurrentAnimals.Add(item);
            foreach (var item in _lionRepo.GetAll()) CurrentAnimals.Add(item);
            foreach (var item in _dragonRepo.GetAll()) CurrentAnimals.Add(item);
            foreach (var item in _phoenixRepo.GetAll()) CurrentAnimals.Add(item);
            foreach (var item in _yetiRepo.GetAll()) CurrentAnimals.Add(item);

            OnPropertyChanged(nameof(FilteredAnimals));
        }

        private void AddLog(string message)
        {
            LogEntries.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}