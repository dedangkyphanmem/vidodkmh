using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class SubjectRelationFileViewController
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
            this.ImportSubjectRelationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportSubjectRelationAction
            // 
            this.ImportSubjectRelationAction.Caption = "Import Subject Relation";
            this.ImportSubjectRelationAction.Category = "View";
            this.ImportSubjectRelationAction.ConfirmationMessage = null;
            this.ImportSubjectRelationAction.Id = "ImportSubjectRelationAction";
            this.ImportSubjectRelationAction.ImageName = null;
            this.ImportSubjectRelationAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportSubjectRelationAction.Shortcut = null;
            this.ImportSubjectRelationAction.Tag = null;
            this.ImportSubjectRelationAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportSubjectRelationAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportSubjectRelationAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.SubjectRelationFile);
            this.ImportSubjectRelationAction.TargetViewId = "SubjectRelationFile_ListView";
            this.ImportSubjectRelationAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportSubjectRelationAction.ToolTip = "Import Subject Relation";
            this.ImportSubjectRelationAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportSubjectRelationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportSubjectRelationAction_Execute);
            // 
            // SubjectRelationFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.SubjectRelationFile);
            this.TargetViewId = "SubjectRelationFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.SubjectRelationFileViewController_Activated);

        }

        private DevExpress.ExpressApp.Actions.SimpleAction ImportSubjectRelationAction;
        #endregion
    }
}
