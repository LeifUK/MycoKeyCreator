namespace MycoKeys.Application.ViewModel
{
    public class KeysListViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public KeysListViewModel(MycoKeys.Library.Database.IKeyManager iKeyManager)
        {
            IKeyManager = iKeyManager;
            Load();
        }

        public void Load()
        {
            Keys = new System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Key>(IKeyManager.GetKeyEnumerator());
        }

        public readonly MycoKeys.Library.Database.IKeyManager IKeyManager;

        private System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Key> _keys;
        public System.Collections.ObjectModel.ObservableCollection<MycoKeys.Library.DBObject.Key> Keys
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

        private MycoKeys.Library.DBObject.Key _selectedKey;
        public MycoKeys.Library.DBObject.Key SelectedKey
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
