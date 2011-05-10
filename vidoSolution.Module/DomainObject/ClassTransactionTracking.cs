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
    [Persistent("ClassTransactionTracking")] 
    public class ClassTransactionTracking : BaseObject
    {
        public ClassTransactionTracking(Session session) : base(session) { }
        public ClassTransactionTracking() : base(Session.DefaultSession) { }       
        public override void AfterConstruction() { base.AfterConstruction(); }        

        StudentClass studentClass;
        [ImmediatePostData]
        [Association("StudentClass-ClassTransactionTrackings")]
        public StudentClass StudentClass
        {
            get { return studentClass; }
            set
            {
                SetPropertyValue<StudentClass>("StudentClass", ref studentClass, value);
                OnChanged("BeforeDebtAmount");
                OnChanged("NumOfStudents");
                OnChanged("PaidAmount");
                OnChanged("DeptAmount");
                OnChanged("RequestAmount");
                OnChanged("DeptPercentage");
                
            }
        }
        Semester _semester;
        [ImmediatePostData]
        [Association("Semester-ClassTransactionTrackings")]
        public Semester Semester
        {
            get { return _semester; }
            set { SetPropertyValue<Semester>("Semester", ref _semester, value); }
        }
        
        decimal? fBeforeDebtAmount = null;
        public decimal? BeforeDebtAmount
        {
            get
            {
                if (!IsLoading && !IsSaving && (fBeforeDebtAmount == null || fBeforeDebtAmount == 0))
                    UpdateBeforeDebtAmount(false);
                return fBeforeDebtAmount;
            }
        }
        public void UpdateBeforeDebtAmount(bool forceChangeEvents)
        {
            if (studentClass != null)
            {
                decimal? oldBeforeDebtAmount = fBeforeDebtAmount;
                decimal tempTotal = 0m;
                //calculate

                XPCollection<AccountTransaction> xpc = new XPCollection<AccountTransaction>(Session,
                   CriteriaOperator.Parse("Student.StudentClass.ClassCode=?",studentClass.ClassCode));              
                foreach (AccountTransaction acc in xpc)
                {
                    if ((acc.Semester != null &&
                        Convert.ToInt32(acc.Semester.SemesterName) < Convert.ToInt32(Semester.SemesterName)) ||
                        acc.TransactingDate < Semester.StartDate)
                        tempTotal += acc.MoneyAmount;
                }

                fBeforeDebtAmount = - tempTotal;
                if (forceChangeEvents)
                    OnChanged("BeforeDebtAmount", oldBeforeDebtAmount, fBeforeDebtAmount);
            }
        }
        public decimal DeptAmount
        {
            get
            {
                return (RequestAmount == null ? 0m : Convert.ToDecimal(RequestAmount)) -
                    (PaidAmount == null ? 0m : Convert.ToDecimal(PaidAmount)) + 
                     (BeforeDebtAmount == null ? 0m : Convert.ToDecimal(BeforeDebtAmount));
            }
        }

        public decimal DeptPercentage
        {
            get
            {
                decimal req=(RequestAmount == null ? 0m : Convert.ToDecimal(RequestAmount));
                decimal before= (BeforeDebtAmount == null ? 0m : Convert.ToDecimal(BeforeDebtAmount));
                if (req + before == 0)
                    return 0;
                else if (Convert.ToDecimal(DeptAmount) >= 0)
                    return Convert.ToDecimal(DeptAmount) / (req + before);
                else //
                    return -1;
            }
        }

        int fNumOfStudents = 0;
        public int NumOfStudents
        {
            get
            {
                if (!IsLoading && !IsSaving &&  (fNumOfStudents == 0))
                    UpdateNumOfStudents(false);
                return fNumOfStudents;
            }
        }
        public void UpdateNumOfStudents(bool forceChangeEvents)
        {
            if (studentClass != null)
            {
                int oldNumOfStudents = fNumOfStudents;
                fNumOfStudents = studentClass.Students.Count;
                if (forceChangeEvents)
                    OnChanged("NumOfStudents", oldNumOfStudents, fNumOfStudents);
            }
        }
        
       

        decimal? fRequestAmount = null;
        public decimal? RequestAmount
        {
            get
            {
                if (!IsLoading && !IsSaving && (fRequestAmount == null || fRequestAmount == 0))
                    UpdateRequestAmount(false);
                return fRequestAmount;
            }
        }
        public void UpdateRequestAmount(bool forceChangeEvents)
        {
            if (StudentClass != null && Semester != null)
            {
                decimal? oldfRequestAmount = fRequestAmount;
                XPCollection<AccountTransaction> xpc = new XPCollection<AccountTransaction>(Session,
                    CriteriaOperator.Parse("Student.StudentClass.ClassCode=? and Semester.SemesterName=?",
                    studentClass.ClassCode, Semester.SemesterName));
                decimal tempTotal = 0m;
                foreach (AccountTransaction acc in xpc)
                {
                    if (acc.MoneyAmount < 0)
                        tempTotal += -acc.MoneyAmount;
                }
                fRequestAmount = tempTotal;
                if (forceChangeEvents)
                    OnChanged("PaidAmount", oldfRequestAmount, fRequestAmount);
            }
        }

        decimal? fPaidAmount = null;
        public decimal? PaidAmount
        {
            get
            {
                if (!IsLoading && !IsSaving && (fPaidAmount ==null|| fPaidAmount == 0))
                    UpdatePaidAmount(false);
                return fPaidAmount;
            }
        }
        public void UpdatePaidAmount(bool forceChangeEvents)
        {
            if (StudentClass != null && Semester != null)
            {
                decimal? oldfPaidAmount = fPaidAmount;
                XPCollection<AccountTransaction> xpc = new XPCollection<AccountTransaction>(Session,
                    CriteriaOperator.Parse("Student.StudentClass.ClassCode=? and Semester.SemesterName=?",
                    studentClass.ClassCode, Semester.SemesterName));
                decimal tempTotal = 0m;
                foreach (AccountTransaction acc in xpc)
                {
                    if (acc.MoneyAmount > 0)
                        tempTotal += acc.MoneyAmount;
                }
                fPaidAmount = tempTotal;
                if (forceChangeEvents)
                    OnChanged("PaidAmount", oldfPaidAmount, fPaidAmount);
            }
        }
    }
}

