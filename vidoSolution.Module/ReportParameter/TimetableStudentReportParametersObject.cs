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
    public class TimetableStudentReportParametersObject : ReportParametersObjectBase
    {
        public TimetableStudentReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            Data.CreateStudentTimetableData(this.Session, Student.StudentCode, semester.SemesterName);

            return CriteriaOperator.Parse("StudentCode = ? ", Student.StudentCode);


        }
        Student  student ;

        [RuleRequiredField("TimetableStudentReportParametersObject Student is Require!", "PreviewReport", "Student cannot be empty!")]
        public Student Student 
        {
            get { return student; }
            set { student= value; }
        }
        Semester semester;

        [RuleRequiredField("TimetableStudentReportParametersObject Semester is Require!", "PreviewReport", "Semester cannot be empty!")]
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
