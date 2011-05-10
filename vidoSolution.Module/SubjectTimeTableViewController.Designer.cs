namespace vidoSolution.Module
{
    partial class SubjectTimeTableViewController
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
            this.ViewSubjectTimetableAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewClassroomTimetableAction
            // 
            this.ViewSubjectTimetableAction.Caption = "View Subject Timetable";
            this.ViewSubjectTimetableAction.Category = "View";
            this.ViewSubjectTimetableAction.ConfirmationMessage = null;
            this.ViewSubjectTimetableAction.Id = "ViewSubjectTimetableAction";
            this.ViewSubjectTimetableAction.ImageName = null;
            this.ViewSubjectTimetableAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewSubjectTimetableAction.Shortcut = null;
            this.ViewSubjectTimetableAction.Tag = null;
            this.ViewSubjectTimetableAction.TargetObjectsCriteria = null;
            this.ViewSubjectTimetableAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewSubjectTimetableAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Subject);
            this.ViewSubjectTimetableAction.TargetViewId = "Subject_ListView";
            this.ViewSubjectTimetableAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewSubjectTimetableAction.ToolTip = "View Subject Timetable";
            this.ViewSubjectTimetableAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewSubjectTimetableAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewSubjectTimetableAction_Execute);
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Subject);
            this.TargetViewId = "Subject_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.SubjectTimeTableViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewSubjectTimetableAction;
        #endregion
    }
}
