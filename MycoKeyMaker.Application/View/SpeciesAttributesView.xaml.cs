using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

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
            ListCollectionView listCollectionView = new ListCollectionView(speciesAttributesViewModel.SpeciesAttributes);
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Title"));
            _dataGridAttributes.ItemsSource = listCollectionView;
        }
    }

    public class ClassTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChoiceAttributeTemplate { get; set; }
        public DataTemplate SizeAttributeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement frameworkElement = container as FrameworkElement;
            if (frameworkElement == null)
            {
                return base.SelectTemplate(item, container);
            }
            if (frameworkElement.Parent is System.Windows.Controls.DataGridCell)
            {
                Model.ISpeciesAttributeValue iSpeciesAttributeValue = (frameworkElement.Parent as System.Windows.Controls.DataGridCell).DataContext as Model.ISpeciesAttributeValue;
                if (!(iSpeciesAttributeValue.Value is string))
                {
                    return SizeAttributeTemplate;
                }
            }
            return ChoiceAttributeTemplate;
        }
    }
}
