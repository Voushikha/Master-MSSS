using System;
using System.Collections.Generic;
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
    {
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
        //5.3 Add NEW STAFF METHOD
        private void AddRecord(int id, string name)
        {
            if (MasterFile.ContainsKey(id))
            {
                statusMessage.Text = "Duplicate ID. Cannot add.";
                return;
            }

            MasterFile[id] = name;
            statusMessage.Text = "Record added successfully.";
        }
        //5.4
    }
}
