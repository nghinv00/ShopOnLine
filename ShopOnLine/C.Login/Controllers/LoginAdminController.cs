using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.Membership.Helper;
using C.Core.Common;
using C.QuanTri.Helper;
using C.UI.Tool;
using System;
using System.Web;
using System.Web.Mvc;

namespace C.Login.Controllers
{
    public class LoginAdminController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(string urlPrefix)
        {
            ViewBag.urlPrefix = urlPrefix;
            qtUser userCookie = CheckCookie();
            if (userCookie != null)
            {
                qtUserService _user = new qtUserService();
                if (_user.CheckUserLogin(userCookie.UserLogin, userCookie.Password))
                {
                    qtUser user = _user.GetUserLogin(userCookie.UserLogin, userCookie.Password);
                    if (user != null)
                    {
                        HttpContext.User.Identity.SetUserLogin(
                            new Account(user.UserId, user.UserName, user.UserLogin, user.Password,
                                        user.Address, user.Sex.ToString(), user.Email, user.Tel, user.Phone,
                                        user.Notes, user.UnitId.ToString(), string.Empty, user.DepartmentId.ToString(), string.Empty,
                                        user.PositionId.ToString(), string.Empty)
                            );
                        string url = String.Empty;
                        if (String.IsNullOrEmpty(urlPrefix))
                            url = QuanTriHelper.GetFirstPermissionUrlByUser(user, Config.Application);
                        else
                            url = urlPrefix;
                        // fix 
                        url = "/DanhMuc/BaoCao/Index";

                        if (!string.IsNullOrEmpty(url))
                        {
                            return Redirect(url);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản chưa được phân quyền");
                        }
                        return View();
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
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string urlPrefix, string Username, string Password, bool? Remember)
        {
            qtUserService _user = new qtUserService();
            if (_user.CheckUserLogin(Username, EncryptUtil.EncryptMD5(Password)))
            {
                qtUser user = _user.GetUserLogin(Username, EncryptUtil.EncryptMD5(Password));
                if (user != null)
                {
                    if (Remember != null && Remember == true)
                    {
                        HttpCookie ckUsername = new HttpCookie("username");
                        ckUsername.Expires = DateTime.Now.AddSeconds(3600);
                        ckUsername.Value = Username;
                        Response.Cookies.Add(ckUsername);
                        HttpCookie ckPassword = new HttpCookie("password");
                        ckPassword.Expires = DateTime.Now.AddSeconds(3600);
                        ckPassword.Value = EncryptUtil.EncryptMD5(Password);
                        Response.Cookies.Add(ckPassword);
                    }
                    
                    HttpContext.User.Identity.SetUserLogin(
                             new Account(user.UserId, user.UserName, user.UserLogin, user.Password,
                                         user.Address, user.Sex.ToString(), user.Email, user.Tel, user.Phone,
                                         user.Notes, user.UnitId.ToString(), string.Empty, user.DepartmentId.ToString(), string.Empty,
                                         user.PositionId.ToString(), string.Empty)
                             );

                    string url = String.Empty;
                    if (String.IsNullOrEmpty(urlPrefix))
                        url = QuanTriHelper.GetFirstPermissionUrlByUser(user, Config.Application);
                    else
                        url = urlPrefix;
                    // fix 
                    url = "/DanhMuc/BaoCao/Index";
                    if (!string.IsNullOrEmpty(url))
                    {
                        return Redirect(url);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản chưa được phân quyền");
                    }
                    return View();
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

        #region CheckCookie
        public qtUser CheckCookie()
        {
            qtUser user = null;
            string username = string.Empty,
                   password = string.Empty;
            if (Request.Cookies["Cusername"] != null)
                username = Request.Cookies["Cusername"].Value;
            if (Request.Cookies["Cpassword"] != null)
                password = Request.Cookies["Cpassword"].Value;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                user = new qtUser { UserLogin = username, Password = password };
            return user;
        }

        #endregion

        #region Logout
        public ActionResult Logout()
        {
            if (Request.Cookies["_Cusername"] != null)
            {
                HttpCookie ckUsername = new HttpCookie("_Cusername");
                ckUsername.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckUsername);
            }
            if (Request.Cookies["_Cpassword"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("_Cpassword");
                ckPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPassword);
            }

            Session["_userid"] = null;
            Session["_username"] = null;
            return RedirectToAction("Index", "LoginAdmin");
        }
        #endregion
    }
}
