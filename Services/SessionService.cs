using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Model;

namespace ToDoProject.Services
{
    public class SessionService
    {
        private static SessionService _instance;
        public static SessionService Instance => _instance == null ? _instance = new SessionService() : _instance;

        public User CurrentUser { get; private set; }

        private SessionService() { }

        public void SetUser(User user) => CurrentUser = user;
        public void Clear() => CurrentUser = null;
    }
}
