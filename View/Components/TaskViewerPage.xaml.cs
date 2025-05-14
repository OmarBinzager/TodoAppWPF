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
    /// Interaction logic for TaskViewer.xaml
    /// </summary>
    public partial class TaskViewerPage : Page
    {
        public TaskViewerPage()
        {
            InitializeComponent();
        }

        private async void DeleteTask(object sender, RoutedEventArgs e)
        {
            var service = DataServiceFactory.GetService();
            await service.DeleteStepsAsync((DataContext as Task).Id);
            await service.DeleteTaskAsync((DataContext as Task).Id);
            MainViewModel.Instance.OnTasksChanged();
            DeleteCommand?.Execute(null);
            MainViewModel.Instance.GoBack();
        }

        private void EditTask(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.OpenDialog(new TaskDialog(DataContext as Task));
        }



        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(TaskViewerPage));




        //public Task Task
        //{
        //    get { return (Task)GetValue(TaskProperty); }
        //    set { SetValue(TaskProperty, value); MessageBox.Show(value.ToString()); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TaskProperty =
        //    DependencyProperty.Register(nameof(Task), typeof(Task), typeof(TaskViewer), new PropertyMetadata(null, OnTaskChanged));

        //private static void OnTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as TaskViewer;
        //    if (control != null)
        //    {
        //        control.DataContext = control.Task; // Bind UI to new Task
        //    }// Update DataContext when Task changes
        //    MessageBox.Show("Changed");
        //}
    }
}
