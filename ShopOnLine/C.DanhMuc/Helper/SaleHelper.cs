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
    public static class SaleHelper
    {
        public static MvcHtmlString CachTinhGiaTriKhuyenMai(this HtmlHelper helper, int CachTinhGiaTriKhuyenMai)
        {
            return new MvcHtmlString(TenCachTinhGiaTriKhuyenMai(CachTinhGiaTriKhuyenMai));
        }

        public static MvcHtmlString TrangThaiKhuyenMai(this HtmlHelper helper, int TrangThai)
        {
            string html = "";
            if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.ChoXuLy.GetHashCode())
            {
                html = " <span class='badge badge-primary'>Chờ xử lý</span>";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.DangChay.GetHashCode())
            {
                html = " <span class='badge badge-success'>Đang chạy</span>";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.TamDung.GetHashCode())
            {
                html = " <span class='badge badge-warning'>Tạm dừng</span>";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.KetThuc.GetHashCode())
            {
                html = " <span class='badge badge-info'>Kết thúc</span>";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.GoBo.GetHashCode())
            {
                html = " <span class='badge badge-danger'>Gỡ bỏ</span>";
            }

            return new MvcHtmlString(html);
        }

        public static IEnumerable<DropDownList> DanhsachCachTinhGiaTriKhuyenMai()
        {
            DropDownList drop = new DropDownList();
            List<DropDownList> ds = new List<DropDownList>();
            ds.Add(new DropDownList { Text = "Giảm theo % giá trị", Value = C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode() });
            ds.Add(new DropDownList { Text = "Giảm theo số tiền", Value = C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoSoTien.GetHashCode() });
            //ds.Add(new DropDownList { Text = "Bán giá cố định", Value = C.Core.Common.CachTinhGiaTriKhuyenMai.BanGiaCodinh.GetHashCode() });
            //ds.Add(new DropDownList { Text = "Đang xử lý", Value = C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoKhoangGiaTri.GetHashCode() });
            return ds;
        }

        public static IEnumerable<DropDownList> DanhsachTrangThaiKhuyenMai()
        {
            DropDownList drop = new DropDownList();
            List<DropDownList> ds = new List<DropDownList>();
            //ds.Add(new DropDownList { Text = "Chờ xử lý", Value = C.Core.Common.TrangThaiChuongTrinhKhuyenMai.ChoXuLy.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đang chạy", Value = C.Core.Common.TrangThaiChuongTrinhKhuyenMai.DangChay.GetHashCode() });
            ds.Add(new DropDownList { Text = "Tạm dừng", Value = C.Core.Common.TrangThaiChuongTrinhKhuyenMai.TamDung.GetHashCode() });
            ds.Add(new DropDownList { Text = "Kết thúc", Value = C.Core.Common.TrangThaiChuongTrinhKhuyenMai.KetThuc.GetHashCode() });
            ds.Add(new DropDownList { Text = "Gữ bỏ", Value = C.Core.Common.TrangThaiChuongTrinhKhuyenMai.GoBo.GetHashCode() });
            return ds;
        }

        public static string TenCachTinhGiaTriKhuyenMai(int giatri)
        {
            string html = "";
            if (giatri == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode())
            {
                html = "Giảm theo % giá trị";
            }
            else if (giatri == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoSoTien.GetHashCode())
            {
                html = "Giảm theo số tiền";
            }
            else if (giatri == C.Core.Common.CachTinhGiaTriKhuyenMai.BanGiaCodinh.GetHashCode())
            {
                html = "Bán giá cố định";
            }
            else if (giatri == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoKhoangGiaTri.GetHashCode())
            {
                html = "Giảm theo khoảng giá trị";
            }
            return html;
        }

        public static string TenTrangThaiKhuyenMai(int TrangThai)
        {
            string html = "";
            if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.ChoXuLy.GetHashCode())
            {
                html = "Chờ xử lý";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.DangChay.GetHashCode())
            {
                html = "Đang chạy";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.TamDung.GetHashCode())
            {
                html = "Tạm dừng";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.KetThuc.GetHashCode())
            {
                html = "Kết thúc";
            }
            else if (TrangThai == C.Core.Common.TrangThaiChuongTrinhKhuyenMai.GoBo.GetHashCode())
            {
                html = "Gỡ bỏ";
            }
            return html;
        }


    }
}
