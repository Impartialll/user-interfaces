using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace INDZ
{
    public partial class Task5Window : Window
    {
        private string _prevFile = null;
        private const string RegKey = @"Software\WpfTasksApp";

        public Task5Window()
        {
            InitializeComponent();
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { CheckFileExists = false };
            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;
                // Створюємо порожній файл, якщо він ще не існує
                if (!File.Exists(path)) File.WriteAllText(path, "");

                // Додаємо в список, якщо його там ще немає
                if (!lstFiles.Items.Contains(path)) lstFiles.Items.Add(path);

                lstFiles.SelectedItem = path; // Робимо поточним
            }
        }

        private void LstFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Автозбереження попереднього файла
            if (_prevFile != null && File.Exists(_prevFile))
            {
                File.WriteAllText(_prevFile, txtEditor.Text);
            }

            bool hasSelection = lstFiles.SelectedItem != null;
            txtEditor.IsEnabled = hasSelection;
            menuClose.IsEnabled = hasSelection;

            // Завантаження вибраного файла
            if (hasSelection)
            {
                _prevFile = lstFiles.SelectedItem.ToString();
                txtEditor.Text = File.ReadAllText(_prevFile);
            }
            else
            {
                txtEditor.Text = "";
                _prevFile = null;
            }
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            if (lstFiles.SelectedItem != null)
            {
                int idx = lstFiles.SelectedIndex;

                // Зберігаємо перед закриттям
                File.WriteAllText(lstFiles.SelectedItem.ToString(), txtEditor.Text);
                lstFiles.Items.RemoveAt(idx);

                // Робимо активним наступний або попередній елемент
                if (lstFiles.Items.Count > 0)
                {
                    lstFiles.SelectedIndex = idx < lstFiles.Items.Count ? idx : lstFiles.Items.Count - 1;
                }
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEditor.IsEnabled = false;
            menuClose.IsEnabled = false;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey))
                {
                    if (key != null)
                    {
                        // Відновлюємо список файлів
                        string[] files = (string[])key.GetValue("Files", new string[0]);
                        foreach (var f in files) lstFiles.Items.Add(f);

                        // БЕЗПЕЧНЕ ЧИТАННЯ (Convert врятує від InvalidCastException)
                        this.Width = Convert.ToDouble(key.GetValue("W", 600.0));
                        this.Height = Convert.ToDouble(key.GetValue("H", 400.0));
                        colList.Width = new GridLength(Convert.ToDouble(key.GetValue("Split", 200.0)));

                        lstFiles.SelectedIndex = Convert.ToInt32(key.GetValue("Sel", -1));

                        // Відновлюємо позицію курсора (якщо вона не виходить за межі тексту)
                        int caret = Convert.ToInt32(key.GetValue("Caret", 0));
                        if (caret <= txtEditor.Text.Length)
                        {
                            txtEditor.CaretIndex = caret;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_prevFile != null && File.Exists(_prevFile))
                {
                    File.WriteAllText(_prevFile, txtEditor.Text);
                }

                // Запис даних у реєстр
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegKey))
                {
                    key.SetValue("Files", lstFiles.Items.Cast<string>().ToArray());

                    // Зберігаємо числа як рядки, щоб уникнути конфліктів типів
                    key.SetValue("W", this.Width.ToString());
                    key.SetValue("H", this.Height.ToString());
                    key.SetValue("Split", colList.Width.Value.ToString());

                    key.SetValue("Sel", lstFiles.SelectedIndex);
                    key.SetValue("Caret", txtEditor.CaretIndex);
                }
            }
            catch
            {
                // Ігноруємо можливі системні заборони на запис у реєстр
            }
        }
    }
}