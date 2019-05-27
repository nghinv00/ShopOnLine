using C.Core.Common;
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
    public class shOrderDetailService : BaseService<shOrderDetail, ShopOnlineDb>
    {

        #region Get DS
        public IEnumerable<shOrderDetail> DanhSachOrderDetail()
        {
            shOrderDetailService _orderDetail = new shOrderDetailService();
            return _orderDetail.FindList().OrderBy(x => x.SortOrder);
        }

        public IEnumerable<shOrderDetail> DanhSachOrderDetail_TheoThoiGian(DateTime StartTime, DateTime EndTime)
        {
            StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 0, 0, 1);
            EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, 23, 59, 59);

            IEnumerable<shOrderDetail> ds = DanhSachOrderDetail().Where(x =>
                                                    (TypeHelper.CompareDate(x.NgayDat.GetValueOrDefault(DateTime.Now), StartTime)
                                                    && TypeHelper.CompareDate(EndTime, x.NgayDat.GetValueOrDefault(DateTime.Now)))
                                                    );

            return ds;
        }


        public IEnumerable<shOrderDetail> DanhSachOrderDetailBy(string OrderGuid, string MemberGuiId, string ProductGuid)
        {
            shOrderDetailService _orderDetail = new shOrderDetailService();
            IEnumerable<shOrderDetail> ds = DanhSachOrderDetail();

            if (!string.IsNullOrWhiteSpace(OrderGuid) || !string.IsNullOrEmpty(OrderGuid))
                ds = ds.Where(x => x.OrderGuid == OrderGuid);

            if (!string.IsNullOrWhiteSpace(MemberGuiId) || !string.IsNullOrEmpty(MemberGuiId))
                ds = ds.Where(x => x.MemberGuiId == MemberGuiId);

            if (!string.IsNullOrWhiteSpace(ProductGuid) || !string.IsNullOrEmpty(ProductGuid))
                ds = ds.Where(x => x.ProductGuid == ProductGuid);

            return ds;
        }
        #endregion

        #region Insert - Update
        public shOrderDetail Insert_Update(
            string OrderDetailGuid,
            int? OrderDetailId,
            string OrderDetailName,
            string OrderGuid,
            string MemberGuid,
            string ProductGuid,
            string ProductName,
            string SectionGuid,
            string SizeGuid,
            int? Number,
            decimal? Prices,
            decimal? Total,
            bool? Status,
            DateTime? CreateDate,
            DateTime? NgayDat)
        {
            shOrderDetailService _orderDetail = new shOrderDetailService();

            shOrderDetail orderDetail = new shOrderDetail();

            if (!string.IsNullOrWhiteSpace(OrderDetailGuid) || !string.IsNullOrEmpty(OrderDetailGuid))
                orderDetail = _orderDetail.FindByKey(OrderDetailGuid);
            else
                orderDetail.OrderDetailGuid = GuidUnique.getInstance().GenerateUnique();

            //orderDetail.OrderDetailId = OrderDetailId;
            orderDetail.OrderDetailName = OrderDetailName;
            orderDetail.OrderGuid = OrderGuid;
            orderDetail.MemberGuiId = MemberGuid;
            orderDetail.ProductGuid = ProductGuid;
            orderDetail.ProductName = ProductName;
            orderDetail.SectionGuid = SectionGuid;
            orderDetail.SizeGuid = SizeGuid;
            orderDetail.Number = Number;
            orderDetail.Prices = Prices;
            orderDetail.Total = Total;
            orderDetail.Status = Status;
            orderDetail.CreateDate = CreateDate;
            orderDetail.NgayDat = NgayDat;

            if (orderDetail.OrderDetailId > 0)
                _orderDetail.Update(orderDetail);
            else
                _orderDetail.Insert(orderDetail);

            return orderDetail;
        }
        #endregion
    }
}
