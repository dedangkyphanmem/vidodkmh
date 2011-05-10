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
    [Persistent("AccountTransactionTracking")]  
    public class AccountTransactionTracking : BaseObject
    {
        public AccountTransactionTracking(Session session) : base(session) { }
        public AccountTransactionTracking() : base(Session.DefaultSession) { }       
        public override void AfterConstruction() { base.AfterConstruction(); }        

        Student student;
        [Association("Student-AccountTransactionTrackings")] 
        public Student Student
        {
            get { return student; }
            set { SetPropertyValue<Student>("Student", ref student, value); }
        }
        Semester _semester;
        [Association("Semester-AccountTransactionTrackings")]
        public Semester Semester
        {
            get { return _semester; }
            set { SetPropertyValue<Semester>("Semester", ref _semester, value); }
        }

        decimal _BeforeDebtValue;
        public Decimal BeforeDebtValue
        {
            get { return _BeforeDebtValue; }
            set { SetPropertyValue<Decimal>("BeforeDebtValue", ref _BeforeDebtValue, value); }
        }
        decimal _RequestValue;
        public Decimal RequestValue
        {
            get { return _RequestValue; }
            set { SetPropertyValue<Decimal>("RequestValue", ref _RequestValue, value); }
        }
        
        decimal _PaidValue;
        public Decimal PaidValue
        {
            get { return _PaidValue; }
            set { SetPropertyValue<Decimal>("PaidValue", ref _PaidValue, value); }
        }

        public Decimal CurrentDebtValue
        {
            get { return (RequestValue + BeforeDebtValue)- PaidValue; }
        }

        public decimal DebtPercentage
        {
            get
            {
                if ((RequestValue + BeforeDebtValue) != 0 && CurrentDebtValue >= 0)
                    return (CurrentDebtValue / (RequestValue + BeforeDebtValue));
                else if ((RequestValue + BeforeDebtValue) == 0)
                    return 0;
                else //CurrentDebtValue>0
                    return -1;
            }
        }
    }
}

