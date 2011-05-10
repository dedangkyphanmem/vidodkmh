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
    [Persistent("Branchs")]
    public class Branch : BaseObject
    {
        public Branch(Session session) : base(session) { }

        Department fdepartment;
        [Association("Department-Branchs")]
        public Department Department
        {
            get { return fdepartment; }
            set { SetPropertyValue<Department>("Customer", ref fdepartment, value); }
        }
        [Association("Branch-Subjects", typeof(Subject))]
        public XPCollection Subjects
        {
            get { return GetCollection("Subjects"); }
        }
        [Association(("Branch-StudentClasses"), typeof(StudentClass))]
        public XPCollection StudentClasses
        {
            get { return GetCollection("StudentClasses"); }
        }


        string fBranchCode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Branch.BranchCode", DefaultContexts.Save)]
        public string BranchCode
        {
            get { return fBranchCode; }
            set { SetPropertyValue<string>("BranchCode", ref fBranchCode, value); }
        }
        string fbranchname;
        [Size(255)]
        public string BranchName
        {
            get { return fbranchname; }
            set { SetPropertyValue<string>("BranchName", ref fbranchname, value); }
        }
        
        string flevelofeducation;
        public string LevelOfEducation
        {
            get { return flevelofeducation; }
            set { SetPropertyValue<string>("levelofeducation", ref flevelofeducation, value); }
        }
        string ftypeofeducation;
        public string TypeOfEducation
        {
            get { return ftypeofeducation; }
            set { SetPropertyValue<string>("typeofeducation", ref ftypeofeducation, value); }
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
      
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
