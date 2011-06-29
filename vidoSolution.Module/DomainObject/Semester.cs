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
    [Persistent("Semester")]
    public class Semester : BaseObject
    {
       
        string fsemestername;
        [Size(5)]
        [RuleUniqueValue("RuleUniqueValue SemesterName for Semester", DefaultContexts.Save)]
        public string SemesterName
        {
            get { return fsemestername; }
            set { SetPropertyValue<string>("SemesterName", ref fsemestername, value); }
        }
		
        DateTime fstart_date;

        [RuleRequiredField("RuleRequiredField StartDate for Semester", DefaultContexts.Save)]
        public DateTime StartDate
        {
            get { return fstart_date; }
            set { SetPropertyValue<DateTime>("start_date", ref fstart_date, value); }
        }
        int fnumber_of_week;
         [RuleRequiredField("RuleRequiredField NumberOfWeek for Semester", DefaultContexts.Save)]
        public int NumberOfWeek
        {
            get { return fnumber_of_week; }
            set { SetPropertyValue<int>("number_of_week", ref fnumber_of_week, value); }
        }
        string fdescription;
        [Size(500)]
        public string Description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
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
        [Association("Semester-ClassTransactionTrackings", typeof(ClassTransactionTracking))]
        public XPCollection ClassTransactionTrackings
        {
            get { return GetCollection("ClassTransactionTrackings"); }
        }
        [Association("Semester-AccountTransactionFiles", typeof(AccountTransactionFile))]
        public XPCollection AccountTransactionFiles
        {
            get
            {
                return GetCollection("AccountTransactionFiles");
            }
        }
        [Association("Semester-AccountTransactions", typeof(AccountTransaction))]
        public XPCollection AccountTransactions
        {
            get
            {
                return GetCollection("AccountTransactions");
            }
        }

        [Association("Semester-AccountTransactionTrackings", typeof(AccountTransactionTracking))]
        public XPCollection AccountTransactionTrackings
        {
            get
            {
                return GetCollection("AccountTransactionTrackings");
            }
        }
        [Association("Semester-WeekReportDatas", typeof(WeekReportData))]
        public XPCollection WeekReportDatas
        {
            get { return GetCollection("WeekReportDatas"); }
        }

        [Association("Semester-TkbTeacher", typeof(TkbTeacher))]
        public XPCollection TkbTeachers
        {
            get { return GetCollection("TkbTeachers"); }
        }

        [Association("Semester-TkbClassroom", typeof(TkbClassroom))]
        public XPCollection TkbClassrooms
        {
            get { return GetCollection("TkbClassrooms"); }
        }

        [Association("Semester-TkbGroup", typeof(TkbGroup))]
        public XPCollection TkbGroups
        {
            get { return GetCollection("TkbGroups"); }
        }

        [Association("Semester-TkbGrade", typeof(TkbGrade))]
        public XPCollection TkbGrades
        {
            get { return GetCollection("TkbGrades"); }
        }

        [Association("Semester-TkbClass", typeof(TkbClass))]
        public XPCollection TkbClasses
        {
            get { return GetCollection("TkbClasses"); }
        }

        [Association("Semester-TkbSubject", typeof(TkbSubject))]
        public XPCollection TkbSubjects
        {
            get { return GetCollection("TkbSubjects"); }
        }

        [Association("Semester-TkbFiles", typeof(TkbFile))]
        public XPCollection TkbFiles
        {
            get { return GetCollection("TkbFiles"); }
        }
        [Association("Semester-TkbCard", typeof(TkbCard))]
        public XPCollection TkbCards
        {
            get { return GetCollection("TkbCards"); }
        }
        [Association("Semester-TkbLessons", typeof(TkbLesson))]
        public XPCollection TkbLessons
        {
            get { return GetCollection("TkbLessons"); }
        }
       [Association(("Semester-Lessons"), typeof(Lesson))]
        public XPCollection Lessons
        {
           get { return GetCollection("Lessons"); }
        }
		
		[Association(("Enrollsemester-StudentClasses"),typeof(StudentClass))]
        public XPCollection EnrollStudentClasses
        {
           get { return GetCollection("EnrollStudentClasses"); }
        }
		
		[Association(("GraduateSemester-StudentClasses"),typeof(StudentClass))]
        public XPCollection GraduateStudentClasses
        {
           get { return GetCollection("GraduateStudentClasses"); }
        }

       [Association("Semester-StudentAccumulations", typeof(StudentAccumulation))]
        public XPCollection StudentAccumulations
        {
            get { return GetCollection("StudentAccumulations"); }
        }
		
        public Semester(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
