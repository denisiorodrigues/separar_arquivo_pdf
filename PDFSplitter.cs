using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace WebsiteDemos
{
    public static class PDFSplitter
    {
        //private static string ORIG = "/uploads/split.pdf";
        
        public static void Executar(string inputFile)
        {
            Directory.CreateDirectory("itext");
            string currentDirectory = ".\\";//Directory.GetCurrentDirectory();
            var pathComplete = Path.Combine(currentDirectory, "itext", inputFile);

            int maxPageCount = 2; // create a new PDF per 2 pages from the original file
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read)));
            IList <PdfDocument> splitDocuments = new CustomPdfSplitter(pdfDocument).SplitByPageCount(maxPageCount);

            foreach (PdfDocument doc in splitDocuments)
            {
                doc.Close();
            }
            pdfDocument.Close();
        }
    }

    public class CustomPdfSplitter : PdfSplitter
    {
        int _partNumber = 1;

        public CustomPdfSplitter(PdfDocument pdfDocument) : base(pdfDocument)
        {
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            try
            {
                return new PdfWriter($"itext/splitDocument_{ _partNumber++ }.pdf");
            }
            catch (FileNotFoundException)
            {
                throw new SystemException();
            }
        }
    }
}