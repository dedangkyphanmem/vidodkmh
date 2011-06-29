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
    [Persistent("RegisterState")]
    public class RegisterState : BaseObject
    {
        public RegisterState(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        [Association("RegisterState-RegisterDetails", typeof(RegisterDetail))]
        public XPCollection RegRegisterDetails
        {
            get { return GetCollection("RegRegisterDetails"); }
        }
        [Association("CheckState-RegisterDetails", typeof(RegisterDetail))]

        public XPCollection CheckRegisterDetails
        {
            get { return GetCollection("CheckRegisterDetails"); }
        }

        string fCode;
        [Size(20)]
        public string Code
        {
            get { return fCode; }
            set { SetPropertyValue("Code", ref fCode, value); }
        }
        //short content
        string _name;
        [Size(100)]
        public string Name
        {
            get { return _name; }
            set
            {
                SetPropertyValue("Name", ref _name, value);
            }
        }
        //full content
        string _FullContent;
        [Size(500)]
        public string Description
        {
            get { return _FullContent; }
            set { SetPropertyValue("Description", ref _FullContent, value); }
        }
        //tag
        string _Tag;
        [Size(100)]
        public string Type
        {
            get { return _Tag; }
            set
            {
                SetPropertyValue("Type", ref _Tag, value);
            }
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
    }
}
