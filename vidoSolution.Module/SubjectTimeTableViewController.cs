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
    public partial class SubjectTimeTableViewController : ViewController
    {
        public SubjectTimeTableViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        void SubjectTimeTableViewController_Activated(object sender, System.EventArgs e)
        {
            
        }
        void ViewSubjectTimetableAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
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

                foreach (Subject subject in View.SelectedObjects)
                {
                    Data.CreateSubjectTimetableData(objectSpace, subject.SubjectCode, semester.SemesterName);

                    biOperator = new ContainsOperator("Subjects", new BinaryOperator("SubjectCode", subject.SubjectCode));
                    classroomsOp= new GroupOperator(GroupOperatorType.Or, classroomsOp, biOperator);

                    strParse += (strParse == "" ? string.Format("SubjectCode='{0}'", subject.SubjectCode) :
                        string.Format(" or SubjectCode='{0}'", subject.SubjectCode));
                }

                ReportData reportData = objectSpace.FindObject<ReportData>(
                    new BinaryOperator("Name", "Lịch môn học"));
               
                ReportServiceController rsc = ((WindowController)sender).Frame.GetController<ReportServiceController>();
                rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); //classroomsOp);
            }
        }

      
       

    }
}
