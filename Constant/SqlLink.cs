using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Constant
{
    public static class SqlLink
    {
        public const string server = ".";
        public const string dbName = "ToDoDB2";
        public const string integeratedSecurity = "true";
        public readonly static string connString = $"Server = {server}; Database = {dbName}; Integrated Security = {integeratedSecurity}";
    }
}
