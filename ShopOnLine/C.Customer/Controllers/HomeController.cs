using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.UI.PagedList;
using C.Membership.Helper;

namespace C.Customer.Controllers
{
    // Trang chủ 
    public class HomeController : CustomController
    {
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Carousel Trang chủ
        public ActionResult Carousel()
        {
            shProductService _product = new shProductService();

            IEnumerable<shProduct> model = _product.DanhSachTopHotProduct();

            return PartialView(model);
        }
        #endregion

        #region Danh sách danh mục chứa các sản phẩm nổi bật 
        [HttpGet]
        public ActionResult Product()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> model = _category.DanhSachCategory_TopHot();

            int max_ = 4;
            IEnumerable<shCategory> dsC = (new shCategoryService()).DanhSachCategory().Where(x => x.ParentId == "abbce1a1efc649eabbd6949f40f83c60").Take(max_);

            int socconlai = 4 - dsC.Count();

            IEnumerable<shProduct> dsP = (new shProductService()).DanhSachProduct_TheoDanhMuc("abbce1a1efc649eabbd6949f40f83c60").Take(socconlai);


            return PartialView("Product", model);
        }
        #endregion

        #region Danh sách tin tức , bài viết nổi bật 
        public ActionResult News()
        {
            shNewService _tintuc = new shNewService();
            IEnumerable<shNew> dsNews = _tintuc.DanhSachNews()
                                        .ToPagedList(1, 5);

            return PartialView(dsNews);
        }
        #endregion

        #region Danh sách ý kiến đánh giá từ khách hàng
        public ActionResult IdeaCustomer()
        {
            shIdeaCustomerService _idea = new shIdeaCustomerService();
            
            return PartialView(_idea.DanhSachIdea());
        }
        #endregion

        #region Giới thiệu về công ty 
        public ActionResult AboutUs()
        {

            return PartialView();
        }
        #endregion

        #region Tìm kiếm đại lý gần nhất 
        public ActionResult SearchAgent()
        {

            #region Dropdonwlist 
            landProvinceService _province = new landProvinceService();

            ViewBag.City = new SelectList(_province.DanhSachProvince(), "ProvinceId", "Name", null);

            ViewBag.Town = new SelectList(new List<landDistrict>(), "DistrictId", "Name", null);

            #endregion

            return PartialView();
        }
        #endregion

        #region ChildMenuProducts
        public ActionResult MenuProduct()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> dsCategory = _category.DanhSachCategory();

            return PartialView("MenuProduct", dsCategory);
        }
        #endregion

        #region Thông báo
        [HttpPost]
        public ActionResult Notify()
        {
            ThongBaoService _thongbao = new ThongBaoService();
            IEnumerable<ThongBao> ds = _thongbao.FindList().Where(x => x.NguoiNhanThongBaoId == User.Identity.GetUserLogin().Userid
                                                                        && x.DaXem == false);

            return PartialView("Notify", ds);
        }

        [HttpPost]
        public ActionResult Notify_Count()
        {
            ThongBaoService _thongbao = new ThongBaoService();
            IEnumerable<ThongBao> ds = _thongbao.FindList().Where(x => x.NguoiNhanThongBaoId == User.Identity.GetUserLogin().Userid
                                                            && x.DaXem == false);

            if (Request.IsAjaxRequest())
            {
                return Json(ds.Count(), JsonRequestBehavior.AllowGet);
            }

            return PartialView("Notify", ds.Count());
        }
        #endregion
    }
}
