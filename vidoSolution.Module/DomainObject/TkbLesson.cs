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
    [VisibleInReports]
    [Persistent("TkbLesson")]
    public class TkbLesson : BaseObject
    {

        private Lesson fLesson=null;
        public Lesson Lesson
        {
            get { return fLesson; }
            set {
                if (fLesson == value)
                    return;
                Lesson prevLesson = fLesson;
                fLesson = value;
                if (IsLoading) return;
                if (prevLesson != null && prevLesson.TkbLesson == this)
                    prevLesson.TkbLesson = null;
                if (fLesson != null)
                    fLesson.TkbLesson = this;
                OnChanged("Lesson");
            }
        }
        private Semester fSemester;
        [Association("Semester-TkbLessons")]
        public Semester Semester
        {
            get { return fSemester; }
            set { SetPropertyValue<Semester>("Semester", ref fSemester, value); }
        }

        string fID;
        [Size(100)]
        public string ID
        {
            get { return fID; }
            set { SetPropertyValue<string>("ID", ref fID, value); }
        }
        public string Name
        {
            get
            {
                if ((Lesson != null) && (Lesson.Semester !=null ))
                    return String.Format("{0}-{1}-{2}", Lesson.Semester.SemesterName, SubjectID, ID);
                else 
                    return "";
            }
        }

        string fclassids;
        [Size(100)]
        public string ClassIDs
        {
            get { return fclassids; }
            set { SetPropertyValue<string>("ClassIDs", ref fclassids, value); }
        }

        string fsubjectid;
        [Size(100)]
        public string SubjectID
        {
            get { return fsubjectid; }
            set { SetPropertyValue<string>("SubjectID", ref fsubjectid, value); }
        }

        int fnumexpectation;
        public int NumExpectation
        {
            get { return fnumexpectation; }
            set { SetPropertyValue<int>("NumExpectation", ref fnumexpectation, value); }
        }

        int fperiodspercard;
        public int PeriodsPerCard
        {
            get { return fperiodspercard; }
            set { SetPropertyValue<int>("PeriodsPerCard", ref fperiodspercard, value); }
        }

        string fperiodsperweek;
        [Size(100)]
        public string PeriodsPerWeek
        {
            get { return fperiodsperweek; }
            set { SetPropertyValue<string>("PeriodsPerWeek", ref fperiodsperweek, value); }
        }

        string fteacherids;
        [Size(100)]
        public string TeacherIDs
        {
            get { return fteacherids; }
            set { SetPropertyValue<string>("TeacherIDs", ref fteacherids, value); }
        }

        string fclassroomids;
        [Size(100)]
        public string ClassroomIDs
        {
            get { return fclassroomids; }
            set { SetPropertyValue<string>("ClassroomIDs", ref fclassroomids, value); }
        }

        string fgroupids;
        [Size(100)]
        public string GroupIDs
        {
            get { return fgroupids; }
            set { SetPropertyValue<string>("GroupIDs", ref fgroupids, value); }
        }

        string fstudentids;
        [Size(100)]
        public string StudentIDs
        {
            get { return fstudentids; }
            set { SetPropertyValue<string>("StudentIDs", ref fstudentids, value); }
        }

        string fweek;
        [Size(50)]
        public string Week
        {
            get { return fweek; }
            set { SetPropertyValue<string>("Week", ref fweek, value); }
        }

        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }
        public TkbLesson(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
