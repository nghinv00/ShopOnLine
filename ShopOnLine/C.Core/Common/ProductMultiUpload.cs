using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace C.Core.Common
{
    public class ProductMultiUpload
    {
        public string key { get; set; }
        public string value { get; set; }
        public HttpFileCollection httpFileCollection { get; set; }

    }
}
