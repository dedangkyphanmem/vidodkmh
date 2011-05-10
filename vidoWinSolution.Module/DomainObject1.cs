using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace vidoWinSolution.Module {
    [DefaultClassOptions]
    public class DomainObject1 : BaseObject {
        public DomainObject1(Session session) : base(session) { }
        private string _Property1;
        public string Property1 {
            get { return _Property1; }
            set { SetPropertyValue("Property1", ref _Property1, value); }
        }
        private string _Property2;
        public string Property2 {
            get { return _Property2; }
            set { SetPropertyValue("Property2", ref _Property2, value); }
        }
    }
}
