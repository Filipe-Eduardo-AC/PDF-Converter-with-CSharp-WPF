using Microsoft.Win32;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;
using System.IO;
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
                    ConvertDocToPDF(pathTextBox.Text);
                    break;
                case 1:
                //To do convert PDF to DOC
                case 2:
                    ConvertPNGToPDF(pathTextBox.Text);
                    break;
                default:
                    MessageBox.Show("Please select an option", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
            }

            OpenFolder(pathTextBox.Text);
        }

        private void ConvertDocToPDF(string docPath)
        {
            WordDocument wordDocument = new WordDocument(docPath, FormatType.Automatic);
            DocToPDFConverter converter = new DocToPDFConverter();
            PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);

            string newPDFPath = docPath.Split('.')[0] + ".pdf";
            pdfDocument.Save(newPDFPath);

            pdfDocument.Close(true);
            wordDocument.Close();
        }

        private void ConvertPDFToDoc(string pdfPath)
        {

        }

        private void ConvertPNGToPDF(string pngPath)
        {
            PdfDocument pdfDoc = new PdfDocument();
            PdfImage pdfImage = PdfImage.FromStream(new FileStream(pngPath, FileMode.Open));
            PdfPage pdfPage = new PdfPage();
            PdfSection pdfSection = pdfDoc.Sections.Add();
            pdfSection.Pages.Insert(0, pdfPage);
            pdfPage.Graphics.DrawImage(pdfImage, 0, 0);

            string newPNGPath = pngPath.Split(".")[0] + ".pdf";
            pdfDoc.Save(newPNGPath);

            pdfDoc.Close(true);
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
