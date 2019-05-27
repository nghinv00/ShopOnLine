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
    public class shCategoryImageService : BaseService<shCategoryImage, ShopOnlineDb>
    {

        public string CategoryImageFileName(string CategoryImageGuid, string CategoryImageId)
        {
            if (!string.IsNullOrWhiteSpace(CategoryImageGuid))
            {
                shCategoryImageService _categoryimage = new shCategoryImageService();
                shCategoryImage category = _categoryimage.FindByKey(CategoryImageGuid);
                
                if (category != null)
                    return category.FileName;
                return string.Empty;
            }
            return string.Empty;
        }


        #region Thêm mới - Hiệu chỉnh Category 
        


        public shCategoryImage Insert_UpdateCategoryImage(
            string CategoryImageGuid,
            int? CategoryId, 
            string CategoryGuid, 
            string FileName, 
            string FileNameGoc,
            int? UserId, 
            bool? Status, 
            DateTime? CreateDate, 
            string Image)
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges


                shCategoryImageService _categoryimage = new shCategoryImageService();
                shCategoryImage category = new shCategoryImage();

                if (!string.IsNullOrWhiteSpace(CategoryImageGuid))
                    category = _categoryimage.FindByKey(CategoryImageGuid);
                else
                    CategoryImageGuid = GuidUnique.getInstance().GenerateUnique();

                category.CategoryImageGuid = CategoryImageGuid;
                //category.CategoryId = CategoryId;
                category.CategoryGuid = CategoryGuid;
                category.FileName = FileName;
                category.FileNameGoc = FileNameGoc;
                category.UserId = UserId;
                category.Status = Status;
                category.CreateDate = CreateDate;
                //category.CategoryImage = CategoryImage;

                category.Image = Image;

                if (category.CategoryId > 0)
                    _categoryimage.Update(category);
                else
                    _categoryimage.Insert(category);

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
                return null;
            }
        }
        #endregion


    }
}
