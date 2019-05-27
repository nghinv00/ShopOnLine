
using C.Core.CustomController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Bảng giá
    public class PriceListController : CustomController
    {

        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
