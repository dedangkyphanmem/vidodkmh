namespace vidoSolution.Module.Web
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
            this.components = new System.ComponentModel.Container();
            
            this.ConfirmRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.BookRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SelectRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PrintRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PrintRegister
            // 
            this.PrintRegister.Caption = "Print Register";
            this.PrintRegister.Category = "View";
            this.PrintRegister.ConfirmationMessage = null;
            this.PrintRegister.Id = "PrintRegister";
            this.PrintRegister.ImageName = null;
            this.PrintRegister.Shortcut = null;
            this.PrintRegister.Tag = null;
            this.PrintRegister.TargetObjectsCriteria = null;
            this.PrintRegister.TargetViewId = null;
            this.PrintRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PrintRegister.ToolTip = null;
            this.PrintRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PrintRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintRegister_Execute);

            // 
            // ConfirmRegister
            // 
            this.ConfirmRegister.Caption = "Confirm Register";
            this.ConfirmRegister.Category = "View";
            this.ConfirmRegister.ConfirmationMessage = null;
            this.ConfirmRegister.Id = "ConfirmRegister";
            this.ConfirmRegister.ImageName = null;
            this.ConfirmRegister.Shortcut = null;
            this.ConfirmRegister.Tag = null;
            this.ConfirmRegister.TargetObjectsCriteria = null;
            this.ConfirmRegister.TargetViewId = null;
            this.ConfirmRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ConfirmRegister.ToolTip = null;
            this.ConfirmRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ConfirmRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConfirmRegister_Execute);
            // 
            // BookRegister
            // 
            this.BookRegister.Caption = "Book Register";
            this.BookRegister.Category = "View";
            this.BookRegister.ConfirmationMessage = null;
            this.BookRegister.Id = "BookRegister";
            this.BookRegister.ImageName = null;
            this.BookRegister.Shortcut = null;
            this.BookRegister.Tag = null;
            this.BookRegister.TargetObjectsCriteria = null;
            this.BookRegister.TargetViewId = null;
            this.BookRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.BookRegister.ToolTip = null;
            this.BookRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.BookRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BookRegister_Execute);
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
            //this.ViewControlsCreated += new System.EventHandler(this.ExportViewController_ViewControlsCreated);
           
            this.Activated += new System.EventHandler(this.ExportViewController_Activated);

        }
        private DevExpress.ExpressApp.Actions.SimpleAction BookRegister;       
        private DevExpress.ExpressApp.Actions.SimpleAction ExportRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction ConfirmRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction SelectRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintRegister;
        
        #endregion
    }
}
