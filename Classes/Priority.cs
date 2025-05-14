using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Model;

namespace ToDoProject.Classes
{
    public class Priority : Category
    {
        private string color;
        public string Color { get { return color; } set { color = value; OnPropertyChanged(nameof(Color)); } }
    }
}
