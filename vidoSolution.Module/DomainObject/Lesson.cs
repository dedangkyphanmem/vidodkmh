using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("Lessons")]
    public class Lesson : BaseObject
    {

        Subject fSubject;
        [RuleRequiredField("Rule Require Field Subject For Lesson", DefaultContexts.Save)]
        [Association("Subject-Lessons")]
        public Subject Subject
        {
            get { return fSubject; }
            set { SetPropertyValue<Subject>("Subject", ref fSubject, value); }
        }
        
		Semester fSemester;
         [RuleRequiredField("Rule Require Field Semester For Lesson", DefaultContexts.Save)]
        [Association("Semester-Lessons")]
        public Semester Semester
        {
            get { return fSemester; }
            set { SetPropertyValue<Semester>("Semester", ref fSemester, value); }
        }
				
		[Association("Lesson-RegisterDetails", typeof(RegisterDetail))]
        public XPCollection RegisterDetails
        {
            get { return GetCollection("RegisterDetails"); }
        }

        [Association("Lesson-StudentResults", typeof(StudentResult))]
        public XPCollection StudentResults
        {
            get { return GetCollection("StudentResults"); }
        }
         [Association("Lesson-TkbSemesters", typeof(TkbSemester))]
        public XPCollection TKBSemesters
        {
            get { return GetCollection("TKBSemesters"); }
        }
         [Association("Lessons-Teachers", typeof(Teacher))]
         public XPCollection Teachers
         {
             get { return GetCollection("Teachers"); }
         }
         private TkbLesson fTkbLesson;
        public TkbLesson TkbLesson
         {
             get { return fTkbLesson; }
             set
             {
                 if (fTkbLesson == value)
                     return;
                 TkbLesson prevfTkbLesson = fTkbLesson;
                 fTkbLesson = value;
                 if (IsLoading) return;
                 if (prevfTkbLesson != null && prevfTkbLesson.Lesson == this)
                     prevfTkbLesson.Lesson = null;
                 if (fTkbLesson != null)
                     fTkbLesson.Lesson = this;
                 OnChanged("TkbLesson");
             }
         }
        int fLessonCode;
        [Size(4)]
        [RuleUniqueValue("RuleUniqueValue Lesson.LessonCode", DefaultContexts.Save)]
        public int LessonCode
        {
            get { return fLessonCode; }
            set { SetPropertyValue<int>("LessonCode", ref fLessonCode, value); }
        }
        
        public string LessonName
        {
            get {
                return LessonCode.ToString("D4") ;
            }
        }


        string fClassIDs;
        public string ClassIDs
        {
            get
            {
                return fClassIDs;

            }
            set
            {
                SetPropertyValue<string>("ClassIDs", ref fClassIDs, value); 
            }
        }

        public string Timetable
        {

            get {
                string tkb = "";                
                foreach (TkbSemester tkbsemester in TKBSemesters)
                {
                   
                    tkb += String.Format("{0,4} | {1,24} | {2,10} | {3,25}\r\n", 
                        tkbsemester.Day, tkbsemester.Period, 
                        (tkbsemester.Classroom==null?"":tkbsemester.Classroom.ClassroomCode),
                        tkbsemester.StringWeeks);
                }
                return tkb;
            }
        }
        bool fCanRegister;
        public bool CanRegister
        {
            get { return fCanRegister; }
            set { SetPropertyValue<bool>("CanRegister", ref fCanRegister, value); }
        }
        private string fLessonNote;
        public string LessonNote
        {
            get { return fLessonNote; }
            set { SetPropertyValue<string>("LessonNote", ref fLessonNote, value); }
        }
        decimal ftuitionfee;
        public decimal TuitionFee
        {
            get { return ftuitionfee; }
            set { SetPropertyValue<decimal>("tuitionfee", ref ftuitionfee, value); }
        }
        int fnumexpectation;
        public int NumExpectation
        {
            get { return fnumexpectation; }
            set { SetPropertyValue<int>("NumExpectation", ref fnumexpectation, value); }
        }
        int fnumregistration;
        public int NumRegistration
        {
            get { return fnumregistration; }
            set { SetPropertyValue<int>("NumRegistration", ref fnumregistration, value); }
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

        
        public Lesson(Session session) : base(session) { }
   
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
