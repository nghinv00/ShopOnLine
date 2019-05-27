using C.Core.CustomController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    /// <summary>
    /// Sản phẩm
    /// /// </summary>
    public class ProductsController : CustomController
    {
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details(int? id)
        {
            return View(id);
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult Products(int? id)
        {
            return View(id);
        }
        #endregion

    }
}
