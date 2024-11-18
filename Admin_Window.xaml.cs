using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace General_GUI
{
    /// <summary>
    /// Interaction logic for Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    {
        private SortedDictionary<int, string> masterFile;
        private KeyValuePair<int, string> selectedRecord;

        // Constructor that accepts the Dictionary and selected record
        public Admin_Window(SortedDictionary<int, string> masterFile, KeyValuePair<int, string> selectedRecord)
        {
            InitializeComponent();
            this.masterFile = masterFile;
            this.selectedRecord = selectedRecord;

            DisplayRecords(); // Populate ListBox with existing records
            PopulateFields(selectedRecord); // Populate fields if a record is passed
        }

        // Method to display records in the ListBox
        private void DisplayRecords()
        {
            lstBoxPreviewAdmin.ItemsSource = masterFile.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
        }

        // Populate fields with selected record data
        private void PopulateFields(KeyValuePair<int, string> record)
        {
            if (record.Key != 0)
            {
                txtBoxStaffID.Text = record.Key.ToString();
                txtBoxStaffName.Text = record.Value;
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if masterFile is null before proceeding
            if (masterFile == null)
            {
                statusMessage.Text = "Data not loaded.";
                return;
            }

            string searchText = txtBoxSearch.Text.ToLower();
            var filteredRecords = masterFile
                .Where(kvp => kvp.Key.ToString().Contains(searchText) || kvp.Value.ToLower().Contains(searchText))
                .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                .ToList();

            lstBoxPreviewAdmin.ItemsSource = filteredRecords;

            // Update status message
            statusMessage.Text = filteredRecords.Any() ? "Filtered results displayed." : "No matching records found.";
        }
        // SelectionChanged event for ListBox to populate fields for editing/deleting
        private void lstBoxPreviewAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBoxPreviewAdmin.SelectedItem is string selectedText)
            {
                var parts = selectedText.Split(": ");
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    txtBoxStaffID.Text = id.ToString();
                    txtBoxStaffName.Text = parts[1];
                }
            }
        }

        // Add a new staff record with a unique ID starting with 77
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBoxStaffID.Text, out int id) && !string.IsNullOrEmpty(txtBoxStaffName.Text))
            {
                // Ensure that the ID begins with 77
                if (!txtBoxStaffID.Text.StartsWith("77"))
                {
                    statusMessage.Text = "Staff ID must start with 77.";
                    return;
                }

                if (masterFile.ContainsKey(id))
                {
                    statusMessage.Text = "Duplicate ID. Cannot add.";
                    return;
                }

                masterFile[id] = txtBoxStaffName.Text;
                statusMessage.Text = $"Record with ID {id} added successfully.";
                DisplayRecords(); // Update ListBox with new entry
                txtBoxStaffID.Clear();
                txtBoxStaffName.Clear();
            }
            else
            {
                statusMessage.Text = "Invalid ID or Name. Please check and try again.";
            }
        }

        // 5.4 Update the name of the current Staff ID
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that we have a valid ID and a non-empty name for the edit operation
            if (int.TryParse(txtBoxStaffID.Text, out int id) && !string.IsNullOrEmpty(txtBoxStaffName.Text))
            {
                // Check if the ID exists in the dictionary
                if (masterFile.ContainsKey(id))
                {
                    // Update the name for the specified ID
                    masterFile[id] = txtBoxStaffName.Text;
                    statusMessage.Text = $"Record with ID {id} updated successfully.";

                    // Refresh the ListBox to display updated records
                    DisplayRecords();

                    // Optionally, clear the text boxes after editing
                    txtBoxStaffID.Clear();
                    txtBoxStaffName.Clear();
                }
                else
                {
                    // Show an error message if the ID was not found
                    statusMessage.Text = "Record not found.";
                }
            }
            else
            {
                // Show an error message if input validation failed
                statusMessage.Text = "Invalid ID or Name. Please check and try again.";
            }
        }


        // Delete the current Staff ID and clear the text boxes
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBoxStaffID.Text, out int id))
            {
                if (masterFile.Remove(id))
                {
                    txtBoxStaffID.Clear();
                    txtBoxStaffName.Clear();
                    statusMessage.Text = $"Record with ID {id} deleted successfully.";
                    DisplayRecords(); // Update ListBox after deletion
                }
                else
                {
                    statusMessage.Text = "Record not found.";
                }
            }
            else
            {
                statusMessage.Text = "Invalid ID. Please check and try again.";
            }
        }

        // Save changes to CSV when Save button is clicked
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveToCsv();
        }

        // Method to save the dictionary to a CSV file
        private void SaveToCsv()
        {
            string filePath = @"D:\Diploma\Complex Data Structure\Assessment\Master MSSS\MalinStaffNamesV3.csv";

            try
            {
                using (var writer = new StreamWriter(filePath, false))
                {
                    foreach (var kvp in masterFile)
                    {
                        writer.WriteLine($"{kvp.Key},{kvp.Value}");
                    }
                }
                statusMessage.Text = "Changes saved to file.";
            }
            catch (Exception ex)
            {
                statusMessage.Text = "Error saving to file: " + ex.Message;
            }
        }


        // Save changes when the window is closed
        // protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        // {
        //     base.OnClosing(e);
        //     SaveToCsv();
        // }


        // Close Admin GUI on Alt + L key press
        private void AdminWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                this.Close();
            }
        }

        // Close Admin GUI and return to MainWindow
        private void btnCloseAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //FILE IO qUESTIN 8 
      


    private void MeasureSaveDataTime()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        SaveToCsv();

        stopwatch.Stop();
        Console.WriteLine($"SaveToCsv execution time: {stopwatch.ElapsedMilliseconds} ms");
        statusMessage.Text = $"SaveToCsv execution time: {stopwatch.ElapsedMilliseconds} ms";
    }

}
}