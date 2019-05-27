
using C.Core.Common;
using C.Core.ExModel;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shSizeService : BaseService<shSetSize, ShopOnlineDb>
    {
        public string SizeName(string SizeGuid)
        {
            if (!string.IsNullOrWhiteSpace(SizeGuid) || !string.IsNullOrEmpty(SizeGuid))
            {

                shSizeService _size = new shSizeService();

                shSetSize size = _size.FindByKey(SizeGuid);

                if (size == null)
                {
                    return string.Empty;
                }
                return size.SizeName;
            }
            return string.Empty;
        }
        public decimal ProductSize_Quantity(string SizeGuid, int Quantity)
        {
            decimal price = ProductPrice(SizeGuid);

            return price * Quantity;
        }

        public decimal ProductPrice(string SizeGuid)
        {
            decimal price = default(decimal);
            if (!string.IsNullOrEmpty(SizeGuid) || !string.IsNullOrWhiteSpace(SizeGuid))
            {
                shSaleService _sale = new shSaleService();
                price = _sale.TinhToanSoTienKhuyenMaiCuaSanPham(DateTime.Now, null, SizeGuid);
            }

            return price;
        }

        public decimal ListProductPrice_Quantity(List<CartItem> ds, decimal MatHangGiamGia, decimal DiemThuong, decimal PhiGiaoHang)
        {

            shSaleService _sale = new shSaleService();
            decimal price = default(decimal);

            foreach (var item in ds)
            {
                price += _sale.TinhToanSoTienKhuyenMaiCuaSanPham(DateTime.Now, null, item.SizeGuid) * item.Quantity;
            }

            if (ds.Count() > 0)
                price -= MatHangGiamGia - DiemThuong - PhiGiaoHang;


            return price;
        }

        #region GetListSize
        public IEnumerable<shSetSize> DanhSachSize()
        {
            shSizeService _size = new shSizeService();

            return _size.FindList().OrderByDescending(m => m.SizeId).OrderBy(m => m.SizeId).Where(m => m.Status == true);
        }
        public IEnumerable<shSetSize> DanhSachSize_ByParent(string SizeGuid)
        {
            IEnumerable<shSetSize> dsSize = DanhSachSize().Where(x => x.ParentId == SizeGuid);

            return dsSize;
        }

        public IEnumerable<shSetSize> DanhSachSize_BySectionGuid(string SectionGuid, string ProductGuid, string SizeGuid)
        {
            IEnumerable<shSetSize> dsSize = DanhSachSize();

            if (!string.IsNullOrWhiteSpace(SectionGuid))
            {
                dsSize = dsSize.Where(m => m.SectionGuid == SectionGuid);
            }

            if (!string.IsNullOrWhiteSpace(ProductGuid))
            {
                dsSize = dsSize.Where(x => x.ProductGuid == ProductGuid);
            }

            if (!string.IsNullOrEmpty(SizeGuid))
            {
                dsSize = dsSize.Where(m => m.SizeGuid == SizeGuid);
            }

            return dsSize.OrderBy(x => x.SortOrder);
        }

        public IEnumerable<shSetSize> DanhSachSize_BySectionGuid_ParentNull(string SectionGuid, string ProductGuid, string SizeGuid)
        {
            IEnumerable<shSetSize> dsSize = DanhSachSize();

            if (!string.IsNullOrWhiteSpace(SectionGuid))
            {
                dsSize = dsSize.Where(m => m.SectionGuid == SectionGuid);
            }

            if (!string.IsNullOrWhiteSpace(ProductGuid))
            {
                dsSize = dsSize.Where(x => x.ProductGuid == ProductGuid);
            }

            if (!string.IsNullOrEmpty(SizeGuid))
            {
                dsSize = dsSize.Where(m => m.SizeGuid == SizeGuid);
            }

            dsSize = dsSize.Where(x => string.IsNullOrWhiteSpace(x.ParentId) || string.IsNullOrEmpty(x.ParentId));

            return dsSize;
        }
        public IPagedList<shSetSize> DanhSachSize_PhanTrang(string SectionGuid, string ProductGuid, string SizeGuid, int pageCurrent, int pageSize)
        {
            IPagedList<shSetSize> pageListSize = DanhSachSize_BySectionGuid(SectionGuid, ProductGuid, SizeGuid).ToPagedList(pageCurrent, pageSize);

            return pageListSize;
        }
        #endregion


        #region Insert - Update shSize 
        public shSetSize InsertUpdateSize(string SizeGuid, int? SizeId, string ProductGuid, string SectionGuid, string SizeName, string Description, decimal? PriceCurrent, int? PercentCurrent, decimal? PriceAfterPercents, int? Number, int? SizeStatus, int? SortOrder, bool? Status, DateTime? CreateDate, string Stuff, string Parent)
        {
            shSizeService _size = new shSizeService();
            shSetSize size = new shSetSize();

            if (!string.IsNullOrWhiteSpace(SizeGuid))
                size = _size.FindByKey(SizeGuid);
            else
                size.SizeGuid = GuidUnique.getInstance().GenerateUnique();

            //size.SizeId = SizeId
            size.ProductGuid = ProductGuid;
            size.SectionGuid = SectionGuid;
            size.SizeName = SizeName;
            size.Description = Description;
            size.PriceCurrent = PriceCurrent;
            size.PercentCurrent = PercentCurrent;
            size.PriceAfterPercents = PriceAfterPercents;
            size.Number = Number;
            size.SizeStatus = SizeStatus;
            size.SortOrder = SortOrder;
            size.Status = Status;
            size.CreateDate = CreateDate;

            Stuff = Stuff.Trim().TrimEnd().TrimStart();

            size.Stuff = Stuff;
            size.ParentId = Parent;

            if (size.SizeId > 0)
                _size.Update(size);
            else
                _size.Insert(size);

            return size;
        }
        #endregion


        public int SoLuongTonSetSize()
        {
            IEnumerable<shSetSize> ds = DanhSachSize().Where(x => x.Inventory > 0);

            return ds.Count();
        }

        public IEnumerable<shSetSize> DanhSachSection_Size(string ProductGuid)
        {
            shSizeService _size = new shSizeService();
            shSectionService _section = new shSectionService();
            IEnumerable<shProductSet> dsParent = _section.DanhSachSection_TheoProductGuid(ProductGuid);
            IEnumerable<shSetSize> dsChild = new List<shSetSize>();
            List<shSetSize> dsSize = new List<shSetSize>();
            foreach (var parent in dsParent)
            {
                dsChild = _size.DanhSachSize_BySectionGuid_ParentNull(parent.SectionGuid, ProductGuid, null);

                foreach (var child in dsChild)
                {
                    child.SizeName = parent.SectionName + " - " + child.SizeName + " - " + child.Stuff;
                    dsSize.Add(child);
                }
            }

            return dsSize;

        }
    }
}
