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
    public class SemesterReportParametersObject : ReportParametersObjectBase
    {
        public SemesterReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            if (ReportDataType == typeof(ClassTransactionTracking))
            {
                Data.CreateStudentClassTrackingData(this.Session, semester.SemesterName);

                return CriteriaOperator.Parse("Semester.SemesterName = ? ", semester.SemesterName);
            }
            else
                return CriteriaOperator.Parse("SemesterName=?", semester.SemesterName);
        }
       
        Semester semester;
        [RuleRequiredField("SemesterReportParametersObject Semester is Require!", "PreviewReport", "Semester cannot be empty!")]
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
