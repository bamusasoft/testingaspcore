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
                var pdfFolder = string.Empty;
                if (_hostingEnvironment.IsDevelopment())
                {
                    pdfFolder = _hostingEnvironment.ContentRootPath;
                }
                else 
                {
                    var homePath = Environment.GetEnvironmentVariable("Home");
                    _logger.LogCritical("Home path is: " + homePath);
                    pdfFolder = Path.Combine(homePath, "data", "PdfLetters");
                    _logger.LogCritical("Pdf Folder path is: " + pdfFolder);

                }

                var webRoot = _hostingEnvironment.WebRootPath;

                string src = Path.Combine(webRoot, "waqfletter", "LetterHead.pdf");

                string dest = string.Empty;
                string fileName = "test.pdf";
                if (_hostingEnvironment.IsDevelopment())
                {
                    dest = Path.Combine(pdfFolder, "PdfLetters", fileName);
                }
                else 
                {
                    dest = Path.Combine(pdfFolder, fileName);
                    _logger.LogCritical("Pdf Folder Path: " + pdfFolder);
                }



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
                _logger.LogError(ex.StackTrace);
            }
            return View();
        }
        
    }
}
