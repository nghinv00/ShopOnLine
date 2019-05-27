using C.Core.Common;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shMemberService : BaseService<shMember, ShopOnlineDb>
    {

        #region Member Login
        public shMember GetMemberLogin(string MemberLogin, string Password)
        {
            shMemberService _Member = new shMemberService();

            var check = _Member.FindList().Where(x => x.MemberLogin == MemberLogin && x.Password == Password).FirstOrDefault();

            return check;
        }

        public bool CheckMemberLogin(string MemberLogin, string Password)
        {
            shMemberService _Member = new shMemberService();

            var check = _Member.FindList().Where(x => x.MemberLogin == MemberLogin && x.Password == Password).Count();

            if (check > 0)
                return true;

            return false;
        }

        public shMember GetMember(string MemberLogin, string Password)
        {
            shMemberService _Member = new shMemberService();

            shMember Member = null;

            var check = _Member.FindList().Where(x => x.MemberLogin == MemberLogin && x.Password == Password);

            if (check.Count() > 0)
            {
                Member = check.FirstOrDefault();
            }

            return Member;
        }
        #endregion

        public string MemberName(int? MemberId)
        {
            if (MemberId.HasValue)
            {
                shMemberService _member = new shMemberService();
                shMember member = _member.FindList().Where(x => x.MemberId == MemberId).FirstOrDefault();

                if (member != null)
                    return member.MemberName;
                return string.Empty;
            }
            return string.Empty;
        }

        public string MemberName(string MemberGuid)
        {
            shMemberService _member = new shMemberService();
            shMember member = _member.FindByKey(MemberGuid);

            if (member != null)
                return member.MemberName;

            return string.Empty;

        }

        public shMember Member(string MemberGuid)
        {
            shMemberService _member = new shMemberService();
            shMember member = _member.FindByKey(MemberGuid);

            if (member != null)
                return member;

            return new shMember();
        }

        #region GetListMember
        public IEnumerable<shMember> DanhSachMember()
        {
            shMemberService _member = new shMemberService();

            IEnumerable<shMember> dsMember = _member.FindList();

            return dsMember;
        }

        public IPagedList<shMember> DanhSachMember_PhanTrang(int page, int pageSize)
        {
            IPagedList<shMember> pageList_dsMember = DanhSachMember().ToPagedList(page, pageSize);

            return pageList_dsMember;
        }
        #endregion

        #region Thêm mới - Hiệu chỉnh Member 
        public shMember ThemMoi_HieuChinhMember(
            string MemberGuid, int? MemberId, string MemberName, string MemberLogin, string Password,
            string ImageFile, string Address, int? Sex, string Email, string Tel,
            string Phone, string Notes, bool? Status, DateTime? CreatedDate, string MapPath, string urlFolder)
        {
            shMemberService _member = new shMemberService();
            shMember member = new shMember();

            if (!string.IsNullOrWhiteSpace(MemberGuid))
            {
                member = _member.FindByKey(MemberGuid);
                if (member.Password != Password)
                {
                    member.Password = EncryptUtil.EncryptMD5(Password);
                }
            }
            else
            {
                member.Password = EncryptUtil.EncryptMD5(Password);
                MemberGuid = GuidUnique.getInstance().GenerateUnique();
            }

            member.MemberGuiId = MemberGuid;
            member.MemberName = MemberName;
            member.MemberLogin = MemberLogin;

            //string forderUpload = CommonHelper.UploadFile(ImageFile, MapPath);
            //if (forderUpload != "" && forderUpload != null)
            //{
            //    member.ImageFile = forderUpload;
            //}

            member.ImageFile = ImageFile;
            member.Address = Address;
            member.Sex = Sex;
            member.Email = Email;
            member.Tel = Tel;
            member.Phone = Phone;
            member.Notes = Notes;
            member.Status = Status;
            member.CreateDate = CreatedDate;

            if (member.MemberId > 0)
                _member.Update(member);
            else
                _member.Insert(member);

            return member;
        }
        #endregion

        #region DS Member đăng ký mới
        public int DanhSachMemberDangKyMoi()
        {
            IEnumerable<shMember> ds = DanhSachMember().Where(x => x.LaDaXem == false);
            return ds.Count();
        }
        #endregion
    }
}
