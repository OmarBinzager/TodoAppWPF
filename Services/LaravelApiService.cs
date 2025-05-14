using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Classes;
using ToDoProject.Model;

namespace ToDoProject.Services
{
    public class LaravelApiService : IDataService
    {

        #region Task
        public Task<bool> AddTaskAsync(Model.Task task)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteTaskAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> GetTaskIdAsync(Model.Task task)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateTaskAsync(Model.Task task)
        {
            throw new NotImplementedException();
        }
        public Task<ObservableCollection<Model.Task>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Category
        public Task<bool> AddCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCategoryIdAsync(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Priority
        public Task<bool> AddPriorityAsync(Priority priority)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeletePriorityAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ObservableCollection<Priority>> GetPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetPriorityIdAsync(Priority priority)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdatePriorityAsync(Priority priority)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Step
        public Task<bool> AddStepAsync(Step step, int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddStepsAsync(Collection<Step> steps, int taskId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteStepAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStepsAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStepAsync(Step step, int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStepsAsync(Collection<Step> steps, int taskId)
        {
            throw new NotImplementedException();
        }
        public Task<ObservableCollection<Step>> GetStepsAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        #endregion




        #region Authentication 
        public Task<User> Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RegisterUser(User user, string password)
        {
            throw new NotImplementedException();
        }
        #region





        #region Dashboard
        public Task<Dictionary<string, object>> GetDashboardDataAsync()
        {
            throw new NotImplementedException();
        }


        #endregion


        #region Status
        public Task<ObservableCollection<Status>> GetStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetStatusIdAsync(Status status)
        {
            throw new NotImplementedException();
        }
        #endregion



        public Task<bool> UpdateFeildAtTable(string table, Dictionary<string, object> data, string whereClause, Dictionary<string, object> whereParams)
        {
            throw new NotImplementedException();
        }



    }
}
