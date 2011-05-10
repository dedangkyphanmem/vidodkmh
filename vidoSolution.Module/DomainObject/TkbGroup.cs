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
    
    [Persistent("TkbGroup")]
    public class TkbGroup : BaseObject
    {

        private Semester semester;
        [Association("Semester-TkbGroup", typeof(Semester))]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue("Semester", ref semester, value); }
        }
        string fID;
        public string ID
        {
            get { return fID; }
            set { SetPropertyValue<string>("ID", ref fID, value); }
        }
        string fclassid;

        [Size(10)]
        public string Classid
        {
            get { return fclassid; }
            set { SetPropertyValue<string>("Classid", ref fclassid, value); }
        }
        string fname;
        [Size(255)]
        public string Name
        {
            get { return fname; }
            set { SetPropertyValue<string>("Name", ref fname, value); }
        }

        bool fentireclass;
        public bool EntireClass
        {
            get { return fentireclass; }
            set { SetPropertyValue<bool>("EntireClass", ref fentireclass, value); }
        }

        int fdivisiontag;
        public int DivisionTag
        {
            get { return fdivisiontag; }
            set { SetPropertyValue<int>("DivisionTag", ref fdivisiontag, value); }
        }

        int fstudentcount;
        public int StudentCount
        {
            get { return fstudentcount; }
            set { SetPropertyValue<int>("StudentCount", ref fstudentcount, value); }
        }
        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }
        public TkbGroup(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
