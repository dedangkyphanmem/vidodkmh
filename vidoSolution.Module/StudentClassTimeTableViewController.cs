using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.SystemModule;
using vidoSolution.Module.DomainObject;
using DevExpress.ExpressApp.Reports;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using vidoSolution.Module.Utilities;
using System.Collections;

namespace vidoSolution.Module
{
    public partial class StudentClassTimeTableViewController : ViewController
    {
        public StudentClassTimeTableViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        void StudentClassTimeTableViewController_Activated(object sender, System.EventArgs e)
        {

        }
        void ViewStudentClassTimetableAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {

            ObjectSpace objectSpace = Application.CreateObjectSpace();
            DialogController selectSemesterAcception;


            ListView lvSemester = Application.CreateListView(objectSpace, typeof(Semester), true);
            lvSemester.Editor.AllowEdit = false;

            e.ShowViewParameters.CreatedView = lvSemester;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.CreateAllControllers = true;

            selectSemesterAcception = new DialogController();
            e.ShowViewParameters.Controllers.Add(selectSemesterAcception);
            selectSemesterAcception.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(selectSemesterAcception_Accepting);
            selectSemesterAcception.AcceptAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            selectSemesterAcception.AcceptAction.TargetObjectType = typeof(Semester);
            selectSemesterAcception.AcceptAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            selectSemesterAcception.AcceptAction.TargetViewType = ViewType.ListView;
            selectSemesterAcception.AcceptAction.Caption = "Chọn";
            selectSemesterAcception.CancelAction.Caption = "Bỏ qua";
        }

        void selectSemesterAcception_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {

            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
            GroupOperator classroomsOp = new GroupOperator(CriteriaOperator.Parse("1=0"));
            ContainsOperator biOperator;
            string strParse = "";
            if (semester != null)
            {

                foreach (StudentClass studentClass in View.SelectedObjects)
                {
                    Data.CreateStudentClassTimetableData(objectSpace, studentClass.ClassCode, semester.SemesterName);

                    biOperator = new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClass.ClassCode));
                    classroomsOp = new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);

                    strParse += (strParse == "" ? string.Format("ClassCode='{0}'", studentClass.ClassCode) :
                        string.Format(" or ClassCode='{0}'", studentClass.ClassCode));
                }

                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Lịch lớp biên chế"));

                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }


        void ViewStudentTransactionTrackingAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            DialogController selectSemesterAcception;


            ListView lvSemester = Application.CreateListView(objectSpace, typeof(Semester), true);
            lvSemester.Editor.AllowEdit = false;

            e.ShowViewParameters.CreatedView = lvSemester;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.CreateAllControllers = true;

            selectSemesterAcception = new DialogController();
            e.ShowViewParameters.Controllers.Add(selectSemesterAcception);
            selectSemesterAcception.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(selectSemesterForTransactionTrackingAcception_Accepting);
            selectSemesterAcception.AcceptAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            selectSemesterAcception.AcceptAction.TargetObjectType = typeof(Semester);
            selectSemesterAcception.AcceptAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            selectSemesterAcception.AcceptAction.TargetViewType = ViewType.ListView;
            selectSemesterAcception.AcceptAction.Caption = "Chọn";
            selectSemesterAcception.CancelAction.Caption = "Bỏ qua";
        }


        void selectSemesterForTransactionTrackingAcception_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {

            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
            //GroupOperator classroomsOp = new GroupOperator(CriteriaOperator.Parse("1=0"));
            //ContainsOperator biOperator;
            string strParse = "";
            if (semester != null)
            {

                foreach (StudentClass studentClass in View.SelectedObjects)
                {
                    Data.CreateStudentClassTransactionTrackingData(objectSpace, studentClass.ClassCode, semester.SemesterName);

                    //biOperator = new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClass.ClassCode));
                    //classroomsOp = new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);

                    strParse += (strParse == "" ? string.Format("Student.StudentClass.ClassCode='{0}'", studentClass.ClassCode) :
                        string.Format(" or Student.StudentClass.ClassCode='{0}'", studentClass.ClassCode));
                }

                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Tỉ lệ nợ sinh viên lớp biên chế"));
                
                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }

        void ViewClassTransactionTrackingAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            

            if (View.SelectedObjects.Count > 0)
            {
                DialogController selectSemesterAcception;
                ListView lvSemester = Application.CreateListView(objectSpace, typeof(Semester), true);
                lvSemester.Editor.AllowEdit = false;

                e.ShowViewParameters.CreatedView = lvSemester;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.CreateAllControllers = true;

                selectSemesterAcception = new DialogController();
                e.ShowViewParameters.Controllers.Add(selectSemesterAcception);
                selectSemesterAcception.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(selectSemesterForClassTransactionTrackingAcception_Accepting);
                selectSemesterAcception.AcceptAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
                selectSemesterAcception.AcceptAction.TargetObjectType = typeof(Semester);
                selectSemesterAcception.AcceptAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
                selectSemesterAcception.AcceptAction.TargetViewType = ViewType.ListView;
                selectSemesterAcception.AcceptAction.Caption = "Chọn";
                selectSemesterAcception.CancelAction.Caption = "Bỏ qua";
            }
            else
            {
                PopUpMessage ms;
                DialogController dc;
                ms = objectSpace.CreateObject<PopUpMessage>();
                ms.Title = "Thông báo";
                ms.Message = string.Format("Vui lòng chọn ít nhất một lớp biên chế để xem kết quả tổng hợp học phí");
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
        void selectSemesterForClassTransactionTrackingAcception_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
            //GroupOperator classroomsOp = new GroupOperator(CriteriaOperator.Parse("1=0"));
            //ContainsOperator biOperator;
            string strParse = "";
            if (semester != null)
            {
                //NestedObjectSpace nobj= objectSpace.CreateNestedObjectSpace();
                foreach (StudentClass studentClass in View.SelectedObjects)
                {

                    ClassTransactionTracking ct = objectSpace.FindObject<ClassTransactionTracking>
                        (CriteriaOperator.Parse("StudentClass.ClassCode = ? and Semester.SemesterName = ?",
                        studentClass.ClassCode, semester.SemesterName));
                    if (ct == null)
                    {
                        ct = objectSpace.CreateObject<ClassTransactionTracking>();
                        ct.Semester = objectSpace.FindObject<Semester>(CriteriaOperator.Parse("SemesterName=?", semester.SemesterName));
                        ct.StudentClass = objectSpace.FindObject<StudentClass>(CriteriaOperator.Parse("ClassCode=?", studentClass.ClassCode));
                        ct.Save();
                    }
                    //biOperator = new ContainsOperator("StudentClasses", new BinaryOperator("ClassCode", studentClass.ClassCode));
                    //classroomsOp = new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);

                    strParse += (strParse == "" ? string.Format("StudentClass.ClassCode='{0}'", studentClass.ClassCode) :
                        string.Format(" or StudentClass.ClassCode='{0}'", studentClass.ClassCode));
                }
                objectSpace.CommitChanges();
                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Tỉ lệ nợ lớp biên chế NHHK"));
                strParse = string.Format("({0}) and Semester.SemesterName= '{1}'", strParse, semester.SemesterName);

                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }
    }
}
