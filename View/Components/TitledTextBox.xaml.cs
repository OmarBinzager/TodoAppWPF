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
    /// Interaction logic for TitledTextBox.xaml
    /// </summary>
    public partial class TitledTextBox : UserControl
    {
        public TitledTextBox()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            title.Text = TextTitle;
            textBox.Text = Text;
            textBox.Hint = Hint;
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TitledTextBox), new PropertyMetadata(""));




        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(TitledTextBox), new PropertyMetadata(""));



        public string TextTitle
        {
            get { return (string)GetValue(TextTitleProperty); }
            set { SetValue(TextTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextTitleProperty =
            DependencyProperty.Register("TextTitle", typeof(string), typeof(TitledTextBox), new PropertyMetadata(""));



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null) TextChanged.Invoke(this, e);
            Text = ((CustomTextBox)sender).Text;
            //MessageBox.Show(Text);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;
    }
}