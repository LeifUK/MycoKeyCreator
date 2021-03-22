namespace MycoKeyMaker.Application.ViewModel
{
    public class KeysListViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public KeysListViewModel(string databaseName, MycoKeyMaker.Library.Database.IKeyManager iKeyManager)
        {
            IKeyManager = iKeyManager;
            Title = "Database: " + databaseName;
            Load();
        }

        public void Load()
        {
            Keys = new System.Collections.ObjectModel.ObservableCollection<MycoKeyMaker.Library.DBObject.Key>(IKeyManager.GetKeyEnumerator());
        }

        public readonly MycoKeyMaker.Library.Database.IKeyManager IKeyManager;

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<MycoKeyMaker.Library.DBObject.Key> _keys;
        public System.Collections.ObjectModel.ObservableCollection<MycoKeyMaker.Library.DBObject.Key> Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                _keys = value;
                NotifyPropertyChanged("Keys");
            }
        }

        private MycoKeyMaker.Library.DBObject.Key _selectedKey;
        public MycoKeyMaker.Library.DBObject.Key SelectedKey
        {
            get
            {
                return _selectedKey;
            }
            set
            {
                _selectedKey = value;
                NotifyPropertyChanged("SelectedKey");
            }
        }

        public void DeleteSelectedKey()
        {
            if (SelectedKey != null)
            {
                IKeyManager.Delete(SelectedKey);
                Load();
            }
        }
    }
}
