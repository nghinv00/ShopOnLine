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
    public class CategoryController : BaseController
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
        public ActionResult Create(string DanhMucGuid, int? DanhMucId, string CategoryName, string CategoryGuid, bool? Status, string Description, int? SortOrder, string FileName)
        {
            shCategory category = new shCategory();
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        // Insert- update Category
                        shCategoryService _category = new shCategoryService();
                        category = _category.ThemMoi_HieuChinhCategory(
                           DanhMucGuid,
                           null,
                           CategoryName,
                           CategoryGuid,
                           null,
                           Status,
                           DateTime.Now,
                           null,
                           Description,
                           SortOrder,
                           FileName);

                        // Insesrt Image 
                        if (!string.IsNullOrEmpty(FileName) || !string.IsNullOrWhiteSpace(FileName))
                        {
                            shCategoryImageService _categoryImage = new shCategoryImageService();
                            shCategoryImage categoryImage = _categoryImage.Insert_UpdateCategoryImage(
                                null,
                                null,
                                category.CategoryGuid,
                                FileName,
                                null,
                                User.Identity.GetUserLogin().Userid,
                                true,
                                DateTime.Now,
                                null
                                );
                        }

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }


            }

            int? page = CommonHelper.FindPageCategory(category.CategoryGuid, category.CategoryId);

            return RedirectToAction("Index", new { page = page });
        }
        #endregion

        #region Chuyển xử lý đơn hàng
        [HttpGet]
        public ActionResult ChuyenNhanVienTheoDoi(string CategoryGuid)
        {
            shCategoryService _category = new shCategoryService();
            shCategory category = _category.FindByKey(CategoryGuid);

            qtUserService _user = new qtUserService();
            IEnumerable<qtUser> ds = _user.DanhSachUser(null, TypeHelper.ToInt32(User.Identity.GetUserLogin().Unitid), null);

            ViewBag.UserId = new SelectList(ds, "UserId", "UserName", category.UserId);

            return PartialView("ChuyenNhanVienTheoDoi", category);
        }

        [HttpPost]
        public ActionResult ChuyenNhanVienTheoDoi(string CategoryGuid, int? UserId, string MoTaNoiDungChuyen)
        {

            shCategoryService _category = new shCategoryService();
            shCategory category = _category.FindByKey(CategoryGuid);

            _category.ThemMoi_HieuChinhCategory(
               category.CategoryGuid,
               null,
               category.CategoryName,
               category.ParentId,
               UserId,
               category.Status,
               category.CreatedDate,
               category.MetaTitle,
               category.Description,
               category.SortOrder,
               category.FileName);


            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

    }
}
