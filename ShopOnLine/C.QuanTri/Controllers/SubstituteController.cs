using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using C.UI.Helpers;
using C.UI.Tool;
using C.QuanTri.Helper;
using C.Core.BaseController;

namespace C.QuanTri.Controllers
{
    public class SubstituteController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int? DepartmentId, int? page)
        {
            qtSubstitutesService _Substitute = new qtSubstitutesService();

            IPagedList<qtSubstitute> Substitute = _Substitute.FindListByPage(DepartmentId, page);

            ViewBag.DonVi = TreeUnit(DepartmentId);

            return View(Substitute);
        }
        private string TreeUnit(int? DepId)
        {
            string html = string.Empty;

            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> list = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);
            if (list != null && list.Count() > 0)
            {
                html += "<div id='tree'>";
                foreach (qtUnit unit in list)
                {
                    qtDepartmentService _dep = new qtDepartmentService();
                    IEnumerable<qtDepartment> dep = _dep.FindList()
                        .Where(m => m.UnitId == unit.UnitId && m.Status == true).OrderBy(m => m.SortOrder);
                    html += "<ul><li>" + unit.UnitName + "</a>";

                    if (dep != null && dep.Count() > 0)
                    {
                        html += "<ul>";
                        foreach (qtDepartment item in dep)
                        {
                            html += "<li><a  href='/QuanTri/Substitute/Index?DepartmentId=" + item.DepartmentId + "'>" + item.DepartmentName +
                                //"(" + item.Substitutes.Count() + ")" +
                                "</a></li>";
                        }
                        html += "</ul>";
                    }
                    html += "</li></ul>";
                }
                html += "</div>";
            }
            return html;

        }
        [HttpGet]
        public ActionResult Create(int? id)
        {
            qtSubstitutesService _Substitutes = new qtSubstitutesService();
            qtSubstitute substitute = new qtSubstitute();
            if (id.HasValue)
            {

                substitute = _Substitutes.FindByKey(id);
                DanhSachNguoiDung(substitute.DepartmentId, substitute.SubstituteId);
                //DropDownListView(substitute.qtDepartment.UnitId, substitute.DepartmentId);
            }
            else
            {
                DanhSachNguoiDung(null, null);
                DropDownListView(null, null);
            }
            return View(substitute);
        }
        [HttpPost]
        public ActionResult Create(int? id, int? UnitId, int? DepartmentId, string AppName, string AppCode, bool IsActive)
        {
            qtSubstitutesService _Substitutes = new qtSubstitutesService();
            qtSubstitute substitute = new qtSubstitute();
            if (id.HasValue)
            {

                substitute = _Substitutes.FindByKey(id);
                DanhSachNguoiDung(substitute.DepartmentId, substitute.SubstituteId);

            }
            else
            {
                DanhSachNguoiDung(DepartmentId, null);
            }

            ViewBag.AppName = AppName;
            ViewBag.AppCode = AppCode;
            ViewBag.IsActive = IsActive;
            DropDownListView(UnitId, substitute.DepartmentId);
            return View(substitute);
        }
        private void DropDownListView(int? unitId, int? departmentId)
        {

            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> listUnit = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);
            ViewBag.UnitId = new SelectList(listUnit, "UnitId", "UnitName", unitId);

            qtDepartmentService _dep = new qtDepartmentService();
            IEnumerable<qtDepartment> listDep = _dep.FindList().Where(m => m.Status == true && m.UnitId == unitId)
                                                    .OrderBy(m => m.SortOrder);
            ViewBag.DepartmentId = new SelectList(listDep, "DepartmentId", "DepartmentName", departmentId);

            //List<Unit> list = _unit.FindList().Where(m => m.IsActive == true).OrderBy(m => m.SortOrder).ToList();
            //AutoComplete program = new AutoComplete();
            //SelectList select = new SelectList(list, "UnitId", "UnitName", unitId);
            //program.languanges = select;
            //ViewBag.UnitId = program.languanges;

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
                IEnumerable<qtDepartment> listDep = _department.FindList()
                                .Where(m => m.Status == true && m.UnitId.Value == TypeHelper.ToInt32(unitId))
                                .OrderBy(m => m.SortOrder);
                ViewBag.DepartmentId = new SelectList(listDep, "DepartmentId", "DepartmentName", dep);
            }
            else
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                ViewBag.DepartmentId = new SelectList(listItems, "Value", "Text", "");
            }
            return PartialView();
        }
        public JsonResult GetDepartmentByUnitId(string UnitId)
        {
            if (UnitId != null && UnitId != "")
            {
                qtDepartmentService _department = new qtDepartmentService();
                List<qtDepartment> listDep = _department.FindList()
                    .Where(m => m.Status == true && m.UnitId.Value == TypeHelper.ToInt32(UnitId))
                    .OrderBy(m => m.SortOrder).ToList();
                return Json(listDep.Select(m => new {
                    DepartmentId = m.DepartmentId,
                    DepartmentName = m.DepartmentName }), 
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        private string DanhSachNguoiDung(int? phongid, int? SubstituteId)
        {
            string html = "";
            if (phongid.HasValue)
            {
                qtUserService _user = new qtUserService();
                IEnumerable<qtUser> list = _user.FindList().Where(m => m.DepartmentId == phongid && m.Status == true);
                if (list.Count() > 0)
                {
                    html += "<table class='grid' style='width: 100%;'>" +
                            "<thead>" +
                            "<tr>" +
                            "<th>#</th>" +
                            "<th> Họ và tên </th>" +
                            "<th> Chức vụ </th>" +
                            "</thead>";
                    foreach (C.Core.Model.qtUser user in list)
                    {
                        string check = checkUser(SubstituteId, user.UserId);
                        html += "<tr>" +
                            "<td><input type='checkbox' value=" + user.UserId + " name='cbxItem' id='cbxItem' " + check + " ></td>" +
                            "<td>" + user.UserName + "</td>" +
                                     "<td>" + user.qtPosition.PositionName + "</td>" +
                            "</tr>";
                    }


                    html += "</table>";
                }
            }
            ViewBag.dsNguoiDung = html;
            return html;
        }

        public JsonResult LoadDanhSachNguoiDung(int? phongid, int? SubstituteId)
        {
            return Json(DanhSachNguoiDung(phongid, SubstituteId), JsonRequestBehavior.AllowGet);
        }

        private string checkUser(int? SubstituteId, int UserId)
        {
            qtSubstituteDetailService _user = new qtSubstituteDetailService();
            var item = _user.FindList().Where(m => m.UserId == UserId && m.SubstituteId == SubstituteId).Count();
            if (item > 0)
                return "checked='checked'";
            return "";
        }

        [HttpPost]
        public ActionResult Save(int? id, int? SubstituteId, string AppName, string AppCode, bool IsActive, int DepartmentId, string[] cbxItem)
        {
            qtSubstitutesService _Substitutes = new qtSubstitutesService();
            qtSubstitute substitute = new qtSubstitute();
            if (SubstituteId.HasValue && SubstituteId.Value > 0)
            {
                substitute = _Substitutes.FindByKey(SubstituteId);
            }
            substitute.AppName = AppName;
            substitute.AppCode = AppCode;
            substitute.IsActive = IsActive;
            substitute.DepartmentId = DepartmentId;
            if (SubstituteId.HasValue && SubstituteId.Value > 0)
                _Substitutes.Update(substitute);
            else
                _Substitutes.Insert(substitute);
            if (cbxItem.Count() > 0)
            {
                qtSubstituteDetailService _detail = new qtSubstituteDetailService();
                _detail.RunSql("Delete SubstituteDetail where SubstituteId=" + substitute.SubstituteId);
                foreach (string item in cbxItem)
                {

                    qtSubstituteDetail detail = new qtSubstituteDetail();
                    detail.SubstituteId = substitute.SubstituteId;
                    detail.UserId = TypeHelper.ToInt32(item);
                    _detail.Insert(detail);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] cbxItem)
        {
            qtSubstitutesService _substitute = new qtSubstitutesService();
            qtSubstitute substitute = new qtSubstitute();
            if (cbxItem != null && cbxItem.Count() > 0)
            {
                qtSubstituteDetailService _substitutedetail = new qtSubstituteDetailService();
                foreach (int item in cbxItem)
                {
                    try
                    {
                        substitute = _substitute.FindByKey(item);
                        if (substitute != null)
                        {
                            _substitutedetail.DeleteObject(_substitutedetail.FindList().Where(m => m.SubstituteId == item).ToList());
                            _substitute.Delete(substitute);
                        }
                    }
                    catch (Exception) { }
                }
            }

            return RedirectToAction("Index");
        }
    }
}