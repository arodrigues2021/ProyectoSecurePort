using System.Web.Optimization;

namespace SecurePort.Web
{

    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/secureport/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js",
                            "~/Scripts/secureport/plugins/bootstrap/js/bootstrap.min.js",
                            "~/Scripts/secureport/plugins/iCheck/jquery.icheck.min.js",
                            "~/Scripts/secureport/plugins/blockUI/jquery.blockUI.js",
                            "~/Scripts/secureport/plugins/perfect-scrollbar/src/jquery.mousewheel.js",
                            "~/Scripts/secureport/plugins/perfect-scrollbar/src/perfect-scrollbar.js",
                            "~/Scripts/secureport/main.js",
                            "~/Scripts/AppSecureport/alert.js",
                            "~/Scripts/AppSecureport/SecurePort.js",
                            "~/Scripts/secureport/form-elements.js",
                            "~/Scripts/secureport/plugins/jquery-validation/dist/jquery.validate.min.js",
                            "~/Scripts/secureport/plugins/flot/jquery.flot.js",
                            "~/Scripts/secureport/plugins/flot/jquery.flot.pie.js",
                            "~/Scripts/secureport/plugins/flot/jquery.flot.resize.min.js",
                            "~/Scripts/secureport/plugins/jquery.sparkline/jquery.sparkline.js",
                            "~/Scripts/secureport/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js",
                            "~/Scripts/secureport/plugins/jquery-ui-touch-punch/jquery.ui.touch-punch.min.js",
                            "~/Scripts/secureport/plugins/jquery-ui/jquery-ui-1.10.1.custom.min.js",
                            "~/Scripts/secureport/plugins/jquery-inputlimiter/jquery.inputlimiter.1.3.1.min.js",
                            "~/Scripts/secureport/plugins/autosize/jquery.autosize.min.js",
                            "~/Scripts/secureport/plugins/select2/select2.min.js",
                            "~/Scripts/secureport/plugins/jquery.maskedinput/src/jquery.maskedinput.js",
                            "~/Scripts/secureport/plugins/jQuery-Tags-Input/jquery.tagsinput.js",
                            "~/Scripts/secureport/plugins/summernote/build/summernote.min.js",
                            "~/Scripts/secureport/plugins/bootstrap-modal/js/bootstrap-modal.js",
                            "~/Scripts/secureport/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                            "~/Scripts/secureport/ui-modals.js",
                            "~/Scripts/secureport/plugins/colorbox/jquery.colorbox-min.js",
                            "~/Scripts/secureport/pages-gallery.js",
                            "~/Scripts/secureport/form-validation.js",
                            "~/Scripts/secureport/formEdit-validation.js",
                            "~/Scripts/jquery.utilidades_pre",
                            "~/Scripts/secureport/plugins/jquery.pulsate/jquery.pulsate.min.js",
                            "~/Scripts/secureport/ui-elements.js",
                            "~/Scripts/secureport/plugins/ladda-bootstrap/dist/spin.min.js",
                            "~/Scripts/secureport/plugins/ladda-bootstrap/dist/ladda.min.js",
                            "~/Scripts/secureport/plugins/ladda-bootstrap/js/prism.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/_jquery").Include(
                                "~/Scripts/jquery-1.4.4.min.js",
                                "~/Scripts/jquery.unobtrusive-ajax.js",
                                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                                "~/Scripts/jquery.validate.min.js",
                                "~/Scripts/jquery.validate.unobtrusive.js"
                                ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/uploadify").Include(
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/vendor/jquery.ui.widget.js",
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/jquery.fileupload.js",
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/jquery.fileupload-process.js",
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/jquery.fileupload-image.js",
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/jquery.fileupload-validate.js",
                            "~/Scripts/SecurePort/plugins/jQuery-File-Upload/js/jquery.fileupload-ui.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                             "~/Content/css/main-responsive.css",
                             "~/Content/css/css/base.css",
                             "~/Content/css/style.css",
                             "~/Content/css/main.css",
                             "~/Content/css/loading.css"));

            bundles.Add(new ScriptBundle("~/bundles/Telerik").Include(
                        "~/Scripts/Telerik/kendo.all.min.js",
                        "~/Scripts/Telerik/kendo.aspnetmvc.min.js",
                        "~/Scripts/Telerik/kendo.timezones.min.js",
                        "~/Scripts/Telerik/kendo.timepicker.min.js",
                        "~/Scripts/kendo.culture.es-ES.min.js"));
            
            bundles.Add(new StyleBundle("~/Content/Telerik/css").Include(
                        "~/Content/Telerik/web/kendo.common.min.css",
                        "~/Content/Telerik/web/kendo.rtl.min.css",
                        "~/Content/Telerik/web/kendo.default.min.css"
                    ));


        }
    }
}

