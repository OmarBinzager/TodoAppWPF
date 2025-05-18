using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
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
using ToDoProject.View;
using System.Windows;
using Windows.System;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Networking;

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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(task.Title), "title");
                content.Add(new StringContent(task.Description), "description");
                content.Add(new StringContent(task.Status.Id.ToString()), "status");
                content.Add(new StringContent(task.DueDate.ToString("yyyy-MM-dd hh:mm:ss")), "due_date");
                content.Add(new StringContent(task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss")), "created_at");

                if (task.Category != null) content.Add(new StringContent(task.Category.Id.ToString()), "category");
                if (task.Priority != null) content.Add(new StringContent(task.Priority.Id.ToString()), "priority");
                if (!string.IsNullOrEmpty(task.Picture))
                {
                    var fileStream = File.OpenRead(task.Picture);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(fileContent, "picture", Path.GetFileName(task.Picture));
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                var response = await client.PostAsync(ApiLink.addTask, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (response.StatusCode == System.Net.HttpStatusCode.Created) {
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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(task.Title), "title");
                content.Add(new StringContent(task.Description), "description");
                content.Add(new StringContent(task.Status.Id.ToString()), "status");
                content.Add(new StringContent(task.DueDate.ToString("yyyy-MM-dd hh:mm:ss")), "due_date");
                content.Add(new StringContent(task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss")), "created_at");
                content.Add(new StringContent(task.CompletedAt.ToString("yyyy-MM-dd hh:mm:ss")), "completed_at");

                if (task.Category != null) if (task.Category.Name != null) content.Add(new StringContent(task.Category.Id.ToString()), "category");
                if (task.Priority != null) if(task.Priority.Name != null) content.Add(new StringContent(task.Priority.Id.ToString()), "priority");
                if (!string.IsNullOrEmpty(task.Picture))
                {
                    if (!task.Picture.Contains(ApiLink.storage))
                    {
                        var fileStream = File.OpenRead(task.Picture);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(fileContent, "picture", Path.GetFileName(task.Picture));
                    }
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                var response = await client.PostAsync(ApiLink.editTask(task.Id), content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
        public async Task<ObservableCollection<Model.Task>> GetTasksAsync()
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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

        public async Task<ObservableCollection<Model.Task>> SearchAsync(string text)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                string url = UrlBuilder.BuildQueryUrl(ApiLink.search, new Dictionary<string, string> { { "search", text } });
                string response = await client.GetStringAsync(url);
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
                var task = new Model.Task();
                task.Id = int.Parse(row["id"].ToString());
                task.Title = row["title"].ToString() ?? "";
                task.Description = row["description"].ToString() ?? "";
                task.DueDate = DateTime.Parse(row["due_date"].ToString());
                task.Picture = (row["picture"] is null) ? "" : $"{ApiLink.storage}/{row["picture"]}";
                task.Category = (row["category"] is null) ? new Category() : new Category { Id = int.Parse(row["category"].ToString()), Name = row["category_name"].ToString() };
                task.Priority = (row["priority"] is null) ? new Priority() : new Priority { Id = int.Parse(row["priority"].ToString()), Name = row["priority_name"].ToString(), Color = row["priority_color"].ToString() };
                task.Status = new Status { Id = int.Parse(row["status"].ToString()), Name = row["status_name"].ToString(), Color = row["status_color"].ToString() };
                task.CreatedAt = row["created_at"] is null ? new DateTime() : DateTime.Parse(row["created_at"].ToString());
                task.CompletedAt = row["completed_at"] is null ? new DateTime() : DateTime.Parse(row["completed_at"].ToString());
                task.Steps = await GetStepsAsync(int.Parse(row["id"].ToString()));
                list.Add(task);
            }
            return list;
        }
        #endregion

        #region Category
        public async Task<bool> AddCategoryAsync(Category category)
        {
            try
            {
                var userId = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                {
                    { "name", category.Name },
                };

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addCategory, data);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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
                var user_id = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                    {
                        { "name", category.Name },
                        { "user_id",  user_id }
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
                var user_id = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                    {
                        { "name", category.Name },
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.editCategory(category.Id), data);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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
                var data = new Dictionary<string, string>()
                {
                    { "name", priority.Name },
                    { "color", priority.Color },
                };

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addPriority, data);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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
                var user_id = SessionService.Instance.CurrentUser.Id.ToString();
                var data = new Dictionary<string, string>()
                    {
                        { "name", priority.Name },
                        { "color", priority.Color },
                        { "user_id",  user_id }
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
        public async Task<bool> UpdatePriorityAsync(Priority priority)
        {
            try
            {
                var data = new Dictionary<string, string>()
                    {
                        { "name", priority.Name },
                    };
                string url = UrlBuilder.BuildQueryUrl(ApiLink.editPriority(priority.Id), data);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
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

        #region Step
        public async Task<bool> AddStepAsync(Step step, int taskId)
        {
            try
            {
                var data = new Dictionary<string, string>()
                {
                    { "step", step.Description },
                    { "step_index", step.Index.ToString() },
                };

                string url = UrlBuilder.BuildQueryUrl(ApiLink.addSteps(taskId), data);

                string response = await client.GetStringAsync(url);
                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null)
                {
                    int id = -1;
                    if (int.TryParse(dict["step_index"].ToString(), out id))
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

        public async Task<bool> AddStepsAsync(Collection<Step> steps, int taskId)
        {
            try
            {

                var json = JsonSerializer.Serialize(StoreStepsInDictionary(steps));
                var content = new StringContent($"{{\"steps\":{json}}}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(ApiLink.addSteps(taskId), content);
                string responseBody = await response.Content.ReadAsStringAsync();
                if ( response.StatusCode == System.Net.HttpStatusCode.Created)
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

        private List<Dictionary<string, string>> StoreStepsInDictionary(Collection<Step> steps)
        {
            var listofSteps = new List<Dictionary<string, string>> ();
            foreach (var step in steps) {
                listofSteps.Add(new Dictionary<string, string> { { "step", step.Description }, { "step_index", step.Index.ToString() } });
            }
            return listofSteps;
        }

        public async Task<bool> DeleteStepAsync(int id)
        {
            throw new NullReferenceException();
        }

        public async Task<bool> DeleteStepsAsync(int taskId)
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.deleteSteps(taskId));
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

        public Task<bool> UpdateStepAsync(Step step, int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStepsAsync(Collection<Step> steps, int taskId)
        {
            try
            {

                var json = JsonSerializer.Serialize(StoreStepsInDictionary(steps));
                var content = new StringContent($"{{\"steps\":{json}}}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(ApiLink.editSteps(taskId), content);
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
        public async Task<Model.User> Authenticate(string email, string password)
        {
            try
            {
                var formObj = new Dictionary<string, string>()
                {
                    { "email", email },
                    { "password", password },
                };
                var json = JsonSerializer.Serialize(formObj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(ApiLink.login, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement data = doc.RootElement;
                string message = data.GetProperty("message").ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var userObj = JsonSerializer.Deserialize<Dictionary<string, object>>(data.GetProperty("user"));
                    var token = data.GetProperty("token");
                    var user = new Model.User()
                    {
                        Id = int.Parse(userObj["id"].ToString()),
                        FirstName = userObj["first_name"].ToString(),
                        LastName = userObj["last_name"].ToString(),
                        Email = userObj["email"].ToString(),
                        Avatar = userObj["avatar"] is null ? "" : $"{ApiLink.storage}/{userObj["avatar"].ToString()}",
                        Token = token.ToString(),
                    };
                    return user;
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> RegisterUser(Model.User user, string password, string passwordConfirm)
        {
            try
            {
                //var userObj = new Dictionary<string, string>()
                //{
                //    { "first_name", user.FirstName },
                //    { "last_name", user.LastName },
                //    { "email", user.Email },
                //    { "password", password },
                //    { "password_confirmation", passwordConfirm }
                //};
                //var json = JsonSerializer.Serialize(userObj);
                var content = new MultipartFormDataContent();
                //content.Add(new StringContent(json, Encoding.UTF8, "application/json")) ;
                content.Add(new StringContent(user.FirstName), "first_name");
                content.Add(new StringContent(user.LastName), "last_name");
                content.Add(new StringContent(user.Email), "email");
                content.Add(new StringContent(password), "password");
                content.Add(new StringContent(passwordConfirm), "password_confirmation");
                if (!string.IsNullOrEmpty(user.Avatar) && File.Exists(user.Avatar)) {
                    var fileStream = File.OpenRead(user.Avatar);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    content.Add(fileContent, "avatar", Path.GetFileName(user.Avatar));
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(ApiLink.register, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObj = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }else
                {
                    MessageBox.Show(responseObj["message"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = await client.PostAsync(ApiLink.logout, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObj = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SessionService.Instance.Clear();
                    return true;
                }
                else
                {
                    MessageBox.Show(responseObj["message"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public async Task<bool> UpdateUser(Model.User user)
        {
            try
            {
                
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(user.FirstName), "first_name");
                content.Add(new StringContent(user.LastName), "last_name");
                content.Add(new StringContent(user.Email), "email");
                if (!string.IsNullOrEmpty(user.Avatar) && File.Exists(user.Avatar))
                {
                    var fileStream = File.OpenRead(user.Avatar);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    content.Add(fileContent, "avatar", Path.GetFileName(user.Avatar));
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(ApiLink.editUser, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement data = doc.RootElement;
                string message = data.GetProperty("message").ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var userObj = JsonSerializer.Deserialize<Dictionary<string, object>>(data.GetProperty("user"));
                    SessionService.Instance.CurrentUser.FirstName = userObj["first_name"].ToString();
                    SessionService.Instance.CurrentUser.LastName = userObj["last_name"].ToString();
                    SessionService.Instance.CurrentUser.Email = userObj["email"].ToString();
                    SessionService.Instance.CurrentUser.Avatar = userObj["avatar"] is null ? "" : $"{ApiLink.storage}/{userObj["avatar"].ToString()}";
                    if (AuthStorage.LoadUser() != null)
                    {
                        AuthStorage.SaveUser(SessionService.Instance.CurrentUser);
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ResetPassword(string oldPassword, string newPassword, string newPasswordConfirm)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(oldPassword), "current_password");
                content.Add(new StringContent(newPassword), "password");
                content.Add(new StringContent(newPasswordConfirm), "password_confirmation");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(ApiLink.resetPassword, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObj = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(responseObj["message"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        #endregion


        #region Dashboard
        public async Task<Dictionary<string, object>> GetDashboardStatsAsync()
        {
            var stats = new Dictionary<string, object>();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                string response = await client.GetStringAsync(ApiLink.getStats);
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (dict != null || dict.Count > 0)
                {
                    stats.Add("tasksCount", int.Parse(dict["total_tasks"].ToString()));
                    if (int.Parse(dict["total_tasks"].ToString()) == 0)
                    {
                        stats.Add("percentage1", 0);
                        stats.Add("percentage2", 0);
                        stats.Add("percentage3", 0);
                        return stats;
                    }
                    double percentage1 = double.Parse(dict["not_started"].ToString());
                    double percentage2 = double.Parse(dict["in_progress"].ToString());
                    double percentage3 = double.Parse(dict["completed"].ToString());
                    stats.Add("percentage1", percentage1);
                    stats.Add("percentage2", percentage2);
                    stats.Add("percentage3", percentage3);
                    return stats;
                }
                else return stats;
            }
            catch (Exception ex) {
                return stats;
            }
            
        }

        public async Task<ObservableCollection<Model.Task>> GetRecentTasks()
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                string response = await client.GetStringAsync(ApiLink.getRecentTasks);
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

        public async Task<ObservableCollection<Model.Task>> GetCompletedTasks()
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{SessionService.Instance.CurrentUser.Token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                string response = await client.GetStringAsync(ApiLink.getCompletedTasks);
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


        #endregion


        #region Status
        public async Task<ObservableCollection<Status>> GetStatusesAsync()
        {
            try
            {
                string response = await client.GetStringAsync(ApiLink.getStatuses);
                List<Dictionary<string, object>> listDict = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
                if (listDict != null || listDict.Count > 0)
                {
                    return await StoreStatusObjects(listDict);
                }
                else return new ObservableCollection<Status>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Status>();
            }
        }

        private async Task<ObservableCollection<Status>> StoreStatusObjects(List<Dictionary<string, object>> listDict)
        {
            var list = new ObservableCollection<Status>();
            foreach (Dictionary<string, object> row in listDict)
            {
                list.Add(new Status { Id = int.Parse(row["id"].ToString()), Name = row["name"].ToString(), Color = row["color"].ToString() });
            }
            return list;
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
