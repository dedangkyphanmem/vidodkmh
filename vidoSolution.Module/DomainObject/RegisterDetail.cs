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
    [Persistent("RegisterDetail")]
    public class RegisterDetail : BaseObject
    {
        Student fStudent;
         //[Indexed("Lesson", Unique = true)]
        [Association("Student-RegisterDetails")]
        public Student Student
        {
            get { return fStudent; }
            set { SetPropertyValue<Student>("Student", ref fStudent, value); }
        }
		
        Lesson fLesson;
        [Association("Lesson-RegisterDetails")]
        public Lesson Lesson
        {
            get { return fLesson; }
            set { SetPropertyValue<Lesson>("Lesson", ref fLesson, value); }
        }

        [Size(100)]
        public string Name
        {
            get
            {
                if (Student != null && Lesson!= null && Lesson.Semester!=null && Lesson.Subject !=null )
                    return String.Format("{0}-{1}-{2}", Student.StudentCode, Lesson.Semester.SemesterName, Lesson.Subject.SubjectCode);
                return "";

            }
            
        }

        
        //public string State
        //{
        //    get { if (fRegState!=null) return fRegState.Name;else return ""; }            
        //}

        RegisterState fRegState;
        [Association("RegisterState-RegisterDetails", typeof(RegisterState))]
        public RegisterState RegisterState
        {
            get { return fRegState; }
            set { SetPropertyValue<RegisterState>("RegisterState", ref fRegState, value); }
        }

        RegisterState fCheckState;
        [Association("CheckState-RegisterDetails", typeof(RegisterState))]
        public RegisterState CheckState
        {
            get { return fCheckState; }
            set { SetPropertyValue<RegisterState>("CheckState", ref fCheckState, value); }
        }
        string fnote;
        [Size(500)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
		
        bool fischange;
        public bool isChange
        {
            get { return fischange; }
            set { SetPropertyValue<bool>("ischange", ref fischange, value); }
        }

        DateTime? fdatecreated=null;
        public DateTime? DateCreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime?>("DateCreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime DateModified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("DateModified", ref fdatemodified, value); }
        }



        [RuleUniqueValue("RegisterDetail.StudentRegLessonSemester", DefaultContexts.Save, "Do not register subject again!")]
        [NonPersistent]
        public string StudentRegLessonSemester
        {
            get
            {
                if ((Student != null) && (Lesson != null))
                    return Student.StudentCode + Lesson.Subject.SubjectCode + Lesson.Semester.SemesterName;
                else
                    return "";
            }
        }
        public RegisterDetail(Session session) : base(session) { }
      
        public override void AfterConstruction() { base.AfterConstruction(); }
        protected  override void OnSaving()
        {
            DateModified = DateTime.Now;
            if (DateCreated == null) DateCreated = DateTime.Now;
            base.OnSaving();
        }
    }

}
