using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("RegisterTime")]
    public class RegisterTime : BaseObject
    {
        public RegisterTime(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        string _Name;
        [Size(255)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }


        [Association("StudentClasses-RegisterTimes", typeof(StudentClass))]
        public XPCollection StudentClasses
        {
            get { return GetCollection("StudentClasses"); }
        }

        bool _active;
        public bool Active
        {
            get { return _active; }
            set { SetPropertyValue("Active", ref _active, value); }
        }

        string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }
       DateTime fbookingdatestart;
       public DateTime BookingDateStart
       {
           get { return fbookingdatestart; }
           set
           { SetPropertyValue<DateTime>("BookingDateStart", ref fbookingdatestart, value); }
       }
       DateTime fbookingdateend;
       public DateTime BookingDateEnd
       {
           get { return fbookingdateend; }
           set
           { SetPropertyValue<DateTime>("BookingDateEnd", ref fbookingdateend, value); }
       }
             DateTime fconfirmdatestart;
       public DateTime ConfirmDateStart
       {
           get { return fconfirmdatestart; }
           set
           { SetPropertyValue<DateTime>("ConfirmDateStart", ref fconfirmdatestart, value); }
       }
       DateTime fConfirmdateend;
       public DateTime ConfirmDateEnd
       {
           get { return fConfirmdateend; }
           set
           { SetPropertyValue<DateTime>("ConfirmDateEnd", ref fConfirmdateend, value); }
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
