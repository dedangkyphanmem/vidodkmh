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
    [Persistent("SubjectRelations")]
    public class SubjectRelations : BaseObject
    {
        Subject _Subject1;
        [Association("SubjectRelation-Subject1", typeof(Subject))]
        public Subject Subject1
        {
            get { return _Subject1; }
            set { SetPropertyValue<Subject>("Subject1", ref _Subject1, value); }
        }
        Subject _Subject2;
        
        [Association("SubjectRelation-Subject2", typeof(Subject))]
        public Subject Subject2
        {
            get { return _Subject2; }
            set { SetPropertyValue<Subject>("Subject2", ref _Subject2, value); }
        }

        int _Type;
        public int Type
        {
            get { return _Type; }
            set { SetPropertyValue<int>("Type", ref _Type,value ); }
        }

       
        public string Note
        {
            get
            {
                switch (Type)
                {
                    case 0: return "Tương đương";
                    case 1: return "Học trước";
                    case 2: return "Tiên quyết";
                    default: return "-";
                };
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
        public SubjectRelations(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
