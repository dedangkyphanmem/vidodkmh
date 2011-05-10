namespace vidoSolution.Module
{
    partial class StudentTimeTableViewController
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
            this.ViewStudentTimetableAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewClassroomTimetableAction
            // 
            this.ViewStudentTimetableAction.Caption = "View Student Timetable";
            this.ViewStudentTimetableAction.Category = "View";
            this.ViewStudentTimetableAction.ConfirmationMessage = null;
            this.ViewStudentTimetableAction.Id = "ViewStudentTimetableAction";
            this.ViewStudentTimetableAction.ImageName = null;
            this.ViewStudentTimetableAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ViewStudentTimetableAction.Shortcut = null;
            this.ViewStudentTimetableAction.Tag = null;
            this.ViewStudentTimetableAction.TargetObjectsCriteria = null;
            this.ViewStudentTimetableAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ViewStudentTimetableAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Student);
            this.ViewStudentTimetableAction.TargetViewId = "Student_ListView";
            this.ViewStudentTimetableAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ViewStudentTimetableAction.ToolTip = "View Student Timetable";
            this.ViewStudentTimetableAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ViewStudentTimetableAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewStudentTimetableAction_Execute);
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Student);
            this.TargetViewId = "Student_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.StudentTimeTableViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction ViewStudentTimetableAction;
        #endregion
    }
}
