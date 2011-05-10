namespace vidoSolution.Module
{
    partial class LessonViewController
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
            this.SetDisableSelectAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SetEnableSelectAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SetDisableSelectAction
            // 
            this.SetDisableSelectAction.Caption = "Set Disable Select Action";
            this.SetDisableSelectAction.Category = "View";
            this.SetDisableSelectAction.ConfirmationMessage = "Really want to Disable CanRegister for chosen Lesson?";
            this.SetDisableSelectAction.Id = "SetDisableSelectAction";
            this.SetDisableSelectAction.ImageName = null;
            this.SetDisableSelectAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.SetDisableSelectAction.Shortcut = null;
            this.SetDisableSelectAction.Tag = null;
            this.SetDisableSelectAction.TargetObjectsCriteria = null;
            this.SetDisableSelectAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.SetDisableSelectAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Lesson);
            this.SetDisableSelectAction.TargetViewId = "Lesson_ListView";
            this.SetDisableSelectAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SetDisableSelectAction.ToolTip = "Set Disable Select Action";
            this.SetDisableSelectAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SetDisableSelectAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetDisableSelectAction_Execute);
            // 
            // SetEnableSelectAction
            // 
            this.SetEnableSelectAction.Caption = "Set Enable Select Action";
            this.SetEnableSelectAction.Category = "View";
            this.SetEnableSelectAction.ConfirmationMessage = "Really want to Set Enable CanRegister for Lesson";
            this.SetEnableSelectAction.Id = "SetEnableSelectAction";
            this.SetEnableSelectAction.ImageName = null;
            this.SetEnableSelectAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.SetEnableSelectAction.Shortcut = null;
            this.SetEnableSelectAction.Tag = null;
            this.SetEnableSelectAction.TargetObjectsCriteria = null;
            this.SetEnableSelectAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.SetEnableSelectAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Lesson);
            this.SetEnableSelectAction.TargetViewId = "Lesson_ListView";
            this.SetEnableSelectAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SetEnableSelectAction.ToolTip = "Set Enable Select Action";
            this.SetEnableSelectAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SetEnableSelectAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetEnableSelectAction_Execute);
       
            // 
            // ClassroomTimeTableViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.Lesson);
            this.TargetViewId = "Lesson_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.LessonViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction SetDisableSelectAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SetEnableSelectAction;
        #endregion
    }
}
