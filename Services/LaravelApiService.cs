using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoProject.Classes;
using ToDoProject.Model;
using Windows.Media.Protection.PlayReady;
using ToDoProject.Constant;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Data;
using System.Windows.Documents;
using System.IO;
using CommonWin32;

namespace ToDoProject.Services
{
    public class LaravelApiService : IDataService
    {

        private static readonly HttpClient client = new HttpClient();

        #region Task
        public async Task<bool> AddTaskAsync(Model.Task task)
        {
            try
            {
                var data = new Dictionary<string, string>()
                {
                    { "title", task.Title },
                    { "description", task.Description },
                    { "picture", task.Picture },
                    { "status", task.Status.Id.ToString() },
                    { "due_date", task.DueDate.ToString("yyyy-MM-dd") },
                    { "created_at", task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") },
                    { "user_id", SessionService.Instance.CurrentUser.Id.ToString() }
                };
                if (task.Category != null) data.Add("category", task.Category.Id.ToString());
                if (task.Priority != null) data.Add("priority", task.Priority.Id.ToString());

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addTask, data);

                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null) {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        task.Id = id;
                        return true;
                    }
                    else return false;
                }else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.deleteTask(id));
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<int> GetTaskIdAsync(Model.Task task)
        {
            try
            {
                var data = new Dictionary<string, string>()
                    {
                        { "title", task.Title },
                        { "description", task.Description },
                        { "due_date", task.DueDate.ToString("yyyy-MM-dd") },
                        { "user_id",  SessionService.Instance.CurrentUser.Id.ToString() }
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.getTaskId, data);
                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        return id;
                    }
                    else return -1;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<bool> UpdateTaskAsync(Model.Task task)
        {
            try
            {
                var data = new Dictionary<string, string>()
                {
                    { "title", task.Title },
                    { "description", task.Description },
                    { "picture", task.Picture },
                    { "status", task.Status.Id.ToString() },
                    { "due_date", task.DueDate.ToString("yyyy-MM-dd") },
                    { "created_at", task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") },
                };
                if (task.Category != null) data.Add("category", task.Category.Id.ToString());
                if (task.Priority != null) data.Add("priority", task.Priority.Id.ToString());

                string url = UrlBuilder.BuildQueryUrl(ApiLink.editTask(task.Id), data);

                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = 0;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ObservableCollection<Model.Task>> GetTasksAsync()
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.getTasks);
                List<Dictionary<string, object>> listDict = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
                if (listDict != null || listDict.Count > 0)
                {
                    return await StoreTaskObjects(listDict);
                }
                else return new ObservableCollection<Model.Task>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Model.Task>();
            }
}

        private async Task<ObservableCollection<Model.Task>> StoreTaskObjects(List<Dictionary<string, object>> listDict)
        {
            var list = new ObservableCollection<Model.Task>();
            foreach (Dictionary<string, object> row in listDict)
            {
                list.Add(new Model.Task()
                {
                    Id = int.Parse(row["id"].ToString()),
                    Title = row["title"].ToString() ?? "",
                    Description = row["description"].ToString() ?? "",
                    DueDate = DateTime.Parse(row["due_date"].ToString()),
                    Picture = (row["picture"] is null) ? "" : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/" + row["picture"].ToString()),
                    Category = (row["category"] is null) ? new Category() : new Category { Id = int.Parse(row["category"].ToString()), Name = row["category_name"].ToString() },
                    Priority = (row["priority"] is null) ? new Priority() : new Priority { Id = int.Parse(row["priority"].ToString()), Name = row["priority_name"].ToString(), Color = row["priority_color"].ToString() },
                    Status = new Status { Id = int.Parse(row["status"].ToString()), Name = row["status_name"].ToString(), Color = row["status_color"].ToString() },
                    CreatedAt = string.IsNullOrEmpty(row["created_at"].ToString()) ? new DateTime() : DateTime.Parse(row["created_at"].ToString()),
                    CompletedAt = string.IsNullOrEmpty(row["completed_at"].ToString()) ? new DateTime() : DateTime.Parse(row["completed_at"].ToString()),
                    Steps = await GetStepsAsync(int.Parse(row["id"].ToString()))
                });
            }
            return list;
        }
        #endregion

        #region Category
        public async Task<bool> AddCategoryAsync(Category category)
        {
            try
            {
                //var userId = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                {
                    { "name", category.Name },
                    { "user_id", "2"},
                };

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addCategory, data);

                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        category.Id = id;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.getCategories);
                List<Dictionary<string, object>> listDict = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
                if (listDict != null || listDict.Count > 0)
                {
                    return await StoreCategoryObjects(listDict);
                }
                else return new ObservableCollection<Category>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Category>();
            }
        }

        private async Task<ObservableCollection<Category>> StoreCategoryObjects(List<Dictionary<string, object>> listDict)
        {
            var list = new ObservableCollection<Category>();
            foreach (Dictionary<string, object> row in listDict)
            {
                list.Add(new Category { Id = int.Parse(row["id"].ToString()), Name = row["name"].ToString() });
            }
            return list;
        }

        public async Task<int> GetCategoryIdAsync(Category category)
        {
            try
            {
                var data = new Dictionary<string, string>()
                    {
                        { "name", category.Name },
                        { "user_id",  SessionService.Instance.CurrentUser.Id.ToString() }
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.getCategoryId, data);
                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        return id;
                    }
                    else return -1;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.deleteCategory(id));
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                var data = new Dictionary<string, string>()
                    {
                        { "name", category.Name },
                        { "user_id",  SessionService.Instance.CurrentUser.Id.ToString() }
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.editCategory(category.Id), data);
                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


        #region Priority
        public async Task<bool> AddPriorityAsync(Priority priority)
        {
            try
            {
                //var userId = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                {
                    { "name", priority.Name },
                    { "color", priority.Color },
                    { "user_id", "2"},
                };

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addCategory, data);

                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        priority.Id = id;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeletePriorityAsync(int id)
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.deletePriority(id));
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ObservableCollection<Priority>> GetPrioritiesAsync()
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.getPriorities);
                List<Dictionary<string, object>> listDict = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
                if (listDict != null || listDict.Count > 0)
                {
                    return await StorePriorityObjects(listDict);
                }
                else return new ObservableCollection<Priority>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Priority>();
            }
        }

        private async Task<ObservableCollection<Priority>> StorePriorityObjects(List<Dictionary<string, object>> listDict)
        {
            var list = new ObservableCollection<Priority>();
            foreach (Dictionary<string, object> row in listDict)
            {
                list.Add(new Priority { Id = int.Parse(row["id"].ToString()), Name = row["name"].ToString(), Color = row["color"].ToString() });
            }
            return list;
        }

        public async Task<int> GetPriorityIdAsync(Priority priority)
        {
            try
            {
                var data = new Dictionary<string, string>()
                    {
                        { "name", priority.Name },
                        { "color", priority.Color },
                        { "user_id",  SessionService.Instance.CurrentUser.Id.ToString() }
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.getPriorityId, data);
                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["id"].ToString(), out id))
                    {
                        return id;
                    }
                    else return -1;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
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
        public async Task<ObservableCollection<Step>> GetStepsAsync(int taskId)
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.getSteps(taskId));
                List<Dictionary<string, object>> listDict = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
                if (listDict != null || listDict.Count > 0)
                {
                    return await StoreStepObjects(listDict);
                }
                else return new ObservableCollection<Step>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Step>();
            }
        }

        private async Task<ObservableCollection<Step>> StoreStepObjects(List<Dictionary<string, object>> listDict)
        {
            var list = new ObservableCollection<Step>();
            foreach (Dictionary<string, object> row in listDict)
            {
                list.Add(new Step { Index = int.Parse(row["step_index"].ToString()), Description = row["step"].ToString() });
            }
            return list;
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
        #endregion


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
