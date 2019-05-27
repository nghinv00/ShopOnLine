using C.Core.Common;
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
    public class shCommentService : BaseService<shComment, ShopOnlineDb>
    {

        #region List Ds Comment 
        public IEnumerable<shComment> DanhSachComment(bool? Status)
        {
            shCommentService _comment = new shCommentService();

            IEnumerable<shComment> ds = _comment.FindList().OrderByDescending(x => x.CreatedDate);

            if (Status.HasValue)
            {
                ds = ds.Where(x => x.Status == Status);
            }
            return ds;
        }
        #endregion


        #region List Ds Comment By ProductGuid
        public IEnumerable<shComment> DanhSachComment_ByProductGuid(string ProductGuid, bool? Status)
        {
            shCommentService _comment = new shCommentService();

            IEnumerable<shComment> dsComment = DanhSachComment(Status);

            if (!string.IsNullOrEmpty(ProductGuid) || !string.IsNullOrWhiteSpace(ProductGuid))
            {
                dsComment = dsComment.Where(x => x.ProductGuid == ProductGuid);
            }

            return dsComment;
        }
        #endregion

        #region List Ds Comment By ProductGuid Phân trang 
        public IPagedList<shComment> DanhSachComment_ByProductGuid_PhanTrang(string ProductGuid, int page, int PageSize, bool Status)
        {
            return DanhSachComment_ByProductGuid(ProductGuid, Status).ToPagedList(page, PageSize);
        }
        #endregion

        #region Insert - Update Comment 
        public shComment Insert_UpdateComment(
            string CommentGuid,
            int? CommentId,
            string MemberGuiId,
            string ProductGuid,
            string Email,
            string MemberName,
            int? Rating,
            string Contents,
            bool? Status,
            DateTime? CreateDate)
        {

            shCommentService _comment = new shCommentService();
            shComment comment = new shComment();

            if (!string.IsNullOrEmpty(CommentGuid) || !string.IsNullOrWhiteSpace(CommentGuid))
                comment = _comment.FindByKey(CommentGuid);
            else
                comment.CommentGuid = GuidUnique.getInstance().GenerateUnique();

            comment.MemberGuiId = MemberGuiId;
            comment.ProductGuid = ProductGuid;
            comment.Email = Email;
            comment.MemberName = MemberName;
            comment.Rating = Rating;
            comment.Contents = Contents;
            comment.Status = Status;
            comment.CreatedDate = CreateDate;

            if (comment.CommentId > 0)
                _comment.Update(comment);
            else
                _comment.Insert(comment);

            return comment;
        }
        #endregion

    }
}
