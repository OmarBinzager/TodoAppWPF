﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using ToDoProject.Services;
using ToDoProject.Classes;
using System.Collections.ObjectModel;
using ToDoProject.Model;
using System.Windows.Input;
using ToDoProject.View.Dialogs;
using CommunityToolkit.Mvvm.Input;
using ToDoProject.View.Components;
using System.Windows.Controls;

namespace ToDoProject.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {


        private string percentageText1;  

        public string PercentageText1
        {
            get { return percentageText1; }
            set { percentageText1 = value; OnPropertyChanged(nameof(PercentageText1)); }
        }

        private string percentageText2;

        public string PercentageText2
        {
            get { return percentageText2; }
            set { percentageText2 = value; OnPropertyChanged(nameof(PercentageText2)); }
        }

        private string percentageText3;

        public string PercentageText3
        {
            get { return percentageText3; }
            set { percentageText3 = value; OnPropertyChanged(nameof(PercentageText3)); }
        }

        private bool _LatestIsLoading;

        public bool LatestIsLoading
        {
            get { return _LatestIsLoading; }
            set { _LatestIsLoading = value; OnPropertyChanged(nameof(LatestIsLoading)); }
        }

        private bool _CompleteTasksIsLoading;

        public bool CompleteTasksIsLoading
        {
            get { return _CompleteTasksIsLoading; }
            set { _CompleteTasksIsLoading = value; OnPropertyChanged(nameof(CompleteTasksIsLoading)); }
        }

        private string _noCompletedTasksMessage;

        public string NoCompletedTasksMessage
        {
            get { return _noCompletedTasksMessage; }
            set { _noCompletedTasksMessage = value; OnPropertyChanged(nameof(NoCompletedTasksMessage)); }
        }
        private string _noLatestTasksMessage;

        public string NoLatestTasksMessage
        {
            get { return _noLatestTasksMessage; }
            set { _noLatestTasksMessage = value; OnPropertyChanged(nameof(NoLatestTasksMessage)); }
        }


        public string Username { get; set; }
        public string WelcomingParag { get; set; }
        private Path path1;

        public Path Path1
        {
            get { return path1; }
            set { path1 = value; 
            }
        }


        private Path path2;

        public Path Path2
        {
            get { return path2; }
            set { path2 = value; 
            }
        }

        private Path path3;

        public Path Path3
        {
            get { return path3; }
            set { path3 = value;
                
            }
        }

        private ObservableCollection<Status> statuses;

        public ObservableCollection<Status> Statuses
        {
            get { return statuses; }
            set { statuses = value; OnPropertyChanged(nameof(Statuses)); }
        }

        private ObservableCollection<Model.Task> completedTasks;

        public ObservableCollection<Model.Task> CompletedTasks
        {
            get { return completedTasks; }
            set { completedTasks = value; OnPropertyChanged(nameof(CompletedTasks)); }
        }

        private ObservableCollection<TaskGroup> collectedTasks;

        public ObservableCollection<TaskGroup> CollectedTasks
        {
            get { return collectedTasks; }
            set { collectedTasks = value; OnPropertyChanged(nameof(CollectedTasks)); }
        }

        private Model.Task selectedTask;

        public Model.Task SelectedTask
        {
            get { return selectedTask; }
            set { selectedTask = value; OnPropertyChanged(nameof(SelectedTask)); }
        }



        public DashboardViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);
            UpdateUICommand = new RelayCommand(UpdateDashboard);
            TaskSelectedCommand = new RelayCommand<Model.Task>(TaskSelected);
            Username = SessionService.Instance.CurrentUser.FirstName;
            WelcomingParag = $"Welcom back, {Username} 👋";
            UpdateDashboard();
        }

        private void DashboardTasksChanged(object sender, EventArgs e)
        {
            UpdateDashboard();
        }

        public ICommand AddTaskCommand { get; set; }
        public ICommand UpdateUICommand { get; set; }
        public ICommand TaskSelectedCommand { get; set; }

        public void AddTask()
        {
            MainViewModel.Instance.TaskAdded += (s, e) => UpdateDashboard();
            MainViewModel.Instance.OpenDialog(new TaskDialog());
        }
        public void UpdateDashboard()
        {
            InitializeProperties();
        }

        public void TaskSelected(Model.Task task)
        {
            MainViewModel.Instance.OpenPage(new TaskViewerPage() { DataContext = task });
        }

        public async System.Threading.Tasks.Task InitializeProperties()
        {
            var service = DataServiceFactory.GetService();
            Dictionary<string, object> stats = await service.GetDashboardStatsAsync();

            Statuses = await service.GetStatusesAsync();
            double percentage1 = 0;
            double percentage2 = 0;
            double percentage3 = 0;
            if (stats.Count > 0)
            {
                percentage1 = double.Parse(stats["percentage1"].ToString());
                percentage2 = double.Parse(stats["percentage2"].ToString());
                percentage3 = double.Parse(stats["percentage3"].ToString());
                setPercentageProgresses(percentage1, percentage2, percentage3);
                if ((int)stats["tasksCount"] != 0)
                {
                    GetCompletedTasks(service);
                    GetRecentTasks(service);
                }
                else
                {
                    NoCompletedTasksMessage = "There aren't any completed tasks yet.";
                    NoLatestTasksMessage = "There aren't any tasks yet, add new task by + button above.";
                }
            }else
            {
                NoCompletedTasksMessage = "There aren't any completed tasks yet.";
                NoLatestTasksMessage = "There aren't any tasks yet, add new task by + button above.";
            }
        }

        private async System.Threading.Tasks.Task GetRecentTasks(IDataService service)
        {
            LatestIsLoading = true;
            CollectedTasks = GroupTasksByDate(await service.GetRecentTasks());
            LatestIsLoading = false;
            if (CollectedTasks.Count < 1)
            {
                NoLatestTasksMessage = "There aren't any tasks yet, add new task by + button above.";
            }
            else
            {
                NoLatestTasksMessage = "";
            }
        }

        private async System.Threading.Tasks.Task GetCompletedTasks(IDataService service)
        {
            CompleteTasksIsLoading = true;
            CompletedTasks = await service.GetCompletedTasks();
            CompleteTasksIsLoading = false;
            if (CompletedTasks.Count < 1)
            {
                NoCompletedTasksMessage = "There aren't any completed tasks yet.";
            }
            else
            {
                NoCompletedTasksMessage = "";
            }
        }

        public ObservableCollection<TaskGroup> GroupTasksByDate(ObservableCollection<Model.Task> tasks)
        {
            return new ObservableCollection<TaskGroup>(tasks
                .OrderByDescending(t => t.CreatedAt) // ترتيب حسب التاريخ
                .GroupBy(t => t.CreatedAt.Date) // تجميع حسب التاريخ فقط (بدون الوقت)
                .Select(g => {
                    var date = g.Key;
                    var dayLabel = date == DateTime.Today ? "Today" :
                                   date == DateTime.Today.AddDays(-1) ? "Yesterday" :
                                   date.ToString("dddd");

                    var dateString = date.ToString("dd MMMM yyyy  ● ") + dayLabel;

                    return new TaskGroup
                    {
                        DateString = dateString,
                        Tasks = new ObservableCollection<Model.Task>(g.ToList())
                    };
                })
                .ToList());
        }

        private void setPercentageProgresses(double percentage1, double percentage2, double percentage3)
        {
            Path1.Data = GetProgress(percentage1);
            PercentageText1 = $"{percentage1}%";
            Path2.Data = GetProgress(percentage2);
            PercentageText2 = $"{percentage2}%";
            Path3.Data = GetProgress(percentage3);
            PercentageText3 = $"{percentage3}%";
        }

        private PathGeometry GetProgress(double percentage)
        {
            double radius = 65; // Half of 150
            double centerX = 75;
            double centerY = 75;
            if (percentage == 100) percentage--;
            double angle = 360 * (percentage / 100);

            var pathFigure = new PathFigure
            {
                StartPoint = new Point(centerX, centerY - radius) // Start at top center
            };

            var arcSegment = new ArcSegment
            {
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = angle > 180,
                Point = ComputeCartesianCoordinate(angle, radius, centerX, centerY)
            };

            var pathGeometry = new PathGeometry();
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private Point ComputeCartesianCoordinate(double angle, double radius, double centerX, double centerY)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);
            double x = centerX + radius * Math.Cos(angleRad);
            double y = centerY + radius * Math.Sin(angleRad);
            return new Point(x, y);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
