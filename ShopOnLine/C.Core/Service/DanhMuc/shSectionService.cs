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
    public class shSectionService : BaseService<shProductSet, ShopOnlineDb>
    {
        #region SectionName
        public string SectionName(string SectionGuid)
        {
            if (!String.IsNullOrWhiteSpace(SectionGuid))
            {
                shSectionService _section = new shSectionService();
                shProductSet section = _section.FindByKey(SectionGuid);

                if (section != null)
                    return section.SectionName;
            }
            return string.Empty;
        }
        #endregion
        #region Ds
        public IEnumerable<shProductSet> DanhSachSection()
        {
            shSectionService _section = new shSectionService();

            return _section.FindList()
                .Where(x => x.Status == true)
                .OrderBy(m => m.SectionId);
        }

        public IEnumerable<shProductSet> DanhSachSection_TheoProductGuid(string ProductGuid)
        {
            IEnumerable<shProductSet> dsSection = DanhSachSection();
            if (!string.IsNullOrWhiteSpace(ProductGuid))
            {
                dsSection = dsSection.Where(m => m.ProductGuid == ProductGuid
                                                //&& (string.IsNullOrEmpty(m.ParentId) || (string.IsNullOrWhiteSpace(m.ParentId)))
                                                )
                                                 .OrderBy(x => x.SectionId);
            }

            return dsSection;
        }

        public IEnumerable<shProductSet> DanhSachSection_TheoProductGuid_ParentNull(string ProductGuid)
        {
            IEnumerable<shProductSet> dsSection = DanhSachSection();
            if (!string.IsNullOrWhiteSpace(ProductGuid))
            {
                dsSection = dsSection.Where(m => m.ProductGuid == ProductGuid
                                                && (string.IsNullOrEmpty(m.ParentId) || (string.IsNullOrWhiteSpace(m.ParentId)))
                                                )
                                                 .OrderBy(x => x.SectionId);
            }

            return dsSection;
        }

        public IEnumerable<shProductSet> DanhSachSection_TheoParentId(string SectionGuid)
        {
            IEnumerable<shProductSet> dsSection = DanhSachSection();
            if (!string.IsNullOrWhiteSpace(SectionGuid))
            {
                dsSection = dsSection.Where(x => x.ParentId == SectionGuid);
            }

            return dsSection;
        }
        #endregion

        #region Insert - Update shProductSet
        public shProductSet Insert_UpdateSection(string SectionGuid, int? SectionId, string ProductGuid, string SectionName, int? SortOrder, int? ItemStatus, bool? Status, DateTime? CreateDate, string ParentId)
        {
            shSectionService _section = new shSectionService();
            shProductSet section = new shProductSet();

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                if (!string.IsNullOrWhiteSpace(SectionGuid))
                    section = _section.FindByKey(SectionGuid);
                else
                    SectionGuid = GuidUnique.getInstance().GenerateUnique();

                section.SectionGuid = SectionGuid;
                //section.SectionId = SectionId;
                section.ProductGuid = ProductGuid;
                section.SectionName = SectionName;
                section.SortOrder = SortOrder;
                section.ItemStatus = ItemStatus;
                section.Status = Status;
                section.CreateDate = CreateDate;

                section.MetaTitle = "/" + StringHelper.ToUnsignString(section.SectionName).ToLower();
                section.ParentId = ParentId;

                if (section.SectionId > 0)
                    _section.Update(section);
                else
                    _section.Insert(section);

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

            return section;
        }
        #endregion

    }
}
