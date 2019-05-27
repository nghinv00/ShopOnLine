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
    public class ProductInventoryController : BaseController
    {

        #region Index
        [HttpGet]
        public ActionResult Index(int? page, int? CategoryId)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ViewBag.page = pageCurrent;

            ListProduct(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost, int? CategoryId)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ViewBag.page = pageCurrent;

            ListProduct(pageCurrent);
            return View();
        }

        public PartialViewResult ListProduct(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shProductService _product = new shProductService();

            IPagedList<shProduct> dsProduct = _product.DanhSachProduct_PhanTrang(pageCurrent, Config.PAGE_SIZE_20, null);

            ViewBag.ListProduct = dsProduct;
            ViewBag.page = pageCurrent;
            return PartialView("ListProduct", dsProduct);
        }

        public PartialViewResult dsSize(string ProductGuid)
        {
            shSizeService _size = new shSizeService();

            IEnumerable<shSetSize> dsSize = _size.DanhSachSection_Size(ProductGuid);

            return PartialView("dsSize", dsSize);
        }

        public PartialViewResult DsChildSection(string SectionGuid)
        {
            shSectionService _section = new shSectionService();

            IEnumerable<shProductSet> dsSection = _section.DanhSachSection_TheoParentId(SectionGuid);

            return PartialView("DsChildSection", dsSection);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            Session[Config.DesignImage] = new List<ProductMultiUpload>();
            Session[Config.MaterialImage] = new List<ProductMultiUpload>();

            if (!string.IsNullOrWhiteSpace(id))
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(id);

                if (product != null)
                {
                    shCategoryService _category = new shCategoryService();

                    ViewBag.citySel = _category.CategoryName(product.CategoryGuid);
                    ViewBag.ProductGuid = product.ProductGuid;

                    #region Image
                    shProductImageService _productImage = new shProductImageService();
                    IEnumerable<shProductImage> DsDesignImage = _productImage.DanhSachProductImage_ByCategory(product.ProductGuid, Config.ProductImageCategory_Design);
                    IEnumerable<shProductImage> DsMaterialImage = _productImage.DanhSachProductImage_ByCategory(product.ProductGuid, Config.ProductImageCategory_Material);
                    ViewBag.DsDesignImage = DsDesignImage;
                    ViewBag.DsMaterialImage = DsMaterialImage;
                    #endregion

                    return View(product);
                }
                else
                {
                    return View(new shProduct());
                }
            }

            if (TempData["ERROR"] != null)
            {
                ModelState.AddModelError("", TempData["ERROR"].ToString());

                shCategoryService _category = new shCategoryService();

                if (TempData["CategoryGuid"] != null)
                {
                    ViewBag.citySel = _category.CategoryName(TempData["CategoryGuid"].ToString());
                }

                ViewBag.CategoryGuid = TempData["CategoryGuid"];
                ViewBag.ProductGuid = TempData["ProductGuid"];
                ViewBag.ProductName = TempData["ProductName"];
                ViewBag.Description = TempData["Description"];
                ViewBag.Details = TempData["Details"];

                TempData["ERROR"] = null;
            }

            return View(new shProduct());
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        public ActionResult Create(string ProductGuid, string CategoryGuid, int? ProductId, string ProductName, string CompleteSetInclude, string Details, int? Number, int? SortOrder, string UserManual, bool? Status, string Description,
                                    string DeleteImage)
        {

            List<ProductMultiUpload> dsDesignImage = (List<ProductMultiUpload>)Session[Config.DesignImage];
            List<ProductMultiUpload> dsMaterialImage = (List<ProductMultiUpload>)Session[Config.MaterialImage];

            shProductService _product = new shProductService();

            string Image = "";
            if (dsDesignImage.Count() > 0)
            {
                var obj = dsDesignImage.FirstOrDefault();
                if (obj != null)
                {
                    Image = obj.value;
                }
            }
            shProduct product = new shProduct();

            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        // 1. Inser , Upload Product 
                        product = _product.Inser_UpdateProduct(
                           ProductGuid,
                           ProductId.Value,
                           CategoryGuid,
                           ProductName,
                           null,
                           CompleteSetInclude,
                           Details,
                           UserManual,
                           null,
                           null,
                           Number,
                           Image,
                           null,
                           null,
                           SortOrder,
                           null,
                           null,
                           Status,
                           DateTime.Now,
                           Description);

                        if (!(product.ProductId > 0))
                        {
                            TempData["ERROR"] = "Cập nhật dữ liệu bản ghi không thành công. Xin vui lòng thao tác lại";
                            TempData["ProductGuid"] = ProductGuid;
                            TempData["CategoryGuid"] = CategoryGuid;
                            TempData["ProductName"] = ProductName;
                            TempData["Description"] = Description;
                            TempData["Details"] = Details;

                            return RedirectToAction("Create", new { id = ProductGuid });
                        }
                        // 2. nếu người dùng có upload file lưu lại lịch sử trong bảng ProductImage
                        shProductImageService _productImage = new shProductImageService();
                        _productImage.InsertAllImageProduct(product.ProductGuid, User.Identity.GetUserLogin().Userid, dsDesignImage, dsMaterialImage, null, null, true, DateTime.Now);

                        // 3. Delete Image 
                        _productImage.DeleteAllImageProduct(DeleteImage);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }

            int? page = CommonHelper.FindPageProduct(product.ProductGuid, product.ProductId, Config.PAGE_SIZE_20);

            return RedirectToAction("Index", new { page });
        }

        #region FileDeleteProductController 
        [HttpPost]
        public JsonResult DeleteImageProduct(string value = "", string category = "")
        {
            string OK = "OK";
            if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(category))
            {
                //get key upload api
                List<ProductMultiUpload> listFile = (List<ProductMultiUpload>)HttpContext.Session[category];

                if (listFile == null)
                {
                    return Json(OK, JsonRequestBehavior.AllowGet);
                }

                value = value.TrimStart().TrimEnd();

                IEnumerable<ProductMultiUpload> dsProductImageDelete = listFile.Where(x => x.value == value);
                if (dsProductImageDelete.Count() > 0)
                {
                    bool check = listFile.Remove(dsProductImageDelete.FirstOrDefault());
                    if (!check)
                        OK = string.Empty;
                }
            }

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Preview
        public ActionResult Preview(string ProductGuid)
        {
            shProductService _product = new shProductService();
            shProduct product = _product.FindByKey(ProductGuid);

            if (product != null)
            {
                #region Image
                shProductImageService _productImage = new shProductImageService();
                IEnumerable<shProductImage> DsDesignImage = _productImage.DanhSachProductImage_ByCategory(product.ProductGuid, Config.ProductImageCategory_Design);
                IEnumerable<shProductImage> DsMaterialImage = _productImage.DanhSachProductImage_ByCategory(product.ProductGuid, Config.ProductImageCategory_Material);
                ViewBag.DsDesignImage = DsDesignImage;
                ViewBag.DsMaterialImage = DsMaterialImage;
                #endregion

                shSectionService _section = new shSectionService();
                IEnumerable<shProductSet> dsSection = _section.DanhSachSection_TheoProductGuid_ParentNull(product.ProductGuid);
                ViewBag.Section = dsSection;

                return PartialView("Preview", product);
            }

            return PartialView("Preview", new shProduct());
        }
        #endregion

        #region Section
        [HttpGet]
        public ActionResult Section(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(id);

                if (product != null)
                {
                    shCategoryService _category = new shCategoryService();


                    return View(product);
                }
                else
                {
                    return View(new shProduct());
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Section(string ProductGuid, int? ProductId)
        {


            return RedirectToAction("Index");
        }


        #endregion
    }
}
