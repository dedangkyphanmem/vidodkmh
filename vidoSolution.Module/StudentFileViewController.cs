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
using DevExpress.Persistent.BaseImpl;

namespace vidoSolution.Module
{
    public partial class StudentFileViewController : ViewController
    {
        public StudentFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void StudentResultFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }

        void ImportStudentAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();            
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            int count = 0;
            int iLine=0;
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            
            foreach (StudentFile actFile in View.SelectedObjects)
            {
                if (actFile.Note == "")
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
                    valueIndexs.Add("MSSV", "");
                    valueIndexs.Add("HO", "");
                    valueIndexs.Add("TEN", "");
                    valueIndexs.Add("NGAYSINH", "");
                    valueIndexs.Add("LOP", "");
                    valueIndexs.Add("PHAI", "");
                    valueIndexs.Add("DANTOC", "");
                    valueIndexs.Add("NOISINH", "");
                    valueIndexs.Add("MATKHAU", "");
                    valueIndexs.Add("KHOA", "");
                    valueIndexs.Add("MANGANH", "");
                    valueIndexs.Add("TENNGANH", "");
                    valueIndexs.Add("MAKHOA", "");
                    valueIndexs.Add("TENKHOA", "");
                    valueIndexs.Add("NHHKNHAPHOC", "");
                    valueIndexs.Add("NHHKTOTNGHIEP", "");

                    columnIndexs.Add("MSSV", -1);
                    columnIndexs.Add("HO", -1);
                    columnIndexs.Add("TEN", -1);
                    columnIndexs.Add("NGAYSINH", -1);
                    columnIndexs.Add("LOP", -1);
                    columnIndexs.Add("PHAI", -1);
                    columnIndexs.Add("DANTOC", -1);
                    columnIndexs.Add("NOISINH", -1);
                    columnIndexs.Add("MATKHAU", -1);
                    columnIndexs.Add("KHOA", -1);
                    columnIndexs.Add("MANGANH", -1);
                    columnIndexs.Add("TENNGANH", -1);
                    columnIndexs.Add("MAKHOA", -1);
                    columnIndexs.Add("TENKHOA", -1);
                    columnIndexs.Add("NHHKNHAPHOC", -1);
                    columnIndexs.Add("NHHKTOTNGHIEP", -1);

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
                        List<Department> listDepartments = new List<Department>();
                        List<Branch> listBranchs = new List<Branch>();
                        List<Semester> listSemesters = new List<Semester>();
                        List<StudentClass> listStudentClasses = new List<StudentClass>();

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

                                if (valueIndexs["MSSV"] == null || valueIndexs["TEN"] == null || valueIndexs["LOP"] == null || valueIndexs["MATKHAU"] == null)
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                       string.Format("Can not import line with  [MSSV or TEN or LOP or MATKHAU] is NULL on {0:dd-mm-yy HH:MM:ss}",DateTime.Now));
                                    continue;
                                }
                                //tạo khoa
                                Department dept;
                                if (valueIndexs["MAKHOA"] == null)
                                    dept = null;
                                else
                                {
                                    dept = listDepartments.Find(l => l.DepartmentCode == valueIndexs["MAKHOA"].ToString());
                                    if (dept == null)
                                        dept = objectSpace.FindObject<Department>(new BinaryOperator("DepartmentCode", valueIndexs["MAKHOA"].ToString()));
                                    if (dept != null)
                                    {
                                        if (valueIndexs["TENKHOA"]==null || dept.DepartmentName != valueIndexs["TENKHOA"].ToString())
                                            fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                                string.Format("Department: \"{0}\" has name \"{1}\" different to \"{2}\" on {3:dd-mm-yy HH:MM:ss}",
                                                    dept.DepartmentCode, dept.DepartmentName, valueIndexs["TENKHOA"], DateTime.Now));
                                        if (!listDepartments.Contains(dept))
                                            listDepartments.Add(dept);
                                    }
                                    else
                                    {
                                        dept = objectSpace.CreateObject<Department>();
                                        dept.DepartmentCode = valueIndexs["MAKHOA"].ToString();
                                        dept.DepartmentName = valueIndexs["TENKHOA"] == null ? null : valueIndexs["TENKHOA"].ToString();
                                        dept.Save();
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                                string.Format("Create new department: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}",
                                                    dept.DepartmentCode, dept.DepartmentName, DateTime.Now));
                                        listDepartments.Add(dept);
                                    }
                                }
                                Branch branch;
                                if (valueIndexs["MANGANH"] == null)
                                    branch = null;
                                else
                                {
                                    branch = listBranchs.Find(l => l.BranchCode == valueIndexs["MANGANH"].ToString().Trim());
                                    if (branch == null)
                                        branch=objectSpace.FindObject<Branch>(new BinaryOperator("BranchCode", valueIndexs["MANGANH"].ToString().Trim()));
                                    if (branch != null)
                                    {
                                        if (valueIndexs["TENNGANH"]==null || branch.BranchName != valueIndexs["TENNGANH"].ToString())
                                            fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                                string.Format("Branch: \"{0}\" has name \"{1}\" different to \"{2}\" on {3:dd-mm-yy HH:MM:ss}",
                                                    branch.BranchCode, branch.BranchName, valueIndexs["TENNGANH"], DateTime.Now));
                                        if (!listBranchs.Contains(branch))
                                            listBranchs.Add(branch);
                                    }
                                    else
                                    {
                                        branch = objectSpace.CreateObject<Branch>();

                                        branch.BranchCode = valueIndexs["MANGANH"].ToString().Trim();
                                        branch.BranchName = valueIndexs["TENNGANH"]==null?null:valueIndexs["TENNGANH"].ToString().Trim();
                                        branch.Department = dept;
                                        branch.Save();
                                        listBranchs.Add(branch);
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                            string.Format("Create new branch: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}",
                                                branch.BranchCode, branch.BranchName, DateTime.Now));
                                    }
                                }
                                Semester semester;
                                if (valueIndexs["NHHKNHAPHOC"] == null)
                                    semester = null;
                                else
                                {
                                    semester = listSemesters.Find(l => l.SemesterName == valueIndexs["NHHKNHAPHOC"].ToString());
                                    if (semester == null)
                                        semester = objectSpace.FindObject<Semester>(new BinaryOperator("SemesterName", valueIndexs["NHHKNHAPHOC"].ToString()));
                                    if (semester != null)
                                    {
                                        if (!listSemesters.Contains(semester))
                                            listSemesters.Add(semester);
                                    }
                                    else
                                    {
                                        semester = objectSpace.CreateObject<Semester>();
                                        semester.SemesterName = valueIndexs["NHHKNHAPHOC"].ToString();
                                        semester.Save();
                                        listSemesters.Add(semester);
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                            string.Format("Create new semester: \"{0}\" on {1:dd-mm-yy HH:MM:ss}",
                                                semester.SemesterName, DateTime.Now));
                                    }
                                }
                                Semester semesterGraduate;
                                if (valueIndexs["NHHKTOTNGHIEP"] == null)
                                    semesterGraduate = null;
                                else
                                {
                                    semesterGraduate = listSemesters.Find(l => l.SemesterName == valueIndexs["NHHKTOTNGHIEP"].ToString());

                                    if (semesterGraduate == null)
                                        semesterGraduate = objectSpace.FindObject<Semester>(new BinaryOperator("SemesterName", valueIndexs["NHHKTOTNGHIEP"].ToString()));
                                    if (semesterGraduate != null)
                                    {
                                        if (!listSemesters.Contains(semesterGraduate))
                                            listSemesters.Add(semesterGraduate);
                                    }
                                    else
                                    {
                                        semesterGraduate = objectSpace.CreateObject<Semester>();
                                        semesterGraduate.SemesterName = valueIndexs["NHHKTOTNGHIEP"].ToString();
                                        semesterGraduate.Save();
                                        listSemesters.Add(semesterGraduate);
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                            string.Format("Create new semester: \"{0}\" on {1:dd-mm-yy HH:MM:ss}",
                                                semesterGraduate.SemesterName, DateTime.Now));
                                    }
                                }
                                StudentClass studClass = listStudentClasses.Find(
                                    l => l.ClassCode== valueIndexs["LOP"].ToString());
                                if (studClass == null)
                                    studClass = objectSpace.FindObject<StudentClass>(
                                        new BinaryOperator("ClassCode", valueIndexs["LOP"].ToString()));
                                if (studClass != null)
                                {
                                    if (studClass.ClassName != valueIndexs["LOP"].ToString() ||
                                        studClass.Branch != branch ||
                                        studClass.EnrollSemester != semester ||
                                        studClass.GraduateSemester != semesterGraduate )
                                    {
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                           string.Format("StudentClass: \"{0}\" has name [{1}] branch[{2}] enrollsemester [{3}] graduate semester [{4}] different to" +
                                            "[{5}]-[{6}]-[{7}]-[{8}] on {9:dd-mm-yy HH:MM:ss}",
                                               studClass.ClassCode, studClass.ClassName, studClass.Branch, studClass.EnrollSemester,studClass.GraduateSemester,
                                               valueIndexs["LOP"].ToString(), branch, semester,semesterGraduate,                                               
                                               DateTime.Now));
                                    }
                                    if (!listStudentClasses.Contains(studClass))
                                        listStudentClasses.Add(studClass);
                                }
                                else
                                {
                                    studClass = objectSpace.CreateObject<StudentClass>();

                                    studClass.ClassCode = valueIndexs["LOP"].ToString();
                                    studClass.ClassName = valueIndexs["LOP"].ToString();
                                    studClass.Branch = branch;
                                    studClass.EnrollSemester = semester;
                                    studClass.GraduateSemester = semesterGraduate;
                                    studClass.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                        string.Format("Create new class: \"{0}\" on {1:dd-mm-yy HH:MM:ss}", valueIndexs["LOP"], 
                                        DateTime.Now));
                                }

                                //tạo sinh viên
                                Student student = objectSpace.FindObject<Student>(new BinaryOperator("StudentCode", valueIndexs["MSSV"].ToString()));
                                if (student != null)
                                {
                                    student.ChangePasswordOnFirstLogon = true;
                                    student.UserName = valueIndexs["MSSV"].ToString();
                                    student.StudentClass = studClass;
                                    student.FirstName = valueIndexs["HO"]==null?null:valueIndexs["HO"].ToString();
                                    student.LastName = valueIndexs["TEN"].ToString();
                                    try
                                    {
                                        DateTime d = new DateTime(1900, 1, 1).AddDays(
                                            Double.Parse(valueIndexs["NGAYSINH"].ToString()) - 2);
                                        student.BirthdayText = d.ToString("dd/MM/yyyy");
                                    }
                                    catch
                                    {
                                        student.BirthdayText = valueIndexs["NGAYSINH"] == null ? null : valueIndexs["NGAYSINH"].ToString();
                                    }
                                    student.BirthPlace = valueIndexs["NOISINH"] == null ? null : valueIndexs["NOISINH"].ToString();
                                    student.Ethnic = valueIndexs["DANTOC"] == null ? null : valueIndexs["DANTOC"].ToString();
                                    if (valueIndexs["PHAI"]==null || valueIndexs["PHAI"].ToString().Trim() == "0" || valueIndexs["PHAI"].ToString().Trim() == "Nam")
                                        student.IsFemale = false;
                                    else if (valueIndexs["PHAI"].ToString().Trim() == "1" || valueIndexs["PHAI"].ToString().Trim() == "Nữ")
                                        student.IsFemale = true;
                                    else
                                    {
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                        string.Format("PHAI value must be \"Nam\" or \"0\" or \"1\" or \"Nữ\", read value was \"{0}\" on {1:dd-mm-yy HH:MM:ss}",
                                        valueIndexs["PHAI"].ToString().Trim(), DateTime.Now));
                                    }
                                    student.Course = valueIndexs["KHOA"] == null ? null : valueIndexs["KHOA"].ToString();
                                    student.SetPassword(valueIndexs["MATKHAU"].ToString());

                                    Role studRole = objectSpace.FindObject<Role>(new BinaryOperator("Name", "Students"));
                                    student.Roles.Add(studRole);
                                    studRole = objectSpace.FindObject<Role>(new BinaryOperator("Name", "Users"));
                                    student.Roles.Add(studRole);
                                    student.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "UPDATE",
                                        string.Format("Update student: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}", student.StudentCode, student.FullName, DateTime.Now));
                                }
                                else
                                {
                                    student = objectSpace.CreateObject<Student>();

                                    student.ChangePasswordOnFirstLogon = true;
                                    student.UserName = valueIndexs["MSSV"].ToString();
                                    student.StudentClass = studClass;
                                    student.FirstName = valueIndexs["HO"] == null ? null : valueIndexs["HO"].ToString();
                                    student.LastName = valueIndexs["TEN"].ToString();
                                    student.BirthdayText = valueIndexs["NGAYSINH"] == null ? null : valueIndexs["NGAYSINH"].ToString();
                                    student.BirthPlace = valueIndexs["NOISINH"] == null ? null : valueIndexs["NOISINH"].ToString();
                                    student.Ethnic = valueIndexs["DANTOC"] == null ? null : valueIndexs["DANTOC"].ToString();
                                    if (valueIndexs["PHAI"] == null || valueIndexs["PHAI"].ToString() == "0")
                                        student.IsFemale = false;
                                    else if (valueIndexs["PHAI"].ToString() == "1")
                                        student.IsFemale = true;
                                    student.Course = valueIndexs["KHOA"] == null ? null : valueIndexs["KHOA"].ToString();
                                    student.SetPassword(valueIndexs["MATKHAU"].ToString());

                                    Role studRole = objectSpace.FindObject<Role>(new BinaryOperator("Name", "Students"));
                                    student.Roles.Add(studRole);
                                    studRole = objectSpace.FindObject<Role>(new BinaryOperator("Name", "Users"));
                                    student.Roles.Add(studRole);
                                   
                                    student.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                        string.Format("Create student: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}", 
                                        student.StudentCode, student.FullName, DateTime.Now));
                                }  
                                objectSpace.CommitChanges();
                                count++;
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
