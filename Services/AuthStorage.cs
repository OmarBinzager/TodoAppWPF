using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Model;

namespace ToDoProject.Services
{
    public class AuthStorage
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyTodoApp",
            "user.json");

        public static void SaveUser(User user)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllText(FilePath, JsonSerializer.Serialize(user));
        }

        public static User LoadUser()
        {
            if (!File.Exists(FilePath)) return null;

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<User>(json);
        }

        public static void ClearUser()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
