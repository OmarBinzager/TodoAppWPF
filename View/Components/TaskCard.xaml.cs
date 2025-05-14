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
    public partial class TaskCard : UserControl
    {
        public TaskCard()
        {
            InitializeComponent();
        }

        public IEnumerable<Status> statuses { get; set; }

        public Task Task
        {
            get { return (Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register(nameof(Task), typeof(Task), typeof(TaskCard), new PropertyMetadata(null));

        private static void OnCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TaskCard control)
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
            DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(TaskCard), new PropertyMetadata(false));



        public EventHandler Click
        {
            get { return (EventHandler)GetValue(ClickProperty); }
            set { SetValue(ClickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Click.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.Register("Click", typeof(EventHandler), typeof(TaskCard), new PropertyMetadata(null));



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TaskCard), new PropertyMetadata(null));    





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
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(TaskCard));



        private  void MenuItem_Click_In_Progress(object sender, RoutedEventArgs e)
        {

            if (isNotCurrentStatus(1))
            {
                ((Task)DataContext).Status = statuses.ToList()[1];
                setStatusToTask(2);
            }
        }

        private bool isCurrentStatus(int statusIndex)
        {
            return ((Task)DataContext).Status == statuses.ToList()[statusIndex];
        }

        private bool isNotCurrentStatus(int statusIndex)
        {
            return !isCurrentStatus(statusIndex);
        }

        private async System.Threading.Tasks.Task setStatusToTask(int statusId)
        {
            var service = DataServiceFactory.GetService();
            var data = new Dictionary<string, object>() { { "status", statusId } };
            if(statusId == 3)
            {
                data.Add("completed_at", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            var param = new Dictionary<string, object>() { { "id", Task.Id } };
            await service.UpdateFeildAtTable("Task", data, "id = @id", param);
            DeleteCommand?.Execute(null);

        }

        private void MenuItem_Click_Not_Started(object sender, RoutedEventArgs e)
        {
            if (isNotCurrentStatus(0))
            {
                ((Task)DataContext).Status = statuses.ToList()[0];
                setStatusToTask(1);
            }
        }

        private void MenuItem_Click_Completed(object sender, RoutedEventArgs e)
        {
            if (isNotCurrentStatus(2))
            {
                ((Task)DataContext).Status = statuses.ToList()[2];
                setStatusToTask(3);
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
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
            {
                Click?.Invoke(this, new EventArgs());
            }
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
