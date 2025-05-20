using laibarysystemDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ToDoProject.Classes;
using ToDoProject.Model;
using ToDoProject.View;
using Windows.Storage.Pickers.Provider;
using Windows.System;

namespace ToDoProject.Services
{
    public class SqlServerDataService : IDataService
    {

        private DBHelper db = DBHelper.Instance;


        public async Task<bool> AddCategoryAsync(Category category)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                Dictionary<string, object> categoryDic = new Dictionary<string, object>() {
                    { "name", category.Name },
                    { "user_id", SessionService.Instance.CurrentUser.Id }
                };
                int result = db.Insert("Category", categoryDic);
                if(result > 0) 
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> AddPriorityAsync(Priority priority)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                Dictionary<string, object> priorityDic = new Dictionary<string, object>() {
                    { "name", priority.Name },
                    { "color", priority.Color },
                    { "user_id", SessionService.Instance.CurrentUser.Id }
                };
                int result = db.Insert("Priority", priorityDic);
                if (result > 0)
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> AddStepAsync(Step step, int taskId)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                Dictionary<string, object> stepDic = new Dictionary<string, object>() {
                    { "task_id",  taskId},
                    { "step", step.Description },
                    { "step_index", step.Index }
                };
                int result = db.Insert("Steps", stepDic);
                if (result > 0)
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> AddStepsAsync(Collection<Step> steps, int taskId)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                bool result = true;
                foreach(Step step in steps)
                {
                    result = result && await AddStepAsync(step, taskId);
                }
                return result;
            });
        }

        public async Task<bool> AddTaskAsync(Model.Task task)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                task.Picture = uploadImage(task.Picture);
                Dictionary<string, object> taskDic = new Dictionary<string, object>() {
                    { "title", task.Title },
                    { "description", task.Description },
                    { "picture", task.Picture },
                    { "status", task.Status.Id },
                    { "due_date", task.DueDate.ToString("yyyy-MM-dd") },
                    { "created_at", task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") },
                    { "user_id", SessionService.Instance.CurrentUser.Id }
                };
                if (task.Category != null) taskDic.Add("category", task.Category.Id);
                if (task.Priority != null) taskDic.Add("priority", task.Priority.Id);
                int result = db.Insert("Task", taskDic);
                if (result > 0)
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var parameter = new Dictionary<string, object>() { { "id", id } };
                int result = db.Delete("Category", "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;

            });
        }

        public async Task<bool> DeletePriorityAsync(int id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var parameter = new Dictionary<string, object>() { {"id" , id } };
                int result = db.Delete("Priority", "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;

            });
        }


        public async Task<bool> DeleteStepAsync(int id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var parameter = new Dictionary<string, object>(){
                    { "id", id }
                };
                int result = db.Delete("Steps", "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;

            });
        }

        public async Task<bool> DeleteStepsAsync(int taskId)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var parameter = new Dictionary<string, object>(){
                    { "task_id", taskId }
                };
                int result = db.Delete("Steps", "task_id = @task_id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;

            });
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var parameter = new Dictionary<string, object>(){
                    { "id", id }
                };
                //var task = db.Select("Task", "id = @id", parameter);
                //if(task.Rows.Count == 1)
                //{
                //    string image = task.Rows[0]["picture"].ToString();
                //    deleteImage(image);
                //}
                int result = db.Delete("Task", "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;

            });
        }

        public async Task<ObservableCollection<Category>> GetCategoriesAsync()
        {

            return await System.Threading.Tasks.Task.Run(() =>
            {
                var result = db.Select("Category", $"user_id = {SessionService.Instance.CurrentUser.Id}");
                var list = new ObservableCollection<Category>();
                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Category { Id = (int)row["id"], Name = row["name"].ToString() });
                }
                return list;
            });
        }

        public async Task<int> GetCategoryIdAsync(Category category)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                DataTable result = db.Select($"Category", $"Name = '{category.Name}' and user_id = {SessionService.Instance.CurrentUser.Id}");
                if (result.Rows.Count == 1)
                    category.Id = (int)result.Rows[0]["id"];
                else
                    category.Id = (int)result.Rows[result.Rows.Count - 1]["id"];
                return category.Id;
            });
        }

        public async Task<ObservableCollection<Priority>> GetPrioritiesAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var result = db.Select("Priority", $"user_id = {SessionService.Instance.CurrentUser.Id}");
                var list = new ObservableCollection<Priority>();
                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Priority { Id = (int)row["id"], Name = row["name"].ToString(), Color = row["color"].ToString() });
                }
                return list;
            });
        }

        public async Task<int> GetPriorityIdAsync(Priority priority)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                DataTable result = db.Select("Priority", $"Name = '{priority.Name}' and Color = '{priority.Color}' and user_id = {SessionService.Instance.CurrentUser.Id}");
                if (result.Rows.Count == 1)
                    priority.Id = (int)result.Rows[0]["id"];
                else
                    priority.Id = (int)result.Rows[result.Rows.Count - 1]["id"];
                return priority.Id;
            });
        }

        public Task<int> GetStatusIdAsync(Status status)
        {
            throw new NotImplementedException();
        }

        public async Task<ObservableCollection<Status>> GetStatusesAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var result = db.Select("Status");
                var list = new ObservableCollection<Status>();
                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Status { Id = (int)row["id"], Name = row["name"].ToString(), Color = row["color"].ToString() });
                }
                return list;
            });
        }

        public async Task<ObservableCollection<Step>> GetStepsAsync(int id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var result = db.Select("Steps", "task_id = " + id);
                var list = new ObservableCollection<Step>();
                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Step { Index = (int)row["step_index"], Description = row["step"].ToString() });
                }
                return list;
            });
        }

        public async Task<int> GetTaskIdAsync(Model.Task task)
        {
            return await System.Threading.Tasks.Task.Run( async () => 
            {
                DataTable result = db.Select("Task", "title = '" + task.Title + "' and CAST(description AS VARCHAR(MAX)) = '" + task.Description + "' and due_date = '" + task.DueDate.ToString("yyyy-MM-dd") + "' and user_id = "+ SessionService.Instance.CurrentUser.Id);
                if (result.Rows.Count == 1)
                    task.Id = (int)result.Rows[0]["id"];
                else
                    task.Id = (int)result.Rows[result.Rows.Count - 1]["id"];
                return task.Id;
            });
        }

        public async Task<ObservableCollection<Model.Task>> GetTasksAsync()
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                string query = $"Select Task.id, title, description, picture, due_date, created_at, completed_at, Category.id as \"category_id\", Category.name as \"category_name\", [Priority].id as \"priority_id\" , [Priority].name as \"priority_name\", [Priority].color as \"priority_color\", [Status].id as \"status_id\", [Status].name as \"status_name\", [Status].color as \"status_color\" from Task left join Category on Task.category = Category.id left join [Priority] on Task.[priority] = [Priority].id join [Status] on Task.[status] = [Status].id WHERE Task.user_id = {SessionService.Instance.CurrentUser.Id}";
                var dt = db.SqlCmd(query);
                return await StoreTaskIntoObjects(dt);
            });
        }

        public async Task<ObservableCollection<Model.Task>> SearchAsync(string text)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                string query = $"Select Task.id, title, description, picture, due_date, created_at, completed_at, Category.id as \"category_id\", Category.name as \"category_name\", [Priority].id as \"priority_id\" , [Priority].name as \"priority_name\", [Priority].color as \"priority_color\", [Status].id as \"status_id\", [Status].name as \"status_name\", [Status].color as \"status_color\" from Task left join Category on Task.category = Category.id left join [Priority] on Task.[priority] = [Priority].id join [Status] on Task.[status] = [Status].id WHERE Task.user_id = {SessionService.Instance.CurrentUser.Id} and (Task.title like '%{text}%'  or Task.description like '%{text}%')";
                var dt = db.SqlCmd(query);
                return await StoreTaskIntoObjects(dt);
            });
        }

        private async Task<ObservableCollection<Model.Task>> StoreTaskIntoObjects(DataTable dt)
        {
            var list = new ObservableCollection<Model.Task>();

            foreach (DataRow row in dt.Rows)
            {
                var task = new Model.Task();
                task.Id = (int)row["id"];
                task.Title = row["title"].ToString() ?? "";
                task.Description = row["description"].ToString() ?? "";
                task.DueDate = (DateTime)row["due_date"];
                task.Picture = !(row["picture"] is string) ? "" : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/" + row["picture"].ToString());
                task.Category = !(row["category_id"] is int) ? new Category() : new Category { Id = (int)row["category_id"], Name = row["category_name"].ToString() };
                task.Priority = !(row["priority_id"] is int) ? new Priority() : new Priority { Id = (int)row["priority_id"], Name = row["priority_name"].ToString(), Color = row["priority_color"].ToString() };
                task.Status = new Status { Id = (int)row["status_id"], Name = row["status_name"].ToString(), Color = row["status_color"].ToString() };
                task.CreatedAt = string.IsNullOrEmpty(row["created_at"].ToString()) ? new DateTime() : (DateTime)row["created_at"];
                task.CompletedAt = row["completed_at"].ToString() == "" || row["completed_at"] is null ? new DateTime() : (DateTime)row["completed_at"];
                task.Steps = await GetStepsAsync((int)row["id"]);
                list.Add(task);
            }

            return list;
        }

        public async Task<ObservableCollection<Model.Task>> GetTasksWhereAsync(string where = "", string rowsCount = "", string order = "")
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                string query = $"Select {rowsCount} Task.id, title, description, picture, due_date, created_at, completed_at, Category.id as \"category_id\", Category.name as \"category_name\", [Priority].id as \"priority_id\" , [Priority].name as \"priority_name\", [Priority].color as \"priority_color\", [Status].id as \"status_id\", [Status].name as \"status_name\", [Status].color as \"status_color\" from Task left join Category on Task.category = Category.id left join [Priority] on Task.[priority] = [Priority].id join [Status] on Task.[status] = [Status].id {where} {order}";

                var dt = db.SqlCmd(query);
                return await StoreTaskIntoObjects(dt);
            });
        }

        public string uploadImage(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            string uploads = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", Path.GetFileName(path));
            if (File.Exists(uploads)) return path;
            if (!File.Exists(path)) return "";
            string rootOfSavedImage = Path.GetFullPath(path);
            if (rootOfSavedImage == uploads) return Path.GetFileName(path);
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(path);

            // Get the path to the 'Uploads' folder
            string uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

            // Ensure the 'Uploads' folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Combine the folder path and unique filename
            string destinationFilePath = Path.Combine(uploadsFolder, uniqueFileName);
            // Copy the file to the 'Uploads' folder
            try
            {
                File.Copy(path, destinationFilePath, true);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return uniqueFileName;
        }

        public void deleteImage(string fileName)
        {
            string fullImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", Path.GetFileName(fileName));
            if (File.Exists(fullImagePath))
            {
                File.Delete(fullImagePath);
            }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            return await System.Threading.Tasks.Task.Run(() => {

                var data = new Dictionary<string, object>(){
                    { "name", category.Name }
                };
                var parameter = new Dictionary<string, object>(){
                    { "id", category.Id }
                };
            int result = db.Update("Category", data, "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> UpdatePriorityAsync(Priority priority)
        {
            return await System.Threading.Tasks.Task.Run(() => {

                var data = new Dictionary<string, object>(){
                    { "name", priority.Name },
                    { "color", priority.Color }
                };
                var parameter = new Dictionary<string, object>(){
                    { "id", priority.Id }
                };
                int result = db.Update("Priority", data, "id = @id", parameter);
                if (result > 0)
                    return true;
                else
                    return false;
            });
        }

        //public async Task<bool> UpdateStepAsync(Step step, int taskId)
        //{
        //    return await System.Threading.Tasks.Task.Run(() => {

        //        var data = new Dictionary<string, object>(){
        //            { "step", step.Description },
        //        };
        //        var parameter = new Dictionary<string, object>(){
        //            { "step_index", step.Index },
        //            { "task_id", taskId}
        //        };
        //        int result = db.Update("Steps", data, "step_index = @step_index and task_id = @task_id", parameter);
        //        if (result > 0)
        //            return true;
        //        else
        //            return false;
        //    });
        //}

        //public async Task<bool> UpdateStepsAsync(Collection<Step> steps, int taskId)
        //{
        //    return await System.Threading.Tasks.Task.Run( async () => {

        //        bool result = true;
        //        foreach (var step in steps) { 
        //            result = result && await UpdateStepAsync(step, taskId);
        //        }
        //        return result;
        //    });
        //}

        public async Task<bool> UpdateTaskAsync(Model.Task task)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
            task.Picture = uploadImage(task.Picture);
            Dictionary<string, object> taskDic = new Dictionary<string, object>() {
                { "title", task.Title },
                { "description", task.Description },
                { "picture", task.Picture },
                { "status", task.Status.Id },
                { "due_date", task.DueDate.ToString("yyyy-MM-dd") },
                { "created_at", task.CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") },
                { "completed_at", task.CompletedAt == new DateTime() ? null : task.CompletedAt.ToString("yyyy-MM-dd hh:mm:ss") }
            };
                if (task.Category.Name != null) taskDic.Add("category", task.Category.Id);
                if (task.Priority.Name != null) taskDic.Add("priority", task.Priority.Id);
                //// delete old picture
                //var oldTask = db.Select("Task", $"id = {task.Id}");
                //if (oldTask.Rows.Count == 1)
                //{
                //    string image = oldTask.Rows[0]["picture"].ToString();
                //    if(task.Picture != image)
                //        deleteImage(image);
                //}
                Dictionary<string, object> taskParams = new Dictionary<string, object>() {
                    { "id", task.Id },
                };
                int result = db.Update("Task", taskDic, "id = @id", taskParams);
                db.SqlCmd("Delete from Steps where task_id = " + task.Id);
                bool stepsResult = await AddStepsAsync(task.Steps, task.Id);
                if (result > 0 && stepsResult)
                    return true;
                else
                    return false;
            });
        }

        public async Task<bool> UpdateFeildAtTable(string table, Dictionary<string, object> data, string whereClause, Dictionary<string, object> whereParams)
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                var result = db.Update(table, data, whereClause, whereParams);
                if (result > 0)
                    return true;
                else return false;
            });
        }

        public async Task<Dictionary<string, object>> GetDashboardStatsAsync()
        {
            return await System.Threading.Tasks.Task.Run(async () =>
            {
                Dictionary<string, object> dashbordDic = new Dictionary<string, object>();

                int taskCount = (int)db.SelectFields("Task", "Count(id) as count", $"user_id = {SessionService.Instance.CurrentUser.Id}").Rows[0]["count"];
                double completedTaskCount = (int) db.SelectFields("Task", "Count(id) as count", $"status = 3 and user_id = {SessionService.Instance.CurrentUser.Id}").Rows[0]["count"];
                double inProgTaskCount = (int)db.SelectFields("Task", "Count(id) as count", $"status = 2 and user_id = {SessionService.Instance.CurrentUser.Id}").Rows[0]["count"];
                double notStartTaskCount = (int)db.SelectFields("Task", "Count(id) as count", $"status = 1 and user_id = {SessionService.Instance.CurrentUser.Id}").Rows[0]["count"];
                dashbordDic.Add("tasksCount", taskCount);

                if (taskCount == 0)
                {
                    dashbordDic.Add("percentage1", 0);
                    dashbordDic.Add("percentage2", 0);
                    dashbordDic.Add("percentage3", 0);
                    return dashbordDic;
                }
                int percentage1 = (int)Math.Ceiling((notStartTaskCount / taskCount) * 100) ;
                int percentage2 = (int)Math.Ceiling((inProgTaskCount / taskCount)* 100) ;
                int percentage3 = (int)Math.Ceiling((completedTaskCount / taskCount) * 100);
                dashbordDic.Add("percentage1", percentage1);
                dashbordDic.Add("percentage2", percentage2);
                dashbordDic.Add("percentage3", percentage3);

                return dashbordDic;
            });
        }

        public async Task<ObservableCollection<Model.Task>> GetRecentTasks()
        {
            ObservableCollection<Model.Task> latestTasks = await GetTasksWhereAsync($"where status != 3 and Task.user_id = {SessionService.Instance.CurrentUser.Id}", "TOP 10", "ORDER BY created_at DESC");
            return latestTasks;
        }
        public async Task<ObservableCollection<Model.Task>> GetCompletedTasks()
        {
            ObservableCollection<Model.Task> completedTasks = await GetTasksWhereAsync($"where status = 3 and Task.user_id = {SessionService.Instance.CurrentUser.Id}", "Top 5", "ORDER BY completed_at DESC");
            return completedTasks;
        }

        public async Task<Model.User> Authenticate(string email, string password)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                Dictionary<string, object> paramdic = new Dictionary<string, object>()
                {
                    {"email" , email},
                    {"password",  password}
                };
                var userData = db.Select("\"User\"", "email = @email and password = @password", paramdic);
                if (userData.Rows.Count != 1) return null;
                Model.User user = new Model.User() { 
                    Id = (int)userData.Rows[0]["id"],
                    Email = email,
                    FirstName = userData.Rows[0]["first_name"].ToString(),
                    LastName = userData.Rows[0]["last_name"].ToString(),
                    Avatar = userData.Rows[0]["avatar"].ToString(),
                };
                return user;
            });
        }

        public async Task<bool> RegisterUser(Model.User user, string password, string passwordConfirm)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                if (password != passwordConfirm) return false;
                user.Avatar = uploadImage(user.Avatar);
                Dictionary<string, object> paramdic = new Dictionary<string, object>()
                {
                    {"email" , user.Email.Trim()},
                    {"first_name",  user.FirstName.Trim()},
                    {"last_name",  user.LastName.Trim()},
                    {"password",  password},
                    {"avatar", user.Avatar }
                };
                var result = db.Insert("\"User\"", paramdic);
                if (result != 1) return false;
                return true;
            });
        }

        public async Task<bool> Logout()
        {
            return await System.Threading.Tasks.Task.Run(() => {
                SessionService.Instance.Clear();
                return true;
            });
        }

        public async Task<bool> UpdateUser(Model.User user)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                user.Avatar = uploadImage(user.Avatar);
                Dictionary<string, object> paramdic = new Dictionary<string, object>()
                {
                    {"email" , user.Email.Trim()},
                    {"first_name",  user.FirstName.Trim()},
                    {"last_name",  user.LastName.Trim()},
                    {"avatar", user.Avatar }
                };
                var parameter = new Dictionary<string, object>(){
                    { "id", user.Id }
                };
                //var oldUser = db.Select("\"User\"", $"id = {user.Id}");
                //if (oldUser.Rows.Count == 1)
                //{
                //    string image = oldUser.Rows[0]["avatar"].ToString();
                //    if (user.Avatar != image)
                //        deleteImage(image);
                //}
                var result = db.Update("\"User\"", paramdic, $"id = @id", parameter);
                if (result != 1) return false;
                SessionService.Instance.CurrentUser.FirstName = user.FirstName;
                SessionService.Instance.CurrentUser.LastName = user.LastName;
                SessionService.Instance.CurrentUser.Email = user.Email;
                SessionService.Instance.CurrentUser.Avatar = user.Avatar;
                if (AuthStorage.LoadUser() != null)
                {
                    AuthStorage.SaveUser(SessionService.Instance.CurrentUser);
                }
                return true;
            });
        }

        public async Task<bool> ResetPassword(string oldPassword, string newPassword, string newPasswordConfirm)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                if (newPassword != newPasswordConfirm) return false;
                Dictionary<string, object> paramdic = new Dictionary<string, object>()
                {
                    {"password" , newPassword},
                };
                var parameter = new Dictionary<string, object>(){
                    { "id", SessionService.Instance.CurrentUser.Id },
                    {"password" , oldPassword},
                };
                var isPasswordCurrect = db.Select("\"User\"", "id = @id and password = @password", parameter);
                if (isPasswordCurrect.Rows.Count != 1) return false;
                var result = db.Update("\"User\"", paramdic, "id = @id and password = @password", parameter);
                if (result != 1) return false;
                return true;
            });
        }
    }
}
