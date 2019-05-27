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
    public static class AccountshipExtension
    {
        public static Account GetUserLogin(this IIdentity identity)
        {
            HttpSessionState session = HttpContext.Current.Session;
            int userid = 0;
            string username = string.Empty;
            string userlogin = string.Empty;
            string password = string.Empty;
            string address = string.Empty;
            string sex = string.Empty;
            string email = string.Empty;
            string tel = string.Empty;
            string phone = string.Empty;
            string notes = string.Empty;
            string unitid = string.Empty;
            string unitname = string.Empty;
            string departmentid = string.Empty;
            string departmentname = string.Empty;
            string positionid = string.Empty;
            string positionname = string.Empty;

            if (session != null)
            {
                if (session["_userid"] != null)
                {
                    userid = TypeHelper.ToInt32(session["_userid"].ToString());
                }
                else
                {
                    //FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect("~/LoginAdmin/Index");
                }

                if (session["_username"] != null)
                {
                    username = session["_username"].ToString();
                }
                if (session["_userlogin"] != null)
                {
                    userlogin = session["_userlogin"].ToString();
                }
                if (session["_password"] != null)
                {
                    password = session["_password"].ToString();

                }
                if (session["_address"] != null)
                {
                    address = session["_address"].ToString();
                }
                if (session["_sex"] != null)
                {
                    sex = session["_sex"].ToString();
                }
                if (session["_email"] != null)
                {
                    email = session["_email"].ToString();

                }
                if (session["_tel"] != null)
                {
                    tel = session["_tel"].ToString();
                }
                if (session["_phone"] != null)
                {
                    phone = session["_phone"].ToString();
                }
                if (session["_notes"] != null)
                {
                    notes = session["_notes"].ToString();
                }

                if (session["_unitid"] != null)
                {
                    unitid = (session["_unitid"].ToString());
                }
                if (session["_unitName"] != null)
                {
                    username = session["_unitName"].ToString();
                }
                if (session["_departmentid"] != null)
                {
                    departmentid = (session["_departmentid"].ToString());
                }
                if (session["_departmentname"] != null)
                {
                    departmentname = session["_departmentname"].ToString();
                }
                if (session["_positionid"] != null)
                {
                    positionid = (session["_positionid"].ToString());
                }
                if (session["_positionname"] != null)
                {
                    positionname = session["_positionname"].ToString();
                }

                return new Account(userid, username, userlogin, password,
                                    address, sex, email, tel, phone, notes,
                                    unitid, unitname, departmentid,
                                    departmentname, positionid, positionname);

            }
            else
            {
                //FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/LoginAdmin/Index", true);

                return default(Account);
            }
        }

        public static void SetUserLogin(this IIdentity identity, Account user)
        {
            HttpSessionState sessiion = HttpContext.Current.Session;
            sessiion.Add("_userid", user.Userid.ToString());
            sessiion.Add("_username", user.Username.ToString());
            sessiion.Add("_userlogin", user.Userlogin.ToString());
            sessiion.Add("_password", user.Password.ToString());
            sessiion.Add("_address", user.Address.ToString());
            sessiion.Add("_sex", user.Sex.ToString());
            sessiion.Add("_email", user.Email.ToString());
            sessiion.Add("_tel", user.Tel.ToString());
            sessiion.Add("_phone", user.Phone.ToString());
            sessiion.Add("_notes", user.Notes.ToString());
            sessiion.Add("_unitid", user.Unitid.ToString());
            sessiion.Add("_unitName", user.UnitName.ToString());
            sessiion.Add("_departmentid", user.Departmentid.ToString());
            sessiion.Add("_departmentname", user.Departmentname.ToString());
            sessiion.Add("_positionid", user.Positionid.ToString());
            sessiion.Add("_positionname", user.Positionname.ToString());

            sessiion.Timeout = 7200;
        }

    }
}
