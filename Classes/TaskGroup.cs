﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Classes
{
    public class TaskGroup
    {
        public string DateString { get; set; }
        public ObservableCollection<Model.Task> Tasks { get; set; }
    }
}
