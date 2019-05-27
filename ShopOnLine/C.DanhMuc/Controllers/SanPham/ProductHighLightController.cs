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
using System.Web;

namespace C.DanhMuc.Controllers
{
    public class ProductHighLightController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page, int? CategoryId)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListProduct(pageCurrent);
            ListProductHighLight(pageCurrent);
            ViewBag.page = pageCurrent;
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost, int? CategoryId)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListProduct(pageCurrent);
            ListProductHighLight(pageCurrent);
            ViewBag.page = pageCurrent;
            return View();
        }

        public PartialViewResult ListProduct(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shProductService _product = new shProductService();

            IPagedList<shProduct> dsProduct = _product.DanhSachProduct().Where(x => x.TopHot != true).ToPagedList(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListProduct = dsProduct;
            return PartialView("ListProduct", dsProduct);
        }

        public PartialViewResult ListProductHighLight(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shProductService _product = new shProductService();

            IPagedList<shProduct> dsProduct = _product.DanhSachTopHotProduct().ToPagedList(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListProductHighLight = dsProduct;
            return PartialView("ListProductHighLight", dsProduct);
        }
        #endregion

        #region HighLight
        public ActionResult HighLight(string[] cbxItem2, int? page)
        {
            shProductService _product = new shProductService();

            _product.HighLight(cbxItem2);

            return RedirectToAction("Index", new { page = page });
        }

        public ActionResult DeleteHighLight(string[] cbxItem1, int? page)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        shProductService _product = new shProductService();

                        _product.UnSubcribeHighLight(cbxItem1);
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
        public ActionResult DialogHighLight(string ProductGuid)
        {
            shProductService _product = new shProductService();
            shProduct product = _product.FindByKey(ProductGuid);

            if (product == null)
            {
                product = new shProduct();
            }

            return PartialView("DialogHighLight", product);
        }

        [HttpPost]
        public ActionResult DialogHighLight(string ProductGuid, string ImageCarousel)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        // 1. cập nhật ảnh đại diện sản phẩm
                        shProductService _product = new shProductService();
                        _product.UploadImageProduct(ProductGuid, ImageCarousel);

                        // 2. Upload file sản phẩm nổi bật
                        shProductImageService _productImage = new shProductImageService();
                        shProductImage productImage = new shProductImage();
                        productImage = _productImage.Insert_UpdateProductImage(
                            null,
                            null,
                            ProductGuid,
                            ImageCarousel,
                            null,
                            User.Identity.GetUserLogin().Userid,
                            true,
                            DateTime.Now,
                            Config.ProductImageCategory_Design,
                            Config.Product_Image_HighLight
                            );

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
