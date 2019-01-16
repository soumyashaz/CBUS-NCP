using System.Web;
using System.Web.Optimization;

namespace CBUSA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            /*Kendo*/
            bundles.Add(new ScriptBundle("~/bundles/kendo", "http://kendo.cdn.telerik.com/2016.2.607/js/kendo.web.min.js").Include(
                     "~/Scripts/kendo/2016.2.607/kendo.web.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendoOther").Include(
                    "~/Scripts/kendo/2016.2.607/kendo.aspnetmvc.min.js",
                    "~/Scripts/kendo.modernizr.custom.js",
                    "~/Scripts/Custom/CBUSAGlobal.js"
                    ));
            //bundles.Add(new StyleBundle("~/Content/css/kendoStyle").Include(
            //       "~/Content/kendo/2016.2.607/kendo.common.min.css",
            //       "~/Content/kendo/2016.2.607/kendo.mobile.all.min.css",
            //       "~/Content/kendo/2016.2.607/kendo.dataviz.min.css",
            //       "~/Content/kendo/2016.2.607/kendo.default.min.css",
            //       "~/Content/kendo/2016.2.607/kendo.dataviz.default.min.css"
            //       ));


            bundles.Add(new StyleBundle("~/Content/css/style").Include(
                     "~/Content/css/Style.css",
                     "~/Content/css/bootstrap.css"
                     ));



            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
