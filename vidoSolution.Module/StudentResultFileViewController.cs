using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using vidoSolution.Module.DomainObject;
using DevExpress.Xpo;
using DevExpress.ExpressApp.SystemModule;
using System.Web;
using System.IO;
using ExcelLibrary.Office.Excel;
using DevExpress.Data.Filtering;

namespace vidoSolution.Module
{
    public partial class StudentResultFileViewController : ViewController
    {
        public StudentResultFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void StudentResultFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }

        void ImportStudentResultAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            //ObjectSpace objectSpace =  ObjectSpaceInMemory.CreateNew();
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            int count = 0;
            int iLine=0;
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            
            foreach (StudentResultFile actFile in View.SelectedObjects)
            {
                if (actFile.Note ==null ||actFile.Note == "")
                    throw new UserFriendlyException("Vui lòng thêm thông tin Ghi chú trước khi import!!!");

                string tempStudentFile;
                string tempStudentFolderPath;
                string tempStudentLogFile;
                string templogname = "";
                if (HttpContext.Current != null)
                {
                    tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                    tempStudentFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + actFile.CsvFile.FileName);
                    templogname = actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
                    tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + templogname);
                }
                else
                {
                    tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
                    tempStudentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", actFile.CsvFile.FileName);
                    templogname = actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
                    tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", templogname);
                }

                if (!Directory.Exists(tempStudentFolderPath))
                    Directory.CreateDirectory(tempStudentFolderPath);
                
                using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
                {
                    Dictionary<string, int> columnIndexs = new Dictionary<string, int>();
                    Dictionary<string, object> valueIndexs = new Dictionary<string, object>();
                    valueIndexs.Add("NHHK", "");
                    valueIndexs.Add("MSSV", "");
                    valueIndexs.Add("HO", "");
                    valueIndexs.Add("TEN", "");
                    valueIndexs.Add("NHOMLOPMH", "");
                    valueIndexs.Add("MAMH", "");
                    valueIndexs.Add("TENMH", "");
                    valueIndexs.Add("SOTC", "");
                    valueIndexs.Add("DTB10", "");
                    valueIndexs.Add("DTB4", "");
                    valueIndexs.Add("DIEMCHU", "");


                    columnIndexs.Add("NHHK", -1);
                    columnIndexs.Add("MSSV", -1);
                    columnIndexs.Add("HO", -1);
                    columnIndexs.Add("TEN", -1);
                    columnIndexs.Add("NHOMLOPMH", -1);
                    columnIndexs.Add("MAMH", -1);
                    columnIndexs.Add("TENMH", -1);
                    columnIndexs.Add("SOTC", -1);
                    columnIndexs.Add("DTB10", -1);
                    columnIndexs.Add("DTB4", -1);
                    columnIndexs.Add("DIEMCHU", -1);

                    // open xls file
                    actFile.CsvFile.SaveToStream(fileStream);
                    fileStream.Close();
                    Workbook book = Workbook.Open(tempStudentFile);
                    Worksheet sheet = book.Worksheets[0];


                    bool foundHeader = false;
                    
                    Row row;
                    //Tìm dòng chứa TEN cột
                    for (iLine = sheet.Cells.FirstRowIndex;
                           iLine <= sheet.Cells.LastRowIndex && !foundHeader; iLine++)
                    {
                        row = sheet.Cells.GetRow(iLine);
                        for (int colIndex = row.FirstColIndex;
                           colIndex <= row.LastColIndex; colIndex++)
                        {
                            Cell cell = row.GetCell(colIndex);
                            if (columnIndexs.ContainsKey(cell.Value.ToString().ToUpper().Trim()))
                            {
                                columnIndexs[cell.Value.ToString().ToUpper().Trim()] = colIndex; //Đã tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
                            }
                        }
                        if (!columnIndexs.Values.Contains(-1))
                        {
                            foundHeader = true;
                        }
                        else
                        {
                            for (int colIndex = row.FirstColIndex; colIndex <= row.LastColIndex; colIndex++)
                            {
                                Cell cell = row.GetCell(colIndex);
                                if (columnIndexs.ContainsKey(cell.Value.ToString().ToUpper().Trim()))
                                {
                                    columnIndexs[cell.Value.ToString().ToUpper().Trim()] = -1; //không tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
                                }
                            }
                        }
                    }
                    if (!foundHeader)
                        throw new UserFriendlyException("Lỗi cấu trúc file");

                    using (System.IO.StreamWriter fileStreamlog = new System.IO.StreamWriter(tempStudentLogFile, true))
                    {
                        fileStreamlog.WriteLine("<html><header><title>" + actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log </title>	" +
                   "<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />" +
                   "</head><body>\r\n<table border=\"1px\"> <tr><Th>DÒNG</Th><th>TÌNH TRẠNG</th><th>THÔNG ĐIỆP</th></Tr>");
                        //Các dòng sau đó đều là dòng dữ liệu
                       
                        List<Student> listStudent = new List<Student>();
                        List<Lesson> listLessons = new List<Lesson>();
                        List<Subject> listSubject = new List<Subject>();
                        List<Semester> listSemester = new List<Semester>();

                        for (; iLine <= sheet.Cells.LastRowIndex; iLine++)
                        {
                            row = sheet.Cells.GetRow(iLine);
                            try
                            {
                                foreach (var column in columnIndexs)
                                {
                                    Cell cell = row.GetCell(column.Value);
                                    valueIndexs[column.Key] = cell.Value;
                                }

                                //tìm sinh viên
                                Student student = listStudent.Find(s => s.StudentCode == valueIndexs["MSSV"].ToString().Trim());
                                if (student == null)
                                    student = objectSpace.FindObject<Student>(CriteriaOperator.Parse("StudentCode = ?", valueIndexs["MSSV"].ToString().Trim()));
                                if (student == null)
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine,"ERROR",
                                        string.Format("Cannot find student: \"{0} - {1} {2} \" on {3:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE",
                                        valueIndexs["MSSV"], valueIndexs["HO"], valueIndexs["TEN"], DateTime.Now));
                                    continue;
                                }
                                else
                                {
                                    if (!(student.FirstName.Contains(valueIndexs["HO"].ToString()) && student.LastName.Contains(valueIndexs["TEN"].ToString())))
                                    {
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                            string.Format("Found student: \"{0}\" but Name:\"{1} {2}\" is not Like \"{3} {4}\" on {5:dd-mm-yy HH:MM:ss}",
                                           valueIndexs["MSSV"], student.FirstName, student.LastName, valueIndexs["HO"], valueIndexs["TEN"],  DateTime.Now));
                                    }
                                    listStudent.Add(student);
                                }
                                //found student
                                //tìm nhóm lớp
                                int nhomlop;
                                if (!int.TryParse(valueIndexs["NHOMLOPMH"].ToString().Trim(), out nhomlop))
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                        string.Format("CANNNOT CONVERT TO NUMBER for LessonCode: \"{0}\" on {3:dd-mm-yy HH:MM:ss}- CANNOT IMPORT THIS LINE",
                                        valueIndexs["NHOMLOPMH"],DateTime.Now));
                                    continue;
                                }
                                Lesson lesson = listLessons.Find(l => l.LessonCode == nhomlop);
                                if (lesson == null)
                                    lesson = objectSpace.FindObject<Lesson>(CriteriaOperator.Parse("LessonCode = ?", nhomlop));
                                if (lesson != null)
                                {
                                    if (lesson.Semester.SemesterName.Trim() != valueIndexs["NHHK"].ToString().Trim())
                                    {
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                            string.Format("Found Lesson \"{0}\" but Semester {1} not same {2} on  {3:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE",
                                        valueIndexs["NHOMLOPMH"], lesson.Semester.SemesterName, valueIndexs["NHHK"],  DateTime.Now));
                                        continue;
                                    }
                                    if (lesson.Subject.SubjectCode != valueIndexs["MAMH"].ToString().Trim())
                                    {
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR", 
                                            string.Format("Found Lesson \"{0}\" but Subject Code {1} not same {2} on {3:dd-mm-yy HH:MM:ss} - CANNOT IMPORT THIS LINE",
                                          valueIndexs["NHOMLOPMH"], lesson.Subject.SubjectCode, valueIndexs["MAMH"], DateTime.Now));
                                        continue;
                                    }
                                    if (!lesson.ClassIDs.Contains(student.StudentClass.ClassCode))
                                        lesson.ClassIDs += "," + student.StudentClass.ClassCode;
                                    if (!listLessons.Contains(lesson))
                                        listLessons.Add(lesson);
                                }
                                else //create new lesson 
                                {
                                    Semester semester = listSemester.Find(s => s.SemesterName == valueIndexs["NHHK"].ToString());
                                    if (semester == null)
                                        semester = objectSpace.FindObject<Semester>(CriteriaOperator.Parse("SemesterName = ?", valueIndexs["NHHK"]));
                                    if (semester == null) //create new semester
                                    {

                                        semester = objectSpace.CreateObject<Semester>();

                                        semester.SemesterName = valueIndexs["NHHK"].ToString();

                                        semester.Save();
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW", 
                                            string.Format("Create Semester:{0} -  on {1:dd-mm-yy HH:MM:ss} ",
                                          valueIndexs["NHHK"],  DateTime.Now));
                                        listSemester.Add(semester);
                                    }
                                    else if (!listSemester.Contains(semester))
                                        listSemester.Add(semester);

                                    Subject subject = listSubject.Find(s => s.SubjectCode == valueIndexs["MAMH"].ToString());
                                    if (subject == null)
                                        subject = objectSpace.FindObject<Subject>(CriteriaOperator.Parse("SubjectCode = ?", valueIndexs["MAMH"]));

                                    if (subject != null)
                                    {
                                        if (subject.SubjectName != valueIndexs["TENMH"].ToString())
                                        {
                                            fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING", 
                                                string.Format("Found Subject \"{0}\" for lesson {1} but Name {2} not same {3} on  {4:dd-mm-yy HH:MM:ss}",
                                            valueIndexs["MAMH"], valueIndexs["NHOMLOPMH"], subject.SubjectName, valueIndexs["TENMH"], DateTime.Now));
                                        }
                                        if (!listSubject.Contains(subject))
                                            listSubject.Add(subject);
                                    }
                                    else//create new subject
                                    {
                                        subject = objectSpace.CreateObject<Subject>();

                                        subject.SubjectCode = valueIndexs["MAMH"].ToString();
                                        subject.SubjectName = valueIndexs["TENMH"].ToString();
                                        subject.Credit = Convert.ToDouble(valueIndexs["SOTC"]);

                                        subject.Save();
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW", 
                                            string.Format("Create Subject:{0} - {1} ({2}TC)  on {3:dd-mm-yy HH:MM:ss} ",
                                          valueIndexs["MAMH"], valueIndexs["TENMH"], valueIndexs["SOTC"],  DateTime.Now));
                                        listSubject.Add(subject);
                                    }


                                    lesson = objectSpace.CreateObject<Lesson>();

                                    lesson.Semester = semester;
                                    lesson.Subject = subject;
                                    lesson.LessonCode = nhomlop;
                                    lesson.CanRegister = false;
                                    lesson.LessonNote = "Tạo mới cho phân hệ điểm";
                                    lesson.ClassIDs = student.StudentClass.ClassCode;

                                    lesson.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW", 
                                        string.Format("Create Lesson :{0} - {1} ({2}TC)  on {3:dd-mm-yy HH:MM:ss}",
                                          nhomlop, valueIndexs["MAMH"], valueIndexs["NHHK"],  DateTime.Now));
                                    listLessons.Add(lesson);
                                }

                                try
                                {
                                    StudentResult studResult = objectSpace.FindObject<StudentResult>(CriteriaOperator.Parse("Student = ? and Lesson=?", student, lesson));
                                    if (studResult == null)
                                    {
                                        studResult = objectSpace.CreateObject<StudentResult>();

                                        studResult.Student = student;
                                        studResult.Lesson = lesson;
                                        studResult.AvgMark10 = Convert.ToDouble(valueIndexs["DTB10"]);
                                        studResult.AvgMark4 = Convert.ToDouble(valueIndexs["DTB4"]);
                                        studResult.AvgChar = valueIndexs["DIEMCHU"].ToString();
                                        studResult.Save();
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW", 
                                            string.Format("Create StudentResult Lesson {0} with Subject Code {1} for student: \"{2}\"-\"{3}\" on {4:dd-mm-yy HH:MM:ss}",
                                           lesson.LessonCode, lesson.Subject.SubjectCode, student.StudentCode, student.FullName, DateTime.Now));
                                    }
                                    else
                                    {
                                        studResult.AvgMark10 = Convert.ToDouble(valueIndexs["DTB10"]);
                                        studResult.AvgMark4 = Convert.ToDouble(valueIndexs["DTB4"]);
                                        studResult.AvgChar = valueIndexs["DIEMCHU"].ToString();
                                        studResult.Save();
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING", 
                                            string.Format("Update StudentResult Lesson {0} with Subject Code {1} for student: \"{2}\"-\"{3}\" on {4:dd-mm-yy HH:MM:ss}",
                                           lesson.LessonCode, lesson.Subject.SubjectCode, student.StudentCode, student.FullName, DateTime.Now));
                                    }
                                    count++;
                                }
                                catch (Exception ex)
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR", 
                                        string.Format("Cannot create StudentResult for student \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} <BR/> {3} <BR/>{4}",
                                        student.StudentCode, student.FullName, DateTime.Now, ex.Message ,ex.StackTrace));                                    
                                }
                                objectSpace.CommitChanges();
                            }
                            catch (Exception ex)
                            {
                                fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                    ex.Message + ex.StackTrace);
                            }                            
                        }
                        fileStreamlog.WriteLine("</table></body></html>");
                        fileStreamlog.Close();
                    }

                    View.ObjectSpace.SetModified(actFile);
                    actFile.IsImported = true;
                    actFile.ResultLink = "/tempFolder/" + templogname;

                    View.ObjectSpace.CommitChanges();

                }
            }
            PopUpMessage ms;
            DialogController dc;
           
            ms = objectSpace.CreateObject<PopUpMessage>();
            ms.Title = "Kết quả import";
            ms.Message = string.Format("Đã thực hiện import {0} kết quả cho {1} dòng trong file\r\n Vui lòng xem link kết quả", count, iLine);
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                 objectSpace, ms);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.CreatedView.Caption = "Thông báo";
            dc = Application.CreateController<DialogController>();
            dc.AcceptAction.Active.SetItemValue("object", false);
            dc.CancelAction.Caption = "Đóng";
            dc.SaveOnAccept = false;
            e.ShowViewParameters.Controllers.Add(dc);

        }

     
    }
}
