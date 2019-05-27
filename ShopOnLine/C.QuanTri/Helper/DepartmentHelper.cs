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
    public static class DepartmentHelper
    {
        public static MvcHtmlString DepartmentName(this HtmlHelper helper, int? DepartmentId)
        {
            qtDepartmentService _departmentService = new qtDepartmentService();

            return new MvcHtmlString(_departmentService.DepartmentName(DepartmentId));
        }
    }
}
