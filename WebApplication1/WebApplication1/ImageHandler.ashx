<%@ WebHandler Language="C#" Class="WebApplication1.ImageHandler" %>

using System;
using System.IO;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1
{
    /// <summary>
    /// Streams files stored in FileStorage. Only serves known image types.
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!int.TryParse(context.Request.QueryString["id"], out int fileId))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var storage = new FileStorageService(server: context.Server);
            var file = storage.GetFile(fileId);

            if (file == null || string.IsNullOrEmpty(file.FilePath) || !File.Exists(file.FilePath))
            {
                context.Response.StatusCode = 404;
                return;
            }

            string contentType = file.ContentType ?? MimeMapping.GetMimeMapping(file.OriginalName);
            if (!contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 415;
                return;
            }

            context.Response.ContentType = contentType;
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(5));

            context.Response.WriteFile(file.FilePath);
        }

        public bool IsReusable => false;
    }
}

