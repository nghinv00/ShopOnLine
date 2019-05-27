using C.Core.BaseController;
using C.Core.ExModel;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.QuanTri.Controllers
{
    public class DanhMucDungChungController : BaseController
    {

        public PartialViewResult Units(int? UnitId)
        {
            qtUnitService _unit = new qtUnitService();

            IEnumerable<qtUnit> danhSachDonVi = _unit.FindList();

            SelectList select = new SelectList(danhSachDonVi, "UnitId", "UnitName", UnitId);

            return PartialView("Units", select);
        }

        public PartialViewResult Departments(int? DepartmentId)
        {
            qtDepartmentService _department = new qtDepartmentService();

            IEnumerable<qtDepartment> danhSachPhongBan = _department.FindList();

            SelectList select = new SelectList(danhSachPhongBan, "UnitId", "UnitName", DepartmentId);

            return PartialView("Departments", select);
        }

        public PartialViewResult Positions(int? PositionId)
        {
            qtPositionService _position = new qtPositionService();
            IEnumerable<qtPosition> dsChucVu = _position.FindList();

            SelectList select = new SelectList(dsChucVu, "PositionId", "PositionName", PositionId);

            return PartialView("Positions", select);
        }

        //public PartialViewResult Categories(string CategoryGuid)
        //{
        //    shCategoryService _category = new shCategoryService();
        //    IEnumerable<shCategory> dsCategory = _category.FindList();

        //    SelectList select = new SelectList(dsCategory, "CategoryGuid", "CategoryName", CategoryGuid);

        //    return PartialView("Categories", select);
        //}

        #region Vẽ hình dạng TreeView cho danh sách Đơn Vị ( Phòng ban nếu có )
        /// <summary>
        /// Có chấp nhận hiển thi thêm phòng ban trong đơn vị hay không 
        /// </summary>
        /// <param name="isShowDepartment"></param>
        /// <returns></returns>
        public JsonResult UnitTreeview(bool? isShowDepartment)
        {
            if (!isShowDepartment.HasValue)
            {
                isShowDepartment = false;
            }

            TreeView tongthe = new TreeView();
            tongthe.name = "Tổng thể";
            tongthe.open = true;
            tongthe.isParent = true;
            tongthe.children = GetUnitTreeView(isShowDepartment.Value);
            tongthe.click = "unitchange(0)";

            return Json(tongthe, JsonRequestBehavior.AllowGet);
        }

        public List<TreeView> GetUnitTreeView(bool isShowDepartment)
        {
            qtUnitService _unit = new qtUnitService();

            IEnumerable<qtUnit> dsUnit = _unit.FindList()
                            .Where(x => x.ParentId == null || x.ParentId == 0);

            List<TreeView> dsTreeView = new List<TreeView>();
            TreeView zTree = null;
            TreeView childZtree = null;

            foreach (var item in dsUnit)
            {
                zTree = new TreeView();
                zTree.name = item.UnitName;
                zTree.isParent = true;
                zTree.id = item.UnitId.ToString();
                zTree.pId = (item.ParentId == null || item.ParentId == 0) ? "" : item.ParentId.ToString();

                IEnumerable<qtUnit> dsChildUnit = _unit.GetUnitsByParentId(item.UnitId);

                List<TreeView> dsChildTreeview = null;

                if (dsChildUnit.Count() > 0)
                {
                    dsChildTreeview = new List<TreeView>();

                    foreach (var childUnit in dsChildUnit)
                    {
                        childZtree = new TreeView();
                        childZtree.id = childUnit.UnitId.ToString();
                        childZtree.pId = (childUnit.ParentId == null || childUnit.ParentId == 0) ? "" : childUnit.ParentId.ToString();
                        childZtree.name = childUnit.UnitName;

                        if (isShowDepartment)
                        {
                            childZtree.isParent = isShowDepartment; // TRUE Áp dụng cho hiển thị đơn vị có phòng ban  đi kèm
                            childZtree.children = DepartmentByUnit(childUnit.UnitId);
                            if (childZtree.children != null && childZtree.children.Count() > 0)// Kiểm tra nếu đơn vi CÓ phòng ban đi kèm thì hiển thị phòng ban
                            {
                                childZtree.open = true;
                            }
                            else // Kiểm tra nếu đơn vi KHÔNG phòng ban đi kèm thì KHÔNG hiển thị
                            {
                                childZtree.open = false;
                            }
                        }
                        else
                        {
                            childZtree.isParent = isShowDepartment;  // FALSE Áp dụng cho hiển thị đơn vị không phòng ban kèm theo
                            childZtree.children = null;
                            childZtree.open = false;
                        }

                        childZtree.click = "unitchange(" + childUnit.UnitId + ")";


                        dsChildTreeview.Add(childZtree);
                    }

                    zTree.children = dsChildTreeview;
                    zTree.open = true;
                    zTree.click = "unitchange(" + item.UnitId + ")";
                }
                else
                {
                    zTree.click = "unitchange(" + item.UnitId + ")";
                    zTree.open = false;
                }
                dsTreeView.Add(zTree);
            }
            return dsTreeView;
        }

        public List<TreeView> DepartmentByUnit(int? UnitId)
        {
            if (UnitId.HasValue)
            {
                TreeView zTree = null;
                List<TreeView> dsTreeview = new List<TreeView>();

                qtDepartmentService _department = new qtDepartmentService();
                IEnumerable<qtDepartment> dsDepartment = _department.DanhSachDepartment_TheoDonVi(UnitId);

                foreach (var item in dsDepartment)
                {
                    zTree = new TreeView();

                    zTree.name = item.DepartmentName;
                    zTree.id = item.DepartmentId.ToString();
                    //zTree.pId = UnitId.ToString();
                    zTree.isParent = false;
                    zTree.children = null;
                    zTree.click = null;
                    zTree.open = true;

                    dsTreeview.Add(zTree);
                }

                return dsTreeview;
            }

            return null;
        }
        #endregion

    }
}
