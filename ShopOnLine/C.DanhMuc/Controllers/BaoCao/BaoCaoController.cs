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
using C.Membership.Helper;

namespace C.DanhMuc.Controllers
{
    public class BaoCaoController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            LoadDuLieu();

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            LoadDuLieu();

            return View();
        }

        public void LoadDuLieu()
        {
            shOrderService _order = new shOrderService();
            @ViewBag.DonHang = _order.SoDonHangTrongNgay(DateTime.Now);
            @ViewBag.DoanhThuTheoNgay = Format.FormatDecimalToString(_order.DoanhThuTheoNgay(DateTime.Now));
            
            shMemberService _member = new shMemberService();
            @ViewBag.NewMember = _member.DanhSachMemberDangKyMoi();

            shProductService _product = new shProductService();
            shSectionService _section = new shSectionService();
            shSizeService _size = new shSizeService();

            @ViewBag.SoLuongTon = _size.SoLuongTonSetSize();
        }

        #endregion

        #region Báo cáo theo tháng
        public ActionResult DoanhThuDonHang()
        {
            DataPoints dataPoints = new DataPoints();

            List<DataPoints> dsDoanhThu = new List<DataPoints>();
            List<DataPoints> dsDonHang = new List<DataPoints>();

            DateTime toDay = DateTime.Now;

            int numberInDay = System.DateTime.DaysInMonth(toDay.Year, toDay.Month);

            shOrderService _order = new shOrderService();
            IEnumerable<shOrder> dsOrder = _order.DanhSachDonHangTrongThang(toDay.Month, toDay.Year);

            // 1..   
            for (int i = 1; i <= numberInDay; i++)
            {
                dataPoints = new DataPoints();
                dataPoints.x = i;
                dataPoints.y = _order.DoanhThuTrongNgay(dsOrder, i);
                dsDoanhThu.Add(dataPoints);

            }

            // 2..
            for (int i = 1; i <= numberInDay; i++)
            {
                dataPoints = new DataPoints();
                dataPoints.x = i;
                dataPoints.y = _order.SoDonTrongNgay(dsOrder, i).Count();
                dsDonHang.Add(dataPoints);
            }

            BaoCao baocao = new BaoCao
            {
                dsDoanhThu = dsDoanhThu,
                dsDonHang = dsDonHang
            };

            if (Request.IsAjaxRequest())
            {
                return Json(baocao, JsonRequestBehavior.AllowGet);
            }

            return PartialView("DoanhThuDonHang", baocao);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                shCategoryService _category = new shCategoryService();
                shCategory category = _category.FindByKey(id);

                if (category != null)
                {
                    ViewBag.DanhMucId = category.CategoryId;
                    ViewBag.DanhMucGuid = category.CategoryGuid;
                    ViewBag.ParentId = category.ParentId;
                    return View(category);
                }
                else
                    return View(new shCategory());
            }

            ViewBag.CategoryId = id;
            return View(new shCategory());
        }

        [HttpPost]
        public ActionResult Create(string DanhMucGuid, int? DanhMucId, string CategoryName, string CategoryGuid, bool? Status, string Description, int? SortOrder, string FileName)
        {
            // Insert Category
            shCategoryService _category = new shCategoryService();
            shCategory category = _category.ThemMoi_HieuChinhCategory(
                DanhMucGuid,
                null,
                CategoryName,
                CategoryGuid,
                HttpContext.User.Identity.GetUserLogin().Userid,
                Status,
                DateTime.Now,
                null,
                Description,
                SortOrder,
                FileName);

            // Insesrt Image 
            if (!string.IsNullOrEmpty(FileName) || !string.IsNullOrWhiteSpace(FileName))
            {
                shCategoryImageService _categoryImage = new shCategoryImageService();
                shCategoryImage categoryImage = _categoryImage.Insert_UpdateCategoryImage(
                    null,
                    null,
                    category.CategoryGuid,
                    FileName,
                    null,
                    User.Identity.GetUserLogin().Userid,
                    true,
                    DateTime.Now,
                    null
                    );
            }

            int? page = CommonHelper.FindPageCategory(category.CategoryGuid, category.CategoryId);

            return RedirectToAction("Index", new { page = page });
        }
        #endregion

    }

    public class DataPoints
    {
        public int x { get; set; }
        public decimal y { get; set; }
    }

    public class BaoCao
    {
        public List<DataPoints> dsDoanhThu { get; set; }
        public List<DataPoints> dsDonHang { get; set; }
    }

}