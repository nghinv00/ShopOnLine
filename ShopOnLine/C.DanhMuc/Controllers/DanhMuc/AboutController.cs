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
    public class AboutController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListAbout(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListAbout(pageCurrent);
            return View();
        }

        public PartialViewResult ListAbout(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shAboutService _about = new shAboutService();
            IPagedList<shAboutu> dsAbout = _about.DanhSachAbout_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);
            ViewBag.ListAbout = dsAbout;
            return PartialView("ListAbout", dsAbout);
        }

        public ActionResult DsChildAbout(string AboutGuid)
        {
            shAboutService _about = new shAboutService();
            IEnumerable<shAboutu> dsAbout = _about.DanhSachAbout_ByParentId(AboutGuid).OrderBy(x => x.AboutId);

            if (dsAbout == null)
            {
                dsAbout = new List<shAboutu>();
            }
            return PartialView("DsChildAbout", dsAbout);
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
            shAboutService _about = new shAboutService();
            shAboutu about = new shAboutu();

            if (!string.IsNullOrWhiteSpace(id))
            {
                about = _about.FindByKey(id);
            }

            IEnumerable<shAboutu> dsAbout = _about.DanhSachAbout();

            ViewBag.Parent = new SelectList(dsAbout, "AboutGuid", "AboutTitle", about.ParentId);

            return View(about);
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        public ActionResult Create(string AboutGuid, string AboutTitle, string AboutName,
            string AboutContent, int? Year, string Parent, string Sign,
            string ImageUrl, int? SortOrder,
            bool? Status, string Url)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        shAboutService _about = new shAboutService();
                        shAboutu about = new shAboutu();

                        about = _about.Insert_About(
                                            AboutGuid,
                                            null,
                                            AboutTitle,
                                            AboutName,
                                            AboutContent,
                                            Year,
                                            Sign,
                                            ImageUrl,
                                            Parent,
                                            SortOrder,
                                            Status,
                                            DateTime.Now);

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
