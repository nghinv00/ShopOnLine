using C.Core.Common;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.DanhMuc.Helper
{
    public static class ProductHelper
    {
        #region image cart order 
        public static MvcHtmlString ProductCartOrder(this HtmlHelper helper, string Image)
        {
            string html = string.Empty;

            html = "<img src='" + Image + "' class='image-cart'  />";

            return new MvcHtmlString(html);
        }
        #endregion

        #region Mvc HtmString  
        public static MvcHtmlString ProductName(this HtmlHelper helper, string ProductGuid)
        {
            string html = string.Empty;

            if (!string.IsNullOrEmpty(ProductGuid) || !string.IsNullOrWhiteSpace(ProductGuid))
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(ProductGuid);
                html = product.ProductName;
            }
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ProductPreview(this HtmlHelper helper, string ProductGuid, string ProductName, string Content)
        {
            string html = string.Empty;

            html = "<a title='" + Content + "' href=javascript:ViewImages('" + ProductGuid +
                   "')>" + ProductName + "</a>";

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ProductHistory(this HtmlHelper helper, string ProductGuid, string Content)
        {
            string html = string.Empty;

            html = "<a href=javascript:History('" + ProductGuid + "')  data-toggle='tooltip' title='' data-original-title='" + Content + "' >" + Content + "</a>";

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ProductLink(this HtmlHelper helper, string ProductGuid)
        {
            string html = string.Empty;

            if (!string.IsNullOrEmpty(ProductGuid) || !string.IsNullOrWhiteSpace(ProductGuid))
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(ProductGuid);
                html = " <a href='" + product.MetaTitle + "-" + product.ProductId + "' target='_blank'>" + product.ProductName + "</a>";
            }
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ProductLinkAndImage(this HtmlHelper helper, string ProductGuid)
        {
            string html = string.Empty;

            if (!string.IsNullOrEmpty(ProductGuid) || !string.IsNullOrWhiteSpace(ProductGuid))
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(ProductGuid);

                html = " <a href='" + product.MetaTitle + "-" + product.ProductId + "' target='_blank'>" +
                    "<img src='" + product.Image + "'" +
                    "alt='" + product.ProductName + "'>" +
                    "</a>";
            }
            return new MvcHtmlString(html);
        }



        public static string ProductHighLightImage(this HtmlHelper helper, string ProductGuid)
        {
            shProductImageService _productImage = new shProductImageService();

            IEnumerable<shProductImage> dsImage = _productImage.FindList()
                                                .Where(x => x.ProductGuid == ProductGuid);

            string image = string.Empty;

            if (dsImage.Count() > 0)
            {
                shProductImage productDefault = dsImage.LastOrDefault();

                shProductImage productImage = dsImage.Where(x => x.Image == Config.Product_Image_HighLight).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(productImage.Image) || !string.IsNullOrEmpty(productImage.Image))
                {
                    return productImage.FileName;
                }

                return productDefault.FileName;
            }
            return image;
        }

        public static string ProductImage(this HtmlHelper helper, string ProductGuid)
        {
            shProductImageService _productImage = new shProductImageService();

            IEnumerable<shProductImage> dsImage = _productImage.FindList().Where(x => x.ProductGuid == ProductGuid);

            string image = string.Empty;

            if (dsImage.Count() > 0)
            {
                image = dsImage.LastOrDefault().FileName;
            }
            return image;
        }
        #endregion

        #region Khuyến mại theo sản phẩm
        public static MvcHtmlString ChuongTrinhKhuyenMaiSanPham(this HtmlHelper helper, string ProductGuid, int LoaiHienThi)
        {
            string html = string.Empty;
            shSaleService _sale = new shSaleService();
            shSaleDetail saleDetail = _sale.KiemTraSanPhamNamTrongChuongTrinhKhuyenMai(DateTime.Now, ProductGuid, null);

            if (LoaiHienThi == 1)
            {
                html = SaleHelper.TenCachTinhGiaTriKhuyenMai(saleDetail.CachTinhGiaTriKhuyenMai.GetValueOrDefault(CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode()));
            }
            else if (LoaiHienThi == 2)
            {
                html = SizeHelper.FormatGiaTriKhuyenMai(
                    saleDetail.CachTinhGiaTriKhuyenMai.GetValueOrDefault(CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode()),
                    saleDetail.GiaTri);
            }
            else if (LoaiHienThi == 3)
            {
                html = saleDetail.StartDate.GetValueOrDefault(DateTime.Now).ToString("dd-MM") + " - " +
                       saleDetail.EndDate.GetValueOrDefault(DateTime.Now).ToString("dd-MM");
            }

            return new MvcHtmlString(html);
        }
        #endregion

        #region Số lượng - số lượng tồn
        public static MvcHtmlString SoLuongSanPham(this HtmlHelper helper, string ProductGuid)
        {
            shSizeService _size = new shSizeService();
            IEnumerable<shSetSize> dsSize = _size.DanhSachSize_BySectionGuid_ParentNull(null, ProductGuid, null);
            int SoLuong = 0;
            foreach (var size in dsSize)
            {
                SoLuong += size.Number.GetValueOrDefault(0);
            }
            return new MvcHtmlString(SoLuong.ToString());
        }

        public static MvcHtmlString SoLuongSanPhamTonKho(this HtmlHelper helper, string ProductGuid)
        {
            shSizeService _size = new shSizeService();
            IEnumerable<shSetSize> dsSize = _size.DanhSachSize_BySectionGuid_ParentNull(null, ProductGuid, null);
            int SoLuongTon = 0;
            foreach (var size in dsSize)
            {
                SoLuongTon += size.Inventory.GetValueOrDefault(0);
            }

            return new MvcHtmlString(SoLuongTon.ToString());
        }


        #endregion

    }
}
