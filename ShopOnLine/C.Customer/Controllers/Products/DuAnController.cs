using C.Core.Common;
using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace C.Customer.Controllers
{
    /// <summary>
    /// Sản phẩm
    /// /// </summary>
    public class DuAnController : CustomController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? id)
        {
            shCategoryService _category = new shCategoryService();
            if (!id.HasValue)
            {
                // Nếu link truyền vào không có id ( ví dụ link tổng quát: san-pham thì mặc định lấy trang danh mục đầu tiên) 
                id = _category.DanhSachCategory().FirstOrDefault().CategoryId;
            }

            shCategory category = _category.FindList().Where(x => x.CategoryId == id).FirstOrDefault();

            ViewBag.CategoryGuid = category.CategoryGuid;
            DsProduct(category.CategoryGuid, 1);
            return View(category);
        }

        public ActionResult DsProduct(string CategoryGuid, int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> ds = _category.DanhSachCategory_ByParentId(CategoryGuid);

            shProductService _product = new shProductService();
            List<shProduct> dsProduct = new List<shProduct>();

            foreach (var item in ds)
            {
                dsProduct.AddRange(_product.DanhSachProduct_TheoDanhMuc(item.CategoryGuid));
            }

            dsProduct.AddRange(_product.DanhSachProduct_TheoDanhMuc(CategoryGuid));

            ViewBag.dsProduct = dsProduct.OrderByDescending(x => x.ProductId).ToPagedList(pageCurrent, Config.PAGE_SIZE_8);
            ViewBag.CategoryGuid = CategoryGuid;
            return PartialView("dsProduct", ViewBag.dsProduct);
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details(int? id)
        {
            shProductService _product = new shProductService();

            shProduct product = _product.FindList().Where(x => x.ProductId == id).FirstOrDefault();

            shProductImageService _productImage = new shProductImageService();
            IEnumerable<shProductImage> dsImage = _productImage.DanhSachProductImage_ByProductGuid(product.ProductGuid);

            ViewBag.dsImageMauMa = dsImage.Where(x => x.ProductImageCategory == Config.ProductImageCategory_Design);
            ViewBag.dsImageChatLieu = dsImage.Where(x => x.ProductImageCategory == Config.ProductImageCategory_Material);


            //shSizeService _size = new shSizeService();
            //IEnumerable<shSize> dsSize = _size.DanhSachSize_BySectionGuid(null, product.ProductGuid, null)
            //                            .OrderBy(x => x.PriceCurrent);

            // Tính toán lại số tiền 
            shSectionService _section = new shSectionService();
            IEnumerable<shProductSet> dsSection = _section.DanhSachSection_TheoProductGuid_ParentNull(product.ProductGuid);
            shProductSet section = new shProductSet();
            if (dsSection != null && dsSection.Count() > 0)
                section = dsSection.FirstOrDefault();
            shSizeService _size = new shSizeService();
            IEnumerable<shSetSize> dsSize = _size.DanhSachSize_BySectionGuid(section.SectionGuid, product.ProductGuid, null);


            ViewBag.ProductSize = section.SectionName + " --- " +
                                    CommonHelper.TinhToanKichThuocMaxMin(dsSize.FirstOrDefault(), dsSize.LastOrDefault());
            ViewBag.ProductPrice = CommonHelper.TinhToanGiaTienMaxMin(dsSize.FirstOrDefault(), dsSize.LastOrDefault());

            // Tính toán số tiền sau khuyến mại
            string productSale = CommonHelper.TinhToanGiaTienSauKhiGiam(dsSize.FirstOrDefault(), dsSize.LastOrDefault());
            if (!string.IsNullOrEmpty(productSale) && !string.IsNullOrWhiteSpace(productSale))
            {
                ViewBag.ProductSale = Format.FormatDecimalToString(Convert.ToDecimal(productSale));
            }
            ViewBag.ListSection = dsSection;
            return View(product);
        }

        #endregion
    }
}
