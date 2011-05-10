using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("Departments")]
    public class Department : BaseObject
    {
        [Association("Department-Branchs", typeof(Branch))]
        public XPCollection Branchs
        {
            get { return GetCollection("Branchs"); }
        }
        
        [Association("Department-Teachers", typeof(Teacher))]
        public XPCollection Teachers
        {
            get { return GetCollection("Teachers"); }
        }
        
        string fdepartmentcode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Department.DepartmentCode", DefaultContexts.Save)]
        public string DepartmentCode
        {
            get { return fdepartmentcode; }
            set { SetPropertyValue<string>("departmentcode", ref fdepartmentcode, value); }
        }
        string fname;
        [Size(255)]
        public string DepartmentName
        {
            get { return fname; }
            set { SetPropertyValue<string>("DepartmentName", ref fname, value); }
        }
        string fphone;
        [Size(50)]
        public string Phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string femail;
        [Size(255)]
        public string Email
        {
            get { return femail; }
            set { SetPropertyValue<string>("email", ref femail, value); }
        }
        string fnote;
        [Size(500)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated = DateTime.MinValue;
        public DateTime DateCreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("DateCreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime DateModified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("DateModified", ref fdatemodified, value); }
        }
        public Department(Session session) : base(session) { }
        //public department() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
