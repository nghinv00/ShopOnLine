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
    public class shNewService : BaseService<shNew, ShopOnlineDb>
    {
        #region GetListNews
        public IEnumerable<shNew> DanhSachNews()
        {
            shNewService _new = new shNewService();

            IEnumerable<shNew> dsNews = _new.FindList()
                                                .Where(x => x.Status == true)
                                                .OrderBy( m=> m.NewId);

            return dsNews;
        }

        public IPagedList<shNew> DanhSachNews_PhanTrang(int page, int pageSize)
        {
            IPagedList<shNew> pageList_dsNews = DanhSachNews().ToPagedList(page, pageSize);

            return pageList_dsNews;
        }
        #endregion

        #region Insert - Create
        public shNew ThemMoi_HieuChinhshNew(string NewGuid, int? NewId, string TitleNew, string Descriptions, string ImageFile, string Summary, string Contents, int? SortOrder, bool? Status, DateTime? CreatedDate)
        {
            shNewService _new = new shNewService();
            shNew tintuc = new shNew();

            if (!string.IsNullOrWhiteSpace(NewGuid))
                tintuc = _new.FindByKey(NewGuid);
            else
                NewGuid = GuidUnique.getInstance().GenerateUnique();
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                tintuc.NewGuiId = NewGuid;
                tintuc.TitleNew = TitleNew;
                tintuc.Descriptions = Descriptions;
                tintuc.ImageFile = ImageFile;
                tintuc.Summary = Summary;
                tintuc.Contents = Contents;
                tintuc.SortOrder = SortOrder;
                tintuc.Status = Status;
                tintuc.CreatedDate = CreatedDate;

                tintuc.MetaTitle = "/tin-tuc/" + StringHelper.ToUnsignString(TitleNew).ToLower();

                if (tintuc.NewId > 0)
                    _new.Update(tintuc);
                else
                    _new.Insert(tintuc);
               
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
                throw;
            }

            return tintuc;


        }
        #endregion

    }
}
