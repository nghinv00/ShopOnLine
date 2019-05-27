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
    public class CategoryHighlightController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListCategory(pageCurrent);
            ListCategoryHighLight(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListCategory(pageCurrent);
            ListCategoryHighLight(pageCurrent);
            return View();
        }

        public PartialViewResult ListCategory(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shCategoryService _category = new shCategoryService();

            IPagedList<shCategory> dsCategory = _category.DanhSachCategory().Where(x => x.TopHot != true).ToPagedList(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListCategory = dsCategory;
            return PartialView("ListCategory", dsCategory);
        }

        public PartialViewResult ListCategoryHighLight(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shCategoryService _category = new shCategoryService();

            IPagedList<shCategory> dsCategory = _category.DanhSachCategory_TopHot().ToPagedList(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListCategoryHighLight = dsCategory;
            return PartialView("ListCategoryHighLight", dsCategory);
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
                        // Insert Category
                        shCategoryService _category = new shCategoryService();
                        category = _category.ThemMoi_HieuChinhCategory(
                            DanhMucGuid,
                            null,
                            CategoryName,
                            CategoryGuid,
                            HttpContext.User.Identity.GetUserLogin().Userid,
                            Status,
                            DateTime.Now,
                            null,
                            Description,
                            SortOrder,
                            FileName); // Phần sắp xếp danh mục cần có để khi di chuyển danh mục dễ dàng hơn

                        // Insesrt Image 
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

        #region HighLight
        public ActionResult HighLight(string[] cbxItem2, int? page)
        {
            shCategoryService _category = new shCategoryService();
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        _category.HighLight(cbxItem2);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            return RedirectToAction("Index", new { page = page });
        }

        public ActionResult DeleteHighLight(string[] cbxItem1, int? page)
        {
            shCategoryService _category = new shCategoryService();
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        _category.UnSubcribeHighLight(cbxItem1);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            return RedirectToAction("Index", new { page = page });
        }
        #endregion

        #region DialogHighLight
        [HttpGet]
        public ActionResult DialogHighLight(string CategoryGuid)
        {
            shCategoryImageService _category = new shCategoryImageService();

            IEnumerable<shCategoryImage> dsCategory = _category.FindList().Where(x => x.CategoryGuid == CategoryGuid);
            string Icon = "/Content/Images/background.jpg";
            shCategoryImage categoryImage = dsCategory.Where(x => x.Image == Config.Category_Image_Icon).LastOrDefault();
            if (categoryImage != null)
            {
                Icon = categoryImage.FileName;
            }
            ViewBag.Icon = "<img src='" + Icon + "' id='previewIcon' name='previewIcon' style='width:auto; height: auto; max-height: 100px; max-width: 100px;' />";

            Icon = "/Content/Images/background.jpg";
            categoryImage = dsCategory.Where(x => x.Image == Config.Category_Image_Icon_Active).LastOrDefault();
            if (categoryImage != null)
            {
                Icon = categoryImage.FileName;
            }
            ViewBag.IconActive = "<img src='" + Icon + "' id='previewIconActive' name='previewIconActive' style='width:auto; height: auto; max-height: 100px; max-width: 100px;' />";

            return PartialView("DialogHighLight");
        }

        [HttpPost]
        public ActionResult DialogHighLight(string CategoryGuid, string ImageIcon, string ImageIconActive)
        {
            // 1. Upload file sản phẩm nổi bật
            shCategoryImageService _categoryImage = new shCategoryImageService();
            shCategoryImage categoryImage = new shCategoryImage();

            _categoryImage.Insert_UpdateCategoryImage(
                null,
                null,
                CategoryGuid,
                ImageIcon,
                null,
                User.Identity.GetUserLogin().Userid,
                true,
                DateTime.Now,
                Config.Category_Image_Icon
                );

            _categoryImage.Insert_UpdateCategoryImage(
                null,
                null,
                CategoryGuid,
                ImageIconActive,
                null,
                User.Identity.GetUserLogin().Userid,
                true,
                DateTime.Now,
                Config.Category_Image_Icon_Active
                );

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}