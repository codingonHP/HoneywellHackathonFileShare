using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace HoneywellHackathonFileShare.Controllers
{
    //[Authorize]
    public class FileManager : ApiController
    {
        [HttpPost]
        [Route("api/upload")]
        public IHttpActionResult UploadFile(HttpPostedFileBase selectedFile, string password)
        {
            byte[] photoBytes = new byte[selectedFile.ContentLength];
            int randIndex = DateTime.Now.Millisecond + new Random(777777).Next();
            try
            {
                if (selectedFile.InputStream.CanRead)
                {
                    selectedFile.InputStream.Position = 0;
                    selectedFile.InputStream.Read(photoBytes, 0, selectedFile.ContentLength);
                }

                var fileName = selectedFile.FileName + "_" + randIndex;
                string targetFolder = HttpContext.Current.Server.MapPath("~/Store/Documents");
                string targetPath = Path.Combine(targetFolder, fileName);

                selectedFile.SaveAs(targetPath);

                return null;
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
    }
}
