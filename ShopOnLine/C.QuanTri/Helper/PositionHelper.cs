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
    public static class PositionHelper
    {
        public static MvcHtmlString PositionName(this HtmlHelper helper, int? PositionId)
        {
            qtPositionService _position = new qtPositionService();
            
            return new MvcHtmlString(_position.PositionName(PositionId));
        }
    }
}
