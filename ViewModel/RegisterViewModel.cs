using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ToDoProject.Model;
using ToDoProject.Services;

namespace ToDoProject.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        private string _avatar;

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(nameof(SelectedImage)); }
        }

        public ImageSource SelectedImage
        {
            get
            {
                var imgUri = new Uri(string.IsNullOrEmpty(Avatar) ? "pack://application:,,,/Assets/default-user.jpg" : Avatar);
                return new BitmapImage(imgUri);
            }
        }
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                ValidateFirstName();
            }
        }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                ValidateLastName();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateEmail();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidatePassword();
                ValidateConfirmPassword(); // revalidate confirm password
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                ValidateConfirmPassword();
            }
        }

        #region Validation Methods

        private void ValidateFirstName()
        {
            ClearErrors(nameof(FirstName));
            if (string.IsNullOrWhiteSpace(FirstName))
                AddError(nameof(FirstName), "First name is required.");
        }

        private void ValidateLastName()
        {
            ClearErrors(nameof(LastName));
            if (string.IsNullOrWhiteSpace(LastName))
                AddError(nameof(LastName), "Last name is required.");
        }

        private void ValidateEmail()
        {
            ClearErrors(nameof(Email));
            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required.");
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
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

        private void ValidateConfirmPassword()
        {
            ClearErrors(nameof(ConfirmPassword));
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
                AddError(nameof(ConfirmPassword), "Please confirm your password.");
            else if (ConfirmPassword != Password)
                AddError(nameof(ConfirmPassword), "Passwords do not match.");
        }

        #endregion

        #region INotifyDataErrorInfo

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register, CanRegister);
            GoToLoginPageCommand = new RelayCommand(GoToLoginPage);
            SelectImageCommand = new RelayCommand(SelectImage);
        }

        public ICommand RegisterCommand { get; set; }
        public ICommand GoToLoginPageCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }


        public void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Title = "Choose an image";
            openFileDialog.Multiselect = false;
            openFileDialog.FileOk += OnImageSelected;
            openFileDialog.ShowDialog();
        }

        private void OnImageSelected(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog = sender as OpenFileDialog;
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Avatar = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        public void Register()
        {
            IsLoading = true;
            ValidateFirstName();
            ValidateLastName();
            ValidatePassword();
            ValidateConfirmPassword();
            ValidateEmail();

            if (HasErrors) { IsLoading = false; return; }
            RegisterUser();
        }

        public async System.Threading.Tasks.Task RegisterUser()
        {
            var service = DataServiceFactory.GetService();
            User user = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Avatar = Avatar,
            };
            IsLoading = true;
            bool result = await service.RegisterUser(user, Password, ConfirmPassword);
            IsLoading = false;
            if (result)
            {

                NavigateToLoginPage();
            }
        }
        public void NavigateToLoginPage()
        {
            AuthViewModel.Instance.NavigateToLogin();
        }
        public bool CanRegister() => !HasErrors;
        public void GoToLoginPage()
        {
            var authView = AuthViewModel.Instance;
            authView.NavigateToLogin();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
