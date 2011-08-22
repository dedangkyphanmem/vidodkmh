using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vidoSolution.Module.DomainObject;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace vidoSolution.Module.Utilities
{
    
    public class Vacancy
    {
        public int day;
        public string period;
        public string weeks;
        public string room;
        public Vacancy(int d, string p, string w, string r)
        {
            day = d;
            period = p;
            weeks = w;
            room = r;
        }
    }
    public static class Utils
    {        
        static Dictionary<Type, string> domainClassType = new Dictionary<Type, string>()
        {
            {typeof(Classroom), "Phòng học"},{typeof(Teacher), "Giảng viên"},
            {typeof(Student), "Sinh viên"},{typeof(Office), "Cơ sở"},
            {typeof(StudentClass), "Lớp biên chế"},{typeof(Lesson), "Nhóm Lớp MH"},
            {typeof(Subject), "Môn học"},{typeof(Branch), "Ngành"},{typeof(Department), "Khoa"}

        };
        public static Dictionary<Type, string> DomainClassTypeName
        {
            get { return domainClassType; }
        }

        public static bool IsConfictPrerequisite(ObjectSpace objectspace, string studentCode, string subjectCode, out int checkresult, out string subjectcoderesult)
        {
            XPCollection<SubjectRelations> xpcSubjectRelation = new XPCollection<SubjectRelations>(objectspace.Session,
                new BinaryOperator("Subject2.SubjectCode",subjectCode));
            checkresult = 0;
            subjectcoderesult = "";
            if (xpcSubjectRelation.Count == 0)
                return false;
            else
            {
                foreach (SubjectRelations subjectRelation in xpcSubjectRelation)
                {
                    if (subjectRelation.Type == 1) //học trước
                    {
                        XPCollection<StudentResult> xpcStudResult = new XPCollection<StudentResult>(objectspace.Session,
                            new GroupOperator(GroupOperatorType.And,new BinaryOperator("Student.StudentCode",studentCode),
                            new BinaryOperator("Lesson.Subject.SubjectCode", subjectRelation.Subject1.SubjectCode)));
                        if (xpcStudResult.Count == 0)
                        {
                            checkresult = 1;
                            subjectcoderesult = subjectRelation.Subject1.SubjectCode;
                            return true;
                        }
                    }
                    else if (subjectRelation.Type == 2) // tiên quyết
                    {
                        XPCollection<StudentResult> xpcStudResult = new XPCollection<StudentResult>(objectspace.Session,
                            new GroupOperator(GroupOperatorType.And, new BinaryOperator("Student.StudentCode", studentCode),
                            new BinaryOperator("Lesson.Subject.SubjectCode", subjectRelation.Subject1.SubjectCode),
                            CriteriaOperator.Parse("AvgMark4>=1.5")));
                        if (xpcStudResult.Count == 0)                            
                        {
                            checkresult = 2;
                            subjectcoderesult = subjectRelation.Subject1.SubjectCode;
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public static string ShortStringWeek(string strWeek)
        {
            string strResult = "";
            
            int i=0, start=-1, stop=-1;
            while (i < strWeek.Length)
            {
                while (i < strWeek.Length && strWeek[i] == '-')
                    i++;
                start = i < strWeek.Length ? i : -1;
                while (i < strWeek.Length && strWeek[i] != '-')
                    i++;
                stop = i - 1;
                if (start != -1)
                    strResult += (strResult == "" ? "" : ",") + string.Format("{0}-{1}", start+1, stop+1);
            }
            return strResult;
        }
        public static string ShortStringPeriod(string period)
        {
            string strResult = "";
            string[] strs = period.Split(',');
            if (strs.Length==1)
                return strs[0];
            if (strs.Length==0)
                return "";

            string start = strs[0];
            int i=0;
            while (i<strs.Length-1)
            {
                while ((i < strs.Length - 1) && (Convert.ToInt32(strs[i + 1]) == Convert.ToInt32(strs[i]) + 1))
                    i++;
                if (i < strs.Length - 1)
                {
                    if (start != strs[i])
                    {
                        strResult += (strResult == "" ? "" : ",") + string.Format("{0}-{1}", start, strs[i]);
                    }
                    else
                        strResult += (strResult == "" ? "" : ",") + string.Format("{0}", start);
                    start = strs[++i];
                }
                else
                {
                    strResult += (strResult == "" ? "" : ",") + string.Format("{0}-{1}", start,strs[i]);
                }
            }
            
            return strResult;
        }

        public static bool IsConfictTKB(List<Vacancy> lvc, Vacancy vc)
        {
            Vacancy nvc = lvc.Find(v => (v.day == vc.day && TestPeriod(v.period, vc.period) && TestWeek(v.weeks, vc.weeks)));
            return (nvc != null);
        }

        public static bool TestPeriod(string p1, string p2)
        {
            string[] lp1 = p1.Split(new char[] { ',' });
            string[] lp2 = p2.Split(new char[] { ',' });
            return lp1.Intersect(lp2).Count() > 0;
        }
        public static bool TestWeek(string w1, string w2)
        {
            int i1 = Convert.ToInt32(w1, 2);
            int i2 = Convert.ToInt32(w2, 2);
            return (i1 & i2) != 0;
        }

        public static Semester NextSemester(Semester curSemester)
        {
            int nhhk = Convert.ToInt32(curSemester.SemesterName);
            nhhk += 1; //NHHK kế tiếp
            Semester sem = curSemester.Session.FindObject<Semester>(new BinaryOperator(
                "SemesterName", nhhk, BinaryOperatorType.Equal));
            if (sem == null) //thử nhhk của năm mới 
            {
                nhhk = (nhhk / 10 + 1) * 10 + 1;
                sem = curSemester.Session.FindObject<Semester>(new BinaryOperator(
                "SemesterName", nhhk, BinaryOperatorType.Equal));
            }

            if (sem == null)
                throw new UserFriendlyException("Người Quản trị chưa thiết lập NHHK tiếp theo, vui lòng liên hệ quản trị viên.");
            return sem;
        }

        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1) + " đồng.";
        }


        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }

    }
    public static class Data
    {

        public static void CreateOfficeTimetableData(Session session, string officeCode, string semesterName)
        {
            XPCollection<Classroom> xpc = new XPCollection<Classroom>(session,
                new BinaryOperator("Office.OfficeCode", officeCode));
            foreach (Classroom classroom in xpc)
            {
                CreateClassroomTimetableData(session, classroom.ClassroomCode, semesterName);
            }
        }

        public static void CreateStudentAccumulation(Session session, string studentCode)
        {
            bool isTrans = false;
            try
            {
                using (XPCollection<Semester> xpw = new XPCollection<Semester>(session,
                               new ContainsOperator("Lessons", new ContainsOperator("StudentResults", new 
                                   BinaryOperator("Student.StudentCode",studentCode)))))
                {
                    foreach (Semester s in xpw)
                    {
                        XPCollection<StudentAccumulation> xpa = new XPCollection<StudentAccumulation>(session,
                            new GroupOperator(GroupOperatorType.And,
                                new BinaryOperator("Student.StudentCode", studentCode),
                                new BinaryOperator("Semester.SemesterName", s.SemesterName)));
                        if (xpa.Count == 0)
                        {
                            StudentAccumulation sa = new StudentAccumulation(session)
                            {
                                Student = session.FindObject<Student>(new BinaryOperator("StudentCode",studentCode)),
                                Semester = s
                            };
                            sa.Save();
                            isTrans = true;   
                        }
                    }       
                    if (isTrans)
                        session.CommitTransaction();                        
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
        }

        public static XPCollection<AccountTransactionTracking> CreateStudentClassTransactionTrackingData(ObjectSpace objectSpace, string classCode, string semesterName)
        {
            try
            {
                using (XPCollection<AccountTransactionTracking> xpw = new XPCollection<AccountTransactionTracking>(objectSpace.Session,
                               new BinaryOperator("Student.StudentClass.ClassCode", classCode)))
                {
                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();

                    StudentClass studentClass = objectSpace.FindObject<StudentClass>(
                        new BinaryOperator("ClassCode", classCode));
                    Semester semester = objectSpace.FindObject<Semester>(
                        new BinaryOperator("SemesterName", semesterName));
                    DateTime semesterLastDate = semester.StartDate.AddDays(semester.NumberOfWeek * 7);
 
                    decimal beforedebtvalue=0;
                    decimal requestvalue= 0;
                    decimal paidvalue = 0;
                    
                    if (studentClass != null && semester != null)
                    {
                        foreach (Student student in studentClass.Students)
                        {
                            beforedebtvalue = 0;
                            requestvalue = 0;
                            paidvalue = 0;
                            foreach (AccountTransaction accTrans in student.AccountTransactions)
                            {
                                if ((accTrans.Semester != null && 
                                    Convert.ToInt32(accTrans.Semester.SemesterName)<Convert.ToInt32(semester.SemesterName))||
                                    accTrans.TransactingDate< semester.StartDate)
                                    beforedebtvalue += accTrans.MoneyAmount;
                                else if (accTrans.TransactingDate <= semesterLastDate || 
                                    (accTrans.Semester!=null && accTrans.Semester.SemesterName ==semester.SemesterName))
                                {
                                    if (accTrans.MoneyAmount < 0) //money has to paid
                                    {
                                        requestvalue += (-accTrans.MoneyAmount);
                                    }
                                    else //money has paid
                                    {
                                        paidvalue += accTrans.MoneyAmount;
                                    }
                                }
                            }                           

                            AccountTransactionTracking accTracking = objectSpace.CreateObject<AccountTransactionTracking>();
                            accTracking.Student = student;
                            accTracking.Semester = semester;
                            accTracking.BeforeDebtValue = -beforedebtvalue;
                            accTracking.PaidValue= paidvalue;
                            accTracking.RequestValue = requestvalue;
                            accTracking.Save();
                        }
                        objectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            return new XPCollection<AccountTransactionTracking>(objectSpace.Session,
                       new BinaryOperator("Student.StudentClass.ClassCode", classCode));

        }
        public static XPCollection<AccountTransactionTracking> CreateStudentClassTransactionTrackingData(Session session, string classCode, string semesterName)
        {
            try
            {
                using (XPCollection<AccountTransactionTracking> xpw = new XPCollection<AccountTransactionTracking>(session,
                               new BinaryOperator("Student.StudentClass.ClassCode", classCode)))
                {
                    session.Delete(xpw);
                    session.CommitTransaction();

                    StudentClass studentClass = session.FindObject<StudentClass>(
                        new BinaryOperator("ClassCode", classCode));
                    Semester semester = session.FindObject<Semester>(
                        new BinaryOperator("SemesterName", semesterName));
                    DateTime semesterLastDate = semester.StartDate.AddDays(semester.NumberOfWeek * 7);

                    decimal beforedebtvalue = 0;
                    decimal requestvalue = 0;
                    decimal paidvalue = 0;

                    if (studentClass != null && semester != null)
                    {
                        //NestedUnitOfWork UOW = session.BeginNestedUnitOfWork();
                        //UOW.BeginTransaction();
                        foreach (Student student in studentClass.Students)
                        {
                            beforedebtvalue = 0;
                            requestvalue = 0;
                            paidvalue = 0;
                            foreach (AccountTransaction accTrans in student.AccountTransactions)
                            {
                                if ((accTrans.Semester != null &&
                                    Convert.ToInt32(accTrans.Semester.SemesterName) < Convert.ToInt32(semester.SemesterName)) ||
                                    accTrans.TransactingDate < semester.StartDate)
                                    
                                    beforedebtvalue += accTrans.MoneyAmount;

                                else if ((accTrans.Semester != null && accTrans.Semester.SemesterName == semester.SemesterName)||
                                    accTrans.TransactingDate <= semesterLastDate )
                                {
                                    if (accTrans.MoneyAmount < 0) //money has to paid
                                    {
                                        requestvalue += (-accTrans.MoneyAmount);
                                    }
                                    else //money has paid
                                    {
                                        paidvalue += accTrans.MoneyAmount;
                                    }
                                }
                            }

                            AccountTransactionTracking accTracking = new AccountTransactionTracking(session);
                            accTracking.Student = student;
                            accTracking.Semester = semester;
                            accTracking.BeforeDebtValue = - beforedebtvalue;
                            accTracking.PaidValue = paidvalue;
                            accTracking.RequestValue = requestvalue;
                            accTracking.Save();
                        }
                        //UOW.CommitTransaction();
                        session.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            return new XPCollection<AccountTransactionTracking>(session,
                       new BinaryOperator("Student.StudentClass.ClassCode", classCode));

        }

        public static XPCollection<WeekReportData> CreateStudentTimetableData(Session session, string studentCode, string semesterName)
        {
            ConstrainstParameter cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(session,
                           new ContainsOperator("Students", new BinaryOperator("StudentCode", studentCode))))
                {

                    session.Delete(xpw);
                    session.CommitTransaction();


                    using (XPCollection<Lesson> xpcLesson = new XPCollection<Lesson>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName", semesterName),
                            new ContainsOperator("RegisterDetails",
                                new BinaryOperator("Student.StudentCode", studentCode)))))
                    {


                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;
                        NestedUnitOfWork UOW =session.BeginNestedUnitOfWork();
                        UOW.BeginTransaction();
                        foreach (Lesson lesson in xpcLesson)
                            foreach (TkbSemester tkbSem in lesson.TKBSemesters)
                            {
                                string[] strperiod = tkbSem.Period.Split(',');
                                List<string> periodTimeList = new List<string>();
                                foreach (string s in strperiod)
                                {
                                    if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                        if (!periodTimeList.Contains("Sáng"))
                                            periodTimeList.Add("Sáng");
                                    if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Chiều"))
                                            periodTimeList.Add("Chiều");
                                    if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Tối"))
                                            periodTimeList.Add("Tối");
                                }

                                foreach (String period in periodTimeList)
                                {
                                    if (!dicTimePeriodforWeekData.ContainsKey(period))
                                    {
                                        currentWeekData = new WeekReportData(UOW);
                                        currentWeekData.Semester = UOW.FindObject<Semester>(
                                            CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                        currentWeekData.Students.Add(UOW.FindObject<Student>(
                                            CriteriaOperator.Parse("StudentCode=?", studentCode)));
                                        currentWeekData.PeriodTime = period;
                                        dicTimePeriodforWeekData.Add(period, currentWeekData);
                                    }

                                    dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                            tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                            Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                                }

                            }
                            UOW.CommitTransaction();
                            session.CommitTransaction();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }

            return new XPCollection<WeekReportData>(session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Students", new BinaryOperator("StudentCode", studentCode))));

        }
        public static XPCollection<WeekReportData> CreateStudentTimetableData(ObjectSpace objectSpace, string studentCode, string semesterName)
        {
            ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
                           //new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",semesterName), 
                            new ContainsOperator("Students", new BinaryOperator("StudentCode", studentCode))))
                {

                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();
                   

                    using(XPCollection<Lesson> xpcLesson = new XPCollection<Lesson>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",semesterName), 
                            new ContainsOperator("RegisterDetails", 
                                new BinaryOperator("Student.StudentCode", studentCode)))))                    
                    
                    {


                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;
                        foreach (Lesson lesson in xpcLesson)
                        foreach (TkbSemester tkbSem in lesson.TKBSemesters)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = objectSpace.CreateObject<WeekReportData>();
                                    currentWeekData.Semester = objectSpace.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.Students.Add(objectSpace.FindObject<Student>(
                                        CriteriaOperator.Parse("StudentCode=?", studentCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        objectSpace.CommitChanges();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }

            return new XPCollection<WeekReportData>(objectSpace.Session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Students", new BinaryOperator("StudentCode", studentCode))));

        }
        
        public static XPCollection<WeekReportData> CreateClassroomTimetableData(Session session, string classroomCode, string semesterName)
        {
            ConstrainstParameter cp = session.FindObject<ConstrainstParameter>(
              new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(session,
                            new ContainsOperator("Classrooms", new BinaryOperator("ClassroomCode", classroomCode))))
                {

                    session.Delete(xpw);
                    session.CommitTransaction();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                            semesterName), new BinaryOperator("Classroom.ClassroomCode", classroomCode))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;
                        NestedUnitOfWork UOW = session.BeginNestedUnitOfWork();
                        UOW.BeginTransaction();
                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = new WeekReportData(UOW);
                                    currentWeekData.Semester = UOW.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.Classrooms.Add(UOW.FindObject<Classroom>(
                                        CriteriaOperator.Parse("ClassroomCode=?", classroomCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        UOW.CommitTransaction();
                        session.CommitTransaction();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }

            return new XPCollection<WeekReportData>(session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Classrooms", new BinaryOperator("ClassroomCode", classroomCode))));

        }
        public static XPCollection<WeekReportData> CreateClassroomTimetableData(ObjectSpace objectSpace, string classroomCode, string semesterName)
        {
            ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
                           new ContainsOperator("Classrooms", new BinaryOperator("ClassroomCode", classroomCode))))
                {

                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                            semesterName), new BinaryOperator("Classroom.ClassroomCode", classroomCode))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;

                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = objectSpace.CreateObject<WeekReportData>();
                                    currentWeekData.Semester = objectSpace.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));                                    
                                    currentWeekData.Classrooms.Add(objectSpace.FindObject<Classroom>(
                                        CriteriaOperator.Parse("ClassroomCode=?", classroomCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        objectSpace.CommitChanges();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            
            return new XPCollection<WeekReportData>(objectSpace.Session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Classrooms", new BinaryOperator("ClassroomCode", classroomCode))));

        }
        
        public static XPCollection<WeekReportData> CreateSubjectTimetableData(Session session, string subjectCode, string semesterName)
        {
            ConstrainstParameter cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(session,
                           new ContainsOperator("Subjects", new BinaryOperator("SubjectCode", subjectCode))))
                {

                    session.Delete(xpw);
                    session.CommitTransaction();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                            semesterName), new BinaryOperator("Lesson.Subject.SubjectCode", subjectCode))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;
                        NestedUnitOfWork UOW =session.BeginNestedUnitOfWork();
                        UOW.BeginTransaction();
                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = new WeekReportData(UOW);
                                    currentWeekData.Semester = UOW.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.Subjects.Add(UOW.FindObject<Subject>(
                                        CriteriaOperator.Parse("SubjectCode=?", subjectCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        UOW.CommitTransaction();
                        session.CommitTransaction();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }

            return new XPCollection<WeekReportData>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                         semesterName), new ContainsOperator("Subjects", new BinaryOperator("SubjectCode", subjectCode))));
        }
        public static XPCollection<WeekReportData> CreateSubjectTimetableData(ObjectSpace objectSpace, string subjectCode, string semesterName)
        {
            ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
                           new ContainsOperator("Subjects", new BinaryOperator("SubjectCode", subjectCode))))
                {

                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                            semesterName), new BinaryOperator("Lesson.Subject.SubjectCode", subjectCode))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;
                        
                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = objectSpace.CreateObject<WeekReportData>();
                                    currentWeekData.Semester = objectSpace.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.Subjects.Add(objectSpace.FindObject<Subject>(
                                        CriteriaOperator.Parse("SubjectCode=?", subjectCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        objectSpace.CommitChanges();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            
            return new XPCollection<WeekReportData>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                         semesterName), new ContainsOperator("Subjects", new BinaryOperator("SubjectCode", subjectCode))));
        }
        
        public static XPCollection<WeekReportData> CreateTeacherTimetableData(ObjectSpace objectSpace, string teacherCode, string semesterName)
        {
            
            ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
                           new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode))))
                            
                {

                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();

                    using (XPCollection<Lesson> xpclesson = new XPCollection<Lesson>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode)))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;


                        foreach (Lesson lesson in xpclesson)
                        {
                            foreach (TkbSemester tkbSem in lesson.TKBSemesters)
                            {

                                string[] strperiod = tkbSem.Period.Split(',');
                                List<string> periodTimeList = new List<string>();
                                foreach (string s in strperiod)
                                {
                                    if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                        if (!periodTimeList.Contains("Sáng"))
                                            periodTimeList.Add("Sáng");
                                    if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount+afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Chiều"))
                                            periodTimeList.Add("Chiều");
                                    if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Tối"))
                                            periodTimeList.Add("Tối");
                                }

                                foreach (String period in periodTimeList)
                                {
                                    if (!dicTimePeriodforWeekData.ContainsKey(period))
                                    {
                                        currentWeekData = objectSpace.CreateObject<WeekReportData>();
                                        currentWeekData.Semester = objectSpace.FindObject<Semester>(
                                            CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                        currentWeekData.Teachers.Add(objectSpace.FindObject<Teacher>(
                                            CriteriaOperator.Parse("TeacherCode=?", teacherCode)));
                                        currentWeekData.PeriodTime = period;

                                        dicTimePeriodforWeekData.Add(period, currentWeekData);
                                    }

                                    dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                            tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                            Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));

                                }
                            }
                        }
                        objectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            return new XPCollection<WeekReportData>(objectSpace.Session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode))));
        }
        public static XPCollection<WeekReportData> CreateTeacherTimetableData(Session session, string teacherCode, string semesterName)
        {
            ConstrainstParameter cp = session.FindObject<ConstrainstParameter>(
              new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(session,
                           new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode))))
                {

                    session.Delete(xpw);
                    session.CommitTransaction();

                    using (XPCollection<Lesson> xpclesson = new XPCollection<Lesson>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode)))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;

                        NestedUnitOfWork UOW = session.BeginNestedUnitOfWork();
                        UOW.BeginTransaction();
                        foreach (Lesson lesson in xpclesson)
                        {
                            foreach (TkbSemester tkbSem in lesson.TKBSemesters)
                            {

                                string[] strperiod = tkbSem.Period.Split(',');
                                List<string> periodTimeList = new List<string>();
                                foreach (string s in strperiod)
                                {
                                    if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                        if (!periodTimeList.Contains("Sáng"))
                                            periodTimeList.Add("Sáng");
                                    if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Chiều"))
                                            periodTimeList.Add("Chiều");
                                    if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                        if (!periodTimeList.Contains("Tối"))
                                            periodTimeList.Add("Tối");
                                }

                                foreach (String period in periodTimeList)
                                {
                                    if (!dicTimePeriodforWeekData.ContainsKey(period))
                                    {
                                        currentWeekData = new WeekReportData(UOW);
                                        currentWeekData.Semester = UOW.FindObject<Semester>(
                                            CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                        currentWeekData.Teachers.Add(UOW.FindObject<Teacher>(
                                            CriteriaOperator.Parse("TeacherCode=?", teacherCode)));
                                        currentWeekData.PeriodTime = period;

                                        dicTimePeriodforWeekData.Add(period, currentWeekData);
                                    }

                                    dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                            tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                            Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));

                                }
                            }
                        }
                        UOW.CommitTransaction();
                        session.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }
            return new XPCollection<WeekReportData>(session,
                           new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                            semesterName), new ContainsOperator("Teachers", new BinaryOperator("TeacherCode", teacherCode))));
      
        }
        
        public static XPCollection<WeekReportData> CreateStudentClassTimetableData(ObjectSpace objectSpace, string studentClassCode, string semesterName)
        {
            ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = objectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
                           new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClassCode))))
                {

                    objectSpace.Delete(xpw);
                    objectSpace.CommitChanges();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                            semesterName), new BinaryOperator("Lesson.ClassIDs", string.Format("%{0}%", studentClassCode), BinaryOperatorType.Like))))
                    {

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;

                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = objectSpace.CreateObject<WeekReportData>();
                                    currentWeekData.Semester = objectSpace.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.StudentClasses.Add(objectSpace.FindObject<StudentClass>(
                                        CriteriaOperator.Parse("ClassCode=?", studentClassCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        objectSpace.CommitChanges();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException("Lỗi thực hiện: " + ex.Message);
            }

            return new XPCollection<WeekReportData>(objectSpace.Session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                         semesterName), new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClassCode))));

        }
        public static XPCollection<WeekReportData> CreateStudentClassTimetableData(Session session, string studentClassCode, string semesterName)
        {
            ConstrainstParameter cp = session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            int afternoonPeriodCount = Convert.ToInt32(cp.Value);

            cp = session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            int morningPeriodCount = Convert.ToInt32(cp.Value);

            try
            {
                using (XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(session,
                           new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClassCode))))
                {

                    session.Delete(xpw);
                    session.CommitTransaction();
                    using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(session,
                       new GroupOperator(GroupOperatorType.And, new BinaryOperator("Lesson.Semester.SemesterName",
                           semesterName), new BinaryOperator("Lesson.ClassIDs", string.Format("%{0}%", studentClassCode), BinaryOperatorType.Like))))
                    {
                    //using (XPCollection<TkbSemester> xpctkbSemester = new XPCollection<TkbSemester>(session,
                    //   new BinaryOperator("Lesson.ClassIDs", string.Format("%{0}%",studentClassCode),BinaryOperatorType.Like)))
                    //{

                        Dictionary<string, WeekReportData> dicTimePeriodforWeekData = new Dictionary<string, WeekReportData>();
                        WeekReportData currentWeekData;

                        NestedUnitOfWork UOW = session.BeginNestedUnitOfWork();
                        UOW.BeginTransaction();

                        foreach (TkbSemester tkbSem in xpctkbSemester)
                        {
                            string[] strperiod = tkbSem.Period.Split(',');
                            List<string> periodTimeList = new List<string>();
                            foreach (string s in strperiod)
                            {
                                if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= morningPeriodCount)
                                    if (!periodTimeList.Contains("Sáng"))
                                        periodTimeList.Add("Sáng");
                                if (Convert.ToInt32(s) > morningPeriodCount && Convert.ToInt32(s) <= (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Chiều"))
                                        periodTimeList.Add("Chiều");
                                if (Convert.ToInt32(s) > (morningPeriodCount + afternoonPeriodCount))
                                    if (!periodTimeList.Contains("Tối"))
                                        periodTimeList.Add("Tối");
                            }

                            foreach (String period in periodTimeList)
                            {
                                if (!dicTimePeriodforWeekData.ContainsKey(period))
                                {
                                    currentWeekData = new WeekReportData(UOW);
                                    currentWeekData.Semester = UOW.FindObject<Semester>(
                                        CriteriaOperator.Parse("SemesterName = ?", semesterName));
                                    currentWeekData.StudentClasses.Add(UOW.FindObject<StudentClass>(
                                        CriteriaOperator.Parse("ClassCode=?", studentClassCode)));
                                    currentWeekData.PeriodTime = period;
                                    dicTimePeriodforWeekData.Add(period, currentWeekData);
                                }

                                dicTimePeriodforWeekData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n\r\n",
                                        tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
                                        Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));
                            }

                        }
                        UOW.CommitTransaction();
                        session.CommitTransaction();
                        
                    }

                }
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(string.Format("Lỗi thực hiện: {0}\r\n Strack trace:{1}", ex.Message, ex.StackTrace));
            }

            return new XPCollection<WeekReportData>(session,
                        new GroupOperator(GroupOperatorType.And, new BinaryOperator("Semester.SemesterName",
                         semesterName), new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClassCode))));
        
        }
        
        public static XPCollection<ClassTransactionTracking> CreateStudentClassTrackingData(ObjectSpace objectSpace, string semesterName)
        {
            string strParse = "";
            try
            {
                using (XPCollection<StudentClass> xpw = new XPCollection<StudentClass>(objectSpace.Session))
                {
                   
                    foreach (StudentClass studentClass in xpw)
                    {
                        ClassTransactionTracking ct = objectSpace.FindObject<ClassTransactionTracking>
                            (CriteriaOperator.Parse("StudentClass.ClassCode = ? and Semester.SemesterName = ?",
                            studentClass.ClassCode, semesterName));
                        if (ct == null)
                        {
                            ct = objectSpace.CreateObject< ClassTransactionTracking>();
                            ct.Semester = objectSpace.FindObject<Semester>(CriteriaOperator.Parse("SemesterName=?", semesterName));
                            ct.StudentClass = objectSpace.FindObject<StudentClass>(CriteriaOperator.Parse("ClassCode=?", studentClass.ClassCode));
                            ct.Save();
                        }

                        strParse += (strParse == "" ? string.Format("StudentClass.ClassCode='{0}'", studentClass.ClassCode) :
                            string.Format(" or StudentClass.ClassCode='{0}'", studentClass.ClassCode));
                    }
                    objectSpace.CommitChanges();
                    strParse = string.Format("({0}) and Semester.SemesterName= '{1}'", strParse, semesterName);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(string.Format("Lỗi thực hiện: {0}\r\n Strack trace:{1}", ex.Message, ex.StackTrace));
            }
            return new XPCollection<ClassTransactionTracking>(objectSpace.Session, CriteriaOperator.Parse(strParse));
        }
        public static XPCollection<ClassTransactionTracking> CreateStudentClassTrackingData(Session session, string semesterName)
        {
            string strParse = "";
            try
            {
                using (XPCollection<StudentClass> xpw = new XPCollection<StudentClass>(session))
                {
                    NestedUnitOfWork UOW = session.BeginNestedUnitOfWork();
                    UOW.BeginTransaction();
                   
                    foreach (StudentClass studentClass in xpw)
                    {

                        ClassTransactionTracking ct = UOW.FindObject<ClassTransactionTracking>
                            (CriteriaOperator.Parse("StudentClass.ClassCode = ? and Semester.SemesterName = ?",
                            studentClass.ClassCode, semesterName));
                        if (ct == null)
                        {
                            ct = new ClassTransactionTracking(UOW);
                            ct.Semester = UOW.FindObject<Semester>(CriteriaOperator.Parse("SemesterName=?", semesterName));
                            ct.StudentClass = UOW.FindObject<StudentClass>(CriteriaOperator.Parse("ClassCode=?", studentClass.ClassCode));
                            ct.Save();
                        }                     

                        strParse += (strParse == "" ? string.Format("StudentClass.ClassCode='{0}'", studentClass.ClassCode) :
                            string.Format(" or StudentClass.ClassCode='{0}'", studentClass.ClassCode));
                    }
                    UOW.CommitTransaction();
                    session.CommitTransaction();
                    strParse = string.Format("({0}) and Semester.SemesterName= '{1}'", strParse, semesterName);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(string.Format("Lỗi thực hiện: {0}\r\n Strack trace:{1}",ex.Message,ex.StackTrace));
            }
            return new XPCollection<ClassTransactionTracking>(session, CriteriaOperator.Parse(strParse));
        }
    }
}