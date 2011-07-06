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
using System.ComponentModel;

namespace vidoSolution.Module.ReportParameter
{
    [NonPersistent]
    public class LessonReportParametersObject : ReportParametersObjectBase
    {
        public LessonReportParametersObject(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            selectAllStudentClass = true;
            showOnlyCheckedStudent = true;
          
        }
        Semester semester;
        [ImmediatePostData(true)]
        public Semester Semester
        {
            get { return semester; }
            set
            {
                SetPropertyValue("Semester", ref semester, value);
                RefreshAvailableSubjects();
                RefreshAvailableLessons();
            }
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

                availableSubjects.Criteria = new ContainsOperator("Lessons", new BinaryOperator("Semester", semester1));
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
        Subject subject;
        [ImmediatePostData(true)]
        [DataSourceProperty("AvailableSubjects")]
        public Subject Subject
        {
            get { return subject; }
            set
            {
                SetPropertyValue("Subject", ref subject, value);
                RefreshAvailableLessons();
            }
        }

        private XPCollection<Lesson> availableLessons;
        [Browsable(false)] // Prohibits showing the AvailableAccessories collection separately 
        public XPCollection<Lesson> AvailableLessons
        {
            get
            {
                if (availableLessons == null)
                {
                    // Retrieve all Accessory objects 
                    availableLessons = new XPCollection<Lesson>(Session);
                    // Filter the retrieved collection according to current conditions 
                    RefreshAvailableLessons();
                }
                // Return the filtered collection of Accessory objects 
                return availableLessons;
            }
        }
        private void RefreshAvailableLessons()
        {
            if (availableLessons == null)
                return;
            // Process the situation when the Semester is not specified (see the Scenario 3 above) 
           
            BinaryOperator biOpSemester=null, biOpSubject=null;
            if (Semester != null)
            {
                biOpSemester = new BinaryOperator("Semester", Semester);
            }
            if (Subject != null)
            {
                biOpSubject = new BinaryOperator("Subject", Subject);
            }
            if (Semester != null && Subject != null)
            {
                availableLessons.Criteria = new GroupOperator(GroupOperatorType.And, biOpSubject, biOpSemester);
            }
            else if (Subject != null)
            {
                // Leave only the current Product's Accessories in the availableAccessories collection 
                availableLessons.Criteria = new BinaryOperator("Subject", Subject);
            }
            else if (Semester != null)
            {
                availableLessons.Criteria = new BinaryOperator("Semester", Semester);
            }

            // Set null for the Accessory property to allow an end-user  
            //to set a new value from the refreshed data source 
            Lesson = null;
        }
        Lesson lesson;
        [ImmediatePostData(true)]
        [DataSourceProperty("AvailableLessons")]
        [RuleRequiredField("Lesson is Require!", "PreviewReport", "Lesson cannot be empty!")]
        public Lesson Lesson
        {
            get { return lesson; }
            set
            {
                SetPropertyValue("Lesson", ref lesson, value);
                RefreshAvailableStudentClasses();
            }
        }
        private XPCollection<StudentClass> availableStudentClasses;
        [Browsable(false)] // Prohibits showing the AvailableAccessories collection separately 
        public XPCollection<StudentClass> AvailableStudentClasses
        {
            get
            {
                if (availableStudentClasses == null)
                {
                    // Retrieve all Accessory objects 
                    availableStudentClasses = new XPCollection<StudentClass>(Session);
                    // Filter the retrieved collection according to current conditions 
                    RefreshAvailableStudentClasses();
                }
                // Return the filtered collection of Accessory objects 
                return availableStudentClasses;
            }
        }
        private void RefreshAvailableStudentClasses()
        {
            
            if (availableStudentClasses == null)
                return;
            if (selectAllStudentClass)
            {
                availableStudentClasses.CriteriaString = "1=0";
                
            }
            else if (Lesson != null)
            {
                availableStudentClasses.Criteria = new ContainsOperator("Students",
                    new ContainsOperator("RegisterDetails", new BinaryOperator("Lesson", Lesson)));
            }           

            // Set null for the Accessory property to allow an end-user  
            //to set a new value from the refreshed data source 
            StudentClass = null;
        }
        StudentClass studentClass;
        [ImmediatePostData(true)]
        [DataSourceProperty("AvailableStudentClasses")]
        public StudentClass StudentClass
        {
            get { return studentClass; }
            set
            {
                SetPropertyValue("StudentClass", ref studentClass, value);
            }
        }

        Boolean selectAllStudentClass;
        [ImmediatePostData(true)]
        public Boolean SelectAllStudentClass
        {
            get { return selectAllStudentClass; }
            set
            {
                SetPropertyValue("SelectAllStudentClass", ref selectAllStudentClass, value);
                RefreshAvailableStudentClasses();
            }
        }
        
        Boolean showOnlyCheckedStudent;
        [ImmediatePostData(true)]
        public Boolean ShowOnlyCheckedStudent
        {
            get { return showOnlyCheckedStudent; }
            set
            {
                SetPropertyValue("ShowOnlyCheckedStudent", ref showOnlyCheckedStudent, value);
            }
        }
       
        public override CriteriaOperator GetCriteria()
        {           
            return CriteriaOperator.Parse("LessonCode = ? ", Lesson.LessonCode);

        }
        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
