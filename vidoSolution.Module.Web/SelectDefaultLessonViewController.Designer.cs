namespace vidoSolution.Module.Web
{
    partial class SelectDefaultLessonViewController
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
            this.SelectDefaultLesson = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SelectDefaultLesson
            // 
            this.SelectDefaultLesson.Caption = "Select Default Lesson";
            this.SelectDefaultLesson.Category = "View";
            this.SelectDefaultLesson.ConfirmationMessage = null;
            this.SelectDefaultLesson.Id = "SelectDefaultLesson";
            this.SelectDefaultLesson.ImageName = null;
            this.SelectDefaultLesson.Shortcut = null;
            this.SelectDefaultLesson.Tag = null;
            this.SelectDefaultLesson.TargetObjectsCriteria = null;
            this.SelectDefaultLesson.TargetViewId = null;
            this.SelectDefaultLesson.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SelectDefaultLesson.ToolTip = null;
            this.SelectDefaultLesson.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SelectDefaultLesson.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SelectRegister_Execute);
            // 
            // SelectDefaultLessonViewController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.ExportViewController_Activated);

        }

        private DevExpress.ExpressApp.Actions.SimpleAction SelectDefaultLesson;
        #endregion
    }
}
