using System.Windows;
using System.Windows.Input;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for KeyView.xaml
    /// </summary>
    public partial class KeyView : Window
    {
        public KeyView()
        {
            InitializeComponent();
        }

        private void _datagridSpecies_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void _datagridAttributes_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void _buttonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private bool EditText(string label, ref string text)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;

            OpenControls.Wpf.Utilities.ViewModel.InputTextViewModel inputTextViewModel = new OpenControls.Wpf.Utilities.ViewModel.InputTextViewModel();
            inputTextViewModel.Title = "MycoKeys.Application";
            inputTextViewModel.Label = label;
            inputTextViewModel.Text = text;

            OpenControls.Wpf.Utilities.View.InputTextView inputTextView = new OpenControls.Wpf.Utilities.View.InputTextView();
            inputTextView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            inputTextView.Owner = this;
            inputTextView.DataContext = inputTextViewModel;

            bool success = (inputTextView.ShowDialog() == true);
            if (success)
            {
                text = inputTextViewModel.Text;
            }
            return success;
        }

        private void _buttonEditKeyName_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Name;
            if (EditText("Name", ref text))
            {
                keyViewModel.Name = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditTitle_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Title;
            if (EditText("Title", ref text))
            {
                keyViewModel.Title = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditCopyright_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Copyright;
            if (EditText("Copyright", ref text))
            {
                keyViewModel.Copyright = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditDescription_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Description;
            if (EditText("Description", ref text))
            {
                keyViewModel.Description = text;
                keyViewModel.Save();
            }
        }

        private void EditSpecies(Library.DBObject.Species species)
        {
            if (species == null)
            {
                return;
            }

            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            ViewModel.SpeciesViewModel speciesViewModel = new ViewModel.SpeciesViewModel(keyViewModel.IKeyManager, keyViewModel.Key, species);
            View.SpeciesView speciesView = new SpeciesView();
            speciesView.DataContext = speciesViewModel;
            speciesView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            speciesView.Owner = this;
            if (speciesView.ShowDialog() == true)
            {
                keyViewModel.LoadSpecies();
            }
        }

        private void _buttonAddSpecies_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            Library.DBObject.Species species = new Library.DBObject.Species();
            EditSpecies(species);
        }

        private void _buttonEditSpecies_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            Library.DBObject.Species species = new Library.DBObject.Species();
            EditSpecies(keyViewModel.SelectedSpecies);
        }

        //private void _dataGridSpeciesDescriptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    EditSpeciesDescription();
        //}

        private void _buttonDeleteSpecies_Click(object sender, RoutedEventArgs e)
        {
            //if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected species?\nThis operation cannot be undone.", "Delete Species", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //{
            //    (DataContext as MycoKeys.Application.ViewModel.KeyViewModel).DeleteSelectedSpecies();
            //}
        }

        private void _dataGridAttributes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditAttribute((DataContext as MycoKeys.Application.ViewModel.KeyViewModel).SelectedAttribute);
        }

        private bool EditAttribute(MycoKeys.Library.DBObject.Attribute attribute)
        {
            if (attribute == null)
            {
                return false;
            }

            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            ViewModel.AttributeViewModel attributeViewModel = new ViewModel.AttributeViewModel(attribute);
            View.AttributeView attributeView = new AttributeView();
            attributeView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            attributeView.Owner = this;
            attributeView.DataContext = attributeViewModel;
            return (attributeView.ShowDialog() == true);
        }

        private void _buttonAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            Library.DBObject.Attribute attribute = new Library.DBObject.Attribute();
            if (EditAttribute(attribute))
            {
                keyViewModel.Insert(attribute);
            }
        }

        private void _buttonEditAttribute_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;
            if (EditAttribute(keyViewModel.SelectedAttribute))
            {
                keyViewModel.Update(keyViewModel.SelectedAttribute);
            }
        }

        private void _buttonDeleteAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected condition and its associations?\nThis operation cannot be undone.", "Delete Condition", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                (DataContext as MycoKeys.Application.ViewModel.KeyViewModel).DeleteSelectedAttribute();
            }
        }

        private void _dataGridSpecies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void _buttonAttributes_Click(object sender, RoutedEventArgs e)
        {
            MycoKeys.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeys.Application.ViewModel.KeyViewModel;

            if (keyViewModel.SelectedSpecies == null)
            {
                return;
            }

            ViewModel.SpeciesAttributesViewModel speciesAttributesViewModel = 
                new ViewModel.SpeciesAttributesViewModel(keyViewModel.IKeyManager, keyViewModel.Key, keyViewModel.SelectedSpecies);
            View.SpeciesAttributesView speciesAttributesView = new SpeciesAttributesView();
            speciesAttributesView.DataContext = speciesAttributesViewModel;
            speciesAttributesView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            speciesAttributesView.Owner = this;
            speciesAttributesView.ShowDialog();
        }
    }
}
