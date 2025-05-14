using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToDoProject.Model
{
    public class Category : INotifyPropertyChanged
    {
        private int id;
        public int Id { get { return id; } set { id = value; OnPropertyChanged(nameof(Id)); } }
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(nameof(Name)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
