namespace vidoSolution.Module.Win
{
    partial class ExportViewController
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
            components = new System.ComponentModel.Container();
            this.ExportRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SelectRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportRegister
            // 
            this.ExportRegister.Caption = "Export Data";
            this.ExportRegister.Category = "View";
            this.ExportRegister.ConfirmationMessage = null;
            this.ExportRegister.Id = "ExportRegister";
            this.ExportRegister.ImageName = null;
            this.ExportRegister.Shortcut = null;
            this.ExportRegister.Tag = null;
            this.ExportRegister.TargetObjectsCriteria = null;
            this.ExportRegister.TargetViewId = null;
            this.ExportRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ExportRegister.ToolTip = null;
            this.ExportRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ExportRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportRegister_Execute);
            // 
            // SelectRegister
            // 
            this.SelectRegister.Caption = "Select Lesson";
            this.SelectRegister.Category = "View";
            this.SelectRegister.ConfirmationMessage = null;
            this.SelectRegister.Id = "SelectRegister";
            this.SelectRegister.ImageName = null;
            this.SelectRegister.Shortcut = null;
            this.SelectRegister.Tag = null;
            this.SelectRegister.TargetObjectsCriteria = null;
            this.SelectRegister.TargetViewId = null;
            this.SelectRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SelectRegister.ToolTip = null;
            this.SelectRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SelectRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SelectRegister_Execute);
            // 
            // ExportViewController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.ExportViewController_Activated);
        }
        
        private DevExpress.ExpressApp.Actions.SimpleAction ExportRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction SelectRegister;
        
        #endregion
    }
}
