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
    public partial class TeacherTimeTableViewController : ViewController
    {
        public TeacherTimeTableViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
      
        void TeacherTimeTableViewController_Activated(object sender, System.EventArgs e)
        {
            
        }
        void ViewTeacherTimetableAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
          
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            DialogController selectSemesterAcception;
        
            ListView lvSemester = Application.CreateListView(objectSpace,typeof(Semester),true);
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
            //Dictionary<string, WeekReportData> dicTeacherPeriodData;
            //WeekReportData currentWeekData;
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
            if (semester != null)
            {
                string strParse = "";
                foreach (Teacher teacher in View.SelectedObjects)
                {
                    Data.CreateTeacherTimetableData(objectSpace, teacher.TeacherCode, semester.SemesterName);



                    strParse += (strParse == "" ? string.Format("TeacherCode='{0}'", teacher.TeacherCode) :
                        string.Format("or TeacherCode='{0}'", teacher.TeacherCode));
                }
                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Lịch giảng viên"));

                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse));
            }
        } 
    }
}
