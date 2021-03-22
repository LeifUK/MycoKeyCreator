
using System.Windows;

namespace MycoKeys.Application.View
{
    /// <summary>
    /// Interaction logic for SpeciesDescriptionView.xaml
    /// </summary>
    public partial class SpeciesView : Window
    {
        public SpeciesView()
        {
            InitializeComponent();
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void _buttonOK_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SpeciesViewModel speciesViewModel = DataContext as ViewModel.SpeciesViewModel;
            speciesViewModel.Save();
            DialogResult = true;
        }
    }
}
