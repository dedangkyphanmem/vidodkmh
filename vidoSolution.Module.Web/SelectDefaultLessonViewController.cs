using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using System.Web;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.Persistent.BaseImpl;
using vidoSolution.Module.DomainObject;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using vidoSolution.Module.Utilities;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.Reports.Web;
using DevExpress.Persistent.Validation;

namespace vidoSolution.Module.Web
{
    public partial class SelectDefaultLessonViewController : ViewController
    {
        private List<Vacancy> listVacancies;

        //private List<string> listStudentCode;
        private Dictionary<string, List<string>> dicStudentRegDetail;

        public SelectDefaultLessonViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        
        private void ExportViewController_Activated(object sender, EventArgs e)
        {
            SelectDefaultLesson.Active.SetItemValue(
               "ObjectType", false);
            //User u = (User)SecuritySystem.CurrentUser;
            //using (XPCollection<Role> xpc = new XPCollection<Role>(u.Roles, new BinaryOperator("Name", "Administrators")))
            //{
            //    using (XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles, new BinaryOperator("Name", "DataAdmins")))
            //    {
            //        if (xpc.Count + xpc2.Count > 0)
            //        {
            //            SelectDefaultLesson.Active.SetItemValue("ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(StudentClass));
            //        }
            //    }
            //}
        }




        void Editor_ControlsCreated(object sender, EventArgs e)
        {
            ASPxGridListEditor gc = sender as ASPxGridListEditor;
            ASPxGridView gridView = (ASPxGridView)gc.Grid;
            // gridView.Settings.ShowFilterRow = false;
            gridView.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            //gridView.Settings.ShowTitlePanel = true;
            gridView.DataBound += gridView_DataBound;
            gridView.SelectionChanged += gridView_SelectionChanged;
            gridView.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;
            gridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
        }

        private void SelectRegister_Execute(object sender, SimpleActionExecuteEventArgs args)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ConstrainstParameter cpNHHK = objectSpace.FindObject<ConstrainstParameter>(
                           new BinaryOperator("Code", "REGISTERSEMESTER"));
            if (cpNHHK == null || cpNHHK.Value == 0 || cpNHHK.Value.ToString().Length!=5)
                throw new UserFriendlyException("Người Quản trị chưa thiết lập NHHK để ĐKMH, vui lòng liên hệ quản trị viên.");

            CriteriaOperatorCollection c = new CriteriaOperatorCollection();

            dicStudentRegDetail = new Dictionary<string, List<string>>();

            CollectionSource newCollectionSource = new CollectionSource(objectSpace, typeof(Lesson));
            listVacancies = new List<Vacancy>();
            foreach (StudentClass studentClass in View.SelectedObjects)
            {
                foreach (Student student in studentClass.Students)
                {
                    if (!dicStudentRegDetail.ContainsKey(student.StudentCode))
                        dicStudentRegDetail.Add(student.StudentCode, new List<string>());

                    foreach (RegisterDetail regDetail in student.RegisterDetails)
                    {
                        foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
                        {   
                            listVacancies.Add( new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode)));
                        }
                        dicStudentRegDetail[student.StudentCode].Add(regDetail.Lesson.Subject.SubjectCode);

                        newCollectionSource.Criteria[regDetail.Lesson.Subject.SubjectCode] =
                            new BinaryOperator("Subject.SubjectCode", regDetail.Lesson.Subject.SubjectCode, BinaryOperatorType.NotEqual);
                    }
                    //listStudentCode.Add(student.StudentCode);
                }
            }

            using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
            {
                Vacancy vc;
                foreach (Lesson lesson in xpLesson)
                {
                    if ((Convert.ToInt32(lesson.Semester.SemesterName) <= cpNHHK.Value) || (lesson.NumRegistration >= lesson.NumExpectation))
                    {
                        //không đăng ký, quá sĩ số, khác nhhk 
                        newCollectionSource.Criteria[lesson.Oid.ToString()] = new BinaryOperator("Oid", lesson.Oid, BinaryOperatorType.NotEqual);

                    }
                    //vi phạm thời khóa biểu
                    foreach (TkbSemester tkbsem in lesson.TKBSemesters)
                    {
                        vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                        if (Utils.IsConfictTKB(listVacancies, vc))
                        {
                            newCollectionSource.Criteria[lesson.Oid.ToString()] = new BinaryOperator("Oid", lesson.Oid, BinaryOperatorType.NotEqual);
                            break;
                        }
                    }

                }
            }

            ListView lv = Application.CreateListView(
                Application.FindListViewId(typeof(Lesson)),
                newCollectionSource, true);
            lv.Editor.AllowEdit = false;
            lv.Editor.ControlsCreated += Editor_ControlsCreated;

            args.ShowViewParameters.CreatedView = lv;
            args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            args.ShowViewParameters.CreateAllControllers = true;
            DialogController selectAcception = new DialogController();
            args.ShowViewParameters.Controllers.Add(selectAcception);
            selectAcception.Accepting += selectAcception_AcceptingAdmin;
        }
    
        void gridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SecuritySystem.CurrentUser is Student)
            {
                Student student = SecuritySystem.CurrentUser as Student;
                string currentClass = student.StudentClass.ClassCode;

                ASPxGridView gc = sender as ASPxGridView;

                List<Vacancy> tempList = new List<Vacancy>(listVacancies);

                List<object> listobj = gc.GetSelectedFieldValues(new string[] { "LessonCode" });
                ObjectSpace objectSpace = Application.CreateObjectSpace();

                Vacancy vc;
                string strFilter = "", strInclude = "";
                foreach (int strLessonCode in listobj)
                {
                    Lesson lesson = objectSpace.FindObject<Lesson>(new BinaryOperator("LessonCode", strLessonCode));
                    strInclude += String.Format("OR ([LessonCode]={0})", lesson.LessonCode);
                    foreach (TkbSemester tkbsem in lesson.TKBSemesters)
                    {
                        vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                        tempList.Add(vc);
                    }
                }
                if (strInclude != "")
                    strInclude = String.Format("({0})", strInclude.Substring(3));

                if (listobj.Count > 0)
                {
                    using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                    {

                        foreach (Lesson lesson in xpLesson)
                        {
                            foreach (TkbSemester tkbsem in lesson.TKBSemesters)
                            {
                                vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                                if (Utils.IsConfictTKB(tempList, vc))
                                {
                                    strFilter += String.Format("AND ([LessonCode]<>{0})", lesson.LessonCode);
                                    break;
                                }
                            }
                        }
                        if (strFilter != "")
                            strFilter = String.Format("({0})", strFilter.Substring(4));

                    }
                }
                if (strInclude != "" && strFilter != "")
                {
                    strFilter = String.Format("({0} OR {1})", strFilter, strInclude);
                }
                else if (strInclude != "")
                {
                    strFilter = strInclude;

                }

                if (strFilter != "")
                {
                    if (gc.FilterExpression.Contains("TkbLesson.ClassIDs"))
                    {
                        gc.FilterExpression = string.Format("([TkbLesson.ClassIDs] Like '%{0}%') AND {1}", currentClass, strFilter);
                    }
                    else
                    {
                        gc.FilterExpression = strFilter;
                    }
                }

            }
        }

        void gridView_DataBound(object sender, EventArgs e)
        {
            if (SecuritySystem.CurrentUser is Student)
            {
                Student student = SecuritySystem.CurrentUser as Student;
                if (student.StudentClass != null)
                {
                    string currentClass = student.StudentClass.ClassCode;
                    ASPxGridView gc = sender as ASPxGridView;
                    if (gc.FilterExpression == "")
                        gc.FilterExpression = string.Format("[TkbLesson.ClassIDs] Like '%{0}%'", currentClass);
                }
            }
        }

         void selectAcception_AcceptingAdmin(object sender, DialogControllerAcceptingEventArgs e)
        {
            Dictionary<string, int> dicLessonCurrentRegNum = new Dictionary<string, int>();
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles,
              new BinaryOperator("Name", "DataAdmins"));
            if (xpc.Count + xpc2.Count > 0)
            {
            
                objectSpace.Session.BeginTransaction();
               
                Student currentStudent; 
                Lesson curLesson;
                Dictionary<string, List<string>> errorstudent = new Dictionary<string, List<string>>();
                int numregok = 0;
                foreach (string studentCode in dicStudentRegDetail.Keys)
                {
                    currentStudent = objectSpace.FindObject<Student>(
                    new BinaryOperator("StudentCode", studentCode));
                    foreach (Lesson lesson in lv.SelectedObjects)
                    {
                        if (!dicLessonCurrentRegNum.ContainsKey(lesson.LessonName))
                        {
                            dicLessonCurrentRegNum[lesson.LessonName] = 0;
                        }
                        //si so chon chua vuot qua
                        if (lesson.NumExpectation > dicLessonCurrentRegNum[lesson.LessonName] + lesson.NumRegistration)
                        {
                            curLesson = objectSpace.FindObject<Lesson>(
                            new BinaryOperator("Oid", lesson.Oid));
                            RegisterDetail regdetail = new RegisterDetail(objectSpace.Session)
                            {
                                Student = currentStudent,
                                Lesson = curLesson,
                                RegisterState = objectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "SELECTED")),
                                CheckState = objectSpace.FindObject<RegisterState>(
                                    new BinaryOperator("Code", "NOTCHECKED"))
                            };
                            RuleSet ruleSet = new RuleSet();

                            RuleSetValidationResult result = ruleSet.ValidateTarget(regdetail, DefaultContexts.Save);
                            if (ValidationState.Invalid ==
                                result.GetResultItem("RegisterDetail.StudentRegLessonSemester").State)
                            {
                                if (!errorstudent.ContainsKey(currentStudent.StudentCode))
                                    errorstudent.Add(currentStudent.StudentCode, new List<string>());
                                if (!errorstudent[currentStudent.StudentCode].Contains(curLesson.Subject.SubjectCode))
                                    errorstudent[currentStudent.StudentCode].Add(curLesson.Subject.SubjectCode);
                                regdetail.Delete();
                            }
                            else
                            {
                                numregok++;
                                dicLessonCurrentRegNum[lesson.LessonName]++;
                                regdetail.Save();
                            }
                        }
                        else
                        {
                            if (!errorstudent.ContainsKey(currentStudent.StudentCode))
                                errorstudent.Add(currentStudent.StudentCode, new List<string>());
                            if (!errorstudent[currentStudent.StudentCode].Contains(lesson.Subject.SubjectCode))
                                errorstudent[currentStudent.StudentCode].Add(lesson.Subject.SubjectCode);
                        }

                    }
                }
                objectSpace.Session.CommitTransaction();
                PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
                if (errorstudent.Count > 0)
                {
                    ms.Title = "Có lỗi khi chọn nhóm MH đăng ký!";
                    ms.Message = string.Format("Đã chọn được cho {0} sinh viên với {1} lượt nhóm MH\r\n", dicStudentRegDetail.Count, numregok);
                    string strmessage = "Không chọn được nhóm môn học do trùng môn đã đăng ký hoặc hết chỗ: ";
                    foreach (KeyValuePair<string, List<string>> keypair in errorstudent)
                    {
                        strmessage += string.Format("Sinh viên:[{0}] - Môn:[",keypair.Key);
                        foreach (string str in keypair.Value)
                        {
                            strmessage+= str +",";
                        }
                        strmessage= strmessage.TrimEnd(',');
                        strmessage += "]\r\n";
                    }
                    ms.Message += strmessage;
                }
                else
                {
                    ms.Title = "Chọn nhóm MH thành công";
                    ms.Message = string.Format("Chọn nhóm MH thành công cho {0} sinh viên với {1} lượt nhóm MH\r\n", dicStudentRegDetail.Count, numregok);
                }
                ShowViewParameters svp = new ShowViewParameters();
                svp.CreatedView = Application.CreateDetailView(
                     objectSpace, ms);
                svp.TargetWindow = TargetWindow.NewModalWindow;
                svp.CreatedView.Caption = "Thông báo";
                DialogController dc = Application.CreateController<DialogController>();
                svp.Controllers.Add(dc);

                dc.SaveOnAccept = false;
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }
     
    }
}
