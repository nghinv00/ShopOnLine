using C.Core.Common;
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
    public static class SectionHelper
    {
        public static MvcHtmlString SectionName(this HtmlHelper helper, string SectionGuid)
        {
            shSectionService _section = new shSectionService();

            shProductSet section = _section.FindByKey(SectionGuid);

            if (section == null)
            {
                section = new shProductSet();
            }

            return new MvcHtmlString(section.SectionName);
        }

        public static MvcHtmlString SectionName_SizeName(this HtmlHelper helper, string SectionGuid, string SizeGuid)
        {
            string html = string.Empty;
            shSectionService _section = new shSectionService();
            shProductSet section = _section.FindByKey(SectionGuid);

            if (section != null)
            {
                html += section.SectionName;
            }

            shSizeService _size = new shSizeService();
            shSetSize size = _size.FindByKey(SizeGuid);

            if (size != null)
            {
                html += "    " + size.SizeName + "    " + size.Stuff;
            }
            return new MvcHtmlString(html);
        }
    }
}
