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
    [Persistent("StudentResults")]
    public class StudentResult : BaseObject
    {
        private Lesson fL;
        [Association("Lesson-StudentResults", typeof(Lesson))]
        public Lesson Lesson
        {
            get { return fL; }
            set { SetPropertyValue<Lesson>("Lesson", ref fL, value); }
        }

        private Student fs;
        [Association("Student-StudentResults", typeof(Student))]
        public Student Student
        {
            get { return fs; }
            set { SetPropertyValue<Student>("Student", ref fs, value); }
        }


        public String ResultName
        {
            get
            {
                if (Student!=null && Lesson!=null)
                    return Student.StudentCode + "-" + Lesson.Semester.SemesterName +
                    "-" + Lesson.Subject.SubjectCode + "-" +
                    favg_mark10.ToString() + "-" + favg_mark4.ToString() + "-" +favg_char;
                return "";
            }
           
        }

        double favg_mark10;
        public double AvgMark10
        {
            get { return favg_mark10; }
            set { SetPropertyValue<double>("AvgMark10", ref favg_mark10, value); }
        }
        double favg_mark4;
        public double AvgMark4
        {
            get { return favg_mark4; }
            set { SetPropertyValue<double>("AvgMark4", ref favg_mark4, value); }
        }
        String favg_char;
        [Size(2)]
        public String AvgChar
        {
            get { return favg_char; }
            set { SetPropertyValue<String>("AvgChar", ref favg_char, value); }
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
      
        public StudentResult(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
