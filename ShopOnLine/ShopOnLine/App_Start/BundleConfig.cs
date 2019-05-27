using System.Web;
using System.Web.Optimization;

namespace ShopOnLine
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery/jquery.validate*"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));



            // C.ShopOnline
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/AdminLTE/bootstrap/css/bootstrap.css",
                      "~/Content/jquery-ui/jquery-ui.css",
                      "~/font-awesome/css/font-awesome.min.css",                      
                      "~/Content/Site.css"
                      ));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                      "~/AdminLTE/ionicons/ionicons.min",
                      "~/AdminLTE/dist/css/AdminLTE.css",
                      "~/AdminLTE/plugins/select2/select2.css",
                      "~/AdminLTE/plugins/iCheck/all.css",
                      "~/AdminLTE/dist/css/skins/_all-skins.min.css",
                      "~/AdminLTE/plugins/iCheck/flat/blue.css",
                      "~/AdminLTE/plugins/morris/morris.css"
                    ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/AdminLTE/plugins/jQuery/jquery-2.2.3.min.js",
                      "~/Scripts/jquery/jquery.validate.js",
                      "~/Scripts/jquery/additional-methods.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/AdminLTE/plugins/jQueryUI/jquery-ui.min.js",
                      "~/AdminLTE/bootstrap/js/bootstrap.min.js",
                      "~/AdminLTE/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/AdminLTE/plugins/fastclick/fastclick.min.js",
                      "~/AdminLTE/plugins/select2/select2.full.min.js",
                      "~/AdminLTE/plugins/iCheck/icheck.min.js",
                      "~/AdminLTE/dist/js/app.js",
                      "~/AdminLTE/dist/js/demo.js",
                      "~/Scripts/Site.js",
                      "~/Scripts/jquery/jquery.selectboxes.js"
                    //"~/Scripts/jQuery-slimScroll-1.3.8/jquery.slimscroll.min.js"
                    ));

            //thu vien js kendo
            bundles.Add(new ScriptBundle("~/js/kendo").Include(
                  "~/Scripts/kendo/kendo.all.min.js"
                    ));


            // Treeview
            bundles.Add(new StyleBundle("~/Treeview/css").Include(
                     "~/AdminLTE/ztree/css/zTreeStyle/zTreeStyle.css"
                     ));
            bundles.Add(new ScriptBundle("~/Treeview/js").Include(
                 "~/AdminLTE/ztree/js/jquery.ztree.core.min.js"
                 ));


            // fron-end 


            BundleTable.EnableOptimizations = true;
        }
    }
}
