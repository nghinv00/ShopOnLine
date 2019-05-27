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
    public class qtPositionService : BaseService<qtPosition, ShopOnlineDb>
    {

        public string PositionName(int? PositionId)
        {
            string PositionName = string.Empty;
            if (PositionId.HasValue)
            {
                qtPositionService _position = new qtPositionService();
                qtPosition position = _position.FindByKey(PositionId);
                if (position != null)
                    return position.PositionName;
                return PositionName;
            }
            return PositionName;
        }


        #region Get List Unit
        public IEnumerable<qtPosition> DanhSachPosition()
        {
            qtPositionService _position = new qtPositionService();
            IEnumerable<qtPosition> dsPosition = _position.FindList();

            return dsPosition;
        }

        public IPagedList<qtPosition> DanhSachPosition_PhanTrang(int page, int pageSize)
        {
            IPagedList<qtPosition> pageList_dsPosition = DanhSachPosition().ToPagedList(page, pageSize);
            return pageList_dsPosition;

        }
        #endregion

        #region Insert - Update qtPosition
        public qtPosition ThemMoi_HieuChinhPosition(int PositionId, string PositionName,
            string Code, int? SortOrder, bool? Status, DateTime? DateCreated)
        {
            qtPositionService _position = new qtPositionService();

            qtPosition position = new qtPosition();

            if (PositionId > 0)
                position = _position.FindByKey(PositionId);

            position.PositionName = PositionName;
            position.Code = Code;
            position.SortOrder = SortOrder;
            position.Status = Status;
            position.CreateDate = DateCreated;

            if (position.PositionId > 0)
                _position.Update(position);
            else
                _position.Insert(position);

            return position;
        }
        #endregion

    }
}
