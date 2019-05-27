using C.Core.Common;
using C.Core.ExModel;
using C.Core.Helper;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shOrderService : BaseService<shOrder, ShopOnlineDb>
    {

        #region Get DS
        public IEnumerable<shOrder> DanhSachOrder()
        {
            shOrderService _order = new shOrderService();

            return _order.FindList().Where(x => x.Status == true).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<shOrder> DanhSachOrder_TheoThoiGian(DateTime StartTime, DateTime EndTime)
        {
            StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 0, 0, 1);
            EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, 23, 59, 59);
            IEnumerable<shOrder> ds = DanhSachOrder();

            ds = ds.Where(x => (TypeHelper.CompareDate(x.NgayDat.GetValueOrDefault(DateTime.Now), StartTime)
                               && TypeHelper.CompareDate(EndTime, x.NgayDat.GetValueOrDefault(DateTime.Now))
                               ));

            return ds;
        }

        public IEnumerable<shOrder> DanhSachOrder_ByStatus(string MemberGuid, int? OrderStatus, int? UserId)
        {
            IEnumerable<shOrder> ds = DanhSachOrder_ByStatus(MemberGuid, OrderStatus);

            if (UserId.HasValue)
            {
                ds = ds.Where(x => x.UserId == UserId);
            }

            return ds;
        }

        public IPagedList<shOrder> DanhSachOrder_PhanTrang(string MemberGuid, int page, int pageSize, int? OrderStatus, int? UserId, string OrderGuid)
        {
            IEnumerable<shOrder> ds = DanhSachOrder_ByStatus(MemberGuid, OrderStatus, UserId);

            if (!string.IsNullOrWhiteSpace(OrderGuid) || !string.IsNullOrEmpty(OrderGuid))
                ds = ds.Where(x => x.OrderGuid == OrderGuid);

                return ds.ToPagedList(page, pageSize);
        }

        public IEnumerable<shOrder> DanhSachOrder_ByStatus(string MemberGuid, int? OrderStatus)
        {
            IEnumerable<shOrder> ds = DanhSachOrder();

            if (!string.IsNullOrWhiteSpace(MemberGuid) || !string.IsNullOrEmpty(MemberGuid))
                ds = ds.Where(x => x.MemberGuid == MemberGuid);

            if (OrderStatus.HasValue)
            {
                if (OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode() ||
                        OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode())
                {
                    ds = ds.Where(x => x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode()
                                        || x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode());
                }
                else
                {
                    ds = ds.Where(x => x.OrderStatus == OrderStatus);
                }
            }

            return ds.OrderBy(x  => x.OrderId);
        }

        public IEnumerable<shOrder> DanhSachOrder_ByStatus(IEnumerable<shOrder> ds, string MemberGuid, int? OrderStatus)
        {
            if (!string.IsNullOrWhiteSpace(MemberGuid) || !string.IsNullOrEmpty(MemberGuid))
                ds = ds.Where(x => x.MemberGuid == MemberGuid);

            if (OrderStatus.HasValue)
            {
                if (OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode())
                {
                    ds = ds.Where(x => x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode()
                                        || x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode());
                }
                else
                {
                    ds = ds.Where(x => x.OrderStatus == OrderStatus);

                }
            }


            return ds;
        }
        #endregion

        #region Inset - Update
        public shOrder Insert_Update(
           string OrderGuid,
           int? OrderId,
           string OrderName,
           string MemberGuid,
           string FullName,
           string Email,
           string Phone,
           string Address,
           decimal? Total,
           int? OrderStatus,
           int? SortOrder,
           int? DistrictId,
           int? ProvinceId,
           decimal FeeShip,
           bool? Status,
           DateTime? CreateDate,
           int? PayType,
           DateTime? NgayDat,
           int? UserId
            )
        {
            shOrderService _order = new shOrderService();
            shOrder order = new shOrder();

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                if (!string.IsNullOrWhiteSpace(OrderGuid) || !string.IsNullOrEmpty(OrderGuid))
                    order = _order.FindByKey(OrderGuid);
                else
                    order.OrderGuid = GuidUnique.getInstance().GenerateUnique();

                //order.OrderGuid = OrderGuid;
                order.MemberGuid = MemberGuid;
                order.FullName = FullName;
                order.Email = Email;
                order.Phone = Phone;
                order.Address = Address;
                order.Total = Total;
                order.OrderStatus = OrderStatus;
                order.SortOrder = SortOrder;
                order.Status = Status;
                order.CreateDate = CreateDate;
                order.FeeShip = FeeShip;
                order.PayType = PayType;
                order.NgayDat = NgayDat;

                order.UserId = UserId;

                if (order.OrderId > 0)
                    _order.Update(order);
                else
                    _order.Insert(order);

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

            }

            return order;
        }
        #endregion

        #region Báo cáo
        public IEnumerable<shOrder> DanhSachDonHangTrongNgay(DateTime dt)
        {
            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> ds = DanhSachOrder().Where(x =>
                                                    x.NgayDat.GetValueOrDefault(DateTime.Now).Date == dt.Date
                                                    && x.Status == true
                                                    && x.OrderStatus != C.Core.Common.OrderStatus.HuyDonHang.GetHashCode()
                                                    );

            return ds;
        }

        public int SoDonHangTrongNgay(DateTime dt)
        {
            IEnumerable<shOrder> dsDonhang = DanhSachDonHangTrongNgay(dt);

            return dsDonhang.Count();
        }

        public decimal DoanhThuTheoNgay(DateTime dt)
        {
            IEnumerable<shOrder> dsDonhang = DanhSachDonHangTrongNgay(dt);

            //.Where(x => (x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode()) ||
            //(x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode()));

            decimal doanhthu = 0M;
            foreach (var item in dsDonhang)
            {
                doanhthu += item.Total.GetValueOrDefault(0) - item.FeeShip.GetValueOrDefault(0);
            }
            return doanhthu;
        }

        public IEnumerable<shOrder> DanhSachDonHangTrongThang(int Month, int Year)
        {
            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> ds = DanhSachOrder().Where(x =>
                                                      x.NgayDat.GetValueOrDefault(DateTime.Now).Date.Month == Month &&
                                                      x.NgayDat.GetValueOrDefault(DateTime.Now).Date.Year == Year
                                                      && (x.OrderStatus != C.Core.Common.OrderStatus.HuyDonHang.GetHashCode()
                                                      // || x.OrderStatus == C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode()
                                                      )
                                                      );
            return ds;
        }

        public IEnumerable<shOrder> SoDonTrongNgay(IEnumerable<shOrder> dsOrder, int Day)
        {
            dsOrder = dsOrder.Where(x => x.NgayDat.GetValueOrDefault(DateTime.Now).Date.Day == Day);

            return dsOrder;

        }

        public decimal DoanhThuTrongNgay(IEnumerable<shOrder> dsOrder, int Day)
        {
            dsOrder = SoDonTrongNgay(dsOrder, Day);

            decimal doanhthu = 0M;
            foreach (var item in dsOrder)
            {
                doanhthu += item.Total.GetValueOrDefault(0) - item.FeeShip.GetValueOrDefault(0);
            }

            return doanhthu;

        }
        #endregion

        #region Doanh thu 

        #endregion

        #region Chuyển xử lý đơn hàng 
        public shOrder ChuyenXuLyDonhang(string OrderGuid, int? UserId, string OrderName, int NguoiThongBaoId)
        {
            try
            {
                // 1. Cập nhật trạng thái người xử lý đơn hàng 
                shOrderService _order = new shOrderService();
                shOrder order = _order.FindByKey(OrderGuid);
                order.UserId = UserId;
                order.OrderName = OrderName;
                _order.Update(order);

                // 2. Ghi lại lịch sử 

                // 3. Thông báo cho người nhận 
                ThongBaoService _thongbao = new ThongBaoService();
                _thongbao.InsertOrUpdate(
                       null,
                       "Thông báo đơn hàng cần xử lý",
                       "Bạn có đơn hàng cần xử lý. Vui lòng truy cập hệ thống để xử lý",
                       "/DanhMuc/Order/Index/s=" + order.OrderGuid,
                       NguoiThongBaoId,
                       UserId,
                       DateTime.Now,
                       false,
                       Config.THONG_BAO_DON_HANG_XU_LY,
                       null
                       );

                return order;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return new shOrder();
            }

            return new shOrder();

        }
        #endregion


        #region Xử lý đơn hàng  
        public shOrder XuLyDonhang(string OrderGuid, string OrderName, int NguoiThongBaoId)
        {
            try
            {
                // 1. Cập nhật trạng thái người xử lý đơn hàng 
                shOrderService _order = new shOrderService();
                shOrder order = _order.FindByKey(OrderGuid);
                //order.UserId = UserId;
                order.OrderName = OrderName;
                order.OrderStatus = OrderStatus.DangGiaoHang.GetHashCode();
                _order.Update(order);

                // 2. Ghi lại lịch sử 
                // 3. Thông báo cho khách hàng tiến trình xử lý đơn hàng 
                ThongBaoService _thongbao = new ThongBaoService();
                shMemberService _member = new shMemberService();
                shMember member = _member.FindByKey(order.MemberGuid);
                int MemberId = member != null ? member.MemberId : 0;

                _thongbao.InsertOrUpdate(
                       null,
                       "Thông báo xử lý đơn hàng",
                       "Đơn hàng của bạn đã được tiếp nhận và xử lý. Nếu có thắc mắc vui lòng liên hệ với quản trị viên để biết thêm chi tiết",
                       null,
                       NguoiThongBaoId,
                       MemberId,
                       DateTime.Now,
                       false,
                       Config.THONG_BAO_DON_HANG_XU_LY,
                       null
                       );

                // 4. Gửi Email thông báo đơn hàng

                string noidungdonhang = EmailHelper.NoiDungDonHang(order, new List<CartItem>());
                string noidungEmail = EmailHelper.NoiDungMailThongBaoXuLyDatHang(noidungdonhang);

                EmailHelper.ThongBaoEmailDonHangMoiToiNguoiDatHang(member.Email, noidungEmail);
                return order;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return new shOrder();
            }

            return new shOrder();

        }
        #endregion
        #region Thông báo 

        #endregion
    }
}
