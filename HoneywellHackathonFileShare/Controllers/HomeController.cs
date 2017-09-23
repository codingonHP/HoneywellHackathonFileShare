using System;
using System.Web;
using System.Web.Mvc;
using HoneywellHackathonFileShare.Util;

namespace HoneywellHackathonFileShare.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase selectedFile)
        {
            return View();
        }

        public JsonResult UploadFile(HttpPostedFileBase selectedFile)
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

                var fileName = fname + "_" + randIndex + ".zip";
                string targetFolder = System.Web.HttpContext.Current.Server.MapPath("~/Store/Documents");
                string tempFolder = System.Web.HttpContext.Current.Server.MapPath("~/Store/tempstore");

                fileName = documentManager.SaveFile(fileBytes, fileName, targetFolder, tempFolder, targetFolder);

                return Json(new { room = "default", fileName = "http://localhost:1782/Store/tempstore" + "/" + fileName });

            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}
