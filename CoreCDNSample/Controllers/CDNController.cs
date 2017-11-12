using CoreCDNSample.Components.CDN;
using CoreCDNSample.Components.CDN.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CoreCDNSample.Controllers
{
    public class CDNController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file1)
        {
            var ms = new MemoryStream();
            file1.CopyTo(ms);

            CDNContext context = new CDNContext(new AzureCDN("UseDevelopmentStorage=true", "corecdn"));
            context.Save(ms.ToArray(), file1.FileName, "images");

            return Index();
        }
    }
}