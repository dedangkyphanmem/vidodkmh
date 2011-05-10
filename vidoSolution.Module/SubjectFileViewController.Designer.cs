using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class SubjectFileViewController
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
            this.ImportSubjectAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportTransactionDataAction
            // 
            this.ImportSubjectAction.Caption = "Import Subject";
            this.ImportSubjectAction.Category = "View";
            this.ImportSubjectAction.ConfirmationMessage = null;
            this.ImportSubjectAction.Id = "ImportSubjectAction";
            this.ImportSubjectAction.ImageName = null;
            this.ImportSubjectAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportSubjectAction.Shortcut = null;
            this.ImportSubjectAction.Tag = null;
            this.ImportSubjectAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportSubjectAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportSubjectAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.SubjectFile);
            this.ImportSubjectAction.TargetViewId = "SubjectFile_ListView";
            this.ImportSubjectAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportSubjectAction.ToolTip = "Import Subject";
            this.ImportSubjectAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportSubjectAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportSubjectAction_Execute);
                       // 
            // AccountTransactionFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.SubjectFile);
            this.TargetViewId = "SubjectFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.SubjectFileViewController_Activated);

        }

        private DevExpress.ExpressApp.Actions.SimpleAction ImportSubjectAction;
        #endregion
    }
}
