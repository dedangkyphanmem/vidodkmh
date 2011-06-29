using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using vidoSolution.Module.ReportParameter;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("StudentClasses")]
    public class StudentClass : BaseObject
    {
        Branch fBranch;
        [Association("Branch-StudentClasses")]
        public Branch Branch
        {
            get { return fBranch; }
            set { SetPropertyValue<Branch>("Branch", ref fBranch, value); }
        }

        [Association("StudentClass-ClassTransactionTrackings", typeof(ClassTransactionTracking))]
        public XPCollection ClassTransactionTrackings
        {
            get { return GetCollection("ClassTransactionTrackings"); }
        }
        [Association("StudentClasses-WeekReportDatas", typeof(WeekReportData))]
        public XPCollection WeekReportDatas
        {
            get { return GetCollection("WeekReportDatas"); }
        }
        [Association(("StudentClass-Students"),typeof(Student))]
        public XPCollection Students
        {
            get { return GetCollection("Students"); }
        }

        [Association("StudentClasses-RegisterTimes", typeof(RegisterTime))]
        public XPCollection RegisterTimes
        {
            get { return GetCollection("RegisterTimes"); }
        }

        
        string fclasscode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Class.ClassCode", DefaultContexts.Save)]
        public string ClassCode
        {
            get { return fclasscode; }
            set { SetPropertyValue<string>("classcode", ref fclasscode, value); }
        }
        string fclassname;
        public string ClassName
        {
            get { return fclassname; }
            set { SetPropertyValue<string>("classname", ref fclassname, value); }
        }
       
        Semester fenrollsemester;      
		[Association("Enrollsemester-StudentClasses")]
        public Semester EnrollSemester
        {
            get { return fenrollsemester; }
            set { SetPropertyValue<Semester>("EnrollSemester", ref fenrollsemester, value); }
        }
        Semester fgraduatesemester;
        [Association("GraduateSemester-StudentClasses")]
        public Semester GraduateSemester
        {
            get { return fgraduatesemester; }
            set { SetPropertyValue<Semester>("GraduateSemester", ref fgraduatesemester, value); }
        }
		
        int fnumstudent;
        public int NumStudent
        {
            get { return fnumstudent; }
            set { SetPropertyValue<int>("numstudent", ref fnumstudent, value); }
        }

        DateTime? fdatecreated = null;
        public DateTime? DateCreated
        {
            get
            {
                return fdatecreated;
            }
            set
            {
                SetPropertyValue<DateTime?>("DateCreated", ref fdatecreated, value);
            }
        }
        DateTime fdatemodified;
        public DateTime DateModified
        {
            get
            {
                return fdatemodified;
            }
            set
            {
                SetPropertyValue<DateTime>("DateModified", ref fdatemodified, value);
            }
        }
        protected override void OnSaving()
        {
            DateModified = DateTime.Now;
            if (DateCreated == null) DateCreated = DateTime.Now;
            base.OnSaving();
        }
      
		
        public StudentClass(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
