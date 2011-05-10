using vidoSolution.Module.DomainObject;
namespace vidoSolution.Module
{
    partial class AccountTransactionFileViewController
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
            this.ImportTransactionDataAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
             // 
            // ImportTransactionDataAction
            // 
            this.ImportTransactionDataAction.Caption = "Import TransactionData ";
            this.ImportTransactionDataAction.Category = "View";
            this.ImportTransactionDataAction.ConfirmationMessage = null;
            this.ImportTransactionDataAction.Id = "ImportTransactionDataAction";
            this.ImportTransactionDataAction.ImageName = null;
            this.ImportTransactionDataAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImportTransactionDataAction.Shortcut = null;
            this.ImportTransactionDataAction.Tag = null;
            this.ImportTransactionDataAction.TargetObjectsCriteria = "IsImported = false";
            this.ImportTransactionDataAction.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.ImportTransactionDataAction.TargetObjectType = typeof(vidoSolution.Module.DomainObject.AccountTransactionFile);
            this.ImportTransactionDataAction.TargetViewId = "AccountTransactionFile_ListView";
            this.ImportTransactionDataAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportTransactionDataAction.ToolTip = "Import Transaction Data";
            this.ImportTransactionDataAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportTransactionDataAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportTransactionDataAction_Execute);
                       // 
            // AccountTransactionFileViewController
            // 
            this.TargetObjectType = typeof(vidoSolution.Module.DomainObject.AccountTransactionFile);
            this.TargetViewId = "AccountTransactionFile_ListView";
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.AccountTransactionFileViewController_Activated);

        }




        private DevExpress.ExpressApp.Actions.SimpleAction ImportTransactionDataAction;
        #endregion
    }
}
