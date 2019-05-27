using C.Core.Common;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace C.Core.Common
{
    public class CommonHelper
    {
        #region Upload 1 file 
        public static string UploadFile(HttpFileCollectionBase ImageFile, string MapPath)
        {
            string fileName = string.Empty;
            string forderUpload = string.Empty;

            string fileSavePath = string.Empty;
            if (ImageFile != null && ImageFile.Count > 0)
            {
                for (int i = 0; i < ImageFile.Count; i++)
                {
                    HttpPostedFileBase file = ImageFile[i];

                    if (file.ContentLength > 0)
                    {
                        string random = GuidUnique.getInstance().GenerateUnique();

                        fileName = System.IO.Path.GetFileName(file.FileName).ToLower().Replace(' ', '_');

                        forderUpload = MapPath + random + "_" + fileName;

                        file.SaveAs(forderUpload);

                        return random + "_" + fileName;
                    }
                }
            }

            return forderUpload;
        }

        public static string UploadFile1(HttpFileCollectionBase ImageFile, string MapPath)
        {
            string fileName = string.Empty;
            string forderUpload = string.Empty;

            string fileSavePath = string.Empty;
            if (ImageFile != null && ImageFile.Count > 0)
            {
                for (int i = 0; i < ImageFile.Count; i++)
                {
                    HttpPostedFileBase file = ImageFile[i];

                    if (file.ContentLength > 0)
                    {
                        string random = GuidUnique.getInstance().GenerateUnique();

                        fileName = random + System.IO.Path.GetFileName(file.FileName).ToLower().Replace(' ', '_');

                        forderUpload = Path.Combine(MapPath, fileName);

                        file.SaveAs(forderUpload);

                        return forderUpload + fileName;
                    }
                }
            }

            return forderUpload;
        }
        #endregion

        #region shCategory : Tìm page khi biết Category 
        public static int? FindPageCategory(string CategoryGuid, int? CategoryId)
        {
            try
            {
                shCategoryService _category = new shCategoryService();

                List<shCategory> dsCategory = _category.DanhSachCategory().ToList();

                int index = dsCategory.FindIndex(x => x.CategoryGuid == CategoryGuid);

                index = (int)index / Config.PAGE_SIZE_10;

                index++;

                return index;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public static int? FindPageProduct(string ProductGuid, int ProductId, int PageSize)
        {
            try
            {
                int page = 1;

                shProductService _product = new shProductService();

                List<shProduct> dsProduct = _product.DanhSachProduct().ToList();

                page = dsProduct.FindIndex(x => x.ProductGuid == ProductGuid);

                page = (int)page / PageSize;

                page++;

                return page;
            }
            catch (Exception)
            {
                return 1;
            }

        }

        public static int FindPageOrder(string OrderGuid, int? OrderStatus, int PageSize, int UserId)
        {
            try
            {
                shOrderService _order = new shOrderService();
                int index = _order.DanhSachOrder_ByStatus(null, OrderStatus, UserId).ToList().FindIndex(x => x.OrderGuid == OrderGuid);

                if (index > 0)
                {
                    index = index / PageSize + 1;
                    return index;
                }
            }
            catch
            {
                return 1;
            }
            return 1;
        }
        #endregion

        #region Check null or Empty
        public bool CheckNullOrEmpty(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Lấy link controller cấp cao nhất của danh mục chứa sản phẩm 
        public static string GetUrlTheFirst(string CategoryGuid)
        {
            shCategoryService _category = new shCategoryService();

            shCategory category = new shCategory();

            string controller = CategoryGuid;

            if (!string.IsNullOrWhiteSpace(CategoryGuid) || !string.IsNullOrEmpty(CategoryGuid))
            {
                category = _category.FindByKey(CategoryGuid);

                if (!string.IsNullOrWhiteSpace(category.ParentId) || !string.IsNullOrEmpty(category.ParentId))
                    controller = GetUrlTheFirst(category.ParentId);
                else
                    controller = category.CategoryGuid;
            }

            return controller;
        }
        #endregion

        #region Lọc danh sách giá tiền cao nhất - thấp nhất hiển thị trên trang người dùng
        public static string TinhToanGiaTienMaxMin(shSetSize first, shSetSize last)
        {
            string price = string.Empty;

            if (first != null)
            {
                if (first.PriceCurrent == null)
                    first.PriceCurrent = 0;
                price += Format.FormatDecimalToString(first.PriceCurrent.Value).Replace(',', '.');
            }

            return price;
        }
        public static string TinhToanGiaTienSauKhiGiam(shSetSize first, shSetSize last)
        {
            string price = string.Empty;

            decimal _price = default(decimal);

            if (first != null)
            {
                if (first.PriceCurrent == null)
                {
                    first.PriceCurrent = 0;
                }
                else
                {
                    shSaleService _sale = new shSaleService();
                    shSale sale = _sale.ChuongTrinhKhuyenMaiHienTai(DateTime.Now);

                    _price = _sale.TinhToanSoTienKhuyenMaiCuaSanPham(DateTime.Now, first.ProductGuid, first.SizeGuid);
                }
            }
            if (_price > 0)
            {
                return _price.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public static string TinhToanKichThuocMaxMin(shSetSize first, shSetSize last)
        {
            string size = string.Empty;

            if (first != null)
            {
                size += first.SizeName;
            }

            //if (last != null)
            //{
            //    size += " - " + last.SizeName;
            //}

            return size;
        }
        #endregion

        #region Danh mục hiển thi cha - con
        public static string LamDanhMucHienThiView(string CategoryGuid)
        {
            string html = string.Empty;

            shCategoryService _category = new shCategoryService();

            shCategory category = _category.FindByKey(CategoryGuid);

            while (!string.IsNullOrWhiteSpace(category.ParentId) || !string.IsNullOrEmpty(category.ParentId))
            {
                category = _category.FindByKey(category.ParentId);

                //html += StringHelper.ConvertToTitleCase(category.CategoryName) + " » ";
                html += category.CategoryName + " » ";
            }

            return html;
        }

        #endregion

        #region GetUserTheoMaCauHinhHeThong
        public static IEnumerable<qtUser> GetUserTheoMaCauHinhHeThong_GetTheoUser(string app_code, int? UnitId)
        {
            qtUserService _userService = new qtUserService();

            qtUser user = new qtUser();

            qtUserConfigService _userConfigService = new qtUserConfigService();

            IEnumerable<qtUserConfig> danhSachCauhinhUserTheoDonVi = _userConfigService.FindList().Where(x => x.IsActive == true && x.AppCode == app_code);

            if (UnitId.HasValue)
            {
                danhSachCauhinhUserTheoDonVi = danhSachCauhinhUserTheoDonVi.Where(x => x.UnitId == UnitId);
            }

            qtUserConfigDetailService _userConfigDetailService = new qtUserConfigDetailService();

            List<qtUserConfigDetail> danhSachUserCauHinhTheoDonVi_LanhDao = new List<qtUserConfigDetail>();

            foreach (var userConfig in danhSachCauhinhUserTheoDonVi)
            {
                IEnumerable<qtUserConfigDetail> danhSachUserMotDonVi = _userConfigDetailService.FindList().Where(x => x.UserConfigId == userConfig.UserConfigId);

                danhSachUserCauHinhTheoDonVi_LanhDao.AddRange(danhSachUserMotDonVi);
            }

            danhSachUserCauHinhTheoDonVi_LanhDao = danhSachUserCauHinhTheoDonVi_LanhDao.OrderBy(x => x.OrderBy).ToList();

            List<qtUser> danhSanhUser_La_LanhDao = new List<qtUser>();
            foreach (var userConfigDetail in danhSachUserCauHinhTheoDonVi_LanhDao)
            {
                user = _userService.FindByKey(userConfigDetail.UserId);

                if (user != null)
                {
                    if (user.Status.GetValueOrDefault(false))
                    {
                        yield return user;
                    }
                }
            }
        }

        public static IEnumerable<int?> GetUserTheoMaCauHinhHeThong_GetTheoUserId(string app_code, int? UnitId)
        {
            qtUserService _userService = new qtUserService();

            qtUser user = new qtUser();

            qtUserConfigService _userConfigService = new qtUserConfigService();

            IEnumerable<qtUserConfig> danhSachCauhinhUserTheoDonVi = _userConfigService.FindList().Where(x => x.IsActive == true && x.AppCode == app_code);

            if (UnitId.HasValue)
            {
                danhSachCauhinhUserTheoDonVi = danhSachCauhinhUserTheoDonVi.Where(x => x.UnitId == UnitId);
            }

            qtUserConfigDetailService _userConfigDetailService = new qtUserConfigDetailService();

            List<qtUserConfigDetail> danhSachUserCauHinhTheoDonVi_LanhDao = new List<qtUserConfigDetail>();

            foreach (var userConfig in danhSachCauhinhUserTheoDonVi)
            {
                IEnumerable<qtUserConfigDetail> danhSachUserMotDonVi = _userConfigDetailService.FindList().Where(x => x.UserConfigId == userConfig.UserConfigId);

                danhSachUserCauHinhTheoDonVi_LanhDao.AddRange(danhSachUserMotDonVi);
            }

            danhSachUserCauHinhTheoDonVi_LanhDao = danhSachUserCauHinhTheoDonVi_LanhDao.OrderBy(x => x.OrderBy).ToList();

            return danhSachUserCauHinhTheoDonVi_LanhDao.Select(x => x.UserId);
        }

        /// <summary>
        /// Kiểm tra tài khoản đăng nhập có phải tài khoản lãnh đạo hệ thống hay không. Lãnh đạo hệ thống sẽ hiển thị tất cả,
        /// nhân viên còn lại sẽ hiển thị theo User nhân viên đó
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="MaCauHinh"></param>
        /// <returns></returns>
        public static int? KiemTraTaiKhoanCoPhaiLanhDaoDonVi(int? UserId, string MaCauHinh)
        {
            IEnumerable<int?> dsUserId = CommonHelper.GetUserTheoMaCauHinhHeThong_GetTheoUserId(MaCauHinh, Units.ChiNhanhShowRoom.GetHashCode());

            if (dsUserId != null && dsUserId.Count() > 0)
            {
                if (dsUserId.Contains(UserId))
                {
                    return null; // return true // lấy tất cả danh sách
                }
            }

            return UserId; // return false   // lấy theo ds userid của nhân viên
        }
        #endregion
    }
}
