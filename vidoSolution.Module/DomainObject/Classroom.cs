using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("Classrooms")]
    public class Classroom : BaseObject
    {
       Office fOffice;  
       [Association("Office-Classrooms")]
       public Office Office {
          get { return fOffice; }
          set { SetPropertyValue<Office>("Office", ref fOffice, value); }
       }

       [Association("Classrooms-WeekReportDatas", typeof(WeekReportData))]
       public XPCollection WeekReportDatas
       {
           get { return GetCollection("WeekReportDatas"); }
       }
        [Association("Classroom-TkbSemesters",typeof(TkbSemester))]
        public XPCollection TkbSemesters
        {
            get { return GetCollection("TkbSemesters"); }
        }
        string fclassroomcode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Classroom.ClassroomCode", DefaultContexts.Save)]
        public string ClassroomCode
        {
            get { return fclassroomcode; }
            set { SetPropertyValue<string>("classroomcode", ref fclassroomcode, value); }
        }
        string fclassroomname;
        [Size(10)]
        public string ClassroomName
        {
            get { return fclassroomname; }
            set { SetPropertyValue<string>("ClassroomName", ref fclassroomname, value); }
        }
        int fcapacity;
        public int Capacity
        {
            get { return fcapacity; }
            set { SetPropertyValue<int>("capacity", ref fcapacity, value); }
        }
       
        string froomtype;
        [Size(50)]
        public string RoomType
        {
            get { return froomtype; }
            set { SetPropertyValue<string>("roomtype", ref froomtype, value); }
        }
        string fnote;
        [Size(500)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated = DateTime.MinValue;
        public DateTime DateCreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("DateCreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime DateModified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("DateModified", ref fdatemodified, value); }
        }
        public Classroom(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
