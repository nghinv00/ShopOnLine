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
    public class PositionController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListPositions(pageCurrent);

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListPositions(pageCurrent);

            return View();
        }

        public PartialViewResult ListPositions(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            qtPositionService _position = new qtPositionService();

            IPagedList<qtPosition> dsPosition = _position.DanhSachPosition_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListPositions = dsPosition;
            return PartialView("ListPositions", dsPosition);
        }

        #endregion

        #region  Create - Update
        [HttpGet]
        public ActionResult Create(int? id)
        {
            qtPositionService _position = new qtPositionService();
            qtPosition position = new qtPosition();

            if (id.HasValue)
            {
                position = _position.FindByKey(id);
                if (position != null)
                {
                    return View(position);
                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống. Vui lòng kiểm tra lại");
                    return View(new qtPosition());
                }
            }
            return View(position);
        }

        [HttpPost]
        public ActionResult Create(
            int? PositionId, string PositionName,
            string Code, int? SortOrder, bool? Status)
        {
            qtPositionService _position = new qtPositionService();
            if (!PositionId.HasValue)
                PositionId = 0;

            qtPosition position = _position.ThemMoi_HieuChinhPosition(
                                PositionId.Value,
                                PositionName,
                                Code,
                                SortOrder,
                                Status,
                                DateTime.Now);

            return RedirectToAction("Index");
        }
        #endregion

    }
}
