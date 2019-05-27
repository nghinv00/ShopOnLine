using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class qtUnitManagerService : BaseService<qtUnitManager, ShopOnlineDb>
    {

        public IPagedList<qtUnitManager> DanhSachNguoiDung(int page, int pageSize)
        {
            qtUserService _user = new qtUserService();
            qtUnitService _unit = new qtUnitService();
            qtUnitManagerService _mangager = new qtUnitManagerService();
            var danhsach = (from mangager in _mangager.FindList()
                            join user in _user.FindList() on mangager.UserId equals user.UserId
                            select mangager).AsQueryable().GroupBy(m => m.UserId).Select(m => m.FirstOrDefault());

            return danhsach.ToPagedList(page, pageSize);
        }
        public int SoluongNguoiDung(int? unitId)
        {
            qtUserService _user = new qtUserService();
            qtUnitService _unit = new qtUnitService();
            qtUnitManagerService _mangager = new qtUnitManagerService();
            var danhsach = (from mangager in _mangager.FindList()
                            join user in _user.FindList() on mangager.UserId equals user.UserId
                            where user.UnitId == unitId
                            select mangager).AsQueryable().GroupBy(m => m.UserId).Select(m => m.FirstOrDefault());
            return danhsach.Count();
        }

        public List<qtUnit> ListUnit_By_UserId(int? userid)
        {
            if (!userid.HasValue) return new List<qtUnit>();
            qtUnitService _unit = new qtUnitService();
            qtUnitManagerService _mangager = new qtUnitManagerService();
            List<qtUnit> danhsach = _mangager.FindList().Where(m => m.UserId == userid.Value)
                                    .Join(_unit.FindList().Where(m => m.Status == true), m => m.UnitId, u => u.UnitId, (m, u) => u)
                                    .Distinct().ToList();
            return danhsach;
        }
    }
}
