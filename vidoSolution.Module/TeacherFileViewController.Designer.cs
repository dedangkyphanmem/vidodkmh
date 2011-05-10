using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class TeacherFileViewController
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
            this.ImportTeacherAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportTransactionDataAction
            // 
            this.ImportTeacherAction.Caption = "Import Teacher";
            this.ImportTeacherAction.Category = "View";
            this.ImportTeacherAction.ConfirmationMessage = null;
            this.ImportTeacherAction.Id = "ImportTeacherAction";
            this.ImportTeacherAction.ImageName = null;
            this.ImportTeacherAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportTeacherAction.Shortcut = null;
            this.ImportTeacherAction.Tag = null;
            this.ImportTeacherAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportTeacherAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportTeacherAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.TeacherFile);
            this.ImportTeacherAction.TargetViewId = "TeacherFile_ListView";
            this.ImportTeacherAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportTeacherAction.ToolTip = "Import Teacher";
            this.ImportTeacherAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportTeacherAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportTeacherAction_Execute);
                       // 
            // AccountTransactionFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.TeacherFile);
            this.TargetViewId = "TeacherFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.TeacherFileViewController_Activated);

        }

        private DevExpress.ExpressApp.Actions.SimpleAction ImportTeacherAction;
        #endregion
    }
}
