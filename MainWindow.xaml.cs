using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace PDF_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (pathTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please select a file", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            switch (conversionDropDown.SelectedIndex)
            {
                case 0: //Convert Doc to PDF

                    break;
                default:
                    MessageBox.Show("Please select an option", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                pathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void OpenFolder(string folderPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                Arguments = folderPath.Substring(0, folderPath.LastIndexOf('\\')),
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }
    }
}
