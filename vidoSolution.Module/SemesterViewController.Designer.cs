namespace vidoSolution.Module
{
    partial class SemesterViewController
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
            this.ViewSemesterClassTrackingAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewClassroomTimetableAction
            // 
            this.ViewSemesterClassTrackingAction.Caption = "View Class Transaction Tracking";
            this.ViewSemesterClassTrackingAction.Category = "View";
            this.ViewSemesterClassTrackingAction.ConfirmationMessage = null;
            this.ViewSemesterClassTrackingAction.Id = "ViewSemesterClassTransactionTrackingAction";
            this.ViewSemesterClassTrackingAction.ImageName = null;
            this.ViewSemesterClassTrackingAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewSemesterClassTrackingAction.Shortcut = null;
            this.ViewSemesterClassTrackingAction.Tag = null;
            this.ViewSemesterClassTrackingAction.TargetObjectsCriteria = null;
            this.ViewSemesterClassTrackingAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewSemesterClassTrackingAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Semester);
            this.ViewSemesterClassTrackingAction.TargetViewId = "Semester_ListView";
            this.ViewSemesterClassTrackingAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewSemesterClassTrackingAction.ToolTip = "View  Class Transaction Tracking";
            this.ViewSemesterClassTrackingAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewSemesterClassTrackingAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewClassTrackingAction_Execute);
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Semester);
            this.TargetViewId = "Semester_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.SemesterViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewSemesterClassTrackingAction;
        #endregion
    }
}
