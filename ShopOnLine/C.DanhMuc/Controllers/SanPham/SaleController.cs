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
using C.DanhMuc.Helper;

namespace C.DanhMuc.Controllers
{
    public class SaleController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListSale(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListSale(pageCurrent);
            return View();
        }

        public PartialViewResult ListSale(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shSaleService _sale = new shSaleService();
            IPagedList<shSale> dsSale = _sale.DanhSach_PhanTrang(pageCurrent, Config.PAGE_SIZE_20);
            ViewBag.ListSale = dsSale;
            return PartialView("ListSale", dsSale);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            shSaleService _sale = new shSaleService();
            shSale sale = _sale.FindByKey(id);
            if (sale != null)
            {

            }
            else
            {
                sale = new shSale();
            }

            ViewBag.Status = new SelectList(SaleHelper.DanhsachTrangThaiKhuyenMai(), "Value", "Text", sale.SaleStatus);
            ViewBag.KhuyenMai = new SelectList(SaleHelper.DanhsachCachTinhGiaTriKhuyenMai(), "Value", "Text", sale.CachTinhGiaTriKhuyenMai);

            return View(new shSale());
        }

        public ActionResult divCategory()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> ds = _category.DanhSachCategory();

            return PartialView("divCategory", ds);
        }

        public ActionResult divProduct()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> ds = _category.DanhSachCategory();

            return PartialView("divProduct", ds);
        }

        public ActionResult dsProduct(string ProductAdd)
        {
            List<shProduct> ds = new List<shProduct>();
            shProduct product = new shProduct();
            shProductService _product = new shProductService();

            if (!string.IsNullOrEmpty(ProductAdd) || !string.IsNullOrWhiteSpace(ProductAdd))
            {
                string[] dsProduct = ProductAdd.Split(';');
                if (dsProduct != null)
                {
                    foreach (var child in dsProduct)
                    {
                        product = _product.FindByKey(child);
                        if (product != null)
                        {
                            ds.Add(product);
                        }
                    }
                }
            }

            ViewBag.ProductAdd = ProductAdd;

            return PartialView("dsProduct", ds);
        }

        [HttpPost]
        public ActionResult Create(string SaleGuid, string SaleName,
            string SaleCode, int? SaleStatus, string Description,
            string StartTime, string EndTime, int? CachTinhGiaTriKhuyenMai,
            decimal? Percent, double? USD, int? DieuKienApDung,
            string[] CagegoryChild,
            string[] ProductGuid1)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        shSaleService _sale = new shSaleService();
                        shSale sale = _sale.ThemMoi_CapNhatKhuyenMai(
                                        SaleGuid,
                                        SaleName,
                                        SaleCode,
                                        SaleStatus,
                                        Description,
                                        StartTime,
                                        EndTime,
                                        CachTinhGiaTriKhuyenMai,
                                        Percent,
                                        USD,
                                        DieuKienApDung,
                                        CagegoryChild,
                                        ProductGuid1,
                                        User.Identity.GetUserLogin().Userid,
                                        true,
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
