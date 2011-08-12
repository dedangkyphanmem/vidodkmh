using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering;
using vidoSolution.Module.DomainObject;
using DevExpress.Persistent.BaseImpl;

namespace vidoSolution.Module
{
    public partial class FilterByUsersViewController : ViewController
    {
        public FilterByUsersViewController()
        {
            InitializeComponent();
            RegisterActions(components);
           
 
        }

        private void FilterByUsersViewController_Activated(object sender, EventArgs e)
        {
            User u = (User)SecuritySystem.CurrentUser;
            if (u is Student)
            {
                Student student = u as Student;
                bool studentlistview = (View.ObjectTypeInfo.Type == typeof(RegisterDetail)) ||
                    (View.ObjectTypeInfo.Type == typeof(StudentResult)) ||
                    (View.ObjectTypeInfo.Type == typeof(StudentAccumulation)) ||
                    (View.ObjectTypeInfo.Type == typeof(AccountTransaction));

                if ((View is ListView) && studentlistview)
                {
                    ((ListView)View).CollectionSource.Criteria["ByStudent"] = new BinaryOperator(
                       "Student.StudentCode", student.StudentCode, BinaryOperatorType.Equal);

                }
                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(RegisterDetail)))
                {
                    ConstrainstParameter cpNHHK = View.ObjectSpace.FindObject<ConstrainstParameter>(
                           new BinaryOperator("Code", "REGISTERSEMESTER"));

                    if (cpNHHK == null || cpNHHK.Value == 0)
                        throw new UserFriendlyException("Người Quản trị chưa thiết lập NHHK để ĐKMH, vui lòng liên hệ quản trị viên.");
                    else
                         ((ListView)View).CollectionSource.Criteria[cpNHHK.Value.ToString()] = new BinaryOperator(
                            "Lesson.Semester.SemesterName", cpNHHK.Value, BinaryOperatorType.Greater);
                }

                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(Student)))
                {
                    ((ListView)View).CollectionSource.Criteria["ByStudent"] = new BinaryOperator(
                       "StudentCode", student.StudentCode, BinaryOperatorType.Equal);
                }
                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(User)))
                {
                    ((ListView)View).CollectionSource.Criteria["ByStudent"] = new BinaryOperator(
                       "Oid", student.Oid, BinaryOperatorType.Equal);
                }
                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(WeekReportData)))
                {
                    ((ListView)View).CollectionSource.Criteria["ByStudent"] = new BinaryOperator(
                       "Name", student.StudentCode, BinaryOperatorType.Equal);
                }
            }
            else if (u.UserName =="guest")
            {

                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(User)))
                {
                    ((ListView)View).CollectionSource.Criteria["ByStudent"] = new BinaryOperator(
                       "Oid", u.Oid, BinaryOperatorType.Equal);
                }
            }
        }
    }
}
