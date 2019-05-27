using C.Core.Common;
using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Tìm kiếm nâng cao 
    public class SearchController : CustomController
    {

        #region Index
        [HttpGet]
        public ActionResult Index(int? page, string keyword, string do_search)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            IEnumerable<TimKiemNangCao> dsTimKiem = TimKiemNangCao(keyword);

            ViewBag.font_keyword = keyword;
            ViewBag.TongSoKetQua = dsTimKiem.Count();
            ViewBag.SoTrang = (dsTimKiem.Count() / 10) + 1;

            IPagedList<TimKiemNangCao> pageList = dsTimKiem.ToPagedList(pageCurrent, Config.PAGE_SIZE_10);
            return View(pageList);
        }

        [HttpPost]
        public ActionResult Index(string keyword, string do_search)
        {

            IEnumerable<TimKiemNangCao> dsTimKiem = TimKiemNangCao(keyword);

            ViewBag.font_keyword = keyword;
            ViewBag.TongSoKetQua = dsTimKiem.Count();
            ViewBag.SoTrang = (dsTimKiem.Count() / 10) + 1;

            IPagedList<TimKiemNangCao> pageList = dsTimKiem.ToPagedList(1, Config.PAGE_SIZE_10);
            return View(pageList);
        }
        #endregion


        public IEnumerable<TimKiemNangCao> TimKiemNangCao(string keyword)
        {
            List<TimKiemNangCao> ds = new List<TimKiemNangCao>();
            TimKiemNangCao timkiem = new TimKiemNangCao();

            // 1. Tìm kiếm trong bảng sản phẩm
            shProductService _product = new shProductService();
            IEnumerable<shProduct> dsProduct = _product.FindList().Where(x => TypeHelper.CompareString(x.ProductName, keyword));
            foreach (var product in dsProduct)
            {
                timkiem = new TimKiemNangCao();
                timkiem.CategoryId = TheLoaiTimKiemNangCao.SanPham.GetHashCode();
                timkiem.Category = "Sản phẩm";
                timkiem.Name = product.ProductName;
                timkiem.MetaTitle = product.MetaTitle + "-" + product.ProductId;
                timkiem.keyword = keyword;
                timkiem.CreatedDate = product.CreateDate;
                timkiem.Description = " Bộ sản phẩm có kết cấu và giá sản phẩm: &nbsp;&nbsp;&nbsp;&nbsp; Kích ...";
                timkiem.ProductGuid = product.ProductGuid;
                ds.Add(timkiem);
            }


            // 3. Tìm kiếm trong bảng set sản phẩm
            shSectionService _section = new shSectionService();
            IEnumerable<shProductSet> dsSet = _section.FindList().Where(x => TypeHelper.CompareString(x.SectionName, keyword));
            foreach (var set in dsSet)
            {
                shProduct product = _product.FindByKey(set.ProductGuid);

                timkiem = new TimKiemNangCao();
                timkiem.CategoryId = TheLoaiTimKiemNangCao.SetSanPham.GetHashCode();
                timkiem.Category = "Sản phẩm";
                timkiem.Name = product.ProductName + "- Set " + set.SectionName + keyword;
                timkiem.MetaTitle = product.MetaTitle + "-" + product.ProductId;
                timkiem.keyword = keyword;
                timkiem.CreatedDate = set.CreateDate;
                timkiem.Description = " Bộ sản phẩm có kết cấu và giá sản phẩm: &nbsp;&nbsp;&nbsp;&nbsp; Kích ...";
                timkiem.ProductGuid = product.ProductGuid;
                ds.Add(timkiem);
            }

            // 4. Tìm kiếm trong bảng kích thước theo set 
            shSizeService _size = new shSizeService();
            IEnumerable<shSetSize> dsSize = _size.FindList().Where(x => TypeHelper.CompareString(x.SizeName, keyword));
            foreach (var size in dsSize)
            {
                shProduct product = _product.FindByKey(size.ProductGuid);

                timkiem = new TimKiemNangCao();
                timkiem.CategoryId = TheLoaiTimKiemNangCao.SetSanPham.GetHashCode();
                timkiem.Category = "Sản phẩm";
                timkiem.Name = product.ProductName + " - Kích thước: " + size.SizeName;
                timkiem.MetaTitle = product.MetaTitle + "-" + product.ProductId;
                timkiem.keyword = keyword;
                timkiem.CreatedDate = size.CreateDate;
                timkiem.Description = " Bộ sản phẩm có kết cấu và giá sản phẩm: &nbsp;&nbsp;&nbsp;&nbsp; Kích ...";
                timkiem.ProductGuid = product.ProductGuid;
                ds.Add(timkiem);
            }


            // 2. Tìm kiếm trong bảng tin tức
            shNewService _new = new shNewService();
            IEnumerable<shNew> dsNew = _new.FindList().Where(x => TypeHelper.CompareString(x.TitleNew, keyword));
            foreach (var news in dsNew)
            {
                timkiem = new TimKiemNangCao();
                timkiem.CategoryId = TheLoaiTimKiemNangCao.TinTuc.GetHashCode();
                timkiem.Category = "Tin Tức";
                timkiem.Name = news.TitleNew;
                timkiem.MetaTitle = news.MetaTitle + "-" + news.NewId;
                timkiem.keyword = keyword;
                timkiem.CreatedDate = news.CreatedDate;
                timkiem.Description = news.Summary;
                ds.Add(timkiem);
            }

            return ds;
        }

    }
}
