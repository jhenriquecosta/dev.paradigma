using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Works.Application.Configuration;
using Works.AspNetCore.Mvc.Controllers;
using Works.Configuration;


namespace Works.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : WorksController
    {
        private IAppFolders _appFolders;

        public UploadFileController(IAppFolders folders)
        {
            this._appFolders = folders;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public void Save(IList<IFormFile> UploadFiles)
        {
            long size = 0;
            try
            {
                foreach (var file in UploadFiles)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = Path.Combine(_appFolders.Upload, filename);
                    size += (int)file.Length;
                    //FileHelper.DeleteIfExists(filename);
                    using FileStream fs = System.IO.File.Create(filename);
                    file.CopyTo(fs);
                    fs.Flush();

                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
        [HttpPost("[action]")]
        [AllowAnonymous]
        public void Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                var filename = Path.Combine(_appFolders.Upload, $@"\{UploadFiles[0].FileName}");
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
    }

}
