using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using System.Reflection;
using System.Text;
using MvcContrib.UI.Grid.Syntax;
using C.UI.Helpers;
using C.UI.PagedList;
namespace C.UI.Helpers
{
    public enum OrderField
    {
        OrderDes
    }

    public static class HtmlHelperExtension
    {
  
        #region Button
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText)
        {
            return Button(htmlHelper, buttonText, "btn btn-default");
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className)
        {
            return new MvcHtmlString(string.Format("<input type=\"button\" value=\"{0}\" class=\"{1}\"/>", buttonText, className));
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className, string buttonId)
        {
            return new MvcHtmlString(string.Format("<input type=\"button\" value=\"{0}\" id=\"{1}\" class=\"{2}\"/>", buttonText, buttonId, className));
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className, string buttonId, string icon)
        {
            return new MvcHtmlString(string.Format("<div  id=\"{1}\" class=\"{2}\"><i class=\"{3}\"></i> {0}</div>", buttonText, buttonId, className, icon));
        }

        #endregion button

        public static MvcHtmlString ResolveUrl(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return MvcHtmlString.Create(urlHelper.Content(url));
        }
        #region html checkbox extension
        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name)
        //{
        //    var attributes = new Dictionary<string, string>();
        //    attributes.Add("class", "minimal");
        //    return htmlHelper.CheckBox(name);
        //}

        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, bool isDisabled)
        {
            var attributes = new Dictionary<string, string>();
            attributes.Add("class", "minimal");
            if (isDisabled)
            {
                attributes.Add("disabled", "disabled");
            }
            return htmlHelper.CheckBox(name, isChecked);
        }
        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, bool isDisabled, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("class", "minimal");
            if (isDisabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }
            return htmlHelper.CheckBox(name, isChecked, htmlAttributes);
        }
        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, bool isDisabled, object htmlAttributes)
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("class", "minimal");
            if (isDisabled)
            {

                attributes.Add("disabled", "disabled");
                int s = htmlAttributes.GetType().GetProperties().Count();
                foreach (PropertyInfo property in htmlAttributes.GetType().GetProperties())
                {
                    object propertyValue = property.GetValue(htmlAttributes, null);
                    attributes.Add(property.Name, propertyValue);
                }
                return htmlHelper.CheckBox(name, isChecked, attributes);
            }
            else
            {
                return htmlHelper.CheckBox(name, isChecked, htmlAttributes);
            }
        }
        public static MvcHtmlString htmlCheckBox(this HtmlHelper htmlHelper, string name)
        {
            string html = "<input type='checkbox' class='minimal' name=" + name + " id=" + name + ">";

            return new MvcHtmlString(html);
        }
        public static MvcHtmlString htmlCheckBox(this HtmlHelper htmlHelper, string name, object val)
        {
            string html = "<input type='checkbox' class='minimal' name=" + name + " id=" + "''" + " value=" + val + ">";

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString htmlCheckBox_DataToggle(this HtmlHelper htmlHelper, string name, object val, string datatoggle)
        {
            string html = "<input type='checkbox' class='minimal' name=" + name + " id=" + "''" + " value=" + val + " data-toggle='tooltip' title='" + datatoggle + "' data-original-title='" + datatoggle + "'>";

            return new MvcHtmlString(html);
        }
        #endregion html checkbox extension

        #region html radiobuttonlist extension

        private static string option(string name, string selectedValue, IEnumerable<SelectListItem> items)
        {
            string html = string.Empty;
            foreach (SelectListItem item in items)
            {
                if (item.Value.ToUpper() == selectedValue.ToUpper())
                {
                    html += "<input type='radio' name='" + name + "' checked='checked' class='minimal' value='" + item.Value + "'/>" +
                        item.Text;
                }
                else
                {
                    html += "<input type='radio'  name='" + name + "' class='minimal' value='" + item.Value + "'/>" +
                        item.Text;
                }
            }
            return html;
        }
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, string selectedValue, IEnumerable<SelectListItem> items)
        {
            var selectList = new System.Web.Mvc.SelectList(items);
            MvcHtmlString html = MvcHtmlString.Create(option(name, selectedValue, items));
            return html;// htmlHelper.RadioButtonList(name, selectList);
        }

        public static MvcHtmlString ToList(this HtmlHelper htmlHelper, IEnumerable<KeyValuePair<KeyValuePair<int, string>, int>> list, string head, string delimiter, string last, string cssClass)
        {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<KeyValuePair<int, string>, int> valuePair in list)
            {
                if (valuePair.Value > 0)
                {
                    builder.Append("<a href='#' docTypeId='" + valuePair.Key.Key + "'>" + "<li id='" + valuePair.Key.Key + "' >" + head + valuePair.Key.Value + "<span>" + delimiter + valuePair.Value + last + " </span></li>" + "</a>");
                }
            }

            MvcHtmlString html = MvcHtmlString.Create(builder.ToString());

            return html;
        }

        public static MvcHtmlString OrderField(this HtmlHelper htmlHelper, int pageIndex, int pageSize, int count)
        {
            if (pageIndex == 0)
                pageIndex = 1;
            if (pageSize * (pageIndex - 1) == _index + pageSize * (pageIndex - 1))
            {
                _index = (pageIndex - 1) * pageSize;
            }
            _index++;

            StringBuilder builder = new StringBuilder();

            builder.Append(_index.ToString());

            MvcHtmlString html = MvcHtmlString.Create(builder.ToString());

            if (_index >= pageSize * (pageIndex - 1) + count)
            {
                _index = 0;
            }
            return html;
        }
        public static MvcHtmlString OrderField(this HtmlHelper htmlHelper, int count)
        {

            _index++;

            StringBuilder builder = new StringBuilder();

            builder.Append(_index.ToString());

            MvcHtmlString html = MvcHtmlString.Create(builder.ToString());
            if (_index >= count)
            {
                _index = 0;
            }
            return html;
        }
        private static int _index = 0;


        //public static MvcHtmlString RadiaoButtonList1(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        //{
        //    TagBuilder tableTag = new TagBuilder("table");
        //    tableTag.AddCssClass("radio-main");
        //    var trTag = new TagBuilder("tr");
        //    foreach (var item in items)
        //    {
        //        var tdTag = new TagBuilder("td");
        //        var rbValue = item.Value ?? item.Text;
        //        var rbName = name + "_" + rbValue;
        //        var radioTag = helper.RadioButton(rbName, rbValue, item.Selected, new { name = name });
        //        var labelTag = new TagBuilder("label");
        //        labelTag.MergeAttribute("for", rbName);
        //        labelTag.MergeAttribute("id", rbName + "_label");
        //        labelTag.InnerHtml = rbValue;
        //        tdTag.InnerHtml = radioTag.ToString() + labelTag.ToString();
        //        trTag.InnerHtml += tdTag.ToString();
        //    }

        //    tableTag.InnerHtml = trTag.ToString();
        //    helper.RadiaoButtonList(
        //    return tableTag;
        //}
        #endregion html radiobuttonlist extension

        #region html dilog
        public static MvcHtmlString openDialog(this HtmlHelper helper, string nameDialog, string namelink, params object[] Values)
        {

            string html = string.Empty;
            html = "<a href=\"Javascript:" + nameDialog + "(";
            if (Values != null && Values.Length > 0)
            {
                foreach (string val in Values)
                {
                    html += "'" + val + "',";
                }
            }
            html = html.TrimEnd(',');
            html += ") \">";
            html += namelink + "</a>";

            return new MvcHtmlString(html);
        }
        #endregion
    }
}
