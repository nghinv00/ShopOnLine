using C.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopOnLine
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Url Seo

            #region Home page
            //create new route   Trang chủ 
            routes.MapRoute(
              name: "HomePage",
              url: "trang-chu",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region sản phẩm 

            #region Đệm
            //create new route  I.
            routes.MapRoute(
              name: "Products",
              url: "san-pham",
              defaults: new { controller = "Dem", action = "Index", id = UrlParameter.Optional }
            );

            //create new route    con 1.1
            routes.MapRoute(
              name: "Product_Dem",
              url: "{id}/dem/{metatile}",
              defaults: new { controller = "Dem", action = "Index", id = UrlParameter.Optional }
            );

            //create new route   1.2
            routes.MapRoute(
              name: "Product_Dem_Title",
              url: "dem/{metatile}-{id}",
              defaults: new { controller = "Dem", action = "Details", id = UrlParameter.Optional }
            );

            #endregion

            #region Bộ chăn ga gối
            //create new route  2
            routes.MapRoute(
              name: "Product_BoChanGaGoi",
              url: "{id}/bo-chan-ga-goi/{metatile}",
              defaults: new { controller = "BoChanGaGoi", action = "Index", id = UrlParameter.Optional }
            );

            //create new route   2.1
            routes.MapRoute(
              name: "Product_BoChanGaGoi_Title",
              url: "bo-chan-ga-goi/{metatile}-{id}",
              defaults: new { controller = "BoChanGaGoi", action = "Details", id = UrlParameter.Optional }
            );
            #endregion

            #region Chăn đông - ruột gối
            //create new route  3.1
            routes.MapRoute(
              name: "Product_ChanDongRuotGoi",
              url: "{id}/chan-dong-ruot-goi/{metatile}",
              defaults: new { controller = "ChanDongRuotGoi", action = "Index", id = UrlParameter.Optional }
            );

            //create new route   3.3
            routes.MapRoute(
              name: "Product_ChanDongRuotGoi_Title",
              url: "chan-dong-ruot-goi/{metatile}-{id}",
              defaults: new { controller = "ChanDongRuotGoi", action = "Details", id = UrlParameter.Optional }
            );
            #endregion

            #region Dự án
            //create new route   4
            routes.MapRoute(
              name: "Product_DuAn",
              url: "{id}/du-an/{metatile}",
              defaults: new { controller = "DuAn", action = "Index", id = UrlParameter.Optional }
            );

            //create new route 4.1
            routes.MapRoute(
              name: "Product_DuAn_Title",
              url: "du-an/{metatile}-{id}",
              defaults: new { controller = "DuAn", action = "Details", id = UrlParameter.Optional }
            );
            #endregion

            #endregion

            #region Giới thiệu 
            //create new route   Giới thiệu
            routes.MapRoute(
              name: "AboutUs",
              url: "gioi-thieu",
              defaults: new { controller = "AboutUs", action = "Index", id = UrlParameter.Optional }
            );

            //create new route   Giới thiệu
            routes.MapRoute(
              name: "AboutUs-Metatitle",
                 url: "gioi-thieu/{metatile}-{id}",
              defaults: new { controller = "AboutUs", action = "Details", id = UrlParameter.Optional }
            );

            #endregion

            #region Đại lý
            //create new route  Đại lý
            routes.MapRoute(
              name: "Agent",
              url: "dai-ly",
              defaults: new { controller = "Agent", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region Bảng giá
            //create new route  Bảng giá
            routes.MapRoute(
              name: "PriceList",
              url: "bang-gia",
              defaults: new { controller = "PriceList", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region Tin tức - sự kiện
            //create new route   Tin tức - sự kiện
            routes.MapRoute(
              name: "News",
              url: "tin-tuc",
              defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
               name: "News-Details",
               url: "tin-tuc/{metatile}-{id}",
               defaults: new { controller = "News", action = "Details", id = UrlParameter.Optional }
             );
            #endregion

            #region Liên hệ
            //create new route  Liên hệ
            routes.MapRoute(
              name: "Contact",
              url: "lien-he",
              defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region Giỏ hàng
            //create new route  Giỏ hàng
            routes.MapRoute(
              name: "AddCart",
              url: "gio-hang",
              defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region Cảm nhận khách hàng - ý kiến khách hàng
            //create new route  Cảm nhận khách hàng - ý kiến khách hàng
            routes.MapRoute(
              name: "YKien",
              url: "y-kien",
              defaults: new { controller = "Service", action = "Index", id = UrlParameter.Optional }
            );
            #endregion 
            #region Tìm kiếm nâng cao         
            routes.MapRoute(
              name: "TimKiemNangCao",
              url: "tim-kiem",
              defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #endregion

            #region Đăng nhập, Đăng xuất
            //create new route  Login
            routes.MapRoute(
              name: "Login",
              url: "dang-nhap",
              defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            // create new route Register 
            routes.MapRoute(
             name: "Register",
             url: "dang-ky",
             defaults: new { controller = "Login", action = "Register", id = UrlParameter.Optional }

           );
            // create new route Logout 
            routes.MapRoute(
             name: "Logout",
             url: "dang-xuat",
             defaults: new { controller = "Login", action = "Logout", id = UrlParameter.Optional }
           );

            // thong-tin-ca-nhan
            routes.MapRoute(
               name: "ThongTinCaNhan",
               url: "thong-tin-ca-nhan",
               defaults: new { controller = "Login", action = "Info", id = UrlParameter.Optional }
            );
            #endregion

            #region Url Default 
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopOnLine.Controllers" }
            );
            #endregion
        }
    }
}
