using C.Core.Common;
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
    public static class CategoryHelper
    {
        public static MvcHtmlString CategoryName(this HtmlHelper helper, string CategoryGuid)
        {
            shCategoryService _category = new shCategoryService();

            return new MvcHtmlString(_category.CategoryName(CategoryGuid));
        }
        public static string CategoryImageIcon(this HtmlHelper helper, string CategoryGuid)
        {
            shCategoryImageService _categoryImage = new shCategoryImageService();
            shCategoryImage categoryimage = _categoryImage.FindList()
                            .Where(x => x.CategoryGuid == CategoryGuid && x.Image == Config.Category_Image_Icon)
                            .LastOrDefault();
            if (categoryimage == null)
            {
                categoryimage = new shCategoryImage();
            }
            return categoryimage.FileName;
        }

        public static string CategoryImageIconActive(this HtmlHelper helper, string CategoryGuid)
        {
            shCategoryImageService _categoryImage = new shCategoryImageService();
            shCategoryImage categoryimage = _categoryImage.FindList()
                            .Where(x => x.CategoryGuid == CategoryGuid && x.Image == Config.Category_Image_Icon_Active)
                            .LastOrDefault();

            if (categoryimage == null)
            {
                categoryimage = new shCategoryImage();
            }
            return categoryimage.FileName;
        }

        public static string CategoryImage(this HtmlHelper helper, string CategoryGuid)
        {
            shCategoryImageService _categoryImage = new shCategoryImageService();
            shCategoryImage categoryimage = _categoryImage.FindList()
                            .Where(x => x.CategoryGuid == CategoryGuid
                                    && (string.IsNullOrWhiteSpace(x.Image) || string.IsNullOrEmpty(x.Image)))
                            .LastOrDefault();
            if (categoryimage == null)
            {
                categoryimage = new shCategoryImage();
            }
            return categoryimage.FileName;
        }


        public static MvcHtmlString DanhMucCha(this HtmlHelper helper, string CategoryGuid, string CategoryName)
        {
            string html = string.Empty;

            shCategoryService _category = new shCategoryService();

            html += CommonHelper.LamDanhMucHienThiView(CategoryGuid);

            html += "<a href='/DanhMuc/Category/Create/" + CategoryGuid + "'  class='category-parent'>" + CategoryName + "</a>";

            return new MvcHtmlString(html);
        }
    }
}
