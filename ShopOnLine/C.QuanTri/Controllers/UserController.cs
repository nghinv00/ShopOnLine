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

namespace C.QuanTri.Controllers
{
    public class UserController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page, int? UnitId, int? DepartmentId, string TenNguoiDung)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListUsers(pageCurrent, UnitId, DepartmentId, TenNguoiDung);

            Departments(UnitId, DepartmentId);

            ViewBag.UnitId = UnitId;
            ViewBag.TenNguoiDung = TenNguoiDung;

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, int? UnitId, int? DepartmentId, string TenNguoiDung, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListUsers(pageCurrent, UnitId, DepartmentId, TenNguoiDung);

            Departments(UnitId, DepartmentId);

            ViewBag.UnitId = UnitId;
            ViewBag.TenNguoiDung = TenNguoiDung;

            return View();
        }

        public PartialViewResult ListUsers(int? page, int? UnitId, int? DepartmentId, string TenNguoiDung)
        {
            int pageCurrent = 1;
            if (page.HasValue)
            {
                pageCurrent = page.Value;
            }

            qtUserService _user = new qtUserService();

            IPagedList pageList_dsUser = _user.DanhSachUser_PhanTrang(TenNguoiDung, UnitId, DepartmentId, pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListUsers = pageList_dsUser;

            return PartialView("ListUsers", pageList_dsUser);
        }

        public PartialViewResult Departments(int? UnitId, int? DepartmentId)
        {
            qtDepartmentService _department = new qtDepartmentService();

            IEnumerable<qtDepartment> danhSachPhongBan = _department.FindList();

            if (UnitId.HasValue)
                danhSachPhongBan = danhSachPhongBan.Where(x => x.UnitId == UnitId);

            ViewBag.DepartmentId = new SelectList(danhSachPhongBan, "DepartmentId", "DepartmentName", DepartmentId);

            return PartialView("Departments", ViewBag.DepartmentId);
        }
        
        #endregion

        #region Create - Update
        [HttpGet]
        public ActionResult Create(int? id, int? UnitId)
        {
            qtUserService _user = new qtUserService();

            qtUser user = new qtUser();

            if (id.HasValue)
            {
                ViewBag.UserId = id;
                user = _user.FindByKey(id);
                if (user == null)
                {
                    ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống. Vui lòng kiểm tra lại");
                    Departments(null, null);
                    return View(new qtUser());
                }
            }

            Departments(user.UnitId, user.DepartmentId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(int? UserId, string UserName, string UserLogin, string Password, string Email, string Tel, int? SortOrder, int? UnitId, int? DepartmentId, int? PositionId, bool? IsAdmin, bool? Status)
        {
            qtUserService _user = new qtUserService();
            qtUser user = new qtUser();

            if (!UserId.HasValue)
            {
                UserId = 0;
            }

            user = _user.ThemMoi_HieuChinhThongTinUser(
                                            UserId.Value,
                                            UserName,
                                            UserLogin,
                                            Password,
                                            SortOrder,
                                            null,
                                            null,
                                            null,
                                            Email,
                                            Tel,
                                            null,
                                            IsAdmin,
                                            null,
                                            UnitId,
                                            DepartmentId,
                                            PositionId,
                                            Status,
                                            DateTime.Now);
            
            return RedirectToAction("Index");

        }
        #endregion

    }
}
