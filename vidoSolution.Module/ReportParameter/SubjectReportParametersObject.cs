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
using System.ComponentModel;

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
        [ImmediatePostData(true)]
        [DataSourceProperty("AvailableSubjects")]
        [RuleRequiredField("SubjectReportParametersObject Subject is Require!", "PreviewReport", "Subject cannot be empty!")]
        public Subject Subject
        {
            get { return subject; }
            set { SetPropertyValue("Subject", ref subject, value); }
        }
        Semester semester;
        [ImmediatePostData(true)]
        [RuleRequiredField("SubjectReportParametersObject Semester is Require!", "PreviewReport", "Semester cannot be empty!")]
        public Semester Semester
        {
            get { return semester; }
            set
            {
                SetPropertyValue("Semester", ref semester, value);
                RefreshAvailableSubjects();
            }
        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }

        private XPCollection<Subject> availableSubjects;
        [Browsable(false)] // Prohibits showing the AvailableAccessories collection separately 
        public XPCollection<Subject> AvailableSubjects
        {
            get
            {
                if (availableSubjects == null)
                {
                    // Retrieve all Accessory objects 
                    availableSubjects = new XPCollection<Subject>(Session);
                    // Filter the retrieved collection according to current conditions 
                    RefreshAvailableSubjects();
                }
                // Return the filtered collection of Accessory objects 
                return availableSubjects;
            }
        }

        private void RefreshAvailableSubjects()
        {
            if (availableSubjects == null)
                return;
            // Process the situation when the Semester is not specified (see the Scenario 3 above) 
            if (Semester == null)
            {
                ConstrainstParameter cpNHHK = Session.FindObject<ConstrainstParameter>(
                           new BinaryOperator("Code", "REGISTERSEMESTER"));
                Semester semester1 = Session.FindObject<Semester>(new BinaryOperator("SemesterName", cpNHHK.Value));
                // Show only Global Accessories when the Product is not specified 
                
                availableSubjects.Criteria = new ContainsOperator("Lessons", new BinaryOperator("Semester",semester1));
            }
            else
            {
                // Leave only the current Product's Accessories in the availableAccessories collection 
                availableSubjects.Criteria = new ContainsOperator("Lessons", new BinaryOperator("Semester", Semester));
                
            }
            // Set null for the Accessory property to allow an end-user  
            //to set a new value from the refreshed data source 
            Subject = null;
        }
    }

}
