using C.Core.Model;
using C.UI.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class qtUnitService : BaseService<qtUnit, ShopOnlineDb>
    {

        public string UnitName(int? UnitId)
        {
            string UnitName = string.Empty;
            if (UnitId.HasValue)
            {
                qtUnitService _unit = new qtUnitService();
                qtUnit unit = _unit.FindByKey(UnitId);
                if (unit != null)
                    return unit.UnitName;
                return UnitName;
            }
            return UnitName;
        }

        #region Get List Unit
        public IEnumerable<qtUnit> DanhSachUnit()
        {
            qtUnitService _unit = new qtUnitService();
            IEnumerable<qtUnit> dsUnit = _unit.FindList();

            return dsUnit;
        }

        public IPagedList<qtUnit> DanhSachUnit_PhanTrang(int page, int pageSize)
        {

            IPagedList<qtUnit> pageList_dsUnit = DanhSachUnit().ToPagedList(page, pageSize);
            return pageList_dsUnit;

        }
        #endregion

        #region Insert - Update qtUnits
        public qtUnit ThemMoi_HieuChinhUnit(int UnitId, string UnitName, string UnitCode, int? ParentId, string LevelCode, int? SortOrder, 
            string Email, string PhoneNumber, bool? Status, DateTime? CreatedDate)
        {
            qtUnitService _unit = new qtUnitService();
            qtUnit unit = new qtUnit();

            if (UnitId > 0)
                unit = _unit.FindByKey(UnitId);

            unit.UnitName = UnitName;
            unit.UnitCode = UnitCode;
            unit.ParentId = ParentId;
            unit.LevelCode = LevelCode;
            unit.SortOrder = SortOrder;
            unit.Email = Email;
            unit.PhoneNumber = PhoneNumber;
            unit.Status = Status;
            unit.CreatedDate = CreatedDate;

            if (UnitId > 0)
                _unit.Update(unit);
            else
                _unit.Insert(unit);

            return unit;
        }
        #endregion

        #region Get Units theo ParentId 
        public IEnumerable<qtUnit> GetUnitsByParentId(int? ParentId)
        {
            qtUnitService _unit = new qtUnitService();

            return _unit.FindList().Where(x => x.ParentId == ParentId);
        }
        #endregion
    }
}
