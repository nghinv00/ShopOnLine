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
    public class qtDepartmentService : BaseService<qtDepartment, ShopOnlineDb>
    {

        public string DepartmentName(int? DepartmentId)
        {
            string DepartmentName = string.Empty;
            if (DepartmentId.HasValue)
            {
                qtDepartmentService _departmentService = new qtDepartmentService();
                qtDepartment department = _departmentService.FindByKey(DepartmentId);
                if (department != null)
                    return department.DepartmentName;
                return DepartmentName;
            }
            return DepartmentName;
        }

        #region Get List Department
        public IEnumerable<qtDepartment> DanhSachDepartment_TheoDonVi(int? UnitId)
        {
            qtDepartmentService _department = new qtDepartmentService();
            IEnumerable<qtDepartment> dsDepartment = _department.FindList();

            if (UnitId.HasValue && UnitId.Value > 0)
                dsDepartment = dsDepartment.Where(x => x.UnitId == UnitId);

            return dsDepartment;
        }

        public IPagedList<qtDepartment> DanhSachDepartment_PhanTrang(int? UnitId, int page, int pageSize)
        {
            IPagedList<qtDepartment> pageList_dsDepartment = DanhSachDepartment_TheoDonVi(UnitId).ToPagedList(page, pageSize);

            return pageList_dsDepartment;

        }

        #endregion

        #region Insert - Update Department 
        public qtDepartment ThemMoi_HieuChinhDepartment(int DepartmentId, string DepartmentName, string Address, string Phone, string Fax, string Email, int? UnitId, int? SortOrder, bool? Status, DateTime? CreatedDate)
        {
            qtDepartmentService _department = new qtDepartmentService();
            qtDepartment department = new qtDepartment();

            if (DepartmentId > 0)
                department = _department.FindByKey(DepartmentId);

            department.DepartmentName = DepartmentName;
            department.Address = Address;
            department.Phone = Phone;
            department.Fax = Fax;
            department.Email = Email;
            department.UnitId = UnitId;
            department.SortOrder = SortOrder;
            department.Status = Status;
            department.CreatedDate = CreatedDate;

            if (DepartmentId > 0)
                _department.Update(department);
            else
            _department.Insert(department);

            return department;
        }
        #endregion
    }
}
