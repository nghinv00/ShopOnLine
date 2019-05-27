using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace C.DanhMuc.Helper
{
    public static class SexHelper
    {
        public static MvcHtmlString SexName(this HtmlHelper helper , int? SexId)
        {
            shSexService _sex = new shSexService();
            return new MvcHtmlString(_sex.SexName(SexId));
        }


    }
}
