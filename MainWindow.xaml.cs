using Microsoft.Win32;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.Drawing;
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
                case 0:
                    ConvertDocToPDF(pathTextBox.Text);
                    break;
                case 1:
                    ConvertPDFToDoc(pathTextBox.Text);
                    break;
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
            WordDocument wordDocument = new WordDocument();
            IWSection section = wordDocument.AddSection();
            section.PageSetup.Margins.All = 0;
            IWParagraph firstParagraph = section.AddParagraph();

            SizeF defaultPageSize = new SizeF(wordDocument.LastSection.PageSetup.PageSize.Width,
                wordDocument.LastSection.PageSetup.PageSize.Height);

            using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfPath))
            {
                for (int i = 0; i < loadedDocument.Pages.Count; i++)
                {
                    using (var image = loadedDocument.ExportAsImage(i, defaultPageSize, false))
                    {
                        IWPicture picture = firstParagraph.AppendPicture(image);
                        picture.Width = image.Width;
                        picture.Height = image.Height;
                    }
                }
            };

            string newPDFPath = pdfPath.Split(".")[0] + ".docx";
            wordDocument.Save(newPDFPath);
            wordDocument.Dispose();
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
