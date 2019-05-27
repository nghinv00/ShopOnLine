using C.Core.CustomController;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Customer.Controllers
{
    // Đại lý
    public class AgentController : CustomController
    {

        #region Index
        public ActionResult Index(int? id, int? state, string key)
        {

            ViewBag.id = id;
            ViewBag.state = state;
            ViewBag.AgentAddress = key;

            #region Dropdonwlist 
            landProvinceService _province = new landProvinceService();

            ViewBag.City = new SelectList(_province.DanhSachProvince(), "ProvinceId", "Name", id);

            landDistrictService _district = new landDistrictService();
            List<landDistrict> ds = new List<landDistrict>();
            if (id.HasValue)
            {
                ds = _district.FindList().Where(x => x.ProvinceId == id).ToList();
            }

            ViewBag.Town = new SelectList(ds, "DistrictId", "Name", state);

            #endregion

            return View();
        }
        #endregion


        #region Agent
        public ActionResult Agent(int? ProvinceId, int? DistrictId, string AgentAddress)
        {
            shAgentService _agent = new shAgentService();

            IEnumerable<shAgent> danhSach = _agent.DanhSachAgentBy(ProvinceId, DistrictId, AgentAddress);

            return PartialView("Agent", danhSach);
        }
        #endregion

    }
}
