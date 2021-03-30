using System.Collections.ObjectModel;

namespace MycoKeyCreator.Application.ViewModel
{
    public class KeyHeaderViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public KeyHeaderViewModel(Library.Database.IKeyManager iKeyManager, Library.DBObject.Key key)
        {
            IKeyManager = iKeyManager;
            Key = key;
            Name = Key.name;
            Title = Key.title;
            Description = Key.description;
            Notes = Key.notes;
            Copyright = Key.copyright;
            Publish = Key.Publish;
            Load();
            _originalLiteratureItems = new ObservableCollection<Library.DBObject.Literature>(_literatureItems);
        }

        public void Load()
        {
            LiteratureItems = new ObservableCollection<Library.DBObject.Literature>(IKeyManager.GetLiteratureEnumeratorForKey(Key.id));
            if (LiteratureItems.Count > 0)
            {
                SelectedLiterature = LiteratureItems[0];
            }
        }

        public void Save()
        {
            Key.name = Name;
            Key.title = Title;
            Key.description = Description;
            Key.notes = Notes;
            Key.copyright = Copyright;
            Key.Publish = Publish;
            if (Key.id == 0)
            {
                IKeyManager.Insert(Key);
            }
            else
            {
                IKeyManager.Update(Key);
            }
            foreach (var literature in LiteratureItems)
            {
                if (_originalLiteratureItems.Contains(literature))
                {
                    IKeyManager.Update(literature);
                    _originalLiteratureItems.Remove(literature);
                }
                else
                {
                    literature.key_id = Key.id;
                    IKeyManager.Insert(literature);
                }
            }
            foreach (var literature in _originalLiteratureItems)
            {
                IKeyManager.Delete(literature);
            }
            _originalLiteratureItems.Clear();
        }

        public readonly Library.Database.IKeyManager IKeyManager;
        public readonly Library.DBObject.Key Key;

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

        private string _notes;
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                NotifyPropertyChanged("Notes");
            }
        }

        private ObservableCollection<Library.DBObject.Literature> _originalLiteratureItems;

        private ObservableCollection<Library.DBObject.Literature> _literatureItems;
        public ObservableCollection<Library.DBObject.Literature> LiteratureItems
        {
            get
            {
                return _literatureItems;
            }
            set
            {
                _literatureItems = value;
                NotifyPropertyChanged("LiteratureItems");
            }
        }

        private Library.DBObject.Literature _selectedLiterature;
        public Library.DBObject.Literature SelectedLiterature
        {
            get
            {
                return _selectedLiterature;
            }
            set
            {
                _selectedLiterature = value;
                NotifyPropertyChanged("SelectedLiterature");
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

        public void DeleteSelectedLiterature()
        {
            if (SelectedLiterature != null)
            {
                IKeyManager.Delete(SelectedLiterature);
                Load();
            }
        }
    }
}
