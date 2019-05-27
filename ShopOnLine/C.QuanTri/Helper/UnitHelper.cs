using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.QuanTri.Helper
{
    public static class UnitHelper
    {
        public static MvcHtmlString UnitName(this HtmlHelper helper, int? UnitId)
        {
            qtUnitService _unit = new qtUnitService();

            return new MvcHtmlString(_unit.UnitName(UnitId));
        }

        public static MvcHtmlString UnitLink(this HtmlHelper helper, int? UnitId, int? UserId)
        {
            qtUserService _user = new qtUserService();
            string UserName = _user.UserName(UserId);

            string html = "<a href='/QuanTri/UnitManager/Create/" + UnitId + "'>" + UserName + "</a>";

            return new MvcHtmlString(html);
        }
    }
}
