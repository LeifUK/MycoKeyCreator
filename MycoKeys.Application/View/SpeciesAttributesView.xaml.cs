using System.Windows;
using System.Windows.Data;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for SpeciesView.xaml
    /// </summary>
    public partial class SpeciesAttributesView : Window
    {
        public SpeciesAttributesView()
        {
            InitializeComponent();
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SpeciesAttributesViewModel speciesAttributesViewModel = (DataContext as ViewModel.SpeciesAttributesViewModel);
            ListCollectionView listCollectionView = new ListCollectionView(speciesAttributesViewModel.SpeciesAttributeValues);
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Attribute.description"));
            _dataGridAttributes.ItemsSource = listCollectionView;
        }
    }
}
