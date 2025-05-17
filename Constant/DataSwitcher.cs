using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoProject.Classes;

namespace ToDoProject.Constant
{
    public static class DataSwitcher
    {
        public static DatabaseType dataType;

        static DataSwitcher()
        {
            StorageType storage = LoadStorageType();
            if (storage != null)
                dataType = storage.Type;
            else
                dataType = DatabaseType.sql;
        }

        public static void SwitchToApi(StorageType storage)
        {
            dataType = DatabaseType.api;
            SaveStorageType(storage);
        }
        public static void SwitchToSql(StorageType storage)
        {
            dataType = DatabaseType.sql;
            SaveStorageType(storage);
        }

        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyTodoApp",
            "database_type.json");

        public static void SaveStorageType(StorageType storage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllText(FilePath, JsonSerializer.Serialize(storage));
        }

        public static StorageType LoadStorageType()
        {
            if (!File.Exists(FilePath)) return null;

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<StorageType>(json);
        }

        public static void ClearStorageType()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }


    }

    public enum DatabaseType { sql, api}
}
