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
    public class RegisterDetailReportParametersObject : ReportParametersObjectBase
    {
        public RegisterDetailReportParametersObject(Session session) : base(session) { }
        public override CriteriaOperator GetCriteria()
        {
            return CriteriaOperator.Parse("Student = ? and Lesson.Semester= ?", Student, Semester );

        }
        Semester semester;
        public Semester Semester
        {
            get { return semester; }
            set { semester = value; }
        }
        Student student;
        public Student Student
        {
            get { return student; }
            set { student = value; }
        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            
            
            return sorting;

           
        }
       
    }

}