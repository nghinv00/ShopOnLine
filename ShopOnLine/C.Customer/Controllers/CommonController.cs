using C.Core.Common;
using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Dùng chung 
    public class CommonController : CustomController
    {
        public ActionResult CategoryLeft(bool? IsMenuChild)
        {
            if (!IsMenuChild.HasValue)
            {
                IsMenuChild = false;
            }

            shCategoryService _category = new shCategoryService();
            IEnumerable<shCategory> dsCategory = _category.DanhSachCategory()
                                                    .Where(x => string.IsNullOrEmpty(x.ParentId)
                                                        || string.IsNullOrWhiteSpace(x.ParentId))
                                                        .ToList();

            List<shCategory> dsDanhMuc = dsCategory.ToList();
            if (IsMenuChild.Value)
            {
                foreach (var item in dsCategory)
                {
                    dsDanhMuc.AddRange(_category.DanhSachCategory_ByParentId(item.CategoryGuid));
                }
            }

            return PartialView("CategoryLeft", dsDanhMuc);
        }

        public ActionResult SectionGuid(string ProductGuid, string SectionGuid)
        {
            shSectionService _section = new shSectionService();

            //IEnumerable<shSection> ds = _section.DanhSachSection_TheoProductGuid(ProductGuid);
            IEnumerable<shProductSet> ds = _section.DanhSachSection_TheoProductGuid_ParentNull(ProductGuid);

            SelectList select = new SelectList(ds, "SectionGuid", "SectionName", SectionGuid);

            return PartialView("SectionGuid", select);
        }

        public ActionResult SizeGuid(string ProductGuid, string SectionGuid, string SizeGuid)
        {
            if (string.IsNullOrWhiteSpace(SectionGuid) || string.IsNullOrEmpty(SectionGuid))
            {
                shSectionService _section = new shSectionService();

                IEnumerable<shProductSet> ds = _section.DanhSachSection_TheoProductGuid_ParentNull(ProductGuid);

                if (ds != null && ds.Count() > 0)
                    SectionGuid = ds.FirstOrDefault().SectionGuid;
            }
           
            shSizeService _size = new shSizeService();
            IEnumerable<shSetSize> dsSize = _size.DanhSachSize_BySectionGuid(SectionGuid, ProductGuid, null);
            foreach (var size in dsSize)
            {
                size.SizeName += " - " + Format.FormatDecimalToString(size.PriceCurrent.GetValueOrDefault(0)).Replace(',', '.');
            }
            SelectList select = new SelectList(dsSize, "SizeGuid", "SizeName", SizeGuid);

            return PartialView("SizeGuid", select);
        } 

        /// <summary>
        /// Những sản phẩm liên quan
        /// </summary>
        /// <param name="ProductGuid"></param>
        /// <returns></returns>
        public ActionResult RelatedProducts(string ProductGuid, string CategoryGuid)
        {
            shProductService _product = new shProductService();

            IEnumerable<shProduct> dsProduct = _product.DanhSachProduct_TheoDanhMuc(CategoryGuid)
                                                        .Where(x => x.ProductGuid != ProductGuid);

            return PartialView("RelatedProducts", dsProduct);
        }

        /// <summary>
        /// Comment của thành viên dành cho sản phẩm 
        /// </summary>
        /// <param name="ProductGuid"></param>
        /// <returns></returns>
        public ActionResult CommentProduct(string ProductGuid, int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shCommentService _comment = new shCommentService();

            IPagedList<shComment> dsComment = _comment.DanhSachComment_ByProductGuid_PhanTrang(ProductGuid, pageCurrent, Config.PAGE_SIZE_2, true);

            return PartialView("CommentProduct", dsComment);
        }

        public ActionResult PostComment(string rating, string name, string email, string content, string productguid)
        {

            shCommentService _comment = new shCommentService();

            shComment comment = _comment.Insert_UpdateComment(
                                            null,
                                            null,
                                            null,
                                            productguid,
                                            email,
                                            name,
                                            TypeHelper.ToInt32(rating),
                                            content,
                                            true,
                                            DateTime.Now);


            if (Request.IsAjaxRequest() )
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return View();
        }


        public ActionResult CategoryCommon()
        {

            return PartialView("CategoryCommon");
        }

        #region Cart - Order 
        public ActionResult Province()
        {
            landProvinceService _province = new landProvinceService();

            SelectList select = new SelectList(_province.DanhSachProvince(), "ProvinceId", "Name", null);

            return PartialView("Province", select);
        }

        public ActionResult District(int? ProvinceId)
        {
            landDistrictService _district = new landDistrictService();

            SelectList select = new SelectList(_district.DanhSachDistrict(ProvinceId), "DistrictId", "Name", null);

            return PartialView("District", select);
        }
        #endregion
    }
}
