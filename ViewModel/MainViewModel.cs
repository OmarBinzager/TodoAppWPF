using CommunityToolkit.Mvvm.Input;
using Jamesnet.Design.Images;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ToDoProject.Classes;
using ToDoProject.Constant;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.Utils;
using ToDoProject.View;
using YamlDotNet.Core.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace ToDoProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public const string Dash = "Dash";
        public const string Board ="board";
        public const string To = "To";
        public const string Do = "-Do";

        private string currentDay { get; set; }
        private string currentDate { get; set; }
        private DispatcherTimer timer { get; set; }
        private bool backButton;

        public bool BackButton
        {
            get { return backButton; }
            set { backButton = value; OnPropertyChanged(nameof(BackButton)); }
        }


        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public SearchView SearchView { get; set; }
        private string _avatar;

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(nameof(Avatar)); }
        }

        private string _searchedText;

        public DispatcherTimer searchDispatcher;
        public string SearchedText
        {
            get { return _searchedText; }
            set { _searchedText = value;
                OnPropertyChanged(SearchedText); }
        }
        public Func<string, System.Threading.Tasks.Task> SearchTextUpdatedAction { get; set; }
        public void OpenSearchPage()
        {
            SearchView = SearchView ?? new SearchView();
            SearchTextUpdatedAction = (SearchView.DataContext as SearchViewModel).SearchTextUpdatedAction;
            frame.Navigate(SearchView);
            BackButton = true;
        }

        public string CurrentDay
        {
            get {
                currentDay = DateTime.Now.ToString("dddd");
                return currentDay; 
            }
            set { currentDay = value; OnPropertyChanged(nameof(CurrentDay)); }
        }
        public string CurrentDate
        {
            get {
                currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                return currentDate; 
            }
            set { currentDate = value; OnPropertyChanged(nameof(CurrentDate)); }
        }

        private string _blurContent;

        public string BlurContent
        {
            get { return _blurContent; }
            set { _blurContent = value; OnPropertyChanged(nameof(BlurContent)); }
        }


        public void StartTimer()
        {
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromHours(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentDate = DateTime.Now.ToString("dd/mm/yyyy");
            currentDay = DateTime.Now.ToString("dddd");
        }
        

        public Frame frame { get; set; }
        public ContentControl ContentControl { get; set; }


        string _headPart1;
        public string headPart1
        {
            get { return _headPart1; }
            set
            {
                _headPart1 = value;
                OnPropertyChanged(nameof(headPart1));
            }
        }
        string _headPart2;
        public string headPart2 { 
            get { return _headPart2; } 
            set {
                _headPart2 = value;
                OnPropertyChanged(nameof(headPart2));
            } }

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get {
                _instance = _instance == null ? new MainViewModel() : _instance;
                return _instance;
            }
        }


        public void Close()
        {
            _instance = null;
        }

        private MainViewModel() {
            NavSelectCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<string>(NavSelect);
            GoBackCommand = new RelayCommand(GoBack);
            LogoutCommand = new RelayCommand(Logout);
            SearchCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<string>(Search);
            BlurContent = "0";
            BackButton = false;
            //var uri = new Uri(string.IsNullOrEmpty(SessionService.Instance.CurrentUser.Avatar) ?"pack://application:,,,/Assets/default-user.jpg" : $"pack://application:,,,/Uploads/{SessionService.Instance.CurrentUser.Avatar}");
            //Avatar = new BitmapImage(uri);
            Avatar = string.IsNullOrEmpty(SessionService.Instance.CurrentUser.Avatar) ? "pack://application:,,,/Assets/default-user.jpg" : SessionService.Instance.CurrentUser.Avatar.Contains(ApiLink.storage) ? SessionService.Instance.CurrentUser.Avatar : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/" + SessionService.Instance.CurrentUser.Avatar); //: $" / Uploads/{SessionService.Instance.CurrentUser.Avatar}"; string.IsNullOrEmpty(SessionService.Instance.CurrentUser.Avatar) ?
            Username = $"{SessionService.Instance.CurrentUser.FirstName} {SessionService.Instance.CurrentUser.LastName}";
            Email = SessionService.Instance.CurrentUser.Email;
            NavigateToDashboard();
        }

        public ICommand GoBackCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public void Logout()
        {
            // Clear user session
            var service = DataServiceFactory.GetService();
            service.Logout();

            // Optionally, clear the user data from storage (e.g., file)
            AuthStorage.ClearUser();

            // Show the login window again
            var mainwindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            var loginWindow = new AuthView();
            loginWindow.Show();
            mainwindow.Close();
            this.Close();
        }

        public void Search(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                OpenSearchPage();
                SearchTextUpdatedAction?.Invoke(query);
            }
        }

        private Page _CurrentPage;

        public Page CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public ICommand NavSelectCommand { get; set; }

        public void NavSelect(string pageId)
        {
            int id = int.Parse(pageId);
            switch (id)
            {
                case 0:
                    NavigateToUserProfile();
                    break;
                case 1:
                    NavigateToDashboard();
                    break;
                case 2:
                    NavigateToTasks();
                    break;
                case 3:
                    NavigateToCategories();
                    break;
                case 4:
                    NavigateToSettings();
                    break;
                case 5:
                    NavigateToHelp();
                    break;
                default:
                    break;
            }
        }

        private void NavigateToHelp()
        {
            NavigateToPage<Help>();
            SetToDoHeader();
        }

        private void NavigateToUserProfile()
        {
            NavigateToPage<UserProfile>();
            SetToDoHeader();
        }

        private void NavigateToSettings()
        {
            NavigateToPage<Settings>();
            SetToDoHeader();
        }

        private void NavigateToCategories()
        {
            NavigateToPage<Categories>();
            SetToDoHeader();
        }

        private void NavigateToTasks()
        {
            NavigateToPage<Tasks>();
            SetToDoHeader();
        }

        private void NavigateToDashboard()
        {
            NavigateToPage<Dashboard>();
            SetDashboardHeader();
        }

        public void NavigateToPage<T>() where T : Page
        {
            SearchView = null;
            if (CurrentPage is T) return;
            CurrentPage = (T)Activator.CreateInstance(typeof(T));
        }

        public void OpenDialog(object dialog)
        {
            ContentControl.Content = dialog;
            BlurContent = "13";
        }

        public void OpenPage(Page page)
        {
            SearchView = null;
            frame.Navigate(page);
            BackButton = true;
        }


        public void GoBack()
        {
            frame.GoBack();
            SearchView = null;
            BackButton = false;
        }

        public void CloseDialog()
        {
            ContentControl.Content = null;
            BlurContent = "0";
        }

        private void SetToDoHeader()
        {
            headPart1 = To;
            headPart2 = Do;
        }
        private void SetDashboardHeader()
        {
            headPart1 = Dash;
            headPart2 = Board;
        }

        public event EventHandler TasksChanged;
        public void OnTasksChanged() => TasksChanged?.Invoke(this, new EventArgs());
        public event EventHandler TaskAdded;
        public void OnTasksAdded() => TaskAdded?.Invoke(this, new EventArgs());

        public event EventHandler PrioritiesChanged;
        public void OnPrioritiesChanged() => PrioritiesChanged?.Invoke(this, new EventArgs());

        public event EventHandler CategoriesChanged;
        public void OnCategoriesChanged() => CategoriesChanged?.Invoke(this, new EventArgs());
        public Icons icons { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
