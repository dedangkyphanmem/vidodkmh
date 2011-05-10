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
    public partial class AccountTransactionFileViewController : ViewController
    {
        public AccountTransactionFileViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        void AccountTransactionFileViewController_Activated(object sender, System.EventArgs e)
        {
           
        }
        List<Guid> actFilesSelect = new List<Guid>();
        void ImportTransactionDataAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            //ObjectSpace objectSpace =  ObjectSpaceInMemory.CreateNew();
            CollectionSource collectionSource = new CollectionSource(objectSpace, typeof(MyImportResult));
            if ((collectionSource.Collection as XPBaseCollection) != null)
            {
                ((XPBaseCollection)collectionSource.Collection).LoadingEnabled = false;
            }
            actFilesSelect.Clear();
            foreach (AccountTransactionFile actFile in View.SelectedObjects)
            {
                if (actFile.Note == "")
                    throw new UserFriendlyException("Vui lòng thêm thông tin Ghi chú trước khi import!!!");
                if (actFile.Semester ==null)
                    throw new UserFriendlyException("Vui lòng thêm thông tin NHHK trước khi import!!!");

                actFilesSelect.Add(actFile.Oid);
                string tempStudentFolderPath;
                string tempStudentFile;
                string tempStudentLogFile;
                if (HttpContext.Current != null)
                {
                    tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                    tempStudentFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + actFile.CsvFile.FileName);
                    tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.txt");
                }
                else
                {
                    tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
                    tempStudentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", actFile.CsvFile.FileName);
                    tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", actFile.CsvFile.FileName + "-log.html");
                }

                if (!Directory.Exists(tempStudentFolderPath))
                    Directory.CreateDirectory(tempStudentFolderPath);
                using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
                {
                    Dictionary<string, int> columnIndexs = new Dictionary<string, int>();
                    Dictionary<string, object> valueIndexs = new Dictionary<string, object>();
                    valueIndexs.Add("MSSV", "");
                    valueIndexs.Add("HOLOT", "");
                    valueIndexs.Add("TEN", "");
                    valueIndexs.Add("SOTIEN", "");
                    valueIndexs.Add("NGAYNOP", "");
                    valueIndexs.Add("DIENGIAI", "");

                    columnIndexs.Add("MSSV", -1);
                    columnIndexs.Add("HOLOT", -1);
                    columnIndexs.Add("TEN", -1);
                    columnIndexs.Add("SOTIEN", -1);
                    columnIndexs.Add("NGAYNOP", -1);
                    columnIndexs.Add("DIENGIAI", -1);

                    // open xls file
                    actFile.CsvFile.SaveToStream(fileStream);
                    fileStream.Close();
                    Workbook book = Workbook.Open(tempStudentFile);
                    Worksheet sheet = book.Worksheets[0];


                    bool foundHeader = false;
                    int iLine;
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


                    //actFile.IsImported  = true;
                    //header row
                    MyImportResult myImportResult = objectSpace.CreateObject<MyImportResult>();
                    
                    row = sheet.Cells.GetRow(iLine - 1);
                    myImportResult.vwKey = iLine - 1;
                    myImportResult.Line = iLine-1;
                    myImportResult.Data = "";
                    foreach (var column in columnIndexs)
                    {
                        Cell cell = row.GetCell(column.Value);
                        valueIndexs[column.Key] = cell.Value;
                        myImportResult.Data += (myImportResult.Data == "" ? "" : "|") + cell.Value.ToString();
                    }
                    myImportResult.CanImport = false;
                    myImportResult.Message = "HEADER LINE";
                    collectionSource.List.Add(myImportResult);

         
                    //Các dòng sau đó đều là dòng dữ liệu
                    for (; iLine <= sheet.Cells.LastRowIndex; iLine++)
                    {
                        myImportResult = objectSpace.CreateObject<MyImportResult>();
                        row = sheet.Cells.GetRow(iLine);
                        myImportResult.vwKey = iLine;
                        myImportResult.Line = iLine;
                        myImportResult.Data = "";
                        myImportResult.Message = "";
                        myImportResult.Status = "OK";
                        myImportResult.CanImport = true;

                        foreach (var column in columnIndexs)
                        {
                            Cell cell = row.GetCell(column.Value);
                            valueIndexs[column.Key] = cell.Value;
                            myImportResult.Data += (myImportResult.Data == "" ? "" : "|") + (valueIndexs[column.Key] == null ? "" : valueIndexs[column.Key].ToString());
                        }
                        // tìm sinh viên
                        if (valueIndexs["MSSV"] == null)
                        {
                            myImportResult.CanImport = false;
                            myImportResult.Status = "LỖI";
                            myImportResult.Message = (myImportResult.Message == "" ? "" : ",") +
                                string.Format("Cannot find student: \"{0}\"", valueIndexs["MSSV"]);
                        }

                        Student student = objectSpace.FindObject<Student>(new BinaryOperator("StudentCode", valueIndexs["MSSV"]));
                        if (student == null)
                        {
                            myImportResult.CanImport = false;
                            myImportResult.Status = "LỖI";
                            myImportResult.Message = (myImportResult.Message == "" ? "" : ",") +
                                string.Format("Cannot find student: \"{0}\"", valueIndexs["MSSV"]);
                        }
                        else
                        {
                            if (valueIndexs["HOLOT"] == null || valueIndexs["TEN"] == null ||
                                student.FirstName.Trim() != valueIndexs["HOLOT"].ToString().Trim() ||
                                student.LastName.Trim() != valueIndexs["TEN"].ToString().Trim())
                            {
                                myImportResult.Status = (myImportResult.Status == "OK" ? "CẢNH BÁO" : myImportResult.Status);
                                myImportResult.Message += (myImportResult.Message == "" ? "" : ",") +
                                    string.Format("Found StudentCode: \"{0}\" but Full Name \"{1} \" is not like \"{2} {3}\"",
                                        student.StudentCode, student.FullName, valueIndexs["HOLOT"], valueIndexs["TEN"]);
                            }
                        }
                        try
                        {
                            DateTime d = new DateTime(1900, 1, 1).AddDays(
                                Double.Parse(valueIndexs["NGAYNOP"].ToString()) - 2);
                        }
                        catch
                        {
                            myImportResult.Status = (myImportResult.Status == "OK" ? "CẢNH BÁO" : myImportResult.Status);
                            myImportResult.Message += (myImportResult.Message == "" ? "" : ",") +
                                string.Format("Can not convert to DateTime value: {0}",valueIndexs["NGAYNOP"]);
                        }

                        try
                        {
                            decimal money = Decimal.Parse(valueIndexs["SOTIEN"].ToString());
                        }
                        catch 
                        {
                            myImportResult.CanImport = false;
                            myImportResult.Status = "LỖI";
                            myImportResult.Message += (myImportResult.Message == "" ? "" : ",") +
                                string.Format("Cannot convert to Decimal value:{0}",valueIndexs["SOTIEN"]);

                        }
                        collectionSource.List.Add(myImportResult);
                    }
                }
            }
            ListView view = Application.CreateListView(Application.GetListViewId(typeof(MyImportResult)),
                collectionSource, false);
            view.Editor.AllowEdit = false;
            e.ShowViewParameters.CreatedView = view;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.CreateAllControllers = true;
            //args.ShowViewParameters.Context = TemplateContext.View;
            DialogController selectAcception = new DialogController();
            e.ShowViewParameters.Controllers.Add(selectAcception);
            
            selectAcception.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(selectAcception_Accepting);

            selectAcception.AcceptAction.Caption = "Import";
            selectAcception.CancelAction.Caption = "Bỏ qua";

        }

        void selectAcception_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
          
            ListView lv = (ListView)((WindowController)sender).Window.View;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
            int i = 0;
            if (View.SelectedObjects.Count > 0)
            {
                if (lv.SelectedObjects.Count > 0)
                {
                    AccountTransactionFile actFile = objectSpace.FindObject<AccountTransactionFile>(
                        CriteriaOperator.Parse("Oid=?", ((AccountTransactionFile)View.SelectedObjects[0]).Oid));

                    string tempStudentFolderPath;                    
                    string tempStudentLogFile;
                    string templogname = "";
                    if (HttpContext.Current != null)
                    {
                        tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                        templogname = actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
                        tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + templogname);
                    }
                    else
                    {
                        tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
                        templogname = actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.html";
                        tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", templogname);
                    }

                    if (!Directory.Exists(tempStudentFolderPath))
                        Directory.CreateDirectory(tempStudentFolderPath);
                    using (System.IO.StreamWriter fileStreamlog = new System.IO.StreamWriter(tempStudentLogFile, true))
                    {
                        fileStreamlog.WriteLine("<html><header><title>" + actFile.CsvFile.FileName + DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log </title>	" +
                   "<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />" +
                   "</head><body>\r\n<table border=\"1px\"> <tr><Th>DÒNG</Th><th>MSSV</th><th>HỌ TÊN</Th><Th>SỐ TIỀN</Th><Th>NGÀY THỰC HIỆN</Th><Th> NỘI DUNG </Th><th>GHI CHÚ</th></Tr>");
                        foreach (MyImportResult myImportResult in lv.SelectedObjects)
                        {

                            if (myImportResult.CanImport)
                            {
                                fileStreamlog.WriteLine("<tr>");
                                AccountTransaction acc = objectSpace.CreateObject<AccountTransaction>();
                                string[] datas = myImportResult.Data.Split('|');
                                fileStreamlog.WriteLine(string.Format("<TD>{0}</TD>", myImportResult.Line));
                                acc.Student = objectSpace.FindObject<Student>(new BinaryOperator("StudentCode", datas[0]));
                                fileStreamlog.WriteLine(string.Format ("<TD>{0}</td><td>{1}</td>",acc.Student.StudentCode,acc.Student.FullName));
                                acc.MoneyAmount = Decimal.Parse(datas[3].ToString());
                                try
                                {
                                    acc.TransactingDate = new DateTime(1900, 1, 1).AddDays(
                                        Double.Parse(datas[4].ToString()) - 2);
                                }
                                catch
                                {
                                    acc.TransactingDate = DateTime.Now;
                                }
                                acc.Semester = objectSpace.FindObject<Semester>(
                                    CriteriaOperator.Parse("SemesterName = ?", actFile.Semester.SemesterName));
                                acc.Description = datas[5].ToString();
                                acc.DateCreated = acc.DateModified = DateTime.Now;
                                acc.ImportDescription = actFile.Note;
                                fileStreamlog.WriteLine(string.Format("<TD>{0:0,0}</td><td>{1:dd-MM-yyyy} {1:HH}:{1:mm}</td><td>{2}</td><td>{3}</td>", 
                                    acc.MoneyAmount, acc.TransactingDate,acc.Description,actFile.Note));
                                acc.Save();
                                i++;
                                fileStreamlog.WriteLine("</tr>");
                            }
                        }
                        fileStreamlog.WriteLine("</table></body></html>");                       
                        fileStreamlog.Close();
                    }
                    objectSpace.SetModified(actFile);
                    actFile.IsImported = true;
                    actFile.ResultLink = "/tempFolder/" + templogname;

                    objectSpace.CommitChanges();
                    this.View.Refresh();
                    ms.Title = "Kết quả import dữ liệu";
                    ms.Message = string.Format("Đã import thành công {0} dòng dữ liệu vào hệ thống", i);
                    ShowViewParameters svp = new ShowViewParameters();
                    svp.CreatedView = Application.CreateDetailView(
                         objectSpace, ms);
                    svp.TargetWindow = TargetWindow.NewModalWindow;
                    svp.CreatedView.Caption = "Thông báo";
                    DialogController dc = Application.CreateController<DialogController>();
                    svp.Controllers.Add(dc);
                    dc.AcceptAction.Active.SetItemValue("object", false);
                    dc.CancelAction.Caption = "Đóng";
                    dc.SaveOnAccept = false;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                }
            }
        }

     
    }
}
