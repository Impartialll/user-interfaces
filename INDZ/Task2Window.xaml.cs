using INDZ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace INDZ
{
    public partial class Task2Window : Window
    {
        private SlidersWindow _sliders;
        public Task2Window() => InitializeComponent();

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            if (_sliders == null || !_sliders.IsLoaded) { _sliders = new SlidersWindow(this); _sliders.Show(); }
            else _sliders.Focus();
        }

        private void PbContainer_Click(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = e.OriginalSource as DependencyObject;
            while (obj != null && !(obj is ProgressBar)) obj = VisualTreeHelper.GetParent(obj);
            if (obj is ProgressBar pb && _sliders != null && _sliders.IsLoaded)
            {
                if (_sliders.FindName("sl" + pb.Tag) is Slider sl) sl.IsEnabled = !sl.IsEnabled;
            }
        }

        private void BtnDefault_Click(object sender, RoutedEventArgs e)
        {
            foreach (var c in pbContainer.Children) if (c is ProgressBar pb) pb.Value = 0;
            _sliders?.Reset();
        }
    }
}