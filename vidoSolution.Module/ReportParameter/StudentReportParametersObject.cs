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

namespace vidoSolution.Module.ReportParameter
{
    [NonPersistent]
    public class StudentReportParametersObject : ReportParametersObjectBase
    {
        public StudentReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            Utilities.Data.CreateStudentAccumulation(this.Session, Student.StudentCode);
            if (ReportDataType == typeof(StudentAccumulation) || ReportDataType == typeof(StudentResult))
            {

                return CriteriaOperator.Parse("Student.StudentCode = ? ", Student.StudentCode);
            }
            else
                return CriteriaOperator.Parse("StudentCode = ? ", Student.StudentCode);


        }
        Student  student ;
         [RuleRequiredField("StudentReportParametersObject Student is Require!", "PreviewReport", "Student cannot be empty!")]
        public Student Student 
        {
            get { return student; }
            set { student= value; }
        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
