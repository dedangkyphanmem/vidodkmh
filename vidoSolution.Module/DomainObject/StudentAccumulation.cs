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
    public class StudentAccumulation : BaseObject
    {
        public StudentAccumulation(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private Semester fsemester;
        [Association("Semester-StudentAccumulations", typeof(Semester))]
        public Semester Semester
        {
            get { return fsemester; }
            set
            {
                SetPropertyValue<Semester>("Semester", ref fsemester, value);
                OnChanged("SemesterCredit");
                OnChanged("SemesterAvgMark10");
                OnChanged("SemesterAvgMark4");
                OnChanged("TotalAccumulateCredit");
                OnChanged("AccumulateAvgMark10");
                OnChanged("AccumulateAvgMark4");
            }
        }

        private Student fs;
        [Association("Student-StudentAccumulations", typeof(Student))]
        public Student Student
        {
            get { return fs; }
            set
            {
                SetPropertyValue<Student>("Student", ref fs, value);
                OnChanged("SemesterCredit");
                OnChanged("SemesterAvgMark10");
                OnChanged("SemesterAvgMark4");
                OnChanged("TotalAccumulateCredit");
                OnChanged("AccumulateAvgMark10");
                OnChanged("AccumulateAvgMark4");
            }
        }


        public String AccumulationName
        {
            get
            {
                if (Student != null && Semester != null)
                    return Student.StudentCode + "-" + Semester.SemesterName +
                    "-" + fTotalAccumulateCredit.ToString() + "-" +
                    favg_mark10.ToString() + "-" + favg_mark4.ToString() + "-";
                return "";
            }

        }

        double fSemesterCredit=0;
        public double SemesterCredit
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    UpdateSemesterCredit(false);
                return fSemesterCredit;
            }
            set { SetPropertyValue<double>("SemesterCredit", ref fSemesterCredit, value); }
        }
        public void UpdateSemesterCredit(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfSemesterCredit = fSemesterCredit;

                double temp = 0.0;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=?", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {
                        temp += sr.Lesson.Subject.Credit;
                    }
                }
                fSemesterCredit = temp;
                if (forceChangeEvents)
                    OnChanged("SemesterCredit", oldfSemesterCredit, fSemesterCredit);
                SetPropertyValue<double>("SemesterCredit", ref fSemesterCredit, fSemesterCredit);
            }
        }

        double favg_marksem10;
        public double SemesterAvgMark10
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    UpdateSemesterAvgMark10(false);
                return favg_marksem10;
            }
            set { SetPropertyValue<double>("SemesterAvgMark10", ref favg_marksem10, value); }
        }
        public void UpdateSemesterAvgMark10(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfavg_marksem10 = favg_marksem10;

                double tempSum = 0.0, tempCount=0.0 ;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=?", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {
                        tempSum += sr.Lesson.Subject.Credit * sr.AvgMark10;
                        tempCount += sr.Lesson.Subject.Credit;
                    }
                }
                favg_marksem10 = tempCount!=0? tempSum /tempCount :0.0 ;
                if (forceChangeEvents)
                    OnChanged("SemesterAvgMark10", oldfavg_marksem10, favg_marksem10);
                SetPropertyValue<double>("SemesterAvgMark10", ref favg_marksem10, favg_marksem10);
            }
        }
        
        double favg_marksem4;
        public double SemesterAvgMark4
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    UpdateSemesterAvgMark4(false); 
                return favg_marksem4;
            }
            set { SetPropertyValue<double>("SemesterAvgMark4", ref favg_marksem4, value); }
        }
        public void UpdateSemesterAvgMark4(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfavg_marksem4 = favg_marksem4;

                double tempSum = 0.0, tempCount = 0.0;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=?", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {
                        tempSum += sr.Lesson.Subject.Credit * sr.AvgMark4;
                        tempCount += sr.Lesson.Subject.Credit;
                    }
                }
                favg_marksem4 = tempCount != 0 ? tempSum / tempCount : 0.0;
                if (forceChangeEvents)
                    OnChanged("SemesterAvgMark4", oldfavg_marksem4, favg_marksem4);
                SetPropertyValue<double>("SemesterAvgMark4", ref favg_marksem4, favg_marksem4);
            }
        }
        
        double fTotalAccumulateCredit;
        public double TotalAccumulateCredit
        {
            get
            {
                if (!IsLoading && !IsSaving )
                    UpdateTotalAccumulateCredit(false); 
                return fTotalAccumulateCredit;
            }
            set { SetPropertyValue<double>("TotalAccumulateCredit", ref fTotalAccumulateCredit, value); }
        }
        public void UpdateTotalAccumulateCredit(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfTotalAccumulateCredit = fTotalAccumulateCredit;

                double  tempCount = 0.0;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=? and AvgMark4>=1.5", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {                        
                        tempCount += sr.Lesson.Subject.Credit;
                    }
                }
                fTotalAccumulateCredit = tempCount;
                if (forceChangeEvents)
                    OnChanged("TotalAccumulateCredit", oldfTotalAccumulateCredit, fTotalAccumulateCredit);
                SetPropertyValue<double>("TotalAccumulateCredit", ref fTotalAccumulateCredit, fTotalAccumulateCredit);
            }
        }

        double favg_mark10;
        public double AccumulateAvgMark10
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    UpdateAccumulateAvgMark10(false); 
                return favg_mark10; }
            set { SetPropertyValue<double>("AccumulateAvgMark10", ref favg_mark10, value); }
        }
        public void UpdateAccumulateAvgMark10(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfavg_mark10 = favg_mark10;

                double tempSum = 0.0, tempCount = 0.0;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=? and AvgMark4>=1.5", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {
                        tempSum += sr.Lesson.Subject.Credit * sr.AvgMark10;
                        tempCount += sr.Lesson.Subject.Credit;
                    }
                }
                favg_mark10 = tempCount != 0 ? tempSum / tempCount : 0.0;
                if (forceChangeEvents)
                    OnChanged("AccumulateAvgMark10", oldfavg_mark10, favg_mark10);
               SetPropertyValue<double>("AccumulateAvgMark10", ref favg_mark10, favg_mark10); 
            }
        }

        double favg_mark4;
        public double AccumulateAvgMark4
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    UpdateAccumulateAvgMark4(false);  
                return favg_mark4; }
            set { SetPropertyValue<double>("AccumulateAvgMark4", ref favg_mark4, value); }
        }
        public void UpdateAccumulateAvgMark4(bool forceChangeEvents)
        {
            if (Student != null & Semester != null)
            {
                double oldfavg_mark4 = favg_mark4;

                double tempSum = 0.0, tempCount = 0.0;
                using (XPCollection<StudentResult> xpc = new XPCollection<StudentResult>(Student.StudentResults,
                    CriteriaOperator.Parse("Lesson.Semester.SemesterName=? and AvgMark4>=1.5", Semester.SemesterName)))
                {
                    foreach (StudentResult sr in xpc)
                    {
                        tempSum += sr.Lesson.Subject.Credit * sr.AvgMark4;
                        tempCount += sr.Lesson.Subject.Credit;
                    }
                }
                favg_mark4 = tempCount != 0 ? tempSum / tempCount : 0.0;
                if (forceChangeEvents)
                    OnChanged("AccumulateAvgMark4", oldfavg_mark4, favg_mark4);
                SetPropertyValue<double>("AccumulateAvgMark4", ref favg_mark4, favg_mark4);
            }
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
    }

}
