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
using ToDoProject.Constant;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.View;

namespace ToDoProject.ViewModel
{
    public class UserProfileViewModel : INotifyPropertyChanged
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
            set { _avatar = value; OnPropertyChanged(nameof(SelectedImage)); setCanCacel(); }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); setCanCacel(); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); setCanCacel(); }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); setCanCacel(); }
        }

        public string SelectedImagebyUser { get; set; }

        public ImageSource SelectedImage
        {
            get
            {
                var imgUri = new Uri(string.IsNullOrEmpty(Avatar) ? "pack://application:,,,/Assets/default-user.jpg" : Avatar.Contains(ApiLink.storage) ? Avatar : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/" + Avatar));
                if (SelectedImagebyUser != null) imgUri = new Uri(SelectedImagebyUser);
                return new BitmapImage(imgUri);
            }
        }

        private bool _canCancel;

        public bool CanCancel
        {
            get { return _canCancel; }
            set { _canCancel = value; OnPropertyChanged(nameof(CanCancel)); }
        }

        public void setCanCacel()
        {
            if (IsValuesEdited())
            {
                CanCancel = true;
            }else
            {
                CanCancel = false;
            }
        }

        public UserProfileViewModel()
        {
            SelectImageCommand = new RelayCommand(SelectImage);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(SaveChanges);
            ResetPasswordCommand = new RelayCommand(GoToResetPasswordView);
            var user = SessionService.Instance.CurrentUser;
            if (user != null) { 
                UserEmail = user.Email;
                Username = $"{user.FirstName} {user.LastName}";
                Avatar = user.Avatar;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
            }
        }


        public ICommand SelectImageCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public void SaveChanges()
        {
            if (IsValuesEdited() && isValidatedFields())
            {
                SaveAsync();
            }
        }


        public async void SaveAsync()
        {
            User user = new User() { Id = SessionService.Instance.CurrentUser.Id, Avatar = Avatar, Email = Email, FirstName = FirstName, LastName = LastName};
            if (SelectedImagebyUser != null) user.Avatar = SelectedImagebyUser;
            var service = DataServiceFactory.GetService();
            var result = await service.UpdateUser(user);
            if (result)
            {
                MessageBox.Show("Information changed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                reload();
                MainViewModel.Instance.Avatar = string.IsNullOrEmpty(Avatar) ? 
                    "pack://application:,,,/Assets/default-user.jpg" : 
                    Avatar.Contains(ApiLink.storage) ? Avatar : 
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/" + Avatar);
                MainViewModel.Instance.Username = Username;
                MainViewModel.Instance.Email = UserEmail;
            }
            else
            {
                MessageBox.Show("Somthing went wrong.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GoToResetPasswordView()
        {
            MainViewModel.Instance.OpenPage(new ResetPasswordView());
            //MainViewModel.Instance.NavigateToPage<UserProfile>();

        }

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
                SelectedImagebyUser = openFileDialog.FileName;
                Avatar = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        public void Cancel()
        {
            reload();
            CanCancel = false;
        }
        private bool IsValuesEdited()
        {
            var user = SessionService.Instance.CurrentUser;
            return (
                user.FirstName != FirstName ||
                user.LastName != LastName ||
                user.Email != Email ||
                user.Avatar != Avatar
                );
        }

        private bool isValidatedFields()
        {
            if (!File.Exists(SelectedImagebyUser))
            {
                if (SelectedImagebyUser != null)
                {
                    MessageBox.Show("Invalid image path", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(FirstName)) { 
                MessageBox.Show( "First name is required.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Last name is required.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email is required.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.", "Not Valid", MessageBoxButton.OK, MessageBoxImage.Information);
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
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
            }
            SelectedImagebyUser = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
