using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.Core.Common;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Membership.Helper;

namespace C.DanhMuc.Controllers
{
    public class BannerController : BaseController
    {

        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListBanner(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListBanner(pageCurrent);
            return View();
        }

        public PartialViewResult ListBanner(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shBannerService _banner = new shBannerService();
            IPagedList<shBanner> dsBanner = _banner.DanhSachBanner_PhanTrang(pageCurrent, Config.PAGE_SIZE_20);
            ViewBag.ListBanner = dsBanner;
            return PartialView("ListBanner", dsBanner);
        }
        #endregion

        #region Banner
        public ActionResult Banner(int? Position)
        {
            shBannerService _banner = new shBannerService();

            if (!Position.HasValue)
                Position = PositionBanner.Position_GioiThieu.GetHashCode();

            shBanner banner = _banner.DanhSachBanner_ByPositionBanner(Position.Value);

            return PartialView("Banner", banner);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.BannerGuid = id;
            if (!string.IsNullOrWhiteSpace(id))
            {
                shBannerService _banner = new shBannerService();
                shBanner banner = _banner.FindByKey(id);

                if (banner != null)
                {
                    return View(banner);
                }

            }

            return View(new shBanner());
        }

        [HttpPost]
        public ActionResult Create(string BannerName, string BannerGuid, int? PositionBanner, bool? Status, string Url)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        shBannerService _banner = new shBannerService();

                        _banner.Insert_Update(BannerGuid, null, BannerName, Url, PositionBanner, null, true, DateTime.Now);

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            return RedirectToAction("Index");
        }
        #endregion

    }
}
