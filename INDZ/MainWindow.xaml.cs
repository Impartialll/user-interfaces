using System.Windows;

namespace INDZ
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();
        private void Btn1_Click(object sender, RoutedEventArgs e) => new Task1Window().Show();
        private void Btn2_Click(object sender, RoutedEventArgs e) => new Task2Window().Show();
        private void Btn3_Click(object sender, RoutedEventArgs e) => new Task3Window().Show();
        private void Btn4_Click(object sender, RoutedEventArgs e) => new Task4Window().Show();
        private void Btn5_Click(object sender, RoutedEventArgs e) => new Task5Window().Show();
    }
}