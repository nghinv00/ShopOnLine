using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Helper
{
    public static class Common
    {
        public static string ConvertToTitleCase(string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static MvcHtmlString FormatTimKiemNangCao(this HtmlHelper helper, string Name, string keyword)
        {
            string html = Name;

            if ((!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name)) && (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword)))
            {
                Name = Name.ToLower();
                keyword = keyword.ToLower();

                int index = Name.IndexOf(keyword, StringComparison.CurrentCulture);

                if (index >= 0)
                {
                    string Name1 = Name.Substring(0, index);

                    string keyword2 = Name.Substring(index, keyword.Length);

                    string Name2 = Name.Substring(Name1.Length + keyword2.Length);

                    html = Name1 + " <font class='font_keyword'><font class='font_keyword'>" + keyword2 + "</font></font> " + Name2;
                }
            }

            return new MvcHtmlString(html);

        }
    }
}
