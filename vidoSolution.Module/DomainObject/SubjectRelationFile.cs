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
    [Persistent("SubjectRelationFile")]
    public class SubjectRelationFile : BaseObject
    {
        public SubjectRelationFile(Session session) : base(session) { }
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
        DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set
            {
                if (createDate == null)
                    SetPropertyValue("CreateDate", ref createDate, DateTime.Now);
            }
        }
       // [Action(ToolTip = "Import Subject Relation to Database", TargetObjectsCriteria = "IsImported=false")]
        //public void ImportSubjectRelation()
        //{
        //    Session session = this.Session;
        //    string tempStudentFolderPath;
        //    string tempStudentFile;
        //    string tempStudentLogFile;
        //    if (HttpContext.Current != null)
        //    {
        //        tempStudentFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
        //        tempStudentFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + CsvFile.FileName);
        //        tempStudentLogFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + CsvFile.FileName+DateTime.Now.ToString("dd-MM-yyyy-HHmmss") + "-log.txt");
        //    }
        //    else
        //    {
        //        tempStudentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder");
        //        tempStudentFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", CsvFile.FileName);
        //        tempStudentLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempFolder/", CsvFile.FileName + "-log.txt");
        //    }

        //    if (!Directory.Exists(tempStudentFolderPath))
        //        Directory.CreateDirectory(tempStudentFolderPath);
          
        //    Dictionary<string, int> columnIndexs = new Dictionary<string, int>();
        //    Dictionary<string, string> valueIndexs = new Dictionary<string, string>();
        //    valueIndexs.Add("MAMH1", "");
        //    valueIndexs.Add("TENMH1", "");
        //    valueIndexs.Add("SOTC1", "");
        //    valueIndexs.Add("MAMH2", "");
        //    valueIndexs.Add("TENMH2", "");
        //    valueIndexs.Add("SOTC2", "");
        //    valueIndexs.Add("QUANHE", "");

        //    columnIndexs.Add("MAMH1", -1);
        //    columnIndexs.Add("TENMH1", -1);
        //    columnIndexs.Add("SOTC1", -1);
        //    columnIndexs.Add("MAMH2", -1);
        //    columnIndexs.Add("TENMH2", -1);
        //    columnIndexs.Add("SOTC2", -1);
        //    columnIndexs.Add("QUANHE", -1);
            
           
        //    using (System.IO.FileStream fileStream = new FileStream(tempStudentFile, FileMode.OpenOrCreate))
        //    {
        //        using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempStudentLogFile, true))
        //        {
        //            CsvFile.SaveToStream(fileStream);
        //            fileStream.Position = 0;
        //            StreamReader r = new StreamReader(fileStream);
        //            string newLine;
        //            bool foundHeader = false;
        //            int iLine = 1;
        //            IsImported = true;
        //            try
        //            {
        //                //Tìm dòng chứa TEN cột
        //                while ((newLine = r.ReadLine()) != null)
        //                {
        //                    string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);
        //                    if (!foundHeader)
        //                        for (int i = 0; i < row.Length; i++)
        //                            if (columnIndexs.ContainsKey(row[i].ToUpper().Trim()))
        //                            {
        //                                foundHeader = true;
        //                                columnIndexs[row[i].ToUpper().Trim()] = i; //Đã tìm thấy dòng chứa TEN cột. Xác định vị trí của cột
        //                            }
        //                    if (foundHeader)
        //                        break;
        //                }
        //                if (columnIndexs.Values.Contains(-1))
        //                    throw new UserFriendlyException("Lỗi cấu trúc file");
        //                //Các dòng sau đó đều là dòng dữ liệu
        //                while ((newLine = r.ReadLine()) != null)
        //                {
        //                    iLine++;
        //                    session.BeginTransaction();
        //                    try
        //                    {
        //                        string[] row = newLine.Split(new string[] { "~" }, StringSplitOptions.None);

        //                        foreach (var column in columnIndexs)
        //                        {
        //                            valueIndexs[column.Key] = row[column.Value];
        //                        }
        //                        Subject subject1 = null, subject2 = null;
        //                        //tìm Subject

        //                        subject1 = session.FindObject<Subject>(CriteriaOperator.Parse("SubjectCode = " + valueIndexs["MAMH1"]));
        //                        subject2 = session.FindObject<Subject>(CriteriaOperator.Parse("SubjectCode = " + valueIndexs["MAMH2"]));
        //                        if (subject1 == null) //create new branch
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Cannot find subject 1:{0} - {1}   on line \"{2}\" on {3:dd-mm-yy HH:MM:ss} - CANNOT IMPORT LINE",
        //                             valueIndexs["MAMH1"], valueIndexs["TENMH1"], iLine, DateTime.Now));
        //                            break;
        //                        }
        //                        if (subject1.SubjectName != valueIndexs["TENMH1"].Trim())
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Warning subject 1:{0} NAME {1} not same {2}   on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} ",
        //                            valueIndexs["MAMH1"], subject1.SubjectName, valueIndexs["TENMH1"], iLine, DateTime.Now));                                    
        //                        }
        //                        if (subject1.Credit != Convert.ToDouble(valueIndexs["SOTC1"].Trim()))
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Warning subject 1:{0} CREDIT {1} not same {2}   on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} ",
        //                            valueIndexs["MAMH1"], subject1.Credit, valueIndexs["SOTC1"], iLine, DateTime.Now));                                    
        //                        }
        //                        if (subject2 == null) //create new branch
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Cannot find subject 2:{0} - {1}   on line \"{2}\" on {3:dd-mm-yy HH:MM:ss} - CANNOT IMPORT LINE",
        //                             valueIndexs["MAMH2"], valueIndexs["TENMH2"], iLine, DateTime.Now));
        //                            break;
        //                        }
        //                         if (subject2.SubjectName != valueIndexs["TENMH2"].Trim())
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Warning subject 2:{0} NAME {1} not same {2}   on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} ",
        //                            valueIndexs["MAMH2"], subject2.SubjectName, valueIndexs["TENMH2"], iLine, DateTime.Now));                                    
        //                        }
        //                        if (subject2.Credit != Convert.ToDouble(valueIndexs["SOTC2"].Trim()))
        //                        {
        //                            fileStreamlog.WriteLine(string.Format("Warning subject 2:{0} CREDIT {1} not same {2}   on line \"{3}\" on {4:dd-mm-yy HH:MM:ss} ",
        //                            valueIndexs["MAMH2"], subject2.Credit, valueIndexs["SOTC2"], iLine, DateTime.Now));                                    
        //                        }

        //                        try
        //                        {

        //                            fileStreamlog.WriteLine(string.Format("Create Subject:{0} - {1} ({2}TC)  on line \"{3}\" on {4:dd-mm-yy HH:MM:ss}",
        //                              valueIndexs["MAMH"], valueIndexs["TENMH"], valueIndexs["SOTC"], iLine, DateTime.Now));

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            //if(subject!=null)
        //                            //fileStreamlog.WriteLine(string.Format("Cannot create Subject \"{0}\"-{1} on {2:dd-mm-yy HH:MM:ss} - Line {3}",
        //                            //    subject.SubjectCode, subject.SubjectName, DateTime.Now, iLine));
        //                            fileStreamlog.WriteLine(ex.Message + ex.StackTrace);
        //                        }

        //                        session.CommitTransaction();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \r\n {1}\r\n {1} on {1:dd-mm-yy HH:MM:ss}", iLine, ex.Message, ex.StackTrace, DateTime.Now));
        //                    }
        //                }
        //                fileStreamlog.WriteLine(string.Format("Create \"{0}\" all StudentResult on {1:dd-mm-yy HH:MM:ss}", iLine, DateTime.Now));
        //            }
        //            catch (UserFriendlyException ex)
        //            {
        //                fileStreamlog.WriteLine(string.Format("Error Line \"{0}\": \"{1}\" on {1:dd-mm-yy HH:MM:ss}", iLine, ex.StackTrace, DateTime.Now));
        //                fileStream.Close();
        //                fileStreamlog.Close();
        //                throw ex;
        //            }
        //            finally
        //            {
        //                fileStream.Close();
        //                fileStreamlog.Close();
        //            }
        //        }
        //    }
           
        //}
    }
}
