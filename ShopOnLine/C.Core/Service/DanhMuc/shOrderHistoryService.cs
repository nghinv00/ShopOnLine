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
    public class shOrderHistoryService : BaseService<shOrderHistory, ShopOnlineDb>
    {


        public shOrderHistory Insert_Update(int? Id, string OrderGuid, int? OrderStatus, string MemberGuid, string Description, int? UserId, bool? Status, DateTime? CreateDate)
        {
            shOrderHistoryService _orderHistory = new shOrderHistoryService();
            shOrderHistory orderHistory = new shOrderHistory();
            orderHistory.OrderGuid = OrderGuid;
            orderHistory.OrderStatus = OrderStatus;
            orderHistory.MemberGuid = MemberGuid;
            orderHistory.Description = Description;
            orderHistory.UserId = UserId;
            orderHistory.Status = Status;
            orderHistory.CreateDate = CreateDate;

            _orderHistory.Insert(orderHistory);

            return orderHistory;
        }

    }
}
