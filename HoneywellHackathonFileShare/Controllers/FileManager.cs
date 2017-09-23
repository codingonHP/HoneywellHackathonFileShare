using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using HoneywellHackathonFileShare.Util;

namespace HoneywellHackathonFileShare.Controllers
{
    //[Authorize]
    public class FileManagerController : ApiController
    {
        [HttpPost]
        //[Route("api/upload")]
        public IHttpActionResult UploadFile(HttpPostedFileBase selectedFile)
        {
            DocumentManager documentManager = new DocumentManager();
            byte[] fileBytes = new byte[selectedFile.ContentLength];
            int randIndex = DateTime.Now.Millisecond + new Random(777777).Next();
            try
            {
                if (selectedFile.InputStream.CanRead)
                {
                    selectedFile.InputStream.Position = 0;
                    selectedFile.InputStream.Read(fileBytes, 0, selectedFile.ContentLength);
                }

                var fname = selectedFile.FileName;
                fileBytes = CompressionUtil.CompressFile(fileBytes, selectedFile.FileName);
                fileBytes = EncryptionUtil.EncryptFile(fileBytes);

                var fileName = fname + "_" + randIndex + ".zip";
                string targetFolder = HttpContext.Current.Server.MapPath("~/Store/Documents");

                documentManager.SaveFile(fileBytes, fileName, targetFolder);

                return Ok();
            }
            catch (Exception )
            {
                //logging 
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/download")]
        public HttpResponseMessage Download()
        {
            var stream = new MemoryStream();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };

            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = RandomFileName()
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        private string RandomFileName()
        {
            return DateTime.Now.ToLongTimeString() + ".zip";
        }

        [HttpPost]
        public IHttpActionResult CreateDirectory(string room)
        {
            var path = HttpContext.Current.Server.MapPath("~/Store/Documents/" + room + "/");
            Directory.CreateDirectory(path);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteDirectory(string room)
        {
            var path = HttpContext.Current.Server.MapPath("~/Store/Documents/" + room + "/");
            Directory.Delete(path);

            return Ok();
        }
    }
}
