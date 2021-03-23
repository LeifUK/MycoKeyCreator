namespace MycoKeyCreator.Application.ViewModel
{
    public class LiteratureViewModel : OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public LiteratureViewModel(Library.DBObject.Literature literature)
        {
            _literature = literature;

            Title = _literature.title;
            Description = _literature.description;
        }

        public void Save()
        {
            _literature.title = Title;
            _literature.description = Description;
        }

        private Library.DBObject.Literature _literature;

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
    }
}
