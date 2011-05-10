using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ICSharpCode.SharpZipLib.Zip;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Data;
using System.Collections;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo.DB;


namespace vidoSolution.Module.DomainObject
{
    [DefaultClassOptions]
    [Persistent("TkbFiles")]
    public class TkbFile : BaseObject
    {
        public TkbFile(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private Semester semester;
        [Association("Semester-TkbFiles", typeof(Semester))]
        public Semester Semester
        {
            get { return semester; }
            set { SetPropertyValue("Semester", ref semester, value); }
        }

        FileData xmlFile;
        public FileData XmlFile
        {
            get { return xmlFile; }
            set { SetPropertyValue("XmlFile", ref xmlFile, value); }
        }

        FileData zipFile;
        public FileData ZipFile
        {
            get { return zipFile; }
            set { SetPropertyValue("ZipFile", ref zipFile, value); }
        }

        string localPath;
        [Size(255)]
        public string LocalPath
        {
            get { return localPath; }
            set { SetPropertyValue("LocalPath", ref localPath, value); }
        }

        DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { SetPropertyValue("CreateDate", ref createDate, value); }
        }

        bool active;
        public bool Active
        {
            get { return active; }
            set { SetPropertyValue("Active", ref active, value); }
        }

        string note;
        [Size(255)]
        public string Note
        {
            get { return note; }
            set { SetPropertyValue("Note", ref note, value); }
        }

        string fResultMessage;
        [Size(255)]
        public string ResultMessage
        {
            get { return fResultMessage; }
            set { SetPropertyValue("ResultMessage", ref fResultMessage, value); }
        }

        [Action(ToolTip = "Giải nén file zip")]
        public void ExtractZipFile()
        {
            string tempZipFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder/tempZipFile");
            string tempUnzipFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder/tempUnzipFile");
            string tempViewTKBFolderPath = HttpContext.Current.Request.MapPath("~/TKB/View");
            string tempZipFile = HttpContext.Current.Request.MapPath("~/tempFolder/tempZipFile/" + zipFile.FileName);

            DirectoryInfo tempZipFolder = new DirectoryInfo(tempZipFolderPath);
            if (tempZipFolder.GetFiles().Length > 0)
            {
                tempZipFolder.Delete(true);
                tempZipFolder.Create();
            }
            DirectoryInfo tempUnzipFolder = new DirectoryInfo(tempUnzipFolderPath);
            if (tempUnzipFolder.GetFiles().Length > 0)
            {
                tempUnzipFolder.Delete(true);
                tempUnzipFolder.Create();
            }
            DirectoryInfo tempViewTKBFolder = new DirectoryInfo(tempViewTKBFolderPath);
            if (tempViewTKBFolder.Exists)
            {
                tempViewTKBFolder.Delete(true);
            }

            using (System.IO.FileStream fileStream = new FileStream(tempZipFile, FileMode.OpenOrCreate))
            {
                zipFile.SaveToStream(fileStream);
                fileStream.Position = 0;

                ZipInputStream zipInStream = new ZipInputStream(fileStream);

                ZipEntry entry;
                while ((entry = zipInStream.GetNextEntry()) != null)
                {
                    string fileName = Path.GetFileName(entry.Name);
                    string folderName = Path.GetDirectoryName(entry.Name);
                    Directory.CreateDirectory(tempUnzipFolderPath + @"\" + folderName);
                    if (fileName != string.Empty)
                    {
                        FileStream fileStreamOut = new FileStream(tempUnzipFolderPath + @"\" + entry.Name, FileMode.Create, FileAccess.Write);

                        int size;
                        byte[] buffer = new byte[fileStream.Length];
                        do
                        {
                            size = zipInStream.Read(buffer, 0, buffer.Length);
                            fileStreamOut.Write(buffer, 0, size);
                        } while (size > 0);

                        fileStreamOut.Close();
                    }
                }
            }

            if (tempUnzipFolder.GetDirectories().Length == 1)
                tempUnzipFolder.GetDirectories()[0].MoveTo(HttpContext.Current.Request.MapPath("TKB/View/"));
            LocalPath = "http://" + HttpContext.Current.Request.Url.Authority + "/TKB/View/index.html";
        }

        [Action(ToolTip = "Import Thời Khóa Biểu vào CSDL")]
        public void ImportTKB()
        {
            #region path
            string tempXmlFolderPath;
            string tempXmlFile;
            string tempXmlFileLog;
            if (HttpContext.Current != null)
            {
                tempXmlFolderPath = HttpContext.Current.Request.MapPath("~/tempFolder");
                tempXmlFile = HttpContext.Current.Request.MapPath("~/tempFolder/" + XmlFile.FileName);
                tempXmlFileLog = HttpContext.Current.Request.MapPath("~/tempFolder/" + XmlFile.FileName + "-log.txt");
            }
            else 
            {
                tempXmlFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"tempFolder/");
                tempXmlFile =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"tempFolder/",XmlFile.FileName);
                tempXmlFileLog=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"tempFolder/" , XmlFile.FileName + "-log.txt");
            }

            if (!Directory.Exists(tempXmlFolderPath))
                Directory.CreateDirectory(tempXmlFolderPath);
            DirectoryInfo tempXmlFolder = new DirectoryInfo(tempXmlFolderPath);
            if (tempXmlFolder.GetFiles().Length > 0)
            {
                tempXmlFolder.Delete(true);
                tempXmlFolder.Create();
            }
            #endregion

            if (Semester != null && Note != "")
            {
                using (System.IO.FileStream fileStream = new FileStream(tempXmlFile, FileMode.OpenOrCreate))
                {
                    using (System.IO.StreamWriter fileStreamlog = new StreamWriter(tempXmlFileLog, true))
                    {
                        Session.BeginTransaction();
                        try
                        {
                      
                            #region Import data
                            XmlFile.SaveToStream(fileStream);


                            fileStream.Position = 0;
                            XElement doc = XElement.Load(fileStream);

                            #region Create List
                            
                            Dictionary<string, TkbGrade> gradeDic = new Dictionary<string, TkbGrade>();
                            Dictionary<string, TkbDay > dayDic = new Dictionary<string, TkbDay>();
                            Dictionary<string, TkbPeriod> periodDic = new Dictionary<string, TkbPeriod>();
                            Dictionary<string, TkbTeacher> teacherDic = new Dictionary<string, TkbTeacher>();
                            Dictionary<string, Subject> subjectDic = new Dictionary<string, Subject>();
                            Dictionary<string, Classroom> roomDic = new Dictionary<string, Classroom>();
                            Dictionary<string, TkbClass> classDic = new Dictionary<string, TkbClass>();
                            Dictionary<string, TkbGroup> groupDic = new Dictionary<string, TkbGroup>();
                            Dictionary<string, Lesson> lessonDic = new Dictionary<string, Lesson>();
                            List<TkbCard> cardsList = new List<TkbCard>();
                            List<TkbSemester> tkbsemesterList = new List<TkbSemester>();
                            #endregion
                            #region Create Grade Data
                            if (doc.Element(XName.Get("grades")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportGrade(uow,doc.Element(XName.Get("grades")), gradeDic, fileStreamlog);
                                uow.CommitTransaction();
                            }
                            #endregion
                            #region Create Day Data
                            if (doc.Element(XName.Get("days")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportDays(uow,doc.Element(XName.Get("days")),dayDic, fileStreamlog);
                                uow.CommitTransaction();

                            }
                            #endregion
                            #region Create Period Data
                            if (doc.Element(XName.Get("periods")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportPeriods(uow,doc.Element(XName.Get("periods")), periodDic,fileStreamlog);
                                uow.CommitTransaction();

                            }
                            #endregion
                            #region Create Teacher Data

                            if (doc.Element(XName.Get("teachers")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportTeachers(uow, doc.Element(XName.Get("teachers")), teacherDic, fileStreamlog);
                                uow.CommitTransaction();

                            }

                            #endregion
                            #region Create Subject Data

                            if (doc.Element(XName.Get("subjects")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportSubjects(Session,doc.Element(XName.Get("subjects")), subjectDic, fileStreamlog);
                                uow.CommitTransaction();

                            }

                            #endregion
                            #region Create Classroom Data

                            if (doc.Element(XName.Get("classrooms")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportClassrooms(Session,doc.Element(XName.Get("classrooms")), roomDic, fileStreamlog);
                                uow.CommitTransaction();

                            }
#endregion 
                            #region Create Class Data

                            if (doc.Element(XName.Get("classes")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportClass(uow,doc.Element(XName.Get("classes")), classDic, fileStreamlog);
                                uow.CommitTransaction();

                            }
                            fileStreamlog.WriteLine(String.Format("Create {0} classes successfully on database {1: dd-mm-yyyy HH:MM:ss}", classDic.Count, DateTime.Now));

                            #endregion
                            #region Create Group Data
                            if (doc.Element(XName.Get("groups")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportGroup(uow,doc.Element(XName.Get("groups")),groupDic, fileStreamlog);
                                uow.CommitTransaction();

                            }
                            #endregion
                            #region Create Lesson Data

                            if (doc.Element(XName.Get("lessons")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportLessons(Session ,doc.Element(XName.Get("lessons")), lessonDic, 
                                    subjectDic, classDic, teacherDic, fileStreamlog);
                                uow.CommitTransaction();

                            }
                            #endregion
                            #region Create Card Data

                            if (doc.Element(XName.Get("cards")) != null)
                            {
                                NestedUnitOfWork uow = Session.BeginNestedUnitOfWork();
                                uow.BeginTransaction();
                                ImportCards(uow, doc.Element(XName.Get("cards")), cardsList, fileStreamlog);
                                uow.CommitTransaction();
                            }

                            #endregion
                            #region Create Semester Data
                            TkbSemester tkbsemester;
                            foreach (TkbCard card in cardsList)
                            {
                                Classroom classroom = (roomDic.ContainsKey(card.ClassroomIDs) ? roomDic[card.ClassroomIDs] : null);
                                Lesson lesson = (lessonDic.ContainsKey(card.LessonID)? lessonDic[card.LessonID]:null);
                                tkbsemester = tkbsemesterList.Find(t => (t.Classroom == classroom && t.Day == card.Day && t.Lesson == lesson));
                                if (tkbsemester != null)
                                {
                                    tkbsemester.Period += "," + card.Period.ToString();
                                    fileStreamlog.WriteLine(String.Format("Update tkbsemester: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", tkbsemester.Name, DateTime.Now));
                                    tkbsemester.Save();
                                }
                                else
                                {
                                    tkbsemester = new TkbSemester(Session)
                                    {
                                        Day = card.Day,
                                        Period = card.Period.ToString(),
                                        Note = this.Note
                                    };
                                    tkbsemester.Lesson = lesson;
                                    tkbsemester.Weeks = (lesson.TkbLesson!=null?lesson.TkbLesson.Week:"");
                                    tkbsemester.Classroom = classroom;
                                    fileStreamlog.WriteLine(String.Format("Create tkbsemester: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", tkbsemester.Name, DateTime.Now));
                                    tkbsemester.Save();
                                    tkbsemesterList.Add(tkbsemester);
                                }
                            }
                            #endregion


                            #endregion

                            fileStreamlog.WriteLine(String.Format("Create {0} lessons Data successfully on {1: dd-mm-yyyy HH:MM:ss}", lessonDic.Count, DateTime.Now));
                            
                        }
                        catch (Exception ex)
                        {
                            fileStreamlog.WriteLine(String.Format("Error \"{0}\" on {1: dd-mm-yyyy HH:MM:ss}", ex.Message, DateTime.Now));
                            fileStreamlog.WriteLine(String.Format("Strack Trace: \"{0}\" on {1: dd-mm-yyyy HH:MM:ss}", ex.StackTrace, DateTime.Now));

                        }
                        finally
                        {
                            fileStreamlog.Close();
                            fileStream.Close();

                        }
                        Session.CommitTransaction();
                        
                    }
                }
                ResultMessage = "See log file for detail: Path= \"./tempFolder/" + XmlFile.FileName + "-log.txt\"";
            }
            else
            {
                ResultMessage = "Cannot import data without Semester and Note ";

            }
        }

        private void ImportCards(UnitOfWork uow, XElement xElement, List<TkbCard> tkbCardList,StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {

                string sid = "", classroomids = "";
                int iday = 0, iperiod = 0;
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "lessonid":
                            sid = column.Value;
                            break;
                        case "classroomids":
                            classroomids = column.Value;
                            break;
                        case "day":
                            iday = int.Parse(column.Value);
                            break;
                        case "period":
                            iperiod = int.Parse(column.Value);
                            break;
                    }

                }
                TkbCard card = new TkbCard(uow)
                {
                    Semester =uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid)),
                    Day = iday,
                    Period = iperiod,
                    ClassroomIDs = classroomids,
                    LessonID = sid,
                    Note = this.Note
                };
                card.Save();
                card.Reload();
                fileStreamlog.WriteLine(String.Format("Create card: \"{0}-{1}-{2}\" successful on {1: dd-mm-yyyy HH:MM:ss}", card.LessonID, card.ClassroomIDs, DateTime.Now));
                tkbCardList.Add(card);
            }
            fileStreamlog.WriteLine(String.Format("Create {0} cards successful on {1: dd-mm-yyyy HH:MM:ss}", tkbCardList.Count, DateTime.Now));
                           
        }

        private void ImportLessons(Session uow, XElement xElement, 
            Dictionary <string,Lesson> lessonDic, 
            Dictionary<string,Subject> subjectDic, 
            Dictionary<string,TkbClass> classDic,
            Dictionary<string,TkbTeacher> teacherDic, 
            StreamWriter fileStreamlog)
        {
            SortProperty sort = new SortProperty("LessonCode", SortingDirection.Descending);
            int nextcode = 0;
            using (XPCollection collection = new XPCollection(uow, typeof(Lesson), null, sort) { TopReturnedObjects = 1 })
            {                
                if (collection.Count > 0)
                {
                    Lesson firstLesson = collection[0] as Lesson;
                    nextcode = firstLesson.LessonCode;
                }
            }
            int newcode=Convert.ToInt32(Semester.SemesterName+"000"); //first lesson of semester
            if (newcode > nextcode)
                nextcode = newcode;
            foreach (XElement row in xElement.Elements())
            {
                string sid = "", classids = "", subjectid = "", teacherids = "", classroomids = "", groupids = "";
                string weeks = "", studentcoustudentidsnt = "", periodsperweek = "";
                int periodspercard = 0;

                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            sid = column.Value;
                            break;
                        case "classids":
                            classids = column.Value;
                            break;
                        case "subjectid":
                            subjectid = column.Value;
                            break;
                        case "periodspercard":
                            periodspercard = int.Parse(column.Value);
                            break;
                        case "periodsperweek":
                            periodsperweek = column.Value;
                            break;
                        case "teacherids":
                            teacherids = column.Value;
                            break;
                        case "classroomids":
                            classroomids = column.Value;
                            break;
                        case "groupids":
                            groupids = column.Value;
                            break;
                        case "studentcoustudentidsnt":
                            studentcoustudentidsnt = column.Value;
                            break;
                        case "weeks":
                            weeks = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbLesson tkblesson = uow.FindObject<TkbLesson>(new BinaryOperator("ID", sid));
                if (tkblesson == null)
                {
                    tkblesson = new TkbLesson(uow)
                    {
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid)),
                        ID = sid,
                        PeriodsPerCard = periodspercard,
                        PeriodsPerWeek = periodsperweek,
                        ClassroomIDs = classroomids,
                        GroupIDs = groupids,
                        StudentIDs = studentcoustudentidsnt,
                        Week = weeks,
                        Note = this.Note,
                        SubjectID = (subjectDic.ContainsKey(subjectid)?subjectDic[subjectid].SubjectCode:null)
                    };
                    tkblesson.ClassIDs = "";
                    if (classids != "")
                    {
                        foreach (string classid in classids.Split(','))
                        {
                            tkblesson.ClassIDs += (classDic.ContainsKey(classid) ?  classDic[classid].Short + "," :"");
                        }
                        tkblesson.ClassIDs= tkblesson.ClassIDs.TrimEnd(',');                        
                    }
                    tkblesson.TeacherIDs = "";
                    if (teacherids != "")
                    {
                        foreach (string teacherid in teacherids.Split(','))
                        {
                            
                            tkblesson.TeacherIDs += (teacherDic.ContainsKey(teacherid)? teacherDic[teacherid].Short + "," : "");
                        }
                        tkblesson.TeacherIDs = tkblesson.TeacherIDs.TrimEnd(',');
                    }
                    tkblesson.Save();
                 
                    fileStreamlog.WriteLine(String.Format("Create tkblesson: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", tkblesson.ID + "-" + tkblesson.SubjectID, DateTime.Now));

                }
                else
                {
                    tkblesson.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    tkblesson.ID = sid;
                    tkblesson.PeriodsPerCard = periodspercard;
                    tkblesson.PeriodsPerWeek = periodsperweek;
                    tkblesson.ClassroomIDs = classroomids;
                    tkblesson.GroupIDs = groupids;
                    tkblesson.StudentIDs = studentcoustudentidsnt;
                    tkblesson.Week = weeks;
                    tkblesson.Note = this.Note;
                    tkblesson.SubjectID = (subjectDic.ContainsKey(subjectid) ? subjectDic[subjectid].SubjectCode : "");
                    tkblesson.ClassIDs = "";
                    tkblesson.ClassIDs = "";
                    if (classids != "")
                    {
                        foreach (string classid in classids.Split(','))
                        {
                            tkblesson.ClassIDs += (classDic.ContainsKey(classid) ? classDic[classid].Short + "," : "");
                        }
                        tkblesson.ClassIDs = tkblesson.ClassIDs.TrimEnd(',');
                    }
                    tkblesson.TeacherIDs = "";
                    if (teacherids != "")
                    {
                        foreach (string teacherid in teacherids.Split(','))
                        {

                            tkblesson.TeacherIDs += (teacherDic.ContainsKey(teacherid) ? teacherDic[teacherid].Short + "," : "");
                        }
                        tkblesson.TeacherIDs = tkblesson.TeacherIDs.TrimEnd(',');
                    }
                    tkblesson.Save();

                    fileStreamlog.WriteLine(String.Format("Update tkblesson: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", tkblesson.ID, DateTime.Now));
                }
               

                Lesson lesson = new Lesson(uow)
                {
                    CanRegister =false, 
                    TkbLesson = tkblesson,
                    Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid)),
                    Subject = (subjectDic.ContainsKey(subjectid) ? subjectDic[subjectid] : null),
                    LessonNote = Note,
                    ClassIDs = tkblesson.ClassIDs,
                    LessonCode = ++nextcode //tăng dần 
                };

                lesson.Save();
                if (lesson.Subject!=null)
                    fileStreamlog.WriteLine(String.Format("Create Lesson: \"{0}\" successfully on {1: dd-mm-yyyy HH:MM:ss}", lesson.Subject.SubjectCode + lesson.TkbLesson.Name, DateTime.Now));
                else
                    fileStreamlog.WriteLine(String.Format("WARNING: Create Lesson: \"{0}\" without Lesson \"{1}\" successfully on {2: dd-mm-yyyy HH:MM:ss}", lesson.TkbLesson.Name,tkblesson.SubjectID, DateTime.Now));

                lessonDic.Add(sid, lesson);

            }

            fileStreamlog.WriteLine(String.Format("Create {0} lesson successfully on {1: dd-mm-yyyy HH:MM:ss}", lessonDic.Count, DateTime.Now));

        }

        private void ImportSubjects(Session uow,XElement xElement, Dictionary<string,Subject> subjectdic, StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                
                string sid = "", sname = "", sshort = "";
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            sid = column.Value;
                            break;
                        case "name":
                            sname = column.Value;
                            break;
                        case "short":
                            sshort = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbSubject subject = uow.FindObject<TkbSubject>(new BinaryOperator("ID", sid));
                if (subject == null)
                {
                    subject = new TkbSubject(uow)
                    {
                        ID = sid,
                        Name = sname,
                        Short = sshort,
                        Note = this.Note,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };
                    subject.Save();
                    
                    fileStreamlog.WriteLine(String.Format("Create TkbSubject: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", subject.Name, DateTime.Now));
                }
                else
                {
                    subject.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    subject.Name = sname;
                    subject.Short = sshort;
                    subject.Note = this.Note;
                    subject.Save();
                    fileStreamlog.WriteLine(String.Format("Update TkbSubject: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", subject.Name, DateTime.Now));
                }


                Subject subj = uow.FindObject<Subject>(new BinaryOperator("SubjectCode", sshort));
                if (subj == null)
                {
                    subj = new Subject(uow) { SubjectCode = sshort, SubjectName = sname, Note = this.Note };
                    subj.Save();

                    fileStreamlog.WriteLine(String.Format("Create Subject: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", subj.SubjectName, DateTime.Now));
                }
                else
                {
                    subj.SubjectName = sname;                    
                    subj.Note = this.Note;
                    subject.Save();
                    fileStreamlog.WriteLine(String.Format("Update Subject: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", subj.SubjectName, DateTime.Now));
                }
                subjectdic.Add(sid, subj);
            }
            fileStreamlog.WriteLine(String.Format("Create {0} subjects successfully on {1: dd-mm-yyyy HH:MM:ss}", subjectdic.Count, DateTime.Now));

        }

    
        private void ImportTeachers(UnitOfWork uow, XElement xElement, Dictionary<string,TkbTeacher> teacherDic , StreamWriter fileStreamlog)
        {
            
            foreach (XElement row in xElement.Elements())
            {

                string tid = "", tname = "", tshort = "", tcolor = "";
                bool tgender = false;
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            tid = column.Value;
                            break;
                        case "name":
                            tname = column.Value;
                            break;
                        case "short":
                            tshort = column.Value;
                            break;
                        case "gender":
                            tgender = (column.Value == "M") ? true : false;
                            break;
                        case "color":
                            tcolor = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbTeacher teacher = uow.FindObject<TkbTeacher>(new BinaryOperator("ID", tid));
                if (teacher == null)
                {
                    teacher = new TkbTeacher(uow)
                    {
                        ID = tid,
                        Name = tname,
                        Gender = tgender,
                        Color = tcolor,
                        Short = tshort,
                        Note = this.Note,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };

                    teacher.Save();
                  
                    fileStreamlog.WriteLine(String.Format("Create teacher: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", teacher.Name, DateTime.Now));
                }
                else
                {
                    teacher.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    teacher.ID = tid;
                    teacher.Name = tname;
                    teacher.Gender = tgender;
                    teacher.Color = tcolor;
                    teacher.Short = tshort;
                    teacher.Note = this.Note;
                    teacher.Save();
                    fileStreamlog.WriteLine(String.Format("Update teacher: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", teacher.Name, DateTime.Now));
                }
                teacherDic.Add(tid,teacher);
            }
            fileStreamlog.WriteLine(String.Format("Create {0} teachers successfully on {1: dd-mm-yyyy HH:MM:ss}", teacherDic.Count, DateTime.Now));

        }

        private void ImportClassrooms(Session  uow, XElement xElement, Dictionary<string,Classroom> roomDic, StreamWriter fileStreamlog)
        {

            foreach (XElement row in xElement.Elements())
            {

                string sid = "", cname = "", cshort = "";
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            sid = column.Value;
                            break;
                        case "name":
                            cname = column.Value;
                            break;
                        case "short":
                            cshort = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbClassroom tkbclassroom = uow.FindObject<TkbClassroom>(new BinaryOperator("ID", sid));
                if (tkbclassroom == null)
                {
                    tkbclassroom = new TkbClassroom(uow)
                    {
                        Name = cname,
                        Short = cshort,
                        ID = sid,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };
                    tkbclassroom.Save();                   
                    fileStreamlog.WriteLine(String.Format("Create tkbclassroom: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", tkbclassroom.ID, DateTime.Now));
                }
                else
                {
                    tkbclassroom.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    tkbclassroom.Name = cname;
                    tkbclassroom.Short = cshort;
                    fileStreamlog.WriteLine(String.Format("Update tkbclassroom: \"{0}\" on database - {1: dd-mm-yyyy HH:MM:ss}", tkbclassroom.ID, DateTime.Now));
                    tkbclassroom.Save();
                }               

                string[] scname = cname.Split('_');
                Office office = uow.FindObject<Office>(new BinaryOperator("OfficeCode", scname[0]));
                if (office == null)
                {
                    office = new Office(uow) { OfficeCode = scname[0], Name = scname[0] };
                    fileStreamlog.WriteLine(String.Format("Create office: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", office.Name, DateTime.Now));
                    office.Save();
                }

                Classroom csroom = uow.FindObject<Classroom>(new BinaryOperator("ClassroomCode", cname));
                if (csroom == null)
                {
                    csroom = new Classroom(uow) { ClassroomCode = cname, ClassroomName = cname, Office = office };
                    fileStreamlog.WriteLine(String.Format("Create classroom: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", csroom.ClassroomCode, DateTime.Now));
                    csroom.Save();
                }
                roomDic.Add(sid, csroom);
            }
            fileStreamlog.WriteLine(String.Format("Create {0} classroom successfully on {1: dd-mm-yyyy HH:MM:ss}", roomDic.Count, DateTime.Now));

        }

        private void ImportPeriods(UnitOfWork uow, XElement xElement, Dictionary<string,TkbPeriod> periodDic, StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                String starttime = "", endtime = "";
                int iname = 0;
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "period":
                            iname = int.Parse(column.Value);
                            break;
                        case "starttime":
                            starttime = column.Value;
                            break;
                        case "endtime":
                            endtime = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbPeriod period = uow.FindObject<TkbPeriod>(new BinaryOperator("Name", iname));
                if (period == null)
                {
                    period = new TkbPeriod(uow) { Name = iname, StartTime = starttime, EndTime = endtime, Note = this.Note };
                    period.Save();
                    fileStreamlog.WriteLine(String.Format("Create period: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", period.Name, DateTime.Now));
                }
                else
                {
                    period.Name = iname;
                    period.StartTime = starttime;
                    period.EndTime = endtime;
                    period.Note = this.Note;
                    period.Save();
                    fileStreamlog.WriteLine(String.Format("Update Period: \"{0}\" successfully on database - {1: dd-mm-yyyy HH:MM:ss}", period.Name, DateTime.Now));
                }
                periodDic.Add(iname.ToString(), period);
            }
        }

        private void ImportDays(UnitOfWork uow, XElement xElement,  Dictionary<string,TkbDay> dayDic,StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                string dname = "", dshort = "";
                int dday = 0;
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "name":
                            dname = column.Value;
                            break;
                        case "short":
                            dshort = column.Value;
                            break;
                        case "day":
                            dday = int.Parse(column.Value);
                            break;
                        default:
                            break;
                    }
                }
                TkbDay day = uow.FindObject<TkbDay>(new BinaryOperator("Day", dday));
                if (day == null)
                {
                    day = new TkbDay(uow) { Day = dday, Short = dshort, Name = dname, Note = this.Note };
                    day.Save();
                    fileStreamlog.WriteLine(String.Format("Create day: \"{0}\" successful on {1: dd-mm-yyyy HH:MM:ss}", day.Name, DateTime.Now));
                }
                else
                {
                    day.Day = dday;
                    day.Short = dshort;
                    day.Name = dname;
                    day.Note = this.Note;
                    day.Save();
                    fileStreamlog.WriteLine(String.Format("Update Day: \"{0}\" successfully on database - {1: dd-mm-yyyy HH:MM:ss}", day.Name, DateTime.Now));
                }
                dayDic.Add(dday.ToString(), day);
            }
        }

        private void ImportGroup(UnitOfWork uow, XElement xElement,Dictionary<string,TkbGroup > groupDic, StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                string grid = "", name = "", classid = "";
                bool entireclass = true;
                int divisiontag = 0, studentcount = 0;
                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            grid = column.Value;
                            break;
                        case "name":
                            name = column.Value;
                            break;
                        case "classid":
                            classid = column.Value;
                            break;
                        case "entireclass":
                            entireclass = column.Value == "0" ? false : true;
                            break;
                        case "divisiontag":
                            divisiontag = int.Parse(column.Value);
                            break;
                        case "studentcount":
                            int temp;
                            studentcount = int.TryParse(column.Value, out temp) ? temp : 0;
                            break;
                        default:
                            break;
                    }
                }

                TkbGroup tkbGroup = uow.FindObject<TkbGroup>(new BinaryOperator("ID", grid));
                if (tkbGroup == null)
                {
                    tkbGroup = new TkbGroup(uow)
                    {
                        ID = grid,
                        Name = name,
                        Classid = classid,
                        DivisionTag = divisiontag,
                        EntireClass = entireclass,
                        StudentCount = studentcount,
                        Note = this.Note,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };
                    tkbGroup.Save();
                    fileStreamlog.WriteLine(string.Format("Create new tkbGroup : \"{0}\" successfully on {1:dd-mm-yy HH:MM:ss}", name, DateTime.Now));
                }
                else
                {
                    tkbGroup.ID = grid;
                    tkbGroup.Name = name;
                    tkbGroup.StudentCount = studentcount;
                    tkbGroup.Classid = classid;
                    tkbGroup.EntireClass = entireclass;
                    tkbGroup.DivisionTag = divisiontag;
                    tkbGroup.Note = this.Note;
                    tkbGroup.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    tkbGroup.Save();
                    fileStreamlog.WriteLine(String.Format("Update tkbGroup: \"{0}\" successfully on database {1: dd-mm-yyyy HH:MM:ss}", name, DateTime.Now));
                }
                groupDic.Add(grid, tkbGroup);
            }
        }

        private void ImportClass(UnitOfWork uow, XElement xElement, Dictionary<string,TkbClass> classDic, StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                string clid = "", clname = "", clshort = "", clteacherid = "", clclassroom = "", clgrade = "";

                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            clid = column.Value;
                            break;
                        case "name":
                            clname = column.Value;
                            break;
                        case "short":
                            clshort = column.Value;
                            break;
                        case "teacherid":
                            clteacherid = column.Value;
                            break;
                        case "classroomids":
                            clclassroom = column.Value;
                            break;
                        case "grade":
                            clgrade = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbClass tkbclass = uow.FindObject<TkbClass>(new BinaryOperator("ID", clid));
                if (tkbclass == null)
                {
                    tkbclass = new TkbClass(uow) { 
                        ID = clid, Name = clname, Short = clshort, 
                        Grade = clgrade, ClassroomIDs = clclassroom, 
                        TeacherIDs = clteacherid, Note = this.Note ,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };
                    tkbclass.Save();
                
                    fileStreamlog.WriteLine(string.Format("Create new tkbclass: \"{0}\" successfully on {1:dd-mm-yy HH:MM:ss}", clname, DateTime.Now));
                }
                else
                {
                    tkbclass.ID = clid;
                    tkbclass.Name = clname;
                    tkbclass.Short = clshort;
                    tkbclass.ClassroomIDs = clclassroom;
                    tkbclass.TeacherIDs = clteacherid;
                    tkbclass.Grade = clgrade;
                    tkbclass.Note = this.Note;
                    tkbclass.Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid));
                    tkbclass.Save();

                    fileStreamlog.WriteLine(String.Format("Update class: \"{0}\" successfully on database {1: dd-mm-yyyy HH:MM:ss}", clname, DateTime.Now));
                }
                classDic.Add(clid, tkbclass);
            }
        }

        private void ImportGrade(UnitOfWork uow, XElement xElement, Dictionary<string,TkbGrade> gradeDic, StreamWriter fileStreamlog)
        {
            foreach (XElement row in xElement.Elements())
            {
                string gid = "", gname = "", gshort = "", ggrade = "";

                foreach (XAttribute column in row.Attributes())
                {
                    switch (column.Name.LocalName)
                    {
                        case "id":
                            gid = column.Value;
                            break;
                        case "name":
                            gname = column.Value;
                            break;
                        case "short":
                            gshort = column.Value;
                            break;
                        case "grade":
                            ggrade = column.Value;
                            break;
                        default:
                            break;
                    }
                }
                TkbGrade grade = uow.FindObject<TkbGrade>(new BinaryOperator("ID", gid));
                if (grade == null)
                {
                    grade = new TkbGrade(uow)
                    {
                        ID = gid,
                        Name = gname,
                        Short = gshort,
                        Grade = ggrade,
                        Note = this.Note,
                        Semester = uow.FindObject<Semester>(new BinaryOperator("Oid", this.Semester.Oid))
                    };
                    grade.Save();
                    fileStreamlog.WriteLine(string.Format("Create new grade: \"{0}\" on {1:dd-mm-yy HH:MM:ss}", ggrade, DateTime.Now));
                }
                else
                {
                    fileStreamlog.WriteLine(String.Format("Grade: \"{0}\" had defined on database {1: dd-mm-yyyy HH:MM:ss}", grade.Name, DateTime.Now));
                }
                gradeDic.Add(gid, grade);
            }
        }
    }
}

/*
    Đã đổi Card.LessonID thành kiểu string (thay cho kiểu int)         
*/
