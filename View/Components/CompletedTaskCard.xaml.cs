using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.View.Dialogs;
using ToDoProject.ViewModel;

namespace ToDoProject.View.Components
{
    /// <summary>
    /// Interaction logic for TaskCard.xaml
    /// </summary>
    public partial class CompletedTaskCard : UserControl
    {
        public CompletedTaskCard()
        {
            InitializeComponent();
        }

        public IEnumerable<Status> statuses { get; set; }

        public Task Task
        {
            get { return (Task)GetValue(TaskProperty); }
            set
            {
                SetValue(TaskProperty, value);

            }
        }

        private void setCompletedBefore(DateTime completeTime)
        {
            TimeSpan time = DateTime.Now - completeTime;
            if (time.Days > 0)
            {
                completedBefore.Text = $"Completed {time.Days} days ago.";
                return;
            }
            if (time.Hours > 0)
            {
                completedBefore.Text = $"Completed {time.Hours} hours ago.";
                return;
            }
            if (time.Minutes > 0)
            {
                completedBefore.Text = $"Completed {time.Minutes} mintues ago.";
                return;
            }
            completedBefore.Text = $"Completed just now.";
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register(nameof(Task), typeof(Task), typeof(CompletedTaskCard), new PropertyMetadata(null));

        private static void OnCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CompletedTaskCard control)
            {
                control.DataContext = e.NewValue;
            }
        }

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(CompletedTaskCard), new PropertyMetadata(false));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.ContextMenu != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }



        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(CompletedTaskCard));


        private bool isCurrentStatus(int statusIndex)
        {
            return ((Task)DataContext).Status == statuses.ToList()[statusIndex];
        }

        private bool isNotCurrentStatus(int statusIndex)
        {
            return !isCurrentStatus(statusIndex);
        }

        private void MenuItem_Click_In_Progress(object sender, RoutedEventArgs e)
        {

            if (isNotCurrentStatus(1))
            {
                ((Task)DataContext).Status = statuses.ToList()[1];
                setStatusToTask(2);
                MainViewModel.Instance.OnTasksChanged();
                DeleteCommand?.Execute(null);
            }
        }


        private async System.Threading.Tasks.Task setStatusToTask(int statusId)
        {
            var service = DataServiceFactory.GetService();
            var data = new Dictionary<string, object>() { { "status", statusId } };
            var param = new Dictionary<string, object>() { { "id", Task.Id } };
            await service.UpdateFeildAtTable("Task", data, "id = @id", param);
        }

        private void MenuItem_Click_Not_Started(object sender, RoutedEventArgs e)
        {
            if (isNotCurrentStatus(0))
            {
                ((Task)DataContext).Status = statuses.ToList()[0];
                setStatusToTask(1);
                MainViewModel.Instance.OnTasksChanged();
                DeleteCommand?.Execute(null);
            }
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.OpenDialog(new TaskDialog(Task));
        }

        private async void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            var service = DataServiceFactory.GetService();
            await service.DeleteStepsAsync((DataContext as Task).Id);
            await service.DeleteTaskAsync((DataContext as Task).Id);
            MainViewModel.Instance.OnTasksChanged();
            DeleteCommand?.Execute(null);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var service = DataServiceFactory.GetService();
            statuses = await service.GetStatusesAsync();
            if (DataContext is Task)
                setCompletedBefore((DataContext as Task).CompletedAt);
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CompletedTaskCard), new PropertyMetadata(null));


        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Command != null)
            {
                Command?.Execute(Task);
            }
        }



        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
