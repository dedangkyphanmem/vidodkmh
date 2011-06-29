using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("Helps")]
    public class MyHelp : BaseObject
    {
        public MyHelp(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        string _Title;
        [Size(255)]
        public string Title
        {
            get { return _Title; }
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
        //tag
        string _HyperLink;
        public string HyperLink
        {
            get { return _HyperLink; }
            set
            {
                SetPropertyValue("HyperLink", ref _HyperLink, value);
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
