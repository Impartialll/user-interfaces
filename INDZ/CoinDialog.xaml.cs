using System;
using System.Windows;

namespace INDZ
{
    public partial class CoinDialog : Window
    {
        private Action<int, int> _updateStatsCallback;
        private Random _random = new Random();

        public string LastInput { get; private set; }

        public CoinDialog(string lastInputValue, Action<int, int> updateStatsCallback)
        {
            InitializeComponent();
            _updateStatsCallback = updateStatsCallback;
            LastInput = lastInputValue;
            txtTosses.Text = lastInputValue; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTosses.Focus();
            txtTosses.SelectAll();
        }

        private bool ProcessTosses()
        {
            if (int.TryParse(txtTosses.Text, out int n) && n > 0)
            {
                LastInput = txtTosses.Text; 
                int headsCount = 0;

                for (int i = 0; i < n; i++)
                {
                    if (_random.Next(2) == 1)
                    {
                        headsCount++;
                    }
                }

                _updateStatsCallback?.Invoke(n, headsCount);
                return true;
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть ціле додатне число.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTosses.Focus();
                txtTosses.SelectAll();
                return false;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessTosses())
            {
                this.DialogResult = true; 
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessTosses())
            {
                txtTosses.Focus();
                txtTosses.SelectAll();
            }
        }
    }
}