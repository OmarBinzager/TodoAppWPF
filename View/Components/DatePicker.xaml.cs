using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoProject.Classes;

namespace ToDoProject.View.Components
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {



        public DatePicker()
        {
            InitializeComponent();
        }




        public DateTime? CurrentMonth
        {
            get { return (DateTime?)GetValue(CurrentMonthProperty); }
            set { SetValue(CurrentMonthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMonthProperty =
            DependencyProperty.Register("CurrentMonth", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(null));




        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(null));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            calendarSwitch.Click += CalendarSwitch_Click;
            calendarBox.MouseLeftButtonUp += CalendarBox_MouseLeftButtonUp;
            popup.Closed += Popup_Closed;
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            calendarSwitch.IsChecked = IsMouseOver;
        }

        private void CalendarBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(calendarBox.SelectedItem is CalendarBoxItem selected)
            {
                SelectedDate = selected.Date;
                if (selected.Date.ToString("yyyyMM") != CurrentMonth?.ToString("yyyyMM"))
                {
                    CurrentMonth = selected.Date;
                    generateCalendar(); 
                }
            }
        }

        private void CalendarSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (calendarSwitch.IsChecked == true) {
                popup.IsOpen = true;
                if(CurrentMonth is null)CurrentMonth = DateTime.Now;
                generateCalendar();
            }
        }

        private void generateCalendar()
        {
            calendarBox.Items.Clear();
            DateTime firstDayOfMonth = new DateTime((int)CurrentMonth?.Year, (int)CurrentMonth?.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int firstOffset = (int)firstDayOfMonth.DayOfWeek;
            int lastOffset = 6 - (int)lastDayOfMonth.DayOfWeek;

            DateTime fDay = firstDayOfMonth.AddDays(-firstOffset);
            DateTime lDay = lastDayOfMonth.AddDays(lastOffset);

            for ( DateTime date = fDay; date <= lDay; date = date.AddDays(1))
                addCalendarBoxItem(date);
            if (SelectedDate != null) calendarBox.SelectedValue = SelectedDate?.ToString("yyyyMMdd");


        }

        private void addCalendarBoxItem(DateTime date)
        {
            CalendarBoxItem item = new CalendarBoxItem
            {
                IsCurrentMonth = date.Month == CurrentMonth?.Month,
                Date = date,
                DateFormat = date.ToString("yyyyMMdd"),
                Content = date.Day.ToString()
            };
            calendarBox.Items.Add(item);
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth?.AddMonths(-1);
            generateCalendar();
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth?.AddMonths(1);
            generateCalendar();
        }
    }
}
