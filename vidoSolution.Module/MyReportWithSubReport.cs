using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Reports;
using System.Drawing.Design;
using DevExpress.XtraReports.UI;
using DevExpress.Xpo;

namespace MainDemo.Module {
    public class MyReportWithSubreports : XafReport { }

    public class CustomReportData : ReportData {
        public CustomReportData(Session session) : base(session) { }
        protected override XafReport CreateReport() {
            MyReportWithSubreports report = new MyReportWithSubreports();
            report.DesignerLoaded += new DevExpress.XtraReports.UserDesigner.DesignerLoadedEventHandler(report_DesignerLoaded);
            return report;
        }
        void report_DesignerLoaded(object sender, DevExpress.XtraReports.UserDesigner.DesignerLoadedEventArgs e) {
            IToolboxService ts = (IToolboxService)e.DesignerHost.GetService(typeof(IToolboxService));
            ts.AddToolboxItem(new ToolboxItem(typeof(XRSubreport)));
        }
    }
}
