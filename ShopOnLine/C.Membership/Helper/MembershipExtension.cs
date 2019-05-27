using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace C.Membership.Helper
{
    public static class MembershipExtension
    {
        public static Member GetMemberLogin(this IIdentity identity)
        {
            HttpSessionState session = HttpContext.Current.Session;

            string _MemberGuid = string.Empty;
            int _MemberId = 0;
            string _MemberName = string.Empty;
            string _MemberLogin = string.Empty;
            string _Password = string.Empty;
            string _ImageFile = string.Empty;
            string _Address = string.Empty;
            int _Sex = 0;
            string _Email = string.Empty;
            string _Tel = string.Empty;
            DateTime _BirthDay = DateTime.Now;
            string _Phone = string.Empty;
            string _Notes = string.Empty;

            if (session != null)
            {
                if (session["MemberGuid"] != null)
                {
                    _MemberGuid = session["MemberGuid"].ToString();
                }
                else
                {
                    //HttpContext.Current.Response.Redirect("~/Login/Index", true);
                }

                if (session["MemberId"] != null)
                {
                    _MemberId = TypeHelper.ToInt32(session["MemberId"].ToString());
                }
                else
                {
                    //HttpContext.Current.Response.Redirect("~/Login/Index", true);
                }
                if (session["MemberName"] != null)
                {
                    _MemberName = session["MemberName"].ToString();
                }

                if (session["MemberLogin"] != null)
                {
                    _MemberLogin = session["MemberLogin"].ToString();
                }

                if (session["Password"] != null)
                {
                    _Password = session["Password"].ToString();
                }

                if (session["ImageFile"] != null)
                {
                    _ImageFile = session["ImageFile"].ToString();
                }

                if (session["Address"] != null)
                {
                    _Address = session["Address"].ToString();
                }

                if (session["Sex"] != null)
                {
                    _Sex = TypeHelper.ToInt32(session["Sex"].ToString());
                }

                if (session["Email"] != null)
                {
                    _Email = session["Email"].ToString();
                }



                if (session["Tel"] != null)
                {
                    _Tel = session["Tel"].ToString();
                }

                if (session["BirthDay"] != null)
                {
                    _BirthDay = TypeHelper.ToDate(session["BirthDay"].ToString());
                }

                if (session["Phone"] != null)
                {
                    _Phone = session["Phone"].ToString();
                }

                if (session["Notes"] != null)
                {
                    _Notes = session["Notes"].ToString();
                }

                return new Member(
                    _MemberGuid,
                    _MemberId,
                    _MemberName,
                    _MemberLogin,
                    _Password,
                    _ImageFile,
                    _Address,
                    _Sex,
                    _Email,
                    _Tel,
                    _BirthDay,
                    _Phone,
                    _Notes);
            }
            else
            {
                //FormsAuthentication.SignOut();
                //HttpContext.Current.Response.Redirect("~/Login/Index", true);

                return default(Member);
            }
        }

        public static void SetMemberLogin(this IIdentity identity, Member member)
        {
            HttpSessionState sessiion = HttpContext.Current.Session;
            sessiion.Add("MemberGuid", member.MemberGuid);
            sessiion.Add("MemberId", member.MemberId);
            sessiion.Add("MemberName", member.MemberName);
            sessiion.Add("MemberLogin", member.MemberLogin);
            sessiion.Add("Password", member.Password);
            sessiion.Add("ImageFile", member.ImageFile);
            sessiion.Add("Address", member);
            sessiion.Add("Sex", member.Sex);
            sessiion.Add("Email", member.Email);
            sessiion.Add("Tel", member.Tel);
            sessiion.Add("BirthDay", member.BirthDay.ToString("dd/MM/yyyy"));
            sessiion.Add("Phone", member.Phone);
            sessiion.Add("Notes", member.Notes);

            sessiion.Timeout = 7200;
        }
    }
}
