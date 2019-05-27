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
    public class shSaleDetailService : BaseService<shSaleDetail, ShopOnlineDb>
    {
        public IEnumerable<shSaleDetail> DanhSach()
        {
            shSaleDetailService _sale = new shSaleDetailService();
            return _sale.FindList().Where(x => x.Status == true).OrderBy(x => x.SaleDetailId);
        }

        public IEnumerable<shSaleDetail> DanhSachBySaleGuid(string SaleGuid)
        {
            return DanhSach().Where(x => x.SaleGuid == SaleGuid);
        }

        public shSaleDetail InsertOrUpdate(
            string SaleDetailGuid,
            int? SaleDetailId,
            string SaleGuid,
            int? CachTinhGiaTriKhuyenMai,
            string GiaTri,
            int? DieuKienApDung,
            DateTime? StartDate,
            DateTime? EndDate,
            String CategoryGuid, //
            string ProductGuid,//
            int? Percents,//
            string Description,//
            string Notes,//
            decimal? PriceAfterPercents,//
            string SaleAttach,//
            bool? Status,//
            DateTime? CreateDate,//
            string MaCauHinh,
            string CategoryGuidProductGuid
            )
        {

            shSaleDetailService _sale = new shSaleDetailService();
            shSaleDetail sale = new shSaleDetail();

            if (!string.IsNullOrEmpty(SaleDetailGuid) || !string.IsNullOrWhiteSpace(SaleDetailGuid))
            {
                sale = _sale.FindByKey(SaleDetailGuid);
            }
            else
            {
                sale.SaleDetailGuid = GuidUnique.getInstance().GenerateUnique();
            }

            //sale.SaleDetailId = SaleDetailId;
            sale.SaleGuid = SaleGuid;
            sale.CachTinhGiaTriKhuyenMai = CachTinhGiaTriKhuyenMai;
            sale.GiaTri = GiaTri;
            sale.DieuKienApDung = DieuKienApDung;
            sale.StartDate = StartDate;
            sale.EndDate = EndDate;
            sale.CategoryGuid = CategoryGuid;
            sale.ProductGuid = ProductGuid;
            sale.Percents = Percents;
            sale.Description = Description;
            sale.Notes = Notes;
            sale.PriceAfterPercents = PriceAfterPercents;
            sale.SaleAttach = SaleAttach;
            sale.Status = Status;
            sale.CreateDate = CreateDate;
            sale.CategoryGuidProductGuid = CategoryGuidProductGuid;
            sale.MaCauHinh = MaCauHinh;

            if (sale.SaleDetailId > 0)
                _sale.Update(sale);
            else
                _sale.Insert(sale);

            return sale;
        }
    }
}
