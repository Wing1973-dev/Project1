using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IVMElectro.Services.Directories;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for StatorsDirectory.xaml
    /// </summary>
    public partial class StatorsDirectory : Window {
        UserControlStatorImage StatorImage1 { get; set; }
        UserControlStatorImage StatorImage2 { get; set; }
        UserControlStatorImage StatorImage3 { get; set; }
        UserControlStatorImage StatorImage4 { get; set; }
        public StatorsDirectory(UserControlStatorImage statorImage1, UserControlStatorImage statorImage2, UserControlStatorImage statorImage3, UserControlStatorImage statorImage4) {
            StatorImage1 = statorImage1; StatorImage2 = statorImage2; StatorImage3 = statorImage3; StatorImage4 = statorImage4;
            InitializeComponent();
        }

        private void btnImage_Checked(object sender, RoutedEventArgs e) {
            switch (((RadioButton)sender).Name) {
                case "btnImage1": {
                        if (ContentGrid.Children.Count > 1) ContentGrid.Children.RemoveAt(ContentGrid.Children.Count - 1);
                        ContentGrid.Children.Add(StatorImage1);
                        Grid.SetColumn(StatorImage1, 1);
                    } break;
                case "btnImage2": {
                        if (ContentGrid.Children.Count > 1) ContentGrid.Children.RemoveAt(ContentGrid.Children.Count - 1);
                        ContentGrid.Children.Add(StatorImage2);
                        Grid.SetColumn(StatorImage2, 1);
                    } break;
                case "btnImage3": {
                        if (ContentGrid.Children.Count > 1) ContentGrid.Children.RemoveAt(ContentGrid.Children.Count - 1);
                        ContentGrid.Children.Add(StatorImage3);
                        Grid.SetColumn(StatorImage3, 1);
                    } break;
                case "btnImage4": {
                        if (ContentGrid.Children.Count > 1) ContentGrid.Children.RemoveAt(ContentGrid.Children.Count - 1);
                        ContentGrid.Children.Add(StatorImage4);
                        Grid.SetColumn(StatorImage4, 1);
                    } break;
            }
        }
    }
}
