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

namespace C.DanhMuc.Controllers
{
    public class DanhMucController : BaseController
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

        public PartialViewResult Categories(string CategoryGuid)
        {
            shCategoryService _category = new shCategoryService();
            IEnumerable<shCategory> dsCategory = _category.FindList().Where(x => x.ParentId == null || x.ParentId == string.Empty).OrderBy(x => x.CategoryId);

            SelectList select = new SelectList(dsCategory, "CategoryGuid", "CategoryName", CategoryGuid);

            return PartialView("Categories", select);
        }

        #region Vẽ hình dạng TreeView cho danh sách Category
        /// <summary>
        /// Vẽ hình dạng TreeView cho danh sách danh mục 
        /// </summary>
        /// <returns>Chuỗi Json chứa hình dạng treeview cho shCategory </returns>
        public JsonResult CategoryTreeview(bool? isProduct)
        {
            if (!isProduct.HasValue)
                isProduct = false;

            TreeView tongthe = new TreeView();
            tongthe.name = "DANH MỤC";
            tongthe.open = true;
            tongthe.isParent = true;
            tongthe.children = GetCategoryTreeview(isProduct.Value);

            if (tongthe.children != null && tongthe.children.Count() > 0 && !isProduct.Value)
            {
                tongthe.name += "(" + tongthe.children.Count() + ")";
            }
            return Json(tongthe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryTreeviewNotRoot(bool? isProduct)
        {
            if (!isProduct.HasValue)
                isProduct = false;

            return Json(GetCategoryTreeview(isProduct.Value), JsonRequestBehavior.AllowGet);
        }

        public List<TreeView> GetCategoryTreeview(bool isProduct)
        {
            shCategoryService _categories = new shCategoryService();

            IEnumerable<shCategory> dsCategory = _categories.DanhSachCategory()
                                    .Where(x => string.IsNullOrWhiteSpace(x.ParentId))
                                    .OrderBy(x => x.CategoryId);

            List<TreeView> dsTreeView = new List<TreeView>();
            TreeView zTree = null;
            TreeView childZtree = null;
            foreach (var item in dsCategory)
            {
                zTree = new TreeView();
                //zTree.open = soDem == 0 ? true : false;
                zTree.isParent = true;
                zTree.id = item.CategoryGuid;
                zTree.pId = item.ParentId;

                IEnumerable<shCategory> dsChildCategory = _categories.GetCategoryByParentId(item.CategoryGuid).OrderBy(x => x.CategoryId);

                List<TreeView> dsChildTreeview = null;
                if (dsChildCategory.Count() > 0) // Có phần tử con
                {
                    dsChildTreeview = new List<TreeView>();

                    foreach (var childCategory in dsChildCategory)
                    {
                        childZtree = new TreeView();
                        childZtree.id = childCategory.CategoryGuid;
                        childZtree.pId = childCategory.ParentId;
                        childZtree.name = childCategory.CategoryName;
                        if (isProduct)
                        {
                            // TRUE Áp dụng cho hiển thị Danh sách sản phẩm theo danh mục
                            childZtree.isParent = isProduct;
                            childZtree.children = ProductByCategoryGuid(childCategory.CategoryGuid);
                            //childZtree.open = true;
                            childZtree.click = "category('" + childCategory.CategoryId + "')";

                            if (childZtree.children != null && childZtree.children.Count() > 0)
                            {
                                childZtree.name = childCategory.CategoryName + "[" + childZtree.children.Count() + "]";
                            }
                        }
                        else
                        {
                            // FALSE Áp dụng KHÔNG  hiển thị Danh sách sản phẩm theo danh mục
                            childZtree.isParent = isProduct;
                            childZtree.children = null;
                            //childZtree.open = false;
                            childZtree.click = "category('" + childCategory.CategoryId + "')";
                        }

                        dsChildTreeview.Add(childZtree);
                    }

                    zTree.children = dsChildTreeview;
                    zTree.open = true;
                    zTree.click = "category('" + item.CategoryId + "')";
                    zTree.name = item.CategoryName + " (" + dsChildCategory.Count() + ")";
                }
                else
                {
                    zTree.click = "category('" + item.CategoryId + "')";
                    zTree.open = false; // 
                    zTree.name = item.CategoryName;
                }

                if (isProduct)
                {
                    zTree.children.AddRange(ProductByCategoryGuid(item.CategoryGuid));
                    if (zTree.children.Count() > 0)
                    {
                        zTree.open = true;
                    }
                }
                //zTree.name += "(" +  zTree.children.Count() + ")";
                dsTreeView.Add(zTree);
            }
            return dsTreeView;
        }

        public List<TreeView> ProductByCategoryGuid(string CategoryGuid)
        {
            if (!string.IsNullOrWhiteSpace(CategoryGuid))
            {
                TreeView zTree = null;
                List<TreeView> dsTreeview = new List<TreeView>();

                shProductService _product = new shProductService();
                IEnumerable<shProduct> dsProduct = _product.DanhSachProduct_TheoDanhMuc(CategoryGuid);

                foreach (var item in dsProduct)
                {
                    zTree = new TreeView();

                    zTree.name = item.ProductName;
                    zTree.id = item.ProductId.ToString();
                    //zTree.pId = UnitId.ToString();
                    zTree.isParent = false;
                    zTree.children = null;
                    zTree.click = null;
                    zTree.open = true;

                    dsTreeview.Add(zTree);
                }

                return dsTreeview;
            }

            return new List<TreeView>();
        }

        #endregion


        #region Vẽ hình dạng TreeView cho danh sách Product con trong Bộ Sản phẩm
        public JsonResult ProductTreeview(bool? IsSection, string ProductGuid)
        {
            shProductService _product = new shProductService();
            shProduct product = _product.FindByKey(ProductGuid);

            if (!IsSection.HasValue)
                IsSection = false;

            if (product == null)
            {
                product = new shProduct();
            }

            TreeView tongthe = new TreeView();
            tongthe.name = product.ProductName;
            tongthe.id = product.ProductGuid;
            tongthe.pId = null;
            tongthe.open = true;
            tongthe.isParent = true;
            tongthe.click = "shProduct('" + product.ProductGuid + "', '', '', '0')";
            tongthe.children = GetProductTreeview(IsSection.Value, ProductGuid);

            return Json(tongthe, JsonRequestBehavior.AllowGet);

        }

        public List<TreeView> GetProductTreeview(bool isSection, string ProductGuid)
        {
            shSectionService _section = new shSectionService();
            shSizeService _size = new shSizeService();

            IEnumerable<shProductSet> dsSection = _section.DanhSachSection().Where(x => x.ProductGuid == ProductGuid && string.IsNullOrWhiteSpace(x.ParentId));

            List<TreeView> dsTreeview = new List<TreeView>();
            TreeView zTree = null;
            TreeView childZtree = null;

            foreach (var section in dsSection)
            {
                // cấp sản phẩm nhỏ 
                zTree = new TreeView();
                zTree.isParent = true;
                zTree.id = section.SectionGuid;
                zTree.pId = section.ProductGuid;

                List<TreeView> dsChildTreeview = null;

                //IEnumerable<shSize> dsChildSize = _size.DanhSachSize_BySectionGuid(section.SectionGuid, null, null);

                IEnumerable<shProductSet> dsChildSection = _section.DanhSachSection().Where(x => x.ParentId == section.SectionGuid).OrderBy(x =>x.SortOrder);

                if (dsChildSection.Count() > 0)
                {
                    dsChildTreeview = new List<TreeView>();

                    foreach (var childSection in dsChildSection)
                    {
                        // Cấp kích thước + Giá tiền 
                        childZtree = new TreeView();
                        childZtree.id = childSection.SectionGuid;
                        childZtree.pId = childSection.ParentId;
                        childZtree.name = childSection.SectionName;

                        childZtree.isParent = false;
                        childZtree.children = null;
                        childZtree.click = "shProduct('' , '" + childSection.SectionGuid + "', '', '1')";

                        dsChildTreeview.Add(childZtree);
                    }

                    zTree.children = dsChildTreeview;
                    zTree.open = true;
                    zTree.click = "shProduct('', '" + section.SectionGuid + "', '', '1')";
                    zTree.name = section.SectionName + "[" + dsChildSection.Count() + "]";
                }
                else
                {
                    // Chưa có Kích Thước
                    zTree.click = "shProduct('' , '" + section.SectionGuid + "', '', '1')";
                    zTree.open = false;
                    zTree.name = section.SectionName;
                }
                //zTree.name += "<a href='javascript:void(0)' onclick='Edit()'>" +
                //              "<span class='fa fa-pencil-square-o' style='transform: scale(1.3, 1.3);' title='Sửa'>" +
                //              "</span></a>";
                dsTreeview.Add(zTree);
            }

            return dsTreeview;
        }
        #endregion

    }
}
