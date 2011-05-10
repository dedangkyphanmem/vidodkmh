using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace vidoSolution.Module.DomainObject
{
    [NonPersistent]
    public class MyImportResult : XPLiteObject
    {
        public MyImportResult(Session session) : base(session) { }        
        public MyImportResult() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        private int fvwKey;
        [Key(true)]
        public int vwKey
        {
            get { return fvwKey; }
            set { SetPropertyValue<int>("vwKey", ref fvwKey, value); }
        }
        
        bool _CanImport;
        public bool CanImport
        {
            get { return _CanImport; }
            set { SetPropertyValue<bool>("CanImport", ref _CanImport, value); }
        }

        int _line;
        public int Line
        {
            get { return _line; }
            set { SetPropertyValue<int>("Line", ref _line, value); }
        }

        string _Status;
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }

        string _Data;        
        public string Data
        {
            get { return _Data; }
            set
            {
                SetPropertyValue("Data", ref _Data, value);
            }
        }

        string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                SetPropertyValue("Message", ref _Message, value);
            }
        }

    }
}
