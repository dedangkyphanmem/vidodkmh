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
    public partial class SubjectFileViewController : ViewController
    {
        public SubjectFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void SubjectFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }

        void ImportSubjectAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();            
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            int count = 0;
            int iLine=0;
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            
            foreach (SubjectFile actFile in View.SelectedObjects)
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
                    valueIndexs.Add("MAMH", "");
                    valueIndexs.Add("TENMH", "");
                    valueIndexs.Add("SOTC", "");
                    valueIndexs.Add("MANGANH", "");
                    valueIndexs.Add("TENNGANH", "");

                    columnIndexs.Add("MAMH", -1);
                    columnIndexs.Add("TENMH", -1);
                    columnIndexs.Add("SOTC", -1);
                    columnIndexs.Add("MANGANH", -1);
                    columnIndexs.Add("TENNGANH", -1);
                   

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

                        List<Branch> listBranches = new List<Branch>();
                        

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

                                if (valueIndexs["MAMH"] == null || valueIndexs["TENMH"] == null || valueIndexs["SOTC"] == null )
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                       string.Format("Can not import line with  [MAMH or TENMH or SOTINCHI] is NULL on {0:dd-mm-yy HH:MM:ss}",DateTime.Now));
                                    continue;
                                }
                                //tạo NGÀNH
                                Branch branch;
                                if (valueIndexs["MANGANH"] == null)
                                    branch = null;
                                else
                                {
                                    branch = listBranches.Find(l => l.BranchCode == valueIndexs["MANGANH"].ToString());
                                    if (branch == null)
                                        objectSpace.FindObject<Branch>(new BinaryOperator("BranchCode", valueIndexs["MANGANH"].ToString()));
                                    if (branch != null)
                                    {
                                        if (valueIndexs["TENNGANH"] == null || branch.BranchName != valueIndexs["TENNGANH"].ToString())
                                            fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                                string.Format("Branch: \"{0}\" has name \"{1}\" different to \"{2}\" on {3:dd-mm-yy HH:MM:ss}",
                                                    branch.BranchCode, branch.BranchName, valueIndexs["TENNGANH"], DateTime.Now));
                                        if (!listBranches.Contains(branch))
                                            listBranches.Add(branch);
                                    }
                                    else
                                    {
                                        branch = objectSpace.CreateObject<Branch>();

                                        branch.BranchCode = valueIndexs["MANGANH"].ToString();
                                        branch.BranchName = valueIndexs["TENNGANH"] == null ? null : valueIndexs["TENNGANH"].ToString();                                        
                                        branch.Save();
                                        listBranches.Add(branch);
                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                            string.Format("Create new branch: \"{0}\"-\"{1}\" on {2:dd-mm-yy HH:MM:ss}",
                                                branch.BranchCode, branch.BranchName, DateTime.Now));
                                    }
                                }
                                
                                //tạo MÔN HỌC
                                Subject subject = objectSpace.FindObject<Subject>(new BinaryOperator("SubjectCode", valueIndexs["MAMH"].ToString()));
                                if (subject != null)
                                {
                                    //subject.SubjectCode = valueIndexs["MAMH"].ToString();
                                    subject.SubjectName = valueIndexs["TENMH"].ToString();
                                    subject.Credit = (double)valueIndexs["SOTC"];
                                    subject.Branch = branch;
                                    subject.Note = actFile.Note;
                                    subject.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "UPDATE",
                                        string.Format("Update subject: [{0}]-[{1}({2:#.#})] on {3:dd-mm-yy HH:MM:ss}",
                                        subject.SubjectCode, subject.SubjectName, subject.Credit, DateTime.Now));
                                }
                                else
                                {
                                    subject = objectSpace.CreateObject<Subject>();
                                    subject.SubjectCode = valueIndexs["MAMH"].ToString();
                                    subject.SubjectName = valueIndexs["TENMH"].ToString();
                                    subject.Credit = (double)valueIndexs["SOTC"];
                                    subject.Branch = branch;
                                    subject.Note = actFile.Note;
                                    subject.Save();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                         string.Format("Create subject: [{0}]-[{1}({2:#.#})] on {3:dd-mm-yy HH:MM:ss}",
                                        subject.SubjectCode, subject.SubjectName, subject.Credit, DateTime.Now));

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
