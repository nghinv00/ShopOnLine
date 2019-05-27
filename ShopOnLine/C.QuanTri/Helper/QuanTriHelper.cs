using C.Core.Model;
using C.Core.Service;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace C.QuanTri.Helper
{
    public static class QuanTriHelper
    {

        #region Menu left
        public static MvcHtmlString MenuLeft(this HtmlHelper helper, string app)
        {
            return GetTreeSiteMapMenuLeft(helper, app, SiteMap.RootNode);
        }

        private static MvcHtmlString GetTreeSiteMapMenuLeft(HtmlHelper helper, string app, SiteMapNode node)
        {
            return GetTreeSiteMapMenuLeft(helper, app, node, true);
        }

        private static MvcHtmlString GetTreeSiteMapMenuLeft(HtmlHelper helper, string app, SiteMapNode node, bool isRoot)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class='sidebar-menu' id='menu-lte'>");
            foreach (SiteMapNode childNode in node.ChildNodes)
            {
                if (CheckAccess(helper, childNode, app))
                {
                    string menuchild = SiteMapMenuChildLeft(helper, childNode, app);
                    string link = GetUrlSiteMapMenuChild(helper, childNode, app, false);
                    if (childNode.ChildNodes.Count > 0 && menuchild != "")
                        link = "#";
                    sb.AppendFormat("<li index='{0}' class='treeview'>", childNode["index"]);
                    if (childNode["icon"] != null && childNode["icon"] != "")
                    {
                        sb.AppendFormat("<a href='{0}'>{1}<span>{2}</span>{3}</a>", link, "<i class='" + childNode["icon"] + "'> </i>", helper.Encode(childNode.Title), "<span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span>");
                    }
                    else
                    {
                        sb.AppendFormat("<a href='{0}'>{1}<span>{2}</span>{3}</a>", link, "<i class='fa fa-arrow-circle-right'> </i>", helper.Encode(childNode.Title), "<span class='pull-right-container'><i class='fa fa-angle-left pull-right'></i></span>");
                    }
                    sb.Append(menuchild);
                    sb.AppendLine("</li>");
                }
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

        private static string SiteMapMenuChildLeft(HtmlHelper helper, SiteMapNode node, string appcode)
        {
            if (node.ChildNodes.Count > 0)
            {
                bool check = false;
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class='treeview-menu'>");
                foreach (SiteMapNode childNode in node.ChildNodes)
                {
                    if (CheckAccess(helper, childNode, appcode))
                    {
                        string target = string.Empty;
                        if (childNode["target"] != null && childNode["target"] != "")
                            target = "target= _blank";
                        if (childNode["number"] != null && childNode["number"] != "")
                        {
                            sb.AppendFormat("<li index='{0}'>", childNode["index"]);
                            if (childNode["icon"] != null && childNode["icon"] != "")
                            {
                                sb.AppendFormat("<a href='{0}' " + target + " ><i class='" + childNode["icon"] + "'> </i>  {1} <span class='number'></span></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            else
                            {
                                sb.AppendFormat("<a href='{0}' " + target + " ><i class='fa fa-caret-right'> </i>  {1} <span class='number'></span></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            sb.AppendLine("</li>");
                            check = true;
                        }
                        else
                        {
                            sb.AppendFormat("<li index='{0}'>", childNode["index"]);
                            if (childNode["icon"] != null && childNode["icon"] != "")
                            {
                                sb.AppendFormat("<a href='{0}'  " + target + " ><i class='" + childNode["icon"] + "'> </i>  {1}</a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            else
                            {
                                sb.AppendFormat("<a href='{0}'  " + target + " ><i class='fa fa-caret-right'> </i>  {1}</a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            sb.AppendLine("</li>");
                            check = true;
                        }
                    }
                }
                sb.AppendLine("</ul>");
                if (check)
                    return sb.ToString();
                else
                    return "";
            }

            return "";
        }
        #endregion

        #region Check role access
        private static bool CheckAccess(HtmlHelper helper, SiteMapNode node, string appcode)
        {
            if (node.Roles.Count == 0)
            {
                return true;
            }
            for (int i = 0; i < node.Roles.Count; i++)
            {
                if (node.Roles[i] != null)
                {
                    string role = node.Roles[i].ToString();
                    HttpSessionState session = HttpContext.Current.Session;
                    if (session["_departmentid"] != null && session["_positionid"] != null && session["_unitid"] != null)
                    {
                        qtRoleService _role = new qtRoleService();
                        if (_role.CheckUserInRole(TypeHelper.ToInt32(session["_unitid"].ToString())
                            , TypeHelper.ToInt32(session["_departmentid"].ToString())
                            , TypeHelper.ToInt32(session["_positionid"].ToString())
                            , appcode
                            , role))
                            return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region GetUrlSiteMapMenuChild
        private static string GetUrlSiteMapMenuChild(HtmlHelper helper, SiteMapNode node, string appcode, bool isRoot)
        {
            foreach (SiteMapNode childNode in node.ChildNodes)
            {
                if (CheckAccess(helper, childNode, appcode))
                {
                    return childNode.Url.ToString();
                }
            }
            return node.Url.ToString();
        }
        #endregion

        #region GetFirstPermissionUrlByUser
        public static string GetFirstPermissionUrlByUser(qtUser user, string appcode)
        {
            foreach (SiteMapNode node in SiteMap.RootNode.ChildNodes)
            {
                foreach (SiteMapNode childNode in node.ChildNodes)
                {
                    if (CheckAccessByUser(childNode, appcode, user))
                    {
                        return childNode.Url;
                    }
                }
            }
            return string.Empty;
        }
        #endregion

        #region CheckAccessByUser
        public static bool CheckAccessByUser(SiteMapNode node, string appcode, qtUser user)
        {
            if (user != null)
            {
                if (node.Roles.Count == 0)
                {
                    return true;
                }
                for (int i = 0; i < node.Roles.Count; i++)
                {
                    if (node.Roles[i] != null)
                    {
                        string role = node.Roles[i].ToString();
                        qtRoleService _role = new qtRoleService();
                        return _role.CheckUserInRole(user.UnitId.Value, user.DepartmentId.Value, user.PositionId.Value, appcode, role);
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
