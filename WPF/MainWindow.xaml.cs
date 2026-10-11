using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Interfaces;
using Core.Models;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private List<BaseAnimal> _animals = new List<BaseAnimal>();
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            _animals.Add(new Monkey("Abu", 80));
            _animals.Add(new Penguin("Kowalski", 90));
            _animals.Add(new SuperLion("Alex", 100));

            RefreshList();
        }

        private void RefreshList()
        {
            AnimalsListBox.ItemsSource = null;
            AnimalsListBox.ItemsSource = _animals;
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            int type = _random.Next(3);
            int energy = _random.Next(50, 101);

            BaseAnimal newAnimal = type switch
            {
                0 => new Monkey($"Monkey_{_random.Next(100, 999)}", energy),
                1 => new Penguin($"Penguin_{_random.Next(100, 999)}", energy),
                _ => new SuperLion($"Lion_{_random.Next(100, 999)}", energy)
            };

            _animals.Add(newAnimal);
            RefreshList();
        }

        private void AnimalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnimalsListBox.SelectedItem is BaseAnimal selected)
            {
                InfoTextBlock.Text = $"Name: {selected.Name}\n" +
                                     $"Type: {selected.GetType().Name}\n" +
                                     $"Energy: {selected.Energy}%\n" +
                                     $"ID: {selected.Id}";

                SoundButton.IsEnabled = true;
                ActionButton.IsEnabled = true;

                bool hasInterface = selected is ISwimmable || selected is IClimb;
                InterfaceButton.IsEnabled = hasInterface;

                if (selected is ISwimmable && selected is IClimb)
                    InterfaceButton.Content = "Interface Action (Swim & Climb)";
                else if (selected is ISwimmable)
                    InterfaceButton.Content = "Interface Action (Swim)";
                else if (selected is IClimb)
                    InterfaceButton.Content = "Interface Action (Climb)";
                else
                    InterfaceButton.Content = "Interface Action (None)";
            }
            else
            {
                SoundButton.IsEnabled = false;
                ActionButton.IsEnabled = false;
                InterfaceButton.IsEnabled = false;
                InfoTextBlock.Text = "Select an animal from the list...";
            }
        }

        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsListBox.SelectedItem is BaseAnimal selected)
            {
                MessageBox.Show(selected.MakeSound(), "Sound", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsListBox.SelectedItem is BaseAnimal selected)
            {
                MessageBox.Show(selected.CrazyAction(), "Crazy Action", MessageBoxButton.OK, MessageBoxImage.Warning);
                RefreshList();
            }
        }

        private void InterfaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsListBox.SelectedItem is BaseAnimal selected)
            {
                string result = "";

                if (selected is ISwimmable swimmable)
                {
                    result += swimmable.Swim() + "\n";
                }
                if (selected is IClimb climbarble)
                {
                    result += climbarble.Climb();
                }

                MessageBox.Show(result.Trim(), "Interface Action", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}