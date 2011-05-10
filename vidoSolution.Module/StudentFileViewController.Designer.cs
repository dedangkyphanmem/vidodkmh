using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class StudentFileViewController
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
            this.ImportStudentAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportTransactionDataAction
            // 
            this.ImportStudentAction.Caption = "Import Student";
            this.ImportStudentAction.Category = "View";
            this.ImportStudentAction.ConfirmationMessage = null;
            this.ImportStudentAction.Id = "ImportStudentAction";
            this.ImportStudentAction.ImageName = null;
            this.ImportStudentAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportStudentAction.Shortcut = null;
            this.ImportStudentAction.Tag = null;
            this.ImportStudentAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportStudentAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportStudentAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentFile);
            this.ImportStudentAction.TargetViewId = "StudentFile_ListView";
            this.ImportStudentAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportStudentAction.ToolTip = "Import Student";
            this.ImportStudentAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportStudentAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportStudentAction_Execute);
                       // 
            // AccountTransactionFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.StudentFile);
            this.TargetViewId = "StudentFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.StudentResultFileViewController_Activated);

        }




        private DevExpress.ExpressApp.Actions.SimpleAction ImportStudentAction;
        #endregion
    }
}
