using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.QuanTri.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role

        public ActionResult Index(int? DepartmentId, int? PositionID, int? UnitId)
        {
            DropDownListView(DepartmentId, PositionID, UnitId);
            GetRole(DepartmentId, PositionID, UnitId);
            ViewBag.Unit = "";
            if (UnitId.HasValue)
                ViewBag.Unit = UnitId.Value;
            if (DepartmentId.HasValue)
                ViewBag.Dep = DepartmentId.Value;
            return View();
        }

        public void GetRole(int? depId, int? posId, int? unitId)
        {
            qtRoleService _service = new qtRoleService();
            IEnumerable<qtRole> list = _service.FindList().AsQueryable().Where(r => r.ParentId == 0 && r.Status == true && r.Application == "QTHT").OrderBy(m => m.SortOrder);
            string html = string.Empty;
            html += "<table class='grid' style='width: 100%;' id='table-role'>" +
            "<thead>" +
            "<tr>" +
            "<th><input type='checkbox' name='checkall' id='checkall'></th>" +
            "<th> Tên chức năng </th>" +
            "<th> Mã chức năng </th>" +
            "</thead>";
            foreach (qtRole role in list)
            {
                string check = checkRole(role.RoleId, depId, posId, unitId);
                html += "<tr>";
                if (role.Description != null && role.Description.ToString() != "")
                {
                    html += "<td style='text-align:center;'><input type='checkbox' value=" + role.RoleId + " name='cbx[]' class='cbx-parent' id='cbx__' " + check + " ></td><td colspan='2'><b>  " + role.RoleName + "</b></td>";
                }
                else
                {
                    html += "<td></td><td colspan='2'>" + role.Application + "-" + role.RoleName + "</td>";
                }
                html += "</tr>";
                html += RoleChild(role.RoleId, depId, posId, unitId);
            }
            html += "</table>";
            ViewBag.RoleList = html;
        }
        public string RoleChild(int roleId, int? depId, int? posId, int? unitId)
        {
            qtRoleService _service = new qtRoleService();
            IEnumerable<qtRole> list = _service.FindList().AsQueryable().Where(r => r.ParentId == roleId && r.Status == true).OrderBy(m => m.SortOrder);
            string html = string.Empty;
            foreach (qtRole item in list)
            {
                string check = checkRole(item.RoleId, depId, posId, unitId);
                html += "<tr>" +
                    "<td style='text-align: center;'><input type='checkbox' value=" + item.RoleId + " parentId='" + item.ParentId + "' name='cbx[]' id='cbx__' " + check + " ></td>" +
                    "<td>&nbsp;&nbsp;&nbsp; + " + item.RoleName + "</td>" +
                    "<td>" + item.Description + "</td>" +
                    "</tr>";

            }
            return html;
        }
        public string checkRole(int roleId, int? depId, int? posId, int? unitId)
        {
            qtUserRoleService _uservice = new qtUserRoleService();
            string check = string.Empty;
            if (!depId.HasValue || !posId.HasValue || !unitId.HasValue)
                return check;
            IEnumerable<qtUserRole> list = _uservice.FindList().AsQueryable().Where(u => u.DepartmentId == depId && u.PositionId == posId && u.UnitId == unitId);

            foreach (qtUserRole item in list)
            {
                if (item.RoleId == roleId)
                    return "checked='checked'";
            }
            return check;
        }

        private void DropDownListView(int? depId, int? posId, int? unitId)
        {

            string dep = "";
            string pos = "";
            string unit = "";
            if (depId.HasValue)
                dep = depId.ToString();
            if (posId.HasValue)
                pos = posId.ToString();
            if (unitId.HasValue)
                unit = unitId.ToString();


            qtPositionService _position = new qtPositionService();
            IEnumerable<qtPosition> listPos = _position.FindList().Where(m => m.Status == true);
            ViewBag.PositionID = new SelectList(listPos, "PositionID", "PositionName", pos);

            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> listUnit = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);
            ViewBag.UnitId = new SelectList(listUnit, "UnitId", "UnitName", unit);
        }
        public ActionResult Create(int? DepartmentId, int? PositionID, int? UnitId, string RoleList)
        {
            DropDownListView(DepartmentId, PositionID, UnitId);
            Save(DepartmentId, PositionID, UnitId, RoleList);

            GetRole(DepartmentId, PositionID, UnitId);

            return RedirectToAction("Index", "Role", new { DepartmentId = DepartmentId, PositionID = PositionID, UnitId = UnitId });
        }
        public void Save(int? depId, int? posId, int? unitId, string rolelist)
        {
            qtUserRoleService _uservice = new qtUserRoleService();
            IEnumerable<qtUserRole> listRole = _uservice.FindList().AsQueryable().Where(u => u.DepartmentId == depId && u.PositionId == posId && u.UnitId == unitId);
            if (listRole != null)
            {
                foreach (qtUserRole item in listRole)
                {
                    _uservice.Delete(item);
                }
            }

            foreach (string id in rolelist.Split(','))
            {
                qtUserRole userRole = new qtUserRole();
                userRole.PositionId = posId.Value;
                userRole.DepartmentId = depId.Value;
                userRole.UnitId = unitId.Value;
                userRole.RoleId = TypeHelper.ToInt32(id);
                _uservice.Insert(userRole);
            }
        }
        public PartialViewResult GetDepartment(string unitId, string depId)
        {
            string dep = "";
            if (depId != "" && depId != string.Empty)
            {
                dep = depId;
            }
            ViewBag.DepartmentId = "";
            qtDepartmentService _department = new qtDepartmentService();
            if (unitId != "" && unitId != string.Empty)
            {
                IEnumerable<qtDepartment> listDep = _department.FindList().Where(m => m.Status == true && m.UnitId.Value == TypeHelper.ToInt32(unitId)).OrderBy(m => m.SortOrder);
                ViewBag.DepartmentId = new SelectList(listDep, "DepartmentId", "DepartmentName", dep);
            }
            else
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                ViewBag.DepartmentId = new SelectList(listItems, "Value", "Text", "");
            }
            return PartialView();
        }
        public PartialViewResult GetDepartmentByUnitId(string unitId)
        {

            qtDepartmentService _department = new qtDepartmentService();
            if (unitId != "" && unitId != string.Empty)
            {
                IEnumerable<qtDepartment> listDep = _department.FindList().Where(m => m.Status == true && m.UnitId.Value == TypeHelper.ToInt32(unitId)).OrderBy(m => m.SortOrder);
                ViewBag.DepartmentId = new SelectList(listDep, "DepartmentId", "DepartmentName");
            }
            else
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                ViewBag.DepartmentId = new SelectList(listItems, "Value", "Text", "");
            }
            return PartialView("GetDepartment");
        }
        [HttpPost]
        public JsonResult GetRoleParentId(int RoleId)
        {
            int? data;
            qtRoleService _role = new qtRoleService();
            qtRole role = _role.FindByKey(RoleId);
            if (role != null && role.ParentId != null) data = role.ParentId;
            else data = null;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
