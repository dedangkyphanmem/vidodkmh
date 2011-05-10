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
    public class ClassroomReportParametersObject : ReportParametersObjectBase
    {
        public ClassroomReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            Data.CreateClassroomTimetableData(this.Session, Classroom.ClassroomCode, semester.SemesterName);
           
            return CriteriaOperator.Parse("ClassroomCode = ? ", Classroom.ClassroomCode);
        }
        Classroom classroom;
        public Classroom Classroom
        {
            get { return classroom; }
            set { classroom = value; }
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
