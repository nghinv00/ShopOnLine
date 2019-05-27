using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Membership.Helper;
using C.Core.Model;
using C.Core.Service;
using C.QuanTri.Helper;
using C.UI.Helpers;
using C.UI.Tool;
using C.UI.PagedList;
using C.Core.BaseController;

namespace C.QuanTri.Controllers
{
    public class UnitManagerController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int? UnitId, int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue) pageCurrent = page.Value;
            qtUnitManagerService _unit= new qtUnitManagerService();
            IPagedList<qtUnitManager> unit = _unit.DanhSachNguoiDung(pageCurrent, 10);
            // 
            ViewBag.DonVi = TreeUnit(UnitId);
            return View(unit);
        }

        private string TreeUnit(int? unitId)
        {
            string html = string.Empty;

            qtUnitService _unit = new qtUnitService();
            qtUnitManagerService _manager = new qtUnitManagerService();

            IEnumerable<qtUnit> list = _unit.FindList().Where(m => m.Status == true).OrderBy(m => m.SortOrder);

            if (list != null && list.Count() > 0)
            {
                html += "<div class='treeview'>";
                foreach (qtUnit unit in list)
                {
                    string css = "";
                    //int soluong = _manager.SoluongNguoiDung(unit.UnitId);
                    int soluong = _manager.SoluongNguoiDung(unit.UnitId);
                    if (unitId.HasValue)
                    {
                        if (unitId == unit.UnitId)
                            css = " expanded";
                    }
                    html += "<ul class='ul-tree '><li class='" + css + "'><img class='toggler " + css + "'  /><a  href='/QuanTri/UnitManager/Index?UnitId=" + unit.UnitId + "'>" + unit.UnitName + "(" + soluong + ")</a>";
                    
                    html += "</li></ul>";
                }
                html += "</div>";
            }
            return html;

        }

        public ActionResult Create(int? Id)
        {
            qtUserService _user = new qtUserService();
            IEnumerable<qtUser> list = _user.FindList().Where(m => m.Status == true && m.IsAdmin == false).OrderBy(m => m.SortOrder);
            ViewBag.User = list.ToSelectList(u => u.UserName + "-" + u.qtPosition.PositionName + "-" + u.qtDepartment.DepartmentName + "-" + u.qtUnit.UnitName, u => u.UserId.ToString(), Id.ToString());
            GetUnit(Id);
            ViewBag.UserId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(int? User, string[] cbxUnit)
        {
            qtUnitManagerService _managerservice = new qtUnitManagerService();
            if (cbxUnit.Length > 0 && cbxUnit.ToString() != "" && cbxUnit[0] != "")
            {
                List<qtUnitManager> list = _managerservice.FindList().Where(u => u.UserId == User).ToList();
                foreach(qtUnitManager unit in list)
                {
                    _managerservice.Delete(unit);
                }
                foreach (string item in cbxUnit)
                {
                    qtUnitManager manager = new qtUnitManager();
                    manager.UserId = User;
                    manager.UnitId = TypeHelper.ToInt32(item);
                    manager.Active = true;
                    _managerservice.Insert(manager);
                }
            }
            return RedirectToAction("Index");
        }

        public void GetUnit(int? userId)
        {
            qtUnitService _service = new qtUnitService();

            IEnumerable<qtUnit> list = _service.FindList().AsQueryable().Where(r => r.Status == true&& r.ParentId!=0);
            string html = string.Empty;
            html += "<table class='grid' style='width: 100%;'>" +
            "<thead>" +
            "<tr>" +
            "<th><input type='checkbox' name='cbxList' id='cbxList'></th>" +
            "<th> Tên đơn vị </th>" +

            "</thead>";
            foreach (qtUnit unit in list)
            {
                string check = checkUnit(userId, unit.UnitId);
                html += "<tr>"+
                "<td style='text-align: center;'><input type='checkbox' value=" + unit.UnitId + " name='cbxUnit' id='cbxUnit' " + check + " ></td>" +
                     "<td>" + unit.UnitName + "</td>" +
                     "</tr>";

            }
            html += "</table>";
            ViewBag.UnitList = html;
        }

        public string checkUnit(int? userId, int? unitId)
        {
            qtUnitManagerService _managerservice = new qtUnitManagerService();
            string check = string.Empty;
            if (!userId.HasValue || !unitId.HasValue)
            {
                return check;
            }
            qtUnitManager list = _managerservice.FindList().AsQueryable()
                            .Where(u => u.UnitId == unitId && u.UserId == userId && u.Active == true)
                            .FirstOrDefault();
            if (list != null)
            {
                return "checked='checked'";
            }
            return check;
        }

        [HttpPost]
        public ActionResult Delete(string[] cbxItem)
        {
            qtUnitManagerService _managerservice = new qtUnitManagerService();
            foreach (var item in cbxItem)
            {
                List<qtUnitManager> list = _managerservice.FindList().Where(m => m.UserId == TypeHelper.ToInt32(item)).ToList();
                if(list.Count()>0)
                {
                    foreach(qtUnitManager unit in list)
                    {
                        _managerservice.Delete(unit.UnitManagerId);
                    }
                }
               
            }
            return RedirectToAction("Index");
        }
    }
}
