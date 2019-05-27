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
    public class qtSubstitutesService : BaseService<qtSubstitute, ShopOnlineDb>
    {
        public IPagedList<qtSubstitute> FindListByPage(int? depId, int? Page)
        {
            qtSubstitutesService _Substitute = new qtSubstitutesService();

            var list = _Substitute.FindList();

            if (depId.HasValue)
                list = list.Where(m => m.DepartmentId == depId);

            return list.ToPagedList(Page.HasValue ? Page.Value : 1, 10);
        }
    }
}
