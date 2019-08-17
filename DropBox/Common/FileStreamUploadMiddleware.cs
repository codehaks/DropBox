//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Internal;
//using Microsoft.Extensions.Primitives;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace DropBox.Common
//{
//    public class FileStreamUploadMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public FileStreamUploadMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            if (context.Request.ContentType != null)
//            {
//                if (context.Request.Headers.Any(x => x.Key == "Content-Disposition"))
//                {
//                    var v = ContentDispositionHeaderValue.Parse(
//                        new StringSegment(context.Request.Headers.First(x => x.Key == "Content-Disposition").Value));
//                    if (HasFileContentDisposition(v))
//                    {
//                        using (var memoryStream = new MemoryStream())
//                        {
//                            context.Request.Body.CopyTo(memoryStream);
//                            var length = memoryStream.Length;
//                            var formCollection = context.Request.Form =
//                                new FormCollection(new Dictionary<string, StringValues>(),
//                                    new FormFileCollection()
//                                        {new FormFile(memoryStream, 0, length, v.Name.Value, v.FileName.Value)});
//                        }
//                    }
//                }
//            }

//            await _next.Invoke(context);
//        }

//        private static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
//        {
//            // this part of code from  https://github.com/aspnet/Mvc/issues/7019#issuecomment-341626892
//            return contentDisposition != null
//                   && contentDisposition.DispositionType.Equals("form-data")
//                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
//                       || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
//        }
//    }
//}
