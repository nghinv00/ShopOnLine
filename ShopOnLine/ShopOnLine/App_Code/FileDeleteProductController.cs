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
    [RoutePrefix("api/FileDeleteProduct")]
    public class FileDeleteProductController : ApiController
    {
        [HttpPost]
        public string DeleteImageProduct()
        {
            string value = "";
            string categoryImage = "";

            string OK = "OK";
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                categoryImage = HttpContext.Current.Request.Files.AllKeys[0];

                HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files[0];

                if (httpPostedFile != null)
                {
                    value = httpPostedFile.FileName;
                }
                // get key upload api 
                //List<ProductMultiUpload> listFile = (List<ProductMultiUpload>)HttpContext.Current.Session[categoryImage];

                //if (listFile == null)
                //{
                //    return OK;
                //}

                //IEnumerable<ProductMultiUpload> dsProductImageDelete = listFile.Where(x => x.value == value);
                //if (dsProductImageDelete.Count() > 0)
                //{
                //    bool check = listFile.Remove(dsProductImageDelete.FirstOrDefault());
                //    if (check)
                //        OK = "OK";
                //    OK = "FAIL";
                //}
            }

            return OK;
        }
    }


}
