using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace INDZ
{
    public partial class MainWindow : Window
    {
        private int _totalTosses = 0;
        private int _totalHeads = 0;

        private string _lastInputValue = "10";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            CoinDialog dialog = new CoinDialog(_lastInputValue, UpdateStats)
            {
                Owner = this 
            };

            dialog.ShowDialog(); 

            _lastInputValue = dialog.LastInput;
        }
        private void UpdateStats(int tossesCount, int headsCount)
        {
            _totalTosses += tossesCount;
            _totalHeads += headsCount;

            double percent = _totalTosses == 0 ? 0 : ((double)_totalHeads / _totalTosses) * 100;

            tbTotalTosses.Text = $"Сумарне число підкидань = {_totalTosses}";
            tbHeadsPercent.Text = $"Відсоток орлів = {Math.Round(percent, 2)}%";
        }
    }
}
