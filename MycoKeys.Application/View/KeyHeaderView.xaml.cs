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

        private void EditLiterature(Library.DBObject.Literature literature)
        {
            if (literature == null)
            {
                return;
            }

            ViewModel.KeyHeaderViewModel keyHeaderViewModel = (DataContext as ViewModel.KeyHeaderViewModel);
            LiteratureView literatureView = new LiteratureView();
            ViewModel.LiteratureViewModel literatureViewModel = new ViewModel.LiteratureViewModel(keyHeaderViewModel.IKeyManager, keyHeaderViewModel.Key, literature);
            literatureView.DataContext = literatureViewModel;
            literatureView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            literatureView.Owner = this;
            if (literatureView.ShowDialog() == true)
            {
                keyHeaderViewModel.Load();
            }
        }

        private void _buttonAddLiterature_Click(object sender, RoutedEventArgs e)
        {
            EditLiterature(new Library.DBObject.Literature());
        }

        private void _buttonEditLiterature_Click(object sender, RoutedEventArgs e)
        {
            EditLiterature((DataContext as ViewModel.KeyHeaderViewModel).SelectedLiterature);
        }

        private void _buttonDeleteLiterature_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.KeyHeaderViewModel).DeleteSelectedLiterature();
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditLiterature((DataContext as ViewModel.KeyHeaderViewModel).SelectedLiterature);
        }
    }
}
