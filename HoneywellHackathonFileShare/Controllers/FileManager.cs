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
