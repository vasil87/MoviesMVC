using System.Web;
using System.Web.Optimization;

namespace TelerikMovies.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/data-tables").Include(
                    "~/Scripts/DataTables/jquery.dataTables.min.js",
                     "~/Scripts/DataTables/dataTables.buttons.min.js",
                    "~/Scripts/DataTables/dataTables.responsive.min.js",
                    "~/Scripts/DataTables/dataTables.select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                       "~/Scripts/materialize.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/materialize.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/data-tables-css").Include(
                    "~/Content/DataTables/css/jquery.dataTables.min.css",
                     "~/Content/DataTables/css/responsive.dataTables.min.css",
                      "~/Content/DataTables/css/select.dataTables.min.css",
                     "~/Content/DataTables/css/buttons.dataTables.min.css"));
        }
    }
}
