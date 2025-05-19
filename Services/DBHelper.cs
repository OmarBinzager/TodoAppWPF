using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoProject.Constant;

namespace laibarysystemDB
{
    public class DBHelper
    {

        

        private static readonly Lazy<DBHelper> _instance = new Lazy<DBHelper>(() => new DBHelper());
        private readonly string _connectionString;

        public static DBHelper Instance => _instance.Value;

        private DBHelper()
        {
            // قراءة connection string من Settings
            _connectionString = SqlLink.connString;
        }


        public int Insert(string tableName, Dictionary<string, object> data)
        {
            var columns = string.Join(", ", data.Keys);
            var parameters = string.Join(", ", data.Keys.Select(k => "@" + k));
            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

            return ExecuteNonQuery(query, data);
        }

        public DataTable Select(string tableName, string whereClause = "", Dictionary<string, object> parameters = null)
        {
            var query = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
                query += $" WHERE {whereClause}";

            return ExecuteQuery(query, parameters);
        }

        public DataTable SelectFields(string tableName, string fields, string whereClause = "", Dictionary<string, object> parameters = null)
        {
            var query = $"SELECT {fields} FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
                query += $" WHERE {whereClause}";

            return ExecuteQuery(query, parameters);
        }

        public DataTable SqlCmd(string sql)
        {
            return ExecuteQuery(sql, null);
        }

        public int Update(string tableName, Dictionary<string, object> data, string whereClause, Dictionary<string, object> whereParams)
        {
            var setClause = string.Join(", ", data.Keys.Select(k => $"{k} = @{k}"));
            var query = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";

            var allParams = new Dictionary<string, object>(data);
            foreach (var p in whereParams)
                allParams[p.Key] = p.Value;

            return ExecuteNonQuery(query, allParams);
        }

        public static bool DatabaseExists()
        {
            try
            {
                using (var connection = new SqlConnection(SqlLink.masterConnString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        $"SELECT database_id FROM sys.databases WHERE Name = @databaseName", connection);
                    command.Parameters.AddWithValue("@databaseName", SqlLink.dbName);
                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public int Delete(string tableName, string whereClause, Dictionary<string, object> parameters)
        {
            var query = $"DELETE FROM {tableName} WHERE {whereClause}";
            return ExecuteNonQuery(query, parameters);
        }

        private DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            var dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue("@" + p.Key, p.Value ?? DBNull.Value);
                }

                try{
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return dt;
        }

        private int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue("@" + p.Key, p.Value ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    return 0;
                }
            }
        }

    }
}
