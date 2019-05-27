using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace C.Customer.Helper
{
    public static class AboutHelper
    {

        public static MvcHtmlString AboutTitle(this HtmlHelper helper, string AboutGuid, string AboutTitle)
        {
            string html = string.Empty;

            html = "<a href='javascript:void(0)' onclick=HienThiBaiViet('" + AboutGuid + "')> " +  AboutTitle + "</a>";

            return new MvcHtmlString(html);
        }
    }
}
