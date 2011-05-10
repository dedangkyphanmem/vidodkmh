using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;

namespace vidoWinSolution.Module {
    public partial class WindowController1 : WindowController {
        public WindowController1() {
            InitializeComponent();
            RegisterActions(components);
            TargetWindowType = WindowType.Main;
        }
        ShowNavigationItemController showNavigationItemController;
        protected override void OnActivated() {
            base.OnActivated();
            showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += new EventHandler<CustomShowNavigationItemEventArgs>(showNavigationItemController_CustomShowNavigationItem);
        }
        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e) {
            showNavigationItemController.CustomShowNavigationItem -= new EventHandler<CustomShowNavigationItemEventArgs>(showNavigationItemController_CustomShowNavigationItem);
            ObjectSpace os = Application.CreateObjectSpace();
            DomainObject1 do1 = os.FindObject<DomainObject1>(new BinaryOperator("Property1", "StartupObject"));
            if (do1 == null) {
                do1 = os.CreateObject<DomainObject1>();
            }
            DetailView view = Application.CreateDetailView(os, do1);
            e.ActionArguments.ShowViewParameters.CreatedView = view;
            e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
            e.Handled = true;
        }
    }
}
