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
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.ExpressApp.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.ExpressApp.Web.Editors.Standard;
using DevExpress.ExpressApp.Web.SystemModule;
using System.Collections;

namespace vidoSolution.Module.Web
{
    public partial class ExportViewController : ViewController
    {
        private List<Vacancy> listVacancies;

        //private List<string> listStudentCode;
        private Dictionary<string, List<string>> dicStudentRegDetail;

        public ExportViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ExportRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            Type TypeObject = View.ObjectTypeInfo.Type;
            string strObj="";
            int countobj = 0;
            
            if (View.ObjectTypeInfo.Type == typeof(ReportData))
            {
                foreach (ReportData obj in View.SelectedObjects)
                {
                    strObj = (strObj == "" ? string.Format("ReportName='{0}'", obj.ReportName) : string.Format("{0} or ReportName='{1}'", strObj, obj.ReportName));
                    countobj++;
                }
            }
            else
            {
                foreach (BaseObject obj in View.SelectedObjects)
                {
                    strObj = (strObj == "" ? string.Format("Oid='{0}'", obj.Oid) : string.Format("{0} or Oid='{1}'", strObj, obj.Oid));
                    countobj++;
                }
            }
            if (countobj > 2000)
                throw new UserFriendlyException(string.Format("Không thể export dữ liệu nhiều hơn 2000 dòng. Bạn đã chọn {0} dòng!!", countobj)); 
            if (strObj != "")
            {
                if (listEditor != null)
                {
                    ASPxGridView gv = listEditor.Grid;
                    using (ASPxGridViewExporter gridViewExporter = new ASPxGridViewExporter())
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            //gridViewExporter.WriteXlsToResponse();
                            gridViewExporter.GridViewID = gv.ID;
                            gridViewExporter.Page = gv.Page;
                            gv.Parent.Controls.Add(gridViewExporter);  
                            gv.BeginUpdate();
                            gv.DataSource = new XPCollection(View.ObjectSpace.Session, TypeObject,
                                CriteriaOperator.Parse(strObj));                          
                            gv.DataBind();
                            gv.EndUpdate();
                            gridViewExporter.DataBind();

                            XlsExportOptions xlsExportOptions = new DevExpress.XtraPrinting.XlsExportOptions();

                            xlsExportOptions.SheetName = View.Caption.Replace(" ", "");

                            xlsExportOptions.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                            xlsExportOptions.ShowGridLines = true;

                            gridViewExporter.WriteXls(stream, xlsExportOptions);
                            byte[] buffer = stream.GetBuffer();
                            string contentType = "application/ms-excel";
                            string contentDisposition = "attachment; filename=Export_Result.xls";
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.Buffer = false;
                            HttpContext.Current.Response.AppendHeader("Content-Type", contentType);
                            HttpContext.Current.Response.AppendHeader("Content-Transfer-Encoding", "binary");
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", contentDisposition);
                            HttpContext.Current.Response.BinaryWrite(buffer);
                            HttpContext.Current.Response.End();
                        }
                    }

                }
            }
        }

        private void ExportViewController_Activated(object sender, EventArgs e)
        {
            if (Frame.GetController<WebDetailViewController>()!=null)
                Frame.GetController<WebDetailViewController>().SaveAndCloseAction.Active.SetItemValue("hideAll", false);
            if (View is DetailView)
            {                
                Frame.GetController<DetailViewController>().SaveAndCloseAction.Active.SetItemValue("hideAll", false);
            }
            if (View is ListView && View.ObjectTypeInfo.Type == typeof(MyImportResult))
            {
                Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("hideAll", false);
            }
            if (View is ListView)
            {
                Frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction.Enabled.SetItemValue("disableAll", false);
                if (View.ObjectTypeInfo.Type == typeof(ReportData))
                    Frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction.Enabled.SetItemValue("disableAll", true);
            }

            bool showRegister = (SecuritySystem.CurrentUser is Student) && ((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail);

            ConfirmRegister.Active.SetItemValue(
              "ObjectType", showRegister);
            SelectRegister.Active.SetItemValue(
               "ObjectType", showRegister);
            BookRegister.Active.SetItemValue(
               "ObjectType", showRegister);

            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles,
              new BinaryOperator("Name", "DataAdmins"));
            if (xpc.Count + xpc2.Count > 0)
            {
                showRegister = showRegister || ((ListView)View).ObjectTypeInfo.Type == typeof(Student);
                SelectRegister.Active.SetItemValue(
                 "ObjectType", showRegister);
                BookRegister.Active.SetItemValue(
                    "ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail));
               
            }


            ExportRegister.Active.SetItemValue(
                         "ObjectType", true);

            View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (listEditor != null)
            {
                ASPxGridView gridControl = (ASPxGridView)listEditor.Grid;
                if (gridControl != null)
                {
                    if (View.ObjectTypeInfo.GetType()==typeof(WeekReportData))
                        new ASPxGridViewCellMerger(gridControl);
                    //gridControl.SettingsText.CommandCancel = "Bỏ qua";
                    //gridControl.SettingsText.CommandClearFilter = "Xóa điều kiện lọc";
                    //gridControl.SettingsText.CommandEdit = "Chỉnh sửa";
                    //gridControl.SettingsText.CommandNew = "Tạo mới";
                    //gridControl.SettingsText.FilterControlPopupCaption = "Tạo điều kiện lọc";
                    //gridControl.SettingsText.EmptyDataRow = "Không có dữ liệu";
                    //gridControl.SettingsText.FilterBarClear = "Xóa điều kiện lọc";
                    //gridControl.SettingsText.FilterBarCreateFilter = "Tạo điều kiện lọc";
                    //gridControl.SettingsText.GroupPanel = "Kéo thả tiêu đề cột vào đây để nhóm lại";
                    //gridControl.SettingsText.HeaderFilterShowAll = "Hiện tất cả";
                    //gridControl.SettingsText.HeaderFilterShowBlanks = "Hiện dòng không có dữ liệu (blank)";
                    //gridControl.SettingsText.HeaderFilterShowNonBlanks = "Hiện dòng có dữ liệu";
                    gridControl.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.NextColumn;
                    gridControl.SettingsBehavior.AutoExpandAllGroups = true;
                    //gridControl.SettingsLoadingPanel.Text = "Đang tải &hellip;";
                    //gridControl.SettingsPager.Summary.Text = "Trang {0}/{1} ({2} dòng dữ liệu)";
                    //gridControl.Settings.ShowStatusBar = GridViewStatusBarMode.Auto;
                    //gridControl.Settings.ShowGroupedColumns = true;
                    gridControl.Settings.ShowFooter = true;
                    gridControl.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleAlways;                    
                    gridControl.Styles.AlternatingRow.Enabled =  DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                    //gridControl.Settings.ShowHorizontalScrollBar = true;
                    
                    foreach (ASPxSummaryItem sumItem in gridControl.TotalSummary)
                    {
                        if (sumItem.SummaryType == DevExpress.Data.SummaryItemType.Count)
                            sumItem.DisplayFormat = "Số lượng = {0:0,0}";
                        if (sumItem.SummaryType == DevExpress.Data.SummaryItemType.Sum)
                            sumItem.DisplayFormat = "Tổng = {0:0,0}";
                        
                    }
                    gridControl.GroupSummary.Clear();
                    foreach (ASPxSummaryItem totalItem in gridControl.TotalSummary)
                    {
                        ASPxSummaryItem groupItem = new ASPxSummaryItem();
                        groupItem.FieldName = totalItem.FieldName;
                        groupItem.DisplayFormat = totalItem.DisplayFormat;
                        groupItem.SummaryType = totalItem.SummaryType;
                        groupItem.ShowInGroupFooterColumn = totalItem.FieldName;
                        gridControl.GroupSummary.Add(groupItem);

                    }
                    
                    GridViewDataColumn hyperLinkColum = gridControl.Columns["HyperLink"] as GridViewDataColumn;
                    
                    if (hyperLinkColum != null)
                    {
                        hyperLinkColum.DataItemTemplate = new HyperLinkTemplate();
                    }
                    hyperLinkColum = gridControl.Columns["Link"] as GridViewDataColumn;
                    if (hyperLinkColum != null)
                    {
                        hyperLinkColum.DataItemTemplate = new HyperLinkTemplate();
                    }
                    hyperLinkColum = gridControl.Columns["ResultLink"] as GridViewDataColumn;
                    if (hyperLinkColum != null)
                    {
                        hyperLinkColum.DataItemTemplate = new HyperLinkTemplate();
                    }
                }
            }
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
            PopUpMessage ms;
            DialogController dc;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            {
                ConstrainstParameter cpNHHK = objectSpace.FindObject<ConstrainstParameter>(
                           new BinaryOperator("Code", "REGISTERSEMESTER"));

                if (cpNHHK == null || cpNHHK.Value == 0)
                    throw new UserFriendlyException("Người Quản trị chưa thiết lập NHHK để ĐKMH, vui lòng liên hệ quản trị viên.");

                if (SecuritySystem.CurrentUser is Student)
                {
                    #region student
                   
                    CriteriaOperatorCollection c = new CriteriaOperatorCollection();

                    if (!IsInBookTime((Student)SecuritySystem.CurrentUser))
                    {                       
                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Không đăng ký được";
                        //ms.Message = string.Format("Chỉ đăng ký được trong khoảng thời gian qui định");
                        ms.Message = string.Format("Chỉ chọn được trong khoảng thời gian qui định\r\n từ ngày {0:dd-MM-yyyy} đến ngày {1:dd-MM-yyyy}",
                                StartConfirmDate, EndConfirmDate);
                        args.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        args.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("object", false);
                        dc.CancelAction.Caption = "Đóng";
                        args.ShowViewParameters.Controllers.Add(dc);

                        dc.SaveOnAccept = false;

                        return;
                    }

                    decimal deptAmount = 0m;
                    if (IsInDeptStudent(objectSpace, (Student)SecuritySystem.CurrentUser, out deptAmount))
                    {
                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Không đăng ký được";
                        ms.Message = string.Format("Không đăng ký được vì bạn đang nợ học phí số tiền {0:0,0}", deptAmount);
                        args.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        args.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("object", false);
                        dc.CancelAction.Caption = "Đóng";
                        args.ShowViewParameters.Controllers.Add(dc);
                        dc.SaveOnAccept = false;
                        return;
                    }


                    CollectionSource newCollectionSource = new CollectionSource(objectSpace, typeof(Lesson));
                    newCollectionSource.Criteria[cpNHHK.Value.ToString()] = new BinaryOperator(
                        "Semester.SemesterName", cpNHHK.Value, BinaryOperatorType.Greater);

                    listVacancies = new List<Vacancy>();
                    ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = listEditor.Grid;
                    gv.Selection.SelectAll();

                    foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
                    {
                        foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
                        {
                            listVacancies.Add(new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, tkbsem.Classroom.ClassroomCode));
                        }
                        newCollectionSource.Criteria[regDetail.Lesson.Subject.SubjectCode] =
                               new BinaryOperator("Subject.SubjectCode", regDetail.Lesson.Subject.SubjectCode, BinaryOperatorType.NotEqual);

                        //newCollectionSource.Criteria[regDetail.Lesson.Oid.ToString()] =
                        //    new BinaryOperator("Oid", regDetail.Lesson.Oid, BinaryOperatorType.NotEqual);
                    }
                    

                    using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                    {
                        Vacancy vc;
                        foreach (Lesson lesson in xpLesson)
                        {
                            if ((!lesson.CanRegister) || (lesson.NumRegistration >= lesson.NumExpectation)) //||(Convert.ToInt32(lesson.Semester.SemesterName)<=cpNHHK.Value)
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
                    selectAcception.AcceptAction.Caption = "Chọn";
                    selectAcception.CancelAction.Caption = "Bỏ qua";
                    selectAcception.Accepting += selectAcception_AcceptingStudent;
                    #endregion
                }
                else
                {
                    
                    CriteriaOperatorCollection c = new CriteriaOperatorCollection();

                    dicStudentRegDetail = new Dictionary<string, List<string>>();

                    CollectionSource newCollectionSource = new CollectionSource(objectSpace, typeof(Lesson));
                    newCollectionSource.Criteria[cpNHHK.Value.ToString()] = new BinaryOperator(
                       "Semester.SemesterName", cpNHHK.Value, BinaryOperatorType.Greater);
                    listVacancies = new List<Vacancy>();
                    foreach (Student student in View.SelectedObjects)
                    {
                        if (!dicStudentRegDetail.ContainsKey(student.StudentCode))
                            dicStudentRegDetail.Add(student.StudentCode, new List<string>());

                        foreach (RegisterDetail regDetail in student.RegisterDetails)
                        {
                            foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
                            {
                                listVacancies.Add(new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, tkbsem.Classroom.ClassroomCode));
                            }
                            dicStudentRegDetail[student.StudentCode].Add(regDetail.Lesson.Subject.SubjectCode);

                            newCollectionSource.Criteria[regDetail.Lesson.Subject.SubjectCode] =
                                new BinaryOperator("Subject.SubjectCode", regDetail.Lesson.Subject.SubjectCode, BinaryOperatorType.NotEqual);
                        }
                        //listStudentCode.Add(student.StudentCode);
                    }

                    using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                    {
                        Vacancy vc;
                        foreach (Lesson lesson in xpLesson)
                        {
                            if ((lesson.NumRegistration >= lesson.NumExpectation))//(Convert.ToInt32(lesson.Semester.SemesterName) <= cpNHHK.Value) ||
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

                    ListView lv = Application.CreateListView(
                        Application.FindListViewId(typeof(Lesson)),
                        newCollectionSource, true);
                    lv.Editor.AllowEdit = false;
                    lv.Editor.ControlsCreated += Editor_ControlsCreated;

                    args.ShowViewParameters.CreatedView = lv;
                    args.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    args.ShowViewParameters.CreateAllControllers = true;
                    //args.ShowViewParameters.Context = TemplateContext.View;
                    DialogController selectAcception = new DialogController();
                    args.ShowViewParameters.Controllers.Add(selectAcception);
                    selectAcception.Accepting += selectAcception_AcceptingAdmin;

                    selectAcception.AcceptAction.Caption = "Chọn";
                    selectAcception.CancelAction.Caption = "Bỏ qua";


                }
            }
        }

        private bool IsInDeptStudent(ObjectSpace objectSpace, Student student, out decimal deptAmount)
        {
            deptAmount = 0;
            try
            {
                ConstrainstParameter cp = objectSpace.FindObject<ConstrainstParameter>(
                   new BinaryOperator("Code", "MINDEPTCANENROLL"));
                
                deptAmount = -Convert.ToDecimal(student.AccountBalance.Value);
                if (deptAmount > Convert.ToDecimal(cp.Value))
                    return true;
            }
            catch { }
            finally { }
            return false;
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
                List<object> listobjOid = gc.GetSelectedFieldValues(new string[] { "Oid" });
                ObjectSpace objectSpace = Application.CreateObjectSpace();

                Vacancy vc;
                string strFilter = "", strInclude = "";
                foreach (Guid guid in listobjOid)
                {
                    Lesson lesson = objectSpace.FindObject<Lesson>(new BinaryOperator("Oid", guid));
                    strInclude += String.Format("OR ([Oid]={0})", lesson.Oid);
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
                    foreach (int strLessonCode in listobj)
                    {
                        strFilter += String.Format("AND ([LessonCode]<>{0})", strLessonCode);
                    }                   

                    using (XPCollection xpLesson = new XPCollection(objectSpace.Session, typeof(Lesson)))
                    {

                        foreach (Lesson lesson in xpLesson)
                        {
                            foreach (TkbSemester tkbsem in lesson.TKBSemesters)
                            {
                                vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                                if (Utils.IsConfictTKB(tempList, vc))
                                {
                                    strFilter += String.Format("AND ([Oid]<>{0})", lesson.Oid);
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
                    if (gc.FilterExpression.Contains("ClassIDs"))
                    {
                        gc.FilterExpression = string.Format("([ClassIDs] Like '%{0}%') AND {1}", currentClass, strFilter);
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
                        gc.FilterExpression = string.Format("[ClassIDs] Like '%{0}%'", currentClass);
                }
            }
        }

        void selectAcception_AcceptingStudent(object sender, DialogControllerAcceptingEventArgs e)
        {
            RuleSet ruleSet = new RuleSet();
            RuleSetValidationResult result;

            if (SecuritySystem.CurrentUser is Student)
            {
                ObjectSpace objectSpace = Application.CreateObjectSpace();
                ListView lv = ((ListView)((WindowController)sender).Window.View);
                if (SecuritySystem.CurrentUser is Student)
                {
                    objectSpace.Session.BeginTransaction();
                     PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
                    ms.Title = "Chọn nhóm MH thành công";
                    ms.Message = "";
                    Student student = SecuritySystem.CurrentUser as Student;
                    Student currentStudent = objectSpace.FindObject<Student>(
                        new BinaryOperator("StudentCode", student.StudentCode));
                    Lesson curLesson;
                    int checkresult, selectsubject=0;
                    double selectcredits = 0;
                    string subjectcoderesult, msgresult;
                    foreach (Lesson lesson in lv.SelectedObjects)
                    {
                        if (Utils.IsConfictPrerequisite(objectSpace, student.StudentCode, lesson.Subject.SubjectCode, out checkresult, out subjectcoderesult))
                        {
                            switch (checkresult)
                            {
                                case 1: msgresult = "học trước"; break;
                                case 2: msgresult = "tiên quyết"; break;
                                default: msgresult = "-"; break;
                            };
                            Subject subj = objectSpace.FindObject<Subject>(CriteriaOperator.Parse("SubjectCode =?", subjectcoderesult));   
                            ms.Message += string.Format("Không chọn được môn [{0}]{1} do vi phạm môn {2} là [{3}]{4}\r\n",
                                lesson.Subject.SubjectCode, lesson.Subject.SubjectName, msgresult, subj.SubjectCode, subj.SubjectName);
                            continue;
                        }
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
                        selectcredits += curLesson.Subject.Credit;
                        selectsubject++;
                        result = ruleSet.ValidateTarget(regdetail, DefaultContexts.Save);
                        if (ValidationState.Invalid ==
                            result.GetResultItem("RegisterDetail.StudentRegLessonSemester").State)
                        {                            
                            regdetail.Delete();
                            selectcredits -= curLesson.Subject.Credit;
                            selectsubject--;
                        }                      

                    }
                    objectSpace.Session.CommitTransaction();
                    
                    View.ObjectSpace.Refresh();
                   
                    ms.Message += string.Format("Tổng số chọn được {0} nhóm lớp môn học với {1} tín chỉ.", selectsubject, selectcredits);
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
        private void BookRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            double numcredits = 0;
            Decimal sumTuitionFee = 0;
           
            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            RuleSet ruleSet = new RuleSet();
            RuleSetValidationResult result;
            PopUpMessage ms;
            DialogController dc;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            if (listEditor != null)
            {

                if (SecuritySystem.CurrentUser is Student)
                {
                    if (!IsInBookTime((Student)SecuritySystem.CurrentUser))
                    {

                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Không đăng ký được";
                        ms.Message = string.Format("Chỉ đăng ký được trong khoảng thời gian qui định\r\n từ ngày {0:dd-MM-yyyy} đến ngày {1:dd-MM-yyyy}",
                    StartConfirmDate, EndConfirmDate);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("object", false);
                        dc.CancelAction.Caption = "Đóng";
                        e.ShowViewParameters.Controllers.Add(dc);

                        dc.SaveOnAccept = false;

                        return;
                    }
                    decimal deptAmount = 0m;
                    if (IsInDeptStudent(objectSpace, (Student)SecuritySystem.CurrentUser, out deptAmount))
                    {   
                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Không đăng ký được";
                        ms.Message = string.Format("Không đăng ký được vì bạn đang nợ học phí số tiền {0:0,0}", deptAmount);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("object", false);
                        dc.CancelAction.Caption = "Đóng";
                        e.ShowViewParameters.Controllers.Add(dc);
                        dc.SaveOnAccept = false;
                        return;
                    }
                    #region student ---------
                    ASPxGridView gv = listEditor.Grid;
                    gv.Selection.SelectAll();

                    //kiem tra rang buoc so tin chi min 
                    foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
                    {
                        numcredits += regDetail.Lesson.Subject.Credit;

                    }
                    ConstrainstParameter cparam = objectSpace.FindObject<ConstrainstParameter>(
                    new BinaryOperator("Code", "MINCREDITS"));
                    if (cparam != null && numcredits < Convert.ToDouble(cparam.Value))
                    {

                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Lỗi đăng ký";
                        ms.Message = string.Format("Không đăng ký ít hơn {0} tín chỉ!", cparam.Value);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("object", false);
                        dc.CancelAction.Caption = "Đóng";
                        e.ShowViewParameters.Controllers.Add(dc);

                        dc.SaveOnAccept = false;

                        return;
                    }
                    //kiem tra rang buoc so tin chi max
                    cparam = View.ObjectSpace.FindObject<ConstrainstParameter>(
                    new BinaryOperator("Code", "MAXCREDITS"));
                    if (cparam != null && numcredits > Convert.ToDouble(cparam.Value))
                    {

                        ms = objectSpace.CreateObject<PopUpMessage>();
                        ms.Title = "Lỗi đăng ký";
                        ms.Message = string.Format("Không đăng ký nhiều hơn {0} tín chỉ!", cparam.Value);
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                             objectSpace, ms);
                        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                        dc = Application.CreateController<DialogController>();
                        e.ShowViewParameters.Controllers.Add(dc);

                        dc.SaveOnAccept = false;

                        return;
                    }

                    numcredits = 0;
                    sumTuitionFee = 0;
                    string lessonCodelist = "";
                    string semestername = "";
                    foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
                    {
                        result = ruleSet.ValidateTarget(regDetail, DefaultContexts.Save);
                        if (ValidationState.Invalid ==
                            result.GetResultItem("RegisterDetail.StudentRegLessonSemester").State)
                        {
                            throw new UserFriendlyException(string.Format ("Đăng ký trùng môn học! Vui lòng bỏ môn trùng, Mã môn = \"{0}\"", regDetail.Lesson.Subject.SubjectCode));
                        }      
                        if (regDetail.RegisterState.Code == "SELECTED")
                        {
                            numcredits += regDetail.Lesson.Subject.Credit;
                            sumTuitionFee += regDetail.Lesson.TuitionFee;
                            View.ObjectSpace.SetModified(regDetail);
                            View.ObjectSpace.SetModified(regDetail.Lesson);
                            regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "BOOKED"));
                            regDetail.CheckState = View.ObjectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "NOTCHECKED"));
                            regDetail.Lesson.NumRegistration++;
                            semestername = regDetail.Lesson.Semester.SemesterName;
                            lessonCodelist += (String.Format("{0}({1})-", regDetail.Lesson.LessonName, regDetail.Lesson.Subject.SubjectCode));
                        }
                    }
                    lessonCodelist = lessonCodelist.TrimEnd('-');
                    //create Account transaction
                    if (sumTuitionFee > 0)
                    {
                        AccountTransaction act = View.ObjectSpace.CreateObject<AccountTransaction>();
                        Student stud = View.ObjectSpace.FindObject<Student>(
                            new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
                        Semester semester = View.ObjectSpace.FindObject<Semester>(
                            new BinaryOperator("SemesterName", semestername));
                        act.Student = stud;
                        act.MoneyAmount = -sumTuitionFee;
                        act.TransactingDate = DateTime.Now;
                        act.Semester = semester;
                        act.Description = string.Format("Học phí [{0}] phải đóng cho {1} tín chỉ đăng ký: [{2}], do [{3}] thực hiện đăng ký", stud.StudentCode, numcredits, lessonCodelist, stud.FullName);
                        act.Save();
                    }
                    View.ObjectSpace.CommitChanges();

                    ms = objectSpace.CreateObject<PopUpMessage>();
                    ms.Title = "Thực hiện đăng ký thành công";
                    ms.Message = string.Format("Bạn đã chọn đăng ký {0} tín chí, với số tiền {1} cho {2} nhóm lớp MH", numcredits, sumTuitionFee, ((ListView)View).SelectedObjects.Count);
                    ms.Message += "\r\n Vui lòng xem kết quả giao dịch học phí và thực hiện xác nhận ";
                    IsInConfirmTime((Student)SecuritySystem.CurrentUser);
                    ms.Message += string.Format("\r\n từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}!!!", StartConfirmDate, EndConfirmDate);
                   
                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                         objectSpace, ms);
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                    dc = Application.CreateController<DialogController>();
                    e.ShowViewParameters.Controllers.Add(dc);

                    dc.SaveOnAccept = false;
                    #endregion

                }
                else
                {

                    Dictionary<string, double> studentnumcredits = new Dictionary<string, double>();
                    Dictionary<string, decimal> studentFee = new Dictionary<string, decimal>();
                    Dictionary<string, string> studentLessonCodeList = new Dictionary<string, string>();
                    Dictionary<string, string> lessonCannotReg = new Dictionary<string, string>();
                    string semesterName = "";
                    foreach (RegisterDetail regDetail in View.SelectedObjects)
                    {

                        if (!studentnumcredits.ContainsKey(regDetail.Student.StudentCode))
                        {
                            studentnumcredits.Add(regDetail.Student.StudentCode, 0);
                        }
                        if (!studentFee.ContainsKey(regDetail.Student.StudentCode))
                        {
                            studentFee.Add(regDetail.Student.StudentCode, 0);
                        }
                        if (!studentLessonCodeList.ContainsKey(regDetail.Student.StudentCode))
                        {
                            studentLessonCodeList.Add(regDetail.Student.StudentCode, "");
                        }
                        View.ObjectSpace.SetModified(regDetail);
                        View.ObjectSpace.SetModified(regDetail.Lesson);
                        if (regDetail.RegisterState.Code == "SELECTED")
                        {
                            if (regDetail.Lesson.NumRegistration >= regDetail.Lesson.NumExpectation)
                            {
                                if (!lessonCannotReg.ContainsKey(regDetail.Lesson.LessonName))
                                {
                                    lessonCannotReg.Add(regDetail.Lesson.LessonName, "");
                                }
                                lessonCannotReg[regDetail.Lesson.LessonName] += string.Format("[{0}]-", regDetail.Student.StudentCode);
                                regDetail.Note = "Over Sized";
                            }
                            else
                            {
                                studentnumcredits[regDetail.Student.StudentCode] += regDetail.Lesson.Subject.Credit;
                                studentFee[regDetail.Student.StudentCode] += regDetail.Lesson.TuitionFee;
                                studentLessonCodeList[regDetail.Student.StudentCode] += (String.Format("{0}({1})-", regDetail.Lesson.LessonName, regDetail.Lesson.Subject.SubjectCode));

                                semesterName = regDetail.Lesson.Semester.SemesterName;
                                regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "BOOKED"));
                                regDetail.Lesson.NumRegistration++;
                            }
                        }
                    }
                    int numstudent = 0;
                    double credit = 0;
                    decimal sumfee = 0;
                    foreach (string studCode in studentLessonCodeList.Keys)
                    {
                        if (studentnumcredits[studCode] > 0)
                        {
                            numstudent++;
                            credit += studentnumcredits[studCode];
                            sumfee += studentFee[studCode];

                            //create Account transaction
                            if (studentFee[studCode] > 0)
                            {
                                AccountTransaction act = View.ObjectSpace.CreateObject<AccountTransaction>();
                                Student studnew = View.ObjectSpace.FindObject<Student>(
                                    new BinaryOperator("StudentCode", studCode));
                                Semester semester = View.ObjectSpace.FindObject<Semester>(
                                    new BinaryOperator("SemesterName", semesterName));
                                act.Semester = semester;
                                act.Student = studnew;
                                act.MoneyAmount = -studentFee[studCode];
                                act.TransactingDate = DateTime.Now;
                                act.Description = string.Format("Học phí sinh viên [{0}] phải đóng cho {1} tín chỉ đăng ký là {2:0,0},\r\n bao gồm: [{3}],\r\n [{4}] thực hiện đăng ký",
                                    studnew.StudentCode, studentnumcredits[studCode], studentFee[studCode], studentLessonCodeList[studCode].TrimEnd('-'), SecuritySystem.CurrentUserName);
                                act.Save();
                            }
                        }
                    }

                    View.ObjectSpace.CommitChanges();

                    ms = objectSpace.CreateObject<PopUpMessage>();
                    ms.Title = "Thực hiện đăng ký (Booked) thành công";
                    ms.Message = string.Format("Bạn đã chọn đăng ký cho {0} sinh viên, với số tiền {1:0,0} cho tổng số {2:0,0} tín chỉ học",
                        numstudent, sumfee, credit);
                    ms.Message += "\r\n Vui lòng xem kết quả giao dịch học phí!!!";
                    if (lessonCannotReg.Count > 0)
                    {
                        string strmessage = "";
                        foreach (KeyValuePair<string, string> keypair in lessonCannotReg)
                        {
                            strmessage += string.Format("Nhóm lớp MH:[{0}], sinh viên [{1}] \r\n", keypair.Key, keypair.Value.TrimEnd('-'));
                        }
                        ms.Message += "\r\nKhông đăng ký được do quá sĩ số:\r\n" + strmessage;
                    }
                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                         objectSpace, ms);
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                    dc = Application.CreateController<DialogController>();
                    e.ShowViewParameters.Controllers.Add(dc);
                    dc.AcceptAction.Active.SetItemValue("object", false);
                    dc.CancelAction.Caption = "Đóng";
                    dc.SaveOnAccept = false;
                }
            }
        }

        private bool IsInBookTime(Student student)
        {
            StartConfirmDate = DateTime.MaxValue;
            EndConfirmDate = DateTime.MinValue;
            if (student.StudentClass.RegisterTimes.Count > 0)
            {
                foreach (RegisterTime regTime in student.StudentClass.RegisterTimes)
                {
                    if (regTime.Active)
                    {
                        if ((regTime.BookingDateStart <= DateTime.Now) && (regTime.BookingDateEnd.AddDays(1) > DateTime.Now))
                        {
                            return true;
                        }
                        else
                        {
                            StartConfirmDate = StartConfirmDate > regTime.BookingDateStart ? regTime.BookingDateStart : StartConfirmDate;
                            EndConfirmDate = EndConfirmDate < regTime.BookingDateEnd ? regTime.BookingDateEnd : EndConfirmDate;
                        }
                    }      
                }
                return false;
            }
            return false;
        }
        DateTime StartConfirmDate;
        DateTime EndConfirmDate;
        private void ConfirmRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();            
            DialogController dc = Application.CreateController<DialogController>();
            if (!IsInConfirmTime((Student)SecuritySystem.CurrentUser))
            {

                ms = objectSpace.CreateObject<PopUpMessage>();
                ms.Title = "Không xác nhận được";
                ms.Message = string.Format("Chỉ xác nhận được trong khoảng thời gian qui định\r\n từ ngày {0:dd-MM-yyyy} đến ngày {1:dd-MM-yyyy}",
                    StartConfirmDate,EndConfirmDate);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                     objectSpace, ms);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.CreatedView.Caption = "Thông báo";
                dc = Application.CreateController<DialogController>();
                dc.AcceptAction.Active.SetItemValue("object", false);
                dc.CancelAction.Caption = "Đóng";
                e.ShowViewParameters.Controllers.Add(dc);

                dc.SaveOnAccept = false;

                return;
            }
            
            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (listEditor != null)
            {
                

                int i = 0,j=0;
                string messsage = "";
                ms.Message = messsage;
                ASPxGridView gv = listEditor.Grid;
                gv.Selection.SelectAll();
                foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
                {
                    if (regDetail.RegisterState.Code == "BOOKED")
                    {
                        regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
                                new BinaryOperator("Code", "PRINTED"));
                    }
                    if (regDetail.CheckState.Code == "CHECKED")
                    {
                        View.ObjectSpace.SetModified(regDetail);
                        regDetail.CheckState = View.ObjectSpace.FindObject<RegisterState>(
                            new BinaryOperator("Code", "CONFIRMED"));
                        j++;
                    }
                    i++;
                }
                View.ObjectSpace.CommitChanges();
                ms.Message= string.Format("Đã chọn in kết quả đăng ký cho \"{0}\" Nhóm MH,\r\n(Có {1} nhóm MH đã được kiểm tra, xác nhận), \r\n Nhấn OK để in kết quả!", i, j);
                ms.Title = "Kết quả xác nhận!";
                dc.Accepting += dc_Accepting_Report;
                dc.CancelAction.Caption = "Bỏ qua";
                dc.SaveOnAccept = false;
                
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(
                    objectSpace, ms);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.CreatedView.Caption = "Thông báo kết quả";
                e.ShowViewParameters.Controllers.Add(dc);
            }
        }

        private bool IsInConfirmTime(Student student)
        {
            StartConfirmDate = DateTime.MaxValue;
            EndConfirmDate = DateTime.MinValue;
            if (student.StudentClass.RegisterTimes.Count > 0)
            {
                foreach (RegisterTime regTime in student.StudentClass.RegisterTimes)
                {

                    if (regTime.Active)
                    {
                        if ((regTime.ConfirmDateStart <= DateTime.Now) && (regTime.ConfirmDateEnd.AddDays(1) >= DateTime.Now))
                        {
                            return true;
                        }
                        else
                        {
                            StartConfirmDate = StartConfirmDate > regTime.ConfirmDateStart ? regTime.ConfirmDateStart : StartConfirmDate;
                            EndConfirmDate = EndConfirmDate < regTime.ConfirmDateEnd ? regTime.ConfirmDateEnd : EndConfirmDate;
                        }
                    }                    
                }
                return false;
            }
            return false;
        }

        void dc_Accepting_Report(object sender, DialogControllerAcceptingEventArgs e)
        {
            Controller controller = sender as Controller;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ReportData rd = objectSpace.FindObject<ReportData>(
                new BinaryOperator("Name", "Kết quả ĐK 1 SV"));
            if (rd != null)
            {
                XafReport report = rd.LoadXtraReport(objectSpace);

                CriteriaOperator criteriaOperator = CriteriaOperator.TryParse(
                        String.Format("[Student.Oid] = '{0}'", SecuritySystem.CurrentUserId));
                Student stud = objectSpace.FindObject<Student>(CriteriaOperator.TryParse(
                        String.Format("[Oid] = '{0}'", SecuritySystem.CurrentUserId)));

                //Frame.GetController<ReportServiceController>().ShowPreview((IReportData)rd, criteriaOperator);
                report.Filtering.Filter = criteriaOperator.ToString();
                MemoryStream stream = new MemoryStream();
                report.ExportToPdf(stream);
                
                if (HttpContext.Current != null)
                {


                    byte[] buffer = stream.GetBuffer();
                    string contentType = "application/pdf";
                    string contentDisposition = "attachment; filename=KQDK_" + stud.StudentCode + ".pdf";
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = false;
                    HttpContext.Current.Response.AppendHeader("Content-Type", contentType);
                    HttpContext.Current.Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", contentDisposition);
                    HttpContext.Current.Response.BinaryWrite(buffer);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    report.ShowPreview();
                }
                controller.Dispose();

            }
        }

        //Dictionary<string, Dictionary<string, WeekReportData>> dicTeacherWeekData= new Dictionary<string,Dictionary<string,WeekReportData>>();
        //private void ViewTeacherWeekTimeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{

        //    ObjectSpace objectSpace = Application.CreateObjectSpace();
        //    DialogController selectSemesterAcception;
        //    dicTeacherWeekData.Clear();
        //    if (View.SelectedObjects.Count == 0)
        //    {
        //        PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
        //        ms.Title = "Thông báo";
        //        ms.Message = "Vui lòng chọn giảng viên";
        //        e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, ms);
        //        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                
        //        //args.ShowViewParameters.Context = TemplateContext.View;
        //        selectSemesterAcception = new DialogController();
        //        e.ShowViewParameters.Controllers.Add(selectSemesterAcception);
        //        selectSemesterAcception.AcceptAction.Active.SetItemValue("Object", false);
        //        selectSemesterAcception.CancelAction.Caption = "Bỏ qua";
        //        return;
        //    }

        //    foreach (Teacher teacher in View.SelectedObjects)
        //    {
        //        if (!dicTeacherWeekData.ContainsKey(teacher.TeacherCode))
        //            dicTeacherWeekData.Add(teacher.TeacherCode, new Dictionary<string,WeekReportData>());
        //    }

        //    ListView lvSemester = Application.CreateListView(objectSpace,typeof(Semester),true);
        //    lvSemester.Editor.AllowEdit = false;           
            
        //    e.ShowViewParameters.CreatedView = lvSemester;
        //    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //    e.ShowViewParameters.CreateAllControllers = true;
        //    //args.ShowViewParameters.Context = TemplateContext.View;
        //    selectSemesterAcception = new DialogController();
        //    e.ShowViewParameters.Controllers.Add(selectSemesterAcception);
        //    selectSemesterAcception.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(selectSemesterAcception_Accepting);
        //    selectSemesterAcception.AcceptAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
        //    selectSemesterAcception.AcceptAction.TargetObjectType = typeof(Semester);
        //    selectSemesterAcception.AcceptAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        //    selectSemesterAcception.AcceptAction.TargetViewType = ViewType.ListView;
        //    selectSemesterAcception.AcceptAction.Caption = "Chọn";
        //    selectSemesterAcception.CancelAction.Caption = "Bỏ qua";
        //}
        //void selectSemesterAcception_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        //{
        //    Dictionary<string, WeekReportData> dicTeacherPeriodData;
        //    WeekReportData currentWeekData;
        //    ObjectSpace objectSpace = Application.CreateObjectSpace();
        //    ListView lv = ((ListView)((WindowController)sender).Window.View);
        //    Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
        //    if (semester != null)
        //    {
        //        foreach (string teachercode in dicTeacherWeekData.Keys)
        //        {
        //            XPCollection<WeekReportData> xpw = new XPCollection<WeekReportData>(objectSpace.Session,
        //                    CriteriaOperator.Parse("Teachers.TeacherCode =?", teachercode));

        //            objectSpace.Delete(xpw);
        //            //objectSpace.Session.PurgeDeletedObjects(); 
        //            objectSpace.CommitChanges();

        //            XPCollection<Lesson> xpclesson = new XPCollection<Lesson>(objectSpace.Session,
        //                new GroupOperator(GroupOperatorType.And,new BinaryOperator("Semester.SemesterName",
        //                    semester.SemesterName),new ContainsOperator("Teachers",new BinaryOperator("TeacherCode",teachercode))));


        //            dicTeacherPeriodData = dicTeacherWeekData[teachercode];

        //            foreach (Lesson lesson in xpclesson)
        //            {
        //                foreach (TkbSemester tkbSem in lesson.TKBSemesters)
        //                {

        //                    string[] strperiod = tkbSem.Period.Split(',');
        //                    ArrayList periodTimeList = new ArrayList();  
        //                    foreach (string s in strperiod)
        //                    {
        //                        if (Convert.ToInt32(s) >= 1 && Convert.ToInt32(s) <= 6)
        //                            if (!periodTimeList.Contains("Sáng")) 
        //                                periodTimeList.Add("Sáng");
        //                        if (Convert.ToInt32(s) >= 6 && Convert.ToInt32(s) <= 12)
        //                            if (!periodTimeList.Contains("Chiều"))
        //                                periodTimeList.Add("Chiều");
        //                        if (Convert.ToInt32(s) > 12 )
        //                            if (!periodTimeList.Contains("Tối"))
        //                                periodTimeList.Add("Tối");
        //                    }

        //                    foreach (String period in periodTimeList)
        //                    {
        //                        if (!dicTeacherPeriodData.ContainsKey(period))
        //                        {
        //                            currentWeekData = objectSpace.CreateObject<WeekReportData>();
        //                            currentWeekData.Semester = objectSpace.FindObject<Semester>(
        //                                CriteriaOperator.Parse("SemesterName = ?", semester.SemesterName));
        //                            //currentWeekData.Name = teachercode;
                                    
        //                            currentWeekData.PeriodTime = period;

        //                            dicTeacherPeriodData.Add(period, currentWeekData);
        //                        }

        //                        dicTeacherPeriodData[period][tkbSem.Day] += string.Format("Lớp:{0}\r\n Môn:{1}-{2}\r\nTiết:{3}\r\nTuần:{4}\r\n",
        //                                tkbSem.Lesson.ClassIDs, tkbSem.Lesson.Subject.SubjectCode, tkbSem.Lesson.Subject.SubjectName,
        //                                Utils.ShortStringPeriod(tkbSem.Period), Utils.ShortStringWeek(tkbSem.StringWeeks));

        //                    }
        //                }
        //            }
        //        }
        //        objectSpace.CommitChanges();
        //        string strParse = "";
        //        foreach (string teachercode in dicTeacherWeekData.Keys)
        //        {
        //            strParse += (strParse == "" ? string.Format("TeacherCode='{0}'", teachercode) : string.Format("or TeacherCode='{0}'", teachercode));                    
        //        }
        //        //foreach (KeyValuePair<string, Dictionary<string, WeekReportData>> kp in dicTeacherWeekData)
        //        //{
        //        //    strParse += (strParse == "" ? string.Format("Name='{0}'", kp.Key) : string.Format("or Name='{0}'", kp.Key));                    
        //        //}
        //        //strParse = string.Format("({0}) and Type='{1}'", strParse, Utils.DomainClassTypeName[typeof(Teacher)]);
        //        ReportData reportData = objectSpace.FindObject<ReportData>(
        //             new BinaryOperator("Name", "Lịch giảng viên"));
        //        //if (reportData != null)
        //        //{
        //        //    XafReport xafReport = reportData.LoadXtraReport(objectSpace);

        //        //    xafReport.Filtering.Filter = CriteriaOperator.Parse(strParse).ToString();
        //        //    xafReport.ShowPreviewDialog();
        //        //}

        //        ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
        //        rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse));
        //    }
        //}
        ////void lvSemesterEditor_ControlsCreated(object sender, EventArgs e)
        //{
        //    ASPxGridListEditor gc = sender as ASPxGridListEditor;
        //    ASPxGridView gridView = (ASPxGridView)gc.Grid;
            
        //    gridView.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;                       
        //    gridView.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;            
        //    gridView.SettingsBehavior.AllowMultiSelection = false;
        ////    gridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            
        //}
        
        
    }
}
