using System.Windows;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for InputTextView.xaml
    /// </summary>
    public partial class InputTextView : Window
    {
        public InputTextView()
        {
            InitializeComponent();
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void _buttonOkay_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
