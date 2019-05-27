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
    public class qtUserConfigService : BaseService<qtUserConfig, ShopOnlineDb>
    {
        public IPagedList<qtUserConfig> FindListByPage(int? UnitId, int? Page)
        {
            qtUserConfigService _userconfig = new qtUserConfigService();
            var list = _userconfig.FindList();
            if (UnitId.HasValue)
                list = list.Where(m => m.UnitId == UnitId);
            return list.ToPagedList(Page.HasValue ? Page.Value : 1, 10);
        }

    }
}
