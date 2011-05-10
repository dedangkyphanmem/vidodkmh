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

namespace vidoSolution.Module
{
    public partial class SemesterViewController : ViewController
    {
        public SemesterViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        void SemesterViewController_Activated(object sender, System.EventArgs e)
        {
            
        }
        void ViewClassTrackingAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
          
            ObjectSpace objectSpace = Application.CreateObjectSpace();
            Semester semester = View.SelectedObjects[0] as Semester;
            Data.CreateStudentClassTrackingData(objectSpace, semester.SemesterName);

            ReportData reportData = objectSpace.FindObject<ReportData>(
                   new BinaryOperator("Name", "Tỉ lệ nợ lớp biên chế NHHK"));
            string strParse = string.Format("Semester.SemesterName= '{0}'", semester.SemesterName);

            ReportServiceController rsc = Frame.GetController<ReportServiceController>();
            rsc.ShowPreview((IReportData)reportData, CriteriaOperator.Parse(strParse)); 
        }

      

      
       

    }
}
