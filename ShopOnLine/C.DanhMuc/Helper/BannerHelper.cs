using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Core.Common;
using C.UI.Tool;

namespace C.DanhMuc.Helper
{
    public static class BannerHelper
    {

        public static MvcHtmlString Position_Banner(this HtmlHelper helper, int? Position)
        {
            if (!Position.HasValue)
                Position = PositionBanner.Position_GioiThieu.GetHashCode();

            string html = string.Empty;

            if (Position == PositionBanner.Position_GioiThieu.GetHashCode())
            {
                html = "Banner Giới thiệu";
            }
            else if (Position == PositionBanner.Position_SanPham.GetHashCode())
            {
                html = "Banner Sản phẩm";
            }
            else if (Position == PositionBanner.Position_DaiLy.GetHashCode())
            {
                html = "Banner Đại lý";
            }
            else if (Position == PositionBanner.Position_BangGia.GetHashCode())
            {
                html = "Banner Bảng giá";
            }
            else if (Position == PositionBanner.Position_TinTucSuKien.GetHashCode())
            {
                html = "Banner Tin tức sự kiện";
            }
            else if (Position == PositionBanner.Position_DaiLy.GetHashCode())
            {
                html = "Banner Liên hệ";
            }

            return new MvcHtmlString(html);
        }

    }
}
