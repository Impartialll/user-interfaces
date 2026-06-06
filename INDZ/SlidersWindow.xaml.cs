using INDZ;
using System.Windows;
using System.Windows.Controls;

namespace INDZ
{
    public partial class SlidersWindow : Window
    {
        private Task2Window _main;
        public SlidersWindow(Task2Window main) { InitializeComponent(); _main = main; }

        private void SlContainer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OriginalSource is Slider sl && _main != null)
                if (_main.FindName("pb" + sl.Tag) is ProgressBar pb) pb.Value = sl.Value;
        }

        public void Reset() { foreach (var c in slContainer.Children) if (c is Slider sl) sl.Value = 0; }
    }
}