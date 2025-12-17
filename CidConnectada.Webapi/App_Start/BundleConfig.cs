using System.Web.Optimization;

namespace CidConnectada.Webapi
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/pi").Include(
                 "~/Scripts/pi/pi.js",
                 "~/Scripts/pi/app.js",
                 "~/Scripts/pi/app-security.js",
                 "~/Scripts/pi/report.js",
                 "~/Scripts/pi/pi-util.js",
                 "~/Scripts/pi/pi-dialog.js",
                 "~/Scripts/pi/pi-cover.js"
             //"~/Scripts/jquery.chromatable.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
            "~/Scripts/knockout-{version}.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquerymask")
                .Include("~/Scripts/jquery.maskedinput*",
                        "~/Scripts/jquery.price_format*"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                //"~/Scripts/jquery.contextMenu.js",
                //"~/Scripts/jquery.contextMenu.min.js",
                "~/Scripts/globalize/globalize.js",
                "~/Scripts/globalize/cultures/globalize.culture.pt-BR.js"
                )
            );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
