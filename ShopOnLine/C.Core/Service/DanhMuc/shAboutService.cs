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
    public class shAboutService : BaseService<shAboutu, ShopOnlineDb>
    {
        #region Ds
        public IEnumerable<shAboutu> DanhSachAbout()
        {
            shAboutService _about = new shAboutService();
            return _about.FindList().OrderBy(x => x.AboutId)
                                .Where(x => string.IsNullOrEmpty(x.ParentId) ||
                                            string.IsNullOrWhiteSpace(x.ParentId)
                                            && x.Status == true);
        }

        public IPagedList<shAboutu> DanhSachAbout_PhanTrang(int pageCurrent, int pageSize)
        {
            IEnumerable<shAboutu> dsAbout = DanhSachAbout();

            return dsAbout.ToPagedList(pageCurrent, pageSize);
        }

        public IEnumerable<shAboutu> DanhSachAbout_ByParentId(string ParentId)
        {
            shAboutService _about = new shAboutService();
            IEnumerable<shAboutu> dsAbout = _about.FindList().Where(x => x.ParentId == ParentId && x.Status == true);
            return dsAbout;
        }

        #endregion

        #region Insert - Update
        public shAboutu Insert_About(string AboutGuid, int? AboutId, string AboutTitle, string AboutName, string AboutContent, int? Year, string Sign, string ImageUrl, string ParentId, int? SortOrder, bool? Status, DateTime? CreateDate)
        {
            shAboutService _about = new shAboutService();
            shAboutu about = new shAboutu();
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                if (!string.IsNullOrWhiteSpace(AboutGuid) || !string.IsNullOrEmpty(AboutGuid))
                {
                    about = _about.FindByKey(AboutGuid);
                }
                else
                {
                    about.AboutGuid = Common.GuidUnique.getInstance().GenerateUnique();
                }

                about.AboutTitle = AboutTitle;
                about.AboutName = AboutName;
                about.AboutContent = AboutContent;
                about.Year = Year;
                about.Sign = Sign;
                about.ImageUrl = ImageUrl;
                about.ParentId = ParentId;
                about.SortOrder = SortOrder;
                about.Status = Status;
                about.CreateDate = CreateDate;

                about.MetaTitle = "/gioi-thieu/" + StringHelper.ToUnsignString(AboutTitle).ToLower();

                if (about.AboutId > 0)
                    _about.Update(about);
                else
                    _about.Insert(about);
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


            return about;
        }
        #endregion
    }
}
