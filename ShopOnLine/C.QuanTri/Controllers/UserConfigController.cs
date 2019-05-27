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
using System.Collections.Specialized;

namespace C.QuanTri.Controllers
{
    public class UserConfigController : BaseController
    {
        // GET: UserConfig
        [HttpGet]
        public ActionResult Index(int? UnitId, int? page)
        {
            qtUserConfigService _userconfig = new qtUserConfigService();
            IPagedList<qtUserConfig> userconfig = _userconfig.FindListByPage(UnitId, page);
            ViewBag.DonVi = TreeUnit(UnitId);
            return View(userconfig);
        }
        private string TreeUnit(int? unitId)
        {
            string html = string.Empty;

            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> list = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);
            if (list != null && list.Count() > 0)
            {
                html += "<div id='tree' class=''>";
                foreach (qtUnit unit in list)
                {
                    qtUserConfigService _userconfig = new qtUserConfigService();
                    IEnumerable<qtUserConfig> userconfig = _userconfig.FindList().Where(m => m.UnitId == unit.UnitId && m.IsActive == true);
                    html += "<ul><li><a  href='/QuanTri/UserConfig/Index?UnitId=" + unit.UnitId + "'>" + unit.UnitName + "(" + userconfig.Count() + ")</a>";
                    if (userconfig != null && userconfig.Count() > 0)
                    {
                        html += "<ul>";
                        foreach (qtUserConfig config in userconfig)
                        {
                            html += "<li>" + config.AppName + "</li>";
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
            qtUserConfigService _userconfig = new qtUserConfigService();
            qtUserConfig userconfig = new qtUserConfig();
            if (id.HasValue)
            {

                userconfig = _userconfig.FindByKey(id);
                DanhSachNguoiDung(userconfig.UnitId, userconfig.UserConfigId);
                DropDownList(userconfig.UnitId);
            }
            else
            {
                DanhSachNguoiDung(null, null);
                DropDownList(null);
            }
            return View(userconfig);
        }
        [HttpPost]
        public ActionResult Create(int? id, int? UnitId, string AppName, string AppCode, bool IsActive)
        {
            qtUserConfigService _userconfig = new qtUserConfigService();
            qtUserConfig userconfig = new qtUserConfig();
            if (id.HasValue)
            {

                userconfig = _userconfig.FindByKey(id);
                DanhSachNguoiDung(userconfig.UnitId, userconfig.UserConfigId);

            }
            else
                DanhSachNguoiDung(UnitId, null);
            ViewBag.AppName = AppName;
            ViewBag.AppCode = AppCode;
            ViewBag.IsActive = IsActive;
            DropDownList(UnitId);
            return View(userconfig);
        }
        private void DropDownList(int? unitId)
        {


            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> listUnit = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);
            ViewBag.UnitId = new SelectList(listUnit, "UnitId", "UnitName", unitId);

        }

        private void DanhSachNguoiDung(int? donviId, int? UserConfigId)
        {
            string html = "";
            if (donviId.HasValue)
            {
                qtUserService _user = new qtUserService();
                qtDepartmentService _dep = new qtDepartmentService();
                IEnumerable<qtDepartment> listdep = _dep.FindList().Where(m => m.UnitId == donviId && m.Status == true);
                if (listdep.Count() > 0)
                {
                    html += "<table class='grid' style='width: 100%;'>" +
                            "<thead>" +
                            "<tr>" +
                            "<th width='15%' >#</th>" +
                            "<th width='35%'> Họ và tên </th>" +
                            "<th width='15%'> Thứ tự </th>" +
                            "<th width='40%'> Chức vụ </th>" +
                            "</thead>";
                    foreach (qtDepartment item in listdep)
                    {
                        html += "<tr>" +
                            "<td colspan='4'>" + item.DepartmentName + "</td>" +
                            "</tr>";
                        IEnumerable<qtUser> list = _user.FindList().Where(m => m.UnitId == donviId && m.DepartmentId == item.DepartmentId && m.Status == true);
                        if (list.Count() > 0)
                        {
                            foreach (C.Core.Model.qtUser user in list)
                            {
                                qtUserConfigDetail check = checkUser(UserConfigId, user.UserId, null);

                                string _checked = check != null ? "checked='checked'" : "";
                                string value = check != null ? check.OrderBy.ToString() : "";
                                html += "<tr>" +
                                    "<td><input type='checkbox' value=" + user.UserId + " name='cbxItem' id='cbxItem' " + _checked + " ></td>" +
                                    "<td>" + user.UserName + "</td>" +
                                     //"<td> </td>"+
                                     "<td>" + " <input type='text' class='width-10' id='OrderBy" + user.UserId + "' name='OrderBy" + user.UserId + "'  value='" + value + "' />" + "</td>" +
                                    "<td>" + user.qtPosition.PositionName + "</td>" +
                                    "</tr>";
                            }
                        }
                    }
                    html += "</table>";
                }
            }
            ViewBag.dsNguoiDung = html;
        }
        
        private string checkUser(int? UserConfigId, int UserId)
        {
            qtUserConfigDetailService _user = new qtUserConfigDetailService();
            var item = _user.FindList().Where(m => m.UserId == UserId && m.UserConfigId == UserConfigId).Count();
            if (item > 0)
                return "checked='checked'";
            return "";
        }

        private qtUserConfigDetail checkUser(int? UserConfigId, int UserId, int? id)
        {
            qtUserConfigDetailService _user = new qtUserConfigDetailService();
            var item = _user.FindList().Where(m => m.UserId == UserId && m.UserConfigId == UserConfigId).FirstOrDefault();
            if (item != null && item.UserConfigDetailId > 0)
                return item;
            return null;
        }

        [HttpPost]
        public ActionResult Save(
            int? id,
            int? UserConfigId,
            string AppName,
            string AppCode,
            bool IsActive,
            int UnitId,
            string[] cbxItem,
            FormCollection collection)
        {
            qtUserConfigService _userconfig = new qtUserConfigService();
            qtUserConfig userconfig = new qtUserConfig();
            if (UserConfigId.HasValue && UserConfigId.Value > 0)
            {
                userconfig = _userconfig.FindByKey(UserConfigId);
            }
            userconfig.AppName = AppName;
            userconfig.AppCode = AppCode;
            userconfig.IsActive = IsActive;
            userconfig.UnitId = UnitId;
            if (UserConfigId.HasValue && UserConfigId.Value > 0)
                _userconfig.Update(userconfig);
            else
                _userconfig.Insert(userconfig);
            if (cbxItem != null && cbxItem.Count() > 0)
            {
                qtUserConfigDetailService _configdetail = new qtUserConfigDetailService();
                _configdetail.RunSql("Delete qtUserConfigDetail where UserConfigId=" + userconfig.UserConfigId);
                foreach (string item in cbxItem)
                {
                    qtUserConfigDetail configdetail = new qtUserConfigDetail();

                    configdetail.UserConfigId = userconfig.UserConfigId;

                    configdetail.UserId = TypeHelper.ToInt32(item);

                    configdetail.OrderBy = TypeHelper.ToInt32(collection["OrderBy" + configdetail.UserId]);

                    _configdetail.Insert(configdetail);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] cbxItem)
        {
            qtUserConfigService _userconfig = new qtUserConfigService();
            qtUserConfig userconfig = new qtUserConfig();
            if (cbxItem != null && cbxItem.Count() > 0)
            {
                qtUserConfigDetailService _userconfigdetail = new qtUserConfigDetailService();
                foreach (int item in cbxItem)
                {
                    try
                    {
                        userconfig = _userconfig.FindByKey(item);
                        if (userconfig != null)
                        {
                            _userconfigdetail.DeleteObject(_userconfigdetail.FindList().Where(m => m.UserConfigId == item).ToList());
                            _userconfig.Delete(userconfig);
                        }
                    }
                    catch (Exception) { }
                }
            }

            return RedirectToAction("Index");
        }
    }
}