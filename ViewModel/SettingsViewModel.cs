using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoProject.Classes;
using ToDoProject.Constant;
using ToDoProject.Services;

namespace ToDoProject.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<StorageType> _storageTypes;

        public ObservableCollection<StorageType> StorageTypes
        {
            get { return _storageTypes; }
            set { _storageTypes = value; OnPropertyChanged(nameof(StorageTypes)); }
        }

        private StorageType _selectedType;

        public StorageType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value; 
                OnPropertyChanged(nameof(SelectedType));
            }
        }


        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; OnPropertyChanged(nameof(SelectedIndex)); }
        }


        public SettingsViewModel()
        {
            StorageTypes = new ObservableCollection<StorageType>
            {
                new StorageType() { Name = "Local", Type = DatabaseType.sql},
                new StorageType() {Name = "Remote", Type = DatabaseType.api}
            };
            if (DataSwitcher.dataType == DatabaseType.sql) SelectedIndex = 0;
            else SelectedIndex = 1;
            TypeSelectedCommand = new RelayCommand(TypeSelected);
        }

        public ICommand TypeSelectedCommand { get; set; }
        private async void SelectType()
        {
            if (SelectedType.Type == DatabaseType.api) {
                var api = new LaravelApiService();
                if(!(await api.IsServerConnected()))
                {
                    MessageBox.Show("Remote Server is not connected.", "Error");
                    return;
                }
            }
            MainViewModel.Instance.Logout();
            if(SelectedType.Type == DatabaseType.sql) DataSwitcher.SwitchToSql(SelectedType);
            else DataSwitcher.SwitchToApi(SelectedType);
        }

        public void TypeSelected()
        {
            SelectType();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
