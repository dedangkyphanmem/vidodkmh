namespace vidoSolution.Module
{
    partial class TeacherTimeTableViewController
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
            this.ViewTeacherTimetableAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewTeacherTimetableAction
            // 
            this.ViewTeacherTimetableAction.Caption = "View Teacher Timetable";
            this.ViewTeacherTimetableAction.Category = "View";
            this.ViewTeacherTimetableAction.ConfirmationMessage = null;
            this.ViewTeacherTimetableAction.Id = "ViewTeacherTimetableAction";
            this.ViewTeacherTimetableAction.ImageName = null;
            this.ViewTeacherTimetableAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewTeacherTimetableAction.Shortcut = null;
            this.ViewTeacherTimetableAction.Tag = null;
            this.ViewTeacherTimetableAction.TargetObjectsCriteria = null;
            this.ViewTeacherTimetableAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewTeacherTimetableAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Teacher);
            this.ViewTeacherTimetableAction.TargetViewId = "Teacher_ListView";
            this.ViewTeacherTimetableAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewTeacherTimetableAction.ToolTip = "View Teacher Timetable";
            this.ViewTeacherTimetableAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewTeacherTimetableAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewTeacherTimetableAction_Execute);
            // 
            // TeacherTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Teacher);
            this.TargetViewId = "Teacher_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.TeacherTimeTableViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewTeacherTimetableAction;
        #endregion
    }
}
