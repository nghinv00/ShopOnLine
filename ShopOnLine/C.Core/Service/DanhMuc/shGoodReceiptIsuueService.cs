using C.Core.Common;
using C.Core.ExModel;
using C.Core.Helper;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shGoodReceiptIsuueService : BaseService<shGoodReceiptIsuue, ShopOnlineDb>
    {
        #region Danh sách

        public IEnumerable<shGoodReceiptIsuue> DanhSachPhieuXuatNhap()
        {
            shGoodReceiptIsuueService _receiptissue = new shGoodReceiptIsuueService();
            return _receiptissue.FindList().Where(x => x.Status == true);
        }
        public IEnumerable<shGoodReceiptIsuue> DanhSachPhieuXuatNhap(string TuNgay, string DenNgay, string TuKhoa, int? TrangThai, int? NguoiTao)
        {
            IEnumerable<shGoodReceiptIsuue> ds = DanhSachPhieuXuatNhap();
            if (TrangThai.HasValue)
                ds = ds.Where(x => x.TrangThai == TrangThai);

            if (NguoiTao.HasValue)
                ds = ds.Where(x => x.UserId == NguoiTao);

            DateTime _tungay = new DateTime(1970, 1, 1);
            DateTime _denngay = DateTime.Now;
            if (!string.IsNullOrEmpty(TuNgay) || !string.IsNullOrWhiteSpace(TuNgay))
            {
                _tungay = TypeHelper.ToDate(TuNgay);
            }

            if (!string.IsNullOrEmpty(TuNgay) || !string.IsNullOrWhiteSpace(TuNgay))
            {
                _denngay = TypeHelper.ToDate(DenNgay);
            }

            ds = ds.Where(x => (TypeHelper.CompareDate(x.CreateDate.Value, _tungay) ||
                                TypeHelper.CompareDate(_denngay, x.CreateDate.Value)));

            shGoodReceiptIsuueService _receiptissue = new shGoodReceiptIsuueService();
            return _receiptissue.FindList().Where(x => x.Status == true);
        }


        public IPagedList<shGoodReceiptIsuue> DanhSachPhieuXuatNhap_PhanTrang(int page, int pageSize)
        {
            IEnumerable<shGoodReceiptIsuue> ds = DanhSachPhieuXuatNhap();
            return ds.ToPagedList(page, pageSize);
        }

        #endregion

        #region Nhập dữ liệu đơn hàng
        public shGoodReceiptIsuue NhapDuLieuKhoHang(
            string ReceiptIsuueGuid,
            string SizeAdd,
            int? TrangThai,
            int? LoaiPhieu,
            int? Kho,
            string GhiChu,
            int UserId,
            int? Phieu,
            string ReceiptIsuueName,
            bool? Status,
            DateTime? CreateDate)
        {
            // 1. insert-update phiếu nhập kho
            shGoodReceiptIsuue receipt = Insert_Update(
                ReceiptIsuueGuid,
                null,
                Phieu,
                ReceiptIsuueName,
                null,
                Kho,
                null,
                LoaiPhieu,
                null,
                GhiChu,
                UserId,
                TrangThai,
                Status,
                CreateDate);

            // 2. chi tiết phiếu 
            shGoodReceiptIsuueDetailService _receiptDetail = new shGoodReceiptIsuueDetailService();
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
                            // 1. Thêm mới chi tiết đơn hàng nhập
                            shGoodReceiptIsuueDetail receiptDetail = _receiptDetail.Insert_Update(
                               obj[2],
                               null,
                               receipt.ReceiptIsuueGuid,
                               size.ProductGuid,
                               size.SectionGuid,
                               size.SizeGuid,
                               TypeHelper.ToInt32(obj[1]),
                               true,
                               CreateDate,
                               Phieu
                               );

                            // 2.  cập nhật số lượng shSize 
                            size.Number += receiptDetail.Number;
                            size.Inventory += receiptDetail.Number;

                            _size.Update(size);

                        }
                    }
                }
            }

            return receipt;
        }
        #endregion

        #region Xuất dữ liệu kho hàng
        public shGoodReceiptIsuue XuatDuLieuKhoHang(
           string ReceiptIsuueGuid,
           string SizeAdd,
           int? TrangThai,
           int? LoaiPhieu,
           int? Kho,
           string GhiChu,
           int UserId,
           int? Phieu,
           string ReceiptIsuueName,
           bool? Status,
           DateTime? CreateDate)
        {
            // 1. insert-update phiếu nhập kho
            shGoodReceiptIsuue receipt = Insert_Update(
                ReceiptIsuueGuid,
                null,
                Phieu,
                ReceiptIsuueName,
                null,
                Kho,
                null,
                LoaiPhieu,
                null,
                GhiChu,
                UserId,
                TrangThai,
                Status,
                CreateDate);

            // 2. chi tiết phiếu 
            shGoodReceiptIsuueDetailService _receiptDetail = new shGoodReceiptIsuueDetailService();
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
                            // 1. Thêm chi tiết đơn hàng nhập
                            shGoodReceiptIsuueDetail receiptDetail = _receiptDetail.Insert_Update(
                                   obj[2],
                                   null,
                                   receipt.ReceiptIsuueGuid,
                                   size.ProductGuid,
                                   size.SectionGuid,
                                   size.SizeGuid,
                                   TypeHelper.ToInt32(obj[1]),
                                   true,
                                   CreateDate,
                                   Phieu
                                   );

                            // 2.  cập nhật số lượng shSize 
                            //size.Number += receiptDetail.Number;
                            size.Inventory -= receiptDetail.Number;
                            _size.Update(size);

                        }
                    }
                }
            }
            return receipt;
        }
        #endregion

        #region Xuất dữ liệu đơn hàng
        public shGoodReceiptIsuue XuatDuLieuDonHang(string OrderGuid, int? OrderStatus, string Description, int UserId, int Phieu, int MaKho, int LoaiPhieu, string GhiChu, int TrangThai, bool? Status, DateTime? CreateDate, string MaDonHang)
        {
            // 1. Cập nhật trạng thái đơn hàng
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);
            order.OrderStatus = OrderStatus;
            _order.Update(order);

            shGoodReceiptIsuue receipt = new shGoodReceiptIsuue();
            if (OrderStatus != C.Core.Common.OrderStatus.HuyDonHang.GetHashCode())
            {
                // 2. ghi lịch sử cập nhật đơn hàng 
                shOrderHistoryService _orderHistory = new shOrderHistoryService();
                shOrderHistory orderHistory = _orderHistory.Insert_Update(
                            null,
                            order.OrderGuid,
                            OrderStatus,
                            null,
                            Description,
                            UserId,
                            true,
                            DateTime.Now);

                // 3. Tạo hóa đơn xuất kho
                receipt = Insert_Update(
                                null,
                                null,
                                Phieu,
                                null,
                                null,
                                MaKho,
                                null,
                                LoaiPhieu,
                                MaDonHang,
                                GhiChu,
                                UserId,
                                TrangThai,
                                Status,
                                CreateDate
                                );


                // 4 Cập nhật số lượng tồn của mỗi sản phẩm 
                shOrderDetailService _orderdetail = new shOrderDetailService();
                IEnumerable<shOrderDetail> ds = _orderdetail.DanhSachOrderDetailBy(order.OrderGuid, order.MemberGuid, null);

                shSizeService _size = new shSizeService();
                shSetSize size = new shSetSize();
                foreach (var item in ds)
                {

                    size = _size.FindByKey(item.SizeGuid);
                    if (size == null)
                        size = new shSetSize();

                    // 5.. Tạo chi tiết hóa đơn xuất hàng hóa 
                    shGoodReceiptIsuueDetailService _receiptDetail = new shGoodReceiptIsuueDetailService();
                    shGoodReceiptIsuueDetail receiptDetail = _receiptDetail.Insert_Update(
                                    null,
                                    null,
                                    receipt.ReceiptIsuueGuid,
                                    size.ProductGuid,
                                    size.SectionGuid,
                                    size.SizeGuid,
                                    item.Number,
                                    Status,
                                    CreateDate,
                                    Phieu);

                    // 6.Update số lượng tồn ở bảng size 

                    size.Inventory = size.Inventory - item.Number;
                    _size.Update(size);
                }

                // 5. Thông báo cho Khach hàng biết đơn hàng đã xử lý
                shMemberService _member = new shMemberService();
                shMember member = _member.FindByKey(order.MemberGuid);
                int MemberId = member != null ? member.MemberId : 0;
                ThongBaoService _thongbao = new ThongBaoService();
                _thongbao.InsertOrUpdate(
                                   null,
                                   "Thông báo đơn hàng đang trong quá trình xử lý",
                                   "Đơn hàng của bạn đang trong quá trình vận chuyển. Vui lòng kiểm tra thông tin cá nhân trong quá trình chúng tôi vận chuyển sản phẩm",
                                   null,
                                   UserId,
                                   MemberId,
                                   DateTime.Now,
                                   false,
                                   Config.THONG_BAO_DA_XU_LY_DON_HANG,
                                   null
                                   );

                // 6. Gửi Email báo xử lý đơn hàng

                string noidungdonhang = EmailHelper.NoiDungDonHang(order, new List<CartItem>());
                string noidungEmail = EmailHelper.NoiDungMailThongBaoXuLyDatHang(noidungdonhang);

                EmailHelper.ThongBaoEmailDonHangMoiToiNguoiDatHang(member.Email, noidungEmail);
            }


            return receipt;
        }
        #endregion

        #region MyRegion
        public shGoodReceiptIsuue Insert_Update(
           string ReceiptIsuueGuid,
           int? ReceiptIssueId,
           int? Phieu,
           string ReceiptIsuueName,
           string SKU,
           int? MaKho,
           string TenKho,
           int? LoaiPhieu,
           string MaDonHang,
           string GhiChu,
           int? UserId,
           int? TrangThai,
           bool? Status,
           DateTime? CreateDate
           )
        {

            shGoodReceiptIsuueService _receipt = new shGoodReceiptIsuueService();
            shGoodReceiptIsuue receipt = new shGoodReceiptIsuue();

            if (!string.IsNullOrEmpty(ReceiptIsuueGuid) || !string.IsNullOrWhiteSpace(ReceiptIsuueGuid))
                receipt = _receipt.FindByKey(ReceiptIsuueGuid);
            else
            {
                receipt.ReceiptIsuueGuid = GuidUnique.getInstance().GenerateUnique();


                qtUnitService _unit = new qtUnitService();
                string _TenKho = _unit.UnitName(MaKho);
                if (string.IsNullOrEmpty(receipt.ReceiptIsuueName) && string.IsNullOrWhiteSpace(receipt.ReceiptIsuueName))
                {
                    receipt.ReceiptIsuueName = StringHelper.ToUnsignString(_TenKho) + "-" + receipt.CreateDate.GetValueOrDefault(DateTime.Now).ToString("dd-MM-yyyy");
                }
                receipt.TenKho = _TenKho;
            }


            receipt.Phieu = Phieu;

            receipt.SKU = SKU;
            receipt.MaKho = MaKho;

            receipt.LoaiPhieu = LoaiPhieu;
            receipt.MaDonHang = MaDonHang;
            receipt.GhiChu = GhiChu;
            receipt.UserId = UserId;
            receipt.TrangThai = TrangThai;
            receipt.Status = Status;
            receipt.CreateDate = CreateDate;

            if (receipt.ReceiptIssueId > 0)
                _receipt.Update(receipt);
            else
                _receipt.Insert(receipt);

            return receipt;
        }
        #endregion
    }
}
