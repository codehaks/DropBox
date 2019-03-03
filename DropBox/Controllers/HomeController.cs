﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DropBox.Common;
using Microsoft.AspNetCore.Hosting;
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

        [HttpPost]

        public IActionResult Upload(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    _db.Context.FileStorage.Upload(file.FileName, file.FileName, stream);
                }
            }
            //return RedirectToAction(nameof(Index));
            return Ok("Done");
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task<IActionResult> Upload1(IFormFile file, [FromServices] IHostingEnvironment env)
        {
            byte[] buffer = new byte[16 * 1024];
            long totalBytes = file.Length;
            //Progress = 0;

            using (FileStream output = System.IO.File.Create(env.ContentRootPath + "/files/" + file.FileName))
            {
                using (Stream input = file.OpenReadStream())
                {
                    long totalReadBytes = 0;
                    int readBytes;

                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalReadBytes += readBytes;
                        //Progress = (int)((float)totalReadBytes / (float)totalBytes * 100.0);

                        await Task.Delay(100); // It is only to make the process slower
                    }
                }
            }
            return View();
        }

        public IActionResult Details(string id)
        {
            var model = _db.Context.FileStorage.FindById(id);
            var memory = new MemoryStream();

            model.CopyTo(memory);
            memory.Position = 0;

            return new FileStreamResult(memory,model.MimeType);
        }
    }
}