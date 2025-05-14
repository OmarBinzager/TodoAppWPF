using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ToDoProject.View;

namespace ToDoProject.ViewModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        public Frame frame { get; set; }


        private Page _CurrentPage;

        public Page CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        private static AuthViewModel _instance;

        public static AuthViewModel Instance
        {
            get
            {
                _instance = _instance == null ? new AuthViewModel() : _instance;
                return _instance;
            }
        }


        public AuthViewModel() {
            NavigateToLogin();
        }

        public void Close()
        {
            _instance = null;
        }

        public void NavigateToLogin()
        {
            NavigateToPage<LoginView>();
        }
        public void NavigateToRegister()
        {
            NavigateToPage<RegisterView>();
        }

        public void NavigateToPage<T>() where T : Page
        {
            if (CurrentPage is T) return;
            CurrentPage = (T)Activator.CreateInstance(typeof(T));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
