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
using System.Runtime.Serialization.Json;
using ToDoProject.Services;

namespace ToDoProject.ViewModel
{
    public class CategoryDialogViewModel : INotifyPropertyChanged
    {

        public Category Category { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private bool _priorityIsLoading;

        public bool PriorityIsLoading
        {
            get { return _priorityIsLoading; }
            set { _priorityIsLoading = value; OnPropertyChanged(nameof(PriorityIsLoading)); }
        }

        private bool _categoryIsLoading;

        public bool CategoryIsLoading
        {
            get { return _categoryIsLoading; }
            set { _categoryIsLoading = value; OnPropertyChanged(nameof(CategoryIsLoading)); }
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

        public CategoryDialogViewModel()
        {
            Category = new Category();
            DialogTitle = "Add Task Category";
            ActionButtonName = "Create";
            InitializeComponents();
        }
        public CategoryDialogViewModel(Category category)
        {
            InitializeComponents();
            Category = category;
            Name = category.Name;
            DialogTitle = "Edit Task Category";
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
            Category.Name = Name;
            if (Validate())
            {
                if (ActionButtonName == "Save")
                    await EditCategory();
                else
                    await AddCategory();
                backToMainWindow();
            }
        }

        private async System.Threading.Tasks.Task AddCategory()
        {
            var service = DataServiceFactory.GetService();
            await service.AddCategoryAsync(Category);
            //await service.GetCategoryIdAsync(Category);
        }

        private void backToMainWindow()
        {
            var mainwindow = MainViewModel.Instance;
            mainwindow.CloseDialog();
            mainwindow.OnCategoriesChanged();
        }

        public async System.Threading.Tasks.Task EditCategory()
        {
            var service = DataServiceFactory.GetService();
            bool reusult = await service.UpdateCategoryAsync(Category);
        }

        private bool Validate()
        {
            if (
                string.IsNullOrEmpty(Name)
               )
            {
                MessageBox.Show("Category name are required.", "Fields Are Empty", MessageBoxButton.OK, MessageBoxImage.Warning);
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
