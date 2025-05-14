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

namespace ToDoProject.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = new ViewModel.DashboardViewModel { Path1 = progressPath1, Path2 = progressPath2, Path3 = progressPath3 };
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as DashboardViewModel).InitializeProperties();
        }
    }
}
