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
   
    [Persistent("TkbClass")]
    public class TkbClass : BaseObject
    {

        private Semester semester;
        [Association("Semester-TkbClass", typeof(Semester))]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue("Semester", ref semester, value); }
        }


        string fid;
        [Size(25)]
        public string ID
        {
            get { return fid; }
            set { SetPropertyValue<string>("ID", ref fid, value); }
        }

        string fshort;
        [Size(20)]
        public string Short
        {
            get { return fshort; }
            set { SetPropertyValue<string>("Short", ref fshort, value); }
        }
        string fname;
        [Size(255)]
        public string Name
        {
            get { return fname; }
            set { SetPropertyValue<string>("Name", ref fname, value); }
        }

        string fteacherids;
        [Size(10)]
        public string TeacherIDs
        {
            get { return fteacherids; }
            set { SetPropertyValue<string>("TeacherID", ref fteacherids, value); }
        }

        string fclassroomids;
        [Size(10)]
        public string ClassroomIDs
        {
            get { return fclassroomids; }
            set { SetPropertyValue<string>("ClassroomIDs", ref fclassroomids, value); }
        }

        string fgrade;
        [Size(10)]
        public string Grade
        {
            get { return fgrade; }
            set { SetPropertyValue<string>("Grade", ref fgrade, value); }
        }

        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }
        public TkbClass(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
