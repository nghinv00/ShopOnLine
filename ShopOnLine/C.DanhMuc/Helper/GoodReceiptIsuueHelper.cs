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
    public static class GoodReceiptIsuueHelper
    {

        public static MvcHtmlString TenPhieu(this HtmlHelper helper, int? Phieu)
        {
            string html = string.Empty;

            if (!Phieu.HasValue)
                Phieu = PhieuNhapXuat.Nhap.GetHashCode();

            if (Phieu == PhieuNhapXuat.Nhap.GetHashCode())
            {
                html = "<span class='phieu phieu-nhap'>Nhập</span>";
            }
            else if (Phieu == PhieuNhapXuat.Xuat.GetHashCode())
            {
                html = "<span class='phieu phieu-xuat'>Xuất</span>";
            }
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString TrangThai(this HtmlHelper helper, int? TrangThai)
        {
            string html = string.Empty;

            if (TrangThai == TrangThaiPhieuNhapXuat.Moi.GetHashCode())
            {
                html = "<span class='status-vb badge badge-primary'>Mới</span>";
            }
            else if (TrangThai == TrangThaiPhieuNhapXuat.DangXuLy.GetHashCode())
            {
                html = "<span class='status-vb badge badge-warning'>Đang xử lý</span>";
            }
            else if (TrangThai == TrangThaiPhieuNhapXuat.HoanThanh.GetHashCode())
            {
                html = "<span class='status-vb badge badge-success'>Hoàn thành</span>";
            }
            else if (TrangThai == TrangThaiPhieuNhapXuat.Huy.GetHashCode())
            {
                html = "<span class='status-vb badge badge-danger'>Hủy</span>";
            }

            return new MvcHtmlString(html);
        }

        public static List<DropDownList> DanhSachTrangThaiNhapXuatKho(int? TrangThai)
        {
            DropDownList drop = new DropDownList();
            List<DropDownList> ds = new List<DropDownList>();
            ds.Add(new DropDownList { Text = "Mới", Value = TrangThaiPhieuNhapXuat.Moi.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đang xử lý", Value = TrangThaiPhieuNhapXuat.DangXuLy.GetHashCode() });
            ds.Add(new DropDownList { Text = "Hoàn thành", Value = TrangThaiPhieuNhapXuat.HoanThanh.GetHashCode() });
            ds.Add(new DropDownList { Text = "Hủy", Value = TrangThaiPhieuNhapXuat.Huy.GetHashCode() });
            return ds;
        }

        public static MvcHtmlString LoaiPhieu(this HtmlHelper helper, int? LoaiPhieu)
        {
            string html = string.Empty;
            LoaiPhieuNhapService _loai = new LoaiPhieuNhapService();
            LoaiPhieuNhap loai = _loai.FindByKey(LoaiPhieu);
            if (loai != null)
            {
                if (loai.LoaiPhieuNhapXuat == PhieuNhapXuat.Nhap.GetHashCode())
                {
                    html = "<span class='loai-nhap'>" + loai.TenLoaiPhieuNhap + "</span>";
                }
                else if (loai.LoaiPhieuNhapXuat == PhieuNhapXuat.Xuat.GetHashCode())
                {
                    html = "<span class='loai-xuat'>" + loai.TenLoaiPhieuNhap + "</span>";
                }
            }
            return new MvcHtmlString(html);
        }

        public static IEnumerable<LoaiPhieuNhap> DanhSachLoaiPhieuNhap_Xuat(int LoaiPhieuNhapXuat)
        {
            LoaiPhieuNhapService _loai = new LoaiPhieuNhapService();
            IEnumerable<LoaiPhieuNhap> ds = _loai.DanhSachPhieuNhap(LoaiPhieuNhapXuat);
            return ds;
        }
    }
}
