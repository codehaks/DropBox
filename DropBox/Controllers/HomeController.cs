using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DropBox.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DropBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteDbContext _db;

        public HomeController(LiteDbContext db)
        {
            _db = db;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var model = _db.Context.FileStorage.FindAll();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string imageFile)
        {
            //remove "data:image/png;base64,"
            var base64 = imageFile.Split(",")[1];

            byte[] data = Convert.FromBase64String(base64);
            using (var stream = new MemoryStream(data, 0, data.Length))
            {

                stream.Position = 0;
                _db.Context.FileStorage.Upload("pic.png", "pic.png", stream);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            var model = _db.Context.FileStorage.FindById(id);
            var memory = new MemoryStream();

            model.CopyTo(memory);
            memory.Position = 0;

            return new FileStreamResult(memory, model.MimeType);
        }
    }
}