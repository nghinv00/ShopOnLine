using C.Core.Common;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shCategoryService : BaseService<shCategory, ShopOnlineDb>
    {

        public string CategoryName(int? CategoryId)
        {
            if (CategoryId.HasValue)
            {
                shCategoryService _category = new shCategoryService();
                shCategory category = _category.FindByKey(CategoryId);

                if (category != null)
                    return category.CategoryName;
                return string.Empty;
            }
            return string.Empty;
        }

        public string CategoryName(string CategoryGuid)
        {
            if (!string.IsNullOrWhiteSpace(CategoryGuid))
            {
                shCategoryService _category = new shCategoryService();
                shCategory category = _category.FindByKey(CategoryGuid);

                if (category != null)
                    return category.CategoryName;
                return string.Empty;
            }
            return string.Empty;
        }

        public string CategoryMetaTitle(string CategoryGuid)
        {
            string metatitle = string.Empty;
            if (!string.IsNullOrWhiteSpace(CategoryGuid))
            {
                shCategoryService _category = new shCategoryService();
                shCategory category = _category.FindByKey(CategoryGuid);

                if (category != null)
                    return category.MetaTitle;
                return metatitle;
            }
            return metatitle;
        }

        #region GetListCategory
        public IEnumerable<shCategory> DanhSachCategory()
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> dsCategory = _category.FindList()
                                        .Where(x => x.Status == true)
                                        .OrderBy(x => x.CategoryId);

            return dsCategory;
        }

        public IPagedList<shCategory> DanhSachCategory_PhanTrang(int page, int pageSize)
        {
            IPagedList<shCategory> pageList_dsCategory = DanhSachCategory().ToPagedList(page, pageSize);

            return pageList_dsCategory;
        }

        public IEnumerable<shCategory> DanhSachCategory_TopHot()
        {
            return DanhSachCategory().Where(x => x.TopHot == true);
        }

        public IEnumerable<shCategory> DanhSachCategory_ByParentId(string ParentId)
        {
            shCategoryService _category = new shCategoryService();

            IEnumerable<shCategory> dsCategory = _category.FindList()
                                        .Where(x => x.Status == true
                                                && x.ParentId == ParentId)
                                        .OrderBy(x => x.CategoryId);

            return dsCategory;
        }
        #endregion

        #region Thêm mới - Hiệu chỉnh Category 
        public shCategory ThemMoi_HieuChinhCategory(string CategoryGuid, int? CategoryId, string CategoryName, string ParentId, int? UserId, bool? Status, DateTime? CreatedDate, string MetaTitle, string Description, int? SortOrder, string FileName)
        {
            shCategoryService _category = new shCategoryService();
            shCategory category = new shCategory();

            if (!string.IsNullOrWhiteSpace(CategoryGuid))
                category = _category.FindByKey(CategoryGuid);
            else
                CategoryGuid = GuidUnique.getInstance().GenerateUnique();

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                category.CategoryGuid = CategoryGuid;
                category.CategoryName = CategoryName;
                category.ParentId = ParentId;
                category.UserId = UserId;
                category.Status = Status;
                category.CreatedDate = CreatedDate;

                category.Description = Description;
                category.SortOrder = SortOrder;

                if (string.IsNullOrWhiteSpace(ParentId) || string.IsNullOrEmpty(ParentId))
                {
                    category.MetaTitle = "/" + StringHelper.ToUnsignString(CategoryName).ToLower()
                                           + "/" + StringHelper.ToUnsignString(Description.Substring(0, 60)).ToLower();

                }
                else
                {
                    category.MetaTitle = TaoLinkUrl(CategoryGuid, CategoryName);
                }
                category.FileName = FileName;

                if (category.CategoryId > 0)
                    _category.Update(category);
                else
                    _category.Insert(category);

                return category;

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return category;
            }
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryGuid"></param>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public string TaoLinkUrl(string CategoryGuid, string CategoryName)
        {

            string controller = CommonHelper.GetUrlTheFirst(CategoryGuid);
            shCategoryService _category = new shCategoryService();
            string Metatitle = _category.CategoryMetaTitle(controller);

            int idex = Metatitle.LastIndexOf("/");
            string meta = Metatitle.Substring(0, idex);

            meta += "/" + StringHelper.ToUnsignString(CategoryName).ToLower();

            return Metatitle;
        }
        #endregion

        #region Get shCategory theo ParentId
        public IEnumerable<shCategory> GetCategoryByParentId(string CategoryGuid)
        {
            shCategoryService _categories = new shCategoryService();

            return _categories.FindList().Where(x => x.ParentId == CategoryGuid);
        }
        #endregion


        #region Cấu hình sản phẩm HightLight 
        public void HighLight(string[] cbxItem)
        {
            foreach (var item in cbxItem)
            {
                UpdateTopHot(item, true);
            }
        }

        public void UnSubcribeHighLight(string[] cbxItem)
        {
            foreach (var item in cbxItem)
            {
                UpdateTopHot(item, false);

            }
        }

        public void UpdateTopHot(string CategoryGuid, bool TopHot)
        {
            shCategoryService _category = new shCategoryService();
            shCategory category = _category.FindByKey(CategoryGuid);
            if (category != null)
            {
                category.TopHot = TopHot;
                _category.Update(category);
            }
        }

        #endregion


    }
}
