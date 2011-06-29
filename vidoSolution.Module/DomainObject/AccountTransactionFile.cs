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
using ExcelLibrary.Office.Excel;

namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("AccountTransactionFile")]
    public class AccountTransactionFile : BaseObject
    {
        public AccountTransactionFile(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        protected override void OnSaving()
        {
            
            if (CreateDate == null) CreateDate = DateTime.Now;
            base.OnSaving();
        }
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
       

        string link;
    
        public string ResultLink
        {
            get { return link; }
            set { SetPropertyValue("ResultLink", ref link, value); }
        }
        DateTime? createDate=null;
        public DateTime? CreateDate
        {
            get { return createDate; }
            set {  SetPropertyValue<DateTime?>("CreateDate", ref createDate, value); }
        }
        Semester semester;
        [Association("Semester-AccountTransactionFiles")]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue<Semester>("Semester", ref semester, value); }
        }
        

        //[Action(ToolTip = "Import Students TuitionFee Transaction to Database",TargetObjectsCriteria="IsImported = false")]        
        public void ImportStudentTuitionFee()
        {
            if (this.Note == "")
                throw new UserFriendlyException("Vui lòng thêm thông tin Ghi chú trước khi import!!!");
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

            using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
            {
                using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempStudentLogFile, true))
                {
                    // open xls file
                    CsvFile.SaveToStream(fileStream);
                    fileStream.Close();
                    Workbook book = Workbook.Open(tempStudentFile);
                    Worksheet sheet = book.Worksheets[0];


                    bool foundHeader = false;
                    int iLine;

                    //Tìm dòng chứa TEN cột
                    for (iLine = sheet.Cells.FirstRowIndex;
                           iLine <= sheet.Cells.LastRowIndex && !foundHeader; iLine++)
                    {
                        Row row = sheet.Cells.GetRow(iLine);
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
                    //Các dòng sau đó đều là dòng dữ liệu
                    IsImported = true;
                    //Các dòng sau đó đều là dòng dữ liệu

                    for (; iLine <= sheet.Cells.LastRowIndex; iLine++)
                    {
                        using (NestedUnitOfWork uow = Session.BeginNestedUnitOfWork())
                        {
                            uow.BeginTransaction();
                            try
                            {

                                Row row = sheet.Cells.GetRow(iLine);
                                foreach (var column in columnIndexs)
                                {
                                    Cell cell = row.GetCell(column.Value);
                                    valueIndexs[column.Key] = cell.Value;
                                }
                                // tìm sinh viên
                                Student student = session.FindObject<Student>(new BinaryOperator("StudentCode", valueIndexs["MSSV"].ToString()));
                                if (student == null)
                                {
                                    fileStreamlog.WriteLine(string.Format("Cannot find student: \"{0}\" on line \"{1}\" on {2:dd-mm-yy HH:MM:ss}", valueIndexs["MSSV"], iLine+1, DateTime.Now));
                                }
                                else
                                {
                                    if (student.FirstName.Trim() != valueIndexs["HOLOT"].ToString().Trim() ||
                                        student.LastName.Trim() != valueIndexs["TEN"].ToString().Trim())
                                    {
                                        fileStreamlog.WriteLine(string.Format("ERROR Found StudentCode: \"{0}\" but Full Name \"{1} \" is not like \"{2} {3}\" on {4:dd-mm-yy HH:MM:ss} - line {5} - DONOT IMPORT DATA",
                                                student.StudentCode, student.FullName, valueIndexs["HOLOT"],
                                                valueIndexs["TEN"], DateTime.Now, iLine+1));

                                    }
                                    else
                                    {

                                        try
                                        {
                                            double date = Double.Parse(valueIndexs["NGAYNOP"].ToString());
                                            AccountTransaction acc = new AccountTransaction(session)
                                            {
                                                Student = student,
                                               
                                                MoneyAmount = Convert.ToDecimal(valueIndexs["SOTIEN"]),
                                                //TransactingDate = Convert.ToDateTime(valueIndexs["NGAYNOP"]),
                                                TransactingDate = new DateTime(1900, 1, 1).AddDays(
                                                    date - 2),
                                                Description = valueIndexs["DIENGIAI"].ToString(),
                                                DateCreated = DateTime.Now,
                                                DateModified = DateTime.Now,
                                                ImportDescription = this.Note
                                            
                                            };
                                            acc.Save();
                                            fileStreamlog.WriteLine(string.Format("Create transaction with money amount {0} for student: \"{1}\"-\"{2}\" on {3:dd-mm-yy HH:MM:ss} - line {4}",
                                                valueIndexs["SOTIEN"], student.StudentCode, student.FullName, 
                                                DateTime.Now, iLine+1));
                                        }
                                        catch (Exception ex)
                                        {
                                            fileStreamlog.WriteLine(string.Format("ERROR: Cannot create Transaction for student \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} - Line {3}",
                                                student.StudentCode, student.FullName, DateTime.Now, iLine+1));
                                            fileStreamlog.WriteLine(ex.Message + ex.StackTrace);
                                        }
                                    }
                                }
                                session.CommitTransaction();
                            }
                            catch (Exception ex)
                            {
                                fileStreamlog.WriteLine(string.Format("ERROR: Line \"{0}\": \r\n {1}\r\n {1} on {1:dd-mm-yy HH:MM:ss}", iLine+1, ex.Message, ex.StackTrace, DateTime.Now));
                            }
                            uow.CommitChanges();
                        }
                    }
                }
            }
            
            #region Importdata
            //using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
            //{
            //    using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempStudentLogFile, true))
            //    {
            //        CsvFile.SaveToStream(fileStream);
            //        fileStream.Position = 0;
            //        StreamReader r = new StreamReader(fileStream);
            //        string newLine;
            //        bool foundHeader = false;
            //        int iLine = 1;
                    
            //        try
            //        {
                     
            //            //Tìm dòng chứa TEN cột
            //            while ((newLine = r.ReadLine()) != null)
            //            {
            //                string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);
            //                if (!foundHeader)
            //                {
            //                    for (int i = 0; i < row.Length; i++)
            //                        if (columnIndexs.ContainsKey(row[i].ToUpper().Trim()))
            //                        {
            //                            columnIndexs[row[i].ToUpper().Trim()] = i; //Đã tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
            //                        }
            //                    if (!columnIndexs.Values.Contains(-1))
            //                    {
            //                        foundHeader = true;
            //                        break;
            //                    }
            //                    else
            //                    {
            //                        for (int i = 0; i < row.Length; i++)
            //                            if (columnIndexs.ContainsKey(row[i].ToUpper().Trim()))
            //                            {
            //                                columnIndexs[row[i].ToUpper().Trim()] = -1; //không tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
            //                            }
            //                    }
            //                }
                            
            //            }
            //            if (!foundHeader)
            //                throw new UserFriendlyException("Lỗi cấu trúc file");
            //            //Các dòng sau đó đều là dòng dữ liệu
            //            IsImported = true;
            //            //Các dòng sau đó đều là dòng dữ liệu
            //            while ((newLine = r.ReadLine()) != null)
            //            {
            //                iLine++;
            //                session.BeginTransaction();
            //                try
            //                {
            //                    string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);

            //                    foreach (var column in columnIndexs)
            //                    {
            //                        valueIndexs[column.Key] = row[column.Value];
            //                    }

            //                     //tìm sinh viên
            //                    Student student = session.FindObject<Student>(new BinaryOperator("StudentCode", valueIndexs["MSSV"]));
            //                    if (student == null)
            //                    {
            //                        fileStreamlog.WriteLine(string.Format("Cannot find student: \"{0}\" on line \"{1}\" on {2:dd-mm-yy HH:MM:ss}", valueIndexs["MSSV"], iLine, DateTime.Now));
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            AccountTransaction acc = new AccountTransaction(session)
            //                            {
            //                                Student = student,
            //                                MoneyAmount = Convert.ToDecimal(valueIndexs["SOTIEN"]),
            //                                TransactingDate = Convert.ToDateTime(valueIndexs["NGAYNOP"]),
            //                                Description = valueIndexs["DIENGIAI"]
            //                            };
            //                            acc.Save();
            //                            fileStreamlog.WriteLine(string.Format("Create transaction with money amount {0} for student: \"{1}\"-\"{2}\" on {3:dd-mm-yy HH:MM:ss} - line {4}", 
            //                                valueIndexs["SOTIEN"], student.StudentCode, student.FullName, DateTime.Now, iLine

            //                                ));
            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            fileStreamlog.WriteLine(string.Format("Cannot create Transaction for student \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} - Line {3}",
            //                                student.StudentCode, student.FullName, DateTime.Now, iLine));
            //                            fileStreamlog.WriteLine(ex.Message + ex.StackTrace);
            //                        }              
            //                    }                             
            //                    session.CommitTransaction();
            //                }
            //                catch (Exception ex)
            //                {
            //                    fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \r\n {1}\r\n {1} on {1:dd-mm-yy HH:MM:ss}", iLine, ex.Message, ex.StackTrace, DateTime.Now));
            //                }
            //            }
            //            fileStreamlog.WriteLine(string.Format("Create \"{0}\" all transaction on {1:dd-mm-yy HH:MM:ss}", iLine, DateTime.Now));
            //        }
            //        catch (UserFriendlyException ex)
            //        {
            //            fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \"{1}\" on {1:dd-mm-yy HH:MM:ss}", iLine, ex.StackTrace, DateTime.Now));
            //            fileStream.Close();
            //            fileStreamlog.Close();
            //            throw ex;
            //        }
            //        finally
            //        {
            //            fileStream.Close();
            //            fileStreamlog.Close();
            //        }
            //    }
            //}
            #endregion

        }
    }

}
