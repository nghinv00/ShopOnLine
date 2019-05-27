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

namespace C.DanhMuc.Controllers
{
    public class DoanhThuController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page, string StartTime, string EndTime)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            LoadDuLieu(StartTime, EndTime);

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost, string StartTime, string EndTime)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            LoadDuLieu(StartTime, EndTime);

            return View();
        }

        public void LoadDuLieu(string StartTime, string EndTime)
        {
            DateTime _tungay = DateTime.Now;
            if (!string.IsNullOrEmpty(StartTime) || !string.IsNullOrWhiteSpace(StartTime))
            {
                _tungay = TypeHelper.ToDate(StartTime);
            }

            DateTime _denngay = DateTime.Now;
            if (!string.IsNullOrEmpty(EndTime) || !string.IsNullOrWhiteSpace(EndTime))
            {
                _denngay = TypeHelper.ToDate(EndTime);
            }

            #region Tổng số đơn hàng
            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> dsDonHang = _order.DanhSachOrder_TheoThoiGian(_tungay, _denngay);
            @ViewBag.TongSoDonHang = dsDonHang.Count();
            decimal tonggiatri = 0M;
            foreach (var item in dsDonHang)
            {
                tonggiatri += item.Total.GetValueOrDefault(0) - item.FeeShip.GetValueOrDefault(0);
            }
            @ViewBag.TongGiaTri = Format.FormatDecimalToString(tonggiatri);
            #endregion

            #region Doanh thu
            IEnumerable<shOrder> dsDonhangHoanThanh = dsDonHang.Where(x => x.OrderStatus == OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode() ||
                                                         x.OrderStatus == OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode());
            @ViewBag.DonHoanThanh = dsDonhangHoanThanh.Count();
            tonggiatri = 0M;
            foreach (var item in dsDonhangHoanThanh)
            {
                tonggiatri += item.Total.GetValueOrDefault(0) - item.FeeShip.GetValueOrDefault(0);
            }
            @ViewBag.DoanhThu = Format.FormatDecimalToString(tonggiatri);
            #endregion

            @ViewBag.DonHangHuy = dsDonHang.Where(x => x.OrderStatus == OrderStatus.HuyDonHang.GetHashCode()).Count();

            ViewBag.StartEnd = _tungay.ToString("dd/MM/yyyy") + " - " + _denngay.ToString("dd/MM/yyyy");
            ViewBag.StartTime = _tungay.ToString("dd/MM/yyyy");
            ViewBag.EndTime = _denngay.ToString("dd/MM/yyyy");
        }
        #region Kinh doanh
        public ActionResult TongGiaTriTheoTrangThaiDonHang(string StartTime, string EndTime)
        {
            DateTime _tungay = DateTime.Now;
            if (!string.IsNullOrEmpty(StartTime) || !string.IsNullOrWhiteSpace(StartTime))
            {
                _tungay = TypeHelper.ToDate(StartTime);
            }

            DateTime _denngay = DateTime.Now;
            if (!string.IsNullOrEmpty(EndTime) || !string.IsNullOrWhiteSpace(EndTime))
            {
                _denngay = TypeHelper.ToDate(EndTime);
            }

            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> dsDonHang = _order.DanhSachOrder_TheoThoiGian(_tungay, _denngay);

            List<BieuDoDonHang> ds = new List<BieuDoDonHang>();
            BieuDoDonHang donhang = new BieuDoDonHang();

            // 1. Đang xử lý
            IEnumerable<shOrder> dsTheoTrangThai = _order.DanhSachOrder_ByStatus(
                                    dsDonHang, null, OrderStatus.DangXuLy.GetHashCode());
            ds.Add(VeBieuDoKinhDoanh(dsTheoTrangThai, "Đang xử lý"));

            // 2 . Đang giao hàng
            dsTheoTrangThai = _order.DanhSachOrder_ByStatus(
                                   dsDonHang, null, OrderStatus.DangGiaoHang.GetHashCode());
            ds.Add(VeBieuDoKinhDoanh(dsTheoTrangThai, "Đang giao hàng"));

            // Đã giao hàng
            dsTheoTrangThai = _order.DanhSachOrder_ByStatus(
                                 dsDonHang, null, OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode());
            ds.Add(VeBieuDoKinhDoanh(dsTheoTrangThai, "Đã giao hàng"));

            // Đã Hủy
            dsTheoTrangThai = _order.DanhSachOrder_ByStatus(
                                 dsDonHang, null, OrderStatus.HuyDonHang.GetHashCode());
            ds.Add(VeBieuDoKinhDoanh(dsTheoTrangThai, "Đã hủy"));

            if (Request.IsAjaxRequest())
            {
                return Json(ds, JsonRequestBehavior.AllowGet);
            }
            return PartialView("TongGiaTriTheoTrangThaiDonHang", dsDonHang);
        }

        public BieuDoDonHang VeBieuDoKinhDoanh(IEnumerable<shOrder> dsTheoTrangThai, string label)
        {
            BieuDoDonHang donhang = new BieuDoDonHang();

            donhang.label = label;
            donhang.y = dsTheoTrangThai.Count();
            return donhang;
        }
        #endregion

        #region Top 10
        public ActionResult TopSanPham(string StartTime, string EndTime, string TheLoai)
        {
            DateTime _tungay = DateTime.Now;
            if (!string.IsNullOrEmpty(StartTime) || !string.IsNullOrWhiteSpace(StartTime))
            {
                _tungay = TypeHelper.ToDate(StartTime);
            }

            DateTime _denngay = DateTime.Now;
            if (!string.IsNullOrEmpty(EndTime) || !string.IsNullOrWhiteSpace(EndTime))
            {
                _denngay = TypeHelper.ToDate(EndTime);
            }

            List<BieuDoTop> dsBieuDo = new List<BieuDoTop>();
            BieuDoTop top = new BieuDoTop();
            shOrderDetailService _orderDetail = new shOrderDetailService();
            shProductService _product = new shProductService();
            ShopOnlineDb db = new ShopOnlineDb();

            IEnumerable<shOrderDetail> dsOrderDetail = _orderDetail.DanhSachOrderDetail_TheoThoiGian(_tungay, _denngay);
            int TongSoLuong = TinhTongSoLuong(dsOrderDetail);
            IEnumerable<string> dsDistinct = dsOrderDetail.Select(x => x.ProductGuid).Distinct();
            foreach (var item in dsDistinct)
            {
                IEnumerable<shOrderDetail> dsTheoSanPham = dsOrderDetail.Where(x => x.ProductGuid == item);
                top = new BieuDoTop();
                top.label = _product.ProductName(item);
                top.y = TinhTongSoLuong(dsTheoSanPham);

                if (TheLoai == Config.SoLuong)
                    top.indexLabel = top.y.ToString();
                else if (TheLoai == Config.DoanhThu)
                    top.indexLabel = Format.FormatDecimalToString(TinhTongSoTien(dsTheoSanPham));

                dsBieuDo.Add(top);
            }

            dsBieuDo = dsBieuDo.OrderBy(x => x.y).Take(Config.Top_10).ToList();

            if (Request.IsAjaxRequest())
            {
                return Json(dsBieuDo, JsonRequestBehavior.AllowGet);
            }
            return PartialView("TopSanPhamTheoSoLuong");

        }


        public int TinhTongSoLuong(IEnumerable<shOrderDetail> dsTheoSanPham)
        {
            int tongsoluong = 0;

            foreach (var item in dsTheoSanPham)
            {
                tongsoluong += item.Number.GetValueOrDefault(0);
            }

            return tongsoluong;
        }

        public decimal TinhTongSoTien(IEnumerable<shOrderDetail> dsTheoSanPham)
        {
            decimal tongsotien = 0M;

            foreach (var item in dsTheoSanPham)
            {
                tongsotien += item.Prices.GetValueOrDefault(0);
            }

            return tongsotien;
        }
        #endregion
        #endregion

        public class BieuDoDonHang
        {
            public string label { get; set; }
            public int y { get; set; }
        }

        public class BieuDoTop
        {
            public string label { get; set; }
            public int y { get; set; }
            public string indexLabel { get; set; }
        }
    }
}