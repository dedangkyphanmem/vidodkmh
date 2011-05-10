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
    [Persistent("Notes")]
    public class MyNote : BaseObject
    {
        public MyNote(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        string _Title;
        [Size(255)]
        public string Title
        {
            get {return _Title;}
            set { SetPropertyValue("Title", ref _Title, value); }
        }
        //short content
        string _ShortContent;
        [Size(255)]
        public string Intro
        {
            get { return _ShortContent; }
            set
            {
                SetPropertyValue("ShortContent", ref _ShortContent, value);
            }
        }
        //full content
        string _FullContent;
        public string FullContent
        {
            get { return _FullContent; }
            set { SetPropertyValue("FullContent", ref _FullContent, value); }
        }
        //tag
        string _Tag;
        public string Tag
        {
            get { return _Tag; }
            set
            {
                SetPropertyValue("Tag", ref _Tag, value);
            }
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
    }

}
