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
    public class TeacherReportParametersObject : ReportParametersObjectBase
    {
        public TeacherReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            Data.CreateTeacherTimetableData(this.Session, teacher.TeacherCode, semester.SemesterName);

            return CriteriaOperator.Parse("TeacherCode = ? ", Teacher.TeacherCode);


        }
        Semester semester;
        public Semester Semester
        {
            get { return semester;}
            set { semester = value; }
        }
        Teacher teacher;
        public Teacher Teacher
        {
            get { return teacher; }
            set { teacher = value; }
        }
         
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
