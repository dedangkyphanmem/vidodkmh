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
    public class SubjectReportParametersObject : ReportParametersObjectBase
    {
        public SubjectReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            if (ReportDataType == typeof(Subject))
            {
                Data.CreateSubjectTimetableData(this.Session, Subject.SubjectCode, semester.SemesterName);
                return CriteriaOperator.Parse("SubjectCode = ? ", Subject.SubjectCode);
            }
            else if (ReportDataType == typeof(Lesson))
            {
                return CriteriaOperator.Parse("Subject.SubjectCode = ? and Semester.SemesterName=?", Subject.SubjectCode, Semester.SemesterName);
            }
            else
                return "";
        }
        Subject subject;
        public Subject Subject
        {
            get { return subject; }
            set { subject = value; }
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
