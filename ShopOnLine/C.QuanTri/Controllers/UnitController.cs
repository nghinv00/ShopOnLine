using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.Core.Common;
using C.UI.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.QuanTri.Controllers
{
    public class UnitController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListUnits(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListUnits(pageCurrent);
            return View();
        }

        public PartialViewResult ListUnits(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            qtUnitService _unit = new qtUnitService();

            IPagedList<qtUnit> dsUnit = _unit.DanhSachUnit_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListUnits = dsUnit;

            return PartialView("ListUnits", dsUnit);
        }
        #endregion

        #region Create - Update
        [HttpGet]
        public ActionResult Create(int? id, int? UnitId)
        {

            qtUnitService _unit = new qtUnitService();
            qtUnit unit = new qtUnit();

            if (id.HasValue)
            {
                unit = _unit.FindByKey(id);
                if (unit != null)
                {
                    ViewBag.DonViId = unit.UnitId;
                    return View(unit);
                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống. Vui lòng kiểm tra lại");
                    return View(new qtUnit());
                }
            }

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? DonViId, string UnitCode, string UnitName, int? UnitId, int? SortOrder, string LevelCode, string Email, string PhoneNumber, bool? Status)
        {
            if (!DonViId.HasValue)
            {
                DonViId = 0;
            }
            qtUnitService _unit = new qtUnitService();

            qtUnit unit = _unit.ThemMoi_HieuChinhUnit(
                                        DonViId.Value, 
                                        UnitName,
                                        UnitCode, 
                                        UnitId, 
                                        LevelCode,
                                        SortOrder, 
                                        Email, 
                                        PhoneNumber,
                                        Status, 
                                        DateTime.Now);

            return RedirectToAction("Index");
        }

        #endregion

    }
}
