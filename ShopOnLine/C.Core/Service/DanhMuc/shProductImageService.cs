using C.Core.Common;
using C.Core.Model;
using C.UI.PagedList;
using C.UI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TV.DbHelper.Service;

namespace C.Core.Service
{
    public class shProductImageService : BaseService<shProductImage, ShopOnlineDb>
    {

        public IEnumerable<shProductImage> DanhSachProductImage()
        {
            shProductImageService _productImage = new shProductImageService();
            return _productImage.FindList().Where(x => x.Status == true)
                                            .OrderBy(x => x.ProductImageId);

        }

        public IEnumerable<shProductImage> DanhSachProductImage_ByCategory(string ProductGuid, string ProductImageCategory)
        {
            shProductImageService _productImage = new shProductImageService();
            IEnumerable<shProductImage> ds = DanhSachProductImage_ByProductGuid(ProductGuid).Where(x => x.ProductImageCategory == ProductImageCategory);
            return ds;
        }

        public IEnumerable<shProductImage> DanhSachProductImage_ByProductGuid(string ProductGuid)
        {
            shProductImageService _productImage = new shProductImageService();
            return _productImage.FindList().Where(x => x.Status == true && x.ProductGuid == ProductGuid)
                                            .OrderBy(x => x.ProductImageId);

        }

        public shProductImage Insert_UpdateProductImage(
            string ProductImageGuid,
            int? ProductImageId,
            string ProductGuid,
            string FileName,
            string FileNameGoc,
            int? UserId,
            bool? Status,
            DateTime? CreateDate,
            string ProductImageCategory,
            string Image)
        {
            shProductImageService _productImage = new shProductImageService();
            shProductImage productImage = new shProductImage();

            if (!string.IsNullOrWhiteSpace(ProductImageGuid))
                productImage = _productImage.FindByKey(ProductImageGuid);
            else
                productImage.ProductImageGuid = GuidUnique.getInstance().GenerateUnique();

            //productImage.ProductImageId = ProductImageId;
            productImage.ProductGuid = ProductGuid;
            productImage.FileName = FileName;
            productImage.FileNameGoc = FileNameGoc;
            productImage.UserId = UserId;
            productImage.Status = Status;
            productImage.CreateDate = CreateDate;

            productImage.ProductImageCategory = ProductImageCategory;
            productImage.Image = Image;

            if (productImage.ProductImageId > 0)
                _productImage.Update(productImage);
            else
                _productImage.Insert(productImage);

            return productImage;
        }

        public void InsertAllImageProduct(
            string ProductGuid,
            int? UserId,
            List<ProductMultiUpload> dsDesignImage,
            List<ProductMultiUpload> dsMaterialImage,
            string folderUpload,
            string MapPath,
            bool? Status,
            DateTime? CreateDate)
        {
            foreach (var item in dsDesignImage)
            {
                Insert_UpdateProductImage(
                    null,
                    null,
                    ProductGuid,
                    item.value,
                    item.key,
                    UserId,
                    Status,
                    CreateDate,
                    Config.ProductImageCategory_Design,
                    null
                    );

            }

            foreach (var item in dsMaterialImage)
            {
                Insert_UpdateProductImage(
                       null,
                       null,
                       ProductGuid,
                       item.value,
                       item.key,
                       UserId,
                       Status,
                       CreateDate,
                       Config.ProductImageCategory_Material,
                    null
                       );
            }
        }

        public void DeleteAllImageProduct(string ProductImageGuid)
        {
            DeleteProductImage(ProductImageGuid.Split(';'));
        }

        public void DeleteProductImage(string[] ListImage)
        {
            shProductImageService _productImage = new shProductImageService();
            shProductImage productImage = new shProductImage();

            foreach (var item in ListImage)
            {
                if (!string.IsNullOrEmpty(item) || !string.IsNullOrWhiteSpace(item))
                {
                    //productImage = _productImage.FindByKey(item);
                    _productImage.Delete(item);

                }
            }
        }
    }
}
