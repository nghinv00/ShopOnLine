using C.Core.Model;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class qtRoleService : BaseService<qtRole, ShopOnlineDb>
    {
        /// <summary>
        /// Kiểm tra quyền của tài khoản đã được phân quyền hay chưa?
        /// </summary>
        /// <param name="UnitId"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <param name="ApplicationCode"></param>
        /// <param name="FunctionCode"></param>
        /// <returns>True nếu đã được phân quyền và False nếu chưa được phân quyền </returns>
        public bool CheckUserInRole(int UnitId, int DepartmentId, int PositionId, string ApplicationCode, string FunctionCode )
        {
            ShopOnlineDb db = new ShopOnlineDb();

            var role = (from u in db.qtUserRoles
                        from r in db.qtRoles
                        where u.RoleId == r.RoleId
                           && u.UnitId == UnitId
                           && u.PositionId == PositionId
                           && u.DepartmentId == DepartmentId
                           && r.Description.Trim() == FunctionCode.Trim()
                        select r.RoleId)
                           .Count();
            if (TypeHelper.ToInt32(role.ToString()) > 0)
                return true;
            return false;
        }

    }
}
