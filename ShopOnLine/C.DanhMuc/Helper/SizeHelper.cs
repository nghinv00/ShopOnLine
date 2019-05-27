using C.Core.Common;
using C.Core.ExModel;
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
    public static class SizeHelper
    {
        public static MvcHtmlString SizeName(this HtmlHelper helper, string SizeGuid)
        {
            shSizeService _size = new shSizeService();

            shSetSize size = _size.FindByKey(SizeGuid);

            if (size == null)
            {
                size = new shSetSize();
            }
            return new MvcHtmlString(size.SizeName);
        }


        public static MvcHtmlString ListProductPrice_Quantity(this HtmlHelper helper, List<CartItem> ds, decimal MatHangGiamGia, decimal DiemThuong, decimal PhiGiaoHang)
        {
            decimal price = ListProductPrice_Quantity(ds, MatHangGiamGia, DiemThuong, PhiGiaoHang);
            return new MvcHtmlString(Format.FormatDecimalToString(price));
        }

        public static decimal ListProductPrice_Quantity(List<CartItem> ds, decimal MatHangGiamGia, decimal DiemThuong, decimal PhiGiaoHang)
        {
            shSizeService _size = new shSizeService();
            decimal price = _size.ListProductPrice_Quantity(ds, MatHangGiamGia, DiemThuong, PhiGiaoHang);
            return price;
        }

        public static MvcHtmlString ProductPrice(this HtmlHelper helper, string SizeGuid)
        {
            shSizeService _size = new shSizeService();

            decimal price = _size.ProductPrice(SizeGuid);

            return new MvcHtmlString(Format.FormatDecimalToString(price));
        }


        public static MvcHtmlString ProductPrice_Quantity(this HtmlHelper helper, string SizeGuid, int Quantity)
        {
            shSizeService _size = new shSizeService();

            decimal price = _size.ProductSize_Quantity(SizeGuid, Quantity);

            return new MvcHtmlString(Format.FormatDecimalToString(price));
        }

        public static MvcHtmlString SizeNameByParentId_SectionGuid(this HtmlHelper helper, string ParentId, string SectionGuid)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = _size.DanhSachSize()
                                .Where(x => x.ParentId == ParentId
                                            && x.SectionGuid == SectionGuid)
                                 .FirstOrDefault();

            string html = string.Empty;
            if (size != null && !string.IsNullOrEmpty(size.SizeName) && !string.IsNullOrWhiteSpace(size.SizeName))
                html = size.SizeName;

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString SizeNameByParentId_SectionGuid_PriceCurrent(this HtmlHelper helper, string ParentId, string SectionGuid)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = _size.DanhSachSize()
                                .Where(x => x.ParentId == ParentId
                                            && x.SectionGuid == SectionGuid)
                                 .FirstOrDefault();

            string html = string.Empty;
            if (size != null && !string.IsNullOrEmpty(size.SizeName) && !string.IsNullOrWhiteSpace(size.SizeName))
                html = Format.FormatDecimalToString(size.PriceCurrent.GetValueOrDefault(0)).Replace(',', '.');

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString IconUpdate_Delete(this HtmlHelper helper, string SizeGuid)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = _size.FindByKey(SizeGuid);

            if (size == null)
            {
                size = new shSetSize();
            }

            string icon = "<a href='javascript:void(0)' onclick=Edit('" + SizeGuid + "','" + size.SectionGuid + "')>" +
                        "<span class='fa fa-pencil-square-o' aria-hidden='true' style='transform: scale(1.3, 1.3);' title='Sửa'></span>" +
                        "</a>";

            icon += "&nbsp;&nbsp;";

            icon += "<a href='javascript:void(0)' onclick=Delete('" + SizeGuid + "','" + size.SectionGuid + "')>" +
                        "<span class='fa fa-trash' aria-hidden='true' style='transform: scale(1.3, 1.3);' title='Xóa'></span> " +
                        "</a>";

            return new MvcHtmlString(icon);
        }

        public static MvcHtmlString FormatDecimalToString(this HtmlHelper helper, decimal? PriceCurrent)
        {
            string html = string.Empty;

            if (PriceCurrent.HasValue)
            {
                html = Format.FormatDecimalToString(PriceCurrent.Value);
            }

            return new MvcHtmlString(html);
        }


        public static MvcHtmlString FormatGiaTriKhuyenMai(this HtmlHelper helper,int? CachTinhGiaTriKhuyenMai,  string GiaTri)
        {
            string html = FormatGiaTriKhuyenMai(CachTinhGiaTriKhuyenMai, GiaTri);

            return new MvcHtmlString(html);
        }

        public static string FormatGiaTriKhuyenMai(int? CachTinhGiaTriKhuyenMai, string GiaTri)
        {
            string html = string.Empty;

            if (!string.IsNullOrWhiteSpace(GiaTri) || !string.IsNullOrEmpty(GiaTri))
            {
                html = Format.FormatDecimalToString(Convert.ToInt32(GiaTri));
            }

            if (CachTinhGiaTriKhuyenMai.GetValueOrDefault(0) == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode())
            {
                html += " %";
            }
            else if (CachTinhGiaTriKhuyenMai.GetValueOrDefault(0) == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoSoTien.GetHashCode())
            {
                html += " VND";
            }
            return html;
        }

        public static MvcHtmlString ChatLieu_KichThuoc(this HtmlHelper helper, string SizeGuid, string SizeName, string Stuff, string ParentId, string SectionGuid)
        {
            string html = string.Empty;

            if (!string.IsNullOrWhiteSpace(SizeName))
            {
                html += SizeName;
            }

            if (!string.IsNullOrWhiteSpace(Stuff))
            {
                html += " - " + Stuff;
            }
            string title = string.Empty;
            string style = string.Empty;
            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                shSectionService _section = new shSectionService();
                shSizeService _size = new shSizeService();
                shSetSize size = _size.FindByKey(ParentId);
                if (size != null)
                    title = "Thuộc bộ sản phẩm: " + _section.SectionName(SectionGuid) + " [" + size.SizeName + "]";
            }
            else
            {
                style = "font-weight:bold; cursor: pointer";
            }
            string span = "<span title='" + title + "' style='" + style + "' class='parent-size'>" + html + "</span>";

            return new MvcHtmlString(span);
        }

        public static MvcHtmlString ChatLieu_KichThuoc_TenSection(this HtmlHelper helper, string SizeGuid, string SizeName, string Stuff, string ParentId, string SectionGuid)
        {
            string html = string.Empty;

            shSectionService _section = new shSectionService();
            shProductSet section = _section.FindByKey(SectionGuid);

            if (!string.IsNullOrWhiteSpace(SizeName))
            {
                html += SizeName;
            }

            if (!string.IsNullOrWhiteSpace(Stuff))
            {
                html += " - " + Stuff;
            }
            string title = string.Empty;
            string style = string.Empty;
            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                shSizeService _size = new shSizeService();
                shSetSize size = _size.FindByKey(ParentId);
                if (size != null)
                    title = section.SectionName + " [" + size.SizeName + "]";
                style = "font-weight:bold;";
            }
            else
            {
                style = "font-weight:bold; cursor: pointer";
            }
            string span = "<span title='" + title + "'  class=''>" +
                          section.SectionName + " - " +
                          "<span class='parent-size' style='" + style + "' >" + html + "</span>" +
                          "</span>";

            return new MvcHtmlString(span);
        }

    }
}
