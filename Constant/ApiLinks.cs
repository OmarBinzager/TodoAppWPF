using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Constant
{
    public static class ApiLink
    {
        // Server 
        public const string server = "127.0.0.1";
        public const string port = "8000";
        public readonly static string serverlink = $"http://{server}:{port}/api";

        #region Task API Links
        // Task
        //global task link
        public readonly static string tasklink = $"{serverlink}/task";
        // task routes
        public readonly static string addTask = $"{tasklink}/add";
        public readonly static string getTaskId = $"{tasklink}/get-id";
        public readonly static string getTasks = $"{tasklink}/get-all";
        // parametered task routes
        public static string deleteTask(int id) => $"{tasklink}/{id}/delete";
        public static string editTask(int id) => $"{tasklink}/{id}/edit";
        public static string getSteps(int id) => $"{tasklink}/{id}/get-steps";
        public static string addStep(int id) => $"{tasklink}/{id}/add-step";
        public static string editSteps(int id) => $"{tasklink}/{id}/edit-steps";
        public static string deleteStep(int id, int stepId) => $"{tasklink}/{id}/delete-step/{stepId}";
        public static string deleteSteps(int id) => $"{tasklink}/{id}/delete-steps";
        #endregion

        #region Category API Links
        // Category
        //global category link
        public readonly static string categorylink = $"{serverlink}/category";
        // category routes
        public readonly static string addCategory = $"{categorylink}/add";
        public readonly static string getCategoryId = $"{categorylink}/get-id";
        public readonly static string getCategories = $"{categorylink}/get-all";
        // parametered category routes
        public static string deleteCategory(int id) => $"{categorylink}/{id}/delete";
        public static string editCategory(int id) => $"{categorylink}/{id}/edit";

        #endregion

        #region Priority API Links
        // Priority
        //global priority link
        public readonly static string prioritylink = $"{serverlink}/priority";
        // priority routes
        public readonly static string addPriority = $"{prioritylink}/add";
        public readonly static string getPriorityId = $"{prioritylink}/get-id";
        public readonly static string getPriorities = $"{prioritylink}/get-all";
        // parametered priority routes
        public static string deletePriority(int id) => $"{prioritylink}/{id}/delete";
        public static string editPriority(int id) => $"{prioritylink}/{id}/edit";

        #endregion

        #region Status API Links
        // Status
        //global status link
        public readonly static string statuslink = $"{serverlink}/status";
        // status routes
        //public readonly static string addPriority = $"{prioritylink}/add";
        public readonly static string getStatuses = $"{statuslink}/get-all";
        #endregion

        #region Dashboard API Links
        //global dashboard link
        public readonly static string dashboardlink = $"{serverlink}/dashboard";
        // dashboard routes
        public readonly static string getStats = $"{dashboardlink}/stats";
        public readonly static string getRecentTasks = $"{dashboardlink}/recent-tasks";
        public readonly static string getCompletedTasks = $"{dashboardlink}/completed-tasks";
        #endregion


    }
}
