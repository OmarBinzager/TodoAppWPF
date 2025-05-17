using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.View;

namespace ToDoProject.ViewModel
{
    public class ResetPasswordViewModel : INotifyPropertyChanged
    {


        private string _useremail;

        public string UserEmail
        {
            get { return _useremail; }
            set { _useremail = value; OnPropertyChanged(nameof(UserEmail)); }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }


        private string _avatar;

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(nameof(SelectedImage)); }
        }

        private string _currentPassword;

        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { _currentPassword = value; OnPropertyChanged(nameof(CurrentPassword)); }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }
        private string _confirmNewPassword;

        public string ConfirmNewPassword
        {
            get { return _confirmNewPassword; }
            set { _confirmNewPassword = value; OnPropertyChanged(nameof(ConfirmNewPassword));}
        }


        public ImageSource SelectedImage
        {
            get
            {
                var imgUri = new Uri(string.IsNullOrEmpty(Avatar) ? "pack://application:,,,/Assets/default-user.jpg" : Avatar);
                return new BitmapImage(imgUri);
            }
        }


        public ResetPasswordViewModel()
        {
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(SaveChanges);
            var user = SessionService.Instance.CurrentUser;
            if (user != null) { 
                UserEmail = user.Email;
                Username = $"{user.FirstName} {user.LastName}";
                Avatar = user.Avatar;
            }
        }


        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public void SaveChanges()
        {
            if (isValidatedFields())
            {
                SaveAsync();
            }
        }

        public async void SaveAsync()
        {
            var service = DataServiceFactory.GetService();
            var result = await service.ResetPassword(CurrentPassword, NewPassword, ConfirmNewPassword);
            if (result)
            {
                MessageBox.Show("Password changed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                MainViewModel.Instance.NavigateToPage<UserProfile>();
            }
            else
            {
                MessageBox.Show("Current Password doesn't correct.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            MainViewModel.Instance.GoBack();
        }

        private bool isValidatedFields()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword)) { 
                MessageBox.Show( "Current password is required.","Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                MessageBox.Show("New Password is required.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (NewPassword.Length < 8)
            {
                MessageBox.Show("New Password must be at least 8 chars.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                MessageBox.Show("Confirm password is required.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (ConfirmNewPassword != NewPassword)
            {
                MessageBox.Show("The password confirmation does not much the new password.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        public void reload()
        {
            var user = SessionService.Instance.CurrentUser;
            if (user != null)
            {
                UserEmail = user.Email;
                Username = $"{user.FirstName} {user.LastName}";
                Avatar = user.Avatar;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
