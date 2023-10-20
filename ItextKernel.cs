using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;

public static class ItextKernel
{
	public static void Executar(string inputFile)
    {
        // Caminho do arquivo PDF de entrada
        //string inputFile = "input.pdf";

        // Diretório de saída para os PDFs separados
        string outputDirectory = "output/pages";
        string outputDirectorySave = "output/new";

        // Cria o diretório de saída se ele não existir
        Directory.CreateDirectory(outputDirectory);
        Directory.CreateDirectory(outputDirectorySave);
        
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(inputFile));

        double total = 0.0;
        double max = 25.0;
        int initialPage = 0;
        int finalPage = 0;
        var documentInfo = pdfDoc.GetDocumentInfo();

        Console.WriteLine($"\nDados Documento");
        Console.WriteLine($"Title: {documentInfo.GetTitle()}");
        GetDocumentSize(inputFile);
        Console.WriteLine($"==================================================");

        string documentPath = string.Empty;

        for (int pageNumber = 1; pageNumber <= pdfDoc.GetNumberOfPages(); pageNumber++)
        {
            documentPath = Path.Combine(outputDirectory, $"page_{pageNumber}.pdf");
            
            PdfDocument newPdfDoc = new PdfDocument(new PdfWriter(documentPath));
            if(initialPage == 0)
            {
                initialPage = pageNumber;
            }

            pdfDoc.CopyPagesTo(pageNumber, pageNumber, newPdfDoc);
            newPdfDoc.Close();

            var sizePage = GetDocumentSize(documentPath);
            total += sizePage;
            Console.WriteLine($"Página: {pageNumber} - Tamanho: {sizePage} - Total: {total}\n\n");
            
            bool endDocument = pageNumber == pdfDoc.GetNumberOfPages();
            bool closeDocument = total > max || endDocument;
            
            if(closeDocument)
            {
                if(endDocument)
                {
                    finalPage = pageNumber;
                }
                else 
                {
                    finalPage = pageNumber-1;
                }

                var finalDocumentPath = Path.Combine(outputDirectorySave, $"page_{initialPage}_to_{finalPage}.pdf");
                var newPdfDocFinal = new PdfDocument(new PdfWriter(finalDocumentPath));
                
                pdfDoc.CopyPagesTo(initialPage, finalPage, newPdfDocFinal);
                Console.WriteLine($"Novo Arquivo : {finalDocumentPath} copiado \n\n");
                
                newPdfDocFinal.Close();
                initialPage = pageNumber;
                total = 0;
            }
        }

        Helper.DeleteFolder(outputDirectory);

        pdfDoc.Close();
        Console.WriteLine("Arquivo PDF separado com sucesso.");
    }

    static double GetDocumentSize(string inputFile)
    {
        // Use a classe FileInfo para obter informações sobre o arquivo
        FileInfo fileInfo = new FileInfo(inputFile);

        // Obtenha o tamanho do arquivo em bytes
        long fileSizeInBytes = fileInfo.Length;

        // Converta o tamanho do arquivo em bytes para megabytes (1 MB = 1,048,576 bytes)
        double fileSizeInMegabytes = (double)fileSizeInBytes / 1_048_576;

        return fileSizeInMegabytes;
    }

    static void GetPageSize(PdfPage page, int numberPage)
    {
         // Acesse a página desejada
        //PdfPage page = pdfDoc.GetPage(pageNumber);

        // Crie um objeto PdfCanvas para a página
        PdfCanvas pdfCanvas = new PdfCanvas(page);

        // Calcule o tamanho da página em bytes
        byte[] pageContent = pdfCanvas.GetContentStream().GetBytes();

        // Converta o tamanho da página em bytes para megabytes (1 MB = 1,048,576 bytes)
        double pageSizeInMegabytes = (double)pageContent.Length / 1048576.0;

        Console.WriteLine($"O tamanho da página {numberPage} ({page.GetPdfObject().Size}) é aproximadamente {pageSizeInMegabytes:F2} MB.");

    }
}