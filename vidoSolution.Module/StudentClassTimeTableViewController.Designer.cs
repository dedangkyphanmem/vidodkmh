namespace vidoSolution.Module
{
    partial class StudentClassTimeTableViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ViewStudentTransactionTrackingAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ViewStudentClassTimetableAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ViewClassTransactionTrackingAction= new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewStudentTransactionTrackingAction
            // 
            this.ViewStudentTransactionTrackingAction.Caption = "View StudentClass Transaction Tracking";
            this.ViewStudentTransactionTrackingAction.Category = "View";
            this.ViewStudentTransactionTrackingAction.ConfirmationMessage = null;
            this.ViewStudentTransactionTrackingAction.Id = "ViewStudentTransactionTrackingAction";
            this.ViewStudentTransactionTrackingAction.ImageName = null;
            this.ViewStudentTransactionTrackingAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewStudentTransactionTrackingAction.Shortcut = null;
            this.ViewStudentTransactionTrackingAction.Tag = null;
            this.ViewStudentTransactionTrackingAction.TargetObjectsCriteria = null;
            this.ViewStudentTransactionTrackingAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewStudentTransactionTrackingAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentClass);
            this.ViewStudentTransactionTrackingAction.TargetViewId = "StudentClass_ListView";
            this.ViewStudentTransactionTrackingAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewStudentTransactionTrackingAction.ToolTip = "View StudentClass Transaction Tracking";
            this.ViewStudentTransactionTrackingAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewStudentTransactionTrackingAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewStudentTransactionTrackingAction_Execute);
                        
            // 
            // ViewClassTransactionTrackingAction
            // 
            this.ViewClassTransactionTrackingAction.Caption = "View Class Transaction Tracking";
            this.ViewClassTransactionTrackingAction.Category = "View";
            this.ViewClassTransactionTrackingAction.ConfirmationMessage = null;
            this.ViewClassTransactionTrackingAction.Id = "ViewClassTransactionTrackingAction";
            this.ViewClassTransactionTrackingAction.ImageName = null;
            this.ViewClassTransactionTrackingAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;
            this.ViewClassTransactionTrackingAction.Shortcut = null;
            this.ViewClassTransactionTrackingAction.Tag = null;
            this.ViewClassTransactionTrackingAction.TargetObjectsCriteria = null;
            this.ViewClassTransactionTrackingAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewClassTransactionTrackingAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentClass);
            this.ViewClassTransactionTrackingAction.TargetViewId = "StudentClass_ListView";
            this.ViewClassTransactionTrackingAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewClassTransactionTrackingAction.ToolTip = "View Class Transaction Tracking";
            this.ViewClassTransactionTrackingAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewClassTransactionTrackingAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewClassTransactionTrackingAction_Execute);

            // 
            // ViewClassroomTimetableAction
            // 
            this.ViewStudentClassTimetableAction.Caption = "View StudentClass Timetable";
            this.ViewStudentClassTimetableAction.Category = "View";
            this.ViewStudentClassTimetableAction.ConfirmationMessage = null;
            this.ViewStudentClassTimetableAction.Id = "ViewStudentClassTimetableAction";
            this.ViewStudentClassTimetableAction.ImageName = null;
            this.ViewStudentClassTimetableAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewStudentClassTimetableAction.Shortcut = null;
            this.ViewStudentClassTimetableAction.Tag = null;
            this.ViewStudentClassTimetableAction.TargetObjectsCriteria = null;
            this.ViewStudentClassTimetableAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewStudentClassTimetableAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentClass);
            this.ViewStudentClassTimetableAction.TargetViewId = "StudentClass_ListView";
            this.ViewStudentClassTimetableAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewStudentClassTimetableAction.ToolTip = "View StudentClass Timetable";
            this.ViewStudentClassTimetableAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewStudentClassTimetableAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewStudentClassTimetableAction_Execute);
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentClass);
            this.TargetViewId = "StudentClass_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.StudentClassTimeTableViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewStudentClassTimetableAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ViewStudentTransactionTrackingAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ViewClassTransactionTrackingAction;
        #endregion
    }
}
