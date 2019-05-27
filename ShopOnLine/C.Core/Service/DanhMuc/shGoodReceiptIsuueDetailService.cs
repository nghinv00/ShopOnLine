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
    public class shGoodReceiptIsuueDetailService : BaseService<shGoodReceiptIsuueDetail, ShopOnlineDb>
    {
        #region Danh sách

        public IEnumerable<shGoodReceiptIsuueDetail> DanhSachPhieuXuatNhap()
        {
            shGoodReceiptIsuueDetailService _receiptissue = new shGoodReceiptIsuueDetailService();
            return _receiptissue.FindList().Where(x => x.Status == true);
        }

        public IEnumerable<shGoodReceiptIsuueDetail> DanhSachPhieuXuatNhap_ByParent(string ReceiptIsuueGuid)
        {
            IEnumerable<shGoodReceiptIsuueDetail> ds = DanhSachPhieuXuatNhap().Where(x => x.ReceiptIsuueGuid == ReceiptIsuueGuid);
            return ds;
        }

        public IEnumerable<shGoodReceiptIsuueDetail> DanhSachPhieuXuatNhap_ByParent(string ReceiptIsuueGuid , string ProductGuid, string SectionGuid, string SizeGuid)
        {
            IEnumerable<shGoodReceiptIsuueDetail> ds = DanhSachPhieuXuatNhap();

            if (!string.IsNullOrEmpty(ReceiptIsuueGuid) || !string.IsNullOrWhiteSpace(ReceiptIsuueGuid))
            {
                ds = ds.Where(x => x.ReceiptIsuueGuid == ReceiptIsuueGuid);
            }

            if (!string.IsNullOrEmpty(ProductGuid) || !string.IsNullOrWhiteSpace(ProductGuid))
            {
                ds = ds.Where(x => x.ProductGuid == ProductGuid);
            }

            if (!string.IsNullOrEmpty(SectionGuid) || !string.IsNullOrWhiteSpace(SectionGuid))
            {
                ds = ds.Where(x => x.SectionGuid == SectionGuid);
            }

            if (!string.IsNullOrEmpty(SizeGuid) || !string.IsNullOrWhiteSpace(SizeGuid))
            {
                ds = ds.Where(x => x.SizeGuid == SizeGuid);
            }

            return ds;
        }

        public IPagedList<shGoodReceiptIsuueDetail> DanhSachPhieuXuatNhap_PhanTrang(int page, int pageSize)
        {
            IEnumerable<shGoodReceiptIsuueDetail> ds = DanhSachPhieuXuatNhap();
            return ds.ToPagedList(page, pageSize);
        }
        #endregion

        public shGoodReceiptIsuueDetail Insert_Update(
            string ReceiptIsuueDetailGuid,
            int? ReceiptIsuueDetailId,
            string ReceiptIsuueGuid,
            string ProductGuid,
            string SectionGuid,
            string SizeGuid,
            int? Number,
            bool? Status,
            DateTime? CreateDate,
            int? Phieu
            )
        {
            shGoodReceiptIsuueDetailService _receipt = new shGoodReceiptIsuueDetailService();
            shGoodReceiptIsuueDetail receipt = new shGoodReceiptIsuueDetail();
            if (!string.IsNullOrEmpty(ReceiptIsuueDetailGuid) || !string.IsNullOrWhiteSpace(ReceiptIsuueDetailGuid))
                receipt = _receipt.FindByKey(ReceiptIsuueGuid);
            else
            {
                receipt.ReceiptIsuueDetailGuid = GuidUnique.getInstance().GenerateUnique();
            }

            //receipt.ReceiptIsuueDetailId = ReceiptIsuueDetailId;
            receipt.ReceiptIsuueGuid = ReceiptIsuueGuid;
            receipt.ProductGuid = ProductGuid;
            receipt.SectionGuid = SectionGuid;
            receipt.SizeGuid = SizeGuid;
            receipt.Number = Number;
            receipt.Status = Status;
            receipt.CreateDate = CreateDate;
            receipt.Phieu = Phieu;

            if (receipt.ReceiptIsuueDetailId > 0)
                _receipt.Update(receipt);
            else
                _receipt.Insert(receipt);

            return receipt;
        }
    }
}
