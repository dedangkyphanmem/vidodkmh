using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.AuditTrail;
using vidoSolution.Module.DomainObject;

namespace vidoSolution.Web
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e)
        {
            WebApplication.OldStyleLayout = false;
            //WebApplication.DefaultPage = "DefaultVertical.aspx";  
            

#if EASYTEST
			DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
			ConfirmationsHelper.IsConfirmationsEnabled = false;
#endif

        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            WebApplication.SetInstance(Session, new vidoSolutionAspNetApplication());
           // WebApplication.Instance.LoggedOn += new EventHandler<LogonEventArgs>(Instance_LoggedOn);
            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Full;
            AuditTrailService.Instance.QueryCurrentUserName += new QueryCurrentUserNameEventHandler(Instance_QueryCurrentUserName);
            AuditTrailService.Instance.CustomizeAuditTrailSettings += new CustomizeAuditSettingsEventHandler(Instance_CustomizeAuditTrailSettings);
            //WebApplication.DefaultPage = "DefaultVertical.aspx";  
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
           
        }

       
        private void Instance_QueryCurrentUserName(object sender, QueryCurrentUserNameEventArgs e)
        {            
            e.CurrentUserName =
                String.Format("Web user ({0}-{1})", HttpContext.Current.Request.UserHostAddress, WebApplication.Instance.Security.UserName);
        }

        static void Instance_CustomizeAuditTrailSettings(object sender,    CustomizeAuditTrailSettingsEventArgs e)
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

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string filePath = HttpContext.Current.Request.PhysicalPath;
            if (!string.IsNullOrEmpty(filePath)
                && (filePath.IndexOf("Images") >= 0) && !System.IO.File.Exists(filePath))
            {
                HttpContext.Current.Response.End();
            }
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e)
        {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
