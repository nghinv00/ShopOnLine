using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Core.Common
{
    public class Config
    {
        public const string Application = "QTHT";

        public static readonly int PAGE_SIZE_10 = 10;
        public static readonly int PAGE_SIZE_20 = 20;
        public static readonly int PAGE_SIZE_8 = 8;
        public static readonly int PAGE_SIZE_2 = 2;
        public static readonly int PAGE_SIZE_3 = 3;
        public static readonly int PAGE_SIZE_9 = 9;

        #region Lưu hình ảnh 
        public const string URL_UPLOAD = "/images/uploadfiletemp/";
        public const string URL_UPLOAD_BANNER = "/images/banner/";
        public const string URL_UPLOAD_CATEGORY = "/images/category/";
        public const string URL_UPLOAD_MEMBER = "/images/member/";
        public const string URL_UPLOAD_NEW = "/images/new/";
        public const string URL_UPLOAD_PRODUCT = "/images/product/";
        public const string URL_UPLOAD_SALE = "/images/sale/";
        public const string thumbs = "thumbs";
        public const string URL_UPLOAD_HIGHLIGHT = "/images/weblink/";

        //Thể loại ảnh upload: ProductImageCategory
        public const string ProductImageCategory_Design = "MauMa";
        public const string ProductImageCategory_Material = "ChatLieu";

        public const string URL_IMAGE_DEFAULT = "/content/images/background.jpg";

        public const string DesignImage = "DesignImage";   // mẫu mã sản phẩm
        public const string MaterialImage = "MaterialImage"; // Chất liệu sản phẩm 
        #endregion

        public const string MetaTitle = "trang-chu";

        #region Order
        public const decimal FeeShip = 50000;
        public const decimal FeeTotal = 2000000;
        #endregion

        #region ProductImage : Cấu hình thể loại ảnh hiển thị trên trang chủ: Ảnh banner, ảnh icon, ảnh sản phẩm nổi bật, ..
        public const string Product_Image_Banner = "Banner";
        public const string Product_Image_HighLight = "HighLight";
        #endregion

        #region CategoryImage
        public const string Category_Image_Icon = "Icon";
        public const string Category_Image_Icon_Active = "IconActive";
        public const string Category_Image_Default = "Default";

        #endregion

        #region Điều kiện áp dụng khuyến mại
        public const string DieuKienApDungKhuyenMai_TatCaSanPham = "TatCa";
        public const string DieuKienApDungKhuyenMai_TheoDanhMuc = "DanhMuc";
        public const string DieuKienApDungKhuyenMai_TheoSanPham = "SanPham ";
        public const string DieuKienApDungKhuyenMai_MaCauHinh_TatCaSanPham = "MaCauHinh_TatCaSanPham ";
        #endregion

        #region Báo cáo
        public const int Top_10 = 10;
        public const string DoanhThu = "DoanhThu";
        public const string SoLuong = "SoLuong";
        #endregion

        #region Mã Cấu hình trong hệ thống
        public const string LANH_DAO_DON_VI = "LANH_DAO_DON_VI";
        public const string TAI_KHOAN_EMAIL_THONG_BAO_TIEP_NHAN_DON_HANG = "TAI_KHOAN_EMAIL_THONG_BAO_TIEP_NHAN_DON_HANG";
        public const string TAI_KHOAN_NHAN_DON_HANG = "TAI_KHOAN_NHAN_DON_HANG";

        #endregion
        
        #region Mã thông báo trong hệ thống
        public const string THONG_BAO_DON_HANG_MOI = "THONG_BAO_DON_HANG_MOI"; // khách hàng gửi notify cho admin
        public const string THONG_BAO_DON_HANG_XU_LY = "THONG_BAO_DON_HANG"; // admin chuyển cho nhân viên xử lý 
        public const string THONG_BAO_DA_XU_LY_DON_HANG = "THONG_BAO_DA_XU_LY_DON_HANG";  // nhân viên đã xử lý đơn hàng và trong qt chuyển tới KH

        #endregion

    }

    #region Cấu hình đơn vị trong hệ thông
    public enum Units
    {
        ChiNhanhShowRoom = 2,

    }
    #endregion


    #region OrderStatus - Trạng thái giao hàng
    public enum OrderStatus
    {
        ThemVaoGioHang = 1, // cshtml
        HinhThucShip = 2,// cshtml
        ThanhToan = 3,// cshtml
        DangXuLy = 4, // backend
        DangGiaoHang = 5,// backend
        DaGiaoHang_ChuaXacNhan = 6,// backend
        DaGiaoHang_DaXacNhan = 7,// backend
        KetThuc = 8,// backend
        HuyDonHang = 9,// backend
    }
    #endregion

    #region Hình thức giao hàng
    public enum PayType
    {
        ThuPhiTanNoi = 1,
        ChuyenKhoanNganHang = 2,
        ShowRoomDaiLy = 3
    }
    #endregion

    #region Vị trí banner 
    public enum PositionBanner
    {
        Position_GioiThieu = 1,
        Position_SanPham = 2,
        Position_DaiLy = 3,
        Position_BangGia = 4,
        //Position_DaiLy = 5,
        Position_TinTucSuKien = 6,
        Position_LienLac = 7
    }
    #endregion

    #region DropDownList
    public class DropDownList
    {
        public string Text { get; set; }
        public int? Value { get; set; }
    }

    public class DropDownList_String
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    #endregion


    #region Nhâp xuất hàng hóa lưu kho
    public enum PhieuNhapXuat
    {
        Nhap = 1,
        Xuat = 2
    }

    public enum TrangThaiPhieuNhapXuat
    {
        Moi = 1,
        DangXuLy = 2,
        HoanThanh = 3,
        Huy = 4
    }

    public enum LoaiPhieuNhapXuat
    {
        MuaHang = 1,
        NhapTra = 2,
        Khacnhap = 3,

        BanLe = 4,
        BanSi = 5,
        XuatTra = 6,
        KhacXuat = 7,
    }
    #endregion

    #region Khuyến mại
    public enum CachTinhGiaTriKhuyenMai
    {
        GiamTheoPhanTramGiaTri = 1,
        GiamTheoSoTien = 2,
        BanGiaCodinh = 3,
        GiamTheoKhoangGiaTri = 4
    }

    public enum TrangThaiChuongTrinhKhuyenMai
    {
        ChoXuLy = 1,
        DangChay = 2,
        TamDung = 3,
        KetThuc = 4,
        GoBo = 5
    }

    public enum DieuKienApDungKhuyenMai
    {
        TatCaSanPham = 1,
        TheoDanhMuc = 2,
        TheoSanPhamRiengBiet = 3
    }
    #endregion

    #region Bảng tìm kiếm
    public enum TheLoaiTimKiemNangCao
    {
        SanPham = 1,
        TinTuc = 2,
        SetSanPham = 3,
        KichThuocSanPham = 4
    }
    public class TimKiemNangCao
    {
        public int CategoryId { get; set; } // 1. tin tức // 2 sản phẩm
        public string Category { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string keyword { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public string ProductGuid { get; set; }

    }

    #endregion
}
