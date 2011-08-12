using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Configuration;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using vidoSolution.Module.DomainObject;

namespace VidoWebServices
{
    /// <summary>
    /// Summary description for VidoWebService
    /// </summary>
    [WebService(Namespace = "http://dkmh.vido.edu.vn/VidoWebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VidoWebService : System.Web.Services.WebService
    {
        public VidoWebService()
        {
            XPDictionary dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(typeof(SimpleUser).Assembly);
            dict.GetDataStoreSchema(typeof(Student).Assembly);
            //We use the demo connection string here.
            string defaultConnectionString = "";
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                defaultConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //Usually, your web service should not update the database. The line below is just for demo purposes.
            IDataStore store = DevExpress.Xpo.XpoDefault.GetConnectionProvider(defaultConnectionString,
                DevExpress.Xpo.DB.AutoCreateOption.None);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dict, store);
            XpoDefault.Session = null;
        }
        
        [WebMethod]
        public string FindStudentID(string userName)
        {
            string result = "OK";
            try
            {
                using (UnitOfWork session = new UnitOfWork(XpoDefault.DataLayer))
                {
                    
                    Student s = session.FindObject<Student>(new BinaryOperator("StudentCode",userName));
                    
                    if (s!=null)
                        result = s.Oid.ToString();
                    else
                    {
                        result = "No student found!!!";
                    }
                }
            }
            catch
            {
                result = "Error";
            }
            return result;
        }
        [WebMethod]
        public string Login(string userName, string passWord)
        {
            string result = "OK";
            try
            {
                using (UnitOfWork session = new UnitOfWork(XpoDefault.DataLayer))
                {

                    User s = session.FindObject<User>(new BinaryOperator("UserName", userName));

                    if (s != null && s.ComparePassword(passWord))
                        result = string.Format("{0}({1})",s.FullName,s.Oid);
                    else
                    {
                        result = "No user found!!!";
                    }
                }
            }
            catch
            {
                result = "Error";
            }
            return result;
        }
    }
}
