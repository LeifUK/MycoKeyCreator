using System.Windows;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for AttributeView.xaml
    /// </summary>
    public partial class AttributeView : Window
    {
        public AttributeView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            (DataContext as ViewModel.AttributeViewModel).Save();
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool EditAttributeValue(Library.DBObject.AttributeChoice attributeChoice)
        {
            if (attributeChoice == null)
            {
                return false;
            }

            ViewModel.InputTextViewModel inputTextViewModel = new ViewModel.InputTextViewModel();
            inputTextViewModel.Title = "MycoKeys.Application";
            inputTextViewModel.Text = attributeChoice.description;

            InputTextView inputTextView = new InputTextView();
            inputTextView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            inputTextView.Owner = this;
            inputTextView.DataContext = inputTextViewModel;

            bool success = inputTextView.ShowDialog() == true;
            if (success)
            {
                attributeChoice.description = inputTextViewModel.Text;
            }

            return success;
        }

        private void EditSelectedAttributeValue()
        {
            ViewModel.AttributeViewModel attributeViewModel = (DataContext as ViewModel.AttributeViewModel);
            if (attributeViewModel.SelectedAttributeChoice == null)
            {
                return;
            }

            EditAttributeValue(attributeViewModel.SelectedAttributeChoice);
            attributeViewModel.Refresh();
        }

        private void _buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Library.DBObject.AttributeChoice attributeValue = new Library.DBObject.AttributeChoice();
            if (EditAttributeValue(attributeValue))
            {
                (DataContext as ViewModel.AttributeViewModel).Add(attributeValue);
            }
        }

        private void _dataGridValues_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedAttributeValue();
        }

        private void _buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedAttributeValue();
        }

        private void _buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete the selectedattribute value?\nThis operation cannot be undone.", "Delete Attribute Value", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                (DataContext as ViewModel.AttributeViewModel).DeleteSelectedValue();
            }
        }

        private void _buttonUp_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.AttributeViewModel).MoveSelectedAttributeUp();
        }

        private void _buttonDown_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.AttributeViewModel).MoveSelectedAttributeDown();
        }
    }
}
