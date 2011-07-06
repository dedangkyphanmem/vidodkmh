namespace vidoSolution.Module
{
    partial class LessonRegisterViewController
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
            this.CancelRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CheckRegister = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CalculateFeeForLesson = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.ViewDetailLessonTimetable = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.DefaultRegisterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CancelRegister
            // 
            this.CancelRegister.Caption = "Delete Lesson";
            this.CancelRegister.Category = "View";
            this.CancelRegister.ConfirmationMessage = "Agree and Delete?";
            this.CancelRegister.Id = "CancelRegister";
            this.CancelRegister.ImageName = null;
            this.CancelRegister.Shortcut = null;
            this.CancelRegister.Tag = null;
            this.CancelRegister.TargetObjectsCriteria = null;
            this.CancelRegister.TargetViewId = null;
            this.CancelRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CancelRegister.ToolTip = null;
            this.CancelRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CancelRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CancelRegister_Execute);
            // 
            // CheckRegister
            // 
            this.CheckRegister.Caption = "Check Lesson";
            this.CheckRegister.Category = "View";
            this.CheckRegister.ConfirmationMessage = "Agree and Set Checked status?";
            this.CheckRegister.Id = "CheckRegister";
            this.CheckRegister.ImageName = null;
            this.CheckRegister.Shortcut = null;
            this.CheckRegister.Tag = null;
            this.CheckRegister.TargetObjectsCriteria = null;
            this.CheckRegister.TargetViewId = null;
            this.CheckRegister.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CheckRegister.ToolTip = null;
            this.CheckRegister.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CheckRegister.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CheckRegister_Execute);
            // 
            // CalculateFeeForLesson
            // 
            this.CalculateFeeForLesson.Caption = "Calculate Fee For Lesson";
            this.CalculateFeeForLesson.Category = "View";
            this.CalculateFeeForLesson.ConfirmationMessage = "Agree and Calculate all selected lessons fee?";
            this.CalculateFeeForLesson.Id = "CalculateFeeForLesson";
            this.CalculateFeeForLesson.ImageName = null;
            this.CalculateFeeForLesson.Shortcut = null;
            this.CalculateFeeForLesson.Tag = null;
            this.CalculateFeeForLesson.TargetObjectsCriteria = null;
            this.CalculateFeeForLesson.TargetViewId = null;
            this.CalculateFeeForLesson.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CalculateFeeForLesson.ToolTip = null;
            this.CalculateFeeForLesson.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CalculateFeeForLesson.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CalculateFeeForLesson_Execute);
            // 
            // ViewDetailLessonTimetable
            // 
            //this.ViewDetailLessonTimetable.AcceptButtonCaption = null;
            //this.ViewDetailLessonTimetable.CancelButtonCaption = null;
            //this.ViewDetailLessonTimetable.Caption = "Xem lịch";
            //this.ViewDetailLessonTimetable.Category = "View";
            //this.ViewDetailLessonTimetable.ConfirmationMessage = null;
            //this.ViewDetailLessonTimetable.Id = "ViewDtailTimetable";
            //this.ViewDetailLessonTimetable.ImageName = null;
            //this.ViewDetailLessonTimetable.Shortcut = null;
            //this.ViewDetailLessonTimetable.Tag = null;
            //this.ViewDetailLessonTimetable.TargetObjectsCriteria = null;
            //this.ViewDetailLessonTimetable.TargetViewId = null;
            //this.ViewDetailLessonTimetable.ToolTip = null;
            //this.ViewDetailLessonTimetable.TypeOfView = null;
            //this.ViewDetailLessonTimetable.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewDetailLessonTimetable_CustomizePopupWindowParams);
                   
            // 
            // DefaultRegisterAction
            // 
            this.DefaultRegisterAction.Caption = "Set Detault Lesson";
            this.DefaultRegisterAction.Category = "View";
            this.DefaultRegisterAction.ConfirmationMessage = null;
            this.DefaultRegisterAction.Id = "DefaultRegisterAction";
            this.DefaultRegisterAction.ImageName = null;
            this.DefaultRegisterAction.Shortcut = null;
            this.DefaultRegisterAction.Tag = null;
            this.DefaultRegisterAction.TargetObjectsCriteria = null;
            this.DefaultRegisterAction.TargetViewId = null;
            this.DefaultRegisterAction.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.DefaultRegisterAction.ToolTip = null;
            this.DefaultRegisterAction.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.DefaultRegisterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DefaultRegister_Execute);
            // 
            // LessonRegisterViewController
            // 
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.Activated += new System.EventHandler(this.LessonRegisterViewController_Activated);

        }

        

        #endregion
        
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewDetailLessonTimetable;
        private DevExpress.ExpressApp.Actions.SimpleAction CheckRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction CancelRegister;
        private DevExpress.ExpressApp.Actions.SimpleAction CalculateFeeForLesson;
        private DevExpress.ExpressApp.Actions.SimpleAction DefaultRegisterAction;
        
        
        
    }
}
