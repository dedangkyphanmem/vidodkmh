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
    public partial class ClassroomTimeTableViewController : ViewController
    {
        public ClassroomTimeTableViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
     
        void ClassroomTimeTableViewController_Activated(object sender, System.EventArgs e)
        {
            
        }
        void ViewClassroomTimetableAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
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
           
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            ListView lv = ((ListView)((WindowController)sender).Window.View);
            Semester semester = lv.SelectedObjects.Count == 0 ? null : lv.SelectedObjects[0] as Semester;
            GroupOperator classroomsOp=new GroupOperator(CriteriaOperator.Parse("1=0"));
            ContainsOperator biOperator;
            string strParse = "";
            if (semester != null)
            {
               
                foreach (Classroom classroom in View.SelectedObjects)
                {
                    Data.CreateClassroomTimetableData(objectSpace, classroom.ClassroomCode, semester.SemesterName);
                    
                    biOperator =new ContainsOperator("Classrooms", new BinaryOperator("ClassroomCode", classroom.ClassroomCode));
                    classroomsOp= new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);
                    
                    strParse += (strParse == "" ? string.Format("ClassroomCode='{0}'", classroom.ClassroomCode) :
                        string.Format("or ClassroomCode='{0}'", classroom.ClassroomCode));
                }

                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Lịch phòng học"));
               
                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }

      
       

    }
}
