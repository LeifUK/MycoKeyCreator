using System.Windows;

namespace MycoKeyCreator.Application.View
{
    /// <summary>
    /// Interaction logic for LiteratureView.xaml
    /// </summary>
    public partial class LiteratureView : Window
    {
        public LiteratureView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.LiteratureViewModel).Save();
            DialogResult = true;
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
