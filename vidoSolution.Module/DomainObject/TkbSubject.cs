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
   
    [Persistent("TkbSubject")]
    public class TkbSubject : BaseObject
    {

        private Semester semester;
        [Association("Semester-TkbSubject", typeof(Semester))]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue("Semester", ref semester, value); }
        }
        string fid;
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
        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }


        public TkbSubject(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
