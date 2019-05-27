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
    public class shProductService : BaseService<shProduct, ShopOnlineDb>
    {

        public string ProductName(string ProductGuid)
        {
            shProductService _product = new shProductService();
            if (!string.IsNullOrWhiteSpace(ProductGuid))
            {
                shProduct product = _product.FindByKey(ProductGuid);

                if (product != null)
                {
                    return product.ProductName;
                }
                return string.Empty;
            }

            return string.Empty;
        }

        #region GetListProduct
        public IEnumerable<shProduct> DanhSachProduct()
        {
            shProductService _product = new shProductService();

            return _product.FindList().OrderBy(x => x.ProductId).Where(x => x.Status == true);
        }

        public IEnumerable<shProduct> DanhSachProduct_TheoDanhMuc(string CategoryGuid)
        {
            IEnumerable<shProduct> dsProduct = DanhSachProduct();

            if (!string.IsNullOrWhiteSpace(CategoryGuid))
            {
                dsProduct = dsProduct.Where(x => x.CategoryGuid == CategoryGuid);
            }

            return dsProduct;
        }

        public IEnumerable<shProduct> DanhSachProduct_ByUserId(int? UserId)
        {
            List<shProduct> dsProduct = new List<shProduct>();
            if (UserId.HasValue)
            {
                shCategoryService _category = new shCategoryService();
                IEnumerable<shCategory> dsParent = _category.FindList().Where(x => x.UserId == UserId);
                foreach (var parent in dsParent)
                {
                    if (!string.IsNullOrEmpty(parent.ParentId) || !string.IsNullOrWhiteSpace(parent.ParentId))   // là danh mục con.. Chỉ được load danh mục con 
                    {
                        dsProduct.AddRange(DanhSachProduct_TheoDanhMuc(parent.CategoryGuid));

                    }
                    else  // là danh mục cha   => tìm các danh muc con 
                    {
                        dsProduct.AddRange(DanhSachProduct_TheoDanhMuc(parent.CategoryGuid));

                        IEnumerable<shCategory> dsChild = _category.DanhSachCategory_ByParentId(parent.CategoryGuid);
                        foreach (var child in dsChild)
                        {
                            if (child.UserId.HasValue)
                            {
                                if (child.UserId == UserId)
                                {
                                    dsProduct.AddRange(DanhSachProduct_TheoDanhMuc(child.CategoryGuid));
                                }
                            }
                            else
                            {
                                dsProduct.AddRange(DanhSachProduct_TheoDanhMuc(child.CategoryGuid));
                            }
                                
                        }
                    }

                }
            }
            else
            {
                dsProduct = DanhSachProduct().ToList();
            }

            return dsProduct.OrderBy(x => x.ProductId);
        }

        public IPagedList<shProduct> DanhSachProduct_PhanTrang(int page, int pageSize, int? UserId)
        {
            IPagedList<shProduct> pageList_dsProduct = DanhSachProduct_ByUserId(UserId).ToPagedList(page, pageSize);

            return pageList_dsProduct;
        }
        #endregion

        #region Get tophot product 
        public IEnumerable<shProduct> DanhSachTopHotProduct()
        {
            return DanhSachProduct().Where(x => x.TopHot == true);
        }
        #endregion

        #region Insert - Create
        public shProduct Inser_UpdateProduct(
            string ProductGuid,
            int ProductId,
            string CategoryGuid,
            string ProductName,
            decimal? PriceCurrent,
            string CompleteSetInclude,
            string Details,
            string UserManual,
            int? PercentCurrent,
            decimal? PriceAfterPercents,
            int? Number,
            string Image,
            int? ViewsProduct,
            int? ProductStatus,
            int? SortOrder,
            string Color,
            string Size,
            bool? Status,
            DateTime? CreateDate,
            string Description)
        {
            shProductService _product = new shProductService();
            shProduct product = new shProduct();
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                if (!string.IsNullOrWhiteSpace(ProductGuid))
                    product = _product.FindByKey(ProductGuid);
                else
                    product.ProductGuid = GuidUnique.getInstance().GenerateUnique();

                //product.ProductId = ProductId;
                product.CategoryGuid = CategoryGuid;
                product.ProductName = ProductName;
                product.PriceCurrent = PriceCurrent;
                product.CompleteSetInclude = CompleteSetInclude;
                product.Details = Details;
                product.UserManual = UserManual;
                product.PercentCurrent = PercentCurrent;
                product.PriceAfterPercents = PriceAfterPercents;
                product.Number = Number;

                product.ViewsProduct = ViewsProduct;
                product.ProductStatus = ProductStatus;
                product.SortOrder = SortOrder;
                product.Color = Color;
                product.Size = Size;
                product.Status = Status;
                product.CreateDate = CreateDate;
                product.Description = Description;

                if (!string.IsNullOrWhiteSpace(Image) || !string.IsNullOrEmpty(Image))
                {
                    product.Image = Image;
                }

                product.MetaTitle = TaoLinkUrl(CategoryGuid, ProductName);

                if (product.ProductId > 0)
                    _product.Update(product);
                else
                    _product.Insert(product);

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

            }

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryGuid">Tên controller/action cấp cao nhất(danh mục bên trên cùng) chứa danh mục của sản phẩm</param>
        /// <param name="ProductName">Tên sản phẩm rewrite thành linnk </param>
        /// <returns></returns>
        public string TaoLinkUrl(string CategoryGuid, string ProductName)
        {
            string controller = CommonHelper.GetUrlTheFirst(CategoryGuid);
            shCategoryService _category = new shCategoryService();
            string Metatitle = _category.CategoryMetaTitle(controller);

            int index = Metatitle.LastIndexOf('/');
            Metatitle = Metatitle.Substring(0, index);

            Metatitle += "/" + StringHelper.ToUnsignString(ProductName).ToLower();

            return Metatitle;
        }
        #endregion

        #region Upload ảnh đại diện sản phẩm 
        public void UploadImageProduct(string ProductGuid, string Image)
        {
            shProductService _product = new shProductService();

            shProduct product = _product.FindByKey(ProductGuid);

            product.Image = Image;

            _product.Update(product);
        }

        #endregion
        #region Cấu hình sản phẩm HightLight 
        public void HighLight(string[] cbxItem)
        {
            shProductService _product = new shProductService();
            shProduct product = new shProduct();

            foreach (var item in cbxItem)
            {
                product = _product.FindByKey(item);
                if (product != null)
                {
                    product.TopHot = true;
                    _product.Update(product);
                }
            }
        }

        public void UnSubcribeHighLight(string[] cbxItem)
        {
            shProductService _product = new shProductService();
            shProduct product = new shProduct();

            foreach (var item in cbxItem)
            {
                product = _product.FindByKey(item);
                if (product != null)
                {
                    product.TopHot = false;
                    _product.Update(product);
                }
            }
        }
        #endregion
    }
}
