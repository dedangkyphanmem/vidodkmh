using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using vidoSolution.Module;
using DevExpress.Persistent.AuditTrail;
using System.Security.Principal;
using vidoSolution.Module.DomainObject;

namespace vidoSolution.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if EASYTEST
			DevExpress.ExpressApp.EasyTest.WinAdapter.RemotingRegistration.Register(4100);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            vidoWinSolutionWindowsFormsApplication winApplication = new vidoWinSolutionWindowsFormsApplication();
            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Full;

            //Subscribe to QueryCurrentUserName event of the AuditTrailService's instance 
            AuditTrailService.Instance.QueryCurrentUserName +=
               new QueryCurrentUserNameEventHandler(Instance_QueryCurrentUserName);
            AuditTrailService.Instance.CustomizeAuditTrailSettings +=
                new CustomizeAuditSettingsEventHandler(Instance_CustomizeAuditTrailSettings);

#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try {
                winApplication.Setup();
                winApplication.Start();
            } catch (Exception e) {
                winApplication.HandleException(e);
            }
        }
        static void Instance_QueryCurrentUserName(object sender, QueryCurrentUserNameEventArgs e)
        {
            e.CurrentUserName = WindowsIdentity.GetCurrent().Name;
        }
        static void Instance_CustomizeAuditTrailSettings(object sender, CustomizeAuditTrailSettingsEventArgs e)
        {
            e.AuditTrailSettings.Clear();
           
            e.AuditTrailSettings.AddType(typeof(AccountTransaction));
            e.AuditTrailSettings.AddType(typeof(Branch));
            e.AuditTrailSettings.AddType(typeof(Classroom));
            e.AuditTrailSettings.AddType(typeof(Department));
            e.AuditTrailSettings.AddType(typeof(Lesson));
            e.AuditTrailSettings.AddType(typeof(Office));
            e.AuditTrailSettings.AddType(typeof(RegisterDetail));
            e.AuditTrailSettings.AddType(typeof(RegisterTime));
            e.AuditTrailSettings.AddType(typeof(Semester));
            e.AuditTrailSettings.AddType(typeof(Student));
            e.AuditTrailSettings.AddType(typeof(Person));
            e.AuditTrailSettings.AddType(typeof(User));
            e.AuditTrailSettings.AddType(typeof(StudentAccumulation));
            e.AuditTrailSettings.AddType(typeof(StudentClass));
            e.AuditTrailSettings.AddType(typeof(StudentResult));
            e.AuditTrailSettings.AddType(typeof(Subject));
            e.AuditTrailSettings.AddType(typeof(SubjectRelations));
            e.AuditTrailSettings.AddType(typeof(Teacher));
        }
    }
}
