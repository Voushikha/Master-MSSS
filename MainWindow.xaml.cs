using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace General_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //4.1 Define Dictionary<int, string> TKEY
        public static Dictionary<int, string> MasterFile = new Dictionary<int, string>();


        public MainWindow()
        {
            InitializeComponent();
        }

        public class Staff
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }


        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            // when "Open Admin Button" is clicked open Admin Window
            Admin_Window adminWindow = new Admin_Window(); // Instantiate the admin window
            adminWindow.ShowDialog();
        }
        //4.9 Open Admin GUI with Alt + A
        private void OpenAdminWindow()
        {
            var selectedRecord = lstBoxFiltered.SelectedItem as KeyValuePair<int, string>;
            Admin_Window adminWindow = new Admin_Window(MasterFile, selectedRecord);
            adminWindow.ShowDialog();
        }


        //4.2  Load Method 
        private void LoadData()
        {
            string filePath = @"D:\Diploma\Complex Data Structure\Assessment\Master MSSS\MalinStaffNamesV3.csv";
            MasterFile.Clear();

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
        }
        // 4.3 Display LstBox1
        private void DisplayData()
        {
            //Key: ID
            //Value: Staff name
            lstBox1.ItemsSource = MasterFile.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
        }



        //lOAD bUTTON
        //Load csv filt to the listBOx
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {  
            LoadData();
            DisplayData();
        }



        // 4.4 Filter nAME -Real-Time Filter by Staff Name method
       
        private void FilterByName(string filterText)
        {
            var filteredData = MasterFile.Where(kvp => kvp.Value.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                                         .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                                         .ToList();
            lstBoxFiltered.ItemsSource = filteredData;
        }

        //4.5 Filter Staff ID
        private void FilterByID(string filterText)
        {
            if (int.TryParse(filterText, out int id))
            {
                var filteredData = MasterFile.Where(kvp => kvp.Key.ToString().Contains(filterText))
                                             .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                                             .ToList();
                lstBoxFiltered.ItemsSource = filteredData;
            }
        }

        //4.6 4.7 Clear AND Focus
        private void ClearAndFocusName()
        {
            txtBoxStaffName.Clear();
            txtBoxStaffName.Focus();
        }
        private void ClearAndFocusID()
        {
            txtBoxStaffID.Clear();
            txtBoxStaffID.Focus();
        }

        //4.8Populate TextBoxes on filtered ListBox
        private void listBoxFiltered_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBoxFiltered.SelectedItem is KeyValuePair<int, string> selectedRecord)
            {
                txtBoxStaffID.Text = selectedRecord.Key.ToString();
                txtBoxStaffName.Text = selectedRecord.Value;
            }
        }
  

        //5.1



        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
         

        //    string filterTerm = txtBoxFilter.Text.ToLower();
        //    var filteredData = staffData.Where(s => s.ID.Contains(filterTerm) || s.Name.ToLower().Contains(filterTerm)).ToList();

        //    // Display the filtered data in the second ListBox
        //    lstBoxFiltered.ItemsSource = filteredData;

        //    if (!filteredData.Any())
        //    {
        //        MessageBox.Show("No matching records found.");
        //    }
        }
        // Display selected record details in TextBoxes
        private void lstBoxFiltered_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        //    if (lstBoxFiltered.SelectedItem is Staff selectedStaff)
        //    {
        //        txtBoxStaffID.Text = selectedStaff.ID;
        //        txtBoxStaffName.Text = selectedStaff.Name;
        //    }
        }

        
    }

}
        
    
