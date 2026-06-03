using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnGoToPage1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Page1());
        }

        private void BtnGoToPage2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Page2());
        }

        private void BtnGoToDockPanel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DockPanelDemo());
        }

        private void BtnGoToGrid_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new GridDemo());
        }

        private void BtnGoToCanvas_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CanvasDemo());
        }

        private void BtnGoToWrapPanel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WrapPanelDemo());
        }

        private void BtnGoToStackPanel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new StackPanelDemo());
        }
    }
}
