using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MycoKeys.Application.ViewModel
{
    public class SpeciesViewModel :  OpenControls.Wpf.Utilities.ViewModel.BaseViewModel
    {
        public SpeciesViewModel(MycoKeys.Library.Database.IKeyManager keyManager, MycoKeys.Library.DBObject.Key key, MycoKeys.Library.DBObject.Species species)
        {
            _iKeyManager = keyManager;
            _key = key;
            Species = species;
            EditedSpecies = Species.Clone();
        }

        public void Save()
        {
            Species.CopyFrom(EditedSpecies);
            Species.key_id = _key.id;
            if (Species.id == 0)
            {
                _iKeyManager.Insert(Species);
            }
            else
            {
                _iKeyManager.Update(Species);
            }
        }

        private readonly MycoKeys.Library.Database.IKeyManager _iKeyManager;
        private readonly MycoKeys.Library.DBObject.Key _key;

        private ObservableCollection<KeyValuePair<string, string>> _speciesFields;
        public ObservableCollection<KeyValuePair<string, string>> SpeciesFields
        {
            get
            {
                return _speciesFields;
            }
            set
            {
                _speciesFields = value;
                NotifyPropertyChanged("SpeciesFields");
            }
        }

        public readonly MycoKeys.Library.DBObject.Species EditedSpecies;
        public readonly MycoKeys.Library.DBObject.Species Species;

        public string Name
        {
            get
            {
                return EditedSpecies.name;
            }
            set
            {
                EditedSpecies.name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Synonyms
        {
            get
            {
                return EditedSpecies.synonyms;
            }
            set
            {
                EditedSpecies.synonyms = value;
                NotifyPropertyChanged("Synonyms");
            }
        }

        public string Common_name
        {
            get
            {
                return EditedSpecies.common_name;
            }
            set
            {
                EditedSpecies.common_name = value;
                NotifyPropertyChanged("Common_name");
            }
        }

        public string Fruiting_body
        {
            get
            {
                return EditedSpecies.fruiting_body;
            }
            set
            {
                EditedSpecies.fruiting_body = value;
                NotifyPropertyChanged("Fruiting_body");
            }
        }

        public string Cap
        {
            get
            {
                return EditedSpecies.cap;
            }
            set
            {
                EditedSpecies.cap = value;
                NotifyPropertyChanged("Cap");
            }
        }

        public string Hymenium
        {
            get
            {
                return EditedSpecies.hymenium;
            }
            set
            {
                EditedSpecies.hymenium = value;
                NotifyPropertyChanged("Hymenium");
            }
        }

        public string Gills
        {
            get
            {
                return EditedSpecies.gills;
            }
            set
            {
                EditedSpecies.gills = value;
                NotifyPropertyChanged("Gills");
            }
        }

        public string Pores
        {
            get
            {
                return EditedSpecies.pores;
            }
            set
            {
                EditedSpecies.pores = value;
                NotifyPropertyChanged("Pores");
            }
        }

        public string Spines
        {
            get
            {
                return EditedSpecies.spines;
            }
            set
            {
                EditedSpecies.spines = value;
                NotifyPropertyChanged("Spines");
            }
        }

        public string Stem
        {
            get
            {
                return EditedSpecies.stem;
            }
            set
            {
                EditedSpecies.stem = value;
                NotifyPropertyChanged("Stem");
            }
        }

        public string Flesh
        {
            get
            {
                return EditedSpecies.flesh;
            }
            set
            {
                EditedSpecies.flesh = value;
                NotifyPropertyChanged("Flesh");
            }
        }

        public string Smell
        {
            get
            {
                return EditedSpecies.smell;
            }
            set
            {
                EditedSpecies.smell = value;
                NotifyPropertyChanged("Smell");
            }
        }

        public string Taste
        {
            get
            {
                return EditedSpecies.taste;
            }
            set
            {
                EditedSpecies.taste = value;
                NotifyPropertyChanged("Taste");
            }
        }

        public string Season
        {
            get
            {
                return EditedSpecies.season;
            }
            set
            {
                EditedSpecies.season = value;
                NotifyPropertyChanged("Season");
            }
        }

        public string Distribution
        {
            get
            {
                return EditedSpecies.distribution;
            }
            set
            {
                EditedSpecies.distribution = value;
                NotifyPropertyChanged("Distribution");
            }
        }

        public string Habitat
        {
            get
            {
                return EditedSpecies.habitat;
            }
            set
            {
                EditedSpecies.habitat = value;
                NotifyPropertyChanged("Habitat");
            }
        }

        public string Spore_print
        {
            get
            {
                return EditedSpecies.spore_print;
            }
            set
            {
                EditedSpecies.spore_print = value;
                NotifyPropertyChanged("Spore_print");
            }
        }

        public string Microscopic_features
        {
            get
            {
                return EditedSpecies.microscopic_features;
            }
            set
            {
                EditedSpecies.microscopic_features = value;
                NotifyPropertyChanged("Microscopic_features");
            }
        }

        public string Edibility
        {
            get
            {
                return EditedSpecies.edibility;
            }
            set
            {
                EditedSpecies.edibility = value;
                NotifyPropertyChanged("Edibility");
            }
        }

        public string Notes
        {
            get
            {
                return EditedSpecies.notes;
            }
            set
            {
                EditedSpecies.notes = value;
                NotifyPropertyChanged("Notes");
            }
        }
    }
}
