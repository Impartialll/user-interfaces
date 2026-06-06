using System;
using System.Windows;

namespace INDZ
{
    public partial class CoinDialog : Window
    {
        private Action<int, int> _update;
        private Random _rnd = new Random();
        public string LastInput { get; private set; }

        public CoinDialog(string input, Action<int, int> update)
        {
            InitializeComponent();
            _update = update;
            LastInput = input;
            txtTosses.Text = input;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) { txtTosses.Focus(); txtTosses.SelectAll(); }

        private bool Process()
        {
            if (int.TryParse(txtTosses.Text, out int n) && n > 0)
            {
                LastInput = txtTosses.Text;
                int h = 0;
                for (int i = 0; i < n; i++) if (_rnd.Next(2) == 1) h++;
                _update?.Invoke(n, h);
                return true;
            }
            return false;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) { if (Process()) DialogResult = true; }
        private void BtnApply_Click(object sender, RoutedEventArgs e) { if (Process()) { txtTosses.Focus(); txtTosses.SelectAll(); } }
    }
}