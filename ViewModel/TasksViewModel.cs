using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.Utils;
using ToDoProject.View.Dialogs;

namespace ToDoProject.ViewModel
{
    public class TasksViewModel : INotifyPropertyChanged
    {

        private string _sideWidth;
        private const string anyString = "Any";

        private ObservableCollection<Status> statuses;
        private ObservableCollection<Priority> priorities;
        private ObservableCollection<Category> categories;

        private Status selectedStatus;
        private Priority selectedPriority;
        private Category selectedCategory;

        public Status SelectedStatus
        {
            get { return selectedStatus; }
            set { 
                selectedStatus = value;
                Tasks = new ObservableCollection<Task>(
                    allTasks.Where((task) => 
                        (task.Status.Name == value.Name || selectedStatus.Name == anyString) && 
                        (task.Category.Name == SelectedCategory.Name || selectedCategory.Name == anyString) && 
                        (task.Priority.Name == selectedPriority.Name || selectedPriority.Name == anyString)
                    ));
                OnPropertyChanged(nameof(SelectedStatus)); }
        }
        public Priority SelectedPriority
        {
            get { return selectedPriority; }
            set { selectedPriority = value;
                Tasks = new ObservableCollection<Task>(
                    allTasks.Where((task) =>
                        (task.Priority.Name == value.Name || selectedPriority.Name == anyString) &&
                        (task.Category.Name == SelectedCategory.Name || selectedCategory.Name == anyString) &&
                        (task.Status.Name == selectedStatus.Name || selectedStatus.Name == anyString)
                    ));
                OnPropertyChanged(nameof(SelectedPriority)); }
        }
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value;
                Tasks = new ObservableCollection<Task>(
                    allTasks.Where((task) =>
                        (task.Category.Name == value.Name || selectedCategory.Name == anyString) &&
                        (task.Status.Name == selectedStatus.Name || selectedStatus.Name == anyString) &&
                        (task.Priority.Name == selectedPriority.Name || selectedPriority.Name == anyString)
                    ));
                OnPropertyChanged(nameof(SelectedCategory)); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; OnPropertyChanged(nameof(Categories)); }
        }
        public ObservableCollection<Status> Statuses
        {
            get { return statuses; }
            set { statuses = value; OnPropertyChanged(nameof(Statuses)); }
        }
        public ObservableCollection<Priority> Priorities
        {
            get { return priorities; }
            set { priorities = value; OnPropertyChanged(nameof(Priorities)); }
        }

        private string _noTasksMessage;

        public string NoTasksMessage
        {
            get { return _noTasksMessage; }
            set { _noTasksMessage = value; OnPropertyChanged(nameof(NoTasksMessage)); }
        }

        public string SideWidth
        {
            get { return _sideWidth; }
            set { _sideWidth = value; OnPropertyChanged(nameof(SideWidth)); }
        }

        private ObservableCollection<Task> allTasks;
        private ObservableCollection<Task> tasks;
        public ObservableCollection<Task> Tasks {
            get {
                if(tasks != null) tasks = new ObservableCollection<Task>(tasks.OrderByDescending((task) => task.CreatedAt));
                return tasks; }
            set { tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        private Task _selectedTask;

        public Task SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; OnPropertyChanged(nameof(SelectedTask)); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public MainViewModel mainViewModel { get; set; }

        public TasksViewModel()
        {
            mainViewModel = MainViewModel.Instance;
            mainViewModel.TasksChanged += MainViewModel_TasksChanged;
            _sideWidth = "0";
            selectedStatus = new Status { Name = anyString };
            selectedCategory = new Category { Name = anyString };
            selectedPriority = new Priority { Name = anyString };

            priorities = new ObservableCollection<Priority>();
            categories = new ObservableCollection<Category>();
            statuses = new ObservableCollection<Status>();

            CloseSideBarCommand = new RelayCommand(CloseSideBar);
            TaskSelectedCommand = new RelayCommand(TaskSelected);
            OpenAddTaskDialogCommand = new RelayCommand(OpenAddTaskDialog);
            loadData();
        }

        public async void loadData()
        {
            await loadTasks();
            await loadFilters();

        }

        private async System.Threading.Tasks.Task loadFilters()
        {
            Priorities.Add(new Priority { Name = anyString });
            Categories.Add(new Category { Name = anyString });
            Statuses.Add(new Status { Name = anyString });

            var service = DataServiceFactory.GetService();
            Priorities.AddRange(await service.GetPrioritiesAsync());
            Statuses.AddRange(await service.GetStatusesAsync());
            Categories.AddRange(await service.GetCategoriesAsync());

            OnPropertyChanged(nameof(Statuses));
            OnPropertyChanged(nameof(Categories));
            OnPropertyChanged(nameof(Priorities));
        }

        private async System.Threading.Tasks.Task loadTasks()
        {
            IsLoading = true;
            var service = DataServiceFactory.GetService();
            allTasks = await service.GetTasksAsync();
            Tasks = allTasks;
            if(Tasks.Count < 1)
            {
                NoTasksMessage = "There aren't any tasks yet, add new task by + button above.";
            }
            else
            {
                NoTasksMessage = "";
            }
            IsLoading = false;
        }

        private void MainViewModel_TasksChanged(object sender, EventArgs e)
        {
            CloseSideBar();
            loadTasks();
        }

        //public ICommand OpenSideBarCommand { get; set; }
        public ICommand CloseSideBarCommand { get; set; }
        public ICommand TaskSelectedCommand { get; set; }
        public ICommand OpenAddTaskDialogCommand { get; set; }


        public void OpenSideBar()
        {
            SideWidth = "1.2*";
        }
        public void CloseSideBar()
        {
            SideWidth = "0";
            SelectedTask = null;
        }

        public void TaskSelected()
        {
            //MessageBox.Show(SelectedTask.Status.Color);
            if(SelectedTask != null){ OpenSideBar(); }
        }
        public void OpenAddTaskDialog()
        {
            MainViewModel.Instance.OpenDialog(new TaskDialog());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
