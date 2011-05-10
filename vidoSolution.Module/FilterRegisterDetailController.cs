using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using vidoSolution.Module.DomainObject;

namespace vidoSolution.Module
{
    public partial class FilterRegisterDetailController : WindowController
    {
        public FilterRegisterDetailController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        

        private void FilterStudentController_Activated(object sender, EventArgs e)
        {
            Window.GetController<NewObjectViewController>().CollectDescendantTypes +=
                new EventHandler<CollectTypesEventArgs>(NewObjectViewController_CollectDescendantTypes);

        }
        private void NewObjectViewController_CollectDescendantTypes(object sender, CollectTypesEventArgs e)
        {
            User u = (User)SecuritySystem.CurrentUser;
            XPCollection<Role> xpc = new XPCollection<Role>(u.Roles,
                new BinaryOperator("Name", "Administrators"));
            if (xpc.Count == 0)
            {
                foreach (Type type in e.Types)
                {
                    if (type.Name == "RegisterDetail") { e.Types.Remove(type); break; }
                }
            }

           

        }
    }
}
