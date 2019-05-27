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
    public class shIdeaCustomerService : BaseService<shIdeaCustomer, ShopOnlineDb>
    {

        public IEnumerable<shIdeaCustomer> DanhSachIdea()
        {
            shIdeaCustomerService _idea = new shIdeaCustomerService();
            return _idea.FindList().Where(x => x.Status == true).OrderBy(x => x.SortOrder);
        }

    }
}
