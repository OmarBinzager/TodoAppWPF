using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SharpVectors.Dom.Svg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;

namespace ToDoProject.ViewModel
{
    public class TaskDialogViewModel : INotifyPropertyChanged
    {


        private ObservableCollection<Priority> priorities;
        public ObservableCollection<Priority> Priorities
        {
            get { return priorities; }
            set {  priorities = value; OnPropertyChanged(nameof(Priorities)); }
        }
        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; OnPropertyChanged(nameof(Categories)); }
        }
        public Task Task { get; set; }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }


        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(nameof(Description)); }
        }

        private Status status;
        public Status Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }


        private Priority priority;
        public Priority Priority
        {
            get { return priority; }
            set { priority = value; OnPropertyChanged(nameof(Priority)); }
        }


        private Category category;
        public Category Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(nameof(Category)); }
        }

        private DateTime createdAt;
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }

        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        private string picture;
        public string Picture
        {
            get { return picture; }
            set {
                if (value is null) return;
                if (value.EndsWith("jpg") || value.EndsWith("png") || value.EndsWith("jpeg") || value.EndsWith("webp"))
                {
                    picture = value;
                    OnPropertyChanged(nameof(Picture)); 
                }
            }
        }


        private ObservableCollection<Step> steps;
        public ObservableCollection<Step> Steps
        {
            get { return steps; }
            set { steps = value; OnPropertyChanged(nameof(Steps)); }
        }


        private string _dialogTitle;

        public string DialogTitle
        {
            get { return _dialogTitle; }
            set { _dialogTitle = value; OnPropertyChanged(nameof(DialogTitle)); }
        }

        public bool IsImageSelected
        {
            get { return !string.IsNullOrEmpty(Picture); }
            set { OnPropertyChanged(nameof(IsImageSelected)); }
        }


        public TaskDialogViewModel()
        {
            Task = new Task();
            Task.Status = new Status { Id = 1 };
            DialogTitle = "Add Task";
            InitializeComponents();
        }
        public TaskDialogViewModel(Task task)
        {
            Task = task;
            InitializeComponents();
            DialogTitle = "Edit Task";
        }

        public async void InitializeComponents()
        {
            Title = Task.Title;
            Description = Task.Description;
            DueDate = Task.DueDate;
            Picture = Task.Picture;
            CreatedAt = Task.CreatedAt;
            if (Task.Steps != null)
            {
                if ( Task.Steps.Count > 0){
                    Steps = Task.Steps;
                    Steps.Add(new Step() { Description = "", Index = Steps.Count + 1 });
                }
                else
                    Steps = new ObservableCollection<Step> { new Step() { Description = "", Index = 1 } };
            }
            else
            {
                Steps = new ObservableCollection<Step> { new Step() { Description = "", Index = 1 } };
            }
            this.Priority = Task.Priority;
            this.Category = Task.Category;
            this.Status = Task.Status;

            CloseDialogCommand = new RelayCommand(CloseDialog);
            StepTextChangedCommand = new RelayCommand<Step>(StepTextChanged);  
            ChooseImageCommand = new RelayCommand(ChooseImage);
            ImageDropedCommand = new RelayCommand<DragEventArgs>(ImageDroped);
            SaveCommand = new RelayCommand(Save);

            var service = DataServiceFactory.GetService();
            Priorities = await service.GetPrioritiesAsync();
            Categories = await service.GetCategoriesAsync();
        }


        public ICommand CloseDialogCommand { get; set; }
        public ICommand StepTextChangedCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public ICommand ImageDropedCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public void ImageDroped(DragEventArgs arg)
        {
            if (arg is null) return;
            if (arg.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var droppedFile = ((string[])arg.Data.GetData(DataFormats.FileDrop))[0];
                Picture = droppedFile;
                OnPropertyChanged(nameof(IsImageSelected));
            }
        }
        public async void Save()
        {
            if (Validate())
            {
                if (DialogTitle == "Edit Task")
                {
                    await EditTask();
                    return;
                }
                await AddTask();
            }
        }

        private void InitializeValuesToTask()
        {
            Task.Title = Title;
            Task.Description = Description;
            Task.Picture = Picture;
            Task.Status = Status;
            Task.Priority = Priority;
            Task.Category = Category;
            Task.Steps = Steps;
            Task.DueDate = DueDate;
            Task.CreatedAt = CreatedAt;
        }

        private bool IsValuesEdited()
        {
            return (Task.Title != Title ||
            Task.Description != Description ||
            Task.Picture != Picture ||
            Task.Status != Status ||
            Task.Priority != Priority ||
            Task.Category != Category ||
            Task.Steps != Steps ||
            Task.DueDate != DueDate ||
            Task.CreatedAt != CreatedAt);
        }

        private async System.Threading.Tasks.Task AddTask()
        {
            InitializeValuesToTask();
            var service = DataServiceFactory.GetService();
            bool result = await service.AddTaskAsync(Task);
            if (result)
            {
                await service.GetTaskIdAsync(Task);
                await service.AddStepsAsync(Task.Steps, Task.Id);
                backToMainWindow();
                return;
            }
            showErrorMessage();
        }

        private static void showErrorMessage()
        {
            MessageBox.Show("Something went wrong!.\nTry again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async System.Threading.Tasks.Task EditTask()
        {
            if (IsValuesEdited()) {
                InitializeValuesToTask();
                var service = DataServiceFactory.GetService();
                var result = await service.UpdateTaskAsync(Task);
                if (result)
                {
                    backToMainWindow();
                    return;
                }
                MessageBox.Show("Something went wrong!.\nTry again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void backToMainWindow()
        {
            var mainwindow = MainViewModel.Instance;
            mainwindow.OnTasksChanged();
            mainwindow.OnTasksAdded();
            mainwindow.CloseDialog();
        }

        private bool Validate()
        {
            if(
                string.IsNullOrEmpty(Title) ||
                string.IsNullOrEmpty(Description)
               )
            {
                MessageBox.Show("Title & Description are required.", "Fields Are Empty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            ClearEmptySteps();
            return true;

        }
        public void ClearEmptySteps()
        {
            var emptySteps = Steps.Where(step => string.IsNullOrEmpty(step.Description)).ToList();
            if(emptySteps.Count >= 1)
                Steps.RemoveAt(Steps.IndexOf(emptySteps[0]));
        }

        public void CloseDialog()
        {
            MainViewModel.Instance.CloseDialog();
        }

        public void StepTextChanged(Step step)
        {

            if (step == null) return;

            // Remove empty steps (except the first step)
            if (string.IsNullOrWhiteSpace(step.Description) && Steps.Count > 1)
            {
                Steps.Remove(step);
                RenumberSteps();
                return;
            }

            // Ensure last step is always available
            if (Steps.Last() == step && !string.IsNullOrWhiteSpace(step.Description))
            {
                Steps.Add(new Step { Index = Steps.Count + 1, Description = "" });
            }

            OnPropertyChanged(nameof(Steps));
        }

        // Renumber steps after deletion
        private void RenumberSteps()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                Steps[i].Index = i + 1;
            }
        }

        public void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Title = "Choose an image";
            openFileDialog.Multiselect = false;
            openFileDialog.FileOk += OnImageSelected;
            openFileDialog.ShowDialog();
        }

        private void OnImageSelected(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog = sender as OpenFileDialog;
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Picture = openFileDialog.FileName;
                OnPropertyChanged(nameof(IsImageSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
