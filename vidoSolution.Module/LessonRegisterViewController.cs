using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using vidoSolution.Module.DomainObject;

using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using vidoSolution.Module.Utilities;
using System.Collections;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Reports;
using System.IO;
using System.Web;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Validation;



namespace vidoSolution.Module
{
    public partial class LessonRegisterViewController : ViewController
    {

        public LessonRegisterViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        #region cm1
        //protected override void OnActivated()
        //{
        //    base.OnActivated();
        //    if (((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail))
        //        RegisterDetailView = (ListView)View;
        //    else
        //        RegisterDetailView = null;
        //    //Event simpleAction1.Active.SetItemValue("ObjectType", View.ObjectTypeInfo.Type == typeof(TrialBalance)); 
        //    //ObjectSpace objectSpace = Application.CreateObjectSpace(); 
        //    //CollectionSource collectionSource = 
        //    //    (CollectionSource)Application.CreateCollectionSource(objectSpace, 
        //    //    typeof(OperationUnit), "OperationUnit_ListView"); 
        //    //ListView listview = Application.CreateListView(
        //    //    "OperationUnit_ListView", collectionSource, true); 
        //    //e.ShowViewParameters.CreatedView = listview; 
        //    //e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow; 
        //    //e.ShowViewParameters.CreateAllControllers = true; 
        //    //DialogController dcOU = new DialogController(); 
        //    //e.ShowViewParameters.Controllers.Add(dcOU); 
        //    //dcOU.Accepting += new EventHandler(dcOU_Accepting); 
        //}
        #endregion
        private void LessonRegisterViewController_Activated(object sender, EventArgs e)
        {

            //   ConfirmRegister.Enabled.SetItemValue("EditMode",
            //((DetailView)View).ViewEditMode == ViewEditMode.Edit);
            //   ((DetailView)View).ViewEditModeChanged +=
            //       new EventHandler<EventArgs>(ConfirmRegisterController_ViewEditModeChanged);
            bool showRegister = (SecuritySystem.CurrentUser is Student) && ((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail);

           
            DefaultRegisterAction.Active.SetItemValue(
                 "ObjectType", false);

            CancelRegister.Active.SetItemValue(
                 "ObjectType", showRegister);
           
            CalculateFeeForLesson.Active.SetItemValue(
                   "ObjectType", false);

            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles,
              new BinaryOperator("Name", "DataAdmins"));
            if (xpc.Count + xpc2.Count > 0)
            {
                showRegister = showRegister || ((ListView)View).ObjectTypeInfo.Type == typeof(Student);

                CancelRegister.Active.SetItemValue(
                   "ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail));
                CheckRegister.Active.SetItemValue(
                    "ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail));
                CalculateFeeForLesson.Active.SetItemValue(
                    "ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(Lesson));
                DefaultRegisterAction.Active.SetItemValue(
                    "ObjectType", ((ListView)View).ObjectTypeInfo.Type == typeof(StudentClass));
               
            }
            else
            {

                CheckRegister.Active.SetItemValue(
                       "ObjectType", false);
            }

        }
        //Manages the ClearFields Action enabled state 
        //void ConfirmRegisterController_ViewEditModeChanged(object sender, EventArgs e)
        //{
        //    ConfirmRegister.Enabled.SetItemValue("EditMode",
        //        ((DetailView)View).ViewEditMode == ViewEditMode.Edit);
        //}
        private void CheckRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            ListEditor listEditor = ((ListView)View).Editor as ListEditor;
            if (listEditor != null)
            {
                foreach (RegisterDetail regDetail in View.SelectedObjects)
                {

                    if (regDetail.CheckState.Code == "NOTCHECKED" && regDetail.RegisterState.Code == "BOOKED")
                    {
                        View.ObjectSpace.SetModified(regDetail);
                        regDetail.CheckState = View.ObjectSpace.FindObject<RegisterState>(
                            new BinaryOperator("Code", "CHECKED"));

                    }
                }
                View.ObjectSpace.CommitChanges();
            }


        }
        private void CancelRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            ListEditor listEditor = ((ListView)View).Editor as ListEditor;
            int i = 0;
            if (listEditor != null)
            {
                foreach (RegisterDetail regDetail in View.SelectedObjects)
                {
                    if (regDetail.RegisterState.Code == "SELECTED")
                    {
                        View.ObjectSpace.SetModified(regDetail);

                        regDetail.Delete();
                        i++;
                    }
                }
                View.ObjectSpace.CommitChanges();
            }
            PopUpMessage ms;
            DialogController dc;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ms = objectSpace.CreateObject<PopUpMessage>();
            ms.Title = "Kết quả hủy";
            ms.Message = string.Format("Đã hủy chọn {0} nhóm lớp MH!", i);
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
        private void CalculateFeeForLesson_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ConstrainstParameter cp = View.ObjectSpace.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "TUITIONFEEPERCREDIT"));
            try
            {
                decimal fee = Convert.ToDecimal(cp.Value);
                
                foreach (Lesson lesson in ((ListView)View).SelectedObjects)
                {
                    View.ObjectSpace.SetModified(lesson);
                    lesson.TuitionFee = Convert.ToDecimal(lesson.Subject.Credit) * fee;
                }
                View.ObjectSpace.CommitChanges();
            }
            catch
            {
                throw new UserFriendlyException("Không có giá học phí mặc định cho 1 tín chỉ (TUITIONFEEPERCREDIT)");
            }
            PopUpMessage ms;
            DialogController dc;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ms = objectSpace.CreateObject<PopUpMessage>();
            ms.Title = "Kết quả cập nhật";
            ms.Message = string.Format("Đã cập nhật học phí cho {0} nhóm lớp MH!", ((ListView)View).SelectedObjects.Count);
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

        #region comment
        
        //private void BookRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    double numcredits = 0;
        //    Decimal sumTuitionFee = 0;
        //    ListEditor listEditor = ((ListView)View).Editor as ListEditor;
        //    PopUpMessage ms;
        //    DialogController dc;
        //    ObjectSpace objectSpace = Application.CreateObjectSpace();
        //    if (listEditor != null)
        //    {
        //        if (SecuritySystem.CurrentUser is Student)
        //        {

        //            //kiem tra rang buoc so tin chi min 
        //            foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
        //            {
        //                numcredits += regDetail.Lesson.Subject.Credit;

        //            }
        //            ConstrainstParameter cparam = objectSpace.FindObject<ConstrainstParameter>(
        //            new BinaryOperator("Code", "MINCREDITS"));
        //            if (cparam != null && numcredits < Convert.ToDouble(cparam.Value))
        //            {

        //                ms = objectSpace.CreateObject<PopUpMessage>();
        //                ms.Title = "Lỗi đăng ký";
        //                ms.Message = string.Format("Không đăng ký ít hơn {0} tín chỉ!", cparam.Value);
        //                e.ShowViewParameters.CreatedView = Application.CreateDetailView(
        //                     objectSpace, ms);
        //                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //                e.ShowViewParameters.CreatedView.Caption = "Thông báo";
        //                dc = Application.CreateController<DialogController>();
        //                dc.AcceptAction.Active.SetItemValue("object", false);
        //                dc.CancelAction.Caption = "Đóng";
        //                dc.SaveOnAccept = false;
        //                e.ShowViewParameters.Controllers.Add(dc);

        //                return;
        //            }
        //            //kiem tra rang buoc so tin chi max
        //            cparam = View.ObjectSpace.FindObject<ConstrainstParameter>(
        //            new BinaryOperator("Code", "MAXCREDITS"));
        //            if (cparam != null && numcredits > Convert.ToDouble(cparam.Value))
        //            {

        //                ms = objectSpace.CreateObject<PopUpMessage>();
        //                ms.Title = "Lỗi đăng ký";
        //                ms.Message = string.Format("Không đăng ký nhiều hơn {0} tín chỉ!", cparam.Value);
        //                e.ShowViewParameters.CreatedView = Application.CreateDetailView(
        //                     objectSpace, ms);
        //                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //                e.ShowViewParameters.CreatedView.Caption = "Thông báo";
        //                dc = Application.CreateController<DialogController>();
        //                dc.AcceptAction.Active.SetItemValue("object", false);
        //                dc.CancelAction.Caption = "Đóng";
        //                dc.SaveOnAccept = false;
        //                e.ShowViewParameters.Controllers.Add(dc);

                    
        //                return;
        //            }

        //            numcredits = 0;
        //            sumTuitionFee = 0;
        //            string lessonCodelist = "";

        //            foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
        //            {
        //                if (regDetail.RegisterState.Code == "SELECTED")
        //                {
        //                    numcredits += regDetail.Lesson.Subject.Credit;
        //                    sumTuitionFee += regDetail.Lesson.TuitionFee;
        //                    View.ObjectSpace.SetModified(regDetail);
        //                    View.ObjectSpace.SetModified(regDetail.Lesson);
        //                    regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
        //                        new BinaryOperator ("Code","BOOKED"));
        //                    regDetail.CheckState = View.ObjectSpace.FindObject<RegisterState>(
        //                        new BinaryOperator("Code", "NOTCHECKED"));
        //                    regDetail.Lesson.NumRegistration++;
        //                    lessonCodelist += (String.Format("{0}({1})-", regDetail.Lesson.LessonName, regDetail.Lesson.Subject.SubjectCode));
        //                }
        //            }
        //            lessonCodelist = lessonCodelist.TrimEnd('-');
        //            //create Account transaction
        //            AccountTransaction act = View.ObjectSpace.CreateObject<AccountTransaction>();
        //            Student stud = View.ObjectSpace.FindObject<Student>(
        //                new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
        //            act.Student = stud;
        //            act.MoneyAmount = -sumTuitionFee;
        //            act.TransactingDate = DateTime.Now;
        //            act.Description = string.Format("Học phí [{0}] phải đóng cho {1} tín chỉ đăng ký: [{2}], do [{3}] thực hiện đăng ký", stud.StudentCode, numcredits, lessonCodelist, stud.FullName);
        //            act.Save();

        //            View.ObjectSpace.CommitChanges();

        //            ms = objectSpace.CreateObject<PopUpMessage>();
        //            ms.Title = "Thực hiện đăng ký thành công";
        //            ms.Message = string.Format("Bạn đã chọn đăng ký {0} nhóm lớp môn học, với số tiền {1}", numcredits, sumTuitionFee);
        //            ms.Message += "\r\n Vui lòng xem kết quả giao dịch học phí!!!";
        //            e.ShowViewParameters.CreatedView = Application.CreateDetailView(
        //                 objectSpace, ms);
        //            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //            e.ShowViewParameters.CreatedView.Caption = "Thông báo";
        //            dc = Application.CreateController<DialogController>();
        //            dc.AcceptAction.Active.SetItemValue("object", false);
        //            dc.CancelAction.Caption = "Đóng";
        //            dc.SaveOnAccept = false;
        //            e.ShowViewParameters.Controllers.Add(dc);

        //        }
        //        else
        //        {

        //            Dictionary<string, double> studentnumcredits = new Dictionary<string, double>();
        //            Dictionary<string, decimal> studentFee = new Dictionary<string, decimal>();
        //            Dictionary<string, string> studentLessonCodeList = new Dictionary<string, string>();
        //            Dictionary<string, string> lessonCannotReg = new Dictionary<string, string>();
        //            foreach (RegisterDetail regDetail in View.SelectedObjects)
        //            {

        //                if (!studentnumcredits.ContainsKey(regDetail.Student.StudentCode))
        //                {
        //                    studentnumcredits.Add(regDetail.Student.StudentCode, 0);
        //                }
        //                if (!studentFee.ContainsKey(regDetail.Student.StudentCode))
        //                {
        //                    studentFee.Add(regDetail.Student.StudentCode, 0);
        //                }
        //                if (!studentLessonCodeList.ContainsKey(regDetail.Student.StudentCode))
        //                {
        //                    studentLessonCodeList.Add(regDetail.Student.StudentCode, "");
        //                }
        //                View.ObjectSpace.SetModified(regDetail);
        //                View.ObjectSpace.SetModified(regDetail.Lesson);
        //                if (regDetail.RegisterState.Code == "SELECTED")
        //                {
        //                    if (regDetail.Lesson.NumRegistration >= regDetail.Lesson.NumExpectation)
        //                    {
        //                        if (!lessonCannotReg.ContainsKey(regDetail.Lesson.LessonName))
        //                        {
        //                            lessonCannotReg.Add(regDetail.Lesson.LessonName, "");
        //                        }
        //                        lessonCannotReg[regDetail.Lesson.LessonName] += string.Format("[{0}]-", regDetail.Student.StudentCode);
        //                        regDetail.Note = "Over Sized";
        //                    }
        //                    else
        //                    {
        //                        studentnumcredits[regDetail.Student.StudentCode] += regDetail.Lesson.Subject.Credit;
        //                        studentFee[regDetail.Student.StudentCode] += regDetail.Lesson.TuitionFee;
        //                        studentLessonCodeList[regDetail.Student.StudentCode] += (String.Format("{0}({1})-", regDetail.Lesson.LessonName, regDetail.Lesson.Subject.SubjectCode));


        //                        regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
        //                        new BinaryOperator("Code", "BOOKED"));
        //                        regDetail.Lesson.NumRegistration++;
        //                    }
        //                }
        //            }
        //            int numstudent = 0;
        //            double credit = 0;
        //            decimal sumfee = 0;
        //            foreach (string studCode in studentLessonCodeList.Keys)
        //            {
        //                if (studentnumcredits[studCode] > 0)
        //                {
        //                    numstudent++;
        //                    credit += studentnumcredits[studCode];
        //                    sumfee += studentFee[studCode];

        //                    //create Account transaction
        //                    AccountTransaction act = View.ObjectSpace.CreateObject<AccountTransaction>();
        //                    Student studnew = View.ObjectSpace.FindObject<Student>(
        //                        new BinaryOperator("StudentCode", studCode));
        //                    act.Student = studnew;
        //                    act.MoneyAmount = -studentFee[studCode];
        //                    act.TransactingDate = DateTime.Now;
        //                    act.Description = string.Format("Học phí sinh viên [{0}] phải đóng cho {1} tín chỉ đăng ký là {2:0,0},\r\n bao gồm: [{3}],\r\n [{4}] thực hiện đăng ký",
        //                        studnew.StudentCode, studentnumcredits[studCode], studentFee[studCode], studentLessonCodeList[studCode].TrimEnd('-'), SecuritySystem.CurrentUserName);
        //                    act.Save();
        //                }
        //            }

        //            View.ObjectSpace.CommitChanges();

        //            ms = objectSpace.CreateObject<PopUpMessage>();
        //            ms.Title = "Thực hiện đăng ký (Booked) thành công";
        //            ms.Message = string.Format("Bạn đã chọn đăng ký cho {0} sinh viên, với số tiền {1:0,0} cho tổng số {2:0,0} tín chỉ học",
        //                numstudent, sumfee, credit);
        //            ms.Message += "\r\n Vui lòng xem kết quả giao dịch học phí!!!";
        //            if (lessonCannotReg.Count > 0)
        //            {
        //                string strmessage = "";
        //                foreach (KeyValuePair<string, string> keypair in lessonCannotReg)
        //                {
        //                    strmessage += string.Format("Nhóm lớp MH:[{0}], sinh viên [{1}] \r\n", keypair.Key, keypair.Value.TrimEnd('-'));
        //                }
        //                ms.Message += "\r\nKhông đăng ký được do quá sĩ số:\r\n" + strmessage;
        //            }
        //            e.ShowViewParameters.CreatedView = Application.CreateDetailView(
        //                 objectSpace, ms);
        //            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //            e.ShowViewParameters.CreatedView.Caption = "Thông báo";
        //            dc = Application.CreateController<DialogController>();
        //            e.ShowViewParameters.Controllers.Add(dc);
        //            dc.AcceptAction.Active.SetItemValue("object", false);
        //            dc.CancelAction.Caption = "Đóng";
        //            dc.SaveOnAccept = false;
        //        }
        //    }
        //}
        //private void ConfirmRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{

        //    ListEditor listEditor = ((ListView)View).Editor as ListEditor;
        //    if (listEditor != null)
        //    {
        //        int i = 0;
        //        string messsage = "";
        //        ObjectSpace objectSpace = Application.CreateObjectSpace();
        //        PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
        //        ms.Message = messsage;
        //        DialogController dc = Application.CreateController<DialogController>();
        //        if (View.SelectedObjects.Count > 0)
        //        {
        //            foreach (RegisterDetail regDetail in ((ListView)View).SelectedObjects)
        //            {
        //                regDetail.RegisterState = View.ObjectSpace.FindObject<RegisterState>(
        //                        new BinaryOperator("Code", "PRINTED"));
        //                if (regDetail.CheckState.Code == "CHECKED")
        //                {
        //                    View.ObjectSpace.SetModified(regDetail);
        //                    regDetail.CheckState = View.ObjectSpace.FindObject<RegisterState>(
        //                        new BinaryOperator("Code", "CONFIRMED"));
        //                    i++;
        //                }
        //            }
        //            View.ObjectSpace.CommitChanges();
        //            messsage = string.Format("Đã xác nhận kết quả cho \"{0}\" Nhóm MH, \r\n Nhấn OK để in kết quả!", i);
        //            ms.Title = "Kết quả xác nhận!";
        //            dc.Accepting += dc_Accepting_Report;
        //            dc.SaveOnAccept = false;
        //        }
        //        else
        //        {
        //            messsage = "Vui lòng chọn tất cả các môn đã kiểm tra";
        //            ms.Title = "Chưa chọn môn xác nhận!";
        //        }


        //        e.ShowViewParameters.CreatedView = Application.CreateDetailView(
        //            objectSpace, ms);
        //        e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //        e.ShowViewParameters.CreatedView.Caption = "Thông báo";
        //        e.ShowViewParameters.Controllers.Add(dc);
        //    }
        //}
       
        #endregion

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
                Student student = objectSpace.FindObject<Student>(criteriaOperator);
                report.Filtering.Filter = criteriaOperator.ToString();
                MemoryStream stream = new MemoryStream();
                report.ExportToPdf(stream);
                if (HttpContext.Current != null)
                {


                    byte[] buffer = stream.GetBuffer();
                    string contentType = "application/pdf";
                    string contentDisposition = "attachment; filename="+ student.StudentCode +".pdf";
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
         
        //private void ViewDetailLessonTimetable_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    if (((ListView)View).ObjectTypeInfo.Type == typeof(RegisterDetail))
        //    {
        //        ListEditor listEditor = ((ListView)View).Editor as ListEditor;

        //        ObjectSpace objectSpace = Application.CreateObjectSpace();
        //        CollectionSource collectionSource =
        //            new CollectionSource(objectSpace, typeof(TkbSemester));
        //        if ((collectionSource.Collection as XPCollection) != null)
        //        {
        //            ((XPCollection)collectionSource.Collection).LoadingEnabled = false;
        //        }

        //        if (listEditor != null)
        //        {
        //            foreach (RegisterDetail regDetail in View.SelectedObjects)
        //            {
        //                foreach (TkbSemester tkbsem in regDetail.Lesson.TKBSemesters)
        //                {
        //                    TkbSemester lview = objectSpace.FindObject<TkbSemester>(
        //                        new BinaryOperator("Oid", tkbsem.Oid));
        //                    ((XPCollection)collectionSource.Collection).Add(lview);
        //                }
        //            }
        //            ListView view = Application.CreateListView(
        //                Application.GetListViewId(typeof(TkbSemester)),
        //                collectionSource, true);
        //            view.Editor.AllowEdit = false;


        //            e.View = view;
        //            e.DialogController.SaveOnAccept = false;
        //        }
        //    }
        //    else if (((ListView)View).ObjectTypeInfo.Type == typeof(Lesson))
        //    {
        //        ListEditor listEditor = ((ListView)View).Editor as ListEditor;

        //        ObjectSpace objectSpace = Application.CreateObjectSpace();
        //        CollectionSource collectionSource =
        //            new CollectionSource(objectSpace, typeof(TkbSemester));
        //        if ((collectionSource.Collection as XPCollection) != null)
        //        {
        //            ((XPCollection)collectionSource.Collection).LoadingEnabled = false;
        //        }

        //        if (listEditor != null)
        //        {

        //            foreach (Lesson lesson in ((ProxyCollection)((ListView)View).CollectionSource.Collection))
        //            {

        //                foreach (TkbSemester tkbsem in lesson.TKBSemesters)
        //                {
        //                    TkbSemester lview = objectSpace.FindObject<TkbSemester>(
        //                        new BinaryOperator("Oid", tkbsem.Oid));

        //                    ((XPCollection)collectionSource.Collection).Add(lview);
        //                }
        //            }
        //            ListView view = Application.CreateListView(
        //                Application.GetListViewId(typeof(TkbSemester)),
        //                collectionSource, false);
        //            view.Editor.AllowEdit = false;


        //            //ASPxGridListEditor gc = (ASPxGridListEditor)view.Editor;
        //            //gc.ControlsCreated += new EventHandler(gc_ControlsCreated);
        //            e.View = view;
        //            e.DialogController.SaveOnAccept = false;

        //        }


        //    }

        //}

        Dictionary<string, List<string>> dicStudentRegDetail;
        List<Vacancy> listVacancies;

        private void DefaultRegister_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();

            dicStudentRegDetail = new Dictionary<string, List<string>>();
            Student currentStudent;
            Lesson curLesson;
            Dictionary<string, List<string>> errorstudent = new Dictionary<string, List<string>>();
            Dictionary<string, int> dicLessonCurrentRegNum = new Dictionary<string, int>();
            int numregok = 0;
            Vacancy vc;
            bool isConflictTKB = false;
            using (XPCollection<Lesson> newCollectionSource = new XPCollection<Lesson>(objectSpace.Session))
            {
                objectSpace.Session.BeginTransaction();
                foreach (StudentClass studentClass in View.SelectedObjects)
                {
                    newCollectionSource.Criteria = CriteriaOperator.Parse(
                        "ClassIDs like ?", string.Format("%{0}%", studentClass.ClassCode));

                    foreach (Student student in studentClass.Students)
                    {
                        listVacancies = new List<Vacancy>();
                        currentStudent = objectSpace.FindObject<Student>(
                        new BinaryOperator("StudentCode", student.StudentCode));

                        foreach (Lesson lesson in newCollectionSource)
                        {
                            isConflictTKB = false;
                            if (!dicLessonCurrentRegNum.ContainsKey(lesson.LessonName))
                            {
                                dicLessonCurrentRegNum[lesson.LessonName] = 0;
                            }
                            foreach (TkbSemester tkbsem in lesson.TKBSemesters)
                            {
                                vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                                if (Utils.IsConfictTKB(listVacancies, vc))
                                {
                                    isConflictTKB = true;
                                    break;
                                }
                            }

                            if (isConflictTKB)
                            {
                                if (!errorstudent.ContainsKey(currentStudent.StudentCode))
                                    errorstudent.Add(currentStudent.StudentCode, new List<string>());
                                if (!errorstudent[currentStudent.StudentCode].Contains(lesson.Subject.SubjectCode))
                                    errorstudent[currentStudent.StudentCode].Add(lesson.Subject.SubjectCode + "{T}");
                            }
                            else
                            {
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
                                            errorstudent[currentStudent.StudentCode].Add(curLesson.Subject.SubjectCode + "{D}");
                                        regdetail.Delete();
                                        //regdetail.Reload();
                                        
                                    }
                                    else
                                    {
                                        numregok++;
                                        if (!dicStudentRegDetail.ContainsKey(student.StudentCode))
                                            dicStudentRegDetail.Add(student.StudentCode,new List<string>());
                                        dicStudentRegDetail[student.StudentCode].Add(curLesson.LessonName);

                                        dicLessonCurrentRegNum[lesson.LessonName]++;
                                        foreach (TkbSemester tkbsem in curLesson.TKBSemesters)
                                        {
                                            vc = new Vacancy(tkbsem.Day, tkbsem.Period, tkbsem.Weeks, (tkbsem.Classroom == null ? "" : tkbsem.Classroom.ClassroomCode));
                                            listVacancies.Add(vc);
                                        }
                                        regdetail.Save();
                                    }
                                }
                                else
                                {
                                    if (!errorstudent.ContainsKey(currentStudent.StudentCode))
                                        errorstudent.Add(currentStudent.StudentCode, new List<string>());
                                    if (!errorstudent[currentStudent.StudentCode].Contains(lesson.Subject.SubjectCode))
                                        errorstudent[currentStudent.StudentCode].Add(lesson.Subject.SubjectCode + "{H}");
                                }
                            }
                        }
                    }
                }
                objectSpace.Session.CommitTransaction();
                PopUpMessage ms = objectSpace.CreateObject<PopUpMessage>();
                if (errorstudent.Count > 0)
                {
                    ms.Title = "Có lỗi khi chọn nhóm MH đăng ký!";
                    ms.Message = string.Format("Đã chọn được cho {0} sinh viên với {1} lượt nhóm MH\r\n", dicStudentRegDetail.Count, numregok);
                    string strmessage = "Không chọn được nhóm môn học do trùng môn đã đăng ký, trùng lịch hoặc hết chỗ: ";
                    foreach (KeyValuePair<string, List<string>> keypair in errorstudent)
                    {
                        strmessage += string.Format("Sinh viên:[{0}] - Môn:[", keypair.Key);
                        foreach (string str in keypair.Value)
                        {
                            strmessage += str + ",";
                        }
                        strmessage = strmessage.TrimEnd(',');
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
