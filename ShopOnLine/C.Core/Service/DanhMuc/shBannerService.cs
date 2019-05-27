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
    public class shBannerService : BaseService<shBanner, ShopOnlineDb>
    {

        #region Ds
        public IEnumerable<shBanner> DanhSachBanner()
        {
            shBannerService _banner = new shBannerService();
            return _banner.FindList().OrderBy(x => x.SortOrder);
        }

        public IPagedList<shBanner> DanhSachBanner_PhanTrang(int pageCurrent, int pageSize)
        {
            IEnumerable<shBanner> dsBanner = DanhSachBanner();

            return dsBanner.ToPagedList(pageCurrent, pageSize);
        }

        public shBanner DanhSachBanner_ByPositionBanner(int PositionBanner)
        {
            shBannerService _banner = new shBannerService();
            shBanner banner = _banner.FindList().Where(x => x.PositionBanner == PositionBanner).FirstOrDefault();
            return banner;
        }
        #endregion

        #region Insert - Update 
        public shBanner Insert_Update(string BannerGuid, int? BannerId, string BannerName, string Url, int? PositionBanner, int? SortOrder, bool? Status, DateTime? CreateDate)
        {
            shBannerService _banner = new shBannerService();
            shBanner banner = new shBanner();

            if (!string.IsNullOrWhiteSpace(BannerGuid) || !string.IsNullOrEmpty(BannerGuid))
            {
                banner = _banner.FindByKey(BannerGuid);
            }
            else
            {
                banner.BannerGuid = GuidUnique.getInstance().GenerateUnique();
            }

            banner.BannerName = BannerName;
            banner.Url = Url;
            banner.PositionBanner = PositionBanner;
            banner.SortOrder = SortOrder;
            banner.Status = Status;
            banner.CreatedDate = CreateDate;

            if (banner.BannerId > 0)
                _banner.Update(banner);
            else
                _banner.Insert(banner);

            return banner;
        }
        #endregion


    }
}
