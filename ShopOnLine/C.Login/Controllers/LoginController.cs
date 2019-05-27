using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.Membership.Helper;
using C.Core.Common;
using C.UI.Tool;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using C.DanhMuc.Helper;
using C.UI.PagedList;

namespace C.Login.Controllers
{
    public class LoginController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(string urlPrefix)
        {
            ViewBag.urlPrefix = urlPrefix;
            shMember memberCookie = CheckCookie();
            if (memberCookie != null)
            {
                shMemberService _member = new shMemberService();
                if (_member.CheckMemberLogin(memberCookie.MemberName, memberCookie.Password))
                {
                    //qtUser user = _user.GetUserLogin(userCookie.UserLogin, userCookie.Password);
                    shMember member = _member.GetMember(memberCookie.MemberLogin, memberCookie.Password);
                    if (member != null)
                    {
                        HttpContext.User.Identity.SetMemberLogin(
                            new Member(
                                member.MemberGuiId,
                                member.MemberId,
                                member.MemberName,
                                member.MemberLogin,
                                member.Password,
                                member.ImageFile,
                                member.Address,
                                member.Sex.GetValueOrDefault(0),
                                member.Email,
                                member.Tel,
                                member.BirthDay.GetValueOrDefault(DateTime.Now),
                                member.Phone,
                                member.Notes
                                )
                            );

                        string url = String.Empty;
                        if (String.IsNullOrEmpty(urlPrefix))
                        {
                            url = "/";
                        }
                        else
                        {
                            url = urlPrefix;
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            return Redirect(url);
                        }

                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string urlPrefix, string MemberName, string Password, bool? Remember)
        {
            shMemberService _member = new shMemberService();

            if (_member.CheckMemberLogin(MemberName, EncryptUtil.EncryptMD5(Password)))
            {
                shMember member = _member.GetMember(MemberName, EncryptUtil.EncryptMD5(Password));
                if (member != null)
                {
                    if (Remember != null && Remember == true)
                    {
                        HttpCookie ckUsername = new HttpCookie("CMemberName");
                        ckUsername.Expires = DateTime.Now.AddSeconds(3600);
                        ckUsername.Value = MemberName;
                        Response.Cookies.Add(ckUsername);
                        HttpCookie ckPassword = new HttpCookie("CPassword");
                        ckPassword.Expires = DateTime.Now.AddSeconds(3600);
                        ckPassword.Value = EncryptUtil.EncryptMD5(Password);
                        Response.Cookies.Add(ckPassword);
                    }

                    HttpContext.User.Identity.SetMemberLogin(
                             new Member(
                                member.MemberGuiId,
                                member.MemberId,
                                member.MemberName,
                                member.MemberLogin,
                                member.Password,
                                member.ImageFile,
                                member.Address,
                                member.Sex.GetValueOrDefault(0),
                                member.Email,
                                member.Tel,
                                member.BirthDay.GetValueOrDefault(DateTime.Now),
                                member.Phone,
                                member.Notes
                                )
                             );

                    string url = string.Empty;
                    if (string.IsNullOrEmpty(urlPrefix))
                    {
                        url = "/";
                    }
                    else
                    {
                        url = urlPrefix;
                    }
                    return Redirect(url);
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại trong hệ thống");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View();
        }
        #endregion

        #region Register
        [HttpGet]
        public ActionResult Register()
        {

            shMemberService _member = new shMemberService();
            shMember member = new shMember();

            if (User.Identity.GetMemberLogin().MemberGuid == null)
            {
                member = _member.FindByKey(User.Identity.GetMemberLogin().MemberGuid);
            }

            if (member == null)
            {
                member = new shMember();
            }

            DropDownListMenu();

            return View(member);
        }

        public void DropDownListMenu()
        {
            shSexService _sex = new shSexService();
            IEnumerable<shSex> dsSex = _sex.FindList();

            ViewBag.Sex = new SelectList(dsSex, "SexId", "SexName", null);

        }

        [HttpPost]
        public ActionResult Register(
            string MemberGuid, int? MemberId, string MemberName, string MemberLogin, string Password,
            string Address, int? Sex, string Tel,
            string Email, string Phone, string Notes, bool? Status, string ImageFile)
        {

            shMemberService _member = new shMemberService();
            shMember member = _member.ThemMoi_HieuChinhMember(
                        MemberGuid,
                        0,
                        MemberName,
                        MemberLogin,
                        Password,
                        ImageFile,
                        Address,
                        Sex,
                        Email,
                        Tel,
                        Phone,
                        Notes,
                        Status,
                        DateTime.Now,
                        null,
                        null
                        );

            return Redirect("/");
        }
        #endregion

        #region Info
        [HttpGet]
        public ActionResult Info()
        {
            if (string.IsNullOrWhiteSpace(User.Identity.GetMemberLogin().MemberGuid) || string.IsNullOrEmpty(User.Identity.GetMemberLogin().MemberGuid))
            {
                return Redirect("/");
            }
            shMemberService _member = new shMemberService();
            shMember member = _member.FindByKey(User.Identity.GetMemberLogin().MemberGuid);
            if (member == null)
                member = new shMember();

            return View(member);
        }

        [HttpPost]
        public ActionResult Info(string MemberGuid, int? MemberId, string MemberName, string MemberLogin, string Password,
            string Address, int? Sex, string Tel,
            string Email, string Phone, string Notes, bool? Status, string ImageFile)
        {
            shMemberService _member = new shMemberService();

            shMember member = _member.ThemMoi_HieuChinhMember(
                        MemberGuid,
                        0,
                        MemberName,
                        MemberLogin,
                        Password,
                        ImageFile,
                        Address,
                        Sex,
                        Email,
                        Tel,
                        Phone,
                        Notes,
                        true,
                        DateTime.Now,
                        null,
                        null
                        );

            return View(member);
        }

        public ActionResult Edit()
        {
            shMemberService _member = new shMemberService();
            shMember member = _member.FindByKey(User.Identity.GetMemberLogin().MemberGuid);
            if (member == null)
                member = new shMember();

            return PartialView("Edit", member);
        }

        public ActionResult Order(int? Status)
        {
            if (!Status.HasValue)
                Status = C.Core.Common.OrderStatus.DangXuLy.GetHashCode();

            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> dsOrder = _order.DanhSachOrder_ByStatus(User.Identity.GetMemberLogin().MemberGuid, Status);

            if (dsOrder == null)
                dsOrder = new List<shOrder>();

            return PartialView("Order", dsOrder);
        }

        public ActionResult ListOrders(int? Status)
        {
            if (!Status.HasValue)
                Status = C.Core.Common.OrderStatus.DangXuLy.GetHashCode();

            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> dsOrder = _order.DanhSachOrder_ByStatus(User.Identity.GetMemberLogin().MemberGuid, Status);

            if (dsOrder == null)
                dsOrder = new List<shOrder>();

            return PartialView("ListOrders", dsOrder);
        }

        public ActionResult DsChildOrder(string OrderGuid)
        {
            shOrderDetailService _orderDetail = new shOrderDetailService();

            IEnumerable<shOrderDetail> ds = _orderDetail.DanhSachOrderDetailBy(OrderGuid, User.Identity.GetMemberLogin().MemberGuid, null);

            if (ds == null)
            {
                ds = new List<shOrderDetail>();
            }

            return PartialView("DsChildOrder", ds);
        }

        public ActionResult Details(string OrderGuid)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            if (order == null)
                order = new shOrder();

            return PartialView("Details", order);
        }

        [HttpGet]
        public ActionResult RatingOrder(string OrderGuid)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            List<DropDownList> ds = OrderHelper.DanhSachTrangThaiGioHang(null);
            ViewBag.OrderStatus = new SelectList(ds, "Value", "Text", C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode());


            return PartialView("RatingOrder", order);
        }

        [HttpPost]
        public ActionResult RatingOrder(string OrderGuid, int? OrderStatus, string Description)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            OrderStatus = C.Core.Common.OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode();

            string MemberGuid = null;
            if (User.Identity.GetMemberLogin() != null)
            {
                MemberGuid = User.Identity.GetMemberLogin().MemberGuid;
            }
            shOrderHistoryService _orderHistory = new shOrderHistoryService();
            shOrderHistory orderHistory = _orderHistory.Insert_Update(
                        null,
                        order.OrderGuid,
                        OrderStatus,
                        MemberGuid,
                        Description,
                        null,
                        true,
                        DateTime.Now);

            order.OrderStatus = OrderStatus;
            _order.Update(order);

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Update OrderStatus 
        public ActionResult OrderStatus(string OrderGuid, int? Status, string Description)
        {
            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            if (order != null)
            {
                shOrderHistoryService _orderHistory = new shOrderHistoryService();

                string MemberGuid = null;
                if (User.Identity.GetMemberLogin() != null)
                    MemberGuid = User.Identity.GetMemberLogin().MemberGuid;

                shOrderHistory orderHistory = _orderHistory.Insert_Update(
                            null,
                            order.OrderGuid,
                            C.Core.Common.OrderStatus.HuyDonHang.GetHashCode(),
                            MemberGuid,
                            Description,
                            null,
                            true,
                            DateTime.Now);

                order.OrderStatus = C.Core.Common.OrderStatus.HuyDonHang.GetHashCode();
                _order.Update(order);
            }

            if (Request.IsAjaxRequest())
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region ThongBao
        public ActionResult ThongBao(int? page)
        {
            ThongBaoService _thongbao = new ThongBaoService();

            IPagedList<ThongBao> ds = _thongbao.DanhSach_PhanTrang(User.Identity.GetMemberLogin().MemberId,
                                        page.GetValueOrDefault(1),
                                        Config.PAGE_SIZE_10);


            return PartialView("ThongBao", ds);
        }
        #endregion

        #region CheckCookie
        public shMember CheckCookie()
        {
            shMember member = null;

            string membername = string.Empty,
                   password = string.Empty;

            if (Request.Cookies["CMemberName"] != null)
            {
                membername = Request.Cookies["CMemberName"].Value;
            }

            if (Request.Cookies["CPassword"] != null)
            {
                password = Request.Cookies["CPassword"].Value;
            }

            if (!string.IsNullOrEmpty(membername) && !string.IsNullOrEmpty(password))
            {
                member = new shMember
                {
                    MemberName = membername,
                    Password = password
                };
            }

            return member;

        }

        #endregion

        #region Logout
        public ActionResult Logout()
        {
            if (Request.Cookies["CMemberName"] != null)
            {
                HttpCookie ckUsername = new HttpCookie("CMemberName");
                ckUsername.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckUsername);
            }

            if (Request.Cookies["CPassword"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("CPassword");
                ckPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPassword);
            }

            Session["MemberId"] = null;
            Session["MemberGuid"] = null;
            Session["MemberName"] = null;

            return Redirect("/");
        }
        #endregion
    }
}
