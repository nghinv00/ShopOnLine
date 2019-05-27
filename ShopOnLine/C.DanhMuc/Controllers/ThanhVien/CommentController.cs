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
    public class CommentController : BaseController
    {

        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListComment(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListComment(pageCurrent);
            return View();
        }

        public PartialViewResult ListComment(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shProductService _product = new shProductService();
            IEnumerable<shProduct> dsProduct = _product.DanhSachProduct();

            shCommentService _comment = new shCommentService();
            IEnumerable<string> dsComment_Product = _comment.DanhSachComment(true).Select(x => x.ProductGuid).Distinct();

            dsProduct = dsProduct.Join(dsComment_Product, x => x.ProductGuid, comment => comment, (x, ProductGuid) => x);
            ViewBag.dsProduct = dsProduct.ToPagedList(pageCurrent, Config.PAGE_SIZE_20);
            return PartialView("ListComment", ViewBag.dsProduct);
        }

        public PartialViewResult DsChildComment(string ProductGuid)
        {
            shCommentService _comment = new shCommentService();

            IEnumerable<shComment> dsComment = _comment.DanhSachComment_ByProductGuid(ProductGuid, null);

            return PartialView("DsChildComment", dsComment);
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

        #region DeleteComment
        public ActionResult DeleteComment(string CommentGuid)
        {
            shCommentService _comment = new shCommentService();


            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CommentGuid) || !string.IsNullOrWhiteSpace(CommentGuid))
                        {
                            shComment comment = _comment.FindByKey(CommentGuid);
                            comment.Status = false;
                            _comment.Update(comment);
                        }

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
