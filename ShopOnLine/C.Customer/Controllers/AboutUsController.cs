using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Giới thiệu
    public class AboutUsController : CustomController
    {

        #region Index
        public ActionResult Index()
        {
            shAboutService _about = new shAboutService();
            shAboutu about = _about.DanhSachAbout().OrderBy(x => x.AboutId).FirstOrDefault();

            IEnumerable<shAboutu> dsChildAbout = _about.DanhSachAbout_ByParentId(about.AboutGuid).OrderBy(x => x.AboutId);
            ViewBag.dsChildAbout = dsChildAbout;
            return View(about);
        }
        #endregion

        #region MenuLeft
        public ActionResult MenuLeft()
        {
            shAboutService _about = new shAboutService();

            IEnumerable<shAboutu> dsAbout = _about.DanhSachAbout();

            if (dsAbout == null)
            {
                dsAbout = new List<shAboutu>();
            }

            return PartialView("MenuLeft", dsAbout);
        }
        #endregion

        #region Details
        public ActionResult Details(int? id)
        {
            shAboutService _about = new shAboutService();
            shAboutu about = new shAboutu();
            
            if (id.HasValue)
            {
                about = _about.FindList().Where(x => x.AboutId == id).FirstOrDefault();
            }

            IEnumerable<shAboutu> dsChild = _about.DanhSachAbout_ByParentId(about.AboutGuid);
            ViewBag.dsChild = dsChild;

            return View(about);
        }
        #endregion
    }
}
