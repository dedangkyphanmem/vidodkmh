using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.SystemModule;
using vidoSolution.Module.DomainObject;
using DevExpress.ExpressApp.Reports;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using vidoSolution.Module.Utilities;
using System.Collections;
using DevExpress.Persistent.BaseImpl;

namespace vidoSolution.Module
{
    public partial class LessonViewController : ViewController
    {
        public LessonViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        void LessonViewController_Activated(object sender, System.EventArgs e)
        {
            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            XPCollection<Role> xpc2 = new XPCollection<Role>(u.Roles,
              new BinaryOperator("Name", "DataAdmins"));
            if (xpc.Count + xpc2.Count > 0)
            {
                SetDisableSelectAction.Active.SetItemValue(
                    "ObjectType", true);
                SetEnableSelectAction.Active.SetItemValue(
                    "ObjectType", true);
            }
            else
            {
                SetDisableSelectAction.Active.SetItemValue(
                    "ObjectType", false);
                SetEnableSelectAction.Active.SetItemValue(
                    "ObjectType", false);
            }
            
        }
        void SetDisableSelectAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            foreach (Lesson l in View.SelectedObjects)
            {
                View.ObjectSpace.SetModified(l);
                l.CanRegister = false;
            }
            View.ObjectSpace.CommitChanges();
        }
        void SetEnableSelectAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            foreach (Lesson l in View.SelectedObjects)
            {
                View.ObjectSpace.SetModified(l);
                l.CanRegister = true;
            }
            View.ObjectSpace.CommitChanges();
        }

      

      
       

    }
}
