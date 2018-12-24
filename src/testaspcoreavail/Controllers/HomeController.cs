using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testaspcoreavail.Models;

namespace testaspcoreavail.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult PrintArabicLetter(int code)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath;
                _logger.LogCritical("Root Path is: " + rootPath);
                var webRoot = _hostingEnvironment.WebRootPath;

                string src = Path.Combine(webRoot, "PdfLetters", "LetterHead.pdf");

                string fileName = "test.pdf";

                string dest = Path.Combine(webRoot, "PdfLetters", fileName);


                var regularFont = Path.Combine(webRoot, "fonts", "trado.TTF");
                var boldFont = Path.Combine(webRoot, "fonts", "tradbdo.TTF");
                string title = Constants.IDENTITFICATION_TITLE;
                string body = Constants.IDENTIFICATION_BODY_TEXT;

                PdfDocumentWriter pdfDocument = new PdfDocumentWriter();
                string resultPath = pdfDocument.WriteArabicIdentification(title, body, src, dest, regularFont, boldFont);
                var stream = new FileStream(resultPath, FileMode.Open); 

                return new FileStreamResult(stream, "application/pdf");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View();
        }
        
    }
}
