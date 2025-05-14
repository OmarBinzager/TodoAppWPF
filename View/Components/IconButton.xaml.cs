using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoProject.View.Components
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {
        public IconButton()
        {
            InitializeComponent();
        }
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(IconButton), new PropertyMetadata(null));



        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(IconButton), new PropertyMetadata(null));



        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(null));


    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace ToDoProject.View.Components
//{
//    /// <summary>
//    /// Interaction logic for IconButton.xaml
//    /// </summary>
//    public class IconButton : Button
//    {

//        static IconButton()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton),
//                new FrameworkPropertyMetadata(typeof(IconButton)));
//        }
//        public double CornerRadius
//        {
//            get { return (double)GetValue(CornerRadiusProperty); }
//            set { SetValue(CornerRadiusProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty CornerRadiusProperty =
//            DependencyProperty.Register("CornerRadius", typeof(double), typeof(IconButton), new PropertyMetadata(0));



//        public Geometry Data
//        {
//            get { return (Geometry)GetValue(DataProperty); }
//            set { SetValue(DataProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty DataProperty =
//            DependencyProperty.Register("Data", typeof(Geometry), typeof(IconButton), new PropertyMetadata(null));



//        public SolidColorBrush IconColor
//        {
//            get { return (SolidColorBrush)GetValue(IconColorProperty); }
//            set { SetValue(IconColorProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for IconColor.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty IconColorProperty =
//            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(Colors.White));


//    }
//}
