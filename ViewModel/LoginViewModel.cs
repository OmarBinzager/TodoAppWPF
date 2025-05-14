using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.View;

namespace ToDoProject.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private bool _rememberMe;

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { _rememberMe = value; OnPropertyChanged(nameof(RememberMe)); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateEmail();
            }
        }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidatePassword();
            }
        }


        public bool Validation()
        {
            return false;
        }

        private void ValidateEmail()
        {
            ClearErrors(nameof(Email));

            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddError(nameof(Email), "Invalid email format.");
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));

            if (string.IsNullOrWhiteSpace(Password))
                AddError(nameof(Password), "Password is required.");
            else if (Password.Length < 6)
                AddError(nameof(Password), "Password must be at least 8 characters.");
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            GoToRegisterPageCommand = new RelayCommand(GoToRegisterPage);
        }

        public ICommand LoginCommand { get; set; }
        public ICommand GoToRegisterPageCommand { get; set; }

        public void Login()
        {
            IsLoading = true;
            ValidateEmail();
            ValidatePassword();

            if (!HasErrors) {
                Authenticate();
            }
            IsLoading = false;
        }

        public async System.Threading.Tasks.Task Authenticate(){
            var service = DataServiceFactory.GetService();
            User user = await service.Authenticate(Email, Password);
            if (user != null)
            {
                SessionService.Instance.SetUser(user);
                if (RememberMe)
                    AuthStorage.SaveUser(user); // Save for next launch

                NavigateToMainApp();
            } else
            {
                MessageBox.Show("Email or Password is wrong please try again.", "Wrong Email or Password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToMainApp()
        {
            var mainWindow = new MainWindow(); // Replace with your MainWindow
            mainWindow.Show();
            var loginWindow = Application.Current.Windows.OfType<AuthView>().FirstOrDefault();
            loginWindow?.Close(); // Close login window if it's open
            AuthViewModel.Instance.Close();
            // Now, show the main window
        }
        //private void OnLoginSuccess(User user)
        //{
        //    // Save user info for next time
        //    AuthStorage.SaveUser(user);

        //    // Set the current user in session
        //    SessionService.Instance.SetUser(user);

        //    // Now, show the main window
        //    Application.Current.MainWindow.Close();  // Close the login window
        //    var mainWindow = new MainWindow();      // Or MainWindowView
        //    mainWindow.Show();                      // Show the main window
        //}

        public bool CanLogin() => !HasErrors;
        public void GoToRegisterPage()
        {
            var authView = AuthViewModel.Instance;
            authView.NavigateToRegister();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        
        #region INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null && _errors.ContainsKey(propertyName)
                ? _errors[propertyName]
                : null;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
