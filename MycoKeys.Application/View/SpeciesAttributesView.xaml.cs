using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

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
    }
}
