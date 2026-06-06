using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace INDZ
{
    public partial class Task3Window : Window
    {
        private Random _rnd = new Random();
        public Task3Window() => InitializeComponent();

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            var tb = new TextBlock
            {
                Text = ((char)_rnd.Next(65, 91)).ToString(),
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                AllowDrop = true
            };
            tb.MouseLeftButtonDown += (s, args) => DragDrop.DoDragDrop(tb, tb, DragDropEffects.Move);
            tb.Drop += Tb_Drop;
            Canvas.SetLeft(tb, _rnd.NextDouble() * Math.Max(0, MainCanvas.ActualWidth - 30));
            Canvas.SetTop(tb, _rnd.NextDouble() * Math.Max(0, MainCanvas.ActualHeight - 30));
            MainCanvas.Children.Add(tb);
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TextBlock)) is TextBlock tb)
            {
                Point p = e.GetPosition(MainCanvas);
                Canvas.SetLeft(tb, p.X); Canvas.SetTop(tb, p.Y);
            }
        }

        private void Tb_Drop(object sender, DragEventArgs e)
        {
            if (sender is TextBlock target && e.Data.GetData(typeof(TextBlock)) is TextBlock source && target != source)
            {
                target.Text += source.Text;
                MainCanvas.Children.Remove(source);
                e.Handled = true;
            }
        }

        private void Trash_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TextBlock)) is TextBlock tb) { MainCanvas.Children.Remove(tb); e.Handled = true; }
        }

        private void BtnTrash_Click(object sender, RoutedEventArgs e) => MainCanvas.Children.Clear();
        private void MenuClear_Click(object sender, RoutedEventArgs e) => MainCanvas.Children.Clear();
        private void MenuExit_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}