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
    [NonPersistent]
    public class PopUpMessage : BaseObject
    {
        public PopUpMessage(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        string _Title;
        [Size(255)]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue("Title", ref _Title, value); }
        }
        //short content
        string fMessage;
        [Size(255)]
        public string Message
        {
            get { return fMessage; }
            set
            {
                SetPropertyValue("Message", ref fMessage, value);
            }
        }       
    }

}

