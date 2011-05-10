using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base; 
using DevExpress.Persistent.Validation;
namespace CDVD
{
    [DefaultClassOptions]
   [Persistent("register_detail")]
    public class register_detail : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        semester fsemesterid;
        [Size(5)]
        public semester semesterid
        {
            get { return fsemesterid; }
            set { SetPropertyValue<semester>("semesterid", ref fsemesterid, value); }
        }
        string fstudentcode;
        [Size(10)]
        public string studentcode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }
        int flessonid;
        public int lessonid
        {
            get { return flessonid; }
            set { SetPropertyValue<int>("lessonid", ref flessonid, value); }
        }
        string fsubjectcode;
        [Size(10)]
        public string subjectcode
        {
            get { return fsubjectcode; }
            set { SetPropertyValue<string>("subjectcode", ref fsubjectcode, value); }
        }
        string fstatus;
        [Size(2147483647)]
        public string status
        {
            get { return fstatus; }
            set { SetPropertyValue<string>("status", ref fstatus, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        bool fischange;
        public bool ischange
        {
            get { return fischange; }
            set { SetPropertyValue<bool>("ischange", ref fischange, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public register_detail(Session session) : base(session) { }
      //  public register_detail() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
   [Persistent("branch")]
    public class branch : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fbrancecode;
        [Size(10)]
        public string brancecode
        {
            get { return fbrancecode; }
            set { SetPropertyValue<string>("brancecode", ref fbrancecode, value); }
        }
        string fbranchname;
        [Size(255)]
        public string branchname
        {
            get { return fbranchname; }
            set { SetPropertyValue<string>("branchname", ref fbranchname, value); }
        }
        department fdepartmentid;
        public department departmentid
        {
            get { return fdepartmentid; }
            set { SetPropertyValue<department>("departmentid", ref fdepartmentid, value); }
        }
        string flevelofeducation;
        public string levelofeducation
        {
            get { return flevelofeducation; }
            set { SetPropertyValue<string>("levelofeducation", ref flevelofeducation, value); }
        }
        string ftypeofeducation;
        public string typeofeducation
        {
            get { return ftypeofeducation; }
            set { SetPropertyValue<string>("typeofeducation", ref ftypeofeducation, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public branch(Session session) : base(session) { }
      //  public branch() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("classroom")]
    public class classroom : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fclassroomcode;
        [Size(10)]
        public string classroomcode
        {
            get { return fclassroomcode; }
            set { SetPropertyValue<string>("classroomcode", ref fclassroomcode, value); }
        }
        int fcapacity;
        public int capacity
        {
            get { return fcapacity; }
            set { SetPropertyValue<int>("capacity", ref fcapacity, value); }
        }
        office fofficeid;
        public office officeid
        {
            get { return fofficeid; }
            set { SetPropertyValue<office>("officeid", ref fofficeid, value); }
        }
        string froomtype;
        [Size(50)]
        public string roomtype
        {
            get { return froomtype; }
            set { SetPropertyValue<string>("roomtype", ref froomtype, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public classroom(Session session) : base(session) { }
      //  public classroom() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("teacher")]
    public class teacher : XPLiteObject
    {
        int foid;
        public int oid
        {
            get { return foid; }
            set
            {
                SetPropertyValue("oid", ref foid, value);
            }
        }
          string fteachercode;
        [Size(10)]
        public string teachercode
        {
            get { return fteachercode; }
            set { SetPropertyValue<string>("teachercode", ref fteachercode, value); }
        }
        string flastname;
        [Size(255)]
        [RuleRequiredField("RuleRequiredField for Teacher.LastName", DefaultContexts.Save)]
        public string lastname
        {
            get { return flastname; }
            set { SetPropertyValue<string>("lastname", ref flastname, value); }
        }
        string ffirstname;
        [Size(255)]
        public string firstname
        {
            get { return ffirstname; }
            set { SetPropertyValue<string>("firstname", ref ffirstname, value); }
        }
        DateTime fbirthdate;
        public DateTime birthdate
        {
            get { return fbirthdate; }
            set { SetPropertyValue<DateTime>("birthdate", ref fbirthdate, value); }
        }
        bool fsex;
        public bool sex
        {
            get { return fsex; }
            set { SetPropertyValue<bool>("sex", ref fsex, value); }
        }
        string femail;
        [Size(255)]
        public string email
        {
            get { return femail; }
            set { SetPropertyValue<string>("email", ref femail, value); }
        }
        string faddress;
        [Size(2147483647)]
        public string address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fphone;
        [Size(50)]
        public string phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string fmobile;
        [Size(50)]
        public string mobile
        {
            get { return fmobile; }
            set { SetPropertyValue<string>("mobile", ref fmobile, value); }
        }
        bool fisNotEmployee;
        public bool isNotEmployee
        {
            get { return fisNotEmployee; }
            set { SetPropertyValue<bool>("isNotEmployee", ref fisNotEmployee, value); }
        }
        department fdepartmentid;
        public department departmentid
        {
            get { return fdepartmentid; }
            set { SetPropertyValue<department>("departmentid", ref fdepartmentid, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public teacher(Session session) : base(session) { }
     //   public teacher() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("lesson")]
    public class lesson : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string flessoncode;
        [Size(10)]
        public string lessoncode
        {
            get { return flessoncode; }
            set { SetPropertyValue<string>("lessoncode", ref flessoncode, value); }
        }
        semester fsemesterid;
        [Size(5)]
        public semester semesterid
        {
            get { return fsemesterid; }
            set { SetPropertyValue<semester>("semesterid", ref fsemesterid, value); }
        }
        string fclassids;
        [Size(255)]
        public string classids
        {
            get { return fclassids; }
            set { SetPropertyValue<string>("classids", ref fclassids, value); }
        }
        string fgroupids;
        [Size(255)]
        public string groupids
        {
            get { return fgroupids; }
            set { SetPropertyValue<string>("groupids", ref fgroupids, value); }
        }
        int fperiodspercard;
        public int periodspercard
        {
            get { return fperiodspercard; }
            set { SetPropertyValue<int>("periodspercard", ref fperiodspercard, value); }
        }
        string fperiodsperweek;
        [Size(10)]
        public string periodsperweek
        {
            get { return fperiodsperweek; }
            set { SetPropertyValue<string>("periodsperweek", ref fperiodsperweek, value); }
        }
        string fsubjectcode;
        [Size(10)]
        public string subjectcode
        {
            get { return fsubjectcode; }
            set { SetPropertyValue<string>("subjectcode", ref fsubjectcode, value); }
        }
        int fteacherids;
        public int teacherids
        {
            get { return fteacherids; }
            set { SetPropertyValue<int>("teacherids", ref fteacherids, value); }
        }
        decimal ftuitionfee;
        public decimal tuitionfee
        {
            get { return ftuitionfee; }
            set { SetPropertyValue<decimal>("tuitionfee", ref ftuitionfee, value); }
        }
        int fnumexpectation;
        public int numexpectation
        {
            get { return fnumexpectation; }
            set { SetPropertyValue<int>("numexpectation", ref fnumexpectation, value); }
        }
        int fnumregistration;
        public int numregistration
        {
            get { return fnumregistration; }
            set { SetPropertyValue<int>("numregistration", ref fnumregistration, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public lesson(Session session) : base(session) { }
      //  public lesson() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("AccountTransaction")]
    public class AccountTransaction : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fstudentcode;
        [Size(10)]
        public string studentcode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }
        decimal fmoneyamount;
        public decimal moneyamount
        {
            get { return fmoneyamount; }
            set { SetPropertyValue<decimal>("moneyamount", ref fmoneyamount, value); }
        }
        DateTime ftransactingdate;
        public DateTime transactingdate
        {
            get { return ftransactingdate; }
            set { SetPropertyValue<DateTime>("transactingdate", ref ftransactingdate, value); }
        }
        string fdescription;
        [Size(500)]
        public string description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public AccountTransaction(Session session) : base(session) { }
     //   public AccountTransaction() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("department")]
    public class department : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fdepartmentcode;
        [Size(10)]
        public string departmentcode
        {
            get { return fdepartmentcode; }
            set { SetPropertyValue<string>("departmentcode", ref fdepartmentcode, value); }
        }
        string fname;
        [Size(255)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        string fphone;
        [Size(50)]
        public string phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string femail;
        [Size(255)]
        public string email
        {
            get { return femail; }
            set { SetPropertyValue<string>("email", ref femail, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public department(Session session) : base(session) { }
        //public department() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

   
    [DefaultClassOptions]
    [Persistent("subject")]
    public class subject : XPLiteObject
    {
        
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fsubjectcode;
        [Size(10)]
        public string subjectcode
        {
            get { return fsubjectcode; }
            set { SetPropertyValue<string>("subjectcode", ref fsubjectcode, value); }
        }
        string fsubjectname;
        [Size(255)]
        public string subjectname
        {
            get { return fsubjectname; }
            set { SetPropertyValue<string>("subjectname", ref fsubjectname, value); }
        }
        double fcredit;
        public double credit
        {
            get { return fcredit; }
            set { SetPropertyValue<double>("credit", ref fcredit, value); }
        }
        branch fbranchid;
        public branch branchid
        {
            get { return fbranchid; }
            set { SetPropertyValue<branch>("branchid", ref fbranchid, value); }
        }
        double ftheoryperiod;
        public double theoryperiod
        {
            get { return ftheoryperiod; }
            set { SetPropertyValue<double>("theoryperiod", ref ftheoryperiod, value); }
        }
        bool fcreatetheoryperiod;
        public bool createtheoryperiod
        {
            get { return fcreatetheoryperiod; }
            set { SetPropertyValue<bool>("createtheoryperiod", ref fcreatetheoryperiod, value); }
        }
        double fexerciseperiod;
        public double exerciseperiod
        {
            get { return fexerciseperiod; }
            set { SetPropertyValue<double>("exerciseperiod", ref fexerciseperiod, value); }
        }
        bool fcreateexerciseperiod;
        public bool createexerciseperiod
        {
            get { return fcreateexerciseperiod; }
            set { SetPropertyValue<bool>("createexerciseperiod", ref fcreateexerciseperiod, value); }
        }
        double fpracticeperiod;
        public double practiceperiod
        {
            get { return fpracticeperiod; }
            set { SetPropertyValue<double>("practiceperiod", ref fpracticeperiod, value); }
        }
        bool fcreatepracticeperiod;
        public bool createpracticeperiod
        {
            get { return fcreatepracticeperiod; }
            set { SetPropertyValue<bool>("createpracticeperiod", ref fcreatepracticeperiod, value); }
        }
        double fexperimentperiod;
        public double experimentperiod
        {
            get { return fexperimentperiod; }
            set { SetPropertyValue<double>("experimentperiod", ref fexperimentperiod, value); }
        }
        bool fcreateexperimentperiod;
        public bool createexperimentperiod
        {
            get { return fcreateexperimentperiod; }
            set { SetPropertyValue<bool>("createexperimentperiod", ref fcreateexperimentperiod, value); }
        }
        double ftutoringperiod;
        public double tutoringperiod
        {
            get { return ftutoringperiod; }
            set { SetPropertyValue<double>("tutoringperiod", ref ftutoringperiod, value); }
        }
        bool fcreatetutoringperiod;
        public bool createtutoringperiod
        {
            get { return fcreatetutoringperiod; }
            set { SetPropertyValue<bool>("createtutoringperiod", ref fcreatetutoringperiod, value); }
        }
        double fselflearningperiod;
        public double selflearningperiod
        {
            get { return fselflearningperiod; }
            set { SetPropertyValue<double>("selflearningperiod", ref fselflearningperiod, value); }
        }
        bool fcreateselflearningperiod;
        public bool createselflearningperiod
        {
            get { return fcreateselflearningperiod; }
            set { SetPropertyValue<bool>("createselflearningperiod", ref fcreateselflearningperiod, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public subject(Session session) : base(session) { }
        public subject() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("tkbsemester")]
    public class tkbsemester : XPLiteObject
    {
        lesson flessonid;
        public lesson lessonid
        {
            get { return flessonid; }
            set { SetPropertyValue<lesson>("lessonid", ref flessonid, value); }
        }
        int fperiod;
        public int period
        {
            get { return fperiod; }
            set { SetPropertyValue<int>("period", ref fperiod, value); }
        }
        string fweeks;
        [Size(25)]
        public string weeks
        {
            get { return fweeks; }
            set { SetPropertyValue<string>("weeks", ref fweeks, value); }
        }
        int fday;
        public int day
        {
            get { return fday; }
            set { SetPropertyValue<int>("day", ref fday, value); }
        }
        Guid foid;
        [Key(true)]
        public Guid oid
        {
            get { return foid; }
            set { SetPropertyValue<Guid>("oid", ref foid, value); }
        }
        public tkbsemester(Session session) : base(session) { }
      // public tkbsemester() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("office")]
    public class office : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fofficecode;
        [Size(10)]
        public string officecode
        {
            get { return fofficecode; }
            set { SetPropertyValue<string>("officecode", ref fofficecode, value); }
        }
        string fname;
        [Size(255)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        string fphone;
        [Size(50)]
        public string phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string faddress;
        [Size(2147483647)]
        public string address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public office(Session session) : base(session) { }
        //public office() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("semmester")]
    [System.ComponentModel.DefaultProperty("start_date")]

    public class semester : XPLiteObject
    {
        string fsemesterid;
        [Key]
        [Size(5)]
    
        public string semesterid
        {
            get { return fsemesterid; }
            set { SetPropertyValue<string>("semesterid", ref fsemesterid, value); }
        }
        DateTime fstart_date;
        [RuleRequiredField("RuleRequiredField for Semaster.SemasterID", DefaultContexts.Save)]
        public DateTime start_date
        {
            get { return fstart_date; }
            set { SetPropertyValue<DateTime>("start_date", ref fstart_date, value); }
        }
        int fnumber_of_week;
        public int number_of_week
        {
            get { return fnumber_of_week; }
            set { SetPropertyValue<int>("number_of_week", ref fnumber_of_week, value); }
        }
        string fdescription;
        [Size(2147483647)]
        public string description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public semester(Session session) : base(session) { }
        //public semester() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("sysdiagrams")]
    public class sysdiagrams : XPLiteObject
    {
        string fname;
        [Size(128)]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }
        int fprincipal_id;
        public int principal_id
        {
            get { return fprincipal_id; }
            set { SetPropertyValue<int>("principal_id", ref fprincipal_id, value); }
        }
        int fdiagram_id;
        [Key(true)]
        public int diagram_id
        {
            get { return fdiagram_id; }
            set { SetPropertyValue<int>("diagram_id", ref fdiagram_id, value); }
        }
        int fversion;
        public int version
        {
            get { return fversion; }
            set { SetPropertyValue<int>("version", ref fversion, value); }
        }
        byte[] fdefinition;
        public byte[] definition
        {
            get { return fdefinition; }
            set { SetPropertyValue<byte[]>("definition", ref fdefinition, value); }
        }
        public sysdiagrams(Session session) : base(session) { }
        //public sysdiagrams() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
   
    public class studentclass : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fclasscode;
        [Size(10)]
        public string classcode
        {
            get { return fclasscode; }
            set { SetPropertyValue<string>("classcode", ref fclasscode, value); }
        }
        string fclassname;
        public string classname
        {
            get { return fclassname; }
            set { SetPropertyValue<string>("classname", ref fclassname, value); }
        }
        int fbranchid;
        public int branchid
        {
            get { return fbranchid; }
            set { SetPropertyValue<int>("branchid", ref fbranchid, value); }
        }
        department fdepartmentid;
        public department departmentid
        {
            get { return fdepartmentid; }
            set { SetPropertyValue<department>("departmentid", ref fdepartmentid, value); }
        }
        string fenrollsemester;
        [Size(5)]
        public string enrollsemester
        {
            get { return fenrollsemester; }
            set { SetPropertyValue<string>("enrollsemester", ref fenrollsemester, value); }
        }
        string fgraduatesemester;
        [Size(5)]
        public string graduatesemester
        {
            get { return fgraduatesemester; }
            set { SetPropertyValue<string>("graduatesemester", ref fgraduatesemester, value); }
        }
        int fnumstudent;
        public int numstudent
        {
            get { return fnumstudent; }
            set { SetPropertyValue<int>("numstudent", ref fnumstudent, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public studentclass(Session session) : base(session) { }
        //public studentclass() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

   
    public class student_result : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        semester fsemesterid;
        [Size(5)]
        public semester semesterid
        {
            get { return fsemesterid; }
            set { SetPropertyValue<semester>("semesterid", ref fsemesterid, value); }
        }
        string fstudentcode;
        [Size(10)]
        public string studentcode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }
        lesson flessonid;
        public lesson lessonid
        {
            get { return flessonid; }
            set { SetPropertyValue<lesson>("lessonid", ref flessonid, value); }
        }
        string fsubjectcode;
        [Size(10)]
        public string subjectcode
        {
            get { return fsubjectcode; }
            set { SetPropertyValue<string>("subjectcode", ref fsubjectcode, value); }
        }
        double favg_mark;
        public double avg_mark
        {
            get { return favg_mark; }
            set { SetPropertyValue<double>("avg_mark", ref favg_mark, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public student_result(Session session) : base(session) { }
        //public student_result() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
   
    public class studentaccumulation : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        semester fsemesterid;
        [Size(5)]
        public semester semesterid
        {
            get { return fsemesterid; }
            set { SetPropertyValue<semester>("semesterid", ref fsemesterid, value); }
        }
        string fstudentcode;
        [Size(10)]
        public string studentcode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }
        double fsemesteravggrade;
        public double semesteravggrade
        {
            get { return fsemesteravggrade; }
            set { SetPropertyValue<double>("semesteravggrade", ref fsemesteravggrade, value); }
        }
        double fsemestercredit;
        public double semestercredit
        {
            get { return fsemestercredit; }
            set { SetPropertyValue<double>("semestercredit", ref fsemestercredit, value); }
        }
        double fcumulatingavggrade;
        public double cumulatingavggrade
        {
            get { return fcumulatingavggrade; }
            set { SetPropertyValue<double>("cumulatingavggrade", ref fcumulatingavggrade, value); }
        }
        double fcumulatingcredit;
        public double cumulatingcredit
        {
            get { return fcumulatingcredit; }
            set { SetPropertyValue<double>("cumulatingcredit", ref fcumulatingcredit, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public studentaccumulation(Session session) : base(session) { }
        //public studentaccumulation() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    
    [DefaultClassOptions]
    public class student : User
    {
        public student(Session session) : base(session) { }
        
        

        SimpleUser faccountid;
        public SimpleUser accountid
        {
            get { return faccountid; }
            set { SetPropertyValue<SimpleUser>("accountid", ref faccountid, value); }
        }
        string fstudentcode;
        [Size(10)]
        public string studentcode
        {
            get { return fstudentcode; }
            set { SetPropertyValue<string>("studentcode", ref fstudentcode, value); }
        }
        string flastname;
        [Size(255)]
        public string lastname
        {
            get { return flastname; }
            set { SetPropertyValue<string>("lastname", ref flastname, value); }
        }
        string ffirstname;
        [Size(255)]
        
        [RuleRequiredField("RuleRequireField Student.FirstName",DefaultContexts.Save)]
        public string firstname
        {
            get { return ffirstname; }
            set { SetPropertyValue<string>("firstname", ref ffirstname, value); }
        }
        int fstudystate;

        [RuleRequiredField("RuleRequireField Student.Studystate", DefaultContexts.Save)]
        public int studystate
        {
            get { return fstudystate; }
            set { SetPropertyValue<int>("studystate", ref fstudystate, value); }
        }
        DateTime fbirthday;
        public DateTime birthday
        {
            get { return fbirthday; }
            set { SetPropertyValue<DateTime>("birthday", ref fbirthday, value); }
        }
        bool fsex;
        [RuleRequiredField("RuleRequireField Student.Sex", DefaultContexts.Save)]
        public bool sex
        {
            get { return fsex; }
            set { SetPropertyValue<bool>("sex", ref fsex, value); }
        }
        string femail;
        [Size(255)]
        
        public string email
        {
            get { return femail; }
            set { SetPropertyValue<string>("email", ref femail, value); }
        }
        string faddress;
        [Size(2147483647)]
        public string address
        {
            get { return faddress; }
            set { SetPropertyValue<string>("address", ref faddress, value); }
        }
        string fphone;
        [Size(50)]
        public string phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string fmobile;
        [Size(50)]
        public string mobile
        {
            get { return fmobile; }
            set { SetPropertyValue<string>("mobile", ref fmobile, value); }
        }
        studentclass fclassid;
        public studentclass classid
        {
            get { return fclassid; }
            set { SetPropertyValue<studentclass>("classid", ref fclassid, value); }
        }
        semester finsemesterid;
        [Size(5)]
        public semester insemesterid
        {
            get { return finsemesterid; }
            set { SetPropertyValue<semester>("insemesterid", ref finsemesterid, value); }
        }
        branch fbranchid;
        public branch branchid
        {
            get { return fbranchid; }
            set { SetPropertyValue<branch>("branchid", ref fbranchid, value); }
        }
        department fdepartmentid;
        public department departmentid
        {
            get { return fdepartmentid; }
            set { SetPropertyValue<department>("departmentid", ref fdepartmentid, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        decimal faccountbalance;
        public decimal accountbalance
        {
            get
            {
                return faccountbalance;
            }
            set { SetPropertyValue<decimal>("accountbalance", ref faccountbalance, value); }

        }
        float fsemestercredit;
        public float semestercredit
        {
            get { return fsemestercredit; }
            set { SetPropertyValue<float>("semestercredit", ref fsemestercredit, value); }
        }
        DateTime fbalancelassmodify;
        public DateTime balancelassmodify
        {
            get { return fbalancelassmodify; }
            set
            {
                SetPropertyValue("balancelassmodify", ref fbalancelassmodify, value);
            }
        }
 
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("register_period")]
    public class register_period : XPLiteObject
    {
        int foid;
        [Key(true)]
        public int oid
        {
            get { return foid; }
            set { SetPropertyValue<int>("oid", ref foid, value); }
        }
        string fregname;
        [Size(255)]
        public string regname
        {
            get { return fregname; }
            set { SetPropertyValue<string>("regname", ref fregname, value); }
        }
        DateTime fstart_register_time;
        public DateTime start_register_time
        {
            get { return fstart_register_time; }
            set { SetPropertyValue<DateTime>("start_register_time", ref fstart_register_time, value); }
        }
        DateTime fend_register_time;
        public DateTime end_register_time
        {
            get { return fend_register_time; }
            set { SetPropertyValue<DateTime>("end_register_time", ref fend_register_time, value); }
        }
        DateTime fstart_approved_time;
        public DateTime start_approved_time
        {
            get { return fstart_approved_time; }
            set { SetPropertyValue<DateTime>("start_approved_time", ref fstart_approved_time, value); }
        }
        DateTime fend_approved_time;
        public DateTime end_approved_time
        {
            get { return fend_approved_time; }
            set { SetPropertyValue<DateTime>("end_approved_time", ref fend_approved_time, value); }
        }
        string fnote;
        [Size(2147483647)]
        public string note
        {
            get { return fnote; }
            set { SetPropertyValue<string>("note", ref fnote, value); }
        }
        DateTime fdatecreated;
        public DateTime datecreated
        {
            get { return fdatecreated; }
            set { SetPropertyValue<DateTime>("datecreated", ref fdatecreated, value); }
        }
        DateTime fdatemodified;
        public DateTime datemodified
        {
            get { return fdatemodified; }
            set { SetPropertyValue<DateTime>("datemodified", ref fdatemodified, value); }
        }
        public register_period(Session session) : base(session) { }
        //public register_period() : base(Session.DefaultSession) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    [DefaultClassOptions]
    [Persistent("tkbFile")]
    public class tkbFile:XPLiteObject
    {
        public tkbFile(Session session) : base(session) { }
        int foid;
        [Key(true)]
        public int oid
        {
            get
            {
                return foid;
            }
            set
            {
                SetPropertyValue("oid", ref foid, value);
            }
        }
        string fNHHK;
        [Size(5)]
        public string NHHK
        {
            get
            {
                return fNHHK;
            }
            set
            {
                SetPropertyValue("NHHK", ref fNHHK, value);
            }
        }
        private FileData xMLfiledata;
        [Action(ToolTip = "Import TKB data")]
        public void ImportTKB()
        {
            if (XMLFiledata != null)
            {
                Console.WriteLine("heeelooo" + xMLfiledata.FileName);
            }

        }
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]     
        public FileData XMLFiledata
        {
            get { return xMLfiledata; }
            set
            {
                SetPropertyValue("File", ref xMLfiledata, value);
            }
        }

        //string fXMLFile;
        //[Size(255)]
        //public string XMLFile
        //{
        //    get
        //    {
        //        return fXMLFile;
        //    }
        //    set
        //    {
        //        SetPropertyValue("XMLFile", ref fXMLFile, value);
        //    }
        //}

        private FileData zIPfiledata;
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ZIPFiledata
        {
            get { return zIPfiledata; }
            set
            {
                SetPropertyValue("File", ref zIPfiledata, value);
            }
        }
        //string fZipFile;
        //[Size(255)]
        //public string ZipFile
        //{
        //    get
        //    {
        //        return fZipFile;
        //    }
        //    set
        //    {
        //        SetPropertyValue("ZipFile", ref fZipFile, value);
        //    }
        //}
        string fWebLocalPath;
        [Size(255)]
        public string WebLocalPath
        {
            get
            {
                return fWebLocalPath;
            }
            set
            {
                SetPropertyValue("WebLocalPath", ref fWebLocalPath, value);
            }
        }
        DateTime fDateCreate;
        public DateTime DateCreate
        {
            get
            {
                return fDateCreate;
            }
            set
            {
                SetPropertyValue("DateCreate", ref fDateCreate, value);
            }
        }
        DateTime fDateModify;
        public DateTime DateModify
        {
            get
            {
                return fDateModify;
            }
            set
            {
                SetPropertyValue("DateModify", ref fDateModify, value);
            }
        }
        bool factive;
        public bool Active
        {
            get
            {
                return factive;
            }
            set
            {
                SetPropertyValue("Active", ref factive, value);
            }
        }
        string fnote;
        public string Note
        {
            get
            {
                return fnote;
            }
            set
            {
                SetPropertyValue("Note", ref fnote, value);
            }
        }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
