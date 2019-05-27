using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Core.Common;
using C.UI.Tool;

namespace C.DanhMuc.Helper
{
    public static class OrderHelper
    {
        public static MvcHtmlString ThongTinDatHang(this HtmlHelper helper, string FullName, string Phone, string Address, string Email)
        {
            string html = string.Empty;

            html = FullName + " / " + Phone + " / " + Email + " / " + Address;

            return new MvcHtmlString(html);

        }

        public static MvcHtmlString PayType(this HtmlHelper helper, int? PayType)
        {
            string html = string.Empty;

            if (!PayType.HasValue)
                PayType = C.Core.Common.PayType.ThuPhiTanNoi.GetHashCode();

            if (PayType == C.Core.Common.PayType.ThuPhiTanNoi.GetHashCode())
            {
                html = "<span>Thanh toán khi nhận hàng</span>";
            }
            else if (PayType == C.Core.Common.PayType.ChuyenKhoanNganHang.GetHashCode())
            {
                html = "<span>Chuyển khoản ngân hàng</span>";
            }
            else if (PayType == C.Core.Common.PayType.ShowRoomDaiLy.GetHashCode())
            {
                html = "<span>Qua Showroom đại lý</span>";
            }

            return new MvcHtmlString(html);

        }

        public static MvcHtmlString OrderStatusName(this HtmlHelper helper, int? Status)
        {
            if (!Status.HasValue)
                Status = OrderStatus.DangXuLy.GetHashCode();

            string html = string.Empty;

            if (Status == OrderStatus.ThemVaoGioHang.GetHashCode())
            {
                html = "<span class='status-vb badge badge-dark'>Thêm vào giỏ hàng</span>";
            }
            else if (Status == OrderStatus.HinhThucShip.GetHashCode())
            {
                html = "<span class='status-vb badge badge-dark'>Hình thức ship</span>";
            }
            else if (Status == OrderStatus.ThanhToan.GetHashCode())
            {
                html = "<span class='status-vb badge badge-secondary'>Thanh toán</span>";
            }
            else if (Status == OrderStatus.DangXuLy.GetHashCode())
            {
                html = "<span class='status-vb badge badge-info'>Đang xử lý</span>";
            }
            else if (Status == OrderStatus.DangGiaoHang.GetHashCode())
            {
                html = "<span class='status-vb badge badge-warning'>Đang giao hàng</span>";
            }
            else if (Status == OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode())
            {
                html = "<span class='status-vb badge badge-success'>Đã giao hàng </span>";
            }
            else if (Status == OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode())
            {
                html = "<span class='status-vb badge badge-info'>Đã giao hàng(Đã xác nhận)</span>";
            }
            else if (Status == OrderStatus.KetThuc.GetHashCode())
            {
                html = "<span class='status-vb badge badge-primary'>Kết thúc</span>";
            }
            else if (Status == OrderStatus.HuyDonHang.GetHashCode())
            {
                html = "<span class='status-vb badge badge-danger'>Hủy đơn hàng</span>";
            }

            return new MvcHtmlString(html);
        }

        public static List<DropDownList> DanhSachTrangThaiGioHang(int? Status)
        {
            DropDownList drop = new DropDownList();
            List<DropDownList> ds = new List<DropDownList>();
            ds.Add(new DropDownList { Text = "Thêm vào giỏ hàng", Value = OrderStatus.ThemVaoGioHang.GetHashCode() });
            ds.Add(new DropDownList { Text = "Hình thức Ship", Value = OrderStatus.HinhThucShip.GetHashCode() });
            ds.Add(new DropDownList { Text = "Thanh toán", Value = OrderStatus.ThanhToan.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đang xử lý", Value = OrderStatus.DangXuLy.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đang giao hàng", Value = OrderStatus.DangGiaoHang.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đã giao hàng", Value = OrderStatus.DaGiaoHang_ChuaXacNhan.GetHashCode() });
            ds.Add(new DropDownList { Text = "Đã giao hàng (Đã xác nhận)", Value = OrderStatus.DaGiaoHang_DaXacNhan.GetHashCode() });
            ds.Add(new DropDownList { Text = "Kết thúc", Value = OrderStatus.KetThuc.GetHashCode() });
            ds.Add(new DropDownList { Text = "Hủy đơn hàng", Value = OrderStatus.HuyDonHang.GetHashCode() });

            return ds;
        }

        public static MvcHtmlString OrderHistoryMemberName_UserName(this HtmlHelper helper, int? UserId, string MemberGuid)
        {
            string html = string.Empty;

            if (UserId.HasValue)
            {
                qtUserService _user = new qtUserService();
                qtUser user = _user.FindByKey(UserId);

                if (user != null)
                {
                    html = user.UserName;
                }
            }

            if (!string.IsNullOrEmpty(MemberGuid) || !string.IsNullOrWhiteSpace(MemberGuid))
            {
                shMemberService _member = new shMemberService();
                shMember member = _member.FindByKey(MemberGuid);

                if (member != null)
                {
                    html = member.MemberName;
                }
            }

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString SoLuong(this HtmlHelper helper, string ProductGuid, string SectionGuid, string SizeGuid, int? SoLuong)
        {
            string html = string.Empty;

            html = "<div class='input-group' style='display: inline-flex !important;'>"
                               + " <span  class='input-group-addon left-right remove' sizeguid='" + SizeGuid + "' onclick=giam('" + SizeGuid + "') data-toggle='tooltip' title='Giảm số lượng' data-original-title='Giảm số lượng' > -&nbsp; </span>"
                               + "<span class='soluong' sizeguid='" + SizeGuid + "'>" + SoLuong + "</span>"
                               + "<span class='input-group-addon left-right add' sizeguid='" + SizeGuid + "' onclick=tang('" + SizeGuid + "') data-toggle='tooltip'  title='Tăng số lượng' data-original-title='Tăng số lượng' > +&nbsp;&nbsp;</span>"
                               + "</div>";

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ThongTinDonhang(this HtmlHelper helper, string OrderGuid)
        {
            string html = string.Empty;

            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            if (order != null)
                html = "Thông tin đơn hàng: " + order.FullName + "/" + order.Phone + "/" + order.Email + "/" + order.Address;


            return new MvcHtmlString(html);

        }

        public static MvcHtmlString GiaTriDonhang(this HtmlHelper helper, string OrderGuid)
        {
            string html = string.Empty;

            shOrderService _order = new shOrderService();
            shOrder order = _order.FindByKey(OrderGuid);

            if (order != null)
                html = "Giá trị đơn hàng: " + Format.FormatDecimalToString(order.Total.GetValueOrDefault(0) - order.FeeShip.GetValueOrDefault(0));

            return new MvcHtmlString(html);

        }

    }
}
