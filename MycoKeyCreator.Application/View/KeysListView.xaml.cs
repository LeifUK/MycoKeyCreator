using System.Windows;
using System.Windows.Input;

namespace MycoKeyCreator.Application.View
{
    /// <summary>
    /// Interaction logic for KeysView.xaml
    /// </summary>
    public partial class KeysListView : Window
    {
        public KeysListView(ViewModel.ISerialise iSerialise)
        {
            _iISerialise = iSerialise;
            InitializeComponent();
        }

        private readonly ViewModel.ISerialise _iISerialise;

        private void _datagridKeys_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ShowKeyView(MycoKeyCreator.Library.Database.IKeyManager iKeyManager, MycoKeyCreator.Library.DBObject.Key key)
        {
            MycoKeyCreator.Application.View.KeyView keyView = new MycoKeyCreator.Application.View.KeyView();
            keyView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keyView.Owner = this;
            MycoKeyCreator.Application.ViewModel.KeyViewModel keyViewModel = new MycoKeyCreator.Application.ViewModel.KeyViewModel(iKeyManager, key);
            keyView.DataContext = keyViewModel;
            keyView.ShowDialog();
        }

        private void EditSelectedKey()
        {
            MycoKeyCreator.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeyCreator.Application.ViewModel.KeysListViewModel;
            if (keysListViewModel.SelectedKey != null)
            {
                ShowKeyView(keysListViewModel.IKeyManager, keysListViewModel.SelectedKey);
                keysListViewModel.Load();
            }
        }

        private void _datagridKeys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedKey();
        }

        private void _buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedKey();
        }

        private void _buttonNew_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyCreator.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeyCreator.Application.ViewModel.KeysListViewModel;

            MycoKeyCreator.Library.DBObject.Key key = new MycoKeyCreator.Library.DBObject.Key();
            View.KeyHeaderView keyHeaderView = new KeyHeaderView();
            ViewModel.KeyHeaderViewModel keyHeaderViewModel = new ViewModel.KeyHeaderViewModel(keysListViewModel.IKeyManager, key);
            keyHeaderView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keyHeaderView.Owner = this;
            keyHeaderView.DataContext = keyHeaderViewModel;
            if (keyHeaderView.ShowDialog() == true)
            {
                ShowKeyView(keysListViewModel.IKeyManager, key);
                keysListViewModel.Load();
            }
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected key?\nThis operation cannot be undone.", "Delete Key", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                (DataContext as MycoKeyCreator.Application.ViewModel.KeysListViewModel).DeleteSelectedKey();
            }
        }

        private void _buttonCloseDB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void _buttonImportDB_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyCreator.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeyCreator.Application.ViewModel.KeysListViewModel;
            _iISerialise.Import(keysListViewModel.IKeyManager);
            keysListViewModel.Load();
        }

        private void _buttonExportDB_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyCreator.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeyCreator.Application.ViewModel.KeysListViewModel;
            _iISerialise.Export(keysListViewModel.IKeyManager);
            keysListViewModel.Load();
        }
    }
}
