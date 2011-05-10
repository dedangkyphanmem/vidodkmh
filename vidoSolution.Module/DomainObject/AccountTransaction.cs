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
    [Persistent("AccountTransactions")]
    public class AccountTransaction : BaseObject
    {

        Student fStudent;
        [Association("Student-AccountTransactions")]
        [RuleRequiredField("RuleRequiredField Student for AccountTransaction", DefaultContexts.Save)]
        public Student Student
        {
            get { return fStudent; }
            set { SetPropertyValue<Student>("Student", ref fStudent, value); }
        }

        Semester semester;
        [RuleRequiredField("RuleRequiredField Semester for AccountTransaction", DefaultContexts.Save)]
        [Association("Semester-AccountTransactions")]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue<Semester>("Semester", ref semester, value); }
        }

        public string Name
        {
            get { 
                string str4LastCharsOid= Oid.ToString();
                str4LastCharsOid =  str4LastCharsOid.Substring(str4LastCharsOid.Length-4,4); 
                if (Student!=null)
                    return String.Format("{0}-{1}-{2:ddMMyyyy}", Student.StudentCode, str4LastCharsOid, TransactingDate);
                return "";
            }
            
        }

        decimal fmoneyamount;
        [RuleRequiredField("RuleRequiredField MoneyAmount for AccountTransaction", DefaultContexts.Save)]
        public decimal MoneyAmount
        {
            get { return fmoneyamount; }
            set { SetPropertyValue<decimal>("moneyamount", ref fmoneyamount, value); }
        }
		
        DateTime ftransactingdate;
         [RuleRequiredField("RuleRequiredField TransactingDate for AccountTransaction", DefaultContexts.Save)]
        public DateTime TransactingDate
        {
            get { return ftransactingdate; }
            set { SetPropertyValue<DateTime>("transactingdate", ref ftransactingdate, value); }
        }
        string fdescription;
        [Size(500)]
        public string Description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }

        string fImportDescription;        
        public string ImportDescription
        {
            get { return fImportDescription; }
            set { SetPropertyValue<string>("ImportDescription", ref fImportDescription, value); }
        }
        DateTime fdatecreated;
        public DateTime DateCreated
        {
            get
            {
                return fdatecreated;
            }
            set
            {
                SetPropertyValue<DateTime>("DateCreated", ref fdatecreated, value);
            }
        }
        DateTime fdatemodified= DateTime.MinValue;
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
        public AccountTransaction(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}