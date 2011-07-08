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
        [ImmediatePostData(true)]
        [Association("Student-AccountTransactions")]
        [RuleRequiredField("RuleRequiredField Student for AccountTransaction", DefaultContexts.Save)]
        public Student Student
        {
            get { return fStudent; }
            set
            {
                SetPropertyValue<Student>("Student", ref fStudent, value); 
                //UpdateName();
            }
        }

        Semester semester;
        [ImmediatePostData(true)]
        [RuleRequiredField("RuleRequiredField Semester for AccountTransaction", DefaultContexts.Save)]
        [Association("Semester-AccountTransactions")]
        public Semester Semester
        {
            get { return semester; }
            set
            {
                SetPropertyValue<Semester>("Semester", ref semester, value); 
                //UpdateName();
            }
        }

        [Persistent("Name")]
        private string name;
        
        private void UpdateName()
        {
            string str6LastCharsOid = Oid.ToString();
            str6LastCharsOid = str6LastCharsOid.Substring(str6LastCharsOid.Length - 6, 6);
            name = String.Format("{0}-{1}-{2:yyMMdd}", str6LastCharsOid, Student != null ? Student.StudentCode :"", TransactingDate);
           
        }
        [PersistentAlias("name")]
        public string Name
        {
            get { return EvaluateAlias("Name").ToString(); }
            
        }

        decimal fmoneyamount;       
        [RuleRequiredField("RuleRequiredField MoneyAmount for AccountTransaction", DefaultContexts.Save)]
        public decimal MoneyAmount
        {
            get { return fmoneyamount; }
            set { SetPropertyValue<decimal>("moneyamount", ref fmoneyamount, value); }
        }
		
        DateTime ftransactingdate;
        [ImmediatePostData(true)]
         [RuleRequiredField("RuleRequiredField TransactingDate for AccountTransaction", DefaultContexts.Save)]
        public DateTime TransactingDate
        {
            get { return ftransactingdate; }
            set
            {
                SetPropertyValue<DateTime>("transactingdate", ref ftransactingdate, value);
                //UpdateName();
            }
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
        DateTime? fdatecreated=null;
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
        public AccountTransaction(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
       //     UpdateName();
        }
        protected override void OnSaving()
        {
            DateModified = DateTime.Now;
            if (DateCreated == null) DateCreated = DateTime.Now;
            UpdateName();
            base.OnSaving();
        }
        protected override void OnLoaded()
        {
            base.OnLoaded();
     //       UpdateName();
        }
    }

}
