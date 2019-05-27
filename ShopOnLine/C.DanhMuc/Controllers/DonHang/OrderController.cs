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
using C.Core.Helper;
using C.Core.ExModel;

namespace C.DanhMuc.Controllers
{
    public class OrderController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page, string id, string s, int? notify)
        {
            // s : OrderId

            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            if (!string.IsNullOrEmpty(s) || !string.IsNullOrWhiteSpace(s))
            {
                pageCurrent = FindPageOrder(s);
                DanhDauDaDoc(s, notify);
            }

            ListOrder(pageCurrent, id, null, s, null);
            DsDropDown(null);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost, string id, string s, int? notify, int? UserId)
        {
            // s : OrderId

            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            if (!string.IsNullOrEmpty(s) || !string.IsNullOrWhiteSpace(s))
            {
                pageCurrent = FindPageOrder(s);
                DanhDauDaDoc(s, notify);
            }

            ListOrder(pageCurrent, id, null, s, UserId);
            DsDropDown(UserId);
            return View();
        }

        public void DsDropDown(int? UserId)
        {
            qtUserService _user = new qtUserService();
            IEnumerable<qtUser> dsUser = _user.DanhSachUser(null, TypeHelper.ToInt32(User.Identity.GetUserLogin().Unitid), null);
            ViewBag.UserId = new SelectList(dsUser, "UserId", "UserName", null);
        }

        public int FindPageOrder(string OrderGuid)
        {
            return CommonHelper.FindPageOrder(
                                 OrderGuid,
                                 C.Core.Common.OrderStatus.DangXuLy.GetHashCode(),
                                 Config.PAGE_SIZE_20,
                                 User.Identity.GetUserLogin().Userid);
        }

        public void DanhDauDaDoc(string OrderGuid, int? notify)
        {
            ThongBaoService _thongbao = new ThongBaoService();

            ThongBao thongbao = _thongbao.FindByKey(notify);

            if (thongbao != null)
            {
                thongbao.DaXem = true;
                _thongbao.Update(thongbao);
            }
        }

        public PartialViewResult ListOrder(int? page, string id, int? OrderStatus, string OrderGuid, int? UserLocDieuKien)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            if (!OrderStatus.HasValue)
                OrderStatus = C.Core.Common.OrderStatus.DangXuLy.GetHashCode();

            shOrderService _order = new shOrderService();

            int? UserId = CommonHelper.KiemTraTaiKhoanCoPhaiLanhDaoDonVi(User.Identity.GetUserLogin().Userid, Config.LANH_DAO_DON_VI);

            if (UserLocDieuKien.HasValue)
            {
                UserId = UserLocDieuKien.Value;
            }

            IPagedList<shOrder> dsOrder = _order.DanhSachOrder_PhanTrang(id, pageCurrent, Config.PAGE_SIZE_20, OrderStatus, UserId, OrderGuid);

            ViewBag.ListOrder = dsOrder;
            return PartialView("ListOrder", dsOrder);
        }

        public ActionResult DsChildOrder(string OrderGuid)
        {
            shOrderDetailService _orderDetail = new shOrderDetailService();
            IEnumerable<shOrderDetail> dsOrderDetail = _orderDetail.DanhSachOrderDetailBy(OrderGuid, null, null);


            if (dsOrderDetail == null)
            {
                dsOrderDetail = new List<shOrderDetail>();
            }
            return PartialView("DsChildOrder", dsOrderDetail);
        }
        #endregion

        #region Xử lý đơn hàng
        [HttpGet]
        public ActionResult OrderProcessing(string OrderGuid)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            List<DropDownList> ds = OrderHelper.DanhSachTrangThaiGioHang(null);
            ViewBag.OrderStatus = new SelectList(ds, "Value", "Text", C.Core.Common.OrderStatus.DangGiaoHang.GetHashCode());

            if (order == null)
                order = new shOrder();

            return PartialView("OrderProcessing", order);
        }

        [HttpPost]
        public ActionResult CheckOrderProcessing(string OrderGuid, int? OrderStatus, string Description)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            string message = "OK";

            shOrderDetailService _orderdetail = new shOrderDetailService();
            IEnumerable<shOrderDetail> ds = _orderdetail.DanhSachOrderDetailBy(order.OrderGuid, order.MemberGuid, null);

            bool check = false;
            shSizeService _size = new shSizeService();
            shSetSize size = new shSetSize();
            foreach (var item in ds)
            {
                size = _size.FindByKey(item.SizeGuid);

                if (item.Number > size.Inventory)
                {
                    check = true;
                }
            }

            if (check)
            {
                message = "Số lượng hàng tồn trong kho không đủ. Xin vui lòng thao tác lại";
            }

            if (Request.IsAjaxRequest())
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        /// </summary>
        /// <param name="OrderGuid"></param>
        /// <param name="OrderStatus"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderProcessing(string OrderGuid, int? OrderStatus, string Description)
        {

            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        shGoodReceiptIsuueService _receipt = new shGoodReceiptIsuueService();
                        if (OrderStatus.HasValue)
                        {
                            if (OrderStatus == C.Core.Common.OrderStatus.DangGiaoHang.GetHashCode())
                            {
                                shOrderService _order = new shOrderService();
                                shOrder order = _order.XuLyDonhang(OrderGuid, null, User.Identity.GetUserLogin().Userid);
                            }
                            else
                            {
                                shGoodReceiptIsuue receipt = _receipt.XuatDuLieuDonHang(
                                                           OrderGuid,
                                                           OrderStatus,
                                                           Description,
                                                           User.Identity.GetUserLogin().Userid,
                                                           PhieuNhapXuat.Xuat.GetHashCode(),
                                                           TypeHelper.ToInt32(User.Identity.GetUserLogin().Unitid),
                                                           LoaiPhieuNhapXuat.BanLe.GetHashCode(),
                                                           Description,
                                                           TrangThaiPhieuNhapXuat.HoanThanh.GetHashCode(),
                                                           true,
                                                           DateTime.Now,
                                                           OrderGuid);
                            }
                        }

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }
            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Chuyển tới nhân viên xử lý đơn hàng
        [HttpGet]
        public ActionResult ChuyenXuLyDonHang(string OrderGuid)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            qtUserService _user = new qtUserService();
            IEnumerable<qtUser> ds = _user.DanhSachUser(null, TypeHelper.ToInt32(User.Identity.GetUserLogin().Unitid), null);

            ViewBag.UserId = new SelectList(ds, "UserId", "UserName", order.UserId);

            return PartialView("ChuyenXuLyDonHang", order);
        }

        [HttpPost]
        public ActionResult ChuyenXuLyDonHang(string OrderGuid, int? UserId, string OrderName)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        shOrderService _order = new shOrderService();
                        shOrder order = _order.ChuyenXuLyDonhang(OrderGuid, UserId, OrderName, User.Identity.GetUserLogin().Userid);

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Hủy đơn hàng 
        public ActionResult OrderStatus(string OrderGuid, int? Status, string Description)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (order != null)
                        {
                            shOrderHistoryService _orderHistory = new shOrderHistoryService();

                            // 1. Hủy đơn hàng
                            int UserId = 0;
                            if (User.Identity.GetUserLogin() != null)
                                UserId = User.Identity.GetUserLogin().Userid;

                            string MemberGuid = null;
                            shOrderHistory orderHistory = _orderHistory.Insert_Update(
                                        null,
                                        order.OrderGuid,
                                        order.OrderStatus,
                                        MemberGuid,
                                        Description,
                                        UserId,
                                        true,
                                        DateTime.Now);

                            order.OrderStatus = C.Core.Common.OrderStatus.HuyDonHang.GetHashCode();
                            _order.Update(order);

                            // 2. Thông báo cho member đơn hàng hủy 
                            shMemberService _member = new shMemberService();
                            shMember member = _member.FindByKey(order.MemberGuid);
                            int MemberId = member != null ? member.MemberId : 0;
                            ThongBaoService _thongbao = new ThongBaoService();
                            _thongbao.InsertOrUpdate(
                                               null,
                                               "Thông báo đơn hàng bị hủy",
                                               "Đơn hàng của bạn đã bị hủy. Nếu có thắc mắc vui lòng liên hệ với quản trị viên để biết thêm chi tiết",
                                               null,
                                               UserId,
                                               MemberId,
                                               DateTime.Now,
                                               false,
                                               Config.THONG_BAO_DA_XU_LY_DON_HANG,
                                               null
                                               );

                            // 3. gửi email thông báo hủy đơn hàng

                            string noidungdonhang = EmailHelper.NoiDungDonHang(order, new List<CartItem>());
                            string noidungEmail = EmailHelper.NoiDungMailThongBaoHuyDatHang(noidungdonhang);
                            EmailHelper.ThongBaoEmailDonHangMoiToiNguoiDatHang(member.Email, noidungEmail);

                        }

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }


            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region DetailOrder
        [HttpGet]
        public ActionResult DetailOrder(string OrderGuid)
        {
            shOrderService _service = new shOrderService();
            shOrder order = _service.FindByKey(OrderGuid);

            if (order == null)
            {
                order = new shOrder();
            }

            shOrderHistoryService _orderHistory = new shOrderHistoryService();
            IEnumerable<shOrderHistory> ds = _orderHistory.FindList().Where(x => x.OrderGuid == OrderGuid && x.Status == true).OrderBy(x => x.CreateDate);
            ViewBag.dsOrderHistory = ds;
            return PartialView("DetailOrder", order);
        }
        #endregion

        #region Banner
        public ActionResult Banner(int? Position)
        {
            shBannerService _banner = new shBannerService();

            if (!Position.HasValue)
                Position = PositionBanner.Position_GioiThieu.GetHashCode();

            shBanner banner = _banner.DanhSachBanner_ByPositionBanner(Position.Value);

            return PartialView("Banner", banner);
        }
        #endregion


    }
}
