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
using System.IO;

namespace C.DanhMuc.Controllers
{
    public class SectionController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(string id, int? page, int? p)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }

            ListSize(id, null, pageCurrent, null, p);
            ViewBag.ProductGuid = id;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, int? page, bool? IsHttpPost, int? p)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }

            ListSize(id, null, pageCurrent, null, p);
            ViewBag.ProductGuid = id;
            return View();
        }

        public PartialViewResult ListSize(string ProductGuid, string SectionGuid, int? page, string SizeGuid, int? p)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }

            shSizeService _size = new shSizeService();
            shSectionService _section = new shSectionService();

            IEnumerable<shSetSize> ListSize = _size.DanhSachSize_BySectionGuid_ParentNull(SectionGuid, ProductGuid, SizeGuid);

            IPagedList<shSetSize> dsSize = ListSize.OrderBy(x => x.SizeId).ToPagedList(pageCurrent, Config.PAGE_SIZE_20);

            ViewBag.ListSize = dsSize;
            ViewBag.p = p;
            return PartialView("ListSize", dsSize);
        }

        public PartialViewResult DsChildSize(string SizeGuid)
        {
            shSizeService _size = new shSizeService();
            shSectionService _section = new shSectionService();

            IEnumerable<shSetSize> ListSize = _size.DanhSachSize_ByParent(SizeGuid);

            return PartialView("DsChildSize", ListSize);
        }


        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id, string s, string p)
        {

            shSectionService _section = new shSectionService();
            shProductService _product = new shProductService();
            shProductSet section = new shProductSet();

            if (!string.IsNullOrWhiteSpace(s))
            {
                section = _section.FindByKey(s);

                ViewBag.parent = string.IsNullOrWhiteSpace(section.ParentId) ? 0 : 1;
            }

            section.ProductGuid = id;
            ViewBag.ProductName = _product.ProductName(id);
            ViewBag.ProductGuid = id;

            IEnumerable<shProductSet> dsSection = _section.DanhSachSection()
                                            .Where(x => x.ProductGuid == id &&
                                                        x.ParentId == null &&
                                                        x.SectionGuid != s)
                                            .OrderBy(x => x.SectionId);
            ViewBag.dsSection = new SelectList(dsSection, "SectionGuid", "SectionName", null);
            ViewBag.p = p;
            return View(section);
        }

        [HttpPost]
        public ActionResult Create(string ProductGuid, string SectionGuid, string SectionName, string ProductName, int? SortOrder, string ParentId, string parent)
        {
            if (parent == "0")
            {
                ParentId = null;
            }
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        shSectionService _section = new shSectionService();
                        shProductSet section = _section.Insert_UpdateSection(
                            SectionGuid,
                            null,
                            ProductGuid,
                            SectionName,
                            SortOrder,
                            null,
                            true,
                            DateTime.Now,
                            ParentId);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }



            return RedirectToAction("Index", new { id = ProductGuid });
        }
        #endregion

        #region Edit Size
        [HttpGet]
        public PartialViewResult EditSize(string SizeGuid, string SectionGuid, string ProductGuid)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = _size.FindByKey(SizeGuid);

            if (size == null)
            {
                size = new shSetSize();
            }

            shSectionService _section = new shSectionService();
            IEnumerable<shProductSet> dsSection = _section.DanhSachSection_TheoProductGuid(ProductGuid);

            ViewBag.SectionGuid = new SelectList(dsSection, "SectionGuid", "SectionName", SectionGuid);

            IEnumerable<shSetSize> dsSize = _size.DanhSachSize_BySectionGuid(null, ProductGuid, null)
                                    .Where(x => string.IsNullOrWhiteSpace(x.ParentId)
                                                || string.IsNullOrEmpty(x.ParentId));
            foreach (var item in dsSize)
            {
                item.SizeName = _section.SectionName(item.SectionGuid) + " - " + item.SizeName;

            }
            ViewBag.Parent = new SelectList(dsSize, "SizeGuid", "SizeName", size.ParentId);
            //size.PriceCurrent = Format.SubStringDotInDecimal(size.PriceCurrent.Value);

            return PartialView("EditSize", size);
        }

        [HttpPost]
        public ActionResult EditSize(string SizeGuid, string ProductGuid, string SectionGuid, string SizeName, decimal? PriceCurrent, string Stuff, string Parent)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = new shSetSize();
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        size = _size.InsertUpdateSize(
                                                  SizeGuid,
                                                  null,
                                                  ProductGuid,
                                                  SectionGuid,
                                                  SizeName,
                                                  null,
                                                  PriceCurrent,
                                                  null,
                                                  null,
                                                  null,
                                                  null,
                                                  null,
                                                  true,
                                                  DateTime.Now,
                                                  Stuff,
                                                  Parent);

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            if (Request.IsAjaxRequest())
            {
                string data = "fail";
                if (size.SizeId > 0)
                    data = "ok";

                return Json(data, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteSize
        public ActionResult DeleteSize(string SizeGuid, string SectionGuid, string ProductGuid)
        {
            shSizeService _size = new shSizeService();
            _size.Delete(SizeGuid);

            if (Request.IsAjaxRequest())
            {

                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteSection
        public ActionResult DeleteSection(string SectionGuid, string ProductGuid)
        {
            shSectionService _section = new shSectionService();
            shProductSet section = _section.FindByKey(SectionGuid);
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        if (section != null)
                        {
                            section.Status = false;
                            _section.Update(section);
                        }

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            if (Request.IsAjaxRequest())
            {

                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion


    }
}
