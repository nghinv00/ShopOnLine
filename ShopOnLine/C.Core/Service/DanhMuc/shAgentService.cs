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
    public class shAgentService : BaseService<shAgent, ShopOnlineDb>
    {

        #region DS
        public IEnumerable<shAgent> DanhSachAgent()
        {
            shAgentService _agent = new shAgentService();
            return _agent.FindList().Where(x => x.Status == true).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<shAgent> DanhSachAgentBy(int? ProvinceId, int? DistrictId, string AgentAddress)
        {
            IEnumerable<shAgent> ds = DanhSachAgent();

            if (ProvinceId.HasValue)
                ds = ds.Where(x => x.ProvinceId == ProvinceId);

            if (DistrictId.HasValue)
                ds = ds.Where(x => x.DistrictId == DistrictId);

            if (!string.IsNullOrEmpty(AgentAddress) || !string.IsNullOrWhiteSpace(AgentAddress))
                ds = ds.Where(x => TypeHelper.CompareString(x.AgentAddress, AgentAddress) || TypeHelper.CompareString(x.AgentName, AgentAddress));

            return ds;
        }
        #endregion


    }
}
