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
using DevExpress.Persistent.Validation;

namespace vidoSolution.Module
{
    public partial class TeacherFileViewController : ViewController
    {
        public TeacherFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void TeacherFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }

        void ImportTeacherAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();            
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            int count = 0;
            int iLine=0;
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            
            foreach (TeacherFile actFile in View.SelectedObjects)
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
                    valueIndexs.Add("MSGV", "");
                    valueIndexs.Add("HO", "");
                    valueIndexs.Add("TEN", "");
                    valueIndexs.Add("TENVIETTAT", "");
                    valueIndexs.Add("NGAYSINH", "");                   
                    valueIndexs.Add("PHAI", "");
                    valueIndexs.Add("EMAIL", "");
                    valueIndexs.Add("DIENTHOAI", "");
                    valueIndexs.Add("DIDONG", "");
                    valueIndexs.Add("LATHINHGIANG", "");                    
                    valueIndexs.Add("MAKHOA", "");
                    valueIndexs.Add("TENKHOA", "");
                    
                    columnIndexs.Add("MSGV", -1);
                    columnIndexs.Add("HO", -1);
                    columnIndexs.Add("TEN", -1);
                    columnIndexs.Add("TENVIETTAT", -1);
                    columnIndexs.Add("NGAYSINH", -1);
                    columnIndexs.Add("PHAI", -1);
                    columnIndexs.Add("EMAIL", -1);
                    columnIndexs.Add("DIENTHOAI", -1);
                    columnIndexs.Add("DIDONG", -1);
                    columnIndexs.Add("LATHINHGIANG", -1);
                    columnIndexs.Add("MAKHOA", -1);
                    columnIndexs.Add("TENKHOA", -1);
                   

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
                     
                        List<Department> listDepartments = new List<Department>();
                        

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

                                if (valueIndexs["MSGV"] == null || valueIndexs["TEN"] == null || valueIndexs["TENVIETTAT"] == null )
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                       string.Format("Can not import line with  [MSGV or TEN or TENVIETTAT] is NULL on {0:dd-mm-yy HH:MM:ss}",DateTime.Now));
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
                                
                                //tạo giảng viên
                                Teacher teacher = objectSpace.FindObject<Teacher>(new BinaryOperator("TeacherCode", valueIndexs["MSGV"].ToString()));
                                if (teacher != null)
                                {
                                    teacher.FirstName = valueIndexs["HO"] == null ? null : valueIndexs["HO"].ToString();
                                    teacher.LastName = valueIndexs["TEN"].ToString();
                                    teacher.ShortName = valueIndexs["TENVIETTAT"].ToString();
                                    //try
                                    //{
                                    //    DateTime d = new DateTime(1900, 1, 1).AddDays(
                                    //        Double.Parse(valueIndexs["NGAYSINH"].ToString()) - 2);
                                    //    teacher.Birthday = d.ToString("dd/MM/yyyy");
                                    //}
                                    //catch
                                    //{
                                        teacher.Birthday = valueIndexs["NGAYSINH"] == null ? null : valueIndexs["NGAYSINH"].ToString();
                                    //}
                                    if (valueIndexs["PHAI"] == null || valueIndexs["PHAI"].ToString() == "0")
                                        teacher.Sex = false;
                                    else if (valueIndexs["PHAI"].ToString() == "1")
                                        teacher.Sex = true;
                                    teacher.Email = valueIndexs["EMAIL"] == null ? null : valueIndexs["EMAIL"].ToString();
                                    teacher.Phone = valueIndexs["DIENTHOAI"] == null ? null : valueIndexs["DIENTHOAI"].ToString();
                                    teacher.Mobile = valueIndexs["DIDONG"] == null ? null : valueIndexs["DIDONG"].ToString();
                                    if (valueIndexs["LATHINHGIANG"] == null || valueIndexs["LATHINHGIANG"].ToString() == "0")
                                        teacher.isNotEmployee = false;
                                    else if (valueIndexs["LATHINHGIANG"].ToString() == "1")
                                        teacher.isNotEmployee = true;
                                    teacher.Department = dept;
                                    teacher.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "UPDATE",
                                        string.Format("Update teacher: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}", teacher.TeacherCode, teacher.FullName, DateTime.Now));
                                }
                                else
                                {
                                    teacher = objectSpace.CreateObject<Teacher>();
                                    teacher.TeacherCode = valueIndexs["MSGV"].ToString();
                                    teacher.FirstName = valueIndexs["HO"] == null ? null : valueIndexs["HO"].ToString();
                                    teacher.LastName = valueIndexs["TEN"].ToString();
                                    teacher.ShortName = valueIndexs["TENVIETTAT"].ToString();
                                    //try
                                    //{
                                    //    DateTime d = new DateTime(1900, 1, 1).AddDays(
                                    //        Double.Parse(valueIndexs["NGAYSINH"].ToString()) - 2);
                                    //    teacher.Birthday = d.ToString("dd/MM/yyyy");
                                    //}
                                    //catch
                                    //{
                                        teacher.Birthday = valueIndexs["NGAYSINH"] == null ? null : valueIndexs["NGAYSINH"].ToString();
                                    //}
                                    if (valueIndexs["PHAI"] == null || valueIndexs["PHAI"].ToString() == "0")
                                        teacher.Sex = false;
                                    else if (valueIndexs["PHAI"].ToString() == "1")
                                        teacher.Sex = true;
                                    teacher.Email = valueIndexs["EMAIL"] == null ? null : valueIndexs["EMAIL"].ToString();
                                    teacher.Phone = valueIndexs["DIENTHOAI"] == null ? null : valueIndexs["DIENTHOAI"].ToString();
                                    teacher.Mobile = valueIndexs["DIDONG"] == null ? null : valueIndexs["DIDONG"].ToString();
                                    if (valueIndexs["LATHINHGIANG"] == null || valueIndexs["LATHINHGIANG"].ToString() == "0")
                                        teacher.isNotEmployee = false;
                                    else if (valueIndexs["LATHINHGIANG"].ToString() == "1")
                                        teacher.isNotEmployee = true;
                                    teacher.Department = dept;
                                    //RuleSet ruleSet = new RuleSet();
                                    //RuleSetValidationResult result = ruleSet.ValidateTarget(teacher, DefaultContexts.Save);
                                    //if (ValidationState.Invalid ==
                                    //    result.GetResultItem("RuleRequiredField for Teacher.TeacherCode").State)
                                    //{
                                    //    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                    //        string.Format("Cannot create teacher: \"{0}\" with null MSSV on {1:dd-mm-yy HH:MM:ss}",
                                    //    teacher.FullName, DateTime.Now));
                                    //    teacher.Delete();
                                    //}
                                    //else
                                    //{
                                    teacher.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                        string.Format("Create teacher: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}",
                                        teacher.TeacherCode, teacher.FullName, DateTime.Now));
                                    //}
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
