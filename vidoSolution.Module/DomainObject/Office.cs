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
    [Persistent("Offices")]
    public class Office : BaseObject
    {
        [Association("Office-Classrooms", typeof(Classroom))]
        public XPCollection Classrooms
        {
            get { return GetCollection("Classrooms"); }
        }

        string fofficecode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Office.OfficeCode", DefaultContexts.Save)]       
        public string OfficeCode
        {
            get { return fofficecode; }
            set { SetPropertyValue<string>("officecode", ref fofficecode, value); }
        }
        string fname;
        [Size(255)]
        public string Name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        string fphone;
        [Size(50)]
        public string Phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string faddress;
        [Size(255)]
        public string Address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fnote;
        [Size(255)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime? fdatecreated = null;
        public DateTime? DateCreated
        {
            get
            {
                return fdatecreated;
            }
            set
            {
                SetPropertyValue<DateTime?>("DateCreated", ref fdatecreated, value);
            }
        }
        DateTime fdatemodified;
        public DateTime DateModified
        {
            get
            {
                return fdatemodified;
            }
            set
            {
                SetPropertyValue<DateTime>("DateModified", ref fdatemodified, value);
            }
        }
        protected override void OnSaving()
        {
            DateModified = DateTime.Now;
            if (DateCreated == null) DateCreated = DateTime.Now;
            base.OnSaving();
        }
        public Office(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
