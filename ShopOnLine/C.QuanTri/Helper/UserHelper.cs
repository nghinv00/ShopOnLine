using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace C.QuanTri.Helper
{
    public static class UserHelper
    {
        public static MvcHtmlString GetFileImage(this HtmlHelper helper, int UserId)
        {
            qtUserService _user = new qtUserService();

            return new MvcHtmlString(_user.GetFileImage(UserId));

        }

        public static void SaveFileImage(int userid, HttpFileCollectionBase FileDinhKem)
        {
            qtUserService _user = new qtUserService();
            qtUser user = new qtUser();
            if (userid > 0)
                user = _user.FindByKey(userid);
            if (FileDinhKem.Count >= 1)
            {
                HttpPostedFileBase file = FileDinhKem[0];
                if (file.FileName != null && file.FileName != "")
                {
                    string ramdom = Guid.NewGuid().ToString();
                    string fordelUpload = HttpContext.Current.Server.MapPath("~/AttachFile/") + ramdom + "_" + file.FileName;
                    file.SaveAs(fordelUpload);
                    user.ImageFile = "~/AttachFile/" + ramdom + "_" + file.FileName;
                    if (userid > 0) _user.Update(user);
                    else _user.Insert(user);
                }
            }
        }

        public static MvcHtmlString UserName(this HtmlHelper helper, int? UserId)
        {
            qtUserService _user = new qtUserService();

            return new MvcHtmlString(_user.UserName(UserId));
        }

        public static MvcHtmlString NhanVienXuLyChinh(this HtmlHelper helper, int? UserId)
        {
            qtUserService _user = new qtUserService();
            string UserName = _user.UserName(UserId);

            UserName = "<span style='font-weight: 600;'>" + UserName + "</span>";

            return new MvcHtmlString(UserName);
        }

        public static MvcHtmlString NhanVienXuLyDanhMuChinh(this HtmlHelper helper, shCategory category)
        {
            qtUserService _user = new qtUserService();
            string UserName = string.Empty;

            if (!string.IsNullOrEmpty(category.ParentId) || !string.IsNullOrWhiteSpace(category.ParentId))
            {
                shCategoryService _category = new shCategoryService();
                shCategory parent = _category.FindByKey(category.ParentId);
                if (parent != null)
                {
                    UserName = "<span data-toggle='tooltip' title='Theo dõi cấp cha' data-original-title='Theo dõi cấp cha' >" + _user.UserName(parent.UserId) + " » </span>";
                }
            }

            UserName += "<span data-toggle='tooltip' title='Theo dõi chính' data-original-title='Theo dõi chính' style='font-weight: 600; color: #72afd2;'>" + _user.UserName(category.UserId) + "</span>";

            return new MvcHtmlString(UserName);
        }
    }
}
