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
    public class shSexService : BaseService<shSex, ShopOnlineDb>
    {

        public string SexName(int? SexId)
        {
            if (SexId.HasValue)
            {
                shSexService _sex = new shSexService();
                shSex sex = _sex.FindByKey(SexId);

                if (sex != null)
                    return sex.SexName;
                return string.Empty;
            }
            return string.Empty;
        }

     

    }
}
