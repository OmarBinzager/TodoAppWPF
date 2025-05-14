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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ToDoProject.View.Components"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ToDoProject.View.Components;assembly=ToDoProject.View.Components"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class AuthTextBox : TextBox
    {
        static AuthTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AuthTextBox), new FrameworkPropertyMetadata(typeof(AuthTextBox)));
        }


        public TextBlock hintTextBlock;
        public TextBox textBox;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            hintTextBlock = GetTemplateChild("HintTextBlock") as TextBlock;
            textBox = GetTemplateChild("TextBox") as TextBox;
            textBox.TextChanged += TextBox_TextChanged;
        }

        public bool ErrorTextVisibility
        {
            get { return (bool)GetValue(ErrorTextVisibilityProperty); }
            set { SetValue(ErrorTextVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorTextVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorTextVisibilityProperty =
            DependencyProperty.Register("ErrorTextVisibility", typeof(bool), typeof(AuthTextBox), new PropertyMetadata(false));



        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(AuthTextBox), new PropertyMetadata(""));


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(Text))
            {
                hintTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                hintTextBlock.Visibility = Visibility.Collapsed;
            }
            OnTextChanged(e);
        }

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(AuthTextBox), new PropertyMetadata(""));





        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IconData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(Geometry), typeof(AuthTextBox), new PropertyMetadata(null));


    }
}
