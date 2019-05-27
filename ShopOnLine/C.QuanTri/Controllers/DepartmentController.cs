using C.Core.BaseController;
using C.Core.Common;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using System;
using System.Web.Mvc;

namespace C.QuanTri.Controllers
{
    public class DepartmentController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page ,int? UnitId)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListDepartments(pageCurrent, UnitId);

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, int? UnitId, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;
            ViewBag.UnitId = UnitId;
            ListDepartments(pageCurrent, UnitId);

            return View();
        }
        public PartialViewResult ListDepartments(int? page, int? UnitId)
        {

            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            qtDepartmentService _department = new qtDepartmentService();

            IPagedList<qtDepartment> dsDepartment = _department.DanhSachDepartment_PhanTrang(UnitId, pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListDepartments = dsDepartment;

            return PartialView("ListDepartments", dsDepartment);
        }
        #endregion

        #region Create - Update
        [HttpGet]
        public ActionResult Create(int? id, int? UnitId)
        {
            qtDepartmentService _department = new qtDepartmentService();
            qtDepartment department = new qtDepartment();

            if (id.HasValue)
            {
                department = _department.FindByKey(id);
                if (department != null)
                {
                    ViewBag.UnitId = department.UnitId;
                    return View(department);
                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống. Vui lòng kiểm tra lại");
                    return View(new qtDepartment());
                }
            }

            ViewBag.UnitId = UnitId;

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? DepartmentId, string DepartmentName, string Address, string Phone,
            string Fax, string Email, int? UnitId, int? SortOrder, bool? Status)
        {

            if (!DepartmentId.HasValue)
                DepartmentId = 0;

            qtDepartmentService _department = new qtDepartmentService();
            qtDepartment department = _department.ThemMoi_HieuChinhDepartment(
                DepartmentId.Value,
                DepartmentName,
                Address,
                Phone,
                Fax,
                Email,
                UnitId,
                SortOrder,
                Status,
                DateTime.Now);

            return RedirectToAction("Index");
        }

        #endregion


    }
}
