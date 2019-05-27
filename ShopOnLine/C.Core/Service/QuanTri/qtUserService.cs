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
    public class qtUserService : BaseService<qtUser, ShopOnlineDb>
    {
        public string GetFileImage(int? UserId)
        {
            string html = "/AdminLTE/dist/img/avatar5.png";
            if (UserId.HasValue)
            {
                qtUserService _user = new qtUserService();
                qtUser user = _user.FindByKey(UserId);
                if (user != null)
                    if (user.ImageFile != null && user.ImageFile != "")
                        html = user.ImageFile.Replace("~", "");
            }
            return html;
        }

        public string UserName(int? UserId)
        {
            string UserName = string.Empty;
            if (UserId.HasValue)
            {
                qtUserService _user = new qtUserService();
                qtUser user = _user.FindByKey(UserId);
                if (user != null)
                    return user.UserName;
                return UserName;
            }
            return UserName;
        }

        #region User Login
        public qtUser GetUserLogin(string UserLogin, string Password)
        {
            qtUserService _user = new qtUserService();
            var check = _user.FindList().Where(x => x.UserLogin == UserLogin && x.Password == Password).FirstOrDefault();
            return check;
        }

        public bool CheckUserLogin(string UserLogin, string Password)
        {
            qtUserService _user = new qtUserService();
            var check = _user.FindList().Where(x => x.UserLogin == UserLogin && x.Password == Password).Count();
            if (check > 0)
                return true;
            return false;
        }

        public qtUser GetUser(string UserLogin, string Password)
        {
            qtUserService _user = new qtUserService();
            qtUser user = null;
            var check = _user.FindList().Where(x => x.UserLogin == UserLogin && x.Password == Password);
            if (check.Count() > 0)
            {
                user = check.FirstOrDefault();
            }
            return user;
        }
        #endregion

        #region GetListUser 
        public IEnumerable<qtUser> DanhSachUser(string TenNguoiDung, int? UnitId, int? DepartmentId)
        {
            qtUserService _user = new qtUserService();

            IEnumerable<qtUser> dsUser = _user.FindList()
                                                    .Where(x => (string.IsNullOrWhiteSpace(TenNguoiDung) ||
                                                                 TypeHelper.CompareString(x.UserName, TenNguoiDung))
                                                          );

            if (UnitId.HasValue)
            {
                dsUser = dsUser.Where(x => x.UnitId == UnitId);
            }

            if (DepartmentId.HasValue)
            {
                dsUser = dsUser.Where(x => x.DepartmentId == DepartmentId);
            }

            return dsUser;
        }

        public IPagedList<qtUser> DanhSachUser_PhanTrang(string TenNguoiDung, int? UnitId, int? DepartmentId, int page, int pageSize)
        {
            IPagedList<qtUser> pageList_dsUser = DanhSachUser(TenNguoiDung, UnitId, DepartmentId).ToPagedList(page, pageSize);

            return pageList_dsUser;
        }
        #endregion

        #region Insert , Update qtUsers
        public qtUser ThemMoi_HieuChinhThongTinUser(int UserId, string UserName, string UserLogin, string Password, int? SortOrder,
            string ImageFile, string Address, int? Sex, string Email, string Tel, string Phone, bool? IsAdmin,
            string Notes, int? UnitId, int? DepartmentId, int? PositionId, bool? Status, DateTime? CreateDate)
        {
            qtUserService _user = new qtUserService();
            qtUser user = new qtUser();
            if (UserId > 0)
            { // Update
                user = _user.FindByKey(UserId);

                if (user.Password != Password)
                {
                    user.Password = EncryptUtil.EncryptMD5(Password);
                }
            }
            else
            { // Insert 
                user.Password = EncryptUtil.EncryptMD5(Password);
            }

            user.UserName = UserName;
            user.UserLogin = UserLogin;
            user.SortOrder = SortOrder;
            //user.ImageFile
            //user.Address
            //user.Sex
            user.Email = Email;
            user.Tel = Tel;
            //user.Phone
            user.IsAdmin = IsAdmin;
            //user.Notes
            user.UnitId = UnitId;
            user.DepartmentId = DepartmentId;
            user.PositionId = PositionId;
            user.Status = Status;
            user.CreatedDate = DateTime.Now;

            if (user.UserId > 0)
            {
                _user.Update(user);
            }
            else
            {
                _user.Insert(user);
            }
            return user;

        }
        #endregion
    }
}
