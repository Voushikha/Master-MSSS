using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace General_GUI
{
    public partial class MainWindow : Window
    {
        // 4.1 Define a public, static Dictionary<int, string> named MasterFile
        public static Dictionary<int, string> MasterFile = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // 4.2 Method to load data from CSV file into Dictionary
        private void LoadData()
        {
            string filePath = @"D:\Diploma\Complex Data Structure\Assessment\Master MSSS\MalinStaffNamesV3.csv";
            MasterFile.Clear();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values.Length == 2 && int.TryParse(values[0], out int id))
                        {
                            MasterFile[id] = values[1];
                        }
                    }
                }
                DisplayData();
                statusMessage.Text = "Data loaded successfully.";
            }
            catch (Exception ex)
            {
                statusMessage.Text = $"Error loading data: {ex.Message}";
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // 4.3 Method to display Dictionary data in a read-only ListBox
        private void DisplayData()
        {
            lstBox1.ItemsSource = MasterFile.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
        }

        // 4.4 Method for real-time filtering by Name
        private void txtBoxFilterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterByName(txtBoxFilterName.Text);
        }
       

        private void FilterByName(string filterText)
        {
            var filteredData = MasterFile
                .Where(kvp => kvp.Value.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                .ToList();
            lstBoxFiltered.ItemsSource = filteredData;
        }

        // 4.5 Method for real-time filtering by ID
        private void txtBoxStaffID_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterByID(txtBoxStaffID.Text);
        }

        private void FilterByID(string filterText)
        {
            if (int.TryParse(filterText, out int id))
            {
                var filteredData = MasterFile
                    .Where(kvp => kvp.Key.ToString().Contains(filterText))
                    .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                    .ToList();
                lstBoxFiltered.ItemsSource = filteredData;
            }
        }
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterTerm = txtBoxFilterName.Text.ToLower();

            var filteredData = MasterFile
                .Where(kvp => kvp.Key.ToString().Contains(filterTerm) || kvp.Value.ToLower().Contains(filterTerm))
                .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                .ToList();

            lstBoxFiltered.ItemsSource = filteredData;

            if (!filteredData.Any())
            {
                statusMessage.Text = "No matching records found.";
            }
            else
            {
                statusMessage.Text = "Filtered results displayed.";
            }
        }



        // 4.6 Clear and focus on Name TextBox
        private void ClearAndFocusName()
        {
            txtBoxFilterName.Clear();
            txtBoxFilterName.Focus();
        }

        // 4.7 Clear and focus on ID TextBox
        private void ClearAndFocusID()
        {
            txtBoxStaffID.Clear();
            txtBoxStaffID.Focus();
        }

        // 4.8 Populate text boxes when selecting an item from filtered ListBox
        private void lstBoxFiltered_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBoxFiltered.SelectedItem is string selectedText)
            {
                var parts = selectedText.Split(": ");
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    txtBoxStaffID.Text = id.ToString();
                    txtBoxStaffName.Text = parts[1];
                }
            }
        }

        // 4.9 Open Admin GUI with Alt + A and pass selected record
        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecord = lstBoxFiltered.SelectedItem as string;
            if (selectedRecord != null)
            {
                var parts = selectedRecord.Split(": ");
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    Admin_Window adminWindow = new Admin_Window(MasterFile, new KeyValuePair<int, string>(id, parts[1]));
                    adminWindow.ShowDialog();
                }
            }
            else
            {
                statusMessage.Text = "No record selected.";
            }
        }

        // Keyboard shortcuts for 4.6, 4.7, and 4.9
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ClearAndFocusName();
            }
            else if (e.Key == System.Windows.Input.Key.I && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ClearAndFocusID();
            }
            else if (e.Key == System.Windows.Input.Key.A && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                btnAdmin_Click(sender, e);
            }
        }
    }
}
