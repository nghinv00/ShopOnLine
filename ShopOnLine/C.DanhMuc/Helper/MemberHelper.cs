using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace C.DanhMuc.Helper
{
    public static class MemberHelper
    {
        public static MvcHtmlString MemberName(this HtmlHelper helper, string MemberGuid)
        {
            shMemberService _member = new shMemberService();

            string html = String.Empty;

            shMember member = _member.Member(MemberGuid);

            if (member != null)
            {
                html = "<a href='/DanhMuc/Order/Index/" + member.MemberGuiId + "' target='_blank'>" + member.MemberName + "</a>";
            }

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString MemberName_Member(this HtmlHelper helper, string MemberGuid)
        {
            shMemberService _member = new shMemberService();

            string html = String.Empty;

            shMember member = _member.Member(MemberGuid);

            if (member != null)
            {
                html = "<a href='/DanhMuc/Member/Create/" + member.MemberGuiId + "' target='_blank'>" + member.MemberName + "</a>";
            }

            return new MvcHtmlString(html);
        }

    }
}
