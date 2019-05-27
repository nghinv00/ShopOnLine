using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Membership.Helper;
using C.Core.Model;
using C.Core.Service;
using C.QuanTri.Helper;

namespace C.QuanTri.Controllers
{
    public class UserInfoController : Controller
    {
        public ActionResult Index()
        {
            qtUserService _user = new qtUserService();
            int userid = User.Identity.GetUserLogin().Userid;
            var user = _user.FindByKey(userid);
            ThongTin(user);
            ViewBag.hdUserId = userid;
            return View(user);
        }

        public void ThongTin(qtUser user)
        {
            qtDepartmentService _department = new qtDepartmentService();
            qtUnitService _unit = new qtUnitService();
            qtPositionService _position = new qtPositionService();
            ViewBag.TenPhong = _department.FindByKey(user.DepartmentId).DepartmentName;
            ViewBag.TenDonVi = _unit.FindByKey(user.UnitId).UnitName;
            ViewBag.ChucVu = _position.FindByKey(user.PositionId).PositionName;
        }

        public ActionResult CapNhatThongTin(int hdUserId, string Tel, string Email)
        {
            qtUserService _user = new qtUserService();
            var user = _user.FindByKey(hdUserId);
            if(user != null)
            {
                user.Tel = Tel;
                user.Email = Email;
                _user.Update(user);
            }

            UserHelper.SaveFileImage(hdUserId, Request.Files);
            return RedirectToAction("Index");
        }

        //Kiem tra ho so ton tai
        //[HttpPost]
        //public JsonResult KiemTraTonTai(string nguoidungId)
        //{
        //    int hosoId = 0;
        //    Cbcc_HoSoCanBoService _hoSoCanBo = new Cbcc_HoSoCanBoService();
        //    IEnumerable<Cbcc_HoSoCanBo> dsHoSoCanBo = _hoSoCanBo.FindList().Where(m => m.TaiKhoanSuDungId == TypeHelper.ToInt32(nguoidungId));
        //    if (dsHoSoCanBo.Count() > 0)
        //        hosoId = dsHoSoCanBo.FirstOrDefault().HoSoCanBoId;
        //    return Json(hosoId, JsonRequestBehavior.AllowGet);
        //}
    }
}
