using System.Windows;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for KeyHeaderView.xaml
    /// </summary>
    public partial class KeyHeaderView : Window
    {
        public KeyHeaderView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.KeyHeaderViewModel).Save();
            DialogResult = true;
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
