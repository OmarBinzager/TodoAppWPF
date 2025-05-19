using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ToDoProject.Classes;
using ToDoProject.Constant;
using ToDoProject.Model;
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
            if (!DatabaseExists())
            {
                CreateDatabase();
                DataSwitcher.SwitchToSql(new StorageType() { Name = "Local", Type = DatabaseType.sql });
                ShowLoginWindow();
            }
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
            //addnewTask();
        }

        //private async void addnewTask()
        //{
        //    LaravelApiService api = new LaravelApiService();
        //    Model.Task task = new Model.Task
        //    {
        //        Id = 6,
        //        Title = "This is an updated task",
        //        Description = "updated task task",
        //        Picture = "",
        //        Status = new Classes.Status { Id = 1 },
        //        CreatedAt = DateTime.Now,
        //    };
        //    Category category = new Category() { Id = 23,Name = "Category" }; 
        //    Priority priority = new Priority() { Id = 22, Name = "Priority", Color = "Blue" };
        //    //var result = await api.DeleteCategoryAsync(23);
        //    //var res = await api.DeletePriorityAsync(22);
        //    //Console.WriteLine(result);
        //    var steps = new Collection<Step>() {
        //        new Step(){ Description = "step 1", Index = 1}, 
        //        new Step(){ Description = "step 2", Index = 2}, 
        //        new Step(){ Description = "step 3", Index = 3},
        //    };
        //    var res = await api.DeleteStepsAsync(6);
        //    //await api.DeleteTaskAsync(6);
        //    Console.WriteLine(res);
        //}

        bool DatabaseExists()
        {
            using (var connection = new SqlConnection(SqlLink.masterConnString))
            {
                connection.Open();
                var command = new SqlCommand(
                    $"SELECT database_id FROM sys.databases WHERE Name = @databaseName", connection);
                command.Parameters.AddWithValue("@databaseName", SqlLink.dbName);
                return command.ExecuteScalar() != null;
            }
        }

        void CreateDatabase()
        {
            string script = File.ReadAllText("../../todo.sql");
            script = script.Replace("TodoDB2", SqlLink.dbName);
            string[] commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (SqlConnection connection = new SqlConnection(SqlLink.masterConnString))
            {
                connection.Open();
                foreach (string commandText in commands)
                {
                    if (string.IsNullOrWhiteSpace(commandText))
                        continue;

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
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
