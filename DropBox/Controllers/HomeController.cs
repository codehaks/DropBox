using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DropBox.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DropBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteDbContext _db;
        private readonly ILogger _logger;
        public HomeController(LiteDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
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
        public IActionResult Create(IFormFile file)
        {
            if (file.Length > 0)
            {
                using(var stream=new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    _db.Context.FileStorage.Upload(file.FileName, file.FileName, stream);
                }  
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateMany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMany(IList<IFormFile> files)
        {
            var totalSize = 0L;
            var uploadTaskList = new List<Task>();
            foreach (var file in files)
            {
               var uploadTask= Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    if (file.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            file.CopyTo(stream);
                            totalSize += file.Length;
                            //Interlocked.Add(ref totalSize, file.Length);
                            stream.Position = 0;
                            _db.Context.FileStorage.Upload(file.FileName, file.FileName, stream);
                        }
                    }
                });

                uploadTaskList.Add(uploadTask);
            }

            await Task.WhenAll(uploadTaskList);
           

            //Parallel.For(0, files.Count, (index) =>
            //{
            //    var file = files[index];
            //    Thread.Sleep(3000);
            //    if (file.Length > 0)
            //    {
            //        using (var stream = new MemoryStream())
            //        {
            //            file.CopyTo(stream);
            //            totalSize += file.Length;
            //            //Interlocked.Add(ref totalSize, file.Length);
            //            stream.Position = 0;
            //            _db.Context.FileStorage.Upload(file.FileName, file.FileName, stream);
            //        }
            //    }
            //});

            _logger.LogWarning($" Total upload size : {totalSize}");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            var model = _db.Context.FileStorage.FindById(id);
            var memory = new MemoryStream();

            model.CopyTo(memory);
            memory.Position = 0;

            return new FileStreamResult(memory,model.MimeType);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var fileInfo= _db.Context.FileStorage.FindById(id);
            return View(fileInfo);
        }

        [HttpPost]
        public IActionResult DeleteFile(string id)
        {
            //var fileInfo = _db.Context.FileStorage.FindById(id);
            _db.Context.FileStorage.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}