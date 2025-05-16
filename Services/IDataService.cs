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
    public interface IDataService
    {
        Task<int> GetTaskIdAsync(Model.Task task);
        Task<int> GetCategoryIdAsync(Model.Category category);
        Task<int> GetPriorityIdAsync(Priority priority);
        Task<int> GetStatusIdAsync(Status status);
        Task<ObservableCollection<Model.Task>> GetTasksAsync();
        Task<ObservableCollection<Model.Task>> SearchAsync(string text);
        Task<ObservableCollection<Category>> GetCategoriesAsync();
        Task<ObservableCollection<Priority>> GetPrioritiesAsync();
        Task<Dictionary<string, object>> GetDashboardStatsAsync();
        Task<ObservableCollection<Model.Task>> GetCompletedTasks();
        Task<ObservableCollection<Model.Task>> GetRecentTasks();
        Task<ObservableCollection<Status>> GetStatusesAsync();
        Task<ObservableCollection<Step>> GetStepsAsync(int taskId);
        Task<bool> AddTaskAsync(Model.Task task);
        Task<bool> AddStepsAsync(Collection<Step> steps, int taskId);
        Task<bool> AddStepAsync(Step step, int taskId);
        //Task<bool> UpdateStepAsync(Step step, int taskId);
        //Task<bool> UpdateStepsAsync(Collection<Step> steps, int taskId);
        Task<bool> DeleteStepsAsync(int taskId);
        Task<bool> DeleteStepAsync(int id);
        Task<bool> UpdateTaskAsync(Model.Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> AddPriorityAsync(Priority priority);
        Task<bool> UpdatePriorityAsync(Priority priority);
        Task<bool> DeletePriorityAsync(int id);

        Task<bool> UpdateFeildAtTable(string table, Dictionary<string, object> data, string whereClause, Dictionary<string, object> whereParams);
        Task<User> Authenticate(string email, string password);
        Task<bool> RegisterUser(User user, string password, string passwordConfirm);
        Task<bool> Logout();

    }
}
