using INDZ;
using System;
using System.Windows;

namespace INDZ
{
    public partial class Task1Window : Window
    {
        private int _total = 0, _heads = 0;
        private string _lastInput = "10";

        public Task1Window() => InitializeComponent();

        private void BtnToss_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CoinDialog(_lastInput, UpdateStats) { Owner = this };
            dlg.ShowDialog();
            _lastInput = dlg.LastInput;
        }

        private void UpdateStats(int tosses, int heads)
        {
            _total += tosses;
            _heads += heads;
            double p = _total == 0 ? 0 : ((double)_heads / _total) * 100;
            tbTotal.Text = $"Сумарне число підкидань = {_total}";
            tbPercent.Text = $"Відсоток орлів = {Math.Round(p, 2)}%";
        }
    }
}