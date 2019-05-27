using C.Core.Common;
using C.Core.CustomController;
using C.Core.ExModel;
using C.Core.Helper;
using C.Core.Model;
using C.Core.Service;
using C.Membership.Helper;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Giỏ hàng
    public class CartController : CustomController
    {
        private const string CartSession = "CartSession";

        #region Index
        [HttpGet]
        public ActionResult Index(int? p)
        {
            string Error = string.Empty;

            if (TempData["Cart"] != null)
            {
                Error = TempData["Cart"] as string;
                TempData["Cart"] = null;
            }


            ViewBag.Error = Error;
            DsProduct(null, p);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? p, string SizeGuid, int? SoLuong)
        {
            string Error = string.Empty;

            if (TempData["Cart"] != null)
            {
                Error = TempData["Cart"] as string;
                TempData["Cart"] = null;
            }

            if (!SoLuong.HasValue)
            {
                SoLuong = 0;
            }

            List<CartItem> list = (List<CartItem>)Session[CartSession];
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item.SizeGuid == SizeGuid)
                    {
                        item.Quantity += SoLuong.Value;
                    }
                }
            }
            Session[CartSession] = list;

            ViewBag.Error = Error;
            DsProduct(null, p);
            return View();
        }
        public ActionResult DsProduct(int? page, int? Status)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }

            int _Status = OrderStatus.ThemVaoGioHang.GetHashCode();
            if (Status.HasValue)
            {
                _Status = Status.Value;
            }

            var cart = Session[CartSession];

            var list = (new List<CartItem>());

            if (cart != null)
            {
                list = cart as List<CartItem>;
            }

            ViewBag.list = list;
            ViewBag.p = _Status;

            #region Dropdonwlist 
            landProvinceService _province = new landProvinceService();

            ViewBag.City = new SelectList(_province.DanhSachProvince(), "ProvinceId", "Name", null);

            ViewBag.Town = new SelectList(new List<landDistrict>(), "DistrictId", "Name", null);

            #endregion

            return PartialView("DsProduct", list);

        }
        #endregion

        #region AddItem
        public ActionResult AddItem(string ProductGuid, int Quantity, string SectionGuid, string SizeGuid)
        {
            var cart = Session[CartSession];

            shProductService _product = new shProductService();
            shProduct product = _product.FindByKey(ProductGuid);

            if (cart != null)
            {
                var list = (List<CartItem>)cart;

                if (list.Exists(x => (x.Product.ProductGuid == ProductGuid) && (x.SectionGuid == SectionGuid) && (x.SizeGuid == SizeGuid)))
                {
                    foreach (var item in list)
                    {
                        if ((item.Product.ProductGuid == ProductGuid) && (item.SectionGuid == SectionGuid) && (item.SizeGuid == SizeGuid))
                        {
                            item.Quantity += Quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = Quantity;
                    item.SectionGuid = SectionGuid;
                    item.SizeGuid = SizeGuid;
                    list.Add(item);
                }

                Session[CartSession] = list;
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = Quantity;
                item.SectionGuid = SectionGuid;
                item.SizeGuid = SizeGuid;
                var listCart = new List<CartItem>();
                listCart.Add(item);

                Session[CartSession] = listCart;
            }

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("trang-chu");
        }
        #endregion

        #region DeleteItem
        public ActionResult DeleteItem(string ProductGuid, string SectionGuid, string SizeGuid)
        {
            var cart = (List<CartItem>)Session[CartSession];

            CartItem delete = null;
            if (cart != null)
            {
                var list = (List<CartItem>)cart;

                foreach (var item in list)
                {
                    if (item.Product.ProductGuid == ProductGuid && item.SectionGuid == SectionGuid && item.SizeGuid == SizeGuid)
                    {
                        delete = item;
                    }
                }

                list.Remove(delete);

                Session[CartSession] = (List<CartItem>)list;
            }
            else
            {
                // giỏ hàng trống   
            }

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("trang-chu");
        }
        #endregion

        #region DeleteAll
        public ActionResult DeleteAll()
        {
            var cart = (List<CartItem>)Session[CartSession];

            Session[CartSession] = null;

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("trang-chu");
        }
        #endregion

        #region Order 
        public ActionResult Order(int? p, int? pay_type, string City, string Town, string order_name, string order_address, string order_phone, string order_email, string base_remark)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Get Infor login
                        string MemberGuid = string.Empty;
                        bool isCoTaiKhoan = false;

                        if (string.IsNullOrEmpty(User.Identity.GetMemberLogin().MemberGuid)
                          || string.IsNullOrWhiteSpace(User.Identity.GetMemberLogin().MemberGuid))
                        {
                            //return Redirect("/dang-nhap?urlPrefix=/gio-hang");
                            isCoTaiKhoan = false;
                        }
                        else
                        {
                            isCoTaiKhoan = true;
                            MemberGuid = User.Identity.GetMemberLogin().MemberGuid;
                        }

                        var cart = Session[CartSession];
                        if (cart == null)
                        {
                            TempData["Cart"] = "Giỏ hàng trống. Vui lòng thao tác lại.";
                            return Redirect("/gio-hang");
                        }

                        cart = cart as List<CartItem>;
                        shSizeService _size = new shSizeService();
                        decimal price = _size.ListProductPrice_Quantity(cart as List<CartItem>, 0, 0, 0);

                        decimal Feeship = Config.FeeShip;
                        if (price > Config.FeeTotal)
                            Feeship = 0;

                        int _Status = OrderStatus.DangXuLy.GetHashCode();

                        shOrderService _order = new shOrderService();
                        #endregion

                        #region Tìm nhân viên random được chọn xử lý đơn hàng
                        // ds nhân viên được phân xử lý đơn hàng 
                        qtUser UserId = new qtUser();
                        List<qtUser> dsUser_XuLyDonhang = CommonHelper.GetUserTheoMaCauHinhHeThong_GetTheoUser(Config.TAI_KHOAN_NHAN_DON_HANG, Units.ChiNhanhShowRoom.GetHashCode()).ToList();

                        int userid_dagiaoviec = 0;
                        IEnumerable<shOrder> dsOrder = _order.DanhSachOrder()
                                                        .Where(x => x.UserId != null && x.UserId != 0)
                                                        .OrderByDescending(x => x.OrderId);

                        foreach (var item in dsOrder)
                        {
                            if (item.UserId != null && item.UserId != 0)
                            {
                                userid_dagiaoviec = item.UserId.Value;
                                break;
                            }
                        }

                        for (int i = 0; i < dsUser_XuLyDonhang.Count(); i++)
                        {
                            qtUser user = dsUser_XuLyDonhang[i];

                            if (user.UserId == userid_dagiaoviec)
                            {
                                if (i == (dsUser_XuLyDonhang.Count() - 1))
                                {
                                    UserId = dsUser_XuLyDonhang[0];
                                }
                                else if (i < (dsUser_XuLyDonhang.Count() - 1))
                                {
                                    UserId = dsUser_XuLyDonhang[i + 1];
                                }
                            }
                        }
                        #endregion

                        #region shOrder 
                        shOrder order = _order.Insert_Update(
                                null,
                                null,
                                null,
                                 MemberGuid,
                                 order_name,
                                 order_email,
                                 order_phone,
                                 order_address,
                                 price,
                                 _Status,
                                 null,
                                 TypeHelper.ToInt32(Town),
                                 TypeHelper.ToInt32(City),
                                 Feeship,
                                 true,
                                 DateTime.Now,
                                 pay_type,
                                 DateTime.Now,
                                 UserId.UserId
                                 );
                        #endregion

                        #region shOrderHistory
                        shOrderHistoryService _orderHistory = new shOrderHistoryService();
                        shOrderHistory orderHistory = _orderHistory.Insert_Update(
                                null,
                                order.OrderGuid,
                                _Status,
                                User.Identity.GetMemberLogin().MemberGuid,
                                "Đặt hàng",
                                null,
                                true,
                                DateTime.Now);

                        #endregion

                        #region shOrderDetail
                        shOrderDetailService _orderDetail = new shOrderDetailService();
                        price = 0;
                        decimal total = 0;
                        foreach (var item in cart as List<CartItem>)
                        {
                            price = _size.ProductPrice(item.SizeGuid);
                            total = price * item.Quantity;

                            _orderDetail.Insert_Update(
                                null,
                                null,
                                null,
                                order.OrderGuid,
                                MemberGuid,
                                item.Product.ProductGuid,
                                item.Product.ProductName,
                                item.SectionGuid,
                                item.SizeGuid,
                                item.Quantity,
                                price,
                                total,
                                true,
                                DateTime.Now,
                                DateTime.Now
                                );
                        }
                        #endregion

                        #region SEND EMAIL, MESSAGE
                        // 1. Gửi Email tới Quản trị viên xác nhận đơn hàng

                        /// ds User cấu hình nhận Email trong hệ thống
                        IEnumerable<qtUser> dsUser = CommonHelper.GetUserTheoMaCauHinhHeThong_GetTheoUser(Config.TAI_KHOAN_EMAIL_THONG_BAO_TIEP_NHAN_DON_HANG, Units.ChiNhanhShowRoom.GetHashCode());
                        string noidungdonhang = EmailHelper.NoiDungDonHang(order, cart as List<CartItem>);
                        string noidungEmail = string.Empty;
                        foreach (var user in dsUser)
                        {
                            noidungEmail = string.Empty;
                            qtUnitService _unit = new qtUnitService();
                            noidungEmail = EmailHelper.NoiDungMailThongBaoQuanTri(_unit.UnitName(Units.ChiNhanhShowRoom.GetHashCode()), 1, noidungdonhang);
                            EmailHelper.ThongBaoEmailDonHangMoi(user.Email, noidungEmail);
                        }

                        if (UserId.UserId > 0)
                        {
                            noidungEmail = string.Empty;
                            qtUnitService _unit = new qtUnitService();
                            noidungEmail = EmailHelper.NoiDungMailThongBaoQuanTri(_unit.UnitName(Units.ChiNhanhShowRoom.GetHashCode()), 1, noidungdonhang);
                            EmailHelper.ThongBaoEmailDonHangMoi(UserId.Email, noidungEmail);
                        }

                        // 2. Gửi Email xác nhận đơn hàng tới khách hàng 
                        noidungEmail = "";
                        if (isCoTaiKhoan)
                        {
                            noidungEmail = EmailHelper.NoiDungMailThongBaoNguoiDatHang(noidungdonhang);
                            EmailHelper.ThongBaoEmailDonHangMoiToiNguoiDatHang(User.Identity.GetMemberLogin().Email, noidungEmail);
                        }
                        // 3. Gửi thông báo tới Quản trị viên nhận thông báo
                        ThongBaoService _thongbao = new ThongBaoService();
                        foreach (var user in dsUser)
                        {
                            int? Memberid = null;
                            if (isCoTaiKhoan)
                            {
                                Memberid = User.Identity.GetMemberLogin().MemberId;
                            }

                            _thongbao.InsertOrUpdate(
                                null,
                                "Thông báo đơn hàng mới. Người đặt hàng: " + order_name + ", giá trị: " + Format.FormatDecimalToString(price),
                                "Bạn có đơn hàng mới cần xử lý từ khách hàng: " + order_name +
                                " với đơn hàng tổng giá trị: " + Format.FormatDecimalToString(price) +
                                " lúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                                ". Vui lòng truy cập hệ thống để xử lý",
                                "/DanhMuc/Order/Index/s=" + order.OrderGuid,
                                Memberid,
                                user.UserId,
                                DateTime.Now,
                                false,
                                Config.THONG_BAO_DON_HANG_MOI,
                                null
                                );
                        }

                        if (UserId.UserId > 0)
                        {
                            int? Memberid = null;
                            if (isCoTaiKhoan)
                            {
                                Memberid = User.Identity.GetMemberLogin().MemberId;
                            }

                            _thongbao.InsertOrUpdate(
                               null,
                               "Thông báo đơn hàng mới. Người đặt hàng: " + order_name + ", giá trị: " + Format.FormatDecimalToString(price),
                               "Bạn có đơn hàng mới cần xử lý từ khách hàng: " + order_name +
                               " với đơn hàng tổng giá trị: " + Format.FormatDecimalToString(price) +
                               " lúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                               ". Vui lòng truy cập hệ thống để xử lý",
                               "/DanhMuc/Order/Index/s=" + order.OrderGuid,
                               Memberid,
                               UserId.UserId,
                               DateTime.Now,
                               false,
                               Config.THONG_BAO_DON_HANG_MOI,
                               null
                               );
                        }

                        #endregion

                        #region Clear data cookies after order 
                        Session[CartSession] = null;
                        #endregion

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }

            return Redirect("/thong-tin-ca-nhan");
        }

        #endregion
    }
}
