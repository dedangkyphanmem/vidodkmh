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
    [Persistent("TkbPeriod")]
    public class TkbPeriod : BaseObject
    {

        int fName;
        public int Name
        {
            get { return fName; }
            set { SetPropertyValue<int>("Name", ref fName, value); }
        }
        String fstarttime;

        public String StartTime
        {
            get { return fstarttime; }
            set { SetPropertyValue<String>("StartTime", ref fstarttime, value); }
        }

        String fendtime;

        public String EndTime
        {
            get { return fendtime; }
            set { SetPropertyValue<String>("EndTime", ref fendtime, value); }
        }
        string _TimePeriod;
        [Size(100)]
        public string TimePeriod
        {
            get { return _TimePeriod; }
            set { SetPropertyValue<string>("TimePeriod", ref _TimePeriod, value); }
        }


        string fnote;
        [Size(100)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("Note", ref fnote, value); }
        }

        public TkbPeriod(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
