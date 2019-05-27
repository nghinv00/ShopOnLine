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
    public class ProductManageController : BaseController
    {
        #region Phiếu nhập - xuất kho
        [HttpGet]
        public ActionResult Index(int? page, string TuNgay, string DenNgay, string TuKhoa, int? TrangThai, int? NguoiTao)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            DropDownList(TuNgay, DenNgay, TuKhoa, TrangThai, NguoiTao);
            ListReceiptIsuue(pageCurrent, TuNgay, DenNgay, TuKhoa, TrangThai, NguoiTao);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, string TuNgay, string DenNgay, string TuKhoa, int? TrangThai, int? NguoiTao, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            DropDownList(TuNgay, DenNgay, TuKhoa, TrangThai, NguoiTao);
            ListReceiptIsuue(pageCurrent, TuNgay, DenNgay, TuKhoa, TrangThai, NguoiTao);
            return View();
        }

        public PartialViewResult ListReceiptIsuue(int? page, string TuNgay, string DenNgay, string TuKhoa, int? TrangThai, int? NguoiTao)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shGoodReceiptIsuueService _receiptissue = new shGoodReceiptIsuueService();
            IEnumerable<shGoodReceiptIsuue> dsPhieuNhapXuat = _receiptissue.DanhSachPhieuXuatNhap(TuNgay, DenNgay, TuKhoa, TrangThai, NguoiTao)
                                                                           .OrderByDescending(x => x.CreateDate);

            ViewBag.ListCategory = dsPhieuNhapXuat;
            return PartialView("ListReceiptIsuue", dsPhieuNhapXuat);
        }

        public void DropDownList(string TuNgay, string DenNgay, string TuKhoa, int? TrangThai, int? NguoiTao)
        {
            /*
             * Trạng thái đơn nhập , xuất hàng
             */

            ViewBag.TuNgay = TuNgay;
            ViewBag.DenNgay = DenNgay;
            ViewBag.TuKhoa = TuKhoa;

            List<DropDownList> ds = GoodReceiptIsuueHelper.DanhSachTrangThaiNhapXuatKho(null);
            ViewBag.TrangThai = new SelectList(ds, "Value", "Text", TrangThai);

            qtUserService _user = new qtUserService();
            IEnumerable<qtUser> dsU = _user.DanhSachUser(null, TypeHelper.ToInt32(User.Identity.GetUserLogin().Unitid), null);
            ViewBag.NguoiTao = new SelectList(dsU, "UserId", "UserName", NguoiTao);

        }
        #endregion

        #region Theo dõi tồn kho sản phẩm

        #endregion

        #region Nhập
        [HttpGet]
        public ActionResult Nhap(string id)
        {
            shGoodReceiptIsuueService _good = new shGoodReceiptIsuueService();

            shGoodReceiptIsuue good = _good.FindByKey(id);
            if (good == null)
            {
                good = new shGoodReceiptIsuue();
            }
            if (good.ReceiptIssueId == 0)
            {
                good.TrangThai = TrangThaiPhieuNhapXuat.HoanThanh.GetHashCode();
            }

            dsNhap(id);
            DropNhapDuLieu(good.LoaiPhieu, good.MaKho, good.TrangThai, PhieuNhapXuat.Nhap.GetHashCode());

            return View(good);
        }

        public void DropNhapDuLieu(int? _LoaiPhieu, int? MaKho, int? TrangThai, int PhieuXuatNhap)
        {
            LoaiPhieuNhapService _loai = new LoaiPhieuNhapService();
            IEnumerable<LoaiPhieuNhap> LoaiPhieu = _loai.DanhSachPhieuNhap(PhieuXuatNhap);
            ViewBag.LoaiPhieu = new SelectList(LoaiPhieu, "LoaiPhieuNhapId", "TenLoaiPhieuNhap", _LoaiPhieu);

            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> dsUnit = _unit.FindList().Where(x => x.ParentId != null && x.ParentId != 0);
            ViewBag.Kho = new SelectList(dsUnit, "UnitId", "UnitName", MaKho);

            ViewBag.SizeGuid = new SelectList(new List<shSetSize>(), "Value", "Text");

            ViewBag.TrangThai = new SelectList(GoodReceiptIsuueHelper.DanhSachTrangThaiNhapXuatKho(null), "Value", "Text", TrangThai);
        }

        public ActionResult dsNhap(string ReceiptIsuueGuid)
        {
            shGoodReceiptIsuueDetailService _goodDetail = new shGoodReceiptIsuueDetailService();
            List<shGoodReceiptIsuueDetail> dsNhap = _goodDetail.DanhSachPhieuXuatNhap_ByParent(ReceiptIsuueGuid).ToList();
            if (dsNhap == null)
            {
                dsNhap = new List<shGoodReceiptIsuueDetail>();
            }
            shSizeService _size = new shSizeService();

            ViewBag.ReceiptIsuueGuid = ReceiptIsuueGuid;
            ViewBag.dsNhap = dsNhap;

            return PartialView("dsNhap", dsNhap);
        }

        public ActionResult dsNhap2(string ReceiptIsuueGuid, string SizeGuid, int? Number, string SizeAdd)
        {
            shGoodReceiptIsuueDetailService _goodDetail = new shGoodReceiptIsuueDetailService();
            List<shGoodReceiptIsuueDetail> dsNhap = new List<shGoodReceiptIsuueDetail>();

            shSizeService _size = new shSizeService();
            shSetSize size = new shSetSize();
            if (!string.IsNullOrEmpty(SizeAdd) || !string.IsNullOrWhiteSpace(SizeAdd))
            {
                string[] dsSizeAdd = SizeAdd.Split(';');

                foreach (var childSize in dsSizeAdd)
                {
                    string[] obj = childSize.Split('$');

                    if (obj != null)
                    {
                        size = _size.FindByKey(obj[0]);
                        if (size != null)
                        {
                            dsNhap.Add(new shGoodReceiptIsuueDetail
                            {
                                ReceiptIsuueDetailGuid = obj[2],
                                ReceiptIsuueGuid = ReceiptIsuueGuid,
                                ProductGuid = size.ProductGuid,
                                SectionGuid = size.SectionGuid,
                                SizeGuid = size.SizeGuid,
                                Number = TypeHelper.ToInt32(obj[1]),
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(SizeGuid) || !string.IsNullOrWhiteSpace(SizeGuid))
            {
                size = _size.FindByKey(SizeGuid);
                dsNhap.Add(new shGoodReceiptIsuueDetail
                {
                    // ReceiptIsuueDetailGuid = GuidUnique.getInstance().GenerateUnique(),
                    ReceiptIsuueGuid = ReceiptIsuueGuid,
                    ProductGuid = size.ProductGuid,
                    SectionGuid = size.SectionGuid,
                    SizeGuid = size.SizeGuid,
                    Number = Number,
                });

                SizeAdd += SizeGuid + "$" + Number + "$" + "" + ";";
            }

            ViewBag.ReceiptIsuueGuid = ReceiptIsuueGuid;
            ViewBag.dsNhap = dsNhap;

            ViewBag.SizeAdd = SizeAdd;

            return PartialView("dsNhap", dsNhap);
        }

        public ActionResult XoaSanPhamDaChon(string ReceiptIsuueGuid, string SizeGuid, string SizeAdd)
        {
            string dsSize = string.Empty;
            List<shGoodReceiptIsuueDetail> dsNhap = new List<shGoodReceiptIsuueDetail>();
            shSizeService _size = new shSizeService();
            shSetSize size = new shSetSize();
            if (!string.IsNullOrEmpty(SizeAdd) || !string.IsNullOrWhiteSpace(SizeAdd))
            {
                string[] dsSizeAdd = SizeAdd.Split(';');

                foreach (var childSize in dsSizeAdd)
                {
                    string[] obj = childSize.Split('$');

                    if (obj != null && obj[0] != "" && obj[0] != null && obj[0] != SizeGuid)
                    {
                        dsSize += obj[0] + "$" + obj[1] + "$" + obj[2] + "$" + ";";
                    }
                }
            }

            return dsNhap2(ReceiptIsuueGuid, null, null, dsSize);
        }

        [HttpPost]
        public ActionResult Nhap(string ReceiptIsuueGuid, string SizeAdd, int? TrangThai, int? LoaiPhieu, int? Kho, string GhiChu, string ReceiptIsuueName)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Kho = 2;

                        shGoodReceiptIsuueService _receipt = new shGoodReceiptIsuueService();
                        _receipt.NhapDuLieuKhoHang(
                            ReceiptIsuueGuid, SizeAdd,
                            TrangThai,
                            LoaiPhieu,
                            Kho,
                            GhiChu,
                            User.Identity.GetUserLogin().Userid,
                            PhieuNhapXuat.Nhap.GetHashCode(),
                            ReceiptIsuueName,
                            true,
                            DateTime.Now
                            );

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

        #region Xuất
        [HttpGet]
        public ActionResult Xuat(string id)
        {
            shGoodReceiptIsuueService _good = new shGoodReceiptIsuueService();

            shGoodReceiptIsuue good = _good.FindByKey(id);
            if (good == null)
            {
                good = new shGoodReceiptIsuue();
            }
            dsNhap(id);
            DropNhapDuLieu(good.LoaiPhieu, good.MaKho, good.TrangThai, PhieuNhapXuat.Xuat.GetHashCode());

            return View(good);
        }

        public ActionResult Xuat(string ReceiptIsuueGuid, string SizeAdd, int? TrangThai, int? LoaiPhieu, int? Kho, string GhiChu, string ReceiptIsuueName)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Kho = 2;

                        shGoodReceiptIsuueService _receipt = new shGoodReceiptIsuueService();
                        _receipt.XuatDuLieuKhoHang(
                            ReceiptIsuueGuid,
                            SizeAdd,
                            TrangThai,
                            LoaiPhieu,
                            Kho,
                            GhiChu,
                            User.Identity.GetUserLogin().Userid,
                            PhieuNhapXuat.Xuat.GetHashCode(),
                            ReceiptIsuueName,
                            true,
                            DateTime.Now
                            );

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

        public ActionResult divProduct()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> ds = _category.DanhSachCategory();

            return PartialView("divProduct", ds);
        }

        public ActionResult SizeGuid(string ProductGuid)
        {
            shSizeService _size = new shSizeService();

            IEnumerable<shSetSize> dsSize = _size.DanhSachSection_Size(ProductGuid);

            ViewBag.SizeGuid = new SelectList(dsSize, "SizeGuid", "SizeName", null);

            return PartialView("SizeGuid", ViewBag.SizeGuid);

        }

    }
}
