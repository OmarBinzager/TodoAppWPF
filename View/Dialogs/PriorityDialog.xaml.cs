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
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.ViewModel;

namespace ToDoProject.View.Dialogs
{
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class PriorityDialog : UserControl
    {
        public PriorityDialog()
        {
            DataContext = new PriorityDialogViewModel();
            InitializeComponent();
        }

        public PriorityDialog(Priority priority)
        {
            DataContext = new PriorityDialogViewModel(priority);
            InitializeComponent();
        }
    }
}
