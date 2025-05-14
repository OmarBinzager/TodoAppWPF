using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.Services;
using ToDoProject.View.Dialogs;

namespace ToDoProject.ViewModel
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {

        ObservableCollection<Priority> priorities;
        ObservableCollection<Category> categories;

        public ObservableCollection<Priority> Priorities { 
            get { return priorities; }
            set { priorities = value; OnPropertyChanged(nameof(Priorities)); }
        }
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        public CategoriesViewModel()
        {
            loadData();
            DeleteCommand = new RelayCommand<Category>(DeleteCategory);
            EditCommand = new RelayCommand<Category>(EditCategory);
            AddCategoryCommand = new RelayCommand(AddCategory);
            AddPriorityCommand = new RelayCommand(AddPriority);

            var mainViewModel = MainViewModel.Instance;
            mainViewModel.CategoriesChanged += MainViewModel_CategoriesChanged;
            mainViewModel.PrioritiesChanged += MainViewModel_CategoriesChanged;

        }

        private void MainViewModel_CategoriesChanged(object sender, EventArgs e)
        {
            loadData();
        }

        public async void loadData()
        {
            var service = DataServiceFactory.GetService();
            Priorities = await service.GetPrioritiesAsync();
            Categories = await service.GetCategoriesAsync();
        }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand AddPriorityCommand { get; set; }


        public void EditCategory(Category category)
        {
            if (category is Priority)
            {
                MainViewModel.Instance.OpenDialog(new PriorityDialog(category as Priority));
            }
            else
            {
                MainViewModel.Instance.OpenDialog(new CategoryDialog(category));
            }
        }
        public void AddCategory()
        {
            MainViewModel.Instance.OpenDialog(new CategoryDialog());
        }
        public void AddPriority()
        {
            MainViewModel.Instance.OpenDialog(new PriorityDialog());
        }
        public async void DeleteCategory(Category category)
        {
            var service = DataServiceFactory.GetService();
            if (category is Priority)
            {
                await service.DeletePriorityAsync((category as Priority).Id);
                Priorities = await service.GetPrioritiesAsync();
            }
            else
            {
                await service.DeleteCategoryAsync(category.Id);
                Categories = await service.GetCategoriesAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
