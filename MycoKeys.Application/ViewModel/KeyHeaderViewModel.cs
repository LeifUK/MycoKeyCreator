namespace MycoKeys.Application.ViewModel
{
    public class KeyHeaderViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public KeyHeaderViewModel(Library.Database.IKeyManager iKeyManager, Library.DBObject.Key key)
        {
            _iKeyManager = iKeyManager;
            _key = key;
            Name = _key.name;
            Title = _key.title;
            Description = _key.description;
            Copyright = _key.copyright;
            Publish = _key.Publish;
        }

        public void Save()
        {
            _key.name = Name;
            _key.title = Title;
            _key.description = Description;
            _key.copyright = Copyright;
            _key.Publish = Publish;
            if (_key.id == 0)
            {
                _iKeyManager.Insert(_key);
            }
            else
            {
                _iKeyManager.Update(_key);
            }
        }

        private Library.Database.IKeyManager _iKeyManager;
        private Library.DBObject.Key _key;

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

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

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private string _copyright;
        public string Copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                _copyright = value;
                NotifyPropertyChanged("Copyright");
            }
        }

        private bool _publish;
        public bool Publish
        {
            get
            {
                return _publish;
            }
            set
            {
                _publish = value;
                NotifyPropertyChanged("Publish");
            }
        }
    }
}
