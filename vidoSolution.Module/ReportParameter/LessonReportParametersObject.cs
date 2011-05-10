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
    public class LessonReportParametersObject : ReportParametersObjectBase
    {
        public LessonReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            return CriteriaOperator.Parse("LessonCode = ? ", Lesson.LessonCode);
            
        }
        Lesson lesson;
        public Lesson Lesson
        {
            get { return lesson; }
            set { lesson = value; }
        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
