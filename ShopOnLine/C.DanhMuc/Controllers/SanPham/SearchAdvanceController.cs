using C.Core.BaseController;
using C.Core.Model;
using C.Core.Service;
using C.Core.Common;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using C.Membership.Helper;
using System.Web;

namespace C.DanhMuc.Controllers
{
    public class SearchAdvanceController : BaseController
    {
        #region Index
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListMembers(pageCurrent);
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? page, bool? IsHttpPost)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            ListMembers(pageCurrent);
            return View();
        }

        public PartialViewResult ListMembers(int? page)
        {
            int pageCurrent = 1;
            if (page.HasValue)
                pageCurrent = page.Value;

            shMemberService _member = new shMemberService();

            IPagedList<shMember> dsMember = _member.DanhSachMember_PhanTrang(pageCurrent, Config.PAGE_SIZE_10);

            ViewBag.ListMembers = dsMember;
            return PartialView("ListMembers", dsMember);
        }

        public void DropDownListMenu()
        {
            shSexService _sex = new shSexService();
            IEnumerable<shSex> dsSex = _sex.FindList();

            ViewBag.Sex = new SelectList(dsSex, "SexId", "SexName", null);

        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            DropDownListMenu();
            if (!string.IsNullOrWhiteSpace(id))
            {
                shMemberService _member = new shMemberService();
                shMember member = _member.FindByKey(id);

                if (member != null)
                {
                    return View(member);
                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống. Vui lòng kiểm tra lại");
                    return View(new shMember());
                }
            }

            ViewBag.MemberId = id;
            return View(new shMember());
        }

        [HttpPost]
        public ActionResult Create(int? MemberId, string MemberGuiId,
            string MemberName, string MemberLogin, string Password,
            string Address, int? Sex, string Tel,
            string Email, string Phone, string Notes, bool? Status,
            string ImageFile)
        {
            using (var context = new ShopOnlineDb())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        shMemberService _member = new shMemberService();
                        shMember member = _member.ThemMoi_HieuChinhMember(
                                    MemberGuiId,
                                    null,
                                    MemberName,
                                    MemberLogin,
                                    Password,
                                    ImageFile,
                                    Address,
                                    Sex,
                                    Email,
                                    Tel,
                                    Phone,
                                    Notes,
                                    Status,
                                    DateTime.Now,
                                    null,
                                    null
                                    );

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbContextTransaction.Rollback();
                    }
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

    }
}
