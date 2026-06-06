using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace INDZ
{
    public partial class Task4Window : Window
    {
        private DispatcherTimer _timer;
        private double _time = 0;
        private Rectangle _square;
        private string _file = "top5.txt";
        private List<double> _topScores = new List<double>();

        public Task4Window()
        {
            InitializeComponent();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += (s, e) => { _time += 0.1; txtTime.Text = _time.ToString("F1"); };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(_file))
            {
                _topScores = File.ReadAllLines(_file).Select(double.Parse).ToList();
                UpdateList();
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (btnStart.Content.ToString() == "Старт")
            {
                btnStart.Content = "Стоп";
                _time = 0; txtTime.Text = "0"; txtPrecision.Text = "0"; txtResult.Text = "0";
                GameCanvas.Children.Clear();

                _square = new Rectangle { Width = 30, Height = 30, Fill = Brushes.Red };
                _square.MouseLeftButtonDown += (s, args) => DragDrop.DoDragDrop(_square, _square, DragDropEffects.Move);

                var r = new Random();
                int corner = r.Next(4);
                double maxW = GameCanvas.ActualWidth - 30, maxH = GameCanvas.ActualHeight - 30;
                Canvas.SetLeft(_square, corner % 2 == 0 ? 0 : maxW);
                Canvas.SetTop(_square, corner < 2 ? 0 : maxH);

                GameCanvas.Children.Add(_square);
                _timer.Start();
            }
            else
            {
                btnStart.Content = "Старт";
                _timer.Stop();
                CalculateResult();
            }
        }

        private void GameCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Rectangle)) is Rectangle sq)
            {
                Point p = e.GetPosition(GameCanvas);
                Canvas.SetLeft(sq, p.X - 15); Canvas.SetTop(sq, p.Y - 15);
            }
        }

        private void CalculateResult()
        {
            if (_square == null || !GameCanvas.Children.Contains(_square)) return;

            double cx = GameCanvas.ActualWidth / 2 - 15;
            double cy = GameCanvas.ActualHeight / 2 - 15;
            double sqX = Canvas.GetLeft(_square), sqY = Canvas.GetTop(_square);

            double dx = Math.Abs(cx - sqX), dy = Math.Abs(cy - sqY);
            txtPrecision.Text = $"X: {dx:F1}, Y: {dy:F1}";

            double res = (_time + 1) * (dx + dy);
            txtResult.Text = res.ToString("F2");

            MessageBox.Show(res < 50 ? "Ви виграли" : "Ви програли");

            _topScores.Add(res);
            _topScores = _topScores.OrderBy(x => x).Take(5).ToList();
            File.WriteAllLines(_file, _topScores.Select(x => x.ToString()));
            UpdateList();
        }

        private void UpdateList() { lstTop5.Items.Clear(); foreach (var s in _topScores) lstTop5.Items.Add(s.ToString("F2")); }
    }
}