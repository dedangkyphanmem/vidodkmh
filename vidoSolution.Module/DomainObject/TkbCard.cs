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
   
    [Persistent("TkbCard")]
    public class TkbCard : BaseObject
    {

        private Semester semester;
        [Association("Semester-TkbCard", typeof(Semester))]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue("Semester", ref semester, value); }
        }

        string flessonid;
        public string LessonID
        {
            get { return flessonid; }
            set { SetPropertyValue<string>("LessonID", ref flessonid, value); }
        }

        string fclassroomids;
        [Size(20)]
        public string ClassroomIDs
        {
            get { return fclassroomids; }
            set { SetPropertyValue<string>("ClassroomIDs", ref fclassroomids, value); }
        }

        int fday;
        public int Day
        {
            get { return fday; }
            set { SetPropertyValue<int>("Day", ref fday, value); }
        }

        int fperiod;
        [Size(10)]
        public int Period
        {
            get { return fperiod; }
            set { SetPropertyValue<int>("Period", ref fperiod, value); }
        }
        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }
        public TkbCard(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
