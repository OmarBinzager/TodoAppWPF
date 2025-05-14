using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using ToDoProject.Classes;
using ToDoProject.Model;
using CommunityToolkit.Mvvm.Input;
using ToDoProject.Services;

namespace ToDoProject.ViewModel
{
    public class PriorityDialogViewModel : INotifyPropertyChanged
    {

        public Priority Priority { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string color;
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(nameof(Color)); }
        }

        private string _dialogTitle;

        public string DialogTitle
        {
            get { return _dialogTitle; }
            set { _dialogTitle = value; OnPropertyChanged(nameof(DialogTitle)); }
        }

        private string actionButtonName;

        public string ActionButtonName
        {
            get { return actionButtonName; }
            set { actionButtonName = value; OnPropertyChanged(nameof(ActionButtonName)); }
        }

        public PriorityDialogViewModel()
        {
            Priority = new Priority();
            DialogTitle = "Add Task Priority";
            ActionButtonName = "Create";
            InitializeComponents();
        }
        public PriorityDialogViewModel(Priority priority)
        {
            Priority = new Priority();
            InitializeComponents();
            Priority = priority;
            Name = priority.Name;
            Color = priority.Color;
            DialogTitle = "Edit Task Priority";
            ActionButtonName = "Save";
        }

        public void InitializeComponents()
        {
            SaveCommand = new RelayCommand(Save);
            CloseDialogCommand = new RelayCommand(Cancel);
        }


        public ICommand CloseDialogCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public async void Save()
        {
            Priority.Name = Name;
            Priority.Color = Color;
            if (Validate())
            {
                if (ActionButtonName == "Save")
                    await EditPriority();
                else
                    await AddPriority();
                backToMainWindow();
            }
        }

        private async System.Threading.Tasks.Task AddPriority()
        {
            var service = DataServiceFactory.GetService();
            await service.AddPriorityAsync(Priority);
            await service.GetPriorityIdAsync(Priority);

        }

        private static void backToMainWindow()
        {
            var mainwindow = MainViewModel.Instance;
            mainwindow.CloseDialog();
            mainwindow.OnPrioritiesChanged();
        }

        public async System.Threading.Tasks.Task EditPriority()
        {
            var service = DataServiceFactory.GetService();
            bool result = await service.UpdatePriorityAsync(Priority);
        }

        private bool Validate()
        {
            if (
                string.IsNullOrEmpty(Name)
               )
            {
                MessageBox.Show("Priority name are required.", "Fields Are Empty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;

        }


        public void Cancel()
        {
            MainViewModel.Instance.CloseDialog();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
