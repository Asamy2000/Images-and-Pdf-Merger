using Microsoft.AspNetCore.Mvc;
using TestPDf.Helpers;

namespace TestPDf.Controllers
{
    public class PdfController : Controller
    {
        private readonly PdfMergeService _pdfMergeService;

        public PdfController(PdfMergeService pdfMergeService)
        {
            _pdfMergeService = pdfMergeService;
        }

        [HttpGet]
        public IActionResult Merge()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Merge(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ViewBag.Message = "Please select at least one file.";
                return View();
            }

            var filePaths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(Path.GetTempPath(), formFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    filePaths.Add(filePath);
                }
            }

            var mergedFileBytes = _pdfMergeService.MergeFiles(filePaths);
            var mergedFileStream = new MemoryStream(mergedFileBytes);

            return File(mergedFileStream, "application/pdf", "merged.pdf");
        }
    }
}
