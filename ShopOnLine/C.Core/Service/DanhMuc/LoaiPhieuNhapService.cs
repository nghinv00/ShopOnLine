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
    public class LoaiPhieuNhapService : BaseService<LoaiPhieuNhap, ShopOnlineDb>
    {
        public IEnumerable<LoaiPhieuNhap> DanhSachPhieuNhap(int LoaiPhieuNhapXuat)
        {
            LoaiPhieuNhapService _loai = new LoaiPhieuNhapService();

            return _loai.FindList().Where(x => x.LoaiPhieuNhapXuat == LoaiPhieuNhapXuat);
        }

    }
}
