using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;

namespace TestPDf.Helpers
{

    //public class PdfMergeService
    //{
    //    public byte[] MergePdfFiles(List<string> pdfFiles)
    //    {
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            using (var writer = new PdfWriter(memoryStream))
    //            {
    //                using (var pdfDocument = new PdfDocument(writer))
    //                {
    //                    PdfMerger pdfMerger = new PdfMerger(pdfDocument);
    //                    foreach (var file in pdfFiles)
    //                    {
    //                        using (var sourcePdf = new PdfDocument(new PdfReader(file)))
    //                        {
    //                            pdfMerger.Merge(sourcePdf, 1, sourcePdf.GetNumberOfPages());
    //                        }
    //                    }
    //                }
    //            }
    //            return memoryStream.ToArray();
    //        }
    //    }
    //}



    public class PdfMergeService
    {
        public byte[] MergeFiles(List<string> filePaths)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdfDocument = new PdfDocument(writer))
                    {
                        var document = new Document(pdfDocument);
                        PdfMerger pdfMerger = new PdfMerger(pdfDocument);

                        foreach (var filePath in filePaths)
                        {
                            var extension = Path.GetExtension(filePath).ToLower();

                            if (extension == ".pdf")
                            {
                                using (var sourcePdf = new PdfDocument(new PdfReader(filePath)))
                                {
                                    pdfMerger.Merge(sourcePdf, 1, sourcePdf.GetNumberOfPages());
                                }
                            }
                            else if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                            {
                                ImageData imageData = ImageDataFactory.Create(filePath);
                                var image = new Image(imageData);
                                document.Add(image);
                            }
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
