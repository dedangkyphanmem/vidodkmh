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
    [Persistent("Students")]
    public class Student : User
    {
        public Student(Session session) : base(session) { }
        [Association("Student-AccountTransactionTrackings", typeof(AccountTransactionTracking))]
        public XPCollection AccountTransactionTrackings
        {
            get { return GetCollection("AccountTransactionTrackings"); }
        }

        [Association("Students-WeekReportDatas", typeof(WeekReportData))]
        public XPCollection WeekReportDatas
        {
            get { return GetCollection("WeekReportDatas"); }
        }

        [Association("Student-AccountTransactions", typeof(AccountTransaction))]
        public XPCollection AccountTransactions
        {
            get { return GetCollection("AccountTransactions"); }
        }

        [Association("Student-RegisterDetails", typeof(RegisterDetail))]
        public XPCollection RegisterDetails
        {
            get { return GetCollection("RegisterDetails"); }
        }


        [Association("Student-StudentResults", typeof(StudentResult))]
        public XPCollection StudentResults
        {
            get { return GetCollection("StudentResults"); }
        }

        [Association("Student-StudentAccumulations", typeof(StudentAccumulation))]
        public XPCollection StudentAccumulations
        {
            get { return GetCollection("StudentAccumulations"); }
        }

        StudentClass fStudentClass;
        [Association("StudentClass-Students")]
        public StudentClass StudentClass
        {
            get { return fStudentClass; }
            set { SetPropertyValue<StudentClass>("StudentClass", ref fStudentClass, value); }
        }

      


        string fstudentcode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Student.StudentCode",DefaultContexts.Save)]
        public string StudentCode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }

        public new DateTime? Birthday
        {
            get
            {
                DateTime fdatetime;
                
                if (DateTime.TryParseExact(fStudentBirthDayText,new string[]{"dd/MM/yyyy"}, 
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat, 
                    System.Globalization.DateTimeStyles.None,out fdatetime))
                    return fdatetime;
                else
                    return null;
            }
        }
        string fStudentBirthDayText;
        [Size(11)]
        public string BirthdayText
        {
            get { return fStudentBirthDayText; }
            set { SetPropertyValue<string>("BirthdayText", ref fStudentBirthDayText, value); }
        }

        
        int fstudystate;
        [RuleRequiredField("RuleRequireField Student.Studystate", DefaultContexts.Save)]
        public int StudyState
        {
            get { return fstudystate; }
            set { SetPropertyValue<int>("studystate", ref fstudystate, value); }
        }
    
        bool fsex;
        [RuleRequiredField("RuleRequireField Student.IsFemale", DefaultContexts.Save)]
        public bool IsFemale
        {
            get { return fsex; }
            set { SetPropertyValue<bool>("IsFemale", ref fsex, value); }
        }

        string faddress;
        [Size(2147483647)]
        public string Address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fBirthPlace;
        [Size(500)]
        public string BirthPlace
        {
            get { return fBirthPlace; }
            set { SetPropertyValue<string>("BirthPlace", ref fBirthPlace, value); }
        }
        string fEthnic;
        [Size(50)]
        public string Ethnic
        {
            get { return fEthnic; }
            set { SetPropertyValue<string>("Ethnic", ref fEthnic, value); }
        }
        string fCourse;
        [Size(10)]
        public string Course
        {
            get { return fCourse; }
            set { SetPropertyValue<string>("Course", ref fCourse, value); }
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

        decimal? faccountbalance=null;
        public decimal? AccountBalance
        {
            get
            {
                if (!IsLoading && !IsSaving && (faccountbalance == null||faccountbalance == 0m))
                    UpdateAccountBalance(false);
                return faccountbalance;
            }
           

        }
        public void UpdateAccountBalance(bool forceChangeEvents)
        {
            decimal? oldaccountbalance = faccountbalance;
            decimal tempTotal = 0m;
            foreach (AccountTransaction detail in AccountTransactions)
                tempTotal += detail.MoneyAmount;
            faccountbalance = tempTotal;
            if (forceChangeEvents)
                OnChanged("AccountBalance", oldaccountbalance, faccountbalance);
        }


        double? fsemestercredit;
        public double? SemesterCredit
        {
            get
            {
                if (!IsLoading && !IsSaving && fsemestercredit == null)
                    UpdateSemesterCredit(false);
                return fsemestercredit;
            }
        }
        public void UpdateSemesterCredit(bool forceChangeEvents)
        {
            double? oldfsemestercredit = fsemestercredit;
            double tempDate = 0;
            foreach (RegisterDetail detail in RegisterDetails)
                tempDate += (detail.Lesson==null || detail.Lesson.Subject==null)? 0:  detail.Lesson.Subject.Credit;
                    
            fsemestercredit = tempDate;
            if (forceChangeEvents)
                OnChanged("SemesterCredit", oldfsemestercredit, fsemestercredit);
        }

        DateTime? fbalancelassmodify;
        public DateTime? BalanceLassModify
        {
            get
            {
                if (!IsLoading && !IsSaving && fbalancelassmodify == null)
                    UpdateBalanceLassModify(false);
                return fbalancelassmodify;
            }
        }
        public void UpdateBalanceLassModify(bool forceChangeEvents)
        {
            DateTime? oldfbalancelassmodify = fbalancelassmodify;
            DateTime tempDate = DateTime.MinValue;
            foreach (AccountTransaction detail in AccountTransactions)
                if (tempDate< detail.TransactingDate)
                    tempDate=detail.TransactingDate;
            fbalancelassmodify = tempDate;
            if (forceChangeEvents)
                OnChanged("BalanceLassModify", oldfbalancelassmodify, fbalancelassmodify);
        }
 
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
