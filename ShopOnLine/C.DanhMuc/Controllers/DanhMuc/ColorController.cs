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
    public class ColorController : BaseController
    {

        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListCategory(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListCategory(pageCurrent);
            return View();
        }

        public PartialViewResult ListCategory(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shCategoryService _category = new shCategoryService();

            IPagedList<shCategory> dsCategory = _category.DanhSachCategory_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListCategory = dsCategory;
            return PartialView("ListCategory", dsCategory);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {

            if (!string.IsNullOrWhiteSpace(id))
            {
                shCategoryService _category = new shCategoryService();
                shCategory category = _category.FindByKey(id);

                if (category != null)
                {
                    ViewBag.DanhMucId = category.CategoryId;
                    ViewBag.DanhMucGuid = category.CategoryGuid;
                    ViewBag.ParentId = category.ParentId;
                    return View(category);
                }
                else
                    return View(new shCategory());
            }

            ViewBag.CategoryId = id;
            return View(new shCategory());
        }

        [HttpPost]
        public ActionResult Create(string DanhMucGuid, int? DanhMucId, string CategoryName, string CategoryId, bool? Status)
        {
            if (!DanhMucId.HasValue)
            {
                DanhMucId = 0;
            }

            //shCategoryService _category = new shCategoryService();
            //shCategory category = _category.ThemMoi_HieuChinhCategory(
            //    DanhMucGuid,
            //    DanhMucId.Value,
            //    CategoryName,
            //    CategoryId,
            //    HttpContext.User.Identity.GetUserLogin().Userid,
            //    Status,
            //    DateTime.Now);


            return RedirectToAction("Index");
        }
        #endregion

    }
}
