namespace MycoKeys.Application.ViewModel
{
    public class LiteratureViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public LiteratureViewModel(Library.Database.IKeyManager keyManager, Library.DBObject.Key key, Library.DBObject.Literature literature)
        {
            _keyManager = keyManager;
            _key = key;
            _literature = literature;

            Description = _literature.description;
            Url = _literature.url;
        }

        public void Save()
        {
            _literature.description = Description;
            _literature.url = Url;
            if (_literature.id == 0)
            {
                _literature.key_id = _key.id;
                _keyManager.Insert(_literature);
            }
            else
            {
                _keyManager.Update(_literature);
            }
        }

        private Library.Database.IKeyManager _keyManager;
        private Library.DBObject.Key _key;
        private Library.DBObject.Literature _literature;

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

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                NotifyPropertyChanged("Url");
            }
        }
    }
}
