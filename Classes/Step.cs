using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Classes
{
    public class Step : INotifyPropertyChanged
    {
		private string description;

		public string Description
		{
			get { return description; }
			set { description = value; OnPropertyChanged(nameof(Description)); }
		}

		private int index;

		public int Index
		{
			get { return index; }
			set { index = value; OnPropertyChanged(nameof(Index)); }
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static explicit operator Step(List<Step> v)
        {
            throw new NotImplementedException();
        }
    }
}
