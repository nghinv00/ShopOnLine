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
    public class landProvinceService : BaseService<landProvince, ShopOnlineDb>
    {

        public IEnumerable<landProvince> DanhSachProvince()
        {
            landProvinceService _province = new landProvinceService();
            return _province.FindList().OrderBy(x => x.SortOrder);
        }
    }
}
