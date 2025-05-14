using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Services
{
    public static class DataServiceFactory
    {
        public static IDataService GetService()
        {
            var backend = Properties.Settings.Default.Backend;
            if (backend == "sql")
            {
                return new SqlServerDataService();
            }
            else
            {
                return new LaravelApiService();
            }
        }
    }
}
