using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoProject.ViewModel;

namespace ToDoProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = MainViewModel.Instance;
            DataContext = vm;
            vm.frame = mainFrame;
            vm.ContentControl = dialogContent;
        }

    }
}
