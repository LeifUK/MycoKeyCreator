using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace MycoKeyMaker.Application.View
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

        private void _buttonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private bool EditText(string label, ref string text)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;

            ViewModel.InputTextViewModel inputTextViewModel = new ViewModel.InputTextViewModel();
            inputTextViewModel.Title = "MycoKeyMaker.Application";
            inputTextViewModel.Text = text;

            InputTextView inputTextView = new InputTextView();
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

        private void EditSpecies(Library.DBObject.Species species)
        {
            if (species == null)
            {
                return;
            }

            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
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
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            Library.DBObject.Species species = new Library.DBObject.Species();
            EditSpecies(species);
        }

        private void _buttonEditSpecies_Click(object sender, RoutedEventArgs e)
        {
            EditSpecies((DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).SelectedSpecies);
        }

        private void _dataGridSpeciesDescriptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSpecies((DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).SelectedSpecies);
        }

        private void _buttonDeleteSpecies_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected species?\nThis operation cannot be undone.", "Delete Species", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                (DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).DeleteSelectedSpecies();
            }
        }

        private void EditChoiceAttribute(MycoKeyMaker.Library.DBObject.Attribute attribute)
        {
            if (attribute == null)
            {
                return;
            }

            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            ViewModel.AttributeViewModel attributeViewModel = new ViewModel.AttributeViewModel(keyViewModel.IKeyManager, keyViewModel.Key, attribute);
            View.AttributeView attributeView = new AttributeView();
            attributeView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            attributeView.Owner = this;
            attributeView.DataContext = attributeViewModel;
            if (attributeView.ShowDialog() == true)
            {
                keyViewModel.ReloadAttributes();
            }
        }
        
        private void _buttonAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;

            View.AttributeTypeView attributeTypeView = new AttributeTypeView();
            ViewModel.AttributeTypeViewModel attributeTypeViewModel = new ViewModel.AttributeTypeViewModel();
            attributeTypeView.DataContext = attributeTypeViewModel;
            attributeTypeView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            attributeTypeView.Owner = this;
            if (attributeTypeView.ShowDialog() != true)
            {
                return;
            }
            if (attributeTypeViewModel.SelectedAttributeType.Value == Library.Database.AttributeType.Choice)
            {
                Library.DBObject.Attribute attribute = new Library.DBObject.Attribute();
                attribute.type = (Int16)Library.Database.AttributeType.Choice;
                attribute.position = (short)keyViewModel.Attributes.Count();
                EditChoiceAttribute(attribute);
            }
            else
            {
                string description = "";
                if (OpenControls.Wpf.Utilities.View.InputTextView.ShowDialog(this, "New Attribute", "Description", ref description))
                {
                    Library.DBObject.Attribute attribute = new Library.DBObject.Attribute();
                    attribute.type = (Int16)attributeTypeViewModel.SelectedAttributeType.Value;
                    attribute.position = (short)keyViewModel.Attributes.Count();
                    attribute.description = description;
                    keyViewModel.AddAttribute(attribute);
                    keyViewModel.ReloadAttributes();
                }
            }
        }

        private void EditAttribute()
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;

            Library.DBObject.Attribute attribute = keyViewModel.SelectedAttribute;
            if (attribute.type == (Int16)Library.Database.AttributeType.Choice)
            {
                EditChoiceAttribute(attribute);
            }
            else if (
                        (attribute.type == (Int16)Library.Database.AttributeType.MaximumSize) ||
                        (attribute.type == (Int16)Library.Database.AttributeType.MinimumSize)
                    )
            {
                string description = attribute.description;
                if (OpenControls.Wpf.Utilities.View.InputTextView.ShowDialog(this, "Edit Attribute", "Description", ref description))
                {
                    attribute.description = description;
                    keyViewModel.UpdateAttribute(attribute);
                    keyViewModel.ReloadAttributes();
                }
            }
        }

        private void _buttonEditAttribute_Click(object sender, RoutedEventArgs e)
        {
            EditAttribute();
        }

        private void _dataGridAttributes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditAttribute();
        }

        private void _buttonDeleteAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected condition and its associations?\nThis operation cannot be undone.", "Delete Condition", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                (DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).DeleteSelectedAttribute();
            }
        }

        private void EditSpeciesAttributes()
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;

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

        private void _dataGridSpecies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSpeciesAttributes();
        }

        private void _buttonAttributes_Click(object sender, RoutedEventArgs e)
        {
            EditSpeciesAttributes();
        }

        private void _buttonMoveAttributeUp_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).MoveSelectedAttributeUp();
        }

        private void _buttonMoveAttributeDown_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel).MoveSelectedAttributeDown();
        }

        private void _buttonEditHeader_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;

            View.KeyHeaderView keyHeaderView = new KeyHeaderView();
            ViewModel.KeyHeaderViewModel keyHeaderViewModel = new ViewModel.KeyHeaderViewModel(keyViewModel.IKeyManager, keyViewModel.Key);
            keyHeaderView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keyHeaderView.Owner = this;
            keyHeaderView.DataContext = keyHeaderViewModel;
            if (keyHeaderView.ShowDialog() == true)
            {
                keyViewModel.LoadHeader();
            }
        }

        private void _buttonEditKeyName_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Name;
            if (EditText("Name", ref text))
            {
                keyViewModel.Name = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditTitle_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Title;
            if (EditText("Title", ref text))
            {
                keyViewModel.Title = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditCopyright_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Copyright;
            if (EditText("Copyright", ref text))
            {
                keyViewModel.Copyright = text;
                keyViewModel.Save();
            }
        }

        private void _buttonEditDescription_Click(object sender, RoutedEventArgs e)
        {
            MycoKeyMaker.Application.ViewModel.KeyViewModel keyViewModel = DataContext as MycoKeyMaker.Application.ViewModel.KeyViewModel;
            string text = keyViewModel.Description;
            if (EditText("Description", ref text))
            {
                keyViewModel.Description = text;
                keyViewModel.Save();
            }
        }
    }
}
