using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToDoProject.Services;
using ToDoProject.View;

namespace ToDoProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var user = AuthStorage.LoadUser();
            if (user != null)
            {
                SessionService.Instance.SetUser(user);
                ShowMainWindow();
            }
            else
            {
                ShowLoginWindow();
            }
        }

        // Show the login window (initial screen before login)
        private void ShowLoginWindow()
        {
            var loginWindow = new AuthView(); // Replace with your LoginWindow
            loginWindow.Show();
        }

        // Show the main window after login
        private void ShowMainWindow()
        {
            var mainWindow = new MainWindow(); // Replace with your MainWindow
            mainWindow.Show();
        }
    }
}
