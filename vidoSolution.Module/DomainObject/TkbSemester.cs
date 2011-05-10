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
    [VisibleInReports]
    [Persistent("TkbSemester")]
    public class TkbSemester : BaseObject
    {
        
      
        private Lesson lesson;
        [Association("Lesson-TkbSemesters", typeof(Lesson))]
        public Lesson Lesson
        {
            get { return lesson; }
            set { SetPropertyValue("Lesson", ref lesson, value); }
        }
        public string Name
        {
            get
            {
                if (Lesson != null)
                    return lesson.LessonCode.ToString("D4");
                else
                    return "";
            }
        }
        int fday;
        public int Day
        {
            get { return fday; }
            set { SetPropertyValue<int>("Day", ref fday, value); }
        }

        string fperiod;
        public string Period
        {
            get { return fperiod; }
            set { SetPropertyValue<string>("Period", ref fperiod, value); }
        }

        Classroom fClassroom;
        [Association("Classroom-TkbSemesters", typeof(Classroom))]
        public Classroom Classroom
        {
            get { return fClassroom; }
            set { SetPropertyValue<Classroom>("Classroom", ref fClassroom, value); }
        }

        string fweeks;
        [Size(25)]
        public string Weeks
        {
            get { return fweeks; }
            set { SetPropertyValue<string>("Weeks", ref fweeks, value); }
        }

       
        [Size(25)]
        public string StringWeeks
        {
            get
            {
                string str = "";
                if (fweeks == null)
                    return str;
                for (int i = 0; i < fweeks.Length ; i++)
                {
                    str += (fweeks[i] == '0' ? "-" : ((i + 1) % 10).ToString());                    
                }
                return str;
            }
            
        }
        
        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }
        public TkbSemester(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
        
    }

}
