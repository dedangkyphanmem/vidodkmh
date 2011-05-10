using System;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;
using vidoSolution.Module.DomainObject;
using vidoSolution.Module.Utilities;

namespace vidoSolution.Module.ReportParameter
{
    [NonPersistent]
    public class StudentClassReportParametersObject : ReportParametersObjectBase
    {
        public StudentClassReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            if (ReportDataType == typeof(StudentClass))
            {
                Data.CreateStudentClassTimetableData(this.Session, StudentClass.ClassCode, semester.SemesterName);

                return CriteriaOperator.Parse("ClassCode = ? ", StudentClass.ClassCode);
            }
            else if (ReportDataType == typeof(RegisterDetail))
            {
                return CriteriaOperator.Parse("Lesson.Semester.SemesterName = ? and Student.StudentClass.ClassCode=? ",
                    semester.SemesterName, StudentClass.ClassCode);
            }
            if (ReportDataType == typeof(AccountTransactionTracking))
            {
                Data.CreateStudentClassTransactionTrackingData(this.Session, StudentClass.ClassCode, semester.SemesterName);

                return CriteriaOperator.Parse("Student.StudentClass.ClassCode = ? ", StudentClass.ClassCode);
            }
            else
            {
                return "";
            }
            
        }
        StudentClass  studentClass ;
        public StudentClass StudentClass 
        {
            get { return studentClass; }
            set { studentClass = value; }
        }
        Semester semester;
        public Semester Semester
        {
            get { return semester; }
            set { semester = value; }
        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
