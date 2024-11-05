using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace General_GUI
{
    /// <summary>
    /// Interaction logic for Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    { //4.1 Define Dictionary<int, string> TKEY
        private Dictionary<int, string> masterFile;
        private KeyValuePair<int, string> selectedRecord;

        // Constructor that accepts the Dictionary and selected record
        public Admin_Window(Dictionary<int, string> masterFile, KeyValuePair<int, string> selectedRecord)
        {
            InitializeComponent();
            this.masterFile = masterFile;
            this.selectedRecord = selectedRecord;

            // If a record is selected, populate the text boxes with its data
            if (selectedRecord.Key != 0)
            {
                txtBoxStaffID.Text = selectedRecord.Key.ToString();
                txtBoxStaffName.Text = selectedRecord.Value;
            }
        }

        public Admin_Window()
        {
            InitializeComponent();
        }

        private void btnCloseAdmin_Click(object sender, RoutedEventArgs e)
        {
            // when " Close Admin Button" is clicked open Main Window
            MainWindow adminWindow = new MainWindow(); // Instantiate the admin window
            adminWindow.ShowDialog();
        }

        //5.2 Receive method from general gui
        public void PopulateFields(KeyValuePair<int, string> selectedRecord)
        {
            if (selectedRecord.Key != 0)
            {
                txtBoxStaffID.Text = selectedRecord.Key.ToString();
                txtBoxStaffName.Text = selectedRecord.Value;
            }
        }
        #region ADD METHOD
        //5.3 Add NEW STAFF METHOD
        private void AddRecord(int id, string name)
        {
            if (masterFile.ContainsKey(id))
            {
                statusMessage.Text = "Duplicate ID. Cannot add.";
                return;
            }

            masterFile[id] = name;
            statusMessage.Text = "Record added successfully.";
        }
        //button add click 
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBoxStaffID.Text, out int id) && !string.IsNullOrEmpty(txtBoxStaffName.Text))
            {
                if (masterFile.ContainsKey(id))
                {
                    statusMessage.Text = "Duplicate ID. Cannot add.";
                    return;
                }

                masterFile[id] = txtBoxStaffName.Text;
                statusMessage.Text = "Record added successfully.";
            }
            else
            {
                statusMessage.Text = "Invalid ID or Name. Please check and try again.";
            }
        }
        #endregion
        //5.4 uPDATE

        private void UpdateRecord(int id, string newName)
        {
            if (masterFile.ContainsKey(id))
            {
                masterFile[id] = newName;
                statusMessage.Text = "Record updated successfully.";
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtBoxStaffID.Text, out int id) && !string.IsNullOrEmpty(txtBoxStaffName.Text))
            {
                if (masterFile.ContainsKey(id))
                {
                    masterFile[id] = txtBoxStaffName.Text;
                    statusMessage.Text = "Record updated successfully.";
                }
                else
                {
                    statusMessage.Text = "Record not found.";
                }
            }
            else
            {
                statusMessage.Text = "Invalid ID or Name. Please check and try again.";
            }
        }

        //dELETE mETHOD

        private void DeleteRecord(int id)
        {
            if (masterFile.Remove(id))
            {
                txtBoxStaffID.Clear();
                txtBoxStaffName.Clear();
                statusMessage.Text = "Record deleted successfully.";
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (int.TryParse(txtBoxStaffID.Text, out int id))
            {
                if (masterFile.Remove(id))
                {
                    txtBoxStaffID.Clear();
                    txtBoxStaffName.Clear();
                    statusMessage.Text = "Record deleted successfully.";
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

        private void SaveToCsv()
        {
            using (var writer = new StreamWriter(@"D:\Diploma\Complex Data Structure\Assessment\Master MSSS\MalinStaffNamesV3.csv"))
            {
                foreach (var kvp in masterFile)
                {
                    writer.WriteLine($"{kvp.Key},{kvp.Value}");
                }
            }
            statusMessage.Text = "Changes saved to file.";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\Diploma\Complex Data Structure\Assessment\Master MSSS\MalinStaffNamesV3.csv";

            try
            {
                using (var writer = new StreamWriter(filePath))
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
    }

}
