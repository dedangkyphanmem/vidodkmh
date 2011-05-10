using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent]
    public class WeekReportData : BaseObject
    {
        public WeekReportData(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [Association("Classrooms-WeekReportDatas", typeof(Classroom))]
        public XPCollection Classrooms
        {
            get { return GetCollection("Classrooms"); }
        }

        [Association("Students-WeekReportDatas", typeof(Student))]
        public XPCollection Students
        {
            get { return GetCollection("Students"); }
        }

        [Association("StudentClasses-WeekReportDatas", typeof(StudentClass))]
        public XPCollection StudentClasses
        {
            get { return GetCollection("StudentClasses"); }
        }
        [Association("Teachers-WeekReportDatas", typeof(Teacher))]
        public XPCollection Teachers
        {
            get { return GetCollection("Teachers"); }
        }
        [Association("Subjects-WeekReportDatas", typeof(Subject))]
        public XPCollection Subjects
        {
            get { return GetCollection("Subjects"); }
        }
        public string this[int day]
        {
            get
            {
                switch (day)
                {
                    case 0:
                        return (Semester==null? "": Semester.SemesterName);
                    case 1:
                        return _PeriodTime;
                    case 2:
                        return _Day2;                        
                    case 3:
                        return _Day3;                        
                    case 4:
                        return _Day4;                        
                    case 5:
                        return _Day5;                        
                    case 6:
                        return _Day6;                        
                    case 7:
                        return _Day7;                        
                    case 8:
                        return _Day8;   
                    default:
                        return "";
                }
               
            }
            set
            {
                switch (day)
                {
                    case 1:
                        SetPropertyValue<string>("PeriodTime", ref _PeriodTime, value);
                        break;
                    case 2:
                        SetPropertyValue<string>("Day2", ref _Day2, value);
                        break;
                    case 3:
                        SetPropertyValue<string>("Day3", ref _Day3, value);
                        break;
                    case 4:
                        SetPropertyValue<string>("Day4", ref _Day4, value);
                        break;
                    case 5:
                        SetPropertyValue<string>("Day5", ref _Day5, value);
                        break;
                    case 6:
                        SetPropertyValue<string>("Day6", ref _Day6, value);
                        break;
                    case 7:
                        SetPropertyValue<string>("Day7", ref _Day7, value);
                        break;
                    case 8:
                        SetPropertyValue<string>("Day8", ref _Day8, value);
                        break;
                    
                }
            }
        }

        Semester _semester;
        [Association("Semester-WeekReportDatas")]
        public Semester Semester
        {
            get { return _semester; }
            set { SetPropertyValue<Semester>("Semester", ref _semester, value); }
        }
        
       

        string _PeriodTime;
        [Size(int.MaxValue)]
        public string PeriodTime
        {
            get { return _PeriodTime; }
            set { SetPropertyValue<string>("PeriodTime", ref _PeriodTime, value); }
        }
        
        string _Day2="";
        [Size(int.MaxValue)]
        public string Day2
        {
            get { return _Day2; }
            set { SetPropertyValue<string>("Day2", ref _Day2, value); }
        }
        string _Day3 = "";
        [Size(int.MaxValue)]
        public string Day3
        {
            get { return _Day3; }
            set { SetPropertyValue<string>("Day3", ref _Day3, value); }
        }
        string _Day4 = "";
        [Size(int.MaxValue)]
        public string Day4
        {
            get { return _Day4; }
            set { SetPropertyValue<string>("Day4", ref _Day4, value); }
        }
        string _Day5 = "";
        [Size(int.MaxValue)]
        public string Day5
        {
            get { return _Day5; }
            set { SetPropertyValue<string>("Day5", ref _Day5, value); }
        }
        string _Day6 = "";
        [Size(int.MaxValue)]
        public string Day6
        {
            get { return _Day6; }
            set { SetPropertyValue<string>("Day6", ref _Day6, value); }
        }
        string _Day7 = "";
        [Size(int.MaxValue)]
        public string Day7
        {
            get { return _Day7; }
            set { SetPropertyValue<string>("Day7", ref _Day7, value); }
        }
        string _Day8 = "";
        [Size(int.MaxValue)]
        public string Day8
        {
            get { return _Day8; }
            set { SetPropertyValue<string>("Day8", ref _Day8, value); }
        }             
    }
}
