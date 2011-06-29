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
    [Persistent("ConstrainstParameter")]
    public class ConstrainstParameter : BaseObject
    {
        public ConstrainstParameter(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        string _Code;
        [Size(100)]
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }
        string _Title;
        [Size(5000)]
        public string Name
        {
            get { return _Title; }
            set { SetPropertyValue("Name", ref _Title, value); }
        }
        //short content
        decimal _Value;
        
        public decimal Value
        {
            get { return _Value; }
            set
            {
                SetPropertyValue("Value", ref _Value, value);
            }
        }
        //full content
        string _FullContent;
        public string Description
        {
            get { return _FullContent; }
            set { SetPropertyValue("Description", ref _FullContent, value); }
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
