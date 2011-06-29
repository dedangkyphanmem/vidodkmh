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
    [Persistent("Subjects")]
    public class Subject : BaseObject
    {
        [Association("SubjectRelation-Subject1", typeof(SubjectRelations))]
        public XPCollection Subject1s
        {
            get { return GetCollection("Subject1s"); }
        }
        [Association("SubjectRelation-Subject2", typeof(SubjectRelations))]
        public XPCollection Subject2s
        {
            get { return GetCollection("Subject2s"); }
        }
        [Association("Subjects-WeekReportDatas", typeof(WeekReportData))]
        public XPCollection WeekReportDatas
        {
            get { return GetCollection("WeekReportDatas"); }
        }
        [Association("Subject-Lessons", typeof(Lesson))]
        public XPCollection Lessons
        {
            get { return GetCollection("Lessons"); }
        }
        
        Branch fBranch;		
		[Association("Branch-Subjects", typeof(Branch))]
        public Branch Branch
        {
            get { return fBranch; }
            set { SetPropertyValue<Branch>("Branch", ref fBranch, value); }
        }
		
      
        string fsubjectcode;
        [Size(10)]
        [RuleUniqueValue("RuleRequireField Subject.SubjectCode", DefaultContexts.Save)]     
        public string SubjectCode
        {
            get { return fsubjectcode; }
            set { SetPropertyValue<string>("subjectcode", ref fsubjectcode, value); }
        }
        string fsubjectname;
        [Size(255)]
        public string SubjectName
        {
            get { return fsubjectname; }
            set { SetPropertyValue<string>("subjectname", ref fsubjectname, value); }
        }
        double fcredit;
        public double Credit
        {
            get { return fcredit; }
            set { SetPropertyValue<double>("credit", ref fcredit, value); }
        }
        #region comment
        //double ftheoryperiod;
        //public double TheoryPeriod
        //{
        //    get { return ftheoryperiod; }
        //    set { SetPropertyValue<double>("theoryperiod", ref ftheoryperiod, value); }
        //}
        //bool fcreatetheoryperiod;
        //public bool CreateTheoryPeriod
        //{
        //    get { return fcreatetheoryperiod; }
        //    set { SetPropertyValue<bool>("createtheoryperiod", ref fcreatetheoryperiod, value); }
        //}
        //double fexerciseperiod;
        //public double ExercisePeriod
        //{
        //    get { return fexerciseperiod; }
        //    set { SetPropertyValue<double>("exerciseperiod", ref fexerciseperiod, value); }
        //}
        //bool fcreateexerciseperiod;
        //public bool CreateExercisePeriod
        //{
        //    get { return fcreateexerciseperiod; }
        //    set { SetPropertyValue<bool>("createexerciseperiod", ref fcreateexerciseperiod, value); }
        //}
        //double fpracticeperiod;
        //public double PracticePeriod
        //{
        //    get { return fpracticeperiod; }
        //    set { SetPropertyValue<double>("practiceperiod", ref fpracticeperiod, value); }
        //}
        //bool fcreatepracticeperiod;
        //public bool CreatePracticePeriod
        //{
        //    get { return fcreatepracticeperiod; }
        //    set { SetPropertyValue<bool>("createpracticeperiod", ref fcreatepracticeperiod, value); }
        //}
        //double fexperimentperiod;
        //public double ExperimentPeriod
        //{
        //    get { return fexperimentperiod; }
        //    set { SetPropertyValue<double>("experimentperiod", ref fexperimentperiod, value); }
        //}
        //bool fcreateexperimentperiod;
        //public bool CreateExperimentPeriod
        //{
        //    get { return fcreateexperimentperiod; }
        //    set { SetPropertyValue<bool>("createexperimentperiod", ref fcreateexperimentperiod, value); }
        //}
        //double ftutoringperiod;
        //public double TutoringPeriod
        //{
        //    get { return ftutoringperiod; }
        //    set { SetPropertyValue<double>("tutoringperiod", ref ftutoringperiod, value); }
        //}
        //bool fcreatetutoringperiod;
        //public bool CreateTutoringPeriod
        //{
        //    get { return fcreatetutoringperiod; }
        //    set { SetPropertyValue<bool>("createtutoringperiod", ref fcreatetutoringperiod, value); }
        //}
        //double fselflearningperiod;
        //public double SelfLearningPeriod
        //{
        //    get { return fselflearningperiod; }
        //    set { SetPropertyValue<double>("selflearningperiod", ref fselflearningperiod, value); }
        //}
        //bool fcreateselflearningperiod;
        //public bool CreateSelfLearningPeriod
        //{
        //    get { return fcreateselflearningperiod; }
        //    set { SetPropertyValue<bool>("createselflearningperiod", ref fcreateselflearningperiod, value); }
        //}
        #endregion

        string fnote;
        [Size(500)]
        public string Note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
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
        public Subject(Session session) : base(session) { }
        
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
