using C.Core.Common;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shSaleService : BaseService<shSale, ShopOnlineDb>
    {

        public IEnumerable<shSale> DanhSach()
        {
            shSaleService _sale = new shSaleService();
            return _sale.FindList().Where(x => x.Status == true).OrderBy(x => x.SaleId);
        }

        public IPagedList<shSale> DanhSach_PhanTrang(int page, int PageSize)
        {
            IPagedList<shSale> ds = DanhSach().ToPagedList(page, PageSize);

            return ds;
        }

        public shSale ChuongTrinhKhuyenMaiHienTai(DateTime dt)
        {

            shSaleService _sale = new shSaleService();
            IEnumerable<shSale> ds = DanhSach();
            shSale sale = new shSale();

            sale = ds.Where(x => TypeHelper.CompareDate(dt, x.StartTime.GetValueOrDefault(dt)) ||
                                    TypeHelper.CompareDate(x.EndTime.GetValueOrDefault(dt), dt))
                                    .FirstOrDefault();

            return sale;
        }

        public shSaleDetail KiemTraSanPhamNamTrongChuongTrinhKhuyenMai(DateTime dt, string ProductGuid, string SizeGuid)
        {   
            shSale sale = ChuongTrinhKhuyenMaiHienTai(dt);
            shSaleDetail saleDetail = new shSaleDetail();
            if (sale == null)
            {
                return saleDetail;
            }

            shSaleDetailService _saleService = new shSaleDetailService();
            IEnumerable<shSaleDetail> ds = _saleService.DanhSachBySaleGuid(sale.SaleGuid);
        
            if (sale.DieuKienApDung == DieuKienApDungKhuyenMai.TatCaSanPham.GetHashCode())
            {
                saleDetail = ds.FirstOrDefault();
                return saleDetail;
            }
            else if (sale.DieuKienApDung == DieuKienApDungKhuyenMai.TheoDanhMuc.GetHashCode())
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(ProductGuid);

                foreach (var item in ds)
                {
                    if (item.CategoryGuidProductGuid == product.CategoryGuid)
                    {
                        saleDetail = item;
                        return item;
                    }
                }
            }
            else if (sale.DieuKienApDung == DieuKienApDungKhuyenMai.TheoSanPhamRiengBiet.GetHashCode())
            {
                shProductService _product = new shProductService();
                shProduct product = _product.FindByKey(ProductGuid);

                foreach (var item in ds)
                {
                    if (item.CategoryGuidProductGuid == product.ProductGuid)
                    {
                        saleDetail = item;
                        return item;
                    }
                }
            }
            return saleDetail;
        }

        public decimal TinhToanSoTienKhuyenMaiCuaSanPham(DateTime dt, string ProductGuid, string SizeGuid)
        {
            shSaleDetail saleDetail = KiemTraSanPhamNamTrongChuongTrinhKhuyenMai(dt, ProductGuid, SizeGuid);

            shSizeService _size = new shSizeService();
            shSetSize size = _size.FindByKey(SizeGuid);
            if (size == null)
            {
                size = new shSetSize();
            }
            decimal price = default(decimal);

            if (saleDetail.SaleDetailId > 0)  // Tồn tại sản phẩm nằm trong danh sach được khuyến mại
            {
                if (saleDetail.CachTinhGiaTriKhuyenMai == CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode())
                {
                    price = size.PriceCurrent.GetValueOrDefault(0) - (size.PriceCurrent.GetValueOrDefault(0) * Convert.ToInt32(saleDetail.GiaTri)) / 100;
                }
                else if (saleDetail.CachTinhGiaTriKhuyenMai == CachTinhGiaTriKhuyenMai.GiamTheoSoTien.GetHashCode())
                {
                    price = size.PriceCurrent.GetValueOrDefault(0) - Convert.ToInt32(saleDetail.GiaTri);
                }
                return Convert.ToInt32(price);
            }
           
            return price;
        }


        #region Insert - Update
        public shSale ThemMoi_CapNhatKhuyenMai(string SaleGuid, string SaleName,
            string SaleCode, int? SaleStatus, string Description,
            string StartTime, string EndTime, int? CachTinhGiaTriKhuyenMai,
            decimal? Percent, double? USD, int? DieuKienApDung,
            string[] CagegoryChild,
            string[] ProductGuid1,
            int? UserId,
            bool Status, DateTime? CreateDate)
        {
            shSaleService _sale = new shSaleService();
            shSale sale = new shSale();
            // 1. Thêm mới bảng shSale
            #region shSale
            string GiaTri = string.Empty;

            if (CachTinhGiaTriKhuyenMai == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoPhanTramGiaTri.GetHashCode())
            {
                GiaTri = Format.FormatDecimalToString(Percent.GetValueOrDefault(0)).Replace(",", "");
                GiaTri = Convert.ToInt32(Percent.GetValueOrDefault(0)).ToString();
            }
            else if (CachTinhGiaTriKhuyenMai == C.Core.Common.CachTinhGiaTriKhuyenMai.GiamTheoSoTien.GetHashCode())
            {
                GiaTri = Convert.ToInt32(USD.GetValueOrDefault(0)).ToString();
            }

            sale = Insert_Update(
                            SaleGuid,
                            null,
                            SaleName,
                            SaleCode,
                            Description,
                            CachTinhGiaTriKhuyenMai,
                            GiaTri,
                            DieuKienApDung,
                            TypeHelper.ToDate(StartTime),
                            TypeHelper.ToDate(EndTime),
                            SaleStatus,
                            UserId,
                            null,
                            Status,
                            CreateDate);
            #endregion

            // 2. Thêm mới bảng shSaleDetail
            #region ShSaleDetail
            string MaCauHinh = Config.DieuKienApDungKhuyenMai_TatCaSanPham;
            List<string> DanhSach = new List<string>();

            if (DieuKienApDung == DieuKienApDungKhuyenMai.TatCaSanPham.GetHashCode())
            {
                MaCauHinh = Config.DieuKienApDungKhuyenMai_TatCaSanPham;
                DanhSach.Add(Config.DieuKienApDungKhuyenMai_MaCauHinh_TatCaSanPham);
            }
            else if (DieuKienApDung == DieuKienApDungKhuyenMai.TheoDanhMuc.GetHashCode())
            {
                MaCauHinh = Config.DieuKienApDungKhuyenMai_TheoDanhMuc;
                DanhSach = CagegoryChild.ToList();

            }
            else if (DieuKienApDung == DieuKienApDungKhuyenMai.TheoSanPhamRiengBiet.GetHashCode())
            {
                MaCauHinh = Config.DieuKienApDungKhuyenMai_TheoSanPham;
                DanhSach = ProductGuid1.ToList();
            }


            shSaleDetailService _saleService = new shSaleDetailService();
            foreach (var item in DanhSach)
            {
                shSaleDetail saleDetail = _saleService.InsertOrUpdate(
                                            null,
                                            null,
                                            sale.SaleGuid,
                                            CachTinhGiaTriKhuyenMai,
                                            GiaTri,
                                            DieuKienApDung,
                                            TypeHelper.ToDate(StartTime),
                                            TypeHelper.ToDate(EndTime),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null, null,
                                            true,
                                            DateTime.Now,
                                            MaCauHinh,
                                            item);

            }
            #endregion

            return sale;
        }

        public shSale Insert_Update(
                string SaleGuid,
                int? SaleId,
                string SaleName,
                string SaleCode,
                string Description,
                int? CachTinhGiaTriKhuyenMai,
                string GiaTri,
                int? DieuKienApDung,
                DateTime? StartTime,
                DateTime? EndTime,
                int? SaleStatus,
                int? UserId,
                string MetaTitle,
                bool? Status,
                DateTime? CreateDate)
        {

            shSaleService _sale = new shSaleService();
            shSale sale = new shSale();

            if (!string.IsNullOrEmpty(SaleGuid) || !string.IsNullOrWhiteSpace(SaleGuid))
            {
                sale = _sale.FindByKey(SaleGuid);
            }
            else
            {
                sale.SaleGuid = GuidUnique.getInstance().GenerateUnique();
            }
            //sale.SaleDetailGuid
            sale.SaleName = SaleName;
            sale.SaleCode = SaleCode;
            sale.Description = Description;
            sale.CachTinhGiaTriKhuyenMai = CachTinhGiaTriKhuyenMai;
            sale.GiaTri = GiaTri;
            sale.DieuKienApDung = DieuKienApDung;
            sale.StartTime = StartTime;
            sale.EndTime = EndTime;
            sale.SaleStatus = SaleStatus;
            sale.UserId = UserId;
            sale.MetaTitle = MetaTitle;
            sale.Status = Status;
            sale.CreateDate = CreateDate;

            if (sale.SaleId > 0)
                _sale.Update(sale);
            else
                _sale.Insert(sale);

            return sale;
        }
        #endregion
    }
}
