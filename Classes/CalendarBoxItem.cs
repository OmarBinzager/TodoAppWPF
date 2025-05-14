using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToDoProject.Classes
{
    public class CalendarBoxItem : ListBoxItem
    {


        public string DateFormat { get; set; }
        public bool IsCurrentMonth
        {
            get { return (bool)GetValue(IsCurrentMonthProperty); }
            set { SetValue(IsCurrentMonthProperty, value); }
        }

        public DateTime Date { get; set; }

        // Using a DependencyProperty as the backing store for IsCurrentMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCurrentMonthProperty =
            DependencyProperty.Register("IsCurrentMonth", typeof(bool), typeof(CalendarBoxItem), new PropertyMetadata(false));


    }
}
