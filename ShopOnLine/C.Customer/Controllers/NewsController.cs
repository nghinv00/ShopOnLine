
using C.Core.Common;
using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Tin tức - sự kiện
    public class NewsController : CustomController
    {

        #region Index
        public ActionResult Index()
        {

            DsNews(1);
            return View();
        }

        public ActionResult DsNews(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shNewService _tintuc = new shNewService();

            IPagedList<shNew> dsNew = _tintuc.DanhSachNews_PhanTrang(pageCurrent, Config.PAGE_SIZE_9);

            ViewBag.DsNews = dsNew;

            return PartialView("DsNews", ViewBag.DsNews);
        }

        #endregion


        #region Details
        public ActionResult Details(int? id)
        {
            shNewService _tintuc = new shNewService();
            shNew tintuc = _tintuc.FindList().Where(x => x.NewId == id).FirstOrDefault();

            if (tintuc == null)
                tintuc = new shNew();




            return View(tintuc);
        }
        #endregion


        #region Sản phẩm liên quan 
        public ActionResult NewsRelated(string NewGuiId)
        {
            shNewService _tintuc = new shNewService();
            IEnumerable<shNew> dsNews = _tintuc.DanhSachNews()
                                        .Where(x => x.NewGuiId != NewGuiId)
                                        .ToPagedList(1, 10);

            return PartialView("NewsRelated", dsNews);
        }
        #endregion

    }
}
