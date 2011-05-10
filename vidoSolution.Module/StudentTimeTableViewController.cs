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
    public partial class StudentTimeTableViewController : ViewController
    {
        public StudentTimeTableViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        void StudentTimeTableViewController_Activated(object sender, System.EventArgs e)
        {
            
        }
        void ViewStudentTimetableAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
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
               
                foreach (Student student in View.SelectedObjects)
                {
                    Data.CreateStudentTimetableData(objectSpace, student.StudentCode, semester.SemesterName);

                    biOperator = new ContainsOperator("Students", new BinaryOperator("StudentCode", student.StudentCode));
                    classroomsOp= new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);

                    strParse += (strParse == "" ? string.Format("StudentCode='{0}'", student.StudentCode) :
                        string.Format(" or StudentCode='{0}'", student.StudentCode));
                }

                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Lịch sinh viên"));
               
                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }

      
       

    }
}
