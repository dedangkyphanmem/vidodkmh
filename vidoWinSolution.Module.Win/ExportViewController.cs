

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using vidoSolution.Module.Utilities;
using DevExpress.Persistent.BaseImpl;
using vidoSolution.Module.DomainObject;
using System.IO;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.Windows.Forms;
using DevExpress.Persistent.Validation;

namespace vidoSolution.Module.Win
{
   
    public partial class ExportViewController : ViewController
    {
        private List<Vacancy> listVacancies;
        private List<string> listStudentCode;
        public ExportViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ExportRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            GridListEditor listEditor = ((DevExpress.ExpressApp.ListView)View).Editor as GridListEditor;
            Type TypeObject = View.ObjectTypeInfo.Type;
            string strObj="";
            foreach (BaseObject obj in View.SelectedObjects)
            {
                strObj = (strObj =="" ? string.Format("Oid='{0}'", obj.Oid) : string.Format("{0} or Oid='{1}'", strObj, obj.Oid));
            }
            if (strObj != "")
            {
                if (listEditor != null)
                {
                    GridView gv = (listEditor.Grid as GridControl).MainView as GridView;
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "xls files (*.xls)|*.xls|All files (*.*)|*.*";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            XlsExportOptions xlsExportOptions = new DevExpress.XtraPrinting.XlsExportOptions();
                            xlsExportOptions.SheetName = View.Caption.Replace(" ", "");
                            xlsExportOptions.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                            xlsExportOptions.ShowGridLines = true;
                            gv.GridControl.DataSource = new XPCollection(View.ObjectSpace.Session, TypeObject,
                              CriteriaOperator.Parse(strObj));
                            
                            gv.ExportToXls(sfd.FileName, xlsExportOptions);
                        }
                    }


                }
            }
        }

        private void ExportViewController_Activated(object sender, EventArgs e)
        {
            bool showRegister = (SecuritySystem.CurrentUser is Student) && ((DevExpress.ExpressApp.ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail);

            SelectRegister.Active.SetItemValue(
               "ObjectType", showRegister);
            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles,
              new BinaryOperator("Name", "DataAdmins"));
            if (xpc.Count + xpc2.Count > 0)
            {
                showRegister = showRegister || ((DevExpress.ExpressApp.ListView)View).ObjectTypeInfo.Type == typeof(Student);
                SelectRegister.Active.SetItemValue(
                 "ObjectType", showRegister);
            }


            ExportRegister.Active.SetItemValue(
                         "ObjectType", true);
        }




        void Editor_ControlsCreated(object sender, EventArgs e)
        {
            GridListEditor gc = sender as GridListEditor;
            GridControl gridView = ((GridControl)gc.Grid);
            // gridView.Settings.ShowFilterRow = false;
            //gridView.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            ////gridView.Settings.ShowTitlePanel = true;
            //gridView.DataBound += gridView_DataBound;
            //DevExpress.Data.SelectionChangedEventHandler gridView_SelectionChanged = null;
            //gridView.SelectionChanged += gridView_SelectionChanged;
            //gridView.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;
            //gridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
        }

        private void SelectRegister_Execute(object sender, SimpleActionExecuteEventArgs args)
        {
            ObjectSpace os = Application.CreateObjectSpace();


            if (SecuritySystem.CurrentUser is Student)
            {
                #region student
                ObjectSpace objectSpace = Application.CreateObjectSpace();
                CriteriaOperatorCollection c = new CriteriaOperatorCollection();

                CollectionSource newCollectionSource = new CollectionSource(objectSpace, typeof(Lesson));
                listVacancies = new List<Vacancy>();
                foreach (RegisterDetail regDetail in ((ProxyCollection)((DevExpress.ExpressApp.ListView)View).CollectionSource.Collection))
                {
                    foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
                    {
                        listVacancies.Add(new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, tkbsem.Classroom.ClassroomCode));
                    }
                    //newCollectionSource.Criteria[regDetail.Lesson.Oid.ToString()] =
                    //    new BinaryOperator("Oid", regDetail.Lesson.Oid, BinaryOperatorType.NotEqual);
                }

                using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                {
                    Vacancy vc;
                    foreach (Lesson lesson in xpLesson)
                    {
                        if ((!lesson.CanRegister) || (lesson.NumRegistration >= lesson.NumExpectation))
                        {
                            //quá sĩ số
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

                DevExpress.ExpressApp.ListView lv = Application.CreateListView(
                    Application.FindListViewId(typeof(Lesson)),
                    newCollectionSource, true);
                lv.Editor.AllowEdit = false;
                lv.Editor.ControlsCreated += Editor_ControlsCreated;

                args.ShowViewParameters.CreatedView = lv;
                args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                args.ShowViewParameters.CreateAllControllers = true;
                DialogController selectAcception = new DialogController();
                args.ShowViewParameters.Controllers.Add(selectAcception);
                selectAcception.Accepting += selectAcception_AcceptingStudent;
                #endregion
            }
            else
            {
                ObjectSpace objectSpace = Application.CreateObjectSpace();
                CriteriaOperatorCollection c = new CriteriaOperatorCollection();
                listStudentCode = new List<string>();
                CollectionSource newCollectionSource = new CollectionSource(objectSpace, typeof(Lesson));
                listVacancies = new List<Vacancy>();
                foreach (Student student in View.SelectedObjects)
                {
                    foreach (RegisterDetail regDetail in student.RegisterDetails)
                    {
                        foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
                        {
                            listVacancies.Add(new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, tkbsem.Classroom.ClassroomCode));
                        }
                        //newCollectionSource.Criteria[regDetail.Lesson.Oid.ToString()] =
                        //    new BinaryOperator("Oid", regDetail.Lesson.Oid, BinaryOperatorType.NotEqual);
                    }
                    listStudentCode.Add(student.StudentCode);
                }
                using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                {
                    Vacancy vc;
                    foreach (Lesson lesson in xpLesson)
                    {
                        if ((!lesson.CanRegister) || (lesson.NumRegistration >= lesson.NumExpectation))
                        {
                            //quá sĩ số
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

                DevExpress.ExpressApp.ListView lv = Application.CreateListView(
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
        }

        //void gridView_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (SecuritySystem.CurrentUser is Student)
        //    {
        //        Student student = SecuritySystem.CurrentUser as Student;
        //        string currentClass = student.StudentClass.ClassCode;

        //        ASPxGridView gc = sender as ASPxGridView;

        //        List<Vacancy> tempList = new List<Vacancy>(listVacancies);

        //        List<object> listobj = gc.GetSelectedFieldValues(new string[] { "LessonCode" });
        //        ObjectSpace objectSpace = Application.CreateObjectSpace();

        //        Vacancy vc;
        //        string strFilter = "", strInclude = "";
        //        foreach (int strLessonCode in listobj)
        //        {
        //            Lesson lesson = objectSpace.FindObject<Lesson>(new BinaryOperator("LessonCode", strLessonCode));
        //            strInclude += String.Format("OR ([LessonCode]={0})", lesson.LessonCode);
        //            foreach (TkbSemester tkbsem in lesson.TKBSemesters)
        //            {
        //                vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
        //                tempList.Add(vc);
        //            }
        //        }
        //        if (strInclude != "")
        //            strInclude = String.Format("({0})", strInclude.Substring(3));

        //        if (listobj.Count > 0)
        //        {
        //            using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
        //            {

        //                foreach (Lesson lesson in xpLesson)
        //                {
        //                    foreach (TkbSemester tkbsem in lesson.TKBSemesters)
        //                    {
        //                        vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
        //                        if (Utils.IsConfictTKB(tempList, vc))
        //                        {
        //                            strFilter += String.Format("AND ([LessonCode]<>{0})", lesson.LessonCode);
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (strFilter != "")
        //                    strFilter = String.Format("({0})", strFilter.Substring(4));

        //            }
        //        }
        //        if (strInclude != "" && strFilter != "")
        //        {
        //            strFilter = String.Format("({0} OR {1})", strFilter, strInclude);
        //        }
        //        else if (strInclude != "")
        //        {
        //            strFilter = strInclude;

        //        }

        //        if (strFilter != "")
        //        {
        //            if (gc.FilterExpression.Contains("TkbLesson.ClassIDs"))
        //            {
        //                gc.FilterExpression = string.Format("([TkbLesson.ClassIDs] Like '%{0}%') AND {1}", currentClass, strFilter);
        //            }
        //            else
        //            {
        //                gc.FilterExpression = strFilter;
        //            }
        //        }

        //    }
        //}

        //void gridView_DataBound(object sender, EventArgs e)
        //{
        //    if (SecuritySystem.CurrentUser is Student)
        //    {
        //        Student student = SecuritySystem.CurrentUser as Student;
        //        string currentClass = student.StudentClass.ClassCode;
        //        ASPxGridView gc = sender as ASPxGridView;
        //        if (gc.FilterExpression == "")
        //            gc.FilterExpression = string.Format("[TkbLesson.ClassIDs] Like '%{0}%'", currentClass);
        //    }
        //}

        void selectAcception_AcceptingStudent(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (SecuritySystem.CurrentUser is Student)
            {
                ObjectSpace objectSpace = Application.CreateObjectSpace();
                DevExpress.ExpressApp.ListView lv = ((DevExpress.ExpressApp.ListView)((WindowController)sender).Window.View);
                if (SecuritySystem.CurrentUser is Student)
                {
                    objectSpace.Session.BeginTransaction();
                    Student student = SecuritySystem.CurrentUser as Student;
                    Student currentStudent = objectSpace.FindObject<Student>(
                        new BinaryOperator("StudentCode", student.StudentCode));
                    Lesson curLesson;
                    foreach (Lesson lesson in lv.SelectedObjects)
                    {
                        curLesson = objectSpace.FindObject<Lesson>(
                        new BinaryOperator("Oid", lesson.Oid));
                        RegisterDetail regdetail = new RegisterDetail(objectSpace.Session)
                        {
                            Student = currentStudent,
                            Lesson = curLesson,
                            RegisterState =  objectSpace.FindObject<RegisterState>(
                                new BinaryOperator ("Code","SELECTED")),
                            CheckState = objectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "NOTCHECKED"))
                        };


                    }
                    objectSpace.Session.CommitTransaction();
                    //               View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                }
            }
        }
        void selectAcception_AcceptingAdmin(object sender, DialogControllerAcceptingEventArgs e)
        {

            ObjectSpace objectSpace = Application.CreateObjectSpace();
            DevExpress.ExpressApp.ListView lv = ((DevExpress.ExpressApp.ListView)((WindowController)sender).Window.View);
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
                foreach (string studentCode in listStudentCode)
                {
                    currentStudent = objectSpace.FindObject<Student>(
                    new BinaryOperator("StudentCode", studentCode));
                    foreach (Lesson lesson in lv.SelectedObjects)
                    {
                        curLesson = objectSpace.FindObject<Lesson>(
                        new BinaryOperator("Oid", lesson.Oid));
                        RegisterDetail regdetail = new RegisterDetail(objectSpace.Session)
                        {
                            Student = currentStudent,
                            Lesson = curLesson,
                            RegisterState = objectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "SELECTED")),
                            CheckState =objectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "NOTCHECKED"))
                        };
                        RuleSet ruleSet = new RuleSet();

                        RuleSetValidationResult result = ruleSet.ValidateTarget(regdetail, DefaultContexts.Save);
                        if (ValidationState.Invalid ==
                            result.GetResultItem("RegisterDetail.StudentRegLessonSemester").State)
                        {
                            regdetail.Delete();                           
                        }
                        else
                        {
                            regdetail.Save();
                        }

                    }
                }
                objectSpace.Session.CommitTransaction();

                PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
                ms.Title = "Lỗi đăng ký";
                ms.Message = string.Format("Error");
                ShowViewParameters svp = new ShowViewParameters();
                svp.CreatedView = Application.CreateDetailView(
                     objectSpace, ms);
                svp.TargetWindow = TargetWindow.NewModalWindow;
                svp.CreatedView.Caption = "Thông báo";
                DialogController  dc = Application.CreateController<DialogController>();
                svp.Controllers.Add(dc);

                dc.SaveOnAccept = false;
                Application.ShowViewStrategy.ShowView(svp,new ShowViewSource(null,null));
                ////               View.ObjectSpace.CommitChanges();
                //View.ObjectSpace.Refresh();
                //ListView view = null;
                //Frame currentFrame = ((ActionBase)sender).Controller.Frame;
                //switch (currentFrame.View.ObjectTypeInfo.Name)
                //{
                //    case "Student":
                //        view = Application.CreateListView(objectSpace,typeof(RegisterDetail),true);
                //        break;                    
                //}
                //currentFrame.SetView(view);
                //e.Cancel = true;
            }
        }
    }
}
