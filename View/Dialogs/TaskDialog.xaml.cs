using System;
using System.Collections.Generic;
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
using ToDoProject.Model;
using ToDoProject.View.Components;
using ToDoProject.ViewModel;

namespace ToDoProject.View.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskDialog.xaml
    /// </summary>
    public partial class TaskDialog : UserControl
    {
        public TaskDialog()
        {
            DataContext = new TaskDialogViewModel();
            InitializeComponent();
        }

        public TaskDialog(Task task)
        {
            DataContext = new TaskDialogViewModel(task);
            InitializeComponent();
        }
    }
}
