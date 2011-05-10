namespace vidoSolution.Module
{
    partial class ClassroomTimeTableViewController
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
            this.ViewClassroomTimetableAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewClassroomTimetableAction
            // 
            this.ViewClassroomTimetableAction.Caption = "View Classroom Timetable";
            this.ViewClassroomTimetableAction.Category = "View";
            this.ViewClassroomTimetableAction.ConfirmationMessage = null;
            this.ViewClassroomTimetableAction.Id = "ViewClassroomTimetableAction";
            this.ViewClassroomTimetableAction.ImageName = null;
            this.ViewClassroomTimetableAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewClassroomTimetableAction.Shortcut = null;
            this.ViewClassroomTimetableAction.Tag = null;
            this.ViewClassroomTimetableAction.TargetObjectsCriteria = null;
            this.ViewClassroomTimetableAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewClassroomTimetableAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Classroom);
            this.ViewClassroomTimetableAction.TargetViewId = "Classroom_ListView";
            this.ViewClassroomTimetableAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewClassroomTimetableAction.ToolTip = "View Classroom Timetable";
            this.ViewClassroomTimetableAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewClassroomTimetableAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewClassroomTimetableAction_Execute);
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Classroom);
            this.TargetViewId = "Classroom_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.ClassroomTimeTableViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewClassroomTimetableAction;
        #endregion
    }
}
