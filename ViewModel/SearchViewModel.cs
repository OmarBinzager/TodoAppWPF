using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.Utils;
using ToDoProject.View.Components;
using ToDoProject.View.Dialogs;

namespace ToDoProject.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged
    {

        private string _noTasksMessage;

        public string NoTasksMessage
        {
            get { return _noTasksMessage; }
            set { _noTasksMessage = value; OnPropertyChanged(nameof(NoTasksMessage)); }
        }

        private ObservableCollection<Model.Task> allTasks;
        private ObservableCollection<Model.Task> tasks;
        public ObservableCollection<Model.Task> Tasks {
            get {
                if(tasks != null) tasks = new ObservableCollection<Model.Task>(tasks.OrderByDescending((task) => task.CreatedAt));
                return tasks; }
            set { tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        private Model.Task _selectedTask;

        public Model.Task SelectedTask
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

        public SearchViewModel()
        {
            mainViewModel = MainViewModel.Instance;
            mainViewModel.TasksChanged += MainViewModel_TasksChanged;
            SearchTextUpdatedAction += SearchTextUpdated;
            TaskSelectedCommand = new RelayCommand(TaskSelected);
        }

        //public async void loadData()
        //{
        //    await loadTasks();

        //}

        public Func<string, System.Threading.Tasks.Task> SearchTextUpdatedAction { get; set; }
        public string SearchedText { get; set; }

        public async System.Threading.Tasks.Task SearchTextUpdated(string searchText)
        {
            await Search(searchText);
        }

        private async System.Threading.Tasks.Task Search(string searchText)
        {
            IsLoading = true;
            var service = DataServiceFactory.GetService();
            allTasks = await service.SearchAsync(searchText);
            Tasks = allTasks;
            if(Tasks.Count < 1)
            {
                NoTasksMessage = "No tasks found.";
            }
            else
            {
                NoTasksMessage = "";
            }
            IsLoading = false;
        }

        private void MainViewModel_TasksChanged(object sender, EventArgs e)
        {
            //Search();
        }

        public ICommand TaskSelectedCommand { get; set; }


        public void TaskSelected()
        {
            //MessageBox.Show(SelectedTask.Status.Color);
            if(SelectedTask != null){ MainViewModel.Instance.OpenPage(new TaskViewerPage() { DataContext = SelectedTask }); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
