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
        public const string trustedCertificate = "True";
        public const string dbName = "ToDoDB2";
        public const string master = "master";
        public const string integeratedSecurity = "True";
        public readonly static string connString = $"Server = {server}; Database = {dbName}; Integrated Security = {integeratedSecurity};Trusted_Connection={trustedCertificate};";
        public readonly static string masterConnString = $"Server = {server}; Database = {master}; Integrated Security = {integeratedSecurity};Trusted_Connection={trustedCertificate};";
    }
}
