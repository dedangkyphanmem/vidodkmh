using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Web;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;

namespace vidoSolution.Module.DomainObject
{
    
     [DefaultClassOptions]
     [Persistent("StudentAccumulationFile")]
    public class StudentAccumulationFile : BaseObject
    {
        public StudentAccumulationFile(Session session) : base(session) { }
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
        DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { if (createDate == null) SetPropertyValue("CreateDate", ref createDate, DateTime.Now); }
        }
        //[Action(ToolTip = "Import Students Accumulation to Database", TargetObjectsCriteria = "IsImported=false")]
        public void ImportStudentAccumulation()
        {
            Session session = this.Session;
            string tempStudentFolderPath;
            string tempStudentFile;
            string tempStudentLogFile;
            if (HttpContext.Current != null)
            {
                tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                tempStudentFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + CsvFile.FileName);
                tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + CsvFile.FileName+DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.txt");
            }
            else
            {
                tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
                tempStudentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", CsvFile.FileName);
                tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", CsvFile.FileName + "-log.txt");
            }

            if (!Directory.Exists(tempStudentFolderPath))
                Directory.CreateDirectory(tempStudentFolderPath);
      
            Dictionary<string, int> columnIndexs = new Dictionary<string, int>();
            Dictionary<string, string> valueIndexs = new Dictionary<string, string>();
            valueIndexs.Add("NHHK", "");
            valueIndexs.Add("MSSV", "");
            valueIndexs.Add("HO", "");
            valueIndexs.Add("TEN", "");           
            valueIndexs.Add("TONGTCHK", "");
            valueIndexs.Add("DTBHK10", "");
            valueIndexs.Add("DTBHK4", "");
            valueIndexs.Add("TONGTCTL", "");
            valueIndexs.Add("DTBTL10", "");
            valueIndexs.Add("DTBTL4", "");

            columnIndexs.Add("NHHK", -1);
            columnIndexs.Add("MSSV", -1);
            columnIndexs.Add("HO", -1);
            columnIndexs.Add("TEN", -1);
            columnIndexs.Add("TONGTCHK", -1);
            columnIndexs.Add("DTBHK10", -1);
            columnIndexs.Add("DTBHK4", -1);
            columnIndexs.Add("TONGTCTL", -1); 
            columnIndexs.Add("DTBTL10", -1);
            columnIndexs.Add("DTBTL4", -1);
            
            using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
            {
                using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempStudentLogFile, true))
                {
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
                        if (!foundHeader)
                            throw new UserFriendlyException("Lỗi cấu trúc file");
                        //Các dòng sau đó đều là dòng dữ liệu
                        IsImported = true;
                        //Các dòng sau đó đều là dòng dữ liệu
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
                                    Student student = session.FindObject<Student>(new BinaryOperator("StudentCode", valueIndexs["MSSV"]));
                                    if (student == null)
                                    {
                                        fileStreamlog.WriteLine(string.Format("Cannot find student: \"{0} - {1} {2} \" on line \"{3}\" on {4:dd-mm-yy HH:MM:ss}",
                                            valueIndexs["MSSV"], valueIndexs["HO"], valueIndexs["TEN"], iLine, DateTime.Now));
                                        continue;
                                    }
                                    //FOUND STUDENT

                                    //TIM SEMESTER
                                    Semester semester = session.FindObject<Semester>(CriteriaOperator.Parse("SemesterName = " + valueIndexs["NHHK"]));
                                    if (semester == null) //create new semester
                                    {
                                        semester = new Semester(session)
                                        {
                                            SemesterName = valueIndexs["NHHK"]
                                        };
                                        semester.Save();
                                        fileStreamlog.WriteLine(string.Format("Create Semester:{0} -  on line \"{1}\" on {2:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE",
                                          valueIndexs["NHHK"], iLine, DateTime.Now));
                                    }

                                    try
                                    {
                                        StudentAccumulation acc = new StudentAccumulation(session)
                                        {
                                            Student = student,
                                            Semester = semester,
                                            SemesterCredit = Convert.ToDouble(valueIndexs["TONGTCHK"]),
                                            SemesterAvgMark10 = Convert.ToDouble(valueIndexs["DTBHK10"]),
                                            SemesterAvgMark4 = Convert.ToDouble(valueIndexs["DTBHK4"]),
                                            TotalAccumulateCredit = Convert.ToDouble(valueIndexs["TONGTCTL"]),
                                            AccumulateAvgMark10 = Convert.ToDouble(valueIndexs["DTBTL10"]),
                                            AccumulateAvgMark4 = Convert.ToDouble(valueIndexs["DTBTL4"])
                                        };
                                        acc.Save();
                                        fileStreamlog.WriteLine(string.Format("Create StudentAccumulation with TotalAccumulateCredit {0} for student: \"{1}\"-\"{2}\" on {3:dd-mm-yy HH:MM:ss} - line {4}",
                                            valueIndexs["TONGTCTL"], student.StudentCode, student.FullName, DateTime.Now, iLine

                                            ));
                                    }
                                    catch (Exception ex)
                                    {
                                        fileStreamlog.WriteLine(string.Format("Cannot create Transaction for student \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} - Line {3}",
                                            student.StudentCode, student.FullName, DateTime.Now, iLine));
                                        fileStreamlog.WriteLine(ex.Message + ex.StackTrace);
                                    }

                                    uow.CommitTransaction();


                                }
                                catch (Exception ex)
                                {
                                    fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \r\n {1}\r\n {1} on {1:dd-mm-yy HH:MM:ss}", iLine, ex.Message, ex.StackTrace, DateTime.Now));
                                }
                            }
                        }
                        fileStreamlog.WriteLine(string.Format("Create \"{0}\" all transaction on {1:dd-mm-yy HH:MM:ss}", iLine, DateTime.Now));
                    }
                    catch (UserFriendlyException ex)
                    {
                        fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \"{1}\" on {1:dd-mm-yy HH:MM:ss}", iLine, ex.StackTrace, DateTime.Now));
                        fileStream.Close();
                        fileStreamlog.Close();
                        throw ex;
                    }
                    finally
                    {
                        fileStream.Close();
                        fileStreamlog.Close();
                    }
                }
            }
           
        }
    }
}
