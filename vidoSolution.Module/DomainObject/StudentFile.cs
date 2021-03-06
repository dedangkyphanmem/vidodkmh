using System;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Globalization;
using DevExpress.Data.Filtering;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("StudentFiles")]
    public class StudentFile : BaseObject
    {
        public StudentFile(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }


        FileData csvFile;
        public FileData CsvFile
        {
            get { return csvFile; }
            set { SetPropertyValue("CsvFile", ref csvFile, value); }
        }



        string note;
        [Size(255)]
        public string Note
        {
            get { return note; }
            set { SetPropertyValue("Note", ref note, value); }
        }
        bool _isImported;
        public bool IsImported
        {
            get { return _isImported; }
            set
            {
                SetPropertyValue<bool>("IsImported", ref _isImported, value);
            }
        }
        string _link;

        public string ResultLink
        {
            get { return _link; }
            set { SetPropertyValue("Link", ref _link, value); }
        }
       
        protected override void OnSaving()
        {
          
            if (createDate == null) createDate = DateTime.Now;
            base.OnSaving();
        }
        DateTime? createDate=null;
        public DateTime? CreateDate
        {
            get { return createDate; }
            set
            {
                if (createDate == null)
                    SetPropertyValue<DateTime?>("CreateDate", ref createDate, DateTime.Now);
            }
        }
       
    }
}
