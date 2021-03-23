using System.Windows;

namespace MycoKeyCreator.Application.View
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
            ViewModel.KeyHeaderViewModel keyHeaderViewModel = (DataContext as ViewModel.KeyHeaderViewModel);
            if (string.IsNullOrEmpty(keyHeaderViewModel.Name))
            {
                return;
            }
            keyHeaderViewModel.Save();
            DialogResult = true;
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool EditLiterature(Library.DBObject.Literature literature)
        {
            if (literature == null)
            {
                return false;
            }

            ViewModel.KeyHeaderViewModel keyHeaderViewModel = (DataContext as ViewModel.KeyHeaderViewModel);
            LiteratureView literatureView = new LiteratureView();
            ViewModel.LiteratureViewModel literatureViewModel = new ViewModel.LiteratureViewModel(literature);
            literatureView.DataContext = literatureViewModel;
            literatureView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            literatureView.Owner = this;
            return (literatureView.ShowDialog() == true);
        }

        private void _buttonAddLiterature_Click(object sender, RoutedEventArgs e)
        {
            Library.DBObject.Literature literature = new Library.DBObject.Literature();
            if (EditLiterature(literature))
            {
                (DataContext as ViewModel.KeyHeaderViewModel).LiteratureItems.Add(literature);
            }
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
