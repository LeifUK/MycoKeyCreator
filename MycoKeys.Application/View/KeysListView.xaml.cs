using System.Windows;
using System.Windows.Input;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for KeysView.xaml
    /// </summary>
    public partial class KeysListView : Window
    {
        public KeysListView()
        {
            InitializeComponent();
        }

        private void _datagridKeys_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ShowKeyView(MycoKeys.Library.Database.IKeyManager iKeyManager, MycoKeys.Library.DBObject.Key key)
        {
            MycoKeys.Application.View.KeyView keyView = new MycoKeys.Application.View.KeyView();
            keyView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keyView.Owner = this;
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = new MycoKeys.Application.ViewModel.KeyViewModel(iKeyManager, key);
            keyView.DataContext = keyViewModel;
            keyView.ShowDialog();
        }

        private void EditSelectedKey()
        {
            MycoKeys.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeys.Application.ViewModel.KeysListViewModel;
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
            MycoKeys.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeys.Application.ViewModel.KeysListViewModel;

            OpenControls.Wpf.Utilities.ViewModel.InputTextViewModel inputTextViewModel = new OpenControls.Wpf.Utilities.ViewModel.InputTextViewModel();
            OpenControls.Wpf.Utilities.View.InputTextView inputTextView = new OpenControls.Wpf.Utilities.View.InputTextView();
            inputTextView.DataContext = inputTextViewModel;

            inputTextViewModel.Label = "Key Name";
            inputTextViewModel.Title = "New Key";
            if ((inputTextView.ShowDialog() == true) && !string.IsNullOrEmpty(inputTextViewModel.Text))
            {
                MycoKeys.Library.DBObject.Key key = new MycoKeys.Library.DBObject.Key();
                key.name = inputTextViewModel.Text;
                keysListViewModel.IKeyManager.Insert(key);

                ShowKeyView(keysListViewModel.IKeyManager, key);
                keysListViewModel.Load();
            }
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeysListViewModel keysListViewModel = DataContext as MycoKeys.Application.ViewModel.KeysListViewModel;
            keysListViewModel.IKeyManager.Delete(keysListViewModel.SelectedKey);
            keysListViewModel.Load();
        }

        private void _buttonConfigure_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _buttonCloseDB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
