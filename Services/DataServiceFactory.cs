using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Constant;

namespace ToDoProject.Services
{
    public static class DataServiceFactory
    {
        public static IDataService GetService()
        {
            if (DataSwitcher.dataType == DatabaseType.sql)
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
