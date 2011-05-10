using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class StudentResultFileViewController
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
            this.ImportStudentResultAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportTransactionDataAction
            // 
            this.ImportStudentResultAction.Caption = "Import Student Result";
            this.ImportStudentResultAction.Category = "View";
            this.ImportStudentResultAction.ConfirmationMessage = null;
            this.ImportStudentResultAction.Id = "ImportStudentResultAction";
            this.ImportStudentResultAction.ImageName = null;
            this.ImportStudentResultAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportStudentResultAction.Shortcut = null;
            this.ImportStudentResultAction.Tag = null;
            this.ImportStudentResultAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportStudentResultAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportStudentResultAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentResultFile);
            this.ImportStudentResultAction.TargetViewId = "StudentResultFile_ListView";
            this.ImportStudentResultAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportStudentResultAction.ToolTip = "Import Student Result";
            this.ImportStudentResultAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportStudentResultAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportStudentResultAction_Execute);
                       // 
            // AccountTransactionFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentResultFile);
            this.TargetViewId = "StudentResultFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.StudentResultFileViewController_Activated);

        }




        private DevExpress.ExpressApp.Actions.SimpleAction ImportStudentResultAction;
        #endregion
    }
}
