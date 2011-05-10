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
    [Persistent("TkbDay")]
    public class TkbDay : BaseObject
    {

         int fday;
        public int Day
        {
            get { return fday; }
            set { SetPropertyValue<int>("Day", ref fday, value); }
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

        public TkbDay(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
}
