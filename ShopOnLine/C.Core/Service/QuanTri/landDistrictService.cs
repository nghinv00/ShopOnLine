
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
    public class landDistrictService : BaseService<landDistrict, ShopOnlineDb>
    {

        public IEnumerable<landDistrict> DanhSachDistrict(int? ProvinceId)
        {
            landDistrictService _district = new landDistrictService();

            IEnumerable<landDistrict> ds = _district.FindList();

            if (ProvinceId.HasValue)
            {
                ds = ds.Where(x => x.ProvinceId == ProvinceId);
            }
            return ds.OrderBy(x => x.SortOrder);
        }

    }
}
