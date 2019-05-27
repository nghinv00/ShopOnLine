using C.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ShopOnLine.App_Code
{
    [RoutePrefix("api/UploadProduct")]
    public class UploadProductController : ApiController
    {
        [HttpPost]
        public string Preview()
        {
            string url = string.Empty;
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                string thumbs = HttpContext.Current.Request.Files.AllKeys[0];

                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files[0];

                if (httpPostedFile != null)
                {
                    // Validate the uploaded image(optional)
                   
                    // Get the complete file path
                    string file_Name = C.Core.Common.GuidUnique.getInstance().GenerateUnique() + httpPostedFile.FileName.ToLower().Replace(' ', '_');

                    // Create folder saving file upload
                    string folderUpload = Config.URL_UPLOAD_PRODUCT + DateTime.Now.Month + "_" + DateTime.Now.Year + "/";
                    if (thumbs == Config.thumbs)
                    {
                        folderUpload += "thumbs/";
                    }
                    string MapPath = HttpContext.Current.Server.MapPath("~" + folderUpload);

                    // check if folderUpload didn't exits then create folder
                    if (!Directory.Exists("" + MapPath))
                    {
                        Directory.CreateDirectory("" + MapPath);
                    }

                    var fileSavePath = Path.Combine(MapPath, file_Name);

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);

                    // set data complete 
                    url = folderUpload + file_Name;
                }
            }
            return url;
        }
    }
}
