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
    public class ThongBaoService : BaseService<ThongBao, ShopOnlineDb>
    {
        #region Ds
        public IEnumerable<ThongBao> DanhSach(int? MemberId)
        {
            ThongBaoService _thongbao = new ThongBaoService();
            return _thongbao.FindList().Where(x => x.NguoiNhanThongBaoId == MemberId);
        }

        public IPagedList<ThongBao> DanhSach_PhanTrang(int? MemberId, int pageCurrent, int pageSize)
        {
            return DanhSach(MemberId).ToPagedList(pageCurrent, pageSize);
        }
        #endregion

        #region Insert - Update
        public ThongBao InsertOrUpdate(int? ThongBaoId, string NoiDungThongBao, string NoiDungCongViec, string LienKet, int? NguoiThongBaoID, int? NguoiNhanThongBaoId, DateTime? ThoiGian, bool? DaXem, string MaLoaiThongBao, int? GiaTri)
        {
            ThongBaoService _thongbao = new ThongBaoService();

            ThongBao thongbao = new ThongBao();

            if (ThongBaoId.HasValue)
            {
                thongbao = _thongbao.FindByKey(ThongBaoId);
                thongbao = thongbao == null ? new ThongBao() : thongbao;
            }
            thongbao.NoiDungThongBao = NoiDungThongBao;
            thongbao.NoiDungCongViec = NoiDungCongViec;

            thongbao.LienKet = LienKet;
            thongbao.NguoiThongBaoID = NguoiThongBaoID;
            thongbao.NguoiNhanThongBaoId = NguoiNhanThongBaoId;
            thongbao.ThoiGian = ThoiGian;
            thongbao.DaXem = DaXem;
            thongbao.MaLoaiThongBao = MaLoaiThongBao;
            thongbao.GiaTri = GiaTri;

            if (thongbao.ThongBaoId > 0)
            {
                _thongbao.Update(thongbao);
            }
            else
            {
                _thongbao.Insert(thongbao);
            }

            return thongbao;
        }
        #endregion

    }
}
