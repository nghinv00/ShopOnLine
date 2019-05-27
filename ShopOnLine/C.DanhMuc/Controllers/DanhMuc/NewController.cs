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
    public class NewController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListNews(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListNews(pageCurrent);
            return View();
        }

        public PartialViewResult ListNews(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shNewService _new = new shNewService();

            IPagedList<shNew> dsNews = _new.DanhSachNews_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListNews = dsNews;
            return PartialView("ListNews", dsNews);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                shNewService _new = new shNewService();
                shNew tintuc = _new.FindByKey(id);

                if (tintuc != null)
                {
                    return View(tintuc);
                }
                else
                    return View(new shNew());
            }

            return View(new shNew());
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        public ActionResult Create(string NewGuiId, int? NewId, string TitleNew, int? SortOrder, string Descriptions, string Summary, string Contents, bool? Status, string ImageFile)
        {
            shNewService _new = new shNewService();
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {


                        if (!string.IsNullOrWhiteSpace(NewGuiId))
                        {
                            NewId = 0;
                        }

                        // check if contents (  ckeditor) null then rollback và yêu cầu người dùng nhập lại
                        if (string.IsNullOrEmpty(Contents))
                        {
                            shNew shnew = _new.FindByKey(NewGuiId);
                            if (shnew == null)
                                shnew = new shNew();

                            //shnew.NewId = NewId.Value;
                            //shnew.Title = Title1;
                            ViewBag.TitleNew = TitleNew;
                            shnew.SortOrder = SortOrder;
                            shnew.Descriptions = Descriptions;
                            shnew.Summary = Summary;
                            shnew.Contents = Contents;
                            shnew.Status = Status;

                            shnew.ImageFile = ImageFile;

                            ModelState.AddModelError("", "Nội dung không được trống. Xin Vui lòng kiểm tra lại. ");
                            return View(shnew);
                        }

                        shNew tintuc = _new.ThemMoi_HieuChinhshNew(
                            NewGuiId,
                            null,
                            TitleNew,
                            Descriptions,
                            ImageFile,
                            Summary,
                            Contents,
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
