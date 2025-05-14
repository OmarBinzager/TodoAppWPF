using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Classes;
using ToDoProject.ViewModel;

namespace ToDoProject.Model
{
    public class Task : INotifyPropertyChanged
    {

        public Task()
        {
            DueDate = DateTime.Now;
            CreatedAt = DateTime.Now;
            CompletedAt = DateTime.Now;
        }

        private int _id;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _description;
        public string Description {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private string _picture;
        public string Picture { 
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(nameof(Picture)); }
        }

        private ObservableCollection<Step> _steps;
        public ObservableCollection<Step> Steps { 
            get { return _steps; } 
            set { _steps = value; OnPropertyChanged(nameof(Steps)); } 
        }

        private Priority _priority;
        public Priority Priority { get { return _priority; } set { _priority = value; OnPropertyChanged(nameof(Priority)); } }

        private Status _status;
        public Status Status { get { return _status; } set { _status = value; OnPropertyChanged(nameof(Status)); } }

        private Category _category;
        public Category Category { get { return _category; } set { _category = value; OnPropertyChanged(nameof(Category)); } }

        private DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } set { _createdAt = value; OnPropertyChanged(nameof(CreatedAt)); } }
        private DateTime _CompletedAt;

        public DateTime CompletedAt
        {
            get { return _CompletedAt; }
            set { _CompletedAt = value; OnPropertyChanged(nameof(CompletedAt)); }
        }


        private DateTime _dueDate;
        public DateTime DueDate { get { return _dueDate; } set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
