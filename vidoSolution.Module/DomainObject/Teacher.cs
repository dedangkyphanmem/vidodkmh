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
    [Persistent("Teachers")]
    public class Teacher : BaseObject
    {
        [Association("Teachers-WeekReportDatas", typeof(WeekReportData))]
        public XPCollection WeekReportDatas
        {
            get { return GetCollection("WeekReportDatas"); }
        }

        [Association("Lessons-Teachers", typeof(Lesson))]
        public XPCollection Lessons
        {
            get { return GetCollection("Lessons"); }
        }

        Department fDepartment;
        [Association("Department-Teachers")]
        public Department Department
        {
            get { return fDepartment; }
            set { SetPropertyValue<Department>("Department", ref fDepartment, value); }
        }

          string fteachercode;
        [Size(10)]
        [RuleRequiredField("RuleRequiredField for Teacher.TeacherCode",DefaultContexts.Save,"TeacherCode is not NULL")]
        public string TeacherCode
        {
            get { return fteachercode; }
            set { SetPropertyValue<string>("teachercode", ref fteachercode, value); }
        }
        string flastname;
        [Size(255)]
        [RuleRequiredField("RuleRequiredField for Teacher.LastName", DefaultContexts.Save)]
        public string LastName
        {
            get { return flastname; }
            set { SetPropertyValue<string>("lastname", ref flastname, value); }
        }
        string ffirstname;
        [Size(255)]
        public string FirstName
        {
            get { return ffirstname; }
            set { SetPropertyValue<string>("firstname", ref ffirstname, value); }
        }
        
       
        public string FullName
        {
            get { return ffirstname+" "+flastname ; }
            
        }

        string fShortname;
        [Size(255)]
        public string ShortName
        {
            get { return fShortname; }
            set { SetPropertyValue<string>("ShortName", ref fShortname, value); }
        }
        string fbirthdate;
        public string Birthday
        {
            get { return fbirthdate; }
            set { SetPropertyValue<string>("Birthday", ref fbirthdate, value); }
        }
        bool fsex;
        public bool Sex
        {
            get { return fsex; }
            set { SetPropertyValue<bool>("sex", ref fsex, value); }
        }
        string femail;
        [Size(255)]
        public string Email
        {
            get { return femail; }
            set { SetPropertyValue<string>("email", ref femail, value); }
        }
        string faddress;
        [Size(2147483647)]
        public string Address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fphone;
        [Size(50)]
        public string Phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string fmobile;
        [Size(50)]
        public string Mobile
        {
            get { return fmobile; }
            set { SetPropertyValue<string>("mobile", ref fmobile, value); }
        }
        bool fisNotEmployee;
        public bool isNotEmployee
        {
            get { return fisNotEmployee; }
            set { SetPropertyValue<bool>("isNotEmployee", ref fisNotEmployee, value); }
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
        public Teacher(Session session) : base(session) { }
        public Teacher() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
