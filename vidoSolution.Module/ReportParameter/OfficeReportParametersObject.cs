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
    public class OfficeReportParametersObject : ReportParametersObjectBase
    {
        public OfficeReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            Data.CreateOfficeTimetableData(this.Session, Office.OfficeCode, semester.SemesterName);

            return CriteriaOperator.Parse("OfficeCode = ? ", Office.OfficeCode);
        }
        Office office;
        [RuleRequiredField("OfficeReportParametersObject Office is Require!", "PreviewReport", "Office cannot be empty!")]
        public Office Office
        {
            get { return office; }
            set { office = value; }
        }
        Semester semester;
        [RuleRequiredField("OfficeReportParametersObject Semester is Require!", "PreviewReport", "Semester cannot be empty!")]
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
