using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Web;
using System.IO;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace vidoSolution.Module.DomainObject
{
   
    [DefaultClassOptions]
    [Persistent("StudentResultFile")]
    public class StudentResultFile : BaseObject
    {
        public StudentResultFile(Session session) : base(session) { }
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
        //[Action(ToolTip = "Import Students Semester Result to Database", TargetObjectsCriteria="IsImported=false")]
        public void ImportStudentResult()
        {
            Session session = this.Session;
            string tempStudentFolderPath;
            string tempStudentFile;
            string tempStudentLogFile;
            string filename = CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
            if (HttpContext.Current != null)
            {
                tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                tempStudentFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + CsvFile.FileName);
                tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + filename);
            }
            else
            {
                tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
                tempStudentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", CsvFile.FileName);
                tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", filename);
            }

            if (!Directory.Exists(tempStudentFolderPath))
                Directory.CreateDirectory(tempStudentFolderPath);
          
            Dictionary<string, int> columnIndexs = new Dictionary<string, int>();
            Dictionary<string, string> valueIndexs = new Dictionary<string, string>();
            valueIndexs.Add("NHHK", "");
            valueIndexs.Add("MSSV", "");
            valueIndexs.Add("HO", "");
            valueIndexs.Add("TEN", "");
            //valueIndexs.Add("NGAYSINH", "");
            valueIndexs.Add("NHOMLOPMH", "");
            valueIndexs.Add("MAMH", "");
            valueIndexs.Add("TENMH", "");
            valueIndexs.Add("SOTC", "");
            valueIndexs.Add("DTB10", "");
            valueIndexs.Add("DTB4", "");
            valueIndexs.Add("DIEMCHU", "");


            columnIndexs.Add("NHHK",-1);
            columnIndexs.Add("MSSV", -1);
            columnIndexs.Add("HO", -1);
            columnIndexs.Add("TEN", -1);
            //columnIndexs.Add("NGAYSINH", -1);
            columnIndexs.Add("NHOMLOPMH", -1);
            columnIndexs.Add("MAMH", -1);
            columnIndexs.Add("TENMH", -1);
            columnIndexs.Add("SOTC", -1); 
            columnIndexs.Add("DTB10", -1); 
            columnIndexs.Add("DTB4", -1);
            columnIndexs.Add("DIEMCHU", -1);
           
            using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
            {
                using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempStudentLogFile, true))
                {
                    fileStreamlog.WriteLine("<html><header><title>"+ CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log </title>	" +
                    "<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />" +
                    "</head><body>");
                    CsvFile.SaveToStream(fileStream);
                    fileStream.Position = 0;
                    StreamReader r = new StreamReader(fileStream);
                    string newLine;
                    bool foundHeader = false;
                    int iLine = 1;
                    
                    try
                    {
                        //Tìm dòng chứa TEN cột
                        while ((newLine = r.ReadLine()) != null)
                        {
                            string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);
                            if (!foundHeader)
                            {
                                for (int i = 0; i < row.Length; i++)
                                    if (columnIndexs.ContainsKey(row[i].ToUpper().Trim()))
                                    {
                                        columnIndexs[row[i].ToUpper().Trim()] = i; //Đã tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
                                    }
                                if (!columnIndexs.Values.Contains(-1))
                                {
                                    foundHeader = true;
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < row.Length; i++)
                                        if (columnIndexs.ContainsKey(row[i].ToUpper().Trim()))
                                        {
                                            columnIndexs[row[i].ToUpper().Trim()] = -1; //không tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
                                        }
                                }
                            }
                            
                        }
                        ResultLink = "./tempFolder/" + CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
                        if (!foundHeader)
                            throw new UserFriendlyException("Lỗi cấu trúc file");
                        //Các dòng sau đó đều là dòng dữ liệu
                        IsImported = true;
                        
                        List<Student> listStudent = new List<Student>();
                        List<Lesson> listLessons = new List<Lesson>();
                        List<Subject> listSubject = new List<Subject>();     
                        List<Semester> listSemester = new List<Semester>();
                        while ((newLine = r.ReadLine()) != null)
                        {
                            iLine++;
                            using (NestedUnitOfWork uow = Session.BeginNestedUnitOfWork())
                            {
                                uow.BeginTransaction();
                                try
                                {
                                    string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);

                                    foreach (var column in columnIndexs)
                                    {
                                        valueIndexs[column.Key] = row[column.Value];
                                    }

                                    //tìm sinh viên
                                    Student student = listStudent.Find(s=> s.StudentCode ==valueIndexs["MSSV"]);
                                    if (student ==null)
                                        student = session.FindObject<Student>(CriteriaOperator.Parse("StudentCode = ?", valueIndexs["MSSV"]));
                                    if (student == null)
                                    {
                                        fileStreamlog.WriteLine(string.Format("ERROR:Cannot find student: \"{0} - {1} {2} \" on line \"{3}\" on {4:dd-mm-yy HH:MM:ss}<br/>",
                                            valueIndexs["MSSV"], valueIndexs["HO"], valueIndexs["TEN"], iLine, DateTime.Now));
                                        continue;
                                    }
                                    else{
                                        if (!(student.FirstName.Contains(valueIndexs["HO"]) &&  student.LastName.Contains(valueIndexs["TEN"])))
                                        {
                                         fileStreamlog.WriteLine(string.Format("WARNING: Found student: \"{0}\" but Name:\"{1} {2}\" is not Like \"{3} {4}\" on line \"{5}\" on {6:dd-mm-yy HH:MM:ss}<br/>",
                                            valueIndexs["MSSV"], student.FirstName,student.LastName,valueIndexs["HO"], valueIndexs["TEN"], iLine, DateTime.Now));
                                        } 
                                        listStudent.Add(student);
                                    }
                                    //found student
                                    //tìm nhóm lớp
                                    int nhomlop;
                                    if (!int.TryParse(valueIndexs["NHOMLOPMH"], out nhomlop))
                                    {
                                        fileStreamlog.WriteLine(string.Format("CANNNOT CONVERT TO NUMBER for LessonCode: \"{0}\" on line \"{3}\" on {4:dd-mm-yy HH:MM:ss}<br/>",
                                            valueIndexs["NHOMLOPMH"], iLine, DateTime.Now));
                                        continue;
                                    }
                                    Lesson lesson = listLessons.Find(l=>l.LessonCode == nhomlop);
                                    if (lesson==null)
                                        lesson = session.FindObject<Lesson>(CriteriaOperator.Parse("LessonCode = ?", nhomlop));
                                    if (lesson != null)
                                    {
                                        if (lesson.Semester.SemesterName != valueIndexs["NHHK"])
                                        {
                                            fileStreamlog.WriteLine(string.Format("Found Lesson \"{0}\" but Semester {1} not same {2} on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE<br/>",
                                            valueIndexs["NHOMLOPMH"], lesson.Semester.SemesterName, valueIndexs["NHHK"], iLine, DateTime.Now));
                                            continue;
                                        }
                                        if (lesson.Subject.SubjectCode != valueIndexs["MAMH"])
                                        {
                                            fileStreamlog.WriteLine(string.Format("Found Lesson \"{0}\" but Subject Code {1} not same {2} on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE<br/>",
                                              valueIndexs["NHOMLOPMH"], lesson.Subject.SubjectCode, valueIndexs["MAMH"], iLine, DateTime.Now));
                                            continue;
                                        }
                                        if (!lesson.ClassIDs.Contains(student.StudentClass.ClassCode))
                                            lesson.ClassIDs += "," + student.StudentClass.ClassCode;
                                        if (!listLessons.Contains(lesson))
                                            listLessons.Add(lesson);
                                    }
                                    else //create new lesson 
                                    {
                                        Semester semester = listSemester.Find(s => s.SemesterName == valueIndexs["NHHK"]);
                                        if (semester ==null)
                                            semester = session.FindObject<Semester>(CriteriaOperator.Parse("SemesterName = ?" ,valueIndexs["NHHK"]));
                                        if (semester == null) //create new semester
                                        {

                                            semester = new Semester(session)
                                            {
                                                SemesterName = valueIndexs["NHHK"]
                                            };
                                            semester.Save();
                                            fileStreamlog.WriteLine(string.Format("Create Semester:{0} -  on line \"{1}\" on {2:dd-mm-yy HH:MM:ss} <br/>",
                                              valueIndexs["NHHK"], iLine, DateTime.Now));
                                            listSemester.Add(semester);
                                        }
                                        else if (!listSemester.Contains(semester))
                                            listSemester.Add(semester);

                                        Subject subject = listSubject.Find(s => s.SubjectCode == valueIndexs["MAMH"]);
                                        if (subject ==null)    
                                            subject=session.FindObject<Subject>(CriteriaOperator.Parse("SubjectCode = ?",valueIndexs["MAMH"]));

                                        if (subject != null)
                                        {
                                            if (subject.SubjectName != valueIndexs["TENMH"])
                                            {
                                                fileStreamlog.WriteLine(string.Format("WARNING: Found Subject \"{0}\" for lesson {1} but Name {2} not same {3} on line \"{4}\" on {5:dd-mm-yy HH:MM:ss} <br/>",
                                                valueIndexs["MAMH"], valueIndexs["NHOMLOPMH"], subject.SubjectName, valueIndexs["TENMH"], iLine, DateTime.Now));                                                
                                            }
                                            if (!listSubject.Contains(subject))
                                                listSubject.Add(subject);
                                        }
                                        else//create new subject
                                        {

                                            subject = new Subject(session)
                                            {
                                                SubjectCode = valueIndexs["MAMH"],
                                                SubjectName = valueIndexs["TENMH"],
                                                Credit = Convert.ToDouble(valueIndexs["SOTC"])
                                            };
                                            subject.Save();
                                            fileStreamlog.WriteLine(string.Format("Create Subject:{0} - {1} ({2}TC)  on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} <br/>",
                                              valueIndexs["MAMH"], valueIndexs["TENMH"], valueIndexs["SOTC"], iLine, DateTime.Now));
                                            listSubject.Add(subject);
                                        }
                                        
                                        

                                        lesson = new Lesson(session)
                                        {
                                            Semester = semester,
                                            Subject = subject,
                                            LessonCode = nhomlop,
                                            CanRegister = false,
                                            LessonNote = "Tạo mới cho phân hệ điểm",
                                            ClassIDs = student.StudentClass.ClassCode
                                        };
                                        lesson.Save();
                                        fileStreamlog.WriteLine(string.Format("Create Lesson :{0} - {1} ({2}TC)  on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} <br/>",
                                              nhomlop, valueIndexs["MAMH"], valueIndexs["NHHK"], iLine, DateTime.Now));
                                        listLessons.Add(lesson);
                                    }

                                    try
                                    {
                                        StudentResult studResult = session.FindObject<StudentResult>(CriteriaOperator.Parse("Student = ? and Lesson=?",student,lesson));
                                        if (studResult == null)
                                        {
                                            studResult = new StudentResult(session)
                                            {
                                                Student = student,
                                                Lesson = lesson,
                                                AvgMark10 = Convert.ToDouble(valueIndexs["DTB10"]),
                                                AvgMark4 = Convert.ToDouble(valueIndexs["DTB4"]),
                                                AvgChar = valueIndexs["DIEMCHU"]
                                            };

                                            studResult.Save();
                                            fileStreamlog.WriteLine(string.Format("Create StudentResult Lesson {0} with Subject Code {1} for student: \"{2}\"-\"{3}\" on {4:dd-mm-yy HH:MM:ss} - line {5} <br/>",
                                               lesson.LessonCode, lesson.Subject.SubjectCode, student.StudentCode, student.FullName, DateTime.Now, iLine));
                                        }
                                        else
                                        {
                                            studResult.AvgMark10 = Convert.ToDouble(valueIndexs["DTB10"]);
                                            studResult.AvgMark4 = Convert.ToDouble(valueIndexs["DTB4"]);
                                            studResult.AvgChar = valueIndexs["DIEMCHU"];
                                            studResult.Save();
                                            fileStreamlog.WriteLine(string.Format("Update StudentResult Lesson {0} with Subject Code {1} for student: \"{2}\"-\"{3}\" on {4:dd-mm-yy HH:MM:ss} - line {5} <br/>",
                                               lesson.LessonCode, lesson.Subject.SubjectCode, student.StudentCode, student.FullName, DateTime.Now, iLine));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        fileStreamlog.WriteLine(string.Format("Cannot create StudentResult for student \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} - Line {3} <br/>",
                                            student.StudentCode, student.FullName, DateTime.Now, iLine));
                                        fileStreamlog.WriteLine(ex.Message + ex.StackTrace);
                                    }

                                    uow.CommitTransaction();
                                }
                                catch (Exception ex)
                                {
                                    fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \r\n {1}\r\n {1} on {1:dd-mm-yy HH:MM:ss} <br/>", iLine, ex.Message, ex.StackTrace, DateTime.Now));
                                }
                            }
                        }
                        fileStreamlog.WriteLine(string.Format("Create \"{0}\" all StudentResult on {1:dd-mm-yy HH:MM:ss} <br/>", iLine, DateTime.Now));
                    }
                    catch (UserFriendlyException ex)
                    {
                        fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \"{1}\" on {1:dd-mm-yy HH:MM:ss} <br/>", iLine, ex.StackTrace, DateTime.Now));
                        fileStreamlog.WriteLine("</body></html>");
                        fileStream.Close();
                        fileStreamlog.Close();
                        throw ex;
                    }
                    finally
                    {
                        fileStreamlog.WriteLine("</body></html>");
                        fileStream.Close();
                        fileStreamlog.Close();
                    }
                }
            }
           
        }
    }
}
