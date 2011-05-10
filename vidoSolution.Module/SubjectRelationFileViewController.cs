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
    public partial class SubjectRelationFileViewController : ViewController
    {
        public SubjectRelationFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void SubjectRelationFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }

        void ImportSubjectRelationAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();            
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            int count = 0;
            int iLine=0;
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            
            foreach (SubjectRelationFile actFile in View.SelectedObjects)
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
                    valueIndexs.Add("MAMH1", "");
                    valueIndexs.Add("TENMH1", "");
                    valueIndexs.Add("SOTC1", "");
                    valueIndexs.Add("MAMH2", "");
                    valueIndexs.Add("TENMH2", "");
                    valueIndexs.Add("SOTC2", "");
                    valueIndexs.Add("QUANHE", "");
                    
                    columnIndexs.Add("MAMH1", -1);
                    columnIndexs.Add("TENMH1", -1);
                    columnIndexs.Add("SOTC1", -1);
                    columnIndexs.Add("MAMH2", -1);
                    columnIndexs.Add("TENMH2", -1);
                    columnIndexs.Add("SOTC2", -1);
                    columnIndexs.Add("QUANHE", -1);
                   

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

                        List<Subject> listSubjects = new List<Subject>();
                        

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

                                if (valueIndexs["MAMH1"] == null || valueIndexs["TENMH1"] == null || valueIndexs["SOTC1"] == null ||
                                    valueIndexs["MAMH2"] == null || valueIndexs["TENMH2"] == null || valueIndexs["SOTC2"] == null || valueIndexs["QUANHE"] == null)
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                       string.Format("Can not import line with NULL value on {0:dd-mm-yy HH:MM:ss}",DateTime.Now));
                                    continue;
                                }
                                                                
                                //tìm MÔN HỌC
                                Subject subject1 = objectSpace.FindObject<Subject>(new BinaryOperator("SubjectCode", valueIndexs["MAMH1"].ToString().Trim()));
                                if (subject1 == null)
                                {                                    
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                        string.Format("Cannot import because cannot find subject: [{0}]-[{1}({2:#.#})] on {3:dd-mm-yy HH:MM:ss}",
                                       valueIndexs["MAMH1"].ToString(), valueIndexs["TENMH1"].ToString(), valueIndexs["SOTC1"].ToString(), DateTime.Now));
                                    continue;
                                }
                                else
                                {
                                    if (subject1.SubjectName != valueIndexs["TENMH1"].ToString().Trim()||
                                        subject1.Credit.ToString().Trim() != valueIndexs["SOTC1"].ToString().Trim())

                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                         string.Format("Subject: [{0}]-[{1}({2:#.#})] is not the same as data [{3}]-[{4}({5:#.#})] on {6:dd-mm-yy HH:MM:ss}",
                                        subject1.SubjectCode, subject1.SubjectName, subject1.Credit, 
                                        valueIndexs["MAMH1"].ToString(), valueIndexs["TENMH1"].ToString(), valueIndexs["SOTC1"].ToString(),
                                        DateTime.Now));
                                }
                                Subject subject2 = objectSpace.FindObject<Subject>(new BinaryOperator("SubjectCode", valueIndexs["MAMH2"].ToString().Trim()));
                                if (subject2 == null)
                                {
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "ERROR",
                                        string.Format("Cannot import because cannot find subject: [{0}]-[{1}({2:#.#})] on {3:dd-mm-yy HH:MM:ss}",
                                       valueIndexs["MAMH2"].ToString(), valueIndexs["TENMH2"].ToString(), valueIndexs["SOTC2"].ToString(), DateTime.Now));
                                    continue;
                                }
                                else
                                {
                                    if (subject2.SubjectName != valueIndexs["TENMH2"].ToString().Trim() ||
                                        subject2.Credit.ToString().Trim() != valueIndexs["SOTC2"].ToString().Trim())

                                        fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "WARNING",
                                             string.Format("Subject: [{0}]-[{1}({2:#.#})] is not the same as data [{3}]-[{4}({5:#.#})] on {6:dd-mm-yy HH:MM:ss}",
                                            subject2.SubjectCode, subject2.SubjectName, subject2.Credit,
                                            valueIndexs["MAMH2"].ToString(), valueIndexs["TENMH2"].ToString(), valueIndexs["SOTC2"].ToString(),
                                            DateTime.Now));
                                }
                                SubjectRelations sr = objectSpace.FindObject<SubjectRelations>(
                                    CriteriaOperator.Parse("Subject1.SubjectCode = ? AND Subject2.SubjectCode=?", subject1.SubjectCode, subject2.SubjectCode));
                                if (sr != null)
                                {
                                    objectSpace.SetModified(sr);
                                    sr.Type = Convert.ToInt32(valueIndexs["QUANHE"].ToString());
                                    objectSpace.CommitChanges();
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "UPDATE",
                                                 string.Format("Subject: [{0}]-[{1}({2:#.#})] RELATED TO [{3}]-[{4}({5:#.#})] WITH TYPE \"{6}\" on {7:dd-mm-yy HH:MM:ss}",
                                                    subject1.SubjectCode, subject1.SubjectName, subject1.Credit,
                                                    subject2.SubjectCode, subject2.SubjectName, subject2.Credit,                                                
                                                    valueIndexs["QUANHE"].ToString(), DateTime.Now));
                                }
                                else
                                {
                                    //tạo quan hệ môn học
                                    sr = objectSpace.CreateObject<SubjectRelations>();
                                    sr.Subject1 = subject1;
                                    sr.Subject2 = subject2;
                                    sr.Type = Convert.ToInt32(valueIndexs["QUANHE"].ToString());
                                    objectSpace.CommitChanges();
                                    count++;
                                    fileStreamlog.WriteLine("<TR><td>{0}</td><td>{1}</td><td>{2}</td></tr>", iLine, "CREATE NEW",
                                                 string.Format("Subject: [{0}]-[{1}({2:#.#})] RELATED TO [{3}]-[{4}({5:#.#})] WITH TYPE \"{6}\" on {7:dd-mm-yy HH:MM:ss}",
                                                    subject1.SubjectCode, subject1.SubjectName, subject1.Credit,
                                                    subject2.SubjectCode, subject2.SubjectName, subject2.Credit,
                                                    valueIndexs["QUANHE"].ToString(), DateTime.Now));
                                }
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
